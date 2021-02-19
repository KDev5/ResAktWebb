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
        string api = "http://informatik12.ei.hv.se/grupp5/api/Activities/"; // REDUNDANT MED RESTHELPER
        string apiNew = "Activities/";
        public async Task<IActionResult> Index()
        {
            var a = await RestHelper.ApiGet<Activity>(apiNew);
            return View(a);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(apiNew, id));
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
            await RestHelper.ApiCreate<Activity>(apiNew, activity);
            return RedirectToAction("Index");
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(apiNew, id));
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Location,Price,StartTime,EndTime")] Activity activity)
        {
            /*using (HttpClient c = new HttpClient())
            {
                var response = await c.PutAsJsonAsync(api + id, activity);
            }*/
            await RestHelper.ApiEdit<Activity>(apiNew + id, activity);

                
            return RedirectToAction("Index","Activities");
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var a = new Activity();
            using (HttpClient c = new HttpClient())
            {
                var r = await c.GetAsync(api + id);
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
                var r = await c.DeleteAsync(api + id);
			}
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}
