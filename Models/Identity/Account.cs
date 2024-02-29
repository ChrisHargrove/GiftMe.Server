using System.Runtime.Serialization;
using Models.Abstract;

namespace Models.Identity;

[DataContract]
public class Account : AbstractModel {
    [DataMember]
    public string Username { get; set; } = null!;

    [DataMember]
    public string? DisplayName { get; set; }
    
    [DataMember]
    public AccountRole Role { get; set; }
}