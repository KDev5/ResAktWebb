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
        string api = "Activities/";
        public async Task<IActionResult> Index()
        {
            var a = await RestHelper.ApiGet<Activity>(api);
            return View(a);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(api, id));
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
            await RestHelper.ApiCreate<Activity>(api, activity);
            return RedirectToAction("Index");
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(api, id));
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Location,Price,StartTime,EndTime")] Activity activity)
        {
            await RestHelper.ApiEdit<Activity>(api + id, activity);
        
            return RedirectToAction("Index","Activities");
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(api, id));
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<Activity>(api, id);
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}
