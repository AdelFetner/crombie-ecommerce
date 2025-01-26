using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Security.Cryptography;
using System.Text;

public class CognitoAuthService
{
    private readonly AmazonCognitoIdentityProviderClient _provider;
    private readonly CognitoUserPool _userPool;
    private readonly string _clientId;
    private readonly string _clientSecret;



    public CognitoAuthService(IConfiguration configuration)
    {
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

    public async Task<string> RegisterAsync(string username, string password)
    {
        try
        {
            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Username = username,
                Password = password,
                SecretHash = CalculateSecretHash(_clientId, _clientSecret, username)
            };

            var response = await _provider.SignUpAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return "Registration successful. Please confirm your email.";
            }

            throw new Exception("Registration failed with unknown error.");
        }
        catch (Exception ex)
        {
            // Log and rethrow exception
            Console.WriteLine($"Error during registration: {ex.Message}");
            throw;
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