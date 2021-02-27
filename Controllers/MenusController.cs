using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResAktWebb.Data;
using ResAktWebb.Models;
using Microsoft.AspNetCore.Authorization;

namespace ResAktWebb.Controllers
{
    [Authorize]
    public class MenusController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();
        string menuApi = "menus/";
        string api = "http://informatik12.ei.hv.se/grupp5/api/Menus/";

        public MenusController(ResAktWebbContext context)
        {
            _context = context;
        }
        
        // GET: Menus
        public async Task<IActionResult> Index()
        {
            return View(await RestHelper.ApiGet<Menu>(menuApi));
        }

        // GET: Menus/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            return View (await RestHelper.ApiGet<Menu>(menuApi, id));
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Menu menu)
        {
            await RestHelper.ApiCreate<Menu>(menuApi, menu);
            return RedirectToAction("Index");

        }
        // GET: Menus/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await RestHelper.ApiGet<Menu>(menuApi, id));
        }

        // POST: Menus/Edit/id

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Menu menu)
        {
            await RestHelper.ApiEdit<Menu>(menuApi + id, menu);
            return RedirectToAction("Index", "Menus");
        }

        // GET: Menus/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {

            return View(await RestHelper.ApiGet<Menu>(menuApi, id));
        }

        // POST: Menus/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<Menu>(menuApi, id);
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
