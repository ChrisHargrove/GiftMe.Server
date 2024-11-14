using System.Net;
using Helpers.Exceptions;
using Helpers.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.ErrorHandling;

/// <summary>
/// Class that handles the rewriting of all purposefully thrown exceptions related to HttpResponses.
/// This way we can choose what information makes it back to the end user.
/// </summary>
public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter {
    public void OnActionExecuting(ActionExecutingContext context) {
    }

    /// <summary>
    /// In this method we check to see if the context contains an Exception if it does and it is of type
    /// HttpResponseException we convert it into a ServerErrorResponse Object and send it back under
    /// the status code of the exception.
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context) {
        if (context.Exception is HttpResponseException httpResponseException) {
            context.Result = new JsonResult(new ServiceErrorResponse(httpResponseException)) {
                StatusCode = (Int32)httpResponseException.StatusCode
            };
            context.ExceptionHandled = true;
        }

        //TODO: This probably needs some cleanup as its kinda just silences a bunch of stuff during errors.
        if (context.Exception is HttpRequestException httpRequestException) {
            HttpResponseException reponseException = new HttpResponseException(httpRequestException.StatusCode ?? HttpStatusCode.InternalServerError);
            context.Result = new JsonResult(new ServiceErrorResponse(reponseException)) {
                StatusCode = (Int32)reponseException.StatusCode
            };
            context.ExceptionHandled = true;
        }

        if (context.Exception != null) {
            Console.Write(context.Exception.Message);
        }
    }

    public int Order { get; } = Int32.MaxValue - 10;
}