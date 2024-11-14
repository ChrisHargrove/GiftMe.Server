namespace ServiceLayer.Services.Firebase.Request;

/// <summary>
/// Represents a response from the Firebase REST API when updating an accounts information.
/// </summary>
public class FirebaseUpdateInfoResponse {
    public string Kind { get; set; } = null!;
    public string LocalId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string IdToken { get; set; } = null!;
    public string NewEmail { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string ExpiresIn { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool EmailVerified { get; set; } = false;
}