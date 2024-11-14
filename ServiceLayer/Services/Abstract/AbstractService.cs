using System.Collections;
using System.Linq.Expressions;
using AutoMapper;
using DataLayer.Database.Schema.Abstract;
using DataLayer.Repositories.Abstract;
using Microsoft.Extensions.Logging;
using Models.Abstract;

namespace ServiceLayer.Services.Abstract;

/// <summary>
/// Base class for all services.
/// </summary>
public abstract class AbstractService(ILogger logger) : IService {
    /// <summary>
    /// Stored reference to the logger.
    /// </summary>
    protected ILogger Logger { get; } = logger;
}

/// <summary>
/// Base class for all services.
/// </summary>
public abstract class AbstractMappedService(ILogger logger, IMapper mapper) : AbstractService(logger) {
    /// <summary>
    /// Stored reference to the mapper object for model conversions.
    /// </summary>
    protected IMapper Mapper { get; } = mapper;
}

/// <summary>
/// Base class for services that require a repository to operate.
/// </summary>
/// <param name="logger"></param>
/// <param name="repository"></param>
/// <typeparam name="TModel"></typeparam>
/// <typeparam name="TSchema"></typeparam>
/// /// <typeparam name="TRepository"></typeparam>
public abstract class AbstractService<TModel, TSchema, TRepository>(TRepository repository, ILogger logger, IMapper mapper) : AbstractMappedService(logger, mapper)
    where TModel : AbstractModel, new ()
    where TSchema : AbstractSchemaModel, new ()
    where TRepository : AbstractRepository<TSchema>
{
    /// <summary>
    /// Stored reference to the repository of this service.
    /// </summary>
    protected TRepository Repository { get; } = repository;

    /// <summary>
    /// Basic Method for creating a new model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<TModel> CreateAsync(TModel model) {
        TSchema? schema = Mapper.Map<TSchema>(model);
        TSchema insertedSchema = await Repository.CreateAsync(schema);
        return Mapper.Map<TModel>(insertedSchema);
    }

    /// <summary>
    /// Basic Method for creating multiple models at once.
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TModel>> CreateAsync(IEnumerable<TModel> models) {
        IEnumerable<TSchema> schema = models.Select(Mapper.Map<TSchema>);
        IEnumerable<TSchema> inserted = await Repository.CreateAsync(schema);
        return inserted.Select(Mapper.Map<TModel>);
    }

    /// <summary>
    /// Basic Method for reading a model by its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TModel> ReadAsync(Guid id) {
        TSchema? schema = await Repository.ReadAsync(id);
        return Mapper.Map<TModel>(schema);
    }

    /// <summary>
    /// Basic Method for reading a set of models by their Ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TModel>> ReadAsync(IEnumerable<Guid> ids) {
        IEnumerable<TSchema> schema = await Repository.ReadAsync(ids);
        return schema.Select(Mapper.Map<TModel>);
    }

    /// <summary>
    /// Basic Method for reading all models of a given type.
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TModel>> ReadAllAsync() {
        IEnumerable<TSchema> schema = await Repository.ReadAsync();
        return schema.Select(Mapper.Map<TModel>);
    }

    /// <summary>
    /// Basic Method for updating a model in the database. Users of this method must specify what fields are going
    /// to be updated by this update.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="selectors"></param>
    /// <returns></returns>
    public virtual async Task<TModel> UpdateAsync(TModel model, params Expression<Func<TSchema, object?>>[] selectors) {
        return Mapper.Map<TModel>(await Repository.UpdateAsync(Mapper.Map<TSchema>(model), selectors));
    }

    /// <summary>
    /// Basic Method for updating a list of models. Users of this method must specify what fields are going
    /// to be updated by this update.
    /// </summary>
    /// <param name="models"></param>
    /// <param name="selectors"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TModel>> UpdateAsync(IEnumerable<TModel> models, params Expression<Func<TSchema, object?>>[] selectors) {
        IEnumerable<TSchema> updatedModels = await Repository.UpdateAsync(models.Select(Mapper.Map<TSchema>), selectors);
        return updatedModels.Select(Mapper.Map<TModel>);
    }

    /// <summary>
    /// Basic Method for deleting a model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<TModel> DeleteAsync(TModel model) {
        TSchema? deleted = await Repository.DeleteAsync(model.Id);
        return Mapper.Map<TModel>(deleted);
    }

    /// <summary>
    /// Basic Method for deleting a model by its Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TModel> DeleteAsync(Guid id) {
        TSchema? deleted = await Repository.DeleteAsync(id);
        return Mapper.Map<TModel>(deleted);
    }

    /// <summary>
    /// Basic Method for deleting a set of models.
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TModel>> DeleteAsync(IEnumerable<TModel> models) {
        IEnumerable<TSchema> deleted = await Repository.DeleteAsync(models.Select(m => m.Id));
        return deleted.Select(Mapper.Map<TModel>);
    }

    /// <summary>
    /// Basic Method for delting a set of models specified by their Ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TModel>> DeleteAsync(IEnumerable<Guid> ids) {
        IEnumerable<TSchema> deleted = await Repository.DeleteAsync(ids);
        return deleted.Select(Mapper.Map<TModel>);
    }
} 