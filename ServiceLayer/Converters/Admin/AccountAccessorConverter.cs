using DataLayer.Database.Schema.Identity;
using Models.Admin;
using ServiceLayer.Converters.Abstract;

namespace ServiceLayer.Converters.Admin;

/// <summary>
/// Converter for the Account Accessor and its schema model representation.
/// </summary>
public class AccountAccessorConverter : AbstractConverter<AccountAccessor, AccountAccessorSchemaModel> {
    public AccountAccessorConverter(): base() {
        
    }
}