using AutoMapper;
using DataLayer.Database.Schema.Identity;
using DataLayer.Repositories.Identity;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;
using Models.Identity;
using ServiceLayer.Services.Abstract;

namespace ServiceLayer.Services.Identity;

/// <summary>
/// This service allows access to everything related to accounts.
/// </summary>
/// <param name="repository"></param>
/// <param name="logger"></param>
/// <param name="mapper"></param>
public class AccountService(AccountRepository repository, FirebaseAuth firebaseAuth, ILogger<AccountService> logger, IMapper mapper) : AbstractService<Account, AccountSchemaModel, AccountRepository>(repository, logger, mapper) {
    /// <summary>
    /// Get an account by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<Account> GetAccountByEmailAsync(string email) {
        AccountSchemaModel? accountSchema = await Repository.ReadByEmailAsync(email);
        if (accountSchema == null) {
            throw new NullReferenceException($"Account for email: {email} not found!");
        }
        return Mapper.Map<Account>(accountSchema);
    }

    /// <summary>
    /// Delete an account from the system.
    /// <remarks>
    /// This method should only be called by the account owner.
    /// </remarks>
    /// </summary>
    /// <param name="account"></param>
    /// <param name="uid"></param>
    /// <returns></returns>
    public async Task<Account> DeleteAccount(Account account, string uid) {
        await firebaseAuth.RevokeRefreshTokensAsync(uid);
        await firebaseAuth.DeleteUserAsync(uid);
        return await DeleteAsync(account);
    }

    /// <summary>
    /// Updates an account data.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task<Account> UpdateAsync(Account account) {
        return await base.UpdateAsync(account, 
            u => u.DisplayName,
            u => u.Email,
            u => u.Username
        );
    }
}