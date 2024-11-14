using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

/// <summary>
/// This class represents the requirement of having a non-expired IdToken when using services 
/// </summary>
public class NonExpiredIdTokenRequirement : IAuthorizationRequirement {
}