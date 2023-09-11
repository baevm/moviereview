using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using moviereview.Data;
using moviereview.Dto;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration _configuration;

        public UserRepository(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this._configuration = configuration;
        }

        public async Task<bool> CreateUser(CreateUserDto userDto)
        {
            var isExistUsername = await GetUser(userDto.Username);
            if (isExistUsername != null)
            {
                return false;
            }

            var isExistEmail = await GetUserByEmail(userDto.Email);
            if (isExistEmail != null)
            {
                return false;
            }
            string passwordHash = HashPassword(userDto.Password);

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                IsActivated = false,
                AvatarURL = ""
            };

            await context.Users.AddAsync(user);
            await Save();

            return true;
        }


        public async Task<User> GetUser(int id)
        {
            return await context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }

        public Task<User> UpdateUser(User user, string password)
        {
            throw new NotImplementedException();
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>{
                new ("username", user.Username),
                new ("id", user.Id.ToString())
            };

            var TokenLifetime = TimeSpan.FromHours(2);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TokenLifetime),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>{
                new ("username", user.Username),
                new ("id", user.Id.ToString())
            };

            var TokenLifetime = TimeSpan.FromDays(14);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TokenLifetime),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerifyPassword(string HashPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, HashPassword);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool ValidateToken(string refreshToken)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            TokenValidationParameters validationParams = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value!)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParams, out SecurityToken validatedToken);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}