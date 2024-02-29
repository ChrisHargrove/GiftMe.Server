using Microsoft.Extensions.Logging;

namespace ServiceLayer.Services.Abstract;

/// <summary>
/// Base class for all services.
/// </summary>
public abstract class AbstractGiftMeService(ILogger logger) : IGiftMeService {
    protected ILogger Logger { get; } = logger;
}