using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace crombie_ecommerce.Services
{
    public class CognitoService
    {
        //private readonly AmazonCognitoIdentityClient _cognitoClient;
        private readonly AmazonCognitoIdentityProviderClient _cognitoClient;
        private readonly CognitoUserPool _userPool;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IConfiguration _configuration;

        public CognitoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _cognitoClient = new AmazonCognitoIdentityProviderClient();
            _userPool = new CognitoUserPool(
                _configuration["AWS:UserPoolId"],
                _configuration["AWS:AppClientId"],
                _cognitoClient
             );

        }

        public async Task SignUpAsync(string email, string password)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId= _configuration["AWS:AppClientId"],
                Username= email,
                Password= password

            };

            signUpRequest.UserAttributes.Add(new AttributeType
            {
                Name = "email",
                Value = email
            });

            await _cognitoClient.SignUpAsync(signUpRequest);
        }
        
        public async Task ResendConfirmationCodeAsync(string email)
        {
            var resendRequest = new ResendConfirmationCodeRequest
            {
                ClientId = _configuration["AWS:AppClientId"],
                Username = email,
            };
            
            await _cognitoClient.ResendConfirmationCodeAsync(resendRequest);
        }

        public async Task ConfirmSignUpAsync(string email, string confirmationCode)
        {
            var confirmRequest = new ConfirmSignUpRequest
            {
                ClientId = _configuration["AWS:AppClientId"],
                Username = email,
                ConfirmationCode = confirmationCode
            };

            await _cognitoClient.ConfirmSignUpAsync(confirmRequest);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var authRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _configuration["AWS:AppClientId"]
            };

            authRequest.AuthParameters.Add("USERNAME", email);
            authRequest.AuthParameters.Add("PASSWORD", password);

            var authResponse = await _cognitoClient.InitiateAuthAsync(authRequest);
            return authResponse.AuthenticationResult.IdToken;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var authResquest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                ClientId = _configuration["AWS:AppClientId"]
            };

            authResquest.AuthParameters.Add("REFRESH_TOKEN",refreshToken);

            var authResponse = await _cognitoClient.InitiateAuthAsync(authResquest);
            return authResponse.AuthenticationResult.IdToken;
        }

    }
}
