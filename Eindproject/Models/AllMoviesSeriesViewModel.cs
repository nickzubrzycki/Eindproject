using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class AllMoviesSeriesViewModel
    {

        public int id { get; set; }

        private string movieOrSerie;
        public string MovieOrSerie {
            get { return movieOrSerie; }
            set { movieOrSerie = value; }
        }
        public string title { get; set; }
        public string original_title { get; set; } //Niet verwijderen. Dit is nodig voor de titel uit Movies.json te halen
        public DateTime? release_date { get; set; }
        public int runtime { get; set; } // 
        public double popularity { get; set; }

        public string  overview { get; set; }

        public double vote_average { get; set; }

        public string poster_path { get; set; }//Afbeelding van movie  of serie

        public string backdrop_path { get; set; }
        public Object[] genres { get; set; }

        public DateTime first_air_date { get; set; }

        //Kijk of het een film is of een serie
        public string status { get; set; }// Is de serie nog in productie

        public int season_number { get; set; }


        public int number_of_episodes { get; set; }

        public int number_of_seasons { get; set; }

        public int vote_count { get; set; }//aantal mensen gestemd op de serie

        public string last_episode_to_air { get; set; }

        public int[] genre_ids { get; set;  } // Opzoeken van genres voor de film

        public Object[] results { get; set; }

        // Als je een error zou terugkrijgen van de api 
        public int StatusCode { get; set; }
    }
}
