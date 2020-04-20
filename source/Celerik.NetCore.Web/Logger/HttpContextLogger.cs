using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Adds extension methods to the HttpContext class in order to log information.
    /// </summary>
    /// <code>
    ///     void SomeMethod(HttpContext context)
    ///     {
    ///         context.LogInfo("Something happened...");
    ///     }
    ///     
    ///     void SomeMethod(HttpContext context)
    ///     {
    ///         context.LogInfo(new HttpContextLog
    ///         {
    ///             Message = "Something happened...",
    ///             IncludeRequestInfo = true
    ///         });
    ///     }
    /// </code>
    public static class HttpContextLogger
    {
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="message">Log message.</param>
        public static void LogDebug(this HttpContext context, string message)
        {
            context.LogDebug(new HttpContextLoggerConfig { Message = message });
        }

        /// <summary>
        /// Writes the diagnostic message including useful HttpContext information at
        /// the Debug level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="config">Log configuration for the HttpContextLoger.</param>
        public static void LogDebug(this HttpContext context, HttpContextLoggerConfig config)
        {
            context.Log(LogLevel.Debug, config);
        }

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="message">Log message.</param>
        public static void LogInfo(this HttpContext context, string message)
        {
            context.LogInfo(new HttpContextLoggerConfig { Message = message });
        }

        /// <summary>
        /// Writes the diagnostic message including useful HttpContext information at
        /// the Info level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="config">Log configuration for the HttpContextLoger.</param>
        public static void LogInfo(this HttpContext context, HttpContextLoggerConfig config)
        {
            context.Log(LogLevel.Information, config);
        }

        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="message">Log message.</param>
        public static void LogWarn(this HttpContext context, string message)
        {
            context.LogWarn(new HttpContextLoggerConfig { Message = message });
        }

        /// <summary>
        /// Writes the diagnostic message including useful HttpContext information at
        /// the Warn level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="config">Log configuration for the HttpContextLoger.</param>
        public static void LogWarn(this HttpContext context, HttpContextLoggerConfig config)
        {
            context.Log(LogLevel.Warning, config);
        }

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="message">Log message.</param>
        public static void LogError(this HttpContext context, string message)
        {
            context.LogError(new HttpContextLoggerConfig { Message = message });
        }

        /// <summary>
        /// Writes the diagnostic message including useful HttpContext information at
        /// the Error level.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="config">Log configuration for the HttpContextLoger.</param>
        public static void LogError(this HttpContext context, HttpContextLoggerConfig config)
        {
            context.Log(LogLevel.Error, config);
        }

        /// <summary>
        /// Writes the diagnostic message including useful HttpContext information. Any exception
        /// thrown by this operation will be hidden in order to avoid breaking the Http Pipeline.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <param name="level">Log level.</param>
        /// <param name="config">Log configuration for the HttpContextLoger.</param>
        private static void Log(this HttpContext context, LogLevel level, HttpContextLoggerConfig config)
        {
            try
            {
                var logger = context.RequestServices.GetRequiredService<ILogger>();
                /*
                if (config.IncludeCorrelationId)
                    details.Add("CorrelationId", context.TraceIdentifier);

                if (config.IncludeRequestInfo)
                {
                    details.Add("Url", context.Request?.Path);
                    details.Add("Method", context.Request?.Method);
                    details.Add("Scheme", context.Request?.Scheme);
                    details.Add("Headers", context.Request?.Headers, jsonify: true);
                    details.Add("QueryString", context.Request?.QueryString, jsonify: true);
                    details.Add("Body", context.ReadBody(), jsonify: true);
                    details.Add("Origin", context.Request?.Headers["Origin"]);
                    details.Add("User", context.User?.Identity?.Name);
                }

                if (config.IsUnhandledException)
                    details.Add("StatusCode", (int)HttpStatusCode.InternalServerError);
                else if (config.IncludeResponseInfo)
                    details.Add("StatusCode", context.Response?.StatusCode);

                if (config.Details != null)
                    details.Add("Details", config.Details, config.Jsonify);
                */
                logger.Log(level, config.Message);
            }
            catch { }
        }

        /// <summary>
        /// Reads the request body of this HttpContext. Any exception thrown
        /// by the reader will be hidden in order to avoid breaking the Http Pipeline.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <returns>Request body as a JObject.</returns>
        private static JObject ReadBody(this HttpContext context)
        {
            try
            {
                using var reader = new StreamReader(context.Request.Body);
                context.Request.Body.Position = 0;
                return JObject.Parse(reader.ReadToEnd());
            }
            catch
            {
                return null;
            }
        }
    }
}
