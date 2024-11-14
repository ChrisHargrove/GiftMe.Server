namespace ServiceLayer.Services.Firebase.Request;

/// <summary>
/// Represents a request to the FIrebase REST API to sign in a user and get a auth token and refresh token.
/// </summary>
public class FirebaseSignInRequest {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool ReturnSecureToken { get; set; } = true;
}