using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class SerieFilmListView
    {
        public int Id { get; set; }

        public int ApiId { get; set; }
        public double Score { get; set; }
        public string OriginalTitle { get; set; }
        public int AantalAfleveringen { get; set; }
        public TimeSpan TijdPerAflevering { get; set; }
        public int AantalGekekenAfleveringen { get; set; }
        public string FilmUrl { get; set; }

    }
}
