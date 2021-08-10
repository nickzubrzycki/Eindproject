using Eindproject.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class LijstController : Controller
    {
        public readonly string api_key = "70f88e8eb928860994e741cfd80e1ff0";
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext _context;

        public LijstController(HttpClient httpClient)
        {
            this.httpClient = httpClient;

        }

        
        public  async Task<IActionResult> Index()
        {
            var response = await httpClient.GetAsync("3/movie/76341?api_key="+api_key);

            response.EnsureSuccessStatusCode();


            using var responseStream = await response.Content.ReadAsStreamAsync();
            Console.WriteLine(response);
            return View();
        }

        /// <summary>
        /// Voeg een nieuwe Film of Serie toe
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View(); 
        }

        /// <summary>
        /// Update gegevens van film of serie
        /// </summary>
        /// <returns></returns>
        public IActionResult Update()
        {
            return View(); 
        }

        /// <summary>
        /// Nakijken gegevens film en serie
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewFilmSerie()
        {
            return View(); 
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
