using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AGranelAPI.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }
        [Column("Username")]
        public string Username { get; set; } = string.Empty;
        [Column("Contraseña")]
        public string Contraseña { get; set; } = string.Empty;
        [JsonIgnore]
        public List<UserRol> Roles { get; set; } = new List<UserRol>();
    }
}
