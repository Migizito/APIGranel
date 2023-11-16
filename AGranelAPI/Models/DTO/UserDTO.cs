namespace AGranelAPI.Models.DTO
{
    public class UserDTO
    {
        // Otras propiedades del usuario
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        // Propiedad para almacenar información de roles
        public List<RolDTO> Roles { get; set; }
    }

}
