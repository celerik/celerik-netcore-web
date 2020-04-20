using System.ComponentModel;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Defines possible implementations of the service.
    /// </summary>
    public enum ApiServiceType
    {
        /// <summary>
        /// The service is implemented using Entity Framework against a
        /// datasource.
        /// </summary>
        [Description("ServiceEF")]
        ServiceEF = 1,

        /// <summary>
        /// The service is implemented using a HttpClient.
        /// </summary>
        [Description("ServiceHttp")]
        ServiceHttp = 2,

        /// <summary>
        /// The service is implemented using mock data.
        /// </summary>
        [Description("ServiceMock")]
        ServiceMock = 3
    }
}
