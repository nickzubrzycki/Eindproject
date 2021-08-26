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

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


        public DateTime ToegeVoegdOp { get; set; } = DateTime.Now;

        public DateTime BewerktOp { get; set; } = DateTime.Now;


    
    
        
    }
}
