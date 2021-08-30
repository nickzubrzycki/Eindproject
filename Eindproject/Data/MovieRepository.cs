using Eindproject.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Data
{
    /// <summary>
    /// Verwijder, update of voeg een serie of film toe aan de lijst van films en series die 
    /// de gebruiker in de database houdt
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        // Data ophalen, veranderen en verwijderen van films en series in de database 
        private readonly ApplicationDbContext applicationDbContext;



        public MovieRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void AddMovieOrSerie(SerieOfFilmInLijst serieOfFilm)
        {
            applicationDbContext.SerieOfFilms.Add(serieOfFilm);
            Save();
            //Ophalen
        }


        /// <summary>
        /// Verwijder de movie of serie uit de lijst van de user. 
        /// </summary>
        /// <param name="MovieId"></param>
        /// <param name="UserId"></param>
        public void DeleteMovieOrSerie(int MovieId, string UserId)
        {
            // Zoek op UserId vanwege dat je die meekrijgt van zodra de user is ingelogd
            var movie  = applicationDbContext.SerieOfFilms.Where(p => p.ApiId == MovieId && p.Lijst.UserId == UserId).SingleOrDefault();
            applicationDbContext.SerieOfFilms.Remove(movie);
            Save();
     
        }
        /// <summary>
        /// Geef de lijst terug van de User. Alle films en series aan die lijst gekoppeld zijn.
        /// Bijvoorbeeld als je aan de user zijn collectie wilt laten zien.
        /// De User kan meerdere collecties hebben
        /// </summary>
        /// <param name="LijstId"></param>
        /// <returns></returns>
        public IEnumerable<SerieOfFilmInLijst> GetAllMoviesAndSeries(int LijstId)
        {

            // Search for every movie or serie that is linked to a given lijst 

            return applicationDbContext.SerieOfFilms.Where(x => x.LijstId == LijstId).ToList();

        }

        /// <summary>
        /// Zoeken naar Film of Serie in de lijst van de user 
        /// </summary>
        /// <param name="MovieId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public SerieOfFilmInLijst GetMovieOrSerie(int MovieId, string UserId)
        {
            return applicationDbContext.SerieOfFilms.SingleOrDefault(m => m.ApiId == MovieId && m.Lijst.UserId == UserId);
        }

        public void UpdateMovieOrSerie(int MovieId, SerieOfFilmInLijst serieOfFilm, string UserId)
        {
            // Zoek de movie in de lijst 
            // Dan vervang alle gegevens meegµegeven in SerieOffILM 
            var movie = applicationDbContext.SerieOfFilms.Where(p => p.ApiId == MovieId && p.Lijst.UserId == UserId).SingleOrDefault();
            if (movie != null)
            {
                movie.aantalAfleveringen = serieOfFilm.aantalAfleveringen;
                movie.aantalGekekenAfleveringen = serieOfFilm.aantalGekekenAfleveringen;
                movie.ApiId = serieOfFilm.ApiId;
                movie.FilmUrl = serieOfFilm.FilmUrl;
                movie.OriginalTitle = serieOfFilm.OriginalTitle;
                movie.Score = serieOfFilm.Score;
                movie.tijdPerAflevering = serieOfFilm.tijdPerAflevering;
            }
            Save();
        }

        /// <summary>
        /// Geef de lijst van alle films en series die al bekeken zijn
        /// Ophalen van films en series die al gekeken of niet voor de watchlist van de user
        /// </summary>
        /// <returns></returns>
        /// 
        public IEnumerable<SerieOfFilmInLijst> GetAllInList(int lijstid)
        {
            return applicationDbContext.SerieOfFilms.Where(x => x.LijstId == lijstid).ToList();
        }

        public IEnumerable<SerieOfFilmInLijst> GetAllMoviesWatch()
        {
            // Kijk voor de when de movie is 
            //Search for the data that has been watched
            return applicationDbContext.SerieOfFilms.Include(x => x.Status)
            .Where(x => x.Status.StatusDescription == "Done").ToList();
        }


        public IEnumerable<SerieOfFilmInLijst> GetAllMoviesNotWatched()
        {
            

            // Join doen op statusid voor vinden van items

            return applicationDbContext.SerieOfFilms.Include(x => x.Status)
            .Where(x => x.Status.StatusDescription == "Watching").ToList();

            
        }

        public void Save()
        {
            applicationDbContext.SaveChanges();
        }
    }

    public interface IMovieRepository
    {
        IEnumerable<SerieOfFilmInLijst> GetAllMoviesAndSeries(int LijstId );

        SerieOfFilmInLijst GetMovieOrSerie(int MovieId, string UserId); // De ApiId van de Serie en Film 

        void AddMovieOrSerie(SerieOfFilmInLijst serieOfFilm); 

        void UpdateMovieOrSerie(int MovieId, SerieOfFilmInLijst serieOfFilm, string UserId);// id voor vinden van Movie, Username voor vinden van lijst

        void DeleteMovieOrSerie(int MovieId, string UserId);//Id van film of serie

        IEnumerable<SerieOfFilmInLijst> GetAllMoviesNotWatched();

        IEnumerable<SerieOfFilmInLijst> GetAllMoviesWatch();
        IEnumerable<SerieOfFilmInLijst> GetAllInList(int id);
        void Save();
    }
}
