using System.Linq.Expressions;
using System.Net;
using Helpers.Exceptions;
using Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using TaskExtensions = Helpers.Extensions.TaskExtensions;

namespace Api.Validation;

/// <summary>
/// <para>
/// This class allows for the validation of individual fields on models so that no incorrect data will make it pass the controllers.
/// </para>
/// <para>
/// It also allows the controller methods to have automatically defined failure states with specific <see cref="HttpStatusCode"/>s and error messages.
/// </para>
/// </summary>
public class ModelValidator(ILogger logger) {
    /// <summary>
    /// DI logger injected at construction.
    /// </summary>
    private ILogger Logger { get; } = logger;
    /// <summary>
    /// List of all the checks that are currently in this validator before execution.
    /// </summary>
    private List<AbstractValidationCheck> Checks { get; } = new();

    /// <summary>
    /// Function that gets executed on a full completion of the validator with no errors.
    /// <remarks>
    /// If no function is set it will default to return 200 Ok.
    /// </remarks>
    /// </summary>
    private Func<Task<ActionResult>> OnSuccessFn { get; set; } = async () => await TaskExtensions.CreateCompletedTask(new OkResult());

    /// <summary>
    /// Sets an async function to be run on successful completion of the validator.
    /// </summary>
    /// <param name="onSuccessFn">The callback to be run on successful completion.</param>
    /// <returns>
    /// Returns the ModelValidator so that methods can be chained in fluent style.
    /// </returns>
    /// <exception cref="ArgumentNullException">If onSuccessFn is null</exception>
    public ModelValidator OnSuccess(Func<Task<ActionResult>> onSuccessFn) {
        OnSuccessFn = onSuccessFn ?? throw new ArgumentNullException(nameof(onSuccessFn));
        return this;
    }
    
    ///
    /// /// <summary>
    /// Sets a sync function to be run on successful completion of the validator, that is wrapped to become async.
    /// </summary>
    /// <param name="onSuccessFn">The callback to be run on successful completion.</param>
    /// <returns>
    /// Returns the ModelValidator so that methods can be chained in fluent style.
    /// </returns>
    /// <exception cref="ArgumentNullException">If onSuccessFn is null</exception>
    public ModelValidator OnSuccess(Func<ActionResult> onSuccessFn) {
        if (onSuccessFn == null) throw new ArgumentNullException(nameof(onSuccessFn));
        OnSuccessFn = onSuccessFn.MakeAsync();
        return this;
    }

    /// <summary>
    /// Executes the model validator. It will go through each validation check in turn and execute each one. It will return with failure state on the first failed validation check.
    /// </summary>
    /// <returns>
    /// Returns an async ActionResult.
    /// </returns>
    /// <exception cref="ValidatorException">If any of the validation checks throw</exception>
    public async Task<ActionResult> CheckAsync() {
        List<string> errors = new();
        HttpStatusCode singleErrorStatus = default;
        HttpStatusCode defaultErrorStatus = default;

        foreach (AbstractValidationCheck check in Checks) {
            try {
                if (!await check.Condition()) {
                    errors.Add(string.IsNullOrEmpty(check.ErrorMessage) ? "Unknown Error." : check.ErrorMessage);
                    singleErrorStatus = check.StatusCode;
                    defaultErrorStatus = check.StatusCode;
                    //TODO: This break will fail on first failure, might want it to run through everything instead.
                    break;
                }
            }
            catch (ValidatorException validatorException) {
                errors.AddRange(validatorException.ErrorMessages);
                singleErrorStatus = validatorException.StatusCode;
            }
            catch (Exception ex) {
                Logger.LogError(ex, "Unexpected Error Occured during model validation!.");
                singleErrorStatus = HttpStatusCode.InternalServerError;
                errors.Add(ex.Message);
            }
        }

        return errors.Count switch {
            0 => await OnSuccessFn(),
            1 => throw new ValidatorException(errors, singleErrorStatus),
            _ => throw new ValidatorException(errors, defaultErrorStatus),
        };
    }

    /// <summary>
    /// Checks to see if the specified predicate invocation returns a non-null object.
    /// </summary>
    /// <param name="accessor">Gives access to the variable being checked. The expression is used to determine the parameter name automatically.</param>
    /// <returns>
    /// Returns the <see cref="ModelValidator"/> so that methods can be chained in fluent style.
    /// </returns>
    /// <exception cref="ArgumentNullException">If supplied accessor is null</exception>
    public ModelValidator Exists(Expression<Func<object?>> accessor) {
        if (accessor == null) throw new ArgumentNullException(nameof(accessor));
        
        string name = accessor.GetMemberName();
        Checks.Add(new ValidationCheck()
            .SetCondition(() => accessor.Compile().Invoke() != null)
            .SetErrorMessage($"{name} cannot be null!"));
        return this;
    }
    
    /// <summary>
    /// Checks to see if the specified predicate invocation returns a non-null object.
    /// </summary>
    /// <param name="predicate">Gives access to the variable being checked.</param>
    /// <param name="name">The name of the variable being checked.</param>
    /// <returns>
    /// Returns the <see cref="ModelValidator"/> so that methods can be chained in fluent style.
    /// </returns>
    /// <exception cref="ArgumentNullException">If supplied accessor is null</exception>
    public ModelValidator Exists(Func<Task<object?>> predicate, string name) {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        
        Checks.Add(new ValidationCheck()
            .SetCondition(async () => await predicate.Invoke() != null)
            .SetErrorMessage($"{name} cannot be null!"));
        return this;
    }
    
    #region Validate Non-Nullable

    /// <summary>
    /// <para>
    /// Validates a string variable to check for lengths and nullability.
    /// </para>
    /// <para>
    /// String cannot be null and must be inside specified lengths.
    /// </para>
    /// </summary>
    /// <param name="accessor">Gives access to the variable being checked. The expression is used to determine the parameter name automatically.</param>
    /// <param name="minLength">Minimum length allowed for string. Defaults to 0.</param>
    /// <param name="maxLength">Maximum length allowed for string. Defaults to int.MaxValue.</param>
    /// <returns>
    /// Returns the <see cref="ModelValidator"/> so that methods can be chained in fluent style.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the supplied accessor is null.</exception>
    public ModelValidator Validate(Expression<Func<string?>> accessor, int minLength = 0, int maxLength = int.MaxValue) {
        if (accessor == null) throw new ArgumentNullException(nameof(accessor));
        
        string name = accessor.GetMemberName();
        Checks.Add(new ValidationCheck()
            .SetCondition(() => {
                string value = accessor.Compile().Invoke()!;
                return !string.IsNullOrWhiteSpace(value) && value.Length >= minLength && value.Length <= maxLength;
            })
            .SetErrorMessage($"{name} cannot be empty and has to have a length between {minLength} and {maxLength}."));
        return this;
    }
    
    #endregion

    #region Validate Nullable

    /// <summary>
    /// <para>
    /// Validates a string variable to check for lengths and nullability.
    /// </para>
    /// <para>
    /// String has to be null, empty, only whitespace or or inside specified lengths.
    /// </para>
    /// </summary>
    /// <param name="accessor">Gives access to the variable being checked. The expression is used to determine the parameter name automatically.</param>
    /// <param name="minLength">Minimum length allowed for string. Defaults to 0.</param>
    /// <param name="maxLength">Maximum length allowed for string. Defaults to int.MaxValue.</param>
    /// <returns>
    /// Returns the <see cref="ModelValidator"/> so that methods can be chained in fluent style.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the supplied accessor is null.</exception>
    public ModelValidator ValidateNullable(Expression<Func<string?>> accessor, int minLength = 0, int maxLength = int.MaxValue) {
        if (accessor == null) throw new ArgumentNullException(nameof(accessor));
        
        string name = accessor.GetMemberName();
        Checks.Add(new ValidationCheck()
            .SetCondition(() => {
                string value = accessor.Compile().Invoke()!;
                return string.IsNullOrWhiteSpace(value) || value.Length >= minLength && value.Length <= maxLength;
            })
            .SetErrorMessage($"{name} cannot be empty and has to have a length between {minLength} and {maxLength}."));
        return this;
    }

    #endregion
    
}