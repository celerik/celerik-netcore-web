using System;
using System.Diagnostics;
using System.Net;
using Celerik.NetCore.Services;
using Celerik.NetCore.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Handler to add global exception handling, specifically we do the following:
    ///     1. Log the exception
    ///     2. Writte a propper ApiResponse&lt;TData&gt; object.
    /// </summary>
    /// <code>
    ///     // Call this handler from the Startup.cs class:
    ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogService logSvc)
    ///     {
    ///         ...
    ///         app.UseExceptionsHandler(config, logSvc);
    ///     }
    /// </code>
    public static class ExceptionsHandler
    {
        /// <summary>
        /// Adds global exception habling by logging the exception and writting a
        /// propper ApiResponse&lt;TData&gt; object.
        /// </summary>
        /// <param name="app">Defines a class that provides the mechanisms to configure
        /// an application's request pipeline.</param>
        /// <param name="logSvc">Reference to the current ILogService intance.</param>
        public static void UseExceptionsHandler(this IApplicationBuilder app, ILogger logSvc)
        {
            logSvc.LogInformation($"Executing '{new StackTrace(0).GetMethodName()}'...");
            logSvc.LogInformation("Using global exception handler from the application's pipeline");

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var error = contextFeature?.Error?.ToString();
                    var formatedError = error?.Replace(Environment.NewLine, $"{Environment.NewLine}\t", StringComparison.InvariantCulture);

                    context.LogError(new HttpContextLoggerConfig
                    {
                        Message = "Internal Server Error",
                        IncludeCorrelationId = true,
                        IncludeRequestInfo = true,
                        Details = formatedError
                    });

                    /*await context.Response.WriteAsync(new ApiResponse<object>
                    {
                        Message = WebResources.Get("ExceptionsHandler.GlobalExceptionMsg"),
                        MessageType = ApiMessageType.Error,
                        Success = false
                    }.ToString());*/
                });
            });
        }
    }
}
