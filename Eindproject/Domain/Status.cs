using System.ComponentModel.DataAnnotations;

namespace Eindproject.Domain
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        // Omschrijving: Als film/Serie gepland, gekeken of aan het kijken is
        public string Omschrijving { get; set; }

    }
}