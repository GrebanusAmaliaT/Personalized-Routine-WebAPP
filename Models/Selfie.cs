using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AplicatieRutina.Models
{
    public class Selfie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; } = "";

        public DateTime UploadDate { get; set; }

        [Required]
        public string UserId { get; set; } = "";

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }

}
