using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Data;
using ResAktWebb.Models;

namespace ResAktWebb.Controllers
{
    public class ActivityBookingsController : Controller
    {
        private readonly ResAktWebbContext _context; // Dbcontext. Används inte för att hämta data.
        private readonly string api = "http://informatik12.ei.hv.se/grupp5/api/Activities/"; // Connectionstring till api


        public ActivityBookingsController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: ActivityBookings
        public async Task<IActionResult> Index()
        {
            var resAktWebbContext = _context.ActivityBooking.Include(a => a.Activity);
            return View(await resAktWebbContext.ToListAsync());
        }

        // GET: ActivityBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityBooking = await _context.ActivityBooking
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityBooking == null)
            {
                return NotFound();
            }

            return View(activityBooking);
        }

        // GET: ActivityBookings/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id");
            return View();
        }

        // POST: ActivityBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerName,NumParticipants,ActivityId")] ActivityBooking activityBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
            return View(activityBooking);
        }

        // GET: ActivityBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityBooking = await _context.ActivityBooking.FindAsync(id);
            if (activityBooking == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
            return View(activityBooking);
        }

        // POST: ActivityBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,NumParticipants,ActivityId")] ActivityBooking activityBooking)
        {
            if (id != activityBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityBookingExists(activityBooking.Id))
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
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
            return View(activityBooking);
        }

        // GET: ActivityBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityBooking = await _context.ActivityBooking
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityBooking == null)
            {
                return NotFound();
            }

            return View(activityBooking);
        }

        // POST: ActivityBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityBooking = await _context.ActivityBooking.FindAsync(id);
            _context.ActivityBooking.Remove(activityBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityBookingExists(int id)
        {
            return _context.ActivityBooking.Any(e => e.Id == id);
        }
    }
}
