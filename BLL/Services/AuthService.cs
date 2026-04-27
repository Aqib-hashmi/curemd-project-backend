using Microsoft.IdentityModel.Tokens;
using StackOverFlowReplica.DAL.Repositories;
using StackOverFlowReplica.Models.payloadModel;
using StackOverFlowReplica.StackOverFlowReplica.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace StackOverFlowReplica.BLL.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly UserRepository _repo;

        public AuthService(UserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public int Register(Register user)
        {

            return _repo.RegisterUser(user);
        }

        public User? Login(string email, string password, out string? errorMessage)
        {
            errorMessage = null;

            var user = _repo.GetUserByEmail(email);

            if (user == null)
            {
                errorMessage = "Email not registered. Please register first.";
                return null;
            }

            if (user.Password != password)
            {
                errorMessage = "Incorrect password. Try again.";
                return null;
            }

            // Password matches
            return user;
        }

        public User? getProfile(int userId)
        {
            return _repo.GetUserById(userId);
        }


        public List<User> GetAllUsers()
        {
            return _repo.GetAllUsers();
        }


        public string GenerateToken(User user)
        {

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var keyString = _config["JWT:key"];

            if (string.IsNullOrEmpty(keyString))
            {
                throw new Exception("JWT Key is missing in appsettings.json");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User? GetUserById(int userId)
        {
            return _repo.GetUserById(userId);
        }

        // BLL/Services/AuthService.cs
        public bool UpdateUser(User user)
        {
            // Optional: check if user exists first
            var existingUser = _repo.GetUserById(user.UserId);
            if (existingUser == null)
                return false;

            return _repo.UpdateUser(user);
        }


        // BLL/Services/AuthService.cs
        public bool ChangeUserStatus(int adminId, int targetUserId, bool isActive)
        {
            bool success = _repo.ChangeUserStatus(adminId, targetUserId, isActive);
            if (!success)
                throw new Exception("Either target user is admin or invalid IDs.");
            return true;
        }
    }
}