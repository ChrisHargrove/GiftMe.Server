using Api.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.GiftList;
using ServiceLayer.Services.GiftListService;

namespace Api.Controllers.GiftLists;

/// <summary>
/// Controller class that gives access to the gift lists. 
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
[Route("giftList")]
public class GiftListController(GiftListService service, ILogger<GiftListController> logger) : AbstractController<GiftListService>(service, logger) {

    /// <summary>
    /// Create a new list for the account who called the endpoint.
    /// </summary>
    /// <remarks>
    /// Currently no limit of how many lists a user can create, might want to limit this to avoid excessive bloat.
    /// </remarks>
    /// <param name="list"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GiftList> CreateList([FromBody] GiftList list) {
        return await Service.CreateAsync(Account.Id, list);
    }
    
    /// <summary>
    /// Gets a specified list from the service using the lists id. 
    /// </summary>
    /// <remarks>
    /// This endpoint will only retrieve a list if it belongs to a user.
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<GiftList> GetListForAccount(Guid id) {
        return await Service.ReadAsync(Account.Id, id);
    }
    
    /// <summary>
    /// Gets all lists for the account.
    /// </summary>
    /// <remarks>
    /// This endpoint will only retrieve lists that belongs to a user.
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<GiftList>> GetAllListsForAccount() {
        return await Service.ReadAllAsync(Account.Id);
    }
    
    /// <summary>
    /// Deletes a list with given Id if it belongs to the account making the call.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<GiftList?> Delete(Guid id) {
        return await Service.DeleteAsync(Account.Id, id);
    }

    /// <summary>
    /// Deletes all the gift lists that belong to the account that made the call to the server.
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IEnumerable<GiftList>> DeleteAll() {
        return await Service.DeleteAllAsync(Account.Id);
    }
}