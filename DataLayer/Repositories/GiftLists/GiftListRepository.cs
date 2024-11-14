using DataLayer.Database;
using DataLayer.Database.Schema.GiftList;
using DataLayer.Repositories.Abstract;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories.GiftLists;

/// <summary>
/// A repository that is used to directly manipulate the data for GiftLists.
/// </summary>
/// <param name="context"></param>
/// <param name="logger"></param>
public class GiftListRepository(DatabaseContext context, ILogger<GiftListRepository> logger) : AbstractRepository<GiftListSchemaModel>(context, logger) {

    /// <summary>
    /// Read a table row by id that also belongs to a give account id.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="listId"></param>
    /// <returns></returns>
    public async Task<GiftListSchemaModel?> ReadByAccountIdAsync(Guid accountId, Guid listId) {
        return await Task.FromResult(Set.FirstOrDefault(e => e.Account.Id == accountId && e.Id == listId));
    }
    
    /// <summary>
    /// Read all table rows that belong to a given account Id.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GiftListSchemaModel>> ReadAllByAccountIdAsync(Guid accountId) {
        return await Task.FromResult(Set.Where(e => e.Account.Id == accountId));
    }
}