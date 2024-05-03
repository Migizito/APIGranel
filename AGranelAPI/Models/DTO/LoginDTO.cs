namespace AGranelAPI.Models.DTO
{
    public class LoginDTO
    {
        public int UsuarioID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
