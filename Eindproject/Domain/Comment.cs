using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Created_Date { get; set; }

        public string Comment_Message { get; set; }

        public string MovieOrSerie_Title { get; set; } // Film of Serie gematched met die comment
    
        public virtual ICollection<Comment> OtherComments { get; set; } // Andere comments gereageerd op die bepaalde Comment
    }
}
