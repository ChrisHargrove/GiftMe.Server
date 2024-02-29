using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers.Extensions;

/// <summary>
/// Extension class for all the helper methods related to the ServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Registers all classes in the AppDomain by their given implemented interface.
    /// </summary>
    /// <param name="serviceCollection">The service collection that these services are being added to.</param>
    /// <typeparam name="T">The base interface all these services must have implemented.</typeparam>
    /// <returns>
    /// Returns the service collection to allow for chaining of other methods after this, in fluent style. 
    /// </returns>
    public static IServiceCollection AddAllScoped<T>(this IServiceCollection serviceCollection) {
        Func<Type, bool> checkFunc = typeof(T).IsInterface
            ? t => t is { IsAbstract: false, IsClass: true } && t.GetInterfaces().Contains(typeof(T))
            : t => t is { IsAbstract: false, IsClass: true } && t.BaseType == typeof(T);
        
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()).Where(checkFunc);
        foreach (Type type in types) {
            Console.WriteLine($"Registering: {type.Name}");
            serviceCollection.AddScoped(type);
        }
        return serviceCollection;
    }
}