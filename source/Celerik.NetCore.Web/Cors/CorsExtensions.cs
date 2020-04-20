using System;
using System.Collections.Generic;
using Celerik.NetCore.Util;
using Microsoft.Extensions.Configuration;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Adds some extension methods related to the Cors functionality.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Gets the CORS configuration stored into the passed-in IConfiguration object.
        /// 
        /// By convention, CORS configuration properties are get from the IConfiguration object
        /// in the following way:
        ///     - Policy: Cors:PolicyName
        ///     - Origins: Cors:Origins
        /// </summary>
        /// <param name="config">The configuration object where we get the CORS configuration.
        /// </param>
        /// <returns>CORS configuration stored into the passed-in IConfiguration object.</returns>
        public static CorsConfig GetCorsConfig(this IConfiguration config)
        {
            KeyValuePair<string, string> get(string key) =>
                new KeyValuePair<string, string>(key, config[key]);

            var map = new
            {
                Policy = get("Cors:PolicyName"),
                Origins = get("Cors:Origins")
            };

            var cors = new CorsConfig
            {
                Policy = EnumUtility.GetValueFromDescription(map.Policy.Value, CorsPolicy.Disabled),
                Origins = map.Origins.Value?.Split(",") ?? Array.Empty<string>()
            };

            if (!string.IsNullOrEmpty(map.Policy.Value) &&
                EnumUtility.GetValueFromDescription<CorsPolicy>(map.Policy.Value) == 0)
                throw new ConfigException($"Invalid '{map.Policy.Key}': '{map.Policy.Value}'");

            if (cors.Policy == CorsPolicy.AllowSpecificOrigins && cors.Origins.Length == 0)
                throw new ConfigException($"Missing '{map.Origins.Key}' value");

            if (cors.Policy == CorsPolicy.AllowSpecificOrigins)
                foreach (var origin in cors.Origins)
                    if (!origin.IsValidUrl())
                        throw new ConfigException($"Invalid '{map.Origins.Key}': '{map.Origins.Value}'");

            return cors;
        }
    }
}
