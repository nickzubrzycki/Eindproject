using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class TrendingMoviesSeriesViewModel
    {

        public int id { get; set; }
        public string original_title { get; set; }

        public double popularity { get; set; }

        public string media_type { get; set; }

        public string  overview { get; set; }

        public double vote_average { get; set; }

        public string poster_path { get; set; }//Afbeelding van movie  of serie

        public string first_air_date { get; set; }

        public Object[] results { get; set; }
    }
}
