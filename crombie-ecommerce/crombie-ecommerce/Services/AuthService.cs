using crombie_ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace crombie_ecommerce.Services
{
    public class AuthService
    {

        private readonly PasswordHasher<User> _passwordHasher;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        //  user registration
        public async Task<User> Register(User user)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                CreatedDate = DateTime.Now,
                Role = user.Role?.ToLower()
            };

            // Hash password
            newUser.Password = _passwordHasher.HashPassword(newUser, user.Password);

            // Save user 
            await _userService.PostUser(newUser);
            return newUser;
        }

        // Add this method for login
        public async Task<string> Login(LoginCredentials credentials)
        {
            // Assume you have a method to get user by email
            var user = await _userService.GetUserByEmail(credentials.Email);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                credentials.Password
            );

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                return CreateJWTAuthToken(user);
            }

            throw new Exception("Invalid credentials");
        }

        public string CreateJWTAuthToken(User user)
        {

            string secretKey = this._configuration["JWT:SECRET"] ?? "";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (secretKey == null)
            {
                throw new Exception("Unable to create token");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Name.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                    new Claim("Roles", user.Role)
                    ]),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
                Issuer = this._configuration["JWT:ISSUER"],
                Audience = this._configuration["JWT:AUDIENCE"]

            };

            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}



