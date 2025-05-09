using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AplicatieRutina.Models
{
    public class Routine
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string RoutineType { get; set; } = string.Empty; // Morning, Evening, etc.

        public string? Products { get; set; } // Comma-separated list for simplification

        [Required]
        public TimeSpan ScheduledTime { get; set; }

        public IdentityUser? User { get; set; }
    }
}