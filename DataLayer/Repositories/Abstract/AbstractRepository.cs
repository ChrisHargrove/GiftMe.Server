using System.Linq.Expressions;
using System.Net;
using DataLayer.Database;
using DataLayer.Database.Schema.Abstract;
using Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories.Abstract;

/// <summary>
/// This is the base class for all repositories in the project.
/// <para>
/// It defines the basic CRUD methods that all repositories will have.
/// </para>
/// </summary>
/// <param name="context"></param>
/// <typeparam name="T">SchemaModel</typeparam>
public abstract class AbstractRepository<T>(DatabaseContext context, ILogger logger) : IRepository
    where T : AbstractSchemaModel, new ()
{
    
    /// <summary>
    /// This is the inherited Database context that allows for access to the tables in the database.
    /// </summary>
    protected DatabaseContext Context { get; } = context;

    /// <summary>
    /// This allows access to logging facilities in all repositories.
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// This allows access to the base set of data that this repository operates on.
    /// </summary>
    protected DbSet<T> Set => Context.Set<T>();

    /// <summary>
    /// Default Create method as part of CRUD. It will create the supplied model into the database.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>The newly created model.</returns>
    public async Task<T> CreateAsync(T model) {
        EntityEntry<T> entry = await Set.AddAsync(model);
        await Context.SaveChangesAsync();
        return await Task.FromResult(entry.Entity);
    }

    /// <summary>
    /// Default Create method for multiple entities as part of CRUD. It will create all the supplied models into the database.
    /// </summary>
    /// <param name="models"></param>
    /// <returns>The newly created models.</returns>
    public async Task<IEnumerable<T>> CreateAsync(IEnumerable<T> models) {
        IEnumerable<EntityEntry<T>> entries = models.Select(m => Set.Add(m));
        await Context.SaveChangesAsync();
        return entries.Select(e => e.Entity);
    }
    
    /// <summary>
    /// Default Read method as part of CRUD. It will read a model from the database using the supplied id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The model that was read from the database.</returns>
    public async Task<T?> ReadAsync(Guid id) {
        return await Set.FindAsync(id);
    }

    /// <summary>
    /// Default Read method for a multi retrieval of models as part of CRUD. It will read multiple models from the database using the supplied ids.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns>The models that were read from the database.</returns>
    public async Task<IEnumerable<T>> ReadAsync(IEnumerable<Guid> ids) {
        return await Task.FromResult(Set.Where(e => ids.Contains(e.Id)));
    }

    /// <summary>
    /// Default Read method that will read all rows from the database.
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IEnumerable<T>> ReadAsync() {
        return await Task.FromResult(Set);
    }

    /// <summary>
    /// Default Update method as part of CRUD. It will update the given model in the database.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="selectors"></param>
    /// <returns>The newly updated model in the database.</returns>
    public async Task<T> UpdateAsync(T model, params Expression<Func<T, object?>>[] selectors) {
        T? entity = Context.Set<T>().Local.FirstOrDefault(e => e.Id == model.Id);
        if (entity == null) {
            throw new HttpResponseException(HttpStatusCode.NotFound, $"Entity of type: {model.GetType().Name} with Id: {model.Id} does not exist!");
        }

        EntityEntry<T> newEntry = Set.Entry(model);
        EntityEntry<T> entry = Set.Entry(entity);
        
        foreach (Expression<Func<T, object?>> selector in selectors) {
            entry.Property(selector).CurrentValue = newEntry.Property(selector).CurrentValue;
        }
        await SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Default Update method for updating multiple models in one operation, as part of CRUD. It will update all the supplied models.
    /// </summary>
    /// <param name="models"></param>
    /// <param name="selectors"></param>
    /// <returns>The newly updated models.</returns>
    public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> models, params Expression<Func<T, object?>>[] selectors) {
        IEnumerable<T> updated = models.Select(m => {
            T? entity = Context.Set<T>().Local.FirstOrDefault(e => e.Id == m.Id);
            if (entity == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound, $"Entity of type: {m.GetType().Name} with Id: {m.Id} does not exist!");
            }

            EntityEntry<T> newEntry = Set.Entry(m);
            EntityEntry<T> entry = Set.Entry(entity);

            foreach (Expression<Func<T, object?>> selector in selectors) {
                entry.Property(selector).CurrentValue = newEntry.Property(selector).CurrentValue;
            }
            return entry.Entity;
        });
        await SaveChangesAsync();
        return updated;
    }

    /// <summary>
    /// Default Delete method to delete a model by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T?> DeleteAsync(Guid id) {
        T? schema = await Set.FirstOrDefaultAsync(e => e.Id == id);
        if (schema == null) {
            return null;
        }
        Set.Remove(schema);
        await SaveChangesAsync();
        return schema;
    }

    /// <summary>
    /// Default delete method to delete many models at once by their ids.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<Guid> ids) {
        List<T> schema = Set.Where(e => ids.Contains(e.Id)).ToList();
        List<EntityEntry<T>> entries = schema.Select(m => Set.Remove(m)).ToList();
        await SaveChangesAsync();
        return entries.Select(e => e.Entity);
    }

    /// <summary>
    /// Default Delete method as part of CRUD. it will delete the specified model from the database.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>The recently deleted model.</returns>
    public async Task<T> DeleteAsync(T model) {
        EntityEntry<T> entry = Set.Remove(model);
        await SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Default Delete method for a multi model delete operation, as part of CRUD. It will delete all supplied models from the database.
    /// </summary>
    /// <param name="models"></param>
    /// <returns>The recently deleted models.</returns>
    public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> models) {
        List<EntityEntry<T>> entries = models.Select(m => Set.Remove(m)).ToList();
        await SaveChangesAsync();
        return entries.Select(e => e.Entity);
    }

    /// <summary>
    /// Utility method to easy the operation of saving data to the database.
    /// </summary>
    /// <returns></returns>
    protected async Task<int> SaveChangesAsync() {
        return await Context.SaveChangesAsync();
    }
}