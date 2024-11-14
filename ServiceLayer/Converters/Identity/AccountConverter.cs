using DataLayer.Database.Schema.Identity;
using Models.Identity;
using ServiceLayer.Converters.Abstract;

namespace ServiceLayer.Converters.Identity;

/// <summary>
/// Converter for the Account and its schema model representation.
/// </summary>
public class AccountConverter: AbstractConverter<Account, AccountSchemaModel> {
    /// <summary>
    /// The constructor is used to register all conversions of the account.
    /// The abstract already handles the basic conversion between account and account schema model
    /// </summary>
    public AccountConverter() : base() {
        
    }
}