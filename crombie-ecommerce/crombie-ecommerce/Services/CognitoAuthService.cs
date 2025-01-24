using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using System;
using System.Threading.Tasks;

public class CognitoAuthService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoProvider;
    private readonly IConfiguration _configuration;
    private readonly  CognitoUserPool _userPoolId;

    public CognitoAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
        _cognitoProvider = new AmazonCognitoIdentityProviderClient();
        _userPoolId = new CognitoUserPool(
            _configuration["AWS:UserPoolId"],
            _configuration["AWS:AppClientId"],
            _cognitoProvider
            );
    }

    //Register a user
    public async Task<string> RegisterAsync(string email, string password)
    {
        var userAttrs = new AttributeType
        {
            Name = "email",
            Value = email,
        };

        var userAttrsList = new List<AttributeType>();

        userAttrsList.Add(userAttrs);

        var signUpRequest = new SignUpRequest
        {
            UserAttributes = userAttrsList,
            ClientId= _configuration["AWS:AppClientId"],
            Username = email,
            Password = password,
            /*UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = email }
            }*/
        };

        var response = await _cognitoProvider.SignUpAsync(signUpRequest);

        return response.UserConfirmed ? "User registered and confirmed" : "User registered, please confirm via email.";
    }

    //Resend the confirmation code to email
    public async Task<string> ResendConfirmationCodeAsync(string email)
    {
        var resendCodeRequest = new ResendConfirmationCodeRequest
        {
            Username = email
        };

        await _cognitoProvider.ResendConfirmationCodeAsync(resendCodeRequest);
        return "Confirmation code resent successfully.";
    }

    //Confirms the user 
    public async Task<string> ConfirmSignupAsync(string code, string email)
    {
        var confirmRequest = new ConfirmSignUpRequest
        {
            
            Username = email,
            ConfirmationCode = code
        };

        await _cognitoProvider.ConfirmSignUpAsync(confirmRequest);
        return "User confirmed successfully.";
    }


    //Login the user after was confirmed 
    public async Task<AuthResponse> LoginUserAsync(string email, string password)
    {
        var authRequest = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "EMAIL", email },
                { "PASSWORD", password }
            }
        };

        var response = await _cognitoProvider.InitiateAuthAsync(authRequest);

        return new AuthResponse
        {
            IdToken = response.AuthenticationResult.IdToken,
            AccessToken = response.AuthenticationResult.AccessToken,
            RefreshToken = response.AuthenticationResult.RefreshToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn
        };
    }


    public async Task<AuthResponse> Refresh(string email, string refreshToken)
    {
        var refreshRequest = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "EMAIL", email },
                { "REFRESH_TOKEN", refreshToken }
            }
        };

        var response = await _cognitoProvider.InitiateAuthAsync(refreshRequest);

        return new AuthResponse
        {
            IdToken = response.AuthenticationResult.IdToken,
            AccessToken = response.AuthenticationResult.AccessToken,
            RefreshToken = refreshToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn
        };
    }
}

public class AuthResponse
{
    public string IdToken { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}

