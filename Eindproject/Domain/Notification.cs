using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    public class Notification
    {
        
        public int NotificationId { get; set; }

        public ApplicationUser User { get; set; }

        public string FromUserId { get; set; }
        [ForeignKey("FromUserId")]
        public ApplicationUser FromUser { get; set; }
        public string ToUserId { get; set;  }

        [ForeignKey("ToUserId")]
        public ApplicationUser ToUser { get; set; }

        public string HeaderMessage { get; set; }

        public string BodyMessage { get; set; }

        public bool IsRead { get; set; }

        public string Url { get; set; } //Verwijzing naar de pagina van de film

        public DateTime CreatedDate { get; set; }

  

    }
}
