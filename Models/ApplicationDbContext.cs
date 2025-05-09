using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AplicatieRutina.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Selfie> Selfies { get; set; }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        public DbSet<Routine> Routines { get; set; }
    }
}
