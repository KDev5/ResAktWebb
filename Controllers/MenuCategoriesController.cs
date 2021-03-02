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
    public class MenuCategoriesController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();
        string menuCatApi = "menuCategories/";
        string menuApi = "menus/";
        public MenuCategoriesController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: MenuCategories
        public async Task<IActionResult> Index(int? id)
        {
            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            var menu = await RestHelper.ApiGet<Menu>(menuApi, id);
            List<MenuCategory> categoriesForMenuId = new List<MenuCategory>();
            ViewData["Menu"] = menu.Name;
            ViewData["route"] = id;
            ViewData["MenuId"] = menu.Id;
            foreach (var item in menuCategories)
            {
                if (item.MenuId == id)
                {
                    categoriesForMenuId.Add(item);
                }
            }
            foreach (var item in categoriesForMenuId)
            {
                System.Diagnostics.Debug.WriteLine(item.MenuId);
                        }

            return View(categoriesForMenuId);
        }

        // GET: MenuCategories/Details/Id
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<MenuCategory>(menuCatApi, id));
        }

        // GET: MenuCategories/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewData["route"] = id;
            var menus = await RestHelper.ApiGet<Menu>(menuApi);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");

            return View();
        }

        // POST: MenuCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MenuId")] MenuCategory menuCategory)
        {
            menuCategory.Id = 0;
            await RestHelper.ApiCreate<MenuCategory>(menuCatApi, menuCategory);
            return RedirectToAction("Index", new { id = menuCategory.MenuId });

        }

        // GET: MenuCategories/Edit/Id
        public async Task<IActionResult> Edit(int? id)
        {
            var menus = await RestHelper.ApiGet<Menu>(menuApi);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");

            return View(await RestHelper.ApiGet<MenuCategory>(menuCatApi, id));
        }

        // POST: MenuCategories/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MenuId")] MenuCategory menuCategory)
        {
            await RestHelper.ApiEdit<MenuCategory>(menuCatApi + id, menuCategory);
            return RedirectToAction("Index", "MenuCategories", new { id = menuCategory.MenuId });
        }

        // GET: MenuCategories/Delete/Id
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<MenuCategory>(menuCatApi, id));
        }

        // POST: MenuCategories/Delete/Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await RestHelper.ApiGet<MenuCategory>(menuCatApi, id);
            await RestHelper.ApiDelete<MenuCategory>(menuCatApi, id);
            return RedirectToAction(nameof(Index), new {id=m.MenuId});
        }

        private bool MenuCategoryExists(int id)
        {
            return _context.MenuCategory.Any(e => e.Id == id);
        }
    }
}
