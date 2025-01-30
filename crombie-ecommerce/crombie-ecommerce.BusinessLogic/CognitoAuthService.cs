using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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
    private readonly ILogger<CognitoAuthService> _logger;

    public CognitoAuthService(IConfiguration configuration, IAmazonCognitoIdentityProvider provider,ShopContext shopContext, ILogger<CognitoAuthService> logger)
    {
        
        _shopContext = shopContext;
        _configuration = configuration;
        _shopContext = shopContext;
        _logger = logger;

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
       

        try
        {
            _logger.LogInformation("Starting the user register.");

            var clientId = _configuration["AWS:ClientId"];
            var clientSecret = _configuration["AWS:ClientSecret"];
            _logger.LogInformation("AWS Credencials are ok");

            // Validar usuario en base de datos
            _logger.LogInformation("Verifying if user is in the database...");
            var existingUser = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("user already exist: {Email}", userDto.Email);
                throw new InvalidOperationException("user registered already.");
            }

            // Crear solicitud para Cognito
            _logger.LogInformation("preparing the application...");

           

            // Cognito
            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                Username = userDto.Email,
                Password = userDto.Password,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, userDto.Email),
                UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "name", Value = userDto.Name },
                new AttributeType { Name = "email", Value = userDto.Email }
            }
            };

            _logger.LogInformation("Sendind request to cognito...");
            var cognitoResponse = await _provider.SignUpAsync(signUpRequest);

            _logger.LogInformation("Cognito answer: {StatusCode}", cognitoResponse.HttpStatusCode);

            // if cognito register is success, add the user 
            if (cognitoResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation("successfully registered.");

                // new user
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = userDto.Name,
                    Email = userDto.Email,
                    // Hash
                    Password = userDto.Password,
                    Address = userDto.Address,
                    IsVerified = false,
                    RoleId = 1,

                };

                _shopContext.Users.Add(newUser);
                await _shopContext.SaveChangesAsync();

                _logger.LogInformation("Registration successful.");

                return "Registration successful. Please confirm your email.";
            }
            return cognitoResponse.UserSub;
        }
        catch (AmazonCognitoIdentityProviderException cognitoEx)
        {
            _logger.LogError(cognitoEx, "Error from Cognito: {Message}", cognitoEx.Message);
            throw new Exception("A problem occurred while registering the user. Try again.");
        }
        
        
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
    public static string CalculateSecretHash(string clientId, string clientSecret, string username)
    {
        var message = username + clientId;
        var key = Encoding.UTF8.GetBytes(clientSecret);

        using (var hmac = new HMACSHA256(key))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash);
        }
    }

    
    
}