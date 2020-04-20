namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Defines Swagger configuration for the current environment.
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Indicates whether Swagger is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Swagger Api version.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Swagger Api name.
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// Swagger Json endpoint.
        /// </summary>
        public string JsonEndpoint { get; set; }
    }
}
