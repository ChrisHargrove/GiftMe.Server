using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Models.Auth.DTO;

/// <summary>
/// DTO Model that allows users to sign in to their account.
/// </summary>
[DataContract]
public class SignIn {
    /// <summary>
    /// The email associated with an account.
    /// </summary>
    [DataMember]
    public required string Email { get; set; }
    
    /// <summary>
    /// The password associated with an account.
    /// </summary>
    [DataMember]
    public required string Password { get; set; }
}