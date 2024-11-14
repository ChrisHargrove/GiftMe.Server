using DataLayer.Database;
using DataLayer.Database.Schema.GiftList;
using DataLayer.Repositories.Abstract;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories.GiftIdeas;

/// <summary>
/// A repository that directly handles working with the GiftIdea data table int eh database.
/// </summary>
/// <param name="context"></param>
/// <param name="logger"></param>
public class GiftIdeaRepository(DatabaseContext context, ILogger<GiftIdeaRepository> logger) : AbstractRepository<GiftIdeaSchemaModel>(context, logger) {
}