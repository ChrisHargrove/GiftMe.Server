namespace ServiceLayer.Services.Firebase.Request;

/// <summary>
/// Represents a request to the Firebase REST API to update an accounts info.
/// </summary>
public class FirebaseUpdateInfoRequest {
    public string IdToken { get; set; } = null!;
    public string LocalId { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Disabled { get; set; } = false;
    public string PhotoUrl { get; set; } = null!;
    public bool ReturnSecureToken { get; set; } = true;
    public string PhoneNumber { get; set; } = null!;
}