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
        string api = "http://informatik12.ei.hv.se/grupp5/api/";

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
                var response = await client.GetAsync(api +"menus/"+ id);
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
                    string x = JsonConvert.SerializeObject(menu);
                    var r = await c.PostAsync(api + "menus/", new StringContent(x, Encoding.UTF8, "application/json"));
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
                var response = await client.GetAsync(api + "menus/" + id);
                string x = await response.Content.ReadAsStringAsync();
                menu = JsonConvert.DeserializeObject<Menu>(x);

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
            {
                var temp = new Menu();
                using (HttpClient c = new HttpClient())
                {
                    string x = JsonConvert.SerializeObject(menu);
                    var r = await c.PutAsync(api + "menus/" +  id, new StringContent(x, Encoding.UTF8, "application/json"));
                }

                return View(temp);
            }
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Menu menu = new Menu();
            try
            {

                var response = await client.GetAsync(api + "menus/" + id);
                string x = await response.Content.ReadAsStringAsync();
                menu = JsonConvert.DeserializeObject<Menu>(x);

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
            using (HttpClient c = new HttpClient())
            {
                var r = await c.DeleteAsync(api + "menus/" + id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
