using System.Runtime.Serialization;
using Models.Abstract;
using Models.Identity;

namespace Models.GiftList;

/// <summary>
/// Represents a collection of items or things that a user would like. Can be optionally time gated.
/// </summary>
[DataContract]
public class GiftList: AbstractModel {
    [DataMember]
    public Guid AccountId { get; set; }
    [DataMember]
    public string Name { get; set; } = null!;
    [DataMember]
    public DateTime? Date { get; set; }
    [DataMember]
    public List<GiftIdea>? GiftIdeas { get; set; }
}