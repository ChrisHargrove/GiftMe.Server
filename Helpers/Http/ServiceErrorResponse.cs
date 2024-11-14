using System.Net;
using System.Runtime.Serialization;
using Helpers.Exceptions;

namespace Helpers.Http;

/// <summary>
/// Default JSON error response that the server will send in the case of an exception.
/// </summary>
/// <param name="responseException"></param>
[DataContract]
public class ServiceErrorResponse(HttpResponseException responseException) {
    [DataMember(EmitDefaultValue = false)]
    public HttpStatusCode StatusCode { get; } = responseException.StatusCode;
    [DataMember(EmitDefaultValue = false)]
    public string? Message { get; } = responseException.Message;
    [DataMember(EmitDefaultValue = false)]
    public ServiceErrorCode ErrorCode { get; } = responseException.ErrorCode;
}