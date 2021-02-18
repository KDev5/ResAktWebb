using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{

	// https://jankcat.com/2016/03/27/net-simple-http-client-helper-class/
	public class RestHelper
	{
		// Skapar en statisk httpclient som kan användas genom hela projektet.
		private static readonly HttpClient client = new HttpClient();
		// Connectionstring till api.
		private static readonly string connString = "http://informatik12.ei.hv.se/grupp5/api/";

		public static async Task<List<T>> ApiGet<T>(string model)
		{
			// Generisk lista som kan spara olika typer av klasser.
			var returnList = new List<T>();
			try
			{
				var res = await client.GetAsync(connString + "MODELNAME GOES HERE");
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Something went wrong!!!: \n \t " + e.ToString());
				throw;
			}

			return returnList;
		}




	}
}
