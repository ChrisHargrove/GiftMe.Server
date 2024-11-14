using Api.Authorization.Helpers;
using Microsoft.AspNetCore.Authorization;
using Models.Identity;
using ServiceLayer.Services.Identity;

namespace Api.Authorization;

/// <summary>
/// Class that represents the handling of authorisation to ensure a minimum account role required to access an endpoint.
/// </summary>
/// <param name="accountService"></param>
/// <param name="logger"></param>
public class AccountRoleHandler(AccountService accountService, ILogger<AccountRoleHandler> logger): AuthorizationHandler<AccountRoleRequirement> {
    
    /// <summary>
    /// Method that handles the checking of the incoming requests user context for the required role for that endpoint.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountRoleRequirement requirement) {
        ClaimsTokenInfo tokenInfo;
        try {
            tokenInfo = ClaimUtils.GetFirebaseUserInfo(context.User);
        }
        catch (Exception e) {
            logger.LogError($"Message: {e.Message}, StackTrace: {e.StackTrace}");
            context.Fail();
            return;
        }

        Account account;
        try {
            account = await accountService.GetAccountByEmailAsync(tokenInfo.Identities.Email);
        }
        catch (Exception e) {
            logger.LogError($"Message: {e.Message}, StackTrace: {e.StackTrace}");
            context.Fail();
            return;
        }
        
        if(!requirement.Roles.Contains(account.Role))
        {
            logger.LogError($"Account with email: ${account.Email} tried to access a protected endpoint!");
            context.Fail();
            return;
        }
        
        context.Succeed(requirement);
    }
}