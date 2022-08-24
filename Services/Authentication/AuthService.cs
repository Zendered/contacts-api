using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ContactsApi.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly ContactsContext context;
        private readonly IConfiguration configuration;

        public AuthService(ContactsContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<(bool res, string msg)> LoginAsync(string username, string password)
        {
            var userExists = await UserExistsAsync(username);
            var user = await context.Users.FirstOrDefaultAsync(context => context.Username == username);

            if (user is null) return (false, "User or password Incorrect");

            var passwd = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
            if (!passwd) return (false, "User or password Incorrect");

            return (true, CreateToken(user));
        }

        public async Task<(bool res, string msg)> RegisterAsync(User user, string password)
        {
            var userExists = await UserExistsAsync(user.Username);
            var msgRes = userExists ? "User already exists" : "User Created";
            if (userExists) return (userExists, msgRes);

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return (userExists, msgRes);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            var user = await context.Users.AnyAsync(user => user.Username == username); ;
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computerHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return $"bearer {tokenHandler.WriteToken(token)}";
        }
    }
}
