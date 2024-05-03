namespace AGranelAPI.Models.DTO
{
    public class UserRegisterDTO
    {
        public required string Username { get; set; }
        public required string Contraseña { get; set; }
        public int Rol { get; set; }
    }
}
