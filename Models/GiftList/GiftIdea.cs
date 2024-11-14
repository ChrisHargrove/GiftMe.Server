using System.Runtime.Serialization;
using Models.Abstract;
using Models.Identity;

namespace Models.GiftList;

/// <summary>
/// Represents an item that has been added to a gift list.
/// </summary>
[DataContract]
public class GiftIdea: AbstractModel {
    [DataMember]
    public Guid GiftListId { get; set; }
    
    [DataMember]
    public string Name { get; set; } = null!;
    [DataMember]
    public int? Cost { get; set; }
    [DataMember]
    public string? Link { get; set; } 
    [DataMember]
    public string? Image { get; set; }
    
    [DataMember]
    public bool? IsPurchased { get; set; }
    [DataMember]
    public Account? Purchaser { get; set; } 
}