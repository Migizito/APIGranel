using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AGranelAPI.Models
{
    public class UserRol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRolesID { get; set; }
        public int UserID { get; set; } // Clave foránea hacia Usuario
        public int RoleID { get; set; } // Clave foránea hacia Rol
        [JsonIgnore]
        [ForeignKey("UserID")]
        public User User { get; set; } // Propiedad de navegación hacia Usuario

        [ForeignKey("RoleID")]
        public Rol Rol { get; set; } // Propiedad de navegación hacia Rol
    }
}
