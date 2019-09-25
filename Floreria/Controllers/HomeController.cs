using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Floreria.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;

namespace Floreria.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListadoFlores()
        {
            var floresList = new Flor { Codigo = "001", Nombre = "Margarita", Origen = "Italia" };
            return View(floresList);
        }
        public IActionResult ListadoFloresRecorre()
        {
            var floresListRecorre = new List<Flor>()
            {
                new Flor { Codigo = "001", Nombre = "Margarita", Origen = "Italia" },
                new Flor { Codigo = "002", Nombre = "Rosa", Origen = "Francia" },
                new Flor { Codigo = "003", Nombre = "Jasmin", Origen = "Argentina" }
            };
                     
            return View(floresListRecorre);
        }
        public IActionResult FloresAPI()
        {
            IEnumerable<Flor> flores = null;

            using (var client = new HttpClient())
            {
                var _clientBaseUrl = client.BaseAddress = new Uri("https://dmq9re9bpi.execute-api.us-west-2.amazonaws.com/test/flor");
                //HTTP GET
                var responseTask = client.GetAsync(_clientBaseUrl);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Flor>>();
                    readTask.Wait();

                    flores = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    flores = Enumerable.Empty<Flor>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(flores);
        }
    }
   
}
