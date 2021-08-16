using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    public class Vriend
    {
        public int VriendId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        
        [ForeignKey("BevriendId")]
        public virtual ApplicationUser Bevriend { get; set; }

        public bool Accepted { get; set; }
    }
}
