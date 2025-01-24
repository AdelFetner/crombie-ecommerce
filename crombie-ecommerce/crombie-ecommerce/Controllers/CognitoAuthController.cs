using Azure.Core;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class CognitoAuthController : ControllerBase
{
    private readonly CognitoAuthService _authService;

    public CognitoAuthController(CognitoAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var userId = await _authService.RegisterAsync(request.Email, request.Password);
            return Ok(new { UserId= userId, Message ="Registered User. Please verify your email." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("resend-code")]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest request)
    {
        try
        {
            await _authService.ResendConfirmationCodeAsync(request.Email);
            return Ok(new { Message = "Code resended to your email." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmRequest request)
    {
        try
        {
            var result = await _authService.ConfirmSignupAsync(request.Code, request.Email);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginUserAsync(request.Email, request.Password);
            return Ok(new {AccessToken = token});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var response = await _authService.Refresh(request.Username, request.RefreshToken);
            return Ok(response);
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
    public string Password { get; set; }
}

public class ResendCodeRequest
{
    public string Email { get; set; }
}

public class ConfirmRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}