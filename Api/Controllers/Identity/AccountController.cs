using System.Collections;
using Api.Controllers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Auth.DTO;
using Models.Identity;
using ServiceLayer.Services.Identity;

namespace Api.Controllers.Identity;

/// <summary>
/// Controller class that defines all the endpints for interacting with direct account data.
/// </summary>
/// <param name="service"></param>
/// <param name="statusService"></param>
/// <param name="logger"></param>
[Route("account")]
public class AccountController(AccountService service, ILogger<AccountController> logger) : AbstractController<AccountService>(service, logger) {
    
    /// <summary>
    /// This will get the account related to the authorised account that made the all using the email of the account.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Account>> GetAccountAsync() {
        return Ok(await Service.GetAccountByEmailAsync(Account.Email));
        
    }

    /// <summary>
    /// Admin only endpoint that will will fetch all the accounts on the platform.
    /// <remarks>
    /// This endpoint can only be accessed by an account with the admin or owner roles. This data would be supplied in the form of the JWT token.
    /// </remarks>
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable>> GetAllAccountsAsync() {
        return (await Service.ReadAllAsync()).ToList();
    }

    /// <summary>
    /// Allows an account to update the data for an account such as:
    /// <list type="bullet">
    /// <item>Email</item>
    /// <item>Username</item>
    /// <item>Display Name</item>
    /// </list>
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<Account>> UpdateAccountAsync(Account account) {
        return await Service.UpdateAsync(account);
    }

    /// <summary>
    /// Allows an account to delete themselves from te platform permanently.
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult<Account>> DeleteAccountAsync() {
        return await Service.DeleteAccount(Account, TokenData.UserId);
    }
}