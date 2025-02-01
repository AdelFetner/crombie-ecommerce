using Amazon.CognitoIdentityProvider.Model;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Dto.Password;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

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
    public async Task<IActionResult> Register(UserDto userDto)
    {
        try
        {
            var user = await _cognitoAuthService.RegisterAsync(userDto);
            return Ok(new { User = user });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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
            var isConfirmed = await _cognitoAuthService.ConfirmSignupAsync(
                code: request.Code,
                userName: request.Email
            );

            if (isConfirmed)
            {
                return Ok(new { Message = "Email confirmed successfully." });
            }
            return BadRequest(new { Error = "Failed to confirm email." });
        }
        catch (Exception ex)
        {
            
            return BadRequest(new { Error = ex.Message });
        }
    
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _cognitoAuthService.InitiateAuthAsync(loginDto.Email, loginDto.Password);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("change-password")]
    [Authorize] //User must be logged in, to change the password this way.
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        try
        {
            var result = await _cognitoAuthService.ChangePassword(changePasswordDto);
            

            if (result)
            {
                return Ok(new { message = "A code has been sent to your email, please check it and confirm the change." });
            }

            return BadRequest(new { message = "Password could not be changed." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}, InnerException: {ex.InnerException}");
            throw new Exception($"Error changing password: {ex.Message}", ex);
        }
    }

    [HttpPost("forgot")]
    //User doesn't have to be logged in, to change password this way.
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var response = await _cognitoAuthService.ForgotPassword(forgotPasswordDto);
            return Ok(new
            {
                message = "A code has been sent to your email, please check it and confirm the change",
                deliveryMethod = response.CodeDeliveryDetails.DeliveryMedium,
                destination = response.CodeDeliveryDetails.Destination
            });
        }
        catch (Exception ex)
        {
           
            return BadRequest(new { message = "Error trying the password recovery process" });
        }
    }

    [HttpPost("forgot/confirm")]
    public async Task<IActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordDto confirmForgotPasswordDto)
    {
        try
        {
            var result = await _cognitoAuthService.ConfirmForgotPasswordAsync(confirmForgotPasswordDto);
            

            if (result)
            {
                return Ok(new { message = "Password successfully changed" });
            }

            return BadRequest(new { message = "Error when confirming password change" });
        }
        catch (Exception ex)
        {
            Console.WriteLine("error");
            return BadRequest(new { message = "Error when confirming password change" });
        }
    }

}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
   
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