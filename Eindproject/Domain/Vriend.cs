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

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string BevriendId { get; set; }
        [ForeignKey("BevriendId")]
        public ApplicationUser Bevriend { get; set; }

        public bool Accepted { get; set; }
    }
}
