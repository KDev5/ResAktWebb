using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResAktWebb.Data;
using ResAktWebb.Models;


namespace ResAktWebb.Controllers
{
    public class ActivityBookingsController : Controller
    {
        private readonly ResAktWebbContext _context; // Dbcontext. Används inte för att hämta data.
        private readonly string api = "ActivityBookings/"; // Connectionstring till api


        public ActivityBookingsController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: ActivityBookings
        public async Task<IActionResult> Index()
        {
            return View(await RestHelper.ApiGet<ActivityBooking>(api));
        }

        // GET: ActivityBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<ActivityBooking>(api, id));
        }

        // GET: ActivityBookings/Create
        public async Task<IActionResult> Create()
        {
            // Får det inte att fungera. errormsg: Cant convert from system.collections.generic.list to IEnumerable
            /*
              
             // ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id");
              var list =  (System.Collections.IEnumerable)RestHelper.ApiGet<Activity>("Activities/");
              ViewData["ActivityId"] = new SelectList(list, "Id", "Id");
             */
            /* var list = help_plsAsync();
             ViewData["ActivityId"] = new SelectList(list, "Id", "Id");*/
            var client = new HttpClient();

            List<Menu> menus = new List<Menu>();
            var menuResponse = await client.GetAsync(api + "menus/");
            string menuJsonResponse = await menuResponse.Content.ReadAsStringAsync();
            menus = JsonConvert.DeserializeObject<List<Menu>>(menuJsonResponse);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");
            return View();
        }
        public async Task<IEnumerable<Activity>> help_plsAsync()
		{
            var foo = await RestHelper.ApiGet<Activity>("Activities/");
            var bar = new List<Activity>();
            foo.ForEach(x => {
                var temp = new Activity();
                temp.Id = x.Id;
                temp.Description = x.Description;
                temp.Location = x.Location;
                temp.Price = x.Price;
                temp.StartTime = x.StartTime;
                temp.EndTime = x.EndTime;
                bar.Add(temp);
            });
            return bar.ToArray();
        }

        // POST: ActivityBookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerName,NumParticipants,ActivityId")] ActivityBooking activityBooking)
        {
            await RestHelper.ApiCreate<ActivityBooking>(api, activityBooking);
            return RedirectToAction("Index");
        }

        // GET: ActivityBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var activityBooking = await RestHelper.ApiGet<ActivityBooking>(api, id);
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
            return View(activityBooking);
        }

        // POST: ActivityBookings/Edit/5
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
                await RestHelper.ApiEdit(api, id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
            return View(activityBooking);
        }

        // GET: ActivityBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<ActivityBooking>(api, id));
        }

        // POST: ActivityBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<ActivityBooking>(api, id);
            return RedirectToAction(nameof(Index));
        }

        /*private bool ActivityBookingExists(int id)
        {
            return _context.ActivityBooking.Any(e => e.Id == id);
        }*/
    }
}
