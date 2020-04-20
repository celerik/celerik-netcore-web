using Celerik.NetCore.Util;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Adds some extensions methods related to the Swagger functionality.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Gets the Swagger configuration stored into the passed-in IConfiguration object.
        /// 
        /// By convention, Swagger configuration properties are get from the IConfiguration object
        /// in the following way:
        ///     - IsEnabled: Swagger:IsEnabled
        ///     - ApiVersion: Swagger:ApiVersion
        ///     - ApiName: Swagger:ApiName
        ///     - JsonEndpoint: Swagger:JsonEndpoint
        /// </summary>
        /// <param name="config">The configuration object where we get the Swagger configuration.
        /// </param>
        /// <returns>Swagger configuration stored into the passed-in IConfiguration object.</returns>
        public static SwaggerConfig GetSwaggerConfig(this IConfiguration config)
        {
            KeyValuePair<string, string> get(string key) =>
                new KeyValuePair<string, string>(key, config[key]);

            var map = new
            {
                Enabled = get("Swagger:IsEnabled"),
                Version = get("Swagger:ApiVersion"),
                Name = get("Swagger:ApiName"),
                Json = get("Swagger:JsonEndpoint"),
            };

            var swager = new SwaggerConfig
            {
                IsEnabled = map.Enabled.Value.IsValidBool() ? bool.Parse(map.Enabled.Value) : false,
                ApiVersion = map.Version.Value,
                ApiName = map.Name.Value,
                JsonEndpoint = map.Json.Value
            };

            if (!string.IsNullOrEmpty(map.Enabled.Value) && !map.Enabled.Value.IsValidBool())
                throw new ConfigException($"Invalid '{map.Enabled.Key}': '{map.Enabled.Value}'");
            if (swager.IsEnabled && string.IsNullOrEmpty(map.Version.Value))
                throw new ConfigException($"Missing '{map.Version.Key}' value");
            if (swager.IsEnabled && string.IsNullOrEmpty(map.Name.Value))
                throw new ConfigException($"Missing '{map.Name.Key}' value");
            if (swager.IsEnabled && string.IsNullOrEmpty(map.Json.Value))
                throw new ConfigException($"Missing '{map.Json.Key}' value");

            return swager;
        }
    }
}
