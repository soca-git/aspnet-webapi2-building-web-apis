using ExploreCalifornia.HttpActionResults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ExploreCalifornia.ExceptionHandlers
{
    public class UnhandledExceptionHandler : ExceptionHandler
    {
        // The ExceptionLogger.Handle() is called when an exception occurs in the application.
        // There can only be a single ExceptionHandler class per application.
        // The 'context' parameter includes the exception that occurred and the current request & response.
        // The ExceptionHandler should throw a HttpResponseMessage().
        public override void Handle(ExceptionHandlerContext context)
        {
// Compiler directives.
// If debugging is enabled.
#if DEBUG
            // Include exception details in response.
            string messageContent = JsonConvert.SerializeObject(context.Exception);
// Else, application in production.
#else
            // Don't expose exception details in response, just provide a generic message.
            string messageContent = @"{ ""message"": ""Woops! Something went wrong!"" }";
#endif
            // ErrorContentResult implements IHttpActionResult and returns a HttpResponseMessage object.
            context.Result = new ErrorContentResult(messageContent, "application/json", context.Request);
        }
    }
}
