using System.Runtime.Serialization;

namespace Models.Auth.DTO;

/// <summary>
/// Represents a token response from either a signup or signin event.
/// </summary>
[DataContract]
public class TokenResponse {
    /// <summary>
    /// The users new JWT authorisation token.
    /// </summary>
    [DataMember]
    public required string Token { get; set; }
}