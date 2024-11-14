namespace ServiceLayer.Services.Firebase.Request;

/// <summary>
/// Represents a request to the Firebase REST API to start OOB (Out Of Band) code requests, for operations such as:
/// <list type="bullet">
/// <item>Verify Email</item>
/// <item>Reset Password</item>
/// </list>
/// </summary>
public class FirebaseSendOobCodeRequest {
    public FirebaseOobType RequestType { get; set; }
    public string? Email { get; set; }
    /// <summary>
    /// Only required when using the Verify and Change Email request type.
    /// </summary>
    public string? NewEmail { get; set; }
    /// <summary>
    /// This is only required when using the verify email request types.
    /// </summary>
    public string? IdToken { get; set; }
}