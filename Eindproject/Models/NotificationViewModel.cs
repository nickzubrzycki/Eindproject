using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public string HeaderMessage { get; set; }

        public string BodyMessage { get; set; }

        public bool IsRead { get; set; }

        public string Url { get; set; } //Verwijzing naar de pagina van de film

        public DateTime CreatedDate { get; set; }
    }
}
