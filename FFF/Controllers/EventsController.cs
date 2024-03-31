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
    public class EventsController : Controller
    {
        private readonly FFFContext _context;

        public EventsController(FFFContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["Employees"] = new MultiSelectList(_context.Employees, "Id", "ViewData");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,SingerName,TicketPrice,Description")] Event @event, List<long> employeeIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);

                if (employeeIds != null)
                {
                    // Retrieve the corresponding employees and add them to the event
                    foreach (var employeeId in employeeIds)
                    {
                        var employee = await _context.Employees.FindAsync(employeeId);
                        if (employee != null)
                        {
                            employee.Events.Add(@event);
                            @event.Employees.Add(employee);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employees"] = new MultiSelectList(_context.Employees, "Id", "ViewData");
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            var selectedEmployeeIds = @event.Employees.Select(e => e.Id).ToList();
            ViewData["Employees"] = new MultiSelectList(_context.Employees, "Id", "ViewData", selectedEmployeeIds);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Date,SingerName,TicketPrice,Description")] Event @event, List<long> employeeIds)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalEvent = await _context.Events.Include(e => e.Employees).FirstOrDefaultAsync(e => e.Id == @event.Id);

                    if (originalEvent == null)
                    {
                        return NotFound();
                    }

                    // Update the event properties in the context
                    _context.Update(@event);

                    // Remove employees that are no longer selected
                    foreach (var employee in originalEvent.Employees.ToList())
                    {
                        if (employeeIds == null || !employeeIds.Contains(employee.Id))
                        {
                            originalEvent.Employees.Remove(employee);
                        }
                    }

                    // Add new employees selected
                    if (employeeIds != null)
                    {
                        foreach (var employeeId in employeeIds)
                        {
                            if (!originalEvent.Employees.Any(e => e.Id == employeeId))
                            {
                                var employee = await _context.Employees.FindAsync(employeeId);
                                if (employee != null)
                                {
                                    originalEvent.Employees.Add(employee);
                                }
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            ViewData["Employees"] = new MultiSelectList(_context.Employees, "Id", "ViewData", employeeIds);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var @event = await _context.Events
        .Include(e => e.Employees)  // Include related employees
        .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            // Remove associated relationships with employees
            foreach (var employee in @event.Employees.ToList())
            {
                @event.Employees.Remove(employee);
            }

            // Now remove the event itself
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(long id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
