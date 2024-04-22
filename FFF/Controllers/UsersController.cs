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
using System.Net.Mail;

namespace FFF.Controllers
{
	public class UsersController : Controller
	{
		private readonly FFFContext _context;
		private readonly EmailSender _emailSender;

		public UsersController(FFFContext context, EmailSender emailSender, Microsoft.AspNetCore.Identity.UserManager<User> userManager, Microsoft.AspNetCore.Identity.SignInManager<User> signInManager)
		{
			_context = context;
			_emailSender = emailSender;
		}

		// GET: Users
		public async Task<IActionResult> Index()
		{
			return View(await _context.Users.ToListAsync());
		}

		// GET: Users/Details/5
		public async Task<IActionResult> Details(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Id.Equals(id));
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
				Role userRole = await _context.Roles.FirstAsync(r => r.Name.Equals(Authorities.User.ToString()));
				user.Roles.Add(userRole);
				if (!_context.Users.Any())
				{
					Role rootRole = await _context.Roles.FirstAsync(r => r.Name.Equals(Authorities.Root.ToString()));
					Role adminRole = await _context.Roles.FirstAsync(r => r.Name.Equals(Authorities.Root.ToString()));
					user.Roles.Add(rootRole);
					user.Roles.Add(adminRole);
				}
				user.Password = StringEncryptor.EncryptString(user.Password);

				_context.Add(user);
				await _context.SaveChangesAsync();
				//return RedirectToAction(nameof(Index));
				// Generate email confirmation token
				var token = Guid.NewGuid().ToString();

				UserToken userToken = new()
				{
					Token = token,
					TokenType = "EmailConfirmation",
					CreatedAt = DateTime.Now,
					User = user
				};
				_context.UserTokens.Add(userToken);
				_context.SaveChanges();

				// Send confirmation email
				SendConfirmationEmail(user.Email, token);

				return RedirectToAction("ConfirmationSent");
			}
			return View(user);
		}

		private void SendConfirmationEmail(string email, string token)
		{
			var callbackUrl = Url.Action("ConfirmEmail", "Account", new { email = email, token = token }, protocol: Request.Scheme);
			var message = new MailMessage("your_email@gmail.com", email, "Confirm your email", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
			message.IsBodyHtml = true;

			var smtpClient = new SmtpClient();
			smtpClient.Send(message);
		}

		public ActionResult ConfirmationSent()
		{
			return View();
		}

		public ActionResult ConfirmEmail(string email, string token)
		{
			// Find the user by email
			var user = _context.Users.FirstOrDefault(u => u.Email == email);

			if (user != null)
			{
				// Find the token in the database
				var userToken = _context.UserTokens.FirstOrDefault(t => t.User.Id == user.Id && t.Token == token && t.TokenType == "EmailConfirmation");

				if (userToken != null)
				{
					// Confirm the email by setting IsEmailConfirmed to true
					user.EmailConfirmed = true;
					_context.SaveChanges();

					// Optionally, sign in the user
					// SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

					return RedirectToAction("EmailConfirmed", "Account");
				}
			}

			// If confirmation fails, show an error message
			return RedirectToAction("ConfirmationFailed", "Account");
		}

		public ActionResult EmailConfirmed()
		{
			return View();
		}

		public ActionResult ConfirmationFailed()
		{
			return View();
		}

		public IActionResult LogIn()
		{
			return View();
		}

		public async Task<IActionResult> LogIn([Bind("Username,Password")] User user)
		{
			if (ModelState.IsValid)
			{
				User loggedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(user.UserName) &&
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
		public async Task<IActionResult> Edit(string id, [Bind("Id,Username,Password,ConfirmPassword,Email")] User user)
		{
			if (!id.Equals(user.Id))
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
		public async Task<IActionResult> Delete(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id.Equals(id));
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

		private bool UserExists(string id)
		{
			return _context.Users.Any(e => e.Id.Equals(id));
		}
	}
}
