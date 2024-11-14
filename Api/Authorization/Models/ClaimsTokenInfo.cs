using Newtonsoft.Json;

namespace Api.Authorization;

/// <summary>
/// This model represents the data that is found inside of the firebase JWT claims.
/// </summary>
public class ClaimsTokenInfo {
    [JsonProperty(PropertyName = "identities")]
    public FirebaseIdentitiesForUser Identities { get; set; } = null!;

    [JsonProperty(PropertyName = "sign_in_provider")]
    public string SignInProvider { get; set; } = null!;
    public int TokenCreated { get; set; }
    public int TokenExpires { get; set; }
    public string AuthorityUrl { get; set; } = null!;
    public Guid NameIdentifier { get; set; }
    public string UserId { get; set; } = null!;
}

/// <summary>
/// This model represents the available providers that could be inside of a firebase claims
/// token.
/// </summary>
public class FirebaseIdentitiesForUser
{
    [JsonProperty(PropertyName = "google.com")]
    public string[] GoogleDotCom { get; set; } = null!;
    [JsonProperty(PropertyName = "apple.com")]
    public string[] AppleDotCom { get; set; } = null!;
    [JsonProperty(PropertyName = "email")]
    public string[] Emails { get; set; } = null!;
    public string Email => Emails[0];
}