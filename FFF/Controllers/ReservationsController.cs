using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FFF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FFF.Areas.Identity.Data;

namespace FFF.Controllers
{
    [Authorize]
	public class ReservationsController : Controller
	{
		private readonly FFFContext _context;
		private readonly UserManager<User> _userManager;

		public ReservationsController(FFFContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Reservations
		public async Task<IActionResult> Index()
		{
			var reservations = _context.Reservations;
			if (!User.IsInRole("Admin"))
			{
				User user = await _userManager.FindByNameAsync(_userManager.GetUserName(User));
				return View(await reservations.Where(r => r.Users.Contains(user)).Include(r => r.Event).ToListAsync());
			}
			return View(await reservations.Include(r => r.Event).ToListAsync());
		}

		// GET: Reservations/Details/5
		public async Task<IActionResult> Details(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reservation = await _context.Reservations
				.Include(r => r.Event)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (reservation == null)
			{
				return NotFound();
			}

			return View(reservation);
		}

		// GET: Reservations/Create
		public IActionResult Create()
		{
			ViewData["EventId"] = new SelectList(_context.Events, "Id", "ViewData");
			return View();
		}

		// POST: Reservations/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,PhoneNumber,Note,EventId")] Reservation reservation)
		{
			if (ModelState.IsValid)
			{
				_context.Add(reservation);

				var @event = await _context.Events.FindAsync(reservation.EventId);
				@event.Reservations.Add(reservation);

				User user = await _userManager.FindByNameAsync(_userManager.GetUserName(User));
				user.Reservations.Add(reservation);
				await _userManager.UpdateAsync(user);

				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["EventId"] = new SelectList(_context.Events, "Id", "ViewData", reservation.EventId);
			return View(reservation);
		}

		// GET: Reservations/Edit/5
		public async Task<IActionResult> Edit(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reservation = await _context.Reservations.FindAsync(id);
			if (reservation == null)
			{
				return NotFound();
			}
			ViewData["EventId"] = new SelectList(_context.Events, "Id", "ViewData", reservation.EventId);
			return View(reservation);
		}

		// POST: Reservations/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(long id, [Bind("Id,Name,PhoneNumber,Note,EventId")] Reservation reservation)
		{
			if (id != reservation.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Retrieve the original reservation including its associated event
					var originalReservation = await _context.Reservations
						.Include(r => r.Event)
						.FirstOrDefaultAsync(r => r.Id == reservation.Id);

					if (originalReservation == null)
					{
						return NotFound();
					}

					User user = await _userManager.FindByNameAsync(_userManager.GetUserName(User));
					user.Reservations.Remove(originalReservation);
					// Update the reservation properties except EventId
					originalReservation.Name = reservation.Name;
					originalReservation.PhoneNumber = reservation.PhoneNumber;
					originalReservation.Note = reservation.Note;

					// If the eventId has changed
					if (originalReservation.EventId != reservation.EventId)
					{
						// Remove the reservation from the previous event's collection
						if (originalReservation.Event != null)
						{
							originalReservation.Event.Reservations.Remove(originalReservation);
						}

						// Retrieve the new event
						var newEvent = await _context.Events.FindAsync(reservation.EventId);

						if (newEvent != null)
						{
							// Add the reservation to the new event's collection
							newEvent.Reservations.Add(originalReservation);
						}

						// Update the EventId of the reservation
						originalReservation.EventId = reservation.EventId;
					}

					// Update the reservation in the Reservations table
					_context.Update(originalReservation);

					user.Reservations.Add(originalReservation);
					await _userManager.UpdateAsync(user);

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReservationExists(reservation.Id))
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
			ViewData["EventId"] = new SelectList(_context.Events, "Id", "ViewData", reservation.EventId);
			return View(reservation);
		}

		// GET: Reservations/Delete/5
		public async Task<IActionResult> Delete(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reservation = await _context.Reservations
				.Include(r => r.Event)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (reservation == null)
			{
				return NotFound();
			}

			return View(reservation);
		}

		// POST: Reservations/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
			var reservation = await _context.Reservations.FindAsync(id);
			_context.Reservations.Remove(reservation);
			var @event = await _context.Events.FindAsync(reservation.EventId);
			@event.Reservations.Remove(reservation);

			User user = await _userManager.FindByNameAsync(_userManager.GetUserName(User));
			user.Reservations.Remove(reservation);
			await _userManager.UpdateAsync(user);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ReservationExists(long id)
		{
			return _context.Reservations.Any(e => e.Id == id);
		}
	}
}
