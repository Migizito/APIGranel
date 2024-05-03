using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AGranelAPI.Models
{
    public class UserRol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioRolID { get; set; }
        public int UsuarioID { get; set; } // Clave foránea hacia Usuario
        public int RolID { get; set; } // Clave foránea hacia Rol
        [JsonIgnore]
        [ForeignKey("UsuarioID")]
        public Usuario User { get; set; } // Propiedad de navegación hacia Usuario

        [ForeignKey("RolID")]
        public Rol Rol { get; set; } // Propiedad de navegación hacia Rol
    }
}
