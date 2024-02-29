namespace Helpers.Extensions;

/// <summary>
/// Static Class that contains all task based utility extensions and helpers.
/// </summary>
public static class TaskExtensions {
    /// <summary>
    /// Creates a new TaskCompletionSource with the given result value and returns the task from it.
    /// </summary>
    /// <param name="result">The completion result.</param>
    /// <typeparam name="T">Any Type</typeparam>
    /// <returns>Returns a task containing the result.</returns>
    public static Task<T> CreateCompletedTask<T>(T result) {
        return new TaskCompletionSource<T>().CompleteWith(result);
    }
    
    /// <summary>
    /// Sets the result of a TaskCompletionSource.
    /// </summary>
    /// <param name="source">The original TaskCompletionSource</param>
    /// <param name="result">The completion result.</param>
    /// <typeparam name="T">Any Type</typeparam>
    /// <returns>Returns the TaskCompletionSource task containing the final result.</returns>
    public static Task<T> CompleteWith<T>(this TaskCompletionSource<T> source, T result) {
        source.SetResult(result);
        return source.Task;
    }
}