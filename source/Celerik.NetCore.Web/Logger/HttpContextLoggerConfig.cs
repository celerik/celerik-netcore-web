namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Defines log configuration for the HttpContextLoger.
    /// </summary>
    public class HttpContextLoggerConfig
    {
        /// <summary>
        /// Log message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Indicates whether CorrelationId should be included.
        /// </summary>
        public bool IncludeCorrelationId { get; set; }

        /// <summary>
        /// Indicates whether Request information should be included.
        /// </summary>
        public bool IncludeRequestInfo { get; set; }

        /// <summary>
        /// Indicates whether Response information should be included.
        /// </summary>
        public bool IncludeResponseInfo { get; set; }

        /// <summary>
        /// Indicates whether the request ended with an unhandled exception.
        /// </summary>
        public bool IsUnhandledException { get; set; }

        /// <summary>
        /// Optional details. E.g: An exception object.
        /// </summary>
        public object Details { get; set; }

        /// <summary>
        /// Indicates whether Details should be serialized to JSON.
        /// </summary>
        public bool Jsonify { get; set; }
    }
}
