namespace AGranelAPI.Models.DTO
{
    public class UserRegisterDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int Rol { get; set; }
    }
}
