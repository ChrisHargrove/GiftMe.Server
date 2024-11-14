using Api.Constants;
using Api.Controllers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Auth.DTO;
using ServiceLayer.Services.Identity;

namespace Api.Controllers.Identity;

/// <summary>
/// Auth controller that handles all the endpoints for authentication such as:
/// <list type="bullet">
///  <item><description>SignUp</description></item>
///  <item><description>SignIn</description></item>
///  <item><description>SignOut</description></item>
///  <item><description>DeleteAccount</description></item>
/// </list>
/// </summary>
[Route("auth")]
public class AuthController(ILogger<AuthController> logger, AuthService authService)
    : AbstractController(logger) {

    private AuthService AuthService { get; } = authService;

    /// <summary>
    /// SignUp a new user
    /// </summary>
    /// <param name="signUp">Model containing all data required to signup a new user.</param>
    /// <returns>
    /// Returns a string that is the users authentication token.
    /// </returns>
    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> SignUpAsync(SignUp signUp) {
        return await Validator
            .Validate(() => signUp.Email)
            .Validate(() => signUp.Password)
            .Validate(() => signUp.Username, AccountConstants.MinNameLength, AccountConstants.MaxNameLength)
            .ValidateNullable(() => signUp.DisplayName, AccountConstants.MinNameLength, AccountConstants.MaxNameLength)
            .OnSuccess(async () => Ok(await AuthService.SignUpAsync(signUp)))
            .CheckAsync();
    }


    /// <summary>
    /// SignIn a user
    /// </summary>
    /// <param name="signIn">Model containing all data required to signin a user.</param>
    /// <returns>
    /// Returns a string that is the users authentication token.
    /// </returns>
    [HttpPost("signin")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> SignInAsync(SignIn signIn)
        => await Validator
            .Exists(() => signIn)
            .Exists(() => signIn.Email)
            .Exists(() => signIn.Password)
            .OnSuccess(async () => Ok(await AuthService.SignInAsync(signIn)))
            .CheckAsync();

    /// <summary>
    /// Signs Out a user.
    /// <remarks>
    /// Internally this will revoke all the users tokens so none of the refresh tokens are valid.
    /// </remarks>
    /// </summary>
    /// <returns></returns>
    [HttpPost("signout")]
    public async Task<ActionResult> SignOutAsync() {
        await AuthService.SignOutAsync(TokenData.UserId, TokenData.Identities.Email);
        return Ok();
    }
}