using System.Net;
using Helpers.Http;

namespace Helpers.Exceptions;

/// <summary>
/// Represents the base type of all exceptions in the service.
/// The default exception constructors assume that its a 500 Internal Server Error with no specific reason ServiceErrorCode
/// Generally it would be advised to use the constructors where you specify a StatusCode and ErrorCode. 
/// </summary>
[Serializable]
public class HttpResponseException : Exception {
    public HttpResponseException() : this(HttpStatusCode.InternalServerError) { }

    public HttpResponseException(HttpStatusCode statusCode, ServiceErrorCode errorCode = ServiceErrorCode.None) {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public HttpResponseException(string? message) : this(HttpStatusCode.InternalServerError, message) { }

    public HttpResponseException(HttpStatusCode statusCode, string? message, ServiceErrorCode errorCode = ServiceErrorCode.None) : base(message) {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public HttpResponseException(string? message, Exception? innerException) : this(HttpStatusCode.InternalServerError, message, innerException) { }

    public HttpResponseException(HttpStatusCode statusCode, string? message, Exception? innerException, ServiceErrorCode errorCode = ServiceErrorCode.None) : base(message, innerException) {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
    
    /// <summary>
    /// The Http Response to return from this exception.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    /// <summary>
    /// The service specific ErrorCode for what went wrong.
    /// </summary>
    public ServiceErrorCode ErrorCode { get; }
}