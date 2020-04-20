using System.Diagnostics;
using Celerik.NetCore.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Handler to enable Swagger. Configuration is read from the appsettings.json file.
    /// </summary>
    /// <example>
    ///     appsettings.json:
    ///     "Swagger": {
    ///         "ApiName": "My Awesome API",
    ///         "ApiVersion": "1.0",
    ///         "IsEnabled": true,
    ///         "JsonEndpoint": "/swagger/v1/swagger.json"
    ///     }
    /// </example>
    /// <code>
    ///     // Call this handler from the Startup.cs class:
    ///     public void ConfigureServices(IServiceCollection services)
    ///     {
    ///         ...
    ///         services.AddSwagger(config, logSvc);
    ///     }
    ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogService logSvc)
    ///     {
    ///         ...
    ///         app.UseSwagger(config, logSvc);
    ///     }
    /// </code>
    public static class SwaggerHandler
    {
        /// <summary>
        /// Adds Swagger services to this IServiceCollection.
        /// 
        /// By convention, Swagger configuration properties are get from the IConfiguration object
        /// in the following way:
        ///     - IsEnabled: Swagger:IsEnabled
        ///     - ApiVersion: Swagger:ApiVersion
        ///     - ApiName: Swagger:ApiName
        ///     - JsonEndpoint: Swagger:JsonEndpoint
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="config">Object to access application configuration properties.</param>
        /// <param name="logSvc">Reference to the current ILogService instance.</param>
        public static void AddSwagger(this IServiceCollection services, IConfiguration config, ILogger logSvc)
        {
            logSvc.LogInformation($"Executing '{new StackTrace(0).GetMethodName()}'...");

            logSvc.LogInformation("Reading Swagger configuration");
            var swagger = config.GetSwaggerConfig();

            if (swagger.IsEnabled)
            {
                logSvc.LogInformation("Adding Swagger generation options to service collection");
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(swagger.ApiVersion,
                        new OpenApiInfo
                        {
                            Title = swagger.ApiName,
                            Version = swagger.ApiVersion
                        }
                    );

                    options.OperationFilter<SwaggerOperationFilter>();
                });
            }
            else
                logSvc.LogInformation("Swagger is disabled");
        }

        /// <summary>
        /// Use Swagger in case it is enabled from the appsettings.json file.
        /// 
        /// By convention, Swagger configuration properties are get from the IConfiguration object
        /// in the following way:
        ///     - IsEnabled: Swagger:IsEnabled
        ///     - ApiVersion: Swagger:ApiVersion
        ///     - ApiName: Swagger:ApiName
        ///     - JsonEndpoint: Swagger:JsonEndpoint
        /// </summary>
        /// <param name="app">Object to configure the application's request pipeline.</param>
        /// <param name="config">Object to access application configuration properties.</param>
        /// <param name="logSvc">Reference to the current ILogService instance.</param>
        public static void UseSwagger(this IApplicationBuilder app, IConfiguration config, ILogger logSvc)
        {
            logSvc.LogInformation($"Executing '{new StackTrace(0).GetMethodName()}'...");

            logSvc.LogInformation("Reading Swagger configuration");
            var swagger = config.GetSwaggerConfig();

            if (swagger.IsEnabled)
            {
                logSvc.LogInformation("Using Swagger from the application's pipeline");
                app.UseSwagger();

                logSvc.LogInformation("Using Swagger UI from the application's pipeline");
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint(swagger.JsonEndpoint, swagger.ApiName);
                });
            }
            else
                logSvc.LogInformation("Swagger is disabled");
        }
    }
}
