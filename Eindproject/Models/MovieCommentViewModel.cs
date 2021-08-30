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

        [Required(ErrorMessage ="You need to fill in a message before posting anything")]
        public string message { get; set; }


    }
}
