using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Routing;

namespace AplicatieRutina.Models
{
    public class UserProfile
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string Age { get; set; }
        [Required] public string SkinType { get; set; }
        [Required] public string HairType { get; set; }

        public int? DaysOnApp { get; set; } = 0;
        public int? Streak { get; set; } = 0;

        // Legatura cu utilizatorul autentificat
        public string UserId { get; set; }  // foreign key

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }  // relație cu tabelul AspNetUsers
    }
}
