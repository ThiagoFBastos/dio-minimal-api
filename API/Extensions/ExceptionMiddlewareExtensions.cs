using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;
using API.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        ErrorDetails errorDetails = new ErrorDetails
                        {
                            Status = context.Response.StatusCode,
                            Title = contextFeature.Error.Message,
                            TraceId = context.TraceIdentifier
                        };

                        errorDetails.Type = errorDetails.Status switch
                        {
                            404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                            _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
                        };

                        await context.Response.WriteAsync(errorDetails.ToString());
                    }
                });
            });
        }
    }
}