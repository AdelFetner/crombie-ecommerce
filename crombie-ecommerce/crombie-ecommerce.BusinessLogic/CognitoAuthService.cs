using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Dto.Password;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography;
using System.Text;

public class CognitoAuthService
{
    private readonly IAmazonCognitoIdentityProvider _provider;
    private readonly CognitoUserPool _userPool;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly ShopContext _shopContext;
    private readonly IConfiguration _configuration;

    public CognitoAuthService(IConfiguration configuration, IAmazonCognitoIdentityProvider provider,ShopContext shopContext)
    {
        
        _shopContext = shopContext;
        _configuration = configuration;
        _shopContext = shopContext;
        

        var awsAccessKeyId = configuration["AWS:AccessKeyId"];
        var awsSecretAccessKey = configuration["AWS:SecretAccessKey"];
        var userPoolId = configuration["AWS:UserPoolId"];
        var region = configuration["AWS:Region"];

        _clientId = configuration["AWS:ClientId"] ?? "";
        _clientSecret = configuration["AWS:ClientSecret"] ?? "";

        var awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
        _provider = new AmazonCognitoIdentityProviderClient(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(region));
        _userPool = new CognitoUserPool(userPoolId, _clientId, _provider, _clientSecret);
    }

    public async Task<string> RegisterAsync(UserDto userDto)
    {
       
            var clientId = _configuration["AWS:ClientId"];
            var clientSecret = _configuration["AWS:ClientSecret"];
           
           
            var existingUser = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                
                throw new InvalidOperationException("User already exist.");
            }

            var passwordHasher = new PasswordHasher();

            string hashedPassword = passwordHasher.HashPassword(userDto.Password);

            // Cognito
            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                Username = userDto.Email,
                Password = userDto.Password,//trying sending the password without hash
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, userDto.Email),
                UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "name", Value = userDto.Name },
                new AttributeType { Name = "email", Value = userDto.Email }
            }
            };

           
            var cognitoResponse = await _provider.SignUpAsync(signUpRequest);

           
            // if cognito register is success, add the user 
            if (cognitoResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // new user
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = userDto.Name,
                    Email = userDto.Email,
                    // Hash
                    Password = userDto.Password,//trying without hash(for test)
                    Address = userDto.Address,
                    IsVerified = false,
                    Image=null,
                    RoleId = 1,

                };

                _shopContext.Users.Add(newUser);
                await _shopContext.SaveChangesAsync();

                

                return "Registration successful. Please confirm your email.";
            }
            return cognitoResponse.UserSub;
       
    }
    public async Task<CodeDeliveryDetailsType> ResendConfirmationCodeAsync(string userName)
    {
        var codeRequest = new ResendConfirmationCodeRequest
        {
            ClientId = this._clientId,
            Username = userName,
            SecretHash = CalculateSecretHash(_clientId, _clientSecret, userName)
        };

        var response = await _provider.ResendConfirmationCodeAsync(codeRequest);

        Console.WriteLine($"Method of delivery is {response.CodeDeliveryDetails.DeliveryMedium}");

        return response.CodeDeliveryDetails;
    }

    public async Task<bool> ConfirmSignupAsync(string code, string userName)
    {
        var signUpRequest = new ConfirmSignUpRequest
        {
            ClientId = this._clientId,
            ConfirmationCode = code,
            Username = userName,
            SecretHash = CalculateSecretHash(_clientId, _clientSecret, userName)
        };

        var response = await _provider.ConfirmSignUpAsync(signUpRequest);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine($"{userName} was confirmed");


            var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email == userName);
            if (user != null)
            {
                
                user.IsVerified = true;
                
                await _shopContext.SaveChangesAsync();
            }


            return true;
          
        }

        return false;
    }
    public async Task<InitiateAuthResponse> InitiateAuthAsync(string userName, string password)
    {
        var authParameters = new Dictionary<string, string>();
        authParameters.Add("USERNAME", userName);
        authParameters.Add("PASSWORD", password);
        authParameters.Add("SECRET_HASH", CalculateSecretHash(_clientId, _clientSecret, userName));

        var authRequest = new InitiateAuthRequest
        {
            ClientId = this._clientId,
            AuthParameters = authParameters,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,

        };
            var response = await _provider.InitiateAuthAsync(authRequest);
            return response;
    }

    //change password:
    public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var authResponse = await InitiateAuthAsync(changePasswordDto.Email, changePasswordDto.CurrentPassword);
            if (authResponse.AuthenticationResult == null)
            {
                throw new InvalidOperationException("Invalid credentials.");
            }
            var forgotPasswordRequest = new ForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = changePasswordDto.Email,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, changePasswordDto.Email)
            };

            var response = await _provider.ForgotPasswordAsync(forgotPasswordRequest);

            return true ;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to initiate password change. Please try again.", ex);
        }
    }

    //forgot password:
    public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var request = new ForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = forgotPasswordDto.Email,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, forgotPasswordDto.Email)
            };

            var response = await _provider.ForgotPasswordAsync(request);
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception("Error at change password proccess.", ex);
        }
    }


    //confirm the password change(with de code send to the email):
    public async Task<bool> ConfirmForgotPasswordAsync(ConfirmForgotPasswordDto confirmForgotPasswordDto)
    {
        try
        {
            var passwordHasher = new PasswordHasher();
            //hash
            string hashedPassword = passwordHasher.HashPassword(confirmForgotPasswordDto.NewPassword);

            var confirmPasswordRequest = new ConfirmForgotPasswordRequest
            {
                ClientId = _clientId,
                Username = confirmForgotPasswordDto.Email,
                ConfirmationCode = confirmForgotPasswordDto.ConfirmationCode,
                Password = hashedPassword,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, confirmForgotPasswordDto.Email)
            };

            var response = await _provider.ConfirmForgotPasswordAsync(confirmPasswordRequest);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                
                var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email == confirmForgotPasswordDto.Email);
                if (user != null)
                {
                    user.Password = hashedPassword; //hash
                    await _shopContext.SaveChangesAsync();
                }

                return true;
            }

            return false;
            throw new Exception("Failed to change password.");
        }
        catch (Exception ex)
        {
            
            throw new Exception("Error while confirming password change.", ex);
        }
    }



    public static string CalculateSecretHash(string clientId, string clientSecret, string username)
    {
        
        var message = username + clientId;
        var data = Encoding.UTF8.GetBytes(message);
        var key = Encoding.UTF8.GetBytes(clientSecret);

        using (var hmac = new HMACSHA256(key))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash);
        }
    }

    
    
}