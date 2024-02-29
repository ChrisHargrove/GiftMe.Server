using System.Runtime.Serialization;

namespace Models.Abstract;

/// <summary>
/// The base of all models for the GiftMe Application
/// </summary>
[DataContract]
public abstract class AbstractModel {
    [DataMember(EmitDefaultValue = false)]
    public Guid Id { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public DateTime CreatedAt { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public DateTime UpdatedAt { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public DateTime? DeletedAt { get; set; }
}