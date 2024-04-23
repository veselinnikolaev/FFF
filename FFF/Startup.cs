using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FFF.Data;
using FFF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FFF
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });

            services.AddControllersWithViews();

			services.AddDbContext<FFFContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("FFFContext")));

            services.AddAuthorization(config =>
            {
				config.AddPolicy("RequireRootRole",
					policy => policy.RequireRole("Root"));
                config.AddPolicy("RequireAdminRole",
                    policy => policy.RequireRole("Admin"));
                config.AddPolicy("RequireRootOrAdminRole",
                    policy => policy.RequireRole("Root").RequireRole("Admin"));
                config.AddPolicy("RequireUserRole",
                    policy => policy.RequireRole("User"));
            });

            services.AddRazorPages(options =>
			{
				options.Conventions.AuthorizePage("/Privacy", "RequireRootOrAdminRole");
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetService<FFFContext>();
                    await dbContext.Database.EnsureCreatedAsync();
					await dbContext.Database.MigrateAsync();
					// Add data seeding code here if needed
				}
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapRazorPages();
			});

			using (var scope = app.ApplicationServices.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				var roles = new[] { "User", "Admin", "Root" };

				foreach (var role in roles)
				{
					if (!await roleManager.RoleExistsAsync(role))
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}
			}
		}
	}
}
