using Newtonsoft.Json;

namespace ServiceLayer.Services.Firebase.Request;

/// <summary>
/// Represents the request to send to the firebase REST API for refreshing a authorization token.
/// </summary>
public class FirebaseRefreshTokenRequest {
    [JsonProperty("grant_type")]
    public string GrantType { get; set; } = "refresh_token";

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = null!;
}