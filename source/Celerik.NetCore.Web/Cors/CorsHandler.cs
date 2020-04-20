using System.Diagnostics;
using Celerik.NetCore.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Handler to enable CORS. Policies are read from the appsettings.json file.
    /// </summary>
    /// <example>
    ///     appsettings.json, enabling CORS for specific origins:
    ///     "Cors": {
    ///         "PolicyName": "AllowSpecificOrigins",
    ///         "Origins": "https://origin1.com,https://origin2.com,"
    ///     }
    /// 
    ///     appsettings.json, enabling CORS for any origin:
    ///     "Cors": {
    ///         "PolicyName": "AllowAnyOrigin"
    ///     }
    /// </example>
    /// <code>
    ///     // Call this handler from the Startup.cs class:
    ///     public void ConfigureServices(IServiceCollection services)
    ///     {
    ///         ...
    ///         // Add this call before services.AddMvc()
    ///         services.AddCors(config, logSvc);
    ///     }
    ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogService logSvc)
    ///     {
    ///         ...
    ///         // Add this call before app.UseMvc()
    ///         app.UseCors(config, logSvc);
    ///     }
    /// </code>
    public static class CorsHandler
    {
        /// <summary>
        /// Adds cross-origin resource sharing services to this IServiceCollection.
        /// 
        /// By convention, CORS configuration properties are get from the IConfiguration object
        /// in the following way:
        ///     - Cors:PolicyName
        ///     - Cors:Origins
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="config">Object to access application configuration properties.</param>
        /// <param name="logSvc">Reference to the current ILogService instance.</param>
        public static void AddCors(this IServiceCollection services, IConfiguration config, ILogger logSvc)
        {
            logSvc.LogInformation($"Executing '{new StackTrace(0).GetMethodName()}'...");

            logSvc.LogInformation("Reading CORS configuration");
            var cors = config.GetCorsConfig();

            if (cors.Policy == CorsPolicy.AllowAnyOrigin)
            {
                logSvc.LogInformation($"Adding CORS policiy '{cors.Policy}' to service collection");
                services.AddCors(options =>
                {
                    options.AddPolicy(cors.Policy.ToString(),
                        builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                    );
                });
            }
            else if (cors.Policy == CorsPolicy.AllowSpecificOrigins)
            {
                logSvc.LogInformation($"Adding CORS policiy '{cors.Policy}' to service collection");
                services.AddCors(options =>
                {
                    options.AddPolicy(cors.Policy.ToString(),
                        builder => builder
                            .WithOrigins(cors.Origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                    );
                });
            }
            else
                logSvc.LogInformation("CORS is disabled");
        }

        /// <summary>
        /// Adds a CORS middleware to the web application pipeline to allow cross domain requests.
        /// 
        /// By convention, CORS configuration properties are get from the IConfiguration object
        /// in the following way:
        ///     - Cors:PolicyName
        ///     - Cors:Origins
        /// </summary>
        /// <param name="app">Object to configure the application's request pipeline.</param>
        /// <param name="config">Object to access application configuration properties.</param>
        /// <param name="logSvc">Reference to the current ILogService instance.</param>
        public static void UseCors(this IApplicationBuilder app, IConfiguration config, ILogger logSvc)
        {
            logSvc.LogInformation($"Executing '{new StackTrace(0).GetMethodName()}'...");

            logSvc.LogInformation("Reading CORS configuration");
            var cors = config.GetCorsConfig();

            if (cors.Policy != CorsPolicy.Disabled)
            {
                logSvc.LogInformation("Using CorsOptionsMiddleware from the application's pipeline");
                app.UseMiddleware<CorsOptionsMiddleware>();

                logSvc.LogInformation($"Using CORS policy: '{cors.Policy}' from the application's pipeline");
                app.UseCors(cors.Policy.ToString());
            }
            else
                logSvc.LogInformation("CORS is disabled");
        }
    }
}
