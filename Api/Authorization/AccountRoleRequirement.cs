using Microsoft.AspNetCore.Authorization;
using Models.Identity;

namespace Api.Authorization;

/// <summary>
/// This class represents a required account role for accessing an endpoint, it allows for an array of roles if many role types are allowed to be combined.
/// </summary>
/// <param name="accountRoles"></param>
public class AccountRoleRequirement(params AccountRole[] accountRoles) : IAuthorizationRequirement {
    public AccountRole[] Roles { get; private set; } = accountRoles;
}