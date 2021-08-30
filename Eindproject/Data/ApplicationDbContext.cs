using Eindproject.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eindproject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // Remote Database raadplegen om data  voor alle films en series in te brengen

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Seed the database with some data so I can test my methods
            base.OnModelCreating(builder);
            // Fill in SerieAndMovie from Online APi
            // Fil in status
            // Fill in lijst 



            builder.Entity<Lijst>().HasData(
                new Lijst { LijstId = 1, });


            builder.Entity<Status>().HasData(
               new Status { StatusId = 1, StatusDescription = "Done" },
               new Status { StatusId = 2, StatusDescription = "Watching" }
           );


            builder.Entity<SerieOfFilmInLijst>().HasData(
            new SerieOfFilmInLijst
            {
                LijstId = 1,
                ApiId = 200,
                OriginalTitle = "Star Trek: The Next Generation Collection",
                FilmUrl = "/jYtNUfMbU6DBbmd4LUS19u4hF4p.jpg",
                Score = 8,
                SerieOfFilmInLijstId = 1,
                StatusId = 1,



            });




        }

        public DbSet<Lijst> Lijsts { get; set; }

        public DbSet<SerieOfFilmInLijst> SerieOfFilms { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Vriend> Vriend { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    
        public DbSet<Comment> Comments { get; set; }
    }
}
