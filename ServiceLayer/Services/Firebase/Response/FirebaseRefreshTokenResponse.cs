using Newtonsoft.Json;

namespace ServiceLayer.Services.Firebase.Response;

/// <summary>
/// Represents a response from the Firebase REST API when making a refresh token request.
/// </summary>
public class FirebaseRefreshTokenResponse {
    [JsonProperty("expires_in")]
    public string ExpiresIn { get; set; } = null!;
    [JsonProperty("token_type")]
    public string TokenType { get; set; } = null!;
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = null!;
    [JsonProperty("id_token")]
    public string IdToken { get; set; } = null!;
    [JsonProperty("user_id")]
    public string UserId { get; set; } = null!;
    [JsonProperty("project_id")]
    public string ProjectId { get; set; } = null!;
}