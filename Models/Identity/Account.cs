using System.Runtime.Serialization;
using Models.Abstract;

namespace Models.Identity;

/// <summary>
/// Represents an account that is capable of accessing the system and all its pertinent data.
/// </summary>
[DataContract]
public class Account : AbstractModel {
    [DataMember]
    public string Username { get; set; } = null!;
    [DataMember]
    public string? DisplayName { get; set; }
    [DataMember]
    public string Email { get; set; } = null!;
    [DataMember]
    public AccountRole Role { get; set; }
    [DataMember]
    public AccessStatus Status { get; set; }
}