using Microsoft.VisualBasic;

namespace Helpers.Extensions;

/// <summary>
/// Extension class that allows for many utility and helper methods when dealing with asynchronous work flows.
/// </summary>
public static class AsyncExtensions {
    /// <summary>
    /// Tasks a synchronous function with a return object and makes it asynchronous with the same return object. 
    /// </summary>
    /// <param name="fn">The function that you wish to make async.</param>
    /// <returns></returns>
    public static Func<Task<T>> MakeAsync<T>(this Func<T> fn)
        => async () => {
            await Task.CompletedTask;
            return fn();
        };

    /// <summary>
    /// Takes a synchronous function with no return type and makes it asynchronous.
    /// </summary>
    /// <param name="act">The function that you wish to make async.</param>
    /// <returns></returns>
    public static Func<Task> MakeAsync(this Action act)
        => async () => {
            await Task.CompletedTask;
            act();
        };
}