using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{
    [AllowAnonymous]
    public class MovieController : Controller
    {
        
        public readonly static string api_key = "70f88e8eb928860994e741cfd80e1ff0";
        public readonly static string base_url = "https://image.tmdb.org/t/p";
        public readonly static string file_size = "/w500";
        // To get the full image =>  base_url, file_size, file_path bv.
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IMovieRepository movieRepository;
        public MovieController(ApplicationDbContext applicationDbContext,
            HttpClient httpClient,
            IMovieRepository movieRepository)
        {
            this.httpClient = httpClient;
            _context = applicationDbContext;
            this.movieRepository = movieRepository;
        }
        public IActionResult Index()
        {
       

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
        public IActionResult ViewFilmSerie(string name, string movietype)
        {
            // Terug ophalen van films of Serie in de file
            // Voor echt Id te gaan halen en te displayen op het scherm
           
            
            int Id = 0;         
            if(movietype == "Serie")
            {
                Id = GetSpecificSerieMovie("series", name);
                string serieUrl = $"3/tv/{Id}?api_key={api_key}&language=en-US";
            }
            else
            {
                Id = GetSpecificSerieMovie("movies", name);
                string movieUrl = $"3/movie/{Id}?api_key={api_key}&language=en-US";
                Console.WriteLine(Id);
            }
            
            
            
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult UnWatchlist(int counter)
        {
            
           

            var vm = movieRepository.GetAllMoviesNotWatched().Select(x => new AllMoviesSeriesViewModel
            {
                title = x.OriginalTitle,
                poster_path = base_url+file_size+x.FilmUrl,

            });
            List<AllMoviesSeriesViewModel[]> allMovies = new List<AllMoviesSeriesViewModel[]>();
            
            int totalNumberOfMovies = vm.Count();
            int remainder = 0;
            int quotient = 0;
            int counterArray = 0;
            counter = Math.Clamp(counter, 0, totalNumberOfMovies-1);
            Console.WriteLine(counter);
            ViewData["Counter"] = counter;
            
            // Aanmaken van list + Aantal van aangemaakt array te bepalen via remainder en quotient
            allMovies = ListOfAllMoviePages(quotient, remainder, totalNumberOfMovies, allMovies);


            // En dan op basis van wat de counter is de array meegeven via de view 

            allMovies = FillInListWithItems(vm.ToList(), counterArray, allMovies);
            ViewData["TotalPages"] = allMovies.Count();
            var moviesAndSeries = allMovies[counter];
            return View(moviesAndSeries);
        }

        public IActionResult Watchlist(int counter)
        {

            var vm = movieRepository.GetAllMoviesWatch().Select(x => new AllMoviesSeriesViewModel
            {
                title = x.OriginalTitle,
                poster_path = base_url + file_size + x.FilmUrl,

            });
            List<AllMoviesSeriesViewModel[]> allMovies = new List<AllMoviesSeriesViewModel[]>();

            int totalNumberOfMovies = vm.Count();
            int remainder = 0;
            int quotient = 0;
            int counterArray = 0;
            counter = Math.Clamp(counter, 0, totalNumberOfMovies);
            Console.WriteLine(counter);
            ViewData["Counter"] = counter;
            // Aanmaken van list + Aantal van aangemaakt array te bepalen via remainder en quotient
            allMovies = ListOfAllMoviePages(quotient, remainder, totalNumberOfMovies, allMovies);


            // En dan op basis van wat de counter is de array meegeven via de view 

            allMovies = FillInListWithItems(vm.ToList(), counterArray, allMovies);


            var moviesAndSeries = new AllMoviesSeriesViewModel[5];
            return View(moviesAndSeries);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string search)
        {
            ViewData["search"] = search;
            ViewData["Base"] = base_url;
            ViewData["File"] = file_size;

            // Pak de Search word to make an Api call
            // Wat als je geen search result terug krijgt 
            // Display een tekst van niet found 
            // Check of the 


            var result = await MultiSearchMovieOrSerie(search);

            // Make a search 

            return View(result);
        }


        /// <summary>
        /// Geeft een lijst terug van een aantal arrays waar alle movieviewmodels inkomen in elke array verdeeld per 5. 
        /// Tenzij er een rest is 
        /// </summary>
        /// <param name="quotient"></param>
        /// <param name="remainder"></param>
        /// <param name="totalNumberOfMovies"></param>
        /// <param name="allMovies"></param>
        /// <returns></returns>
        private List<AllMoviesSeriesViewModel[]> ListOfAllMoviePages(int quotient,
            int remainder,
            int totalNumberOfMovies, 
            List<AllMoviesSeriesViewModel[]> allMovies)
        {
            // Even getal deelbaar door 5 dus 
            if (totalNumberOfMovies % 5 == 0)
            {
                quotient = totalNumberOfMovies / 5;
                for (int i = 0; i < quotient; i++)
                {
                    AllMoviesSeriesViewModel[] allMovies1 = new AllMoviesSeriesViewModel[5];
                    allMovies.Add(allMovies1);
                    
                }

                return allMovies;
            }
            else
            {
                if(totalNumberOfMovies < 5)
                {
                    // Als het getal kleiner is dan 5 dan total aantal movies in 1 array 
                    allMovies.Add(new AllMoviesSeriesViewModel[totalNumberOfMovies]);
                    return allMovies;
                }
                else
                {
                    remainder = totalNumberOfMovies % 5;
                    quotient = (totalNumberOfMovies - remainder) / 5;
                    Console.WriteLine("list1: " + allMovies.Count());
                    for (int i = 0; i < quotient; i++)
                    {
                        AllMoviesSeriesViewModel[] allMovies1 = new AllMoviesSeriesViewModel[5];
                        allMovies.Add(allMovies1);

                    }
                    AllMoviesSeriesViewModel[] allMovies2 = new AllMoviesSeriesViewModel[remainder];
                    allMovies.Add(allMovies2);

                    return allMovies;


                }

            }
        }
        
        private List<AllMoviesSeriesViewModel[]> FillInListWithItems(
            List<AllMoviesSeriesViewModel> vm, int counterArray,
            List<AllMoviesSeriesViewModel[]> allMovies)
        {

            // Alle vm per array ga vullen 
            while (vm.Any())
            {
                if (allMovies[counterArray].Any())
                {
                    for (int i = 0; i < allMovies[counterArray].Length; i++)
                    {
                        allMovies[counterArray][i] = vm.First();
                        vm.RemoveAt(0);

                    }
                }
                counterArray++;
            }

            return allMovies;

        }


        private List<int> GetMovieOrSerieInfo(int id, string typeMovie)
        {
            string movieUrl = $"3/movie/{id}?api_key={api_key}&language=en-US";
            string TvUrl = $"3/tv/{id}?api_key={api_key}&language=en-US";
            List<int> MovieInfo;
            


            if(typeMovie == "Serie")
            {
                
                AllMoviesSeriesViewModel serie = MakeRequestMovieSerie(id, TvUrl).Result;
                MovieInfo = new List<int>{ serie.number_of_episodes, serie.season_number, serie.number_of_seasons };
                return MovieInfo;
            }
            else
            {
               
                AllMoviesSeriesViewModel movie = MakeRequestMovieSerie(id, movieUrl).Result;
                MovieInfo = new List<int> { (int)movie.runtime };
                return MovieInfo;
            }
            
        }
        /// <summary>
        /// Search for the trending Movies and Series
        /// </summary>
        public void TrendingMoviesAndSeries()
        {
            string url = "3/trending/all/day?api_key=" + api_key;

            //Checken of het result dat je terug krijgt niet leeg is 
            // Als het leeg is dan een error page laten zien dat er iets fout 
            List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels = GetAllMoviesAndSeries(url).Result;

            // Display the trending movies in a carousel 



        }


        /// <summary>
        /// Geeft alle films en series die nog niet zijn uitgekomen maar die wel al een release datum hebben
        /// </summary>
        private void GetAllMoviesAndSeriesNotReleased()
        {
            string url = $"3/discover/movie?api_key={api_key}&language=en-US&sort_by=popularity.desc&" +
               $"include_adult=false&include_video=false&page=1&" +
               $"primary_release_year={DateTime.Now.Year + 1}&with_watch_monetization_types=flatrate";

            List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels = GetAllMoviesAndSeries(url).Result;

        }
        /// <summary>
        /// De films of series die juist zijn uitgekomen ophalen via Api. Allemaal ophalen en 20 max laten zien.
        /// Laten kiezen of series moeten worden opgehaald of films
        /// </summary>
        public void GetAllMoviesAndSeriesReleased()
        {
            string url = $"3/discover/movie?api_key={api_key}&language=en-US&sort_by=popularity.desc&" +
                $"include_adult=false&include_video=false&page=1&" +
                $"primary_release_year={DateTime.Now.Year}&with_watch_monetization_types=flatrate";

            List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels = GetAllMoviesAndSeries(url).Result;
            // Handle different error that occur with switch

            //Kijken naar de status code van de eerste 


        }
        /// <summary>
        /// Haalt een series of movies op vanuit de API en maakt van een json een viewmodel.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<List<AllMoviesSeriesViewModel>> GetAllMoviesAndSeries(string url)
        {
            List<AllMoviesSeriesViewModel> allMoviesSeriesViews = new List<AllMoviesSeriesViewModel>();
            AllMoviesSeriesViewModel moviesSeriesViewModel;
            var response = await httpClient.GetAsync(url);
            try
            {
                // Aanpassen van Get string async naar get sync


                // Check that response was successful or throw exception
                response.EnsureSuccessStatusCode();

                // Read response asynchronously as JsonValue


                var result = await response.Content.ReadAsStringAsync();


                moviesSeriesViewModel = JsonConvert.DeserializeObject<AllMoviesSeriesViewModel>(result);
                foreach (var mov in moviesSeriesViewModel.results)
                {
                    // Omzetten van object attributen naar MovieViewModel 
                    // Omzetten naar json file en dan mappen 
                    var json = JsonConvert.SerializeObject(mov);

                    moviesSeriesViewModel = JsonConvert.DeserializeObject<AllMoviesSeriesViewModel>(json);
                    moviesSeriesViewModel.StatusCode = (int)response.StatusCode;
                    if (moviesSeriesViewModel.release_date == null)
                    {
                        moviesSeriesViewModel.MovieOrSerie = "Serie";
                        int Id = GetSpecificSerieMovie("series", moviesSeriesViewModel.original_name);
                        List<int> serie = GetMovieOrSerieInfo(Id,
                            moviesSeriesViewModel.MovieOrSerie);
                        moviesSeriesViewModel.number_of_episodes = serie[0];
                        moviesSeriesViewModel.season_number = serie[1];
                        moviesSeriesViewModel.number_of_seasons = serie[2];
                    }
                    else
                    {
                        moviesSeriesViewModel.MovieOrSerie = "Movie";
       
                    }
                    // Plak de statuscode aan de eerste object dat je meegeeft 


                    Console.WriteLine(moviesSeriesViewModel.title);
                    // Voor elke object invoegen in een list van movieobjects
                    allMoviesSeriesViews.Add(moviesSeriesViewModel);
                }
                return allMoviesSeriesViews;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                moviesSeriesViewModel = new AllMoviesSeriesViewModel();
                moviesSeriesViewModel.StatusCode = (int)e.StatusCode;
                allMoviesSeriesViews.Add(moviesSeriesViewModel);

                // Handle failure
            }
            return allMoviesSeriesViews;

        }
        /// <summary>
        /// Laten tonen als je een bepaalde error terugkrijgt van de API response  met een gepaste errorpage voor de user
        /// </summary>
        /// <param name="allMoviesSeriesViewModels"></param>
        private string ErrorHandlingRequestAPI(List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels)
        {
            int Statuscode = allMoviesSeriesViewModels.SingleOrDefault().StatusCode;

            switch (Statuscode)
            {

                case 404:
                    return "No result found.";
                case 500:
                    return "An error occurred in the server";
                    

            }
            return null;
        }


        //Multisearch only for movies and  series 



        /// <summary>
        /// Zoek naar een bepaalde film of serie online via de API
        /// </summary>
        private Task<List<AllMoviesSeriesViewModel>> MultiSearchMovieOrSerie(string query)
        {
            // Merge the series and movies together
            string urlMovies = $"3/search/movie?api_key={api_key}&language=en-US&page=1&query={query}&include_adult=false";
            string urlSeries = $"3/search/tv?api_key={api_key}&language=en-US&page=1&query={query}&include_adult=false";

            List<AllMoviesSeriesViewModel> allMoviesViewModels = GetAllMoviesAndSeries(urlMovies).Result;
            List<AllMoviesSeriesViewModel> allSeriesViewModels = GetAllMoviesAndSeries(urlSeries).Result;

            var AllMoviesSeries = allMoviesViewModels.Concat(allSeriesViewModels).ToList();
            return Task.FromResult(AllMoviesSeries);

        }

        private async Task<AllMoviesSeriesViewModel> MakeRequestMovieSerie(int id, string url)
        {

            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();


            string responseStream = await response.Content.ReadAsStringAsync();

            AllMoviesSeriesViewModel movieViewModel = System.Text.Json.JsonSerializer.Deserialize<AllMoviesSeriesViewModel>(responseStream);

            return await Task.FromResult(movieViewModel);

        }

        /*
         * Voorbeeld:  Random ophalen van een aantal films of series in json file
         */


        public int GetSpecificSerieMovie(string filename, string title)
        {
            List<AllMoviesSeriesViewModel> listOfMovies = new List<AllMoviesSeriesViewModel>();
            string filePath = SearchFile(filename);
            string json = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
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
                // Search in the filename to 
                SearchIdMovieSerie movieView = jsonSerializer.Deserialize<SearchIdMovieSerie>(jsonReader);
                
                if (movieView.original_title == title)
                {
                    Console.WriteLine(movieView.id);
                    return movieView.id;
                }
                else if (movieView.original_name == title)
                {
                    return movieView.id;
                }

               
            }
            return 0;

        }

        public  string SearchFile(string movieOrSerie)
        {
            // get to the correct directory in Json 

            // Vind de file name van de een movie of een serie 
            string fileName = string.Empty;
            string eindProjectPath = Directory.GetCurrentDirectory() + @"\Data\Json";
            DirectoryInfo directoryInfoEindproject = new DirectoryInfo(eindProjectPath);
            foreach (var directory in directoryInfoEindproject.GetDirectories())
            {
                if (directory.Name.Contains(movieOrSerie) == true)
                {
                    fileName = directory.GetFiles().FirstOrDefault().FullName;
                }

            }
            
            return fileName;



        }


    }


}
