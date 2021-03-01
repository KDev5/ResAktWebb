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
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace ResAktWebb.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();
        string api = "http://informatik12.ei.hv.se/grupp5/api/";
        string menuItemApi = "menuItems/";
        string menuCatApi = "menuCategories/";
        string menuApi = "menus/";

        public MenuItemsController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(int? id)
        {
            var menuItems = await RestHelper.ApiGet<MenuItems>(menuItemApi);
            var menuCategory = await RestHelper.ApiGet<MenuCategory>(menuCatApi, id);
            ViewData["route"] = id;
            List<MenuItems> itemsForCategoryId = new List<MenuItems>();
            ViewData["MenuCategory"] = menuCategory.Name;
                foreach (var item in menuItems)
                {
                    if (item.MenuCategoryId == id)
                    {
                        itemsForCategoryId.Add(item);
                    }
                    System.Diagnostics.Debug.WriteLine(item.Name + ", " + item.MenuCategoryId);
                }
            return View(itemsForCategoryId);
        }

        // GET: MenuItems/Details/Id
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<MenuItems>(menuItemApi, id));
        }

        // GET: MenuItems/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewData["route"]= id;

            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            ViewData["MenuCatId"] = new SelectList(menuCategories, "Id", "Name");
            return View();
        }

        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Allergies,Price,MenuCategoryId")] MenuItems menuItems)
        {
            using (HttpClient c = new HttpClient())
            {
                string dataAsJson = JsonConvert.SerializeObject(menuItems);
                var r = await c.PostAsync("http://localhost:64014/api/MenuItems/", new StringContent(dataAsJson, Encoding.UTF8, "application/json"));
            }
            //await RestHelper.ApiCreate<MenuItems>(menuItemApi, menuItems);
            return RedirectToAction("Index", new { id = menuItems.MenuCategoryId });
        }

        // GET: MenuItems/Edit/Id
        public async Task<IActionResult> Edit(int? id)
        {
            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuApi);
            ViewData["MenuCatId"] = new SelectList(menuCategories, "Id", "Name");

            return View(await RestHelper.ApiGet<MenuItems>(menuItemApi, id));
        }

        // POST: MenuItems/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Allergies,Price,MenuCategoryId")] MenuItems menuItems)
        {
            await RestHelper.ApiEdit<MenuItems>(menuItemApi + id, menuItems);
            return RedirectToAction("Index", "MenuItems", new { id = menuItems.MenuCategoryId });
        }

        // GET: MenuItems/Delete/Id
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<MenuItems>(menuItemApi, id));
        }

        // POST: MenuItems/Delete/Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<MenuItems>(menuItemApi, id);
            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemsExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
