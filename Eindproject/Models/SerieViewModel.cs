using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class SerieViewModel
    {
        public Object created_by { get; set; }

        public  string overview { get; set; }

        public int season_number { get; set; }


        public int number_of_episodes { get; set; }

        public int number_of_seasons { get; set; }

        public string original_name { get; set; }

        public int popularity { get; set; }

        public string poster_path { get; set; }

        public Object production_companies { get; set; }

        public string status { get; set; }// Is de serie nog in productie

        public double vote_average { get; set; }

        public int vote_count { get; set; }//aantal mensen gestemd op de serie

        public string last_episode_to_air { get; set; }


    }
}
