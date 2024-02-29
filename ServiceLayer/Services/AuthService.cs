using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Auth.DTO;
using ServiceLayer.Services.Abstract;
using TaskExtensions = Helpers.Extensions.TaskExtensions;

namespace ServiceLayer.Services;

/// <summary>
/// Service for handling all database interactions related to authentication.
/// </summary>
/// <param name="logger"></param>
public class AuthService(ILogger<AuthService> logger) : AbstractGiftMeService(logger) {

    public async Task<TokenResponse> SignUpAsync(SignUp signUp) {
        //TODO: need to check if user exists in DB based on email.
        //TODO: need to check firebase to see if user already exists.
        
        //TODO: if user doesnt exist on either then create a new firebase user and get back the JWT
        //TODO: create user in database and seed initial data.
        //TODO: store users refresh token.
        
        return await TaskExtensions.CreateCompletedTask(new TokenResponse {Token = "Some-Super-Secret-JWT-Token"});
    }

    public async Task<TokenResponse> SignInAsync(SignIn signIn) {
        //TODO: try login to firebase user, if fail check if its because token is out of date
        //TODO: if token is out of date fetch refresh token from db
        //TODO: attempt to refresh the token before sending it back.
        
        return await TaskExtensions.CreateCompletedTask(new TokenResponse {Token = "Some-Super-Secret-JWT-Token"});
    }

    public async Task SignOutAsync() {
        //TODO: delete all tokens for the given user in the db so they are forces to do a fresh signin.

        await Task.CompletedTask;
    }
    
}