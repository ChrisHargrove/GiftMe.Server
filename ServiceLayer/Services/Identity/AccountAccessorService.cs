using System.Collections;
using System.Net;
using AutoMapper;
using DataLayer.Database.Schema.Identity;
using DataLayer.Repositories.Identity;
using Helpers.Exceptions;
using Microsoft.Extensions.Logging;
using Models.Admin;
using Models.Identity;
using ServiceLayer.Services.Abstract;

namespace ServiceLayer.Services.Identity;

/// <summary>
/// Service class for handling changes to the Account Accessor data.
/// <remarks>
/// Should not be used in any endpoints that are NOT and admin or owner AccountRole.
/// </remarks>
/// </summary>
/// <param name="accountRepository"></param>
/// <param name="repository"></param>
/// <param name="logger"></param>
/// <param name="mapper"></param>
public class AccountAccessorService(AccountRepository accountRepository, AccountAccessorRepository repository, ILogger<AccountAccessorService> logger, IMapper mapper): AbstractService<AccountAccessor, AccountAccessorSchemaModel, AccountAccessorRepository>(repository, logger, mapper) {

    /// <summary>
    /// Update the accounts access status specified by their email address.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <exception cref="HttpResponseException"></exception>
    public async Task<AccountAccessor> UpdateAccountStatus(string email, AccessStatus status) {
        var account = await accountRepository.ReadByEmailAsync(email);
        if (account == null) {
            throw new HttpResponseException(HttpStatusCode.NotFound, $"No account found for email: {email}");
        }
        account.Status = status;
        AccountAccessorSchemaModel? accountStatus = await Repository.ReadByEmailAsync(email);
        if (status == AccessStatus.Accepted) {
            if (accountStatus != null) {
                accountStatus.Status = status;
                await Repository.DeleteAsync(accountStatus);
            }
        }
        else {
            if (accountStatus != null) {
                accountStatus.Status = status;
                accountStatus = await Repository.UpdateAsync(accountStatus, m => m.Status);
            }
            else {
                await Repository.CreateAsync(new AccountAccessorSchemaModel() {
                    Status = status,
                    AccountId = account.Id,
                });
            }
        }

        await accountRepository.UpdateAsync(account, a => a.Status);
        return Mapper.Map<AccountAccessor>(accountStatus);
    }
}