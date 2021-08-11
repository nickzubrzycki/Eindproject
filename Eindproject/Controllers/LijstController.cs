using Eindproject.Data;
using Eindproject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    [AllowAnonymous]
    public class LijstController : Controller
    {
        public readonly string api_key = "70f88e8eb928860994e741cfd80e1ff0";
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext _context;
        public LijstController(ApplicationDbContext applicationDbContext, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            _context = applicationDbContext; 
        }
        public async Task<IActionResult> Index()
        {

            var response = await httpClient.GetAsync("3/movie/3?api_key=" + api_key);

            response.EnsureSuccessStatusCode();


            string responseStream = await response.Content.ReadAsStringAsync();

            MovieViewModel movieViewModel = JsonSerializer.Deserialize<MovieViewModel>(responseStream);

            Console.WriteLine(movieViewModel.runtime);

            // Mappen van array

            // Daily File reports upload.
            // Get the file from the internet 
            //



            // Proberen om Daily Exports file in te brengen om deze te gebruiken om Id's uithalen
            // Dit heb je nodig om de films en series op te halen



            return View();
        }

        /// <summary>
        /// Ontvang de dagelijkse of wekelijkse trending items. De dagelijkse trending-lijst houdt items bij over de periode van een dag, terwijl items een halfwaardetijd van 24 uur hebben. 
        /// De wekelijkse lijst houdt items bij over een periode van 7 dagen, 
        /// met een halfwaardetijd van 7 dagen.
        /// </summary>
        public void TrendingMoviesOrSeries()
        {

        }
        /// <summary>
        /// Krijg alle films id's die opgeslagen zijn in een dagelijkse file report om te zoeken
        /// naar films via de API. Dit kan je ook doen voor de series
        /// </summary>
        public void getAllMoviesFromApi()
        {
            // File ophalen via http client 
            // File omzetten naar een json file
            // in de file gaan en alle id's eruit halen
            
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
