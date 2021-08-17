﻿using Eindproject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Models
{
    public class MovieViewModel
    {
        public int id { get; set; }
        public string original_title { get; set; }

        public Object genres { get; set; }
        public string release_date { get; set;}

        public string overview { get; set; }

        public double popularity { get; set; }

        public int runtime { get; set; }
        public string poster_path { get; set; }

    }
}
