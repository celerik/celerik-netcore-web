namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Defines the configuration needed to add core services.
    /// </summary>
    public class ApiConfig
    {
        /// <summary>
        /// The relative path under application root where resource
        /// files are located.
        /// </summary>
        public string ResourcesPath { get; set; }
            = "Resources";

        /// <summary>
        /// Format string used to format timestamp in logging messages.
        /// </summary>
        public string LoggingTimestampFormat { get; set; }
            = "[yyyy-MM-dd HH:mm:ss]";

        /// <summary>
        /// The key in the config file where we get the name of the
        /// SqlServer connection string. If the key is defined in the
        /// config file, a SqlServer DbContext will be added to the 
        /// service collection. To resolve the connection string value,
        /// the name is searched in the environment variables, or in
        /// the config file. This configuration only applies when the
        /// service type is ApiServiceType.ServiceEF.
        /// </summary>
        public string SqlServerConnectionStringKey { get; set; }
            = "SqlServerConnectionString";
    }
}
