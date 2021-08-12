using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    public class SerieOfFilm
    {
        [ForeignKey("Lijst")]
        public int LijstId { get; set; }
        //Inbrengen van remote Database => Foreign Key
        public int SerieOfFilmId { get; set; }

        public double Score { get; set; }

        public int aantalAfleveringen { get; set; }

        public TimeSpan tijdPerAflevering { get; set; }

        public int aantalGekekenAfleveringen { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }


        public Status Status { get; set; }

        public Lijst Lijst { get; set; }
    }
}
