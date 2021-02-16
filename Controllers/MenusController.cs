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
    public class MenusController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();


        public MenusController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            List<Menu>menus = new List<Menu>();
            try
            {
               
                    var response = await client.GetAsync("http://193.10.202.82/grupp5/api/menus");
                    string jsonresponse = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<List<Menu>>(jsonresponse);
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menus);
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            Menu menu = new Menu();
            try
            {
                var response = await client.GetAsync("http://193.10.202.82/grupp5/api/menus/"+id);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                menu = JsonConvert.DeserializeObject<Menu>(jsonresponse);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menu);

        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Menu menu)
        {

            {
                var temp = new Menu();
                using (HttpClient c = new HttpClient())
                {
                    string dataAsJson = JsonConvert.SerializeObject(menu);
                    var r = await c.PostAsync("http://193.10.202.82/grupp5/api/menus/", new StringContent(dataAsJson, Encoding.UTF8, "application/json"));
                }

                return View(temp);
            }

        }
        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Menu menu = new Menu();
            try
            {
                var response = await client.GetAsync("http://193.10.202.82/grupp5/api/menus/" + id);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                menu = JsonConvert.DeserializeObject<Menu>(jsonresponse);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
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
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Menu menu = new Menu();
            try
            {

                var response = await client.GetAsync("http://193.10.202.82/grupp5/api/menus/" + id);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                menu = JsonConvert.DeserializeObject<Menu>(jsonresponse);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
