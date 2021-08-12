using Eindproject.Data;
using Eindproject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    [AllowAnonymous]
    public class LijstController : Controller
    {
        private readonly string api_key = "70f88e8eb928860994e741cfd80e1ff0";
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext _context;
        public LijstController(ApplicationDbContext applicationDbContext, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            _context = applicationDbContext; 
        }
        public async Task<IActionResult> Index()
        {
            //var response = await httpClient.GetAsync("3/movie/3?api_key="+api_key);

            //response.EnsureSuccessStatusCode();


            //string responseStream = await response.Content.ReadAsStringAsync();

            //MovieViewModel movieViewModel = JsonSerializer.Deserialize<MovieViewModel>(responseStream);

            //Console.WriteLine(movieViewModel.original_title);

            GetAllMoviesAndSeries();

           
            return View();
        }

        public void TrendingMoviesAndSeries()
        {

        }


        public void GetAllMoviesAndSeries()
        {
            List<MovieViewModel> listOfMovies = new List<MovieViewModel>();
            string filePath = SearchFile("movies");
            int counter = 0;
            string json = string.Empty;
            using(StreamReader sr = new StreamReader(filePath))
            {
                json = sr.ReadToEnd();
            }
            var jsonReader = new JsonTextReader(new StringReader(json))
            {
                SupportMultipleContent = true // This is important!
            };

            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            while (jsonReader.Read())
            {
                if(counter == 20)
                {
                    break;
                }
                MovieViewModel movieView = jsonSerializer.Deserialize<MovieViewModel>(jsonReader);

                listOfMovies.Add(movieView);

            }

        }

        public string SearchFile(string movieOrSerie)
        {
            // get to the correct directory in Json 

            // Vind de file name van de een movie of een serie 
            string fileName = string.Empty;
            string eindProjectPath = Directory.GetCurrentDirectory()+@"\Data\Json";
            DirectoryInfo directoryInfoEindproject = new DirectoryInfo(eindProjectPath);
            foreach(var directory in directoryInfoEindproject.GetDirectories())
            {
                Console.WriteLine(directory.Name);
                if(directory.Name.Contains(movieOrSerie) == true)
                {
                    fileName = directory.GetFiles().FirstOrDefault().FullName;
                }

            }
            Console.WriteLine(fileName);
            return fileName;



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
