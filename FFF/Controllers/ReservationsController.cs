using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FFF.Data;
using FFF.Models;

namespace FFF.Controllers
{
	public class ReservationsController : Controller
	{
		private readonly FFFContext _context;

		public ReservationsController(FFFContext context)
		{
			this._context = context;
		}

		// GET: Reservations
		public async Task<IActionResult> Index()
		{
			return View(await _context.Reservations.ToListAsync());
		}

		// GET: Reservations/Details/5
		public async Task<IActionResult> Details(long? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reservation = await _context.Reservations
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
			var events = _context.Events.ToList();
			ViewBag.Events = events;
			return View();
		}

		// POST: Reservations/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Note,EventId")] Reservation reservation)
		{
			if (ModelState.IsValid)
			{
				_context.Add(reservation);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
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
			return View(reservation);
		}

		// POST: Reservations/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(long id, [Bind("Id,Name,PhoneNumber,Note")] Reservation reservation)
		{
			if (id != reservation.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(reservation);
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
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ReservationExists(long id)
		{
			return _context.Reservations.Any(e => e.Id == id);
		}
	}
}
