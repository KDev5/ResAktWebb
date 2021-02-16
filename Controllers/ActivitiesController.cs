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
    public class ActivitiesController : Controller
    {
        private readonly ResAktWebbContext _context;

        public ActivitiesController(ResAktWebbContext context)
        {
            _context = context;
        }

        // Connectionstring till api
        string api = "http://informatik12.ei.hv.se/grupp5/api/";


        //
        public async Task<IActionResult> Index()
        {
            List<Activity> a = new List<Activity>();
            HttpClient client = new HttpClient();

            var r = await client.GetAsync(api + "Activities");
            string jsonR = await r.Content.ReadAsStringAsync();
			try
			{
                a = JsonConvert.DeserializeObject < List<Activity> >(jsonR) ;
			}
			catch (Exception)
			{

				throw;
			}

            return View(a);
          //  return View(await _context.Activity.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var a = new Activity();
			using (HttpClient c = new HttpClient ())
			{
                var r = await c.GetAsync(api + "Activities/" + id);
                string jR = await r.Content.ReadAsStringAsync();
				try
				{
                    a = JsonConvert.DeserializeObject<Activity>(jR);
				}
				catch (Exception)
				{

					throw;
				}
			}

            return View(a);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Location,Price,StartTime,EndTime")] Activity activity)
        {

			using (HttpClient c = new HttpClient())
			{
                var r = await c.PostAsJsonAsync(api + "Activities", activity);
			}


            return RedirectToAction("Index");
           /* if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);*/
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var a = new Activity();
            // Hämta activity
			using (HttpClient c = new HttpClient())
			{
                var r = await c.GetAsync(api + "Activities/" + id);
                var jR = await r.Content.ReadAsStringAsync();

                a = JsonConvert.DeserializeObject<Activity>(jR);
			}
            return View(a);
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Location,Price,StartTime,EndTime")] Activity activity)
        {
				using (HttpClient c = new HttpClient())
				{
                    var response = await c.PutAsJsonAsync(api + "Activities/" + id, activity);
				}
                
                return RedirectToAction("Details","Activities");
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var a = new Activity();
            using (HttpClient c = new HttpClient())
            {
                var r = await c.GetAsync(api + "Activities/" + id);
                var jR = await r.Content.ReadAsStringAsync();

                a = JsonConvert.DeserializeObject<Activity>(jR);
            }

            return View(a);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			using (HttpClient c = new HttpClient())
			{
                var r = await c.DeleteAsync(api + "Activities/" + id);
			}
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}
