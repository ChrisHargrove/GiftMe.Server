using System.Security.Claims;
using Newtonsoft.Json;

namespace Api.Authorization.Helpers;

/// <summary>
/// This class is just here to contain the utility methods related to extracting data from the JWT during
/// authorization.
/// </summary>
public static class ClaimUtils {
    /// <summary>
    /// This method attempts to deserialize the firebase claim information from a JWT.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">In the event that deserialization of the claim fails.</exception>
    public static ClaimsTokenInfo GetFirebaseUserInfo(ClaimsPrincipal user)
    {
        Claim? firebaseClaim = user.Claims.FirstOrDefault(c => c.Type == "firebase");
        if (firebaseClaim == null) {
            throw new NullReferenceException("Firebase claim is null!");
        }

        ClaimsTokenInfo? firebaseUserInfo = JsonConvert.DeserializeObject<ClaimsTokenInfo>(firebaseClaim.Value);
        if (firebaseUserInfo == null) {
            throw new NullReferenceException("Firebase user info was null!");
        }
        
        //If these are reached and throw...then we're probably being targeted.
        firebaseUserInfo.TokenCreated = int.Parse(user.Claims.FirstOrDefault(claim => claim.Type == "iat")!.Value);
        firebaseUserInfo.TokenExpires = int.Parse(user.Claims.FirstOrDefault(claim => claim.Type == "exp")!.Value);
        firebaseUserInfo.UserId = user.Claims.FirstOrDefault(claim => claim.Type == "user_id")!.Value;
        firebaseUserInfo.AuthorityUrl = user.Claims.FirstOrDefault(claim => claim.Type == "iss")!.Value;
        return firebaseUserInfo;
    }

    /// <summary>
    /// Helper method to quickly get the email of the JWT user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GetUserEmail(ClaimsPrincipal user) {
        ClaimsTokenInfo tokenInfo = GetFirebaseUserInfo(user);
        return tokenInfo.Identities.Email;
    }
}