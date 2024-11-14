using AutoMapper;
using DataLayer.Database.Schema.Abstract;
using Models.Abstract;

namespace ServiceLayer.Converters.Abstract;

/// <summary>
/// Base converter class that gets auto registered upon application startup, it handles registering the basic conversion
/// between a schema and its concrete model.
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TSchema"></typeparam>
public abstract class AbstractConverter<TModel, TSchema> : Profile, IConverter
    where TModel : AbstractModel, new()
    where TSchema : AbstractSchemaModel, new()
{
    protected AbstractConverter() {
        CreateMap<TModel, TSchema>();
        CreateMap<TSchema, TModel>();
    }
}