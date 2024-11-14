using Api.Authorization.Helpers;
using Helpers.Http;
using Microsoft.AspNetCore.Authorization;
using Models.Auth.DTO;
using ServiceLayer.Services.Identity;

namespace Api.Authorization;

/// <summary>
/// Class that handles the requirement of having an up to date IdToken for authorisation.
/// If ths handler finds that the IdToken is invalid it will deny access to protected Endpoints.
/// If the token is valid but expired, it will attempt to refresh the token and if unsuccessful will
/// deny access to the endpoint. Otherwise it will allow access to the endpoint and place the updated IdToken into the
/// headers of the request response.
/// </summary>
/// <param name="authService"></param>
/// <param name="httpContextAccessor"></param>
/// <param name="config"></param>
/// <param name="logger"></param>
public class RefreshTokenHandler(AuthService authService, IHttpContextAccessor httpContextAccessor, IConfiguration config, ILogger<RefreshTokenHandler> logger): AuthorizationHandler<NonExpiredIdTokenRequirement> {
    /// <summary>
    /// This method checks to see if the context can be authorised. First it tries to get the claims after that it tries to ensure
    /// that the IdToken is non-expired.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NonExpiredIdTokenRequirement requirement) {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) {
            context.Fail();
            return;
        }

        ClaimsTokenInfo tokenInfo;
        try {
            tokenInfo = ClaimUtils.GetFirebaseUserInfo(httpContext.User);
        }
        catch (Exception e) {
            logger.LogError($"Message: {e.Message}, StackTrace: {e.StackTrace}");
            context.Fail();
            return;
        }

        string? firebaseAuthority = config.GetSection("Jwt:Firebase:ValidIssuer").Get<string>();
        if (firebaseAuthority == null || tokenInfo.AuthorityUrl != firebaseAuthority)
        {
            logger.LogError("Authority was invalid");
            context.Fail();
            return;
        }

        await RefreshUserToken(context, tokenInfo, httpContext);
        context.Succeed(requirement);
    }

    /// <summary>
    /// This method is were the magic happens. First we compare the unix timestamp of now to the token expire time.
    /// If it has expired, attempt to refresh. if it fails throw an exception.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="tokenInfo"></param>
    /// <param name="httpContext"></param>
    private async Task RefreshUserToken(AuthorizationHandlerContext context, ClaimsTokenInfo tokenInfo, HttpContext httpContext) {
        if (!(await authService.HasRefreshTokenAsync(tokenInfo.Identities.Email))) {
            context.Fail();
            return;
        }
        
        long nowUnix = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        if (nowUnix >= tokenInfo.TokenExpires) {
            try
            {
                //AuthService will attempt to refresh the IdToken using the stored RefreshToken in the DB for this user.
                TokenResponse tokenResponse = await authService.RefreshTokenAsync(tokenInfo.Identities.Email);
                httpContext.Request.Headers.Append(RequestHeaders.AuthorizationKey, $"Bearer {tokenResponse.Token}");
                logger.LogInformation("Successfully refreshed token");

                //Add the new IdToken into the headers for the client to extract and store.
                httpContext.Response.Headers.Append(ResponseHeaders.TokenRefresh, tokenResponse.Token);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to refresh token.\nMessage {ex.Message}\nStackTrace: {ex.StackTrace}");

                httpContext.Response.Headers.Append(ResponseHeaders.TokenRefresh, "Failure");
                context.Fail();
            }
        }
    }
}