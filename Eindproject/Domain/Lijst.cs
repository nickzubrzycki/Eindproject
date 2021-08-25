using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    public class Lijst
    {
        [Key]
        public int LijstId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public DateTime ToegeVoegdOp { get; set; }

        public DateTime BewerktOp { get; set; }

        public ICollection<SerieOfFilm> serieFilmInLijsts { get; set; }

    
    
        
    }
}
