using moviereview.Dto;
using moviereview.Models;

namespace moviereview.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        Task<User> GetUser(string username);
        Task<User> GetUserByEmail(string email);
        Task<bool> CreateUser(CreateUserDto userDto);
        Task<User> UpdateUser(User user, string password);
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken(User user);
        public bool ValidateToken(string refreshToken);
        public bool VerifyPassword(string HashPassword, string password);
        Task<bool> Save();
    }
}