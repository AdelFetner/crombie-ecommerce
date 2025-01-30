using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly CognitoAuthService _cognitoAuthService;

    public AuthController(CognitoAuthService cognitoAuthService)
    {
        _cognitoAuthService = cognitoAuthService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Address))
        {
            return BadRequest(new { Error = "All fields (Email, Password, Name, Address) are required." });
        }

        try
        {
            var result = await _cognitoAuthService.RegisterAsync(
                email:request.Email,
                name:request.Name,
                address:request.Address,
                password: request.Password
               
            );

            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("resend-code")]
    public async Task<IActionResult> ResendCode([FromBody] ResendRequest request)
    {
        try
        {
            var result = await _cognitoAuthService.ResendConfirmationCodeAsync(request.Email);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmRequest request)
    {
        try
        {
            var result = await _cognitoAuthService.ConfirmSignupAsync(request.Code, request.Email);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _cognitoAuthService.InitiateAuthAsync(request.Email, request.Password);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
}

public class ConfirmRequest
{
    public string Code { get; set; }
    public string Email { get; set; }
}

public class ResendRequest
{
    public string Email { get; set; }
}

public class RefreshRequest
{
    public string userName { get; set; }
    public string refreshToken { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}