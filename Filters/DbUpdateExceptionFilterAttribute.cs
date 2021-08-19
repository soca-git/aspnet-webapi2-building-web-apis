using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace ExploreCalifornia.Filters
{
    public class DbUpdateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // If the exception is a DbUpdateException.
            if (context.Exception is DbUpdateException)
            {
                // '?' -> nullable object; if object does not exists, assigns sqlException to null.
                SqlException sqlException = context.Exception?.InnerException?.InnerException as SqlException;

                // SQL Server docs: Unique constraint violation.
                if (sqlException?.Number == 2627)
                {
                    // Throw an appropriate HttpResponseException (409 Conflict).
                    context.Response = new HttpResponseMessage(HttpStatusCode.Conflict);
                }
                else
                {
                    // Throw an internal server error (500).
                    context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
        }
    }
}
