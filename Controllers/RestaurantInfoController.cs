﻿using System;
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
    public class RestaurantInfoController : Controller
    {
        private readonly ResAktWebbContext _context;

        public RestaurantInfoController(ResAktWebbContext context)
        {
            _context = context;
        }

        string api = "RestaurantInfoes/";
        // GET: RestaurantInfo
        public async Task<IActionResult> Index()
        {
            var a = await RestHelper.ApiGet<RestaurantInfo>(api);
            return View(a);
        }

        // GET: RestaurantInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<RestaurantInfo>(api, id));
        }

        // GET: RestaurantInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Open,Closed")] RestaurantInfo restaurantInfo)
        {
            await RestHelper.ApiCreate<RestaurantInfo>(api, restaurantInfo);
            return RedirectToAction("Index");
        }

        // GET: RestaurantInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await RestHelper.ApiGet<RestaurantInfo>(api, id));
        }

        // POST: RestaurantInfo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Open,Closed")] RestaurantInfo restaurantInfo)
        {
            await RestHelper.ApiEdit<RestaurantInfo>(api + id, restaurantInfo);

            return RedirectToAction("Index", "Activities");
        }


        // GET: RestaurantInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<RestaurantInfo>(api, id));
        }

        // POST: RestaurantInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<RestaurantInfo>(api, id);
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantInfoExists(int id)
        {
            return _context.RestaurantInfo.Any(e => e.Id == id);
        }
    }
}