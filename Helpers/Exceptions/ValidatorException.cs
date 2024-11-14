using System.Net;
using System.Text;

namespace Helpers.Exceptions;

/// <summary>
/// Exception used during model validation.
/// </summary>
public class ValidatorException : HttpResponseException {
    /// <summary>
    /// Constructs a new exception with given messages and a final HttpStatusCode.
    /// </summary>
    /// <param name="errorMessages">A List of errors that occured during validation.</param>
    /// <param name="statusCode">The final HttpStatusCode of the validation.</param>
    public ValidatorException(List<string> errorMessages, HttpStatusCode statusCode) : base(statusCode) {
        ErrorMessages = errorMessages;
    }
    
    /// <summary>
    /// Constructs a new exception with given messages and a final HttpStatusCode.
    /// </summary>
    /// <param name="errorMessage">An error that occured during validation.</param>
    /// <param name="statusCode">The final HttpStatusCode of the validation.</param>
    public ValidatorException(string errorMessage, HttpStatusCode statusCode) : this([errorMessage], statusCode) {
    }

    /// <summary>
    /// Constructs a new exception with given messages and a final HttpStatusCode, as well as a inner exception.
    /// </summary>
    /// <param name="errorMessage">An errors that occured during validation.</param>
    /// <param name="statusCode">The final ttpStatusCode of the validation.</param>
    /// <param name="innerException">The inner exception that triggered this exception.</param>
    public ValidatorException(string errorMessage, HttpStatusCode statusCode, Exception innerException) : this([errorMessage], statusCode, innerException){
    }

    /// <summary>
    /// Constructs a new exception with given messages and a final HttpStatusCode, as well as a inner exception.
    /// </summary>
    /// <param name="errorMessages">A list of errors that occured during validation.</param>
    /// <param name="statusCode">The final HttpStatusCode of the validation.</param>
    /// <param name="innerException">The inner exception that triggered this exception.</param>
    public ValidatorException(List<string> errorMessages, HttpStatusCode statusCode, Exception innerException) : base(statusCode, string.Empty, innerException) {
        ErrorMessages = errorMessages;
    }
    
    /// <summary>
    /// The list of error messages that occured in this exception.
    /// </summary>
    public List<string> ErrorMessages { get; }
    
    /// <summary>
    /// Converts the exception into a correctly formatted message.
    /// </summary>
    public override string Message {
        get {
            StringBuilder builder = new StringBuilder("Validator found following issues:\n");
            ErrorMessages.ForEach(m => builder.AppendLine(m));
            builder.AppendLine(base.Message);
            return builder.ToString();
        }
    }
}