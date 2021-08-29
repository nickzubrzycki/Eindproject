using Eindproject.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class MovieCommentViewModel : AllMoviesSeriesViewModel
    {
        public Dictionary<ApplicationUser, Comment>  UserToComment { get; set; }

        [Required]
        public string message { get; set; }
        [Required]
        public string LoggedInUserName { get; set; }

    }
}
