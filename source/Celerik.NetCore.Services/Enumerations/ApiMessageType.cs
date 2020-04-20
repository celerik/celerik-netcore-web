using System.ComponentModel;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Defines the possible types of messages that the API can
    /// send in the response.
    /// </summary>
    public enum ApiMessageType
    {
        /// <summary>
        /// The process ran successfully and some contextual
        /// information is sent.
        /// </summary>
        [Description("info")]
        Info = 1,

        /// <summary>
        /// The process ran successfully.
        /// </summary>
        [Description("success")]
        Success = 2,

        /// <summary>
        /// The process ran successfully but there is a warning
        /// message.
        /// </summary>
        [Description("warning")]
        Warning = 3,

        /// <summary>
        /// There was an error executing the process.
        /// </summary>
        [Description("error")]
        Error = 4
    }
}
