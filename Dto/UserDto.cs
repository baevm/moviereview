namespace moviereview.Dto
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class TokensDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}