using System.Net;
using Helpers.Extensions;

namespace Api.Validation;

/// <summary>
/// Base class for any validation check that needs to be run on a data model.
/// </summary>
public abstract class AbstractValidationCheck {
    /// <summary>
    /// Constructs a new validation check.
    /// </summary>
    /// <param name="errorMessage">What the failure message should be.</param>
    /// <param name="statusCode">What the failure status code should be.</param>
    protected AbstractValidationCheck(string errorMessage, HttpStatusCode statusCode) {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }
    /// <summary>
    /// The validation checks error message if it fails.
    /// </summary>
    public string ErrorMessage { get; protected set; }
    /// <summary>
    /// The validation checks completion status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; protected set; }
    /// <summary>
    /// The condition to be executed when the validation check is run.
    /// <remarks>
    /// Defaults to return true if no condition is specified.
    /// </remarks>
    /// </summary>
    public Func<Task<bool>> Condition { get; protected set; } = async () => await new TaskCompletionSource<bool>().CompleteWith(true);
    /// <summary>
    /// A callback that is executed on validation check failure.
    /// <remarks>
    /// Defaults to empty lambda if nothing is provided.
    /// </remarks>
    /// </summary>
    public Action OnFailed { get; protected set; } = () => { };
}

/// <summary>
/// <inheritdoc cref="AbstractValidationCheck"/>
/// </summary>
/// <typeparam name="TModelCheck">The type of validation check, this is so that the fluent returns can correctly cast.</typeparam>
public abstract class AbstractValidationCheck<TModelCheck> : AbstractValidationCheck
    where TModelCheck : AbstractValidationCheck<TModelCheck> {
    
    /// <summary>
    /// Constructs a new validation check.
    /// </summary>
    /// <param name="errorMessage">What the failure message should be.</param>
    /// <param name="statusCode">What the failure status code should be. <para>See <see cref="HttpStatusCode"/></para></param>
    public AbstractValidationCheck(string errorMessage, HttpStatusCode statusCode) : base(errorMessage, statusCode) {
    }

    /// <summary>
    /// Sets an async condition for the validation check.
    /// <remarks>
    /// Throws ArgumentNullException if a null condition is passed.
    /// </remarks>
    /// </summary>
    /// <param name="condition">The condition that you want to execute. 
    /// <para>
    /// Returns true on success, false on failure.
    /// </para>
    /// </param>
    /// <returns>
    /// Returns the ValidationCheck so that the methods can be chained in fluent style.
    /// </returns>
    public TModelCheck SetCondition(Func<Task<bool>> condition) {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        return (TModelCheck)this;
    }

    /// <summary>
    /// Sets an sync condition for the validation check and wraps it to make it async.
    /// <remarks>
    /// Throws ArgumentNullException if a null condition is passed.
    /// </remarks>
    /// </summary>
    /// <param name="condition">The condition that you want to execute. 
    /// <para>
    /// Returns true on success, false on failure.
    /// </para>
    /// </param>
    /// <returns>
    /// Returns the ValidationCheck so that the methods can be chained in fluent style.
    /// </returns>
    public TModelCheck SetCondition(Func<bool> condition) {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        Condition = condition.MakeAsync();
        return (TModelCheck)this;
    }

    /// <summary>
    /// Sets the expected error message on failure for this validation check.
    /// <remarks>
    /// Throws ArgumentNullException if a null errorMessage is passed.
    /// </remarks>
    /// </summary>
    /// <param name="errorMessage">The error message that is expected on failure.</param>
    /// <returns>
    /// Returns the ValidationCheck so that the methods can be chained in fluent style.
    /// </returns>
    public TModelCheck SetErrorMessage(string errorMessage) {
        ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        return (TModelCheck)this;
    }

    /// <summary>
    /// Sets the expected <see cref="HttpStatusCode" /> of the validation check.
    /// </summary>
    /// <param name="statusCode">The returned status code of the validation check.</param>
    /// <returns>
    /// Returns the ValidationCheck so that the methods can be chained in fluent style.
    /// </returns>
    public TModelCheck SetStatusCode(HttpStatusCode statusCode) {
        StatusCode = statusCode;
        return (TModelCheck)this;
    }

    /// <summary>
    /// Sets the callback to be executed on validation failure.
    /// <remarks>
    /// Throws ArgumentNullException if a null onFailure callback is passed.
    /// </remarks>
    /// </summary>
    /// <param name="onFailed">The callback you wish to be executed on validation failure.</param>
    /// <returns>
    /// Returns the ValidationCheck so that the methods can be chained in fluent style.
    /// </returns>
    public TModelCheck SetOnFailed(Action onFailed) {
        OnFailed = onFailed ?? throw new ArgumentNullException(nameof(onFailed));
        return (TModelCheck)this;
    }
}