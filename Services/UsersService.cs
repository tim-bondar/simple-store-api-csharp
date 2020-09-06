using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleStore.DB;
using SimpleStore.Helpers;
using SimpleStore.Models;

namespace SimpleStore.Services
{
    public class UserService : IUserService
    {
        private const string TableName = "users";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly SimpleStoreContext _context;

        public UserService(IConfiguration config,
            IHttpContextAccessor contextAccessor,
            SimpleStoreContext context)
        {
            _config = config;
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var hash = GetPasswordHash(model.Password);
            var user = await _context.Users.FirstOrDefaultAsync(x => model.UserName == x.UserName && hash == x.PasswordHash);
            if (user == null || user.PasswordHash != hash)
                return null;

            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetCurrent()
        {
            var userId = _contextAccessor.ExtractUserId();

            if (userId == null)
                return null;

            return await GetById(userId.Value);
        }

        public static string GetPasswordHash(string password)
        {
            // SHA512 is disposable by inheritance.
            using var sha256 = SHA256.Create();
            // Send a sample text to hash.
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            // Get the hashed string.
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);

            var claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, user.Id.ToString() },
                { "fullName", user.FullName },
                { "role", user.IsAdmin ? "Admin" : "User" },
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),

                // Expires in 1 day
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                Claims = claims,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
