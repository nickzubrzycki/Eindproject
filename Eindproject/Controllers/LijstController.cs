using Eindproject.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    public class LijstController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LijstController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext; 
        }
        public IActionResult Index()
        {
            var response = await httpClient.GetAsync("3/movie/3?api_key="+api_key);

            response.EnsureSuccessStatusCode();


            string responseStream = await response.Content.ReadAsStringAsync();

            MovieViewModel movieViewModel = JsonSerializer.Deserialize<MovieViewModel>(responseStream);

            Console.WriteLine(movieViewModel.original_title);


            // Proberen om Daily Exports file in te brengen om deze te gebruiken om Id's uithalen
            // Dit heb je nodig om de films en series op te halen


            // Display the object of a movie on the screen
            // Extract the values from the Json file by  deserialize 
            // Map that to a object 
            // Problem is you don't know what the json file contains from attributes?

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
