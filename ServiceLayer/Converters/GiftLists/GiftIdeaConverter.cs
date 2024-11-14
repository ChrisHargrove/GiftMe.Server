using DataLayer.Database.Schema.GiftList;
using Models.GiftList;
using ServiceLayer.Converters.Abstract;

namespace ServiceLayer.Converters.GiftLists;

/// <summary>
/// Converter for the GiftIdea and its schema model representation.
/// </summary>
public class GiftIdeaConverter: AbstractConverter<GiftIdea, GiftIdeaSchemaModel> {
    public GiftIdeaConverter(): base() {
        
    }
}