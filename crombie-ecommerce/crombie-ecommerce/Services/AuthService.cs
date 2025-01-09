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
            this._userService = userService;
            this._configuration = configuration;
            this._passwordHasher = new PasswordHasher<User>();  

            //Por que da error?
            User NewUser = new User();
            NewUser.UserId = Guid.NewGuid();
            NewUser.Password = this._passwordHasher.HashPassword(NewUser, user.Password);
            NewUser.Name = user.Name;
            NewUser.Email = user.Email;
            NewUser.CreatedDate = DateTime.Now;
            NewUser.Role = user.Role?.ToLower();
            await this._userService.CreateUser(NewUser);
            return NewUser;


            //Donde va esta parte?
            PasswordVerificationResult IsCorrectPassword = this._passwordHasher.VerifyHashedPassword(logginUser, logginUser.Password, credentials.Password);

            if (IsCorrectPassword == PasswordVerificationResult.Success) 
            {
                return this.CreateJWTAuthToken(logginUser);
            } else 
            {
                throw new Exception("Unable to authenticate");
            }



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
