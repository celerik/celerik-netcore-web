namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Defines CORS configuration for the current environment.
    /// </summary>
    public class CorsConfig
    {
        /// <summary>
        /// The CORS Policy to be applied.
        /// </summary>
        public CorsPolicy Policy { get; set; }

        /// <summary>
        /// List of Allowed Origins, when the policy is: AllowSpecificOrigins.
        /// </summary>
        public string[] Origins { get; set; }
    }
}
