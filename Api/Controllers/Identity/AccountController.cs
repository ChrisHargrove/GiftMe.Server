using Api.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using TaskExtensions  = Helpers.Extensions.TaskExtensions;

namespace Api.Controllers.Identity;

[Route("account")]
public class AccountController(ILogger<AccountController> logger) : AbstractController(logger) {
    
    [HttpGet]
    public async Task<ActionResult<Account>> GetAccountAsync() {
        return await TaskExtensions.CreateCompletedTask(new Account {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DisplayName = "Fake Account",
            Username = "Fake Account",
            Role = AccountRole.User,
        });
    }

    public async Task<ActionResult<Account>> UpdateAccountAsync(Account account) {
        return await TaskExtensions.CreateCompletedTask(account);
    } 
        
    
    [HttpDelete]
    public async Task<ActionResult> DeleteAccountAsync() {
        return await TaskExtensions.CreateCompletedTask(new OkResult());
    }
}