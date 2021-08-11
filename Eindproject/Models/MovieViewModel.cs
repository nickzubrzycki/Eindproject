using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class MovieViewModel
    {
        public string original_title { get; set; }

        public Object genres { get; set; }
        public string release_date { get; set;}

        public string overview { get; set; }

        public double popularity { get; set; }

        public int runtime { get; set; }

    }
}
