using System.Diagnostics.CodeAnalysis;
using Celerik.NetCore.Util;
using Newtonsoft.Json;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Defines the standarized response for all services.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Optional message in case something happened during the service execution.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Describes the type of message.
        /// </summary>
        [JsonConverter(typeof(EnumDescriptionJsonConverter))]
        public ApiMessageType? MessageType { get; set; }

        /// <summary>
        /// Indicates whether the service was successfully executed.
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// Defines the standarized response for all services.
    /// </summary>
    /// <typeparam name="TData">Type of the Data property.</typeparam>
    public class ApiResponse<TData>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="data">Data sent as the response.</param>
        public ApiResponse(TData data = default)
            => Data = data;

        /// <summary>
        /// Data sent as the response.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Optional message in case something happened during the service execution.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Describes the type of message.
        /// </summary>
        [JsonConverter(typeof(EnumDescriptionJsonConverter))]
        public ApiMessageType? MessageType { get; set; }

        /// <summary>
        /// Indicates whether the service was successfully executed.
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Returns a JSON string that represents the current object.
        /// </summary>
        /// <returns>JSON string that represents the current object.</returns>
        public override string ToString() => JsonConvert.SerializeObject(this);

        /// <summary>
        /// Converts an ApiResponse to an ApiResponse&lt;TData&gt;.
        /// </summary>
        /// <param name="response">The object to cast.</param>
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Implicit operators should not throw exceptions")]
        public static implicit operator ApiResponse<TData>(ApiResponse response)
            => new ApiResponse<TData>
            {
                Data = default,
                Message = response.Message,
                MessageType = response.MessageType,
                Success = response.Success
            };
    }
}
