namespace AGranelAPI.Models.DTO
{
    public class UserDTO
    {
        // Otras propiedades del usuario
        public int UsuarioID { get; set; }
        public string Username { get; set; }

        // Propiedad para almacenar información de roles
        public List<RolDTO> Roles { get; set; }
    }

}
