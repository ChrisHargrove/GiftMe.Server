using System.Net;

namespace Api.Validation;

/// <summary>
/// Basic validation check with default message and status of:
/// <para>
/// ErrorMessage - "Invalid Parameter!"
/// StatusCode - "BadRequest"
/// </para>
/// </summary>
public class ValidationCheck() : AbstractValidationCheck<ValidationCheck>(DefaultInvalidMessage, DefaultStatusCode) {
    private const string DefaultInvalidMessage = "Invalid Parameter!";
    private const HttpStatusCode DefaultStatusCode = HttpStatusCode.BadRequest;
}