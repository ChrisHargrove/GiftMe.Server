using Api.Authorization;
using Api.Authorization.Helpers;
using Api.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using ServiceLayer.Services.Identity;
using ServiceLayer.Services.Abstract;

namespace Api.Controllers.Abstract;

/// <summary>
/// The base class of all controllers in the API
/// </summary>
/// [Authorize]
[Authorize]
[Authorize(Policy = "Security")]
[ApiController]
public abstract class AbstractController(ILogger logger) : Controller {
    /// <summary>
    /// Stored reference to the ILogger.
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Inherited getter that allows all Controllers access to a Validator.
    /// </summary>
    protected ModelValidator Validator => new(Logger);

    /// <summary>
    /// Backing field for the Account getter.
    /// </summary>
    private Account? _account;
    /// <summary>
    /// Gets the account information related to this authorised user by extracting the data from the IdToken and getting the related
    /// user from the database.
    /// </summary>
    protected Account Account {
        get {
            if (_account == null) {
                Task<Account> accountTask = GetService<AccountService>()!.GetAccountByEmailAsync(ClaimUtils.GetUserEmail(HttpContext.User));
                _account = accountTask.GetAwaiter().GetResult();
            }
            return _account;
        }
    }

    /// <summary>
    /// Private backing storage for the lazy evaluated claims token.
    /// </summary>
    private ClaimsTokenInfo? _tokenInfo;
    /// <summary>
    /// Getter for the ClaimsTokenInfo of the current request.
    /// <remarks>
    /// This getter is lazy evaluated, so the data is populated on first access.
    /// </remarks>
    /// </summary>
    protected ClaimsTokenInfo TokenData {
        get {
            if (_tokenInfo == null) {
                _tokenInfo = ClaimUtils.GetFirebaseUserInfo(HttpContext.User);
            }

            return _tokenInfo;
        }
    }
    
    /// <summary>
    /// Method to retrieve any service of a given type inside of the controllers.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    protected TService? GetService<TService>()
        => (TService?)HttpContext.RequestServices.GetService(typeof(TService));
    
}

/// <summary>
/// Base class for all controllers that also have an associated service class.
/// </summary>
/// <param name="service"></param>
/// <param name="logger"></param>
/// <typeparam name="TService"></typeparam>
public abstract class AbstractController<TService>(TService service, ILogger logger) : AbstractController(logger)
    where TService : IService {
    /// <summary>
    /// Stored reference to the service.
    /// </summary>
    protected TService Service { get; } = service;
}