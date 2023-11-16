using System.ComponentModel.DataAnnotations;

namespace AGranelAPI.Models
{
    public class Rol
    {
        [Key]
        public int RoleID { get; set; }
        public string NombreDelRol { get; set; }
        public List<UserRol> Usuarios { get; set; }
    }
}
