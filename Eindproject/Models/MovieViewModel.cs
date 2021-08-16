using Eindproject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class MovieViewModel
    {
        public string Title { get; set; }

        public string Overview { get; set; }

        public string PosterUrl { get; set; }

        public double Score { get; set; }

        public int AantalAfleveringen { get; set; }

        public int AantalGekekenAfleveringen { get; set; }

        public string Status { get; set; }

    }
}
