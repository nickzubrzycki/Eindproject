using Eindproject.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eindproject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // Remote Database raadplegen om data  voor alle films en series in te brengen

        

        public DbSet<Lijst> Lijsts { get; set; }

        public DbSet<SerieOfFilm> SerieOfFilms { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Vriend> Vriend { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
