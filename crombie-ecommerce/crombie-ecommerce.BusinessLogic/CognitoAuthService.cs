using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using crombie_ecommerce.BusinessLogic;
using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;

public class CognitoAuthService
{
    private readonly AmazonCognitoIdentityProviderClient _provider;
    private readonly CognitoUserPool _userPool;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly RoleService _roleService;
    private readonly ShopContext _shopContext;



    public CognitoAuthService(IConfiguration configuration, RoleService roleService, ShopContext shopContext)
    {
        _roleService = roleService;
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

    public async Task<string> RegisterAsync(string email, string password, string name, string address)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address))
        {
            throw new ArgumentException("Username, password, name, and address are required");
        }

        
        var existingUser = _shopContext.Users.FirstOrDefault(u => u.Email == email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        try
        {
            //cognito
            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Username = email,
                Password = password,
              
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, email)
            };

            
            var response = await _provider.SignUpAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                //create user in table  with role "user" as default
                var newUser = new User
                {
                    UserId = Guid.NewGuid(), 
                    Name = name,
                    Email = email,
                    Password = password, //hash
                    Address = address,
                    IsVerified = false, 
                    RoleId = 1, 
                    
                };

                _shopContext.Users.Add(newUser);
                await _shopContext.SaveChangesAsync();

                return "Registration successful. Please confirm your email.";
            }

            throw new Exception("Registration failed with unknown error.");
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error: {ex.Message}");

            if (ex is AmazonCognitoIdentityProviderException cognitoEx)
            {
                Console.WriteLine($"Cognito error code: {cognitoEx.ErrorCode}");
                Console.WriteLine($"Cognito error message: {cognitoEx.Message}");
            }

            throw new ApplicationException("Error during user registration", ex);
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
                _shopContext.Users.Update(user);
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