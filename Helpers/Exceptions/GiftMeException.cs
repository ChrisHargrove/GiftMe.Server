using Helpers.Http;

namespace Helpers.Exceptions;

/// <summary>
/// Base class for all user facing exceptions that get thrown.
/// </summary>
public class GiftMeException : Exception {
    protected GiftMeException() : base() {
    }

    /// <summary>
    /// Constructs a new GiftMeException with the supplied message.
    /// </summary>
    /// <param name="message">The message to be in the exception.</param>
    public GiftMeException(string message) : base(message) {
    }

    /// <summary>
    /// Constructs a new GiftMeException with the supplied message and errorCode.
    /// </summary>
    /// <param name="message">The message to be in the exception.</param>
    /// <param name="errorCode">The error code to denote what has gone wrong.</param>
    public GiftMeException(string message, GiftMeErrorCodes errorCode) : base(message) {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Constructs a new GiftMeException with the supplied message and inner exception..
    /// </summary>
    /// <param name="message">The message to be in the exception.</param>
    /// <param name="innerException">The inner exception that triggered this exception.</param>
    protected GiftMeException(string message, Exception innerException) : base(message, innerException) {
    }
    
    /// <summary>
    /// The error code of the Exception.
    /// </summary>
    public GiftMeErrorCodes ErrorCode { get; protected set; }
}