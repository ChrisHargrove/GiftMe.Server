using DataLayer.Database;
using DataLayer.Database.Schema.Identity;
using DataLayer.Repositories.Abstract;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories.Identity;

/// <summary>
/// Repository for interacting with Accounts in the application.
/// </summary>
/// <param name="context"></param>
public class AccountRepository(DatabaseContext context, ILogger<AccountRepository> logger) : AbstractRepository<AccountSchemaModel>(context, logger) {

    /// <summary>
    /// Check if a username has already been used inside of the database.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<bool> CheckUserExistenceByUsernameAsync(string username) {
        return await Task.FromResult(Set
            .FirstOrDefault(e => e.Username == username) != null);
    }

    /// <summary>
    /// Check to see if an email has already been used inside of the database.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> CheckUserExistenceByEmailAsync(string email) {
        return await Task.FromResult(Set
            .FirstOrDefault(e => e.Email == email) != null);
    }

    /// <summary>
    /// Get an account entry that is corresponding to the supplied email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<AccountSchemaModel?> ReadByEmailAsync(string email) {
        return await Task.FromResult(Set
            .FirstOrDefault(e => e.Email == email));
    }
}