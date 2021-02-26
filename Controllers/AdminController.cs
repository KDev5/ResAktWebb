using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Data;
using ResAktWebb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ResAktWebb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Admin admin)
        {
            Admin verifiedUser = null;
            using (HttpClient client = new HttpClient())
            {

                var content = new StringContent(JsonConvert.SerializeObject(admin), Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("http://informatik10.ei.hv.se/UserService/Login", content))
                {
                    string jR = await response.Content.ReadAsStringAsync();
                    verifiedUser = JsonConvert.DeserializeObject<Admin>(jR);
                }
            }

            if (verifiedUser != null)
            {
                await AuthenticateUser(verifiedUser);
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Inloggningen är inte godkänd");
                return View();
            }
        }
        private async Task AuthenticateUser(Admin validatedLogin)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, validatedLogin.Username));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }


        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,CustomerName,NumParticipants,TableNum")] Reservation reservation)
        //{
        //    var a = new Reservation();
        //    using (HttpClient c = new HttpClient())
        //    {
        //        string dataAsJson = JsonConvert.SerializeObject(reservation);
        //        var r = await c.PostAsync("http://193.10.202.82/grupp5/api/reservations/", new StringContent(dataAsJson, Encoding.UTF8, "application/json"));
        //    }
        //    return View(a);

        //____________________

        //    var a = new Reservation();
        //    using (HttpClient c = new HttpClient())
        //    {
        //        var r = await c.GetAsync("http://193.10.202.82/grupp5/api/reservations/" + id);
        //        string jsonResponse = await r.Content.ReadAsStringAsync();
        //        try
        //        {
        //            a = JsonConvert.DeserializeObject<Reservation>(jsonResponse);
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }
        //    return View(a);


    }
}
