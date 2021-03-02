﻿using Microsoft.AspNetCore.Mvc;
using ResAktWebb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ResAktWebb.Controllers
{
	public class GuestController : Controller
	{
		public IActionResult Index()
		{
			// Hit skall guest komma efter inloggning. SUB-Meny behöver finnas för Mina sidor, Restauranginfo och visa bokade aktiviteter/bord.

			return View();
		}

		public async Task<IActionResult> GetActivities()
		{
			var a = await RestHelper.ApiGet<Activity>("Activities");

			return View(a);
		}

		public async Task<IActionResult> GetReservations()
		{
			var a = await RestHelper.ApiGet<Reservation>("Reservations");

			return View(a);
		}

		public async Task<IActionResult> RestaurantInfo()
		{
			var info = await RestHelper.ApiGet<RestaurantInfo>("restaurantInfoes/");
			
			return View(info);
		}

	}
}
