using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.Exception;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CraftBackEnd.Extensions
{
    public static class GlobalErrorHandling
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger) {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null) {
                        var ex = contextFeature.Error;

                        if (ex.GetType() == typeof(ValidationErrorException)) {
                            logger.LogError($"User Error: {ex}");
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        } else {
                            logger.LogError($"Something went wrong: {contextFeature.Error}");
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        }

                        await context.Response.WriteAsync(new ErrorDetails() {
                            StatusCode = context.Response.StatusCode,
                            Message = ex.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
