using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using KotoriCore.Exceptions;
using KotoriServer.Tokens;
using Newtonsoft.Json;

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
            else if (exception is KotoriException kotori)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)kotori.StatusCode;

                var error = new ErrorResult
                {
                    Message = exception.Message,
                    Type = exception.GetType().ToString()
                };

                if (exception is KotoriProjectException kpe)
                    error.Id = kpe.Identifier;

                if (exception is KotoriDocumentException kde)
                    error.Id = kde.Identifier;

                if (exception is KotoriDocumentTypeException kdte)
                    error.Id = kdte.Identifier;

                return context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
                        
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var error2 = new ErrorResult
            {
                Message = exception.Message,
                Type = exception.GetType().ToString()
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(error2));
        }
    }
}

