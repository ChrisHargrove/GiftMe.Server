using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLayer.Services.Abstract;
using ServiceLayer.Services.Firebase.Request;
using ServiceLayer.Services.Firebase.Response;

namespace ServiceLayer.Services.Firebase;

/// <summary>
/// Utility class that handles the exchange of Firebase tokens so that the server can be the authority when it comes to serving out tokens.
/// </summary>
/// <param name="logger"></param>
/// <param name="config"></param>
public class FirebaseIdentityService(ILogger<FirebaseIdentityService> logger, IConfiguration config) : AbstractService(logger) {
    /// <summary>
    /// Base url for all endpoints that go to the identity platform.
    /// </summary>
    private const string GoogleIdentityUrlBase = "https://identitytoolkit.googleapis.com/v1/";

    /// <summary>
    /// Base url for the refresh token endpoint as it it different to the rest of the identity platform.
    /// </summary>
    private const string GoogleSecureTokenUrlBase = "https://securetoken.googleapis.com/v1/token?key=";
    
    /// <summary>
    /// Internal use Http Client for all the requests that need to be sent.
    /// </summary>
    private HttpClient Client { get; } = new();
    
    /// <summary>
    /// Utility for getting the Project Api Key for firebase. This should be a web application in the Firebase Project.
    /// </summary>
    private string? ApiKey { get; } = config.GetValue<string>("FirebaseWebApiKey");

    /// <summary>
    /// Getter for the Sign Up Url from Google Identity Services
    /// </summary>
    private string SignUpUrl => $"{GoogleIdentityUrlBase}accounts:signUp?key={ApiKey}";
    /// <summary>
    /// Getter for the Sign In Url from Google Identity Services
    /// </summary>
    private string SignInUrl => $"{GoogleIdentityUrlBase}accounts:signInWithPassword?key={ApiKey}";
    /// <summary>
    /// Getter for the Update Url from Google Identity Services
    /// </summary>
    private string UpdateUrl => $"{GoogleIdentityUrlBase}accounts:update?key={ApiKey}";
    /// <summary>
    /// Getter for the OOB Url from Google Identity Services
    /// </summary>
    private string SendOobUrl => $"{GoogleIdentityUrlBase}accounts:sendOobCode?key={ApiKey}";

    /// <summary>
    /// Full Url for the refresh token endpoint with google identity platform.
    /// </summary>
    private string FirebaseRefreshTokenUrl => $"{GoogleSecureTokenUrlBase}{ApiKey}";

    /// <summary>
    /// Sends a request to the Google Identity Services to signup a new account using their email and password.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseAuthResponse> SignUpAsync(string email, string password, string? displayName) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseSignUpRequest>(SignUpUrl, new FirebaseSignUpRequest {
            Email = email,
            Password = password,
            DisplayName = displayName
        });
        return await response.Content.ReadFromJsonAsync<FirebaseAuthResponse>() ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Sends a request to the Google Identity Services to signin an account using their email and password.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseAuthResponse> SignInAsync(string email, string password) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseSignInRequest>(SignInUrl, new FirebaseSignInRequest() {
            Email = email,
            Password = password,
            ReturnSecureToken = true,
        });
        return await response.Content.ReadFromJsonAsync<FirebaseAuthResponse>() ?? throw new InvalidOperationException();
    }
    
    /// <summary>
    /// Sends a request to the Google Identity Services to update am accounts password.
    /// </summary>
    /// <param name="idToken"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseUpdateInfoResponse> ChangePasswordAsync(string idToken, string password) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseUpdateInfoRequest>(UpdateUrl, new FirebaseUpdateInfoRequest() {
            IdToken = idToken,
            Password = password,
        });
        return await response.Content.ReadFromJsonAsync<FirebaseUpdateInfoResponse>() ?? throw new InvalidOperationException();
    }
    
    /// <summary>
    /// Sends a request to the Google Identity Services tp update an accounts email. 
    /// </summary>
    /// <param name="idToken"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseUpdateInfoResponse> ChangeEmailAsync(string idToken, string email) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseUpdateInfoRequest>(UpdateUrl, new FirebaseUpdateInfoRequest() {
            IdToken = idToken,
            Email = email,
        });
        return await response.Content.ReadFromJsonAsync<FirebaseUpdateInfoResponse>() ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Sends a request to the Google Identity Services to start an OOB (Out Of Band) operation to reset an accounts password.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseSendOobCodeResponse> ResetPasswordAsync(string email) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseSendOobCodeRequest>(SendOobUrl, new FirebaseSendOobCodeRequest() {
            RequestType = FirebaseOobType.PASSWORD_RESET,
            Email = email,
        });
        return await response.Content.ReadFromJsonAsync<FirebaseSendOobCodeResponse>() ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Sends a request to the Google Identity Services to start an OOB (Out Of Band) operation to verify an accounts email.
    /// </summary>
    /// <param name="idToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseSendOobCodeResponse> VerifyEmailAsync(string idToken) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseSendOobCodeRequest>(SendOobUrl, new FirebaseSendOobCodeRequest() {
            RequestType = FirebaseOobType.VERIFY_EMAIL,
            IdToken = idToken,
        });
        return await response.Content.ReadFromJsonAsync<FirebaseSendOobCodeResponse>() ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Sends a request to the Google Identity Services to start refresh and accounts authorization token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FirebaseRefreshTokenResponse> RefreshTokenAsync(string refreshToken) {
        HttpResponseMessage response = await Client.PostAsJsonAsync<FirebaseRefreshTokenRequest>(FirebaseRefreshTokenUrl, new FirebaseRefreshTokenRequest() {
            RefreshToken = refreshToken
        });
        return await response.Content.ReadFromJsonAsync<FirebaseRefreshTokenResponse>() ?? throw new InvalidOperationException();
    }
}