namespace ServiceLayer.Services.Firebase.Response;

/// <summary>
/// Represents a response from the Firebase REST API when making a signup or signin request.
/// </summary>
public class FirebaseAuthResponse {
    public string Kind { get; set; } = null!;
    public string IdToken { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string ExpiresIn { get; set; } = null!;
    public string LocalId { get; set; } = null!;
}