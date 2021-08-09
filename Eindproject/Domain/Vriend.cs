using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Domain
{
    /// <summary>
    /// Een Vriend kan toegevoegd worden aan de gebruiker. Een gebruiker heeft een lijst van 
    /// vrienden die hij kan zien als ook de  Films/Series dat ze met elkaar leuk vinden
    /// De Gebruiker kan een profiel van een vriend zien
    /// </summary>
    public class Vriend
    {
        public int GebruikerId { get; set; }

        public string Voornaam { get; set; }

        public string Achternaam { get; set; }

        public string Foto { get; set; }

        public Lijst Lijst { get; set; } 


        public string Fullname() 
        {
            return Voornaam + " " + Achternaam;
        }
    }
}
