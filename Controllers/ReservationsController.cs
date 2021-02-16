using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Data;
using ResAktWebb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace ResAktWebb.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ResAktWebbContext _context;

        public ReservationsController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var a = new List<Reservation>();
            using (HttpClient c = new HttpClient())
            {
                var r = await c.GetAsync("http://193.10.202.82/grupp5/api/reservations");
                string jsonResponse = await r.Content.ReadAsStringAsync();
                try
                {
                    a = JsonConvert.DeserializeObject<List<Reservation>>(jsonResponse);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(a);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var a = new Reservation();
            using (HttpClient c = new HttpClient())
            {
                var r = await c.GetAsync("http://193.10.202.82/grupp5/api/reservations/" + id);
                string jsonResponse = await r.Content.ReadAsStringAsync();
                try
                {
                    a = JsonConvert.DeserializeObject<Reservation>(jsonResponse);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(a);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,CustomerName,NumParticipants,TableNum")] Reservation reservation)
        {
            var a = new Reservation();
            using (HttpClient c = new HttpClient())
            {
                string dataAsJson = JsonConvert.SerializeObject(reservation);
                var r = await c.PostAsync("http://193.10.202.82/grupp5/api/reservations/", new StringContent(dataAsJson, Encoding.UTF8, "application/json"));
            }
            return View(a);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var a = new Reservation();
            using (HttpClient c = new HttpClient())
            {
                var r = await c.GetAsync("http://193.10.202.82/grupp5/api/reservations/" + id);
                string jsonResponse = await r.Content.ReadAsStringAsync();
                try
                {
                    a = JsonConvert.DeserializeObject<Reservation>(jsonResponse);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(a);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,CustomerName,NumParticipants,TableNum")] Reservation reservation)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        //D
        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
