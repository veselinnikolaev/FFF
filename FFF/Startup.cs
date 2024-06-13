using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FFF.Models;
using Microsoft.AspNetCore.Identity;
using FFF.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace FFF
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddControllersWithViews();

            services.AddDbContext<FFFContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HomeDb")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<FFFContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddAuthorization(config =>
            {
                config.AddPolicy("RequireAdminRole",
                    policy => policy.RequireRole("Admin"));
                config.AddPolicy("RequireUserRole",
                    policy => policy.RequireRole("User"));
            });

            services.AddMvc();
            services.AddRazorPages(options =>
            {
                options.Conventions.AllowAnonymousToFolder("/Home");
                options.Conventions.AllowAnonymousToFolder("/Shared");
                options.Conventions.AllowAnonymousToAreaFolder("Identity", "/Pages");
                options.Conventions.AuthorizeFolder("/Reservations");
                options.Conventions.AuthorizeFolder("/Employees", "RequireAdminRole");
                options.Conventions.AuthorizeFolder("/Events", "RequireAdminRole");
                options.Conventions.AuthorizeFolder("/Users", "RequireAdminRole");
            });
            services.AddServerSideBlazor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication(); // Ensure this comes before UseAuthorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            SeedRoles(app).Wait(); // Call the seeding method to create roles
        }

        private async Task SeedRoles(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "User", "Admin" };

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