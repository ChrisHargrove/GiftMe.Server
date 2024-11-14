using Api.Controllers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Admin;
using ServiceLayer.Services.Identity;

namespace Api.Controllers.Admin;

/// <summary>
/// Controller class for handling all of the access to the admin panel for allowing access to the accounts. This feature is a requirement to stop
/// excessive users from accessing the service without permission. This way we can selective allow accounts access.
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[Route("admin/access")]
[Authorize("Admin")]
public class AccountAccessController(AccountAccessorService service, ILogger logger) : AbstractController<AccountAccessorService>(service, logger) {
    
    /// <summary>
    /// Get all the current account accessors that are not accepted.
    /// <para>
    /// These requests can be in any of the following states:
    /// <list type="bullet">
    /// <item>Pending</item>
    /// <item>Denied</item>
    /// <item>Blocked</item>
    /// <item>Banned</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountAccessor>>> GetAll() {
        return (await Service.ReadAllAsync()).ToList();
    }

    /// <summary>
    /// Set a new state for an existing accessor. If the status is set to accepted then the corresponding account
    /// will be granted access to the service, the accessor will then also be removed from the database as it is no
    /// longer required.
    /// </summary>
    /// <param name="accountAccessor"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<AccountAccessor>> UpdateStatus([FromBody]AccountAccessor accountAccessor) {
        return await Service.UpdateAccountStatus(accountAccessor.Account.Email, accountAccessor.Status);
    }
}