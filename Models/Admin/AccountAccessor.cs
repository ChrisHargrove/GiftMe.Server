using System.Runtime.Serialization;
using Models.Abstract;
using Models.Identity;

namespace Models.Admin;

/// <summary>
/// Represents first time user access to the system or when an admin has suspended account activity.
/// </summary>
[DataContract]
public class AccountAccessor: AbstractModel {
    [DataMember]
    public AccessStatus Status { get; set; }
    [DataMember]
    public Account Account { get; set; }
}