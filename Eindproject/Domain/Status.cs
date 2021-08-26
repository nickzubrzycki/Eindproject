using System;
using System.ComponentModel.DataAnnotations;

namespace Eindproject.Domain
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        public bool? StatusWatch { get; set; } 


    }
}