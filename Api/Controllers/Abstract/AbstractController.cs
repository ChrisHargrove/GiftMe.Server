using Api.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Abstract;

/// <summary>
/// The base class of all controllers in the API
/// </summary>
[ApiController]
public abstract class AbstractController : Controller {
    /// <summary>
    /// Bae constructor that is required to pass the logger to the Validator.
    /// </summary>
    /// <param name="logger"></param>
    protected AbstractController(ILogger logger) : base() {
        Logger = logger;
    }
    
    /// <summary>
    /// Stored reference to the ILogger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Inherited getter that allows all Controllers access to a Validator.
    /// </summary>
    protected ModelValidator Validator => new ModelValidator(Logger);
}