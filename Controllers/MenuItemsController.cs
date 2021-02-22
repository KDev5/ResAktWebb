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
    public class MenuItemsController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();
        string api = "http://informatik12.ei.hv.se/grupp5/api/";

        public MenuItemsController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(int? id)
        {
            List<MenuItems> menuItems = new List<MenuItems>();

            List<MenuItems> itemsForCategoryId = new List<MenuItems>();

            MenuCategory menuCategory = new MenuCategory();
            var menuResponse = await client.GetAsync(api + "menuCategories/" + id);
            string menuJsonResponse = await menuResponse.Content.ReadAsStringAsync();
            menuCategory = JsonConvert.DeserializeObject<MenuCategory>(menuJsonResponse);
            ViewData["MenuCategory"] = menuCategory.Name;
            try
            {

                var itemResponse = await client.GetAsync(api + "menuItems");
                string Jsonresponse = await itemResponse.Content.ReadAsStringAsync();
                menuItems = JsonConvert.DeserializeObject<List<MenuItems>>(Jsonresponse);
                foreach (var item in menuItems)
                {
                    if (item.MenuCategoryId == id)
                    {
                        itemsForCategoryId.Add(item);
                    }
                    System.Diagnostics.Debug.WriteLine(item.Name + ", " + item.MenuCategoryId);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(itemsForCategoryId);
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItems = await _context.MenuItems
                .Include(m => m.MenuCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItems == null)
            {
                return NotFound();
            }

            return View(menuItems);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            ViewData["MenuCategoryId"] = new SelectList(_context.MenuCategory, "Id", "Id");
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Allergies,Price,MenuCategoryId")] MenuItems menuItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuCategoryId"] = new SelectList(_context.MenuCategory, "Id", "Id", menuItems.MenuCategoryId);
            return View(menuItems);
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItems = await _context.MenuItems.FindAsync(id);
            if (menuItems == null)
            {
                return NotFound();
            }
            ViewData["MenuCategoryId"] = new SelectList(_context.MenuCategory, "Id", "Id", menuItems.MenuCategoryId);
            return View(menuItems);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Allergies,Price,MenuCategoryId")] MenuItems menuItems)
        {
            if (id != menuItems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemsExists(menuItems.Id))
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
            ViewData["MenuCategoryId"] = new SelectList(_context.MenuCategory, "Id", "Id", menuItems.MenuCategoryId);
            return View(menuItems);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItems = await _context.MenuItems
                .Include(m => m.MenuCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItems == null)
            {
                return NotFound();
            }

            return View(menuItems);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItems = await _context.MenuItems.FindAsync(id);
            _context.MenuItems.Remove(menuItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemsExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
