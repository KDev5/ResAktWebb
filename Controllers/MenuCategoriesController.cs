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

namespace ResAktWebb.Controllers
{
    public class MenuCategoriesController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();
        string api = "http://informatik12.ei.hv.se/grupp5/api/";
        string menuCatApi = "menuCategories";
        public MenuCategoriesController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: MenuCategories
        public async Task<IActionResult> Index(int? id)
        {

            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            //var menu = await RestHelper.ApiGet<MenuCategory>("menus", id);

            List<MenuCategory> categoriesForMenuId = new List<MenuCategory>();
            Menu menu = new Menu();
            var menuResponse = await client.GetAsync(api + "menus/" + id);
            string menuJsonResponse = await menuResponse.Content.ReadAsStringAsync();
            menu = JsonConvert.DeserializeObject<Menu>(menuJsonResponse);
            ViewData["Menu"] = menu.Name;
                foreach (var item in menuCategories)
                {
                    if (item.MenuId == id)
                    {
                        categoriesForMenuId.Add(item);
                    }
                    System.Diagnostics.Debug.WriteLine(item.Name + ", " + item.MenuId);
                }
            
            return View(categoriesForMenuId);
        }

        // GET: MenuCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            MenuCategory menuCategory = new MenuCategory();
            try
            {
                var response = await client.GetAsync(api + "MenuCategories/" + id);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                menuCategory = JsonConvert.DeserializeObject<MenuCategory>(jsonresponse);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menuCategory);
        }

        // GET: MenuCategories/Create
        public async Task<IActionResult> Create()
        {
            List<Menu> menus = new List<Menu>();
            var menuResponse = await client.GetAsync(api + "menus/");
            string menuJsonResponse = await menuResponse.Content.ReadAsStringAsync();
            menus = JsonConvert.DeserializeObject<List<Menu>>(menuJsonResponse);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");
            return View();
        }

        // POST: MenuCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MenuId")] MenuCategory menuCategory)
        {

           

            var temp = new MenuCategory();
            using (HttpClient c = new HttpClient())
            {
                string x = JsonConvert.SerializeObject(menuCategory);
                var r = await c.PostAsync(api + "MenuCategories/", new StringContent(x, Encoding.UTF8, "application/json"));
            }

            return View(temp);

        }

        // GET: MenuCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["id"] = id;
            List<Menu> menus = new List<Menu>();
            var menuResponse = await client.GetAsync(api + "menus/");
            string menuJsonResponse = await menuResponse.Content.ReadAsStringAsync();
            menus = JsonConvert.DeserializeObject<List<Menu>>(menuJsonResponse);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");
            MenuCategory menuCategory = new MenuCategory();
            try
            {
                var response = await client.GetAsync(api + "MenuCategories/" + id);
                string x = await response.Content.ReadAsStringAsync();
                menuCategory = JsonConvert.DeserializeObject<MenuCategory>(x);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menuCategory);
        }

        // POST: MenuCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MenuId")] MenuCategory menuCategory)
        {
            {
                var temp = new Menu();
                using (HttpClient c = new HttpClient())
                {
                    string x = JsonConvert.SerializeObject(menuCategory);
                    var r = await c.PutAsync(api + "MenuCategories/" + id, new StringContent(x, Encoding.UTF8, "application/json"));
                }

                return View(temp);
            }
        }

        // GET: MenuCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            MenuCategory menuCategory = new MenuCategory();
            try
            {

                var response = await client.GetAsync(api + "MenuCategories/" + id);
                string x = await response.Content.ReadAsStringAsync();
                menuCategory = JsonConvert.DeserializeObject<MenuCategory>(x);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menuCategory);
        }

        // POST: MenuCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient c = new HttpClient())
            {
                var r = await c.DeleteAsync(api + "MenuCategories/" + id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MenuCategoryExists(int id)
        {
            return _context.MenuCategory.Any(e => e.Id == id);
        }
    }
}
