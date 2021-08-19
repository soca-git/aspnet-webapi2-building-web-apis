using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ExploreCalifornia.Loggers
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        // The ExceptionLogger.Log() is called when an exception occurs in the application.
        // There can only be a single ExceptionLogger class per application.
        // This class is intended for logging exceptions only, it does not return any values.
        // The 'context' parameter includes the exception that occurred and the current request.
        // HttpResponseExceptions do not trigger the ExceptionLogger, since they are not pure exceptions,
        // but serve as a method to return error information to the client from Web API.
        public override void Log(ExceptionLoggerContext context)
        {
            var logMessage = context.Exception.Message;
            Debug.WriteLine(logMessage);
        }
    }
}
