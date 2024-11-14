using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Models.Auth.DTO;

/// <summary>
/// Base class for all account creation requests.
/// </summary>
[DataContract]
public class AbstractSignUp {
    /// <summary>
    /// The unique username for a created account.
    /// </summary>
    [DataMember]
    public required string Username { get; set; }

    /// <summary>
    /// The non-unique displayName for a created account.
    /// </summary>
    [DataMember]
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// The date of birth of the owner of the account.
    /// </summary>
    [DataMember]
    public DateTime DateOfBirth { get; set; }
}