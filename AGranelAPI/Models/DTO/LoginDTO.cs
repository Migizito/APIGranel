namespace AGranelAPI.Models.DTO
{
    public class LoginDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
