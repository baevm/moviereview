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
        public string GenerateToken(User user, LoginUserDto loginUserDto);
        Task<bool> Save();
    }
}