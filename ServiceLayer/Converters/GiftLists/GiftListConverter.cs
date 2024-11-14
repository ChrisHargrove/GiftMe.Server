using DataLayer.Database.Schema.GiftList;
using Models.GiftList;
using ServiceLayer.Converters.Abstract;

namespace ServiceLayer.Converters.GiftLists;

/// <summary>
/// Converter for the GiftList and its schema model representation.
/// </summary>
public class GiftListConverter: AbstractConverter<GiftList, GiftListSchemaModel> {
    public GiftListConverter(): base() {
        
    }
}