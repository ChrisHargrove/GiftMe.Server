using System.Net;
using AutoMapper;
using DataLayer.Database.Schema.Identity;
using DataLayer.Repositories.Identity;
using FirebaseAdmin.Auth;
using Helpers.Exceptions;
using Microsoft.Extensions.Logging;
using Models.Auth.DTO;
using Models.Identity;
using ServiceLayer.Services.Abstract;
using ServiceLayer.Services.Firebase;
using ServiceLayer.Services.Firebase.Response;

namespace ServiceLayer.Services.Identity;

/// <summary>
/// Service for handling all database interactions related to authentication.
/// </summary>
/// <param name="logger"></param>
public class AuthService(FirebaseAuth firebaseAuth, FirebaseIdentityService identityService, AccountRepository accountRepo, ILogger<AuthService> logger, IMapper mapper) 
    : AbstractService<Account, AccountSchemaModel, AccountRepository>(accountRepo, logger, mapper) {

    /// <summary>
    /// Signup a new account from a signup request.
    /// </summary>
    /// <param name="signUp"></param>
    /// <returns></returns>
    /// <exception cref="HttpResponseException"></exception>
    public async Task<TokenResponse> SignUpAsync(SignUp signUp) {
        if (string.IsNullOrWhiteSpace(signUp.Email) || string.IsNullOrWhiteSpace(signUp.Password)) {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid Signup Payload");
        }
        
        if (await Repository.CheckUserExistenceByEmailAsync(signUp.Email)) {
            throw new HttpResponseException(HttpStatusCode.Conflict, "Failed to signup Email already in use.");
        }

        try {
            await firebaseAuth.GetUserByEmailAsync(signUp.Email);
            throw new HttpResponseException(HttpStatusCode.Conflict, "Failed to signup Email already in use.");
        }
        catch (FirebaseAuthException) {
            FirebaseAuthResponse authResponse = await identityService.SignUpAsync(signUp.Email, signUp.Password, signUp.DisplayName);
            await Repository.CreateAsync(new AccountSchemaModel() {
                Email = signUp.Email,
                Username = signUp.Username,
                DisplayName = signUp.DisplayName,
                Role = AccountRole.User,
                RefreshToken = authResponse.RefreshToken,
                DateOfBirth = signUp.DateOfBirth,
            });
            
            //TODO: create accountCreationRequest model for admins to deal with.

            return new TokenResponse { Token = authResponse.IdToken };
        }
    }

    /// <summary>
    /// Signin an account using a signin request.
    /// </summary>
    /// <param name="signIn"></param>
    /// <returns></returns>
    /// <exception cref="HttpResponseException"></exception>
    public async Task<TokenResponse> SignInAsync(SignIn signIn) {
        if (string.IsNullOrWhiteSpace(signIn.Email) || string.IsNullOrWhiteSpace(signIn.Password)) {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid SignIn Payload");
        }
        
        if (!(await Repository.CheckUserExistenceByEmailAsync(signIn.Email))) {
            throw new HttpResponseException(HttpStatusCode.NotFound, "User does not exist.");
        }

        FirebaseAuthResponse authResponse = await identityService.SignInAsync(signIn.Email, signIn.Password);
        AccountSchemaModel? account = await Repository.ReadByEmailAsync(signIn.Email);
        if (account == null) {
            throw new HttpResponseException($"Entity does not exist for email: {signIn.Email}");
        }
        account.RefreshToken = authResponse.RefreshToken;
        await Repository.UpdateAsync(account);
        
        return new TokenResponse {Token = authResponse.IdToken};
    }

    /// <summary>
    /// Sign out an account using their email and firebase uid.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="email"></param>
    /// <exception cref="HttpResponseException"></exception>
    public async Task SignOutAsync(string uid, string email) {
        await firebaseAuth.RevokeRefreshTokensAsync(uid);
        AccountSchemaModel? account = await Repository.ReadByEmailAsync(email);
        if (account == null) {
            throw new HttpResponseException($"Entity does not exist for email: {email}");
        }
        account.RefreshToken = null;
        await Repository.UpdateAsync(account);
    }

    /// <summary>
    /// Utility method to check if an account, specified by their email, has a currently stored refresh token.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> HasRefreshTokenAsync(string email) {
        AccountSchemaModel? account = await Repository.ReadByEmailAsync(email);
        return account?.RefreshToken != null;
    }

    /// <summary>
    /// Refreshes an accounts, specified by email, authorization token.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="HttpResponseException"></exception>
    public async Task<TokenResponse> RefreshTokenAsync(string email) {
        AccountSchemaModel? account = await Repository.ReadByEmailAsync(email);
        if (account?.RefreshToken == null) {
            throw new HttpResponseException($"Couldn't refresh token for email: {email} as the Entity doesn't exist!");
        }
        
        FirebaseRefreshTokenResponse response = await identityService.RefreshTokenAsync(account.RefreshToken);
        account.RefreshToken = response.RefreshToken;
        await Repository.UpdateAsync(account);

        return new TokenResponse { Token = response.IdToken };
    }
    
}