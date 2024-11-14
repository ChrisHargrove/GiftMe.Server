using System.Collections;
using DataLayer.Database;
using DataLayer.Database.Schema.Identity;
using DataLayer.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories.Identity;

/// <summary>
/// Repository that allows access and manipulation of table data related to AccountAccessors. 
/// </summary>
/// <param name="context"></param>
/// <param name="logger"></param>
public class AccountAccessorRepository(DatabaseContext context, ILogger<AccountAccessorRepository> logger) : AbstractRepository<AccountAccessorSchemaModel>(context, logger) {
    
    /// <summary>
    /// Get an accessor entry that relates to an account by email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<AccountAccessorSchemaModel?> ReadByEmailAsync(string email) {
        return await Set
            .Include(m => m.Account)
            .FirstOrDefaultAsync(m => m.Account.Email == email);
    }

    /// <summary>
    /// Reads all table rows from this particular table and also includes their account data to the accessor.
    /// </summary>
    /// <returns></returns>
    public override async Task<IEnumerable<AccountAccessorSchemaModel>> ReadAsync() {
        return await Set
            .Include(m => m.Account)
            .ToListAsync();
    }
}