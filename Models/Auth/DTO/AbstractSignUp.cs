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
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// The non-unique displayName for a created account.
    /// </summary>
    [DataMember]
    public string? DisplayName { get; set; }
}