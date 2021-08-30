using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    public class SerieOfFilmInLijst
    {
        public int SerieOfFilmInLijstId { get; set; }
        public int ApiId { get; set; }
        public int LijstId { get; set; }
        [ForeignKey("LijstId")]
        public Lijst Lijst { get; set; }    
        public double Score { get; set; }
        public string OriginalTitle { get; set; }
        public int aantalAfleveringen { get; set; }
        public TimeSpan tijdPerAflevering { get; set; }
        public int aantalGekekenAfleveringen { get; set; }  
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        public string FilmUrl { get; set; }
    }
}
