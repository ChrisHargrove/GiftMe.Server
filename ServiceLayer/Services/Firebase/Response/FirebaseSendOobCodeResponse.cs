namespace ServiceLayer.Services.Firebase.Response;

/// <summary>
/// Represents a response from the Firebase REST API when starting OOB (Out Of Band) operations.
/// </summary>
public class FirebaseSendOobCodeResponse {
    public string Kind { get; set; } = null!;
    public string OobCode { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string OobLink { get; set; } = null!;
}