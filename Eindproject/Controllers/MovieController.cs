using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private static MovieCommentViewModel MovieComment;
        Random rng = new Random();

        //Id's zijn hardcoded => komen van api moviedb.org genres
        private readonly Dictionary<string, int> allGenresMovies =
            new Dictionary<string, int>() 
            { {"Action", 28},
              {"Adventure", 12 },
              {"Animation", 16},
              {"Horror", 27 },
              { "Fantasy", 14},
              { "Romance", 10749 },
              { "Science Fiction", 878},
              { "Documentary", 99 },
              { "Crime", 80 },
              { "Family", 10751}
            };

        private readonly Dictionary<string, int> allGenresSeries =
            new Dictionary<string, int>()
            { {"Action & Adventure", 10759},
              {"Animation", 16},
              { "Comedy", 35},
              { "Crime", 80 },
              { "Family", 10751},
              {"Documentary", 99 },
              {"Drama", 18 },
              {"Mystery", 9648 },
              {"Kids", 10762 }
            };


        private readonly ICommentRepository commentRepository;
        private readonly UserManager<ApplicationUser> userManager; 
        private int itemsPerPage = 10; 
        public MovieController(ApplicationDbContext applicationDbContext,
            HttpClient httpClient,
            IMovieRepository movieRepository, 
            ICommentRepository commentRepository, 
            UserManager<ApplicationUser> userManager)
        {
            this.httpClient = httpClient;
            _context = applicationDbContext;
            this.movieRepository = movieRepository;
            this.commentRepository = commentRepository;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            //Oproepen van Not realeased movies 
            // Trending movies 
            // Popular movies 
            List<AllMoviesSeriesViewModel[]> allMoviesForStartPage = new List<AllMoviesSeriesViewModel[]>();

            ViewData["Base"] = base_url;
            ViewData["File"] = file_size;


            allMoviesForStartPage.Add(TrendingMoviesAndSeries());
            allMoviesForStartPage.Add(GetAllMoviesNotReleased());
            allMoviesForStartPage.Add(GetMostPopularMovies());


            return View(allMoviesForStartPage);
        }


        /// <summary>
        /// Voeg een nieuwe Film of Serie toe
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromRoute] AllMoviesSeriesViewModel ms)
        {
            var newMovie = new SerieOfFilmInLijst
            {
                ApiId = ms.id
            };

            _context.SerieOfFilms.Add(newMovie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
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
        /// Post a new Comment from the user on the movie
        /// </summary>sdfqdfs
        /// <param name="movieCommentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewFilmSerie([FromForm] MovieCommentViewModel movieCommentViewModel)
        {
            
                if(ModelState.IsValid)
                {
                    Console.WriteLine("dsfjlkmdf");
                    Comment comment = new Comment();
                    comment.Created_Date = DateTime.Now;
                    var user = await userManager.GetUserAsync(User);
                    comment.UserId = user.Id;
                    comment.Comment_Message = movieCommentViewModel.message;
                    if (MovieComment.MovieOrSerie == "Movie")
                    {
                        comment.MovieOrSerie_Title = MovieComment.original_title;
                        commentRepository.AddComment(comment);
                        MovieComment.UserToComment = GenerateCommentsForMovie(comment.MovieOrSerie_Title);
                    }
                    else
                    {
                        comment.MovieOrSerie_Title = MovieComment.original_name;
                        commentRepository.AddComment(comment);
                        MovieComment.UserToComment = GenerateCommentsForMovie(comment.MovieOrSerie_Title);
                    }


                }

            //Zorgen dat als ik een view terug geef

            return View(MovieComment);

  
        }
        /// <summary>
        /// Edit comment from User with a modal
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int CommentId)
        {
            var comment = commentRepository.GetComment(CommentId);
            CommentViewModel commentView = new CommentViewModel();
            commentView.Comment_Id = CommentId;
            commentView.Comment_Message = comment.Comment_Message;
            commentView.Created_Date = comment.Created_Date;
            return PartialView("CommentModalPartial", commentView);
        }

        /// <summary>
        /// Post new Comment to database
        /// </summary>
        /// <param name="Comment_Id"></param>
        /// <param name="Comment_Message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Comment_Id, string Comment_Message)
        {
            Comment comment = commentRepository.GetComment(Comment_Id);
            comment.Comment_Message = Comment_Message;
            comment.Created_Date = DateTime.Now;
            commentRepository.UpdateComment(comment);
            return PartialView("CommentModalPartial", new CommentViewModel());
        }

        [HttpPost]
        [Route("Movie/Comment/{id}")]
        public IActionResult Comment(int? id)
        {
            
            if(id == null)
            {
                return NotFound();
            }else
            {
                commentRepository.DeleteComment((int)id);
                if(MovieComment.MovieOrSerie == "Movie")
                {
                    MovieComment.UserToComment = GenerateCommentsForMovie(MovieComment.original_title);
                }
                else
                {
                    MovieComment.UserToComment = GenerateCommentsForMovie(MovieComment.original_name);
                }

            }


            return Json(new { success = true, order = MovieComment });
        }

        /// <summary>
        /// Nakijken gegevens film en serie
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ViewFilmSerie(string name, string movietype)
        {
            // Terug ophalen van films of Serie in de file
            // Voor echt Id te gaan halen en te displayen op het scherm
            // View Al aanmaken
            MovieCommentViewModel vm = null;
         
            if(movietype == "Serie")
            {
                int id = GetSpecificSerieMovie("series", name);
                string tvUrl = $"3/tv/{id}?api_key={api_key}&language=en-US";
                //Opvullen van alle comments for the serie 

               
                vm = GetMovieOrSerie(tvUrl).Result;
                vm.poster_path = base_url + file_size + vm.poster_path;
            }
            else
            {
                int id = GetSpecificSerieMovie("movies", name);
                string movieUrl = $"3/movie/{id}?api_key={api_key}&language=en-US";
                //openhalen via url 
                //View vullen
                vm = GetMovieOrSerie(movieUrl).Result;
                vm.poster_path = base_url + file_size + vm.poster_path;

            }
          
            vm.UserToComment = GenerateCommentsForMovie(name);
            MovieComment = vm;
            return View(vm);
        }
        public IActionResult ViewRandomFilmSerie()
        {
            // Terug ophalen van films of Serie in de file
            // Voor echt Id te gaan halen en te displayen op het scherm
            // View Al aanmaken
            AllMoviesSeriesViewModel vm = null;
            string movietype = "Movie";
            
            int random = rng.Next(0, 10);

            if (random % 2 == 0)
                movietype = "Serie";


            if (movietype == "Serie")
            {
                int id = GetRandomMovie("series");
                string tvUrl = $"3/tv/{id}?api_key={api_key}&language=en-US";
                //openhalen via url 
                //View vullen
                vm = GetMovieOrSerie(tvUrl).Result;
                vm.poster_path = base_url + file_size + vm.poster_path;
                Console.WriteLine(vm.poster_path, vm.overview);

  
                return View(vm);
            }
            else
            {
                int id = GetRandomMovie("movies");
                string movieUrl = $"3/movie/{id}?api_key={api_key}&language=en-US";
                //openhalen via url 
                //View vullen
                vm = GetMovieOrSerie(movieUrl).Result;

                vm.poster_path = base_url + file_size + vm.poster_path;
                Console.WriteLine(vm.poster_path, vm.overview);
                return View(vm);
            }
        
        }
            
        
        public IActionResult Delete([FromRoute] int Id)
        {
            var ms = _context.SerieOfFilms.FirstOrDefault(x => x.ApiId == Id);

            var vm = new MovieDeleteViewModel
            {
                Id = ms.ApiId,
                Title = ms.OriginalTitle,
                poster_path = ms.FilmUrl
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult ConfirmDelete([FromRoute] int Id)
        {
            _context.SerieOfFilms.Remove(_context.SerieOfFilms.FirstOrDefault(x => x.ApiId == Id));
            _context.SaveChanges();

            return RedirectToAction(nameof(Watchlist));
        }

        public IActionResult UnWatchlist(int counter)
        {

            var vm = movieRepository.GetAllMoviesNotWatched().Select(x => new AllMoviesSeriesViewModel
            {
                title = x.OriginalTitle,
                poster_path = base_url + file_size + x.FilmUrl,

            });
            List<AllMoviesSeriesViewModel[]> allMovies = new List<AllMoviesSeriesViewModel[]>();

            int totalNumberOfMovies = vm.Count();
            int remainder = 0;
            int quotient = 0;
            int counterArray = 0;
            counter = Math.Clamp(counter, 0, totalNumberOfMovies - 1);
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

            return View(vm);
        }


        public IActionResult Genre(string name)
        {
            //Zoek met de naam string de  Id op om een API call te maken voor de Genres op te halen
            bool genreInDicMovie = allGenresMovies.TryGetValue(name, out int idMovie);
            
            
            if(genreInDicMovie == true)
            {
                var urlMovie = $"https://api.themoviedb.org/3/discover/movie?api_key={api_key}&with_genres={idMovie}";
                var moviesForGenre =   GetAllMoviesAndSeries(urlMovie).Result;
                ViewData["genre"] = name;
                ViewData["Base"] = base_url;
                ViewData["File"] = file_size;
                return View(moviesForGenre);

            }
            else
            {
                return NotFound();
            }
        
        }


        public IActionResult GenreSerie(string name)
        {
            bool genreInDicSerie = allGenresSeries.TryGetValue(name, out int idSerie);
            if(genreInDicSerie == true)
            {
                var url = $"https://api.themoviedb.org/3/discover/tv?api_key={api_key}&language=en-US&sort_by=popularity.desc&page=1&timezone=America%2FNew_York&with_genres={idSerie}&include_null_first_air_dates=false&with_watch_monetization_types=flatrate";
                var SerieForGenre = GetAllMoviesAndSeries(url).Result;
                ViewData["genre"] = name;
                ViewData["Base"] = base_url;
                ViewData["File"] = file_size;
                return View(SerieForGenre);
            }
            else
            {
                return NotFound();
            }
        }


        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(  string search,  int counter)
        {
            ViewData["search"] = search;
            ViewData["Base"] = base_url;
            ViewData["File"] = file_size;
            var result = new List<AllMoviesSeriesViewModel>();
            result = await MultiSearchMoviesOrSeries(search);

            if ( search !=  null)
            {
                List<AllMoviesSeriesViewModel[]> allMovies = new List<AllMoviesSeriesViewModel[]>();
                if (result.Any())
                {

                    int totalNumberOfMovies = result.Count();
                    int remainder = 0;
                    int quotient = 0;
                    int counterArray = 0;


                    // Aanmaken van list + Aantal van aangemaakt array te bepalen via remainder en quotient
                    allMovies = ListOfAllMoviePages(quotient, remainder, totalNumberOfMovies, allMovies);

                    counter = Math.Clamp(counter, 0, allMovies.Count() - 1);
                    ViewData["Counter"] = counter;


                    // En dan op basis van wat de counter is de array meegeven via de view 
                    allMovies = FillInListWithItems(result.ToList(), counterArray, allMovies);
                    ViewData["TotalPages"] = allMovies.Count();
                    var moviesAndSeries = allMovies[counter];
                    return View(moviesAndSeries);


                }
                else
                {

                    ViewData["Counter"] = 0;
                    ViewData["TotalPages"] = 0;
                    return View(new AllMoviesSeriesViewModel[10]);
                }

            }
            else
            {

                ViewData["Counter"] = 0;
                ViewData["TotalPages"] = 0;

                return View(new AllMoviesSeriesViewModel[10]);
            }
            
         

            // Make a search 

            
        }


        private void GetLatestMoviesTrailers()
        {

            // Display the 3 latest trailers of movies in the cinema
            // Find the movies that has just been released
            // Search on the date from now
            // Movies just released
            
        }

        /// <summary>
        /// Build a comment section in the detail for each film or serie.The comments order recent from oldest
        /// </summary>
        private Dictionary<ApplicationUser, Comment> GenerateCommentsForMovie(string title)
        {
            //Dictionary<int, ApplicationUser> UserIdToUser = MapUserIdToUser();
            List<Comment> CommentsOrderedByDate = OrderComments(commentRepository.GetComments(title).ToList());
            Dictionary<ApplicationUser, Comment> UserToComment = new Dictionary<ApplicationUser, Comment>();
            // Haal alle userId's uit de lijst 
            foreach(var comment in CommentsOrderedByDate)
            {
                if(comment.Comment_Message != null)
                {
                    var commentUserTuple = MapUserWithComment(ListAllUsers(), comment);
                    UserToComment.Add(commentUserTuple.Item1, commentUserTuple.Item2);
                }
                
            }

            return UserToComment;


      
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
            if (totalNumberOfMovies % itemsPerPage == 0)
            {
                quotient = totalNumberOfMovies / itemsPerPage;
                for (int i = 0; i < quotient; i++)
                {
                    AllMoviesSeriesViewModel[] allMovies1 = new AllMoviesSeriesViewModel[itemsPerPage];
                    allMovies.Add(allMovies1);

                }

                return allMovies;
            }
            else
            {
                if(totalNumberOfMovies < itemsPerPage)
                {
                    // Als het getal kleiner is dan 5 dan total aantal movies in 1 array 
                    allMovies.Add(new AllMoviesSeriesViewModel[totalNumberOfMovies]);
                    return allMovies;
                }
                else
                {
                    remainder = totalNumberOfMovies % itemsPerPage;
                    quotient = (totalNumberOfMovies - remainder) / itemsPerPage;
                    for (int i = 0; i < quotient; i++)
                    {
                        AllMoviesSeriesViewModel[] allMovies1 = new AllMoviesSeriesViewModel[itemsPerPage];
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

            List<int> MovieInfo;

            if (typeMovie == "Serie")
            {
                string TvUrl = $"3/tv/{id}?api_key={api_key}&language=en-US";
                AllMoviesSeriesViewModel serie = MakeRequestMovieSerie(id, TvUrl).Result;
                MovieInfo = new List<int> { serie.number_of_episodes, serie.season_number, serie.number_of_seasons };
                return MovieInfo;
            }
            else
            {
                string movieUrl = $"3/movie/{id}?api_key={api_key}&language=en-US";
                AllMoviesSeriesViewModel movie = MakeRequestMovieSerie(id, movieUrl).Result;
                MovieInfo = new List<int> { (int)movie.runtime };
                return MovieInfo;
            }

        }
        /// <summary>
        /// Search for the trending Movies and Series
        /// </summary>
        private AllMoviesSeriesViewModel[] TrendingMoviesAndSeries()
        {
            string url = $"3/trending/all/day?api_key={api_key}"; 

            //Checken of het result dat je terug krijgt niet leeg is 
            // Als het leeg is dan een error page laten zien dat er iets fout 
            List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels = GetAllMoviesAndSeries(url).Result;

            AllMoviesSeriesViewModel[] trendingMoviesAndSeries = ConvertListMoviesToArray(allMoviesSeriesViewModels);

            return trendingMoviesAndSeries;
            // Display the trending movies in a carousel 



        }


        /// <summary>
        /// Geeft alle films en series die nog niet zijn uitgekomen maar die wel al een release datum hebben
        /// </summary>
        private AllMoviesSeriesViewModel[] GetAllMoviesNotReleased()
        {
            string url = $"3/discover/movie?api_key={api_key}&language=en-US&sort_by=popularity.desc&" +
               $"include_adult=false&include_video=false&page=1&" +
               $"primary_release_year={DateTime.Now.Year + 1}&with_watch_monetization_types=flatrate";

            List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels = GetAllMoviesAndSeries(url).Result;

            AllMoviesSeriesViewModel[] moviesNotReleased = ConvertListMoviesToArray(allMoviesSeriesViewModels);
            return moviesNotReleased;
        }
        /// <summary>
        /// Geeft de 10 meest populaire films terug die momenteel in de cinema zijn
        /// </summary>
        /// <returns></returns>
        public AllMoviesSeriesViewModel[] GetMostPopularMovies()
        {
            string url = $"3/discover/movie?api_key={api_key}&language=en-US&sort_by=popularity.desc&" +
                $"include_adult=false&include_video=false&page=1&" +
                $"primary_release_year={DateTime.Now.Year}&with_watch_monetization_types=flatrate";

            List<AllMoviesSeriesViewModel> allMoviesSeriesViewModels = GetAllMoviesAndSeries(url).Result;
            //Omzetten van list naar array. 10 best er in doen
            AllMoviesSeriesViewModel[] mostPopularMovies = ConvertListMoviesToArray(allMoviesSeriesViewModels);

            return mostPopularMovies;
         


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
                        if(moviesSeriesViewModel.original_name != null)
                        {
                            int Id = GetSpecificSerieMovie("series", moviesSeriesViewModel.original_name);
                            List<int> serie = GetMovieOrSerieInfo(Id,
                                moviesSeriesViewModel.MovieOrSerie);
                            moviesSeriesViewModel.number_of_episodes = serie[0];
                            moviesSeriesViewModel.season_number = serie[1];
                            moviesSeriesViewModel.number_of_seasons = serie[2];
                        }
                       
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
            }finally
             {
                moviesSeriesViewModel = new AllMoviesSeriesViewModel();
                allMoviesSeriesViews.Add(moviesSeriesViewModel);
            }
            return allMoviesSeriesViews;

        }
        private async Task<MovieCommentViewModel> GetMovieOrSerie(string url)
        {
            MovieCommentViewModel moviesSeriesViewModel;
            var response = await httpClient.GetAsync(url);
            try
            {
                // Aanpassen van Get string async naar get sync
                // Check that response was successful or throw exception
                response.EnsureSuccessStatusCode();

                // Read response asynchronously as JsonValue
                var result = await response.Content.ReadAsStringAsync();
                moviesSeriesViewModel = JsonConvert.DeserializeObject<MovieCommentViewModel>(result);

                // Omzetten van object attributen naar MovieViewModel 
                // Omzetten naar json file en dan mappen 
                var json = JsonConvert.SerializeObject(moviesSeriesViewModel);

                moviesSeriesViewModel = JsonConvert.DeserializeObject<MovieCommentViewModel>(json);
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
                // het ene object toevoegen 
                return moviesSeriesViewModel;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                moviesSeriesViewModel = new MovieCommentViewModel();
                moviesSeriesViewModel.StatusCode = (int)e.StatusCode;

                // Handle failure
            }
            return moviesSeriesViewModel;

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



       
 
        /// <summary>
        /// Vraagt alle movies en series op basis van keywoord dat is ingegeven van de user
        /// </summary>
        private Task<List<AllMoviesSeriesViewModel>> MultiSearchMoviesOrSeries(string query)
        {
            // Merge the series and movies together
            string urlMovies = $"3/search/movie?api_key={api_key}&language=en-US&page=1&query={query}&include_adult=false";
            string urlSeries = $"3/search/tv?api_key={api_key}&language=en-US&page=1&query={query}&include_adult=false";

            List<AllMoviesSeriesViewModel> allMoviesViewModels = GetAllMoviesAndSeries(urlMovies).Result;
            List<AllMoviesSeriesViewModel> allSeriesViewModels = GetAllMoviesAndSeries(urlSeries).Result;

  
            var AllMoviesSeries = allMoviesViewModels.Concat(allSeriesViewModels).ToList();
            return Task.FromResult(AllMoviesSeries);

        }
        /// <summary>
        /// Maakt een request op een bepaalde film of serie op te halen via de api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gaat de Id uit de json files movies of series halen om een request te kunnen doen voor een bepaalde
        /// Serie of film op te halen. 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="title"></param>
        /// <returns></returns>
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
                //hier checken op mijn random generated id ipv title
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
        public int GetRandomMovie(string filename)
        {
            List<AllMoviesSeriesViewModel> listOfMovies = new List<AllMoviesSeriesViewModel>();

            int rid = 0;

            bool found = false;

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

        Start:

            if (filename == "movies")
            {
                rid = rng.Next(860342);
            }
            else
            {
                rid = rng.Next(130940);
            }

            while (jsonReader.Read())
            {
                // Search in the filename to 
                SearchIdMovieSerie movieView = jsonSerializer.Deserialize<SearchIdMovieSerie>(jsonReader);
                //hier checken op mijn random generated id ipv title
                if (movieView.id == rid)
                {
                    return movieView.id;
                }
            }
            if (found == false)
            {
                goto Start;
            }
            return 0;
        }
        /// <summary>
        /// Zoek naar een bepaalde file voor movies of series
        /// </summary>
        /// <param name="movieOrSerie"></param>
        /// <returns></returns>
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

        private AllMoviesSeriesViewModel[] ConvertListMoviesToArray(List<AllMoviesSeriesViewModel> allMovies)
        {
            AllMoviesSeriesViewModel[] allMoviesArray = new AllMoviesSeriesViewModel[10];
            for (int i = 0; i < 10; i++)
            {
                allMoviesArray[i] = allMovies[i];
            }
            return allMoviesArray;
        }

        private List<ApplicationUser> ListAllUsers()
        {
            var AllUser = userManager.Users;
            //Omzetten van IQuerable naar IEnumerable
            List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            foreach(var ms in AllUser)
            {
                applicationUsers.Add(ms);
            }

            return applicationUsers;
        }

        /// <summary>
        /// Ordered de comments van oudste naar niewste. Moeten gemapt worden met comment en in dictionary gestoken worden
        /// Daarom gaan de oudste er eerst in en dan pas als laatste de nieuwste
        /// </summary>
        /// <returns></returns>
        private List<Comment> OrderComments(List<Comment> comments)
        {
            return comments.OrderBy(p => p.Created_Date).ToList();
        }

        /// <summary>
        /// Map the Userid die in de Database van de Comment table zitten met de 
        /// User van de db
        /// </summary>
        /// <returns></returns>
        private Tuple<ApplicationUser, Comment> MapUserWithComment (List<ApplicationUser> AllUsers, Comment comment)
        {
            
            foreach (var ms in AllUsers)
            {
               if(ms.Id == comment.UserId)
               {
                    return Tuple.Create(ms, comment);
               }
            }
            return null;
        }

      
    }

}
