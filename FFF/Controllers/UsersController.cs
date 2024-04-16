using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FFF.Data;
using FFF.Models;
using Ninject;
using FFF.Configurations;
using FFF.Services;
using Microsoft.AspNet.Identity;

namespace FFF.Controllers
{
	public class UsersController : Controller
	{
		private readonly FFFContext _context;
		private readonly EmailService _emailService;
		private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
		private readonly Microsoft.AspNetCore.Identity.SignInManager<User> _signInManager;

		public UsersController(FFFContext context, EmailService emailService, Microsoft.AspNetCore.Identity.UserManager<User> userManager, Microsoft.AspNetCore.Identity.SignInManager<User> signInManager)
		{
			_context = context;
			_emailService = emailService;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		// GET: Users
		public async Task<IActionResult> Index()
		{
			return View(await _context.Users.ToListAsync());
		}

		// GET: Users/Details/5
		public async Task<IActionResult> Details(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		public IActionResult Register()
		{
			return View();
		}

		// POST: Users/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register([Bind("Username,Password,ConfirmPassword,Email")] User user)
		{
			if (ModelState.IsValid)
			{
				Role userRole = await _context.Roles.FirstAsync(r => r.Authority.Equals("User"));
				user.Roles.Add(userRole);
				if (!_context.Users.Any())
				{
					Role rootRole = await _context.Roles.FirstAsync(r => r.Authority.Equals("Root"));
					Role adminRole = await _context.Roles.FirstAsync(r => r.Authority.Equals("Admin"));
					user.Roles.Add(rootRole);
					user.Roles.Add(adminRole);
				}
				user.Password = StringEncryptor.EncryptString(user.Password);

				_context.Add(user);
				await _context.SaveChangesAsync();
				//return RedirectToAction(nameof(Index));
				// Generate email confirmation token

				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

				// Build callback URL for email confirmation
				var callbackUrl = Url.Action("ConfirmEmail", "Users", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

				// Send confirmation email
				await _emailService.SendAsync(new IdentityMessage
				{
					Destination = user.Email,
					Subject = "Confirm your email",
					Body = $"Please confirm your email by clicking <a href='{callbackUrl}'>here</a>"
				});

				ViewBag.Message = "Check your email and confirm your account, you must be confirmed before you can log in.";

				return View("Info");
			}
			return View(user);
		}

		public IActionResult LogIn()
		{
			return View();
		}

		public async Task<IActionResult> LogIn([Bind("Username,Password")] User user)
		{
			if (ModelState.IsValid)
			{
				User loggedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(user.Username) &&
				StringEncryptor.DecryptString(u.Password).Equals(user.Password));
				if (loggedUser != null)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(user);
		}

		// GET: Users/Edit/5
		public async Task<IActionResult> Edit(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(long id, [Bind("Id,Username,Password,ConfirmPassword,Email")] User user)
		{
			if (id != user.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(user);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}

		// GET: Users/Delete/5
		public async Task<IActionResult> Delete(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
			var user = await _context.Users.FindAsync(id);
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(long id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
