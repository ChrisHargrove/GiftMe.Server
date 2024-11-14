using AutoMapper;
using DataLayer.Database.Schema.GiftList;
using DataLayer.Repositories.GiftLists;
using Microsoft.Extensions.Logging;
using Models.GiftList;
using ServiceLayer.Services.Abstract;

namespace ServiceLayer.Services.GiftListService;

/// <summary>
/// Service Class for the GiftList. It provides some extra methods for interacting with the system to fetch data appropriately.
/// </summary>
/// <param name="repository"></param>
/// <param name="logger"></param>
/// <param name="mapper"></param>
public class GiftListService(GiftListRepository repository, ILogger<GiftListService> logger, IMapper mapper) : AbstractService<GiftList, GiftListSchemaModel, GiftListRepository>(repository, logger, mapper) {

    /// <summary>
    /// Get a gift list with a given Id from a specific account Id
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="listId"></param>
    /// <returns></returns>
    public async Task<GiftList> ReadAsync(Guid accountId, Guid listId) {
        //TODO: need to return a 404 or 403 if either we can find the list or the list doesn't belong to user.
        GiftListSchemaModel? entity = await Repository.ReadByAccountIdAsync(accountId, listId);
        return Mapper.Map<GiftList>(entity);
    }
    
    /// <summary>
    /// Get all gift lists for a given account Id.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GiftList>> ReadAllAsync(Guid accountId) {
        IEnumerable<GiftListSchemaModel> entities = await Repository.ReadAllByAccountIdAsync(accountId);
        return entities.Select(Mapper.Map<GiftList>);
    }

    /// <summary>
    /// Create a new GiftList on a specific account id.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<GiftList> CreateAsync(Guid accountId, GiftList list) {
        GiftListSchemaModel created =  Mapper.Map<GiftListSchemaModel>(list);
        created.AccountId = accountId;
        // created.GiftIdeas = new List<GiftIdeaSchemaModel>();
        return Mapper.Map<GiftList>(await Repository.CreateAsync(created));
    }

    /// <summary>
    /// Deletes a giftlist with a given Id from a specific account specified by their account Id.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="listId"></param>
    /// <returns></returns>
    public async Task<GiftList?> DeleteAsync(Guid accountId, Guid listId) {
        //TODO: need to return a 404 or 403 if either we can find the list or the list doesn't belong to user.
        GiftListSchemaModel? entity = await Repository.ReadByAccountIdAsync(accountId, listId);
        if (entity != null) {
            return Mapper.Map<GiftList>(await Repository.DeleteAsync(entity));
        }
        //TODO: throw exception here!
        //TODO: need some validation that the Id is owned by the account, as otherwise they are deleting someone else's stuff!!!
        return null;
    }
    
    /// <summary>
    /// Deletes all gift lists from an account specified by their account Id.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GiftList>> DeleteAllAsync(Guid accountId) {
        var lists = (await Repository.ReadAllByAccountIdAsync(accountId)).ToList();
        return (await Repository.DeleteAsync(lists)).Select(Mapper.Map<GiftList>);
    }
}