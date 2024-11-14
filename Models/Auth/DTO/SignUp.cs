using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Models.Auth.DTO;

/// <summary>
/// Model for when a user wishes to create an account using email and password.
/// </summary>
[DataContract]
public class SignUp : AbstractSignUp {
    /// <summary>
    /// The users email associated with their new account request.
    /// </summary>
    [DataMember]
    public required string Email { get; set; }
    
    /// <summary>
    /// The users password associated with their new account request.
    /// </summary>
    [DataMember]
    public required string Password { get; set; }
}