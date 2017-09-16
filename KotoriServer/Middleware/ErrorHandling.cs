using KotoriServer.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KotoriServer.Middleware
{
    /// <summary>
    /// Error handling middleware.
    /// </summary>
    public class ErrorHandling
    {
        readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Middleware.ErrorHandling"/> class.
        /// </summary>
        /// <param name="next">Next request.</param>
        public ErrorHandling(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke the specified context.
        /// </summary>
        /// <param name="context">Context.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="exception">Exception.</param>
        static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is KotoriAuthException kae)
            {
                if (kae.EmptyKey)
                    code = HttpStatusCode.Unauthorized;
                else
                    code = HttpStatusCode.Forbidden;
            }
                        
            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}

