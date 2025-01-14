using crombie_ecommerce.Models;
using crombie_ecommerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthsController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data.");
            }

            var newUser = await _authService.Register(user);
            return CreatedAtAction(nameof(Register), new { id = newUser.UserId }, newUser);



            /* try
             {
                 var newUser = await _authService.Register(user);
                 return Ok(newUser);

             }
             catch (Exception ex)
             {
                 return BadRequest(ex.Message);*/
        }



        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginCredentials credentials)
        {
            if (credentials == null || string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest("Invalid login credentials.");
            }

            try
            {
                var token = await _authService.Login(credentials);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }



            /*try
            {
                var token = await _authService.Login(credentials);
                return Ok(new { token });

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);*/
        }

    }
}
