namespace ServiceLayer.Services.Firebase.Request;

/// <summary>
/// Represents a request to the FIrebase REST API to sign up a new user with an email and password.
/// </summary>
public class FirebaseSignUpRequest {
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? DisplayName { get; set; }
}