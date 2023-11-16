using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AGranelAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Column("Nombre")]
        public string Username { get; set; } = string.Empty;
        [Column("CorreoElectronico")]
        public string Email { get; set; } = string.Empty;
        [Column("Contraseña")]
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public List<UserRol> Roles { get; set; } = new List<UserRol>();
        public List<Venta> Ventas { get; set; } = new List<Venta> { };
    }
}
