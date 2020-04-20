using System.ComponentModel;

namespace Celerik.NetCore.Web
{
    public enum CorsPolicy
    {
        [Description("Disabled")]
        Disabled = 1,

        [Description("AllowSpecificOrigins")]
        AllowSpecificOrigins = 2,

        [Description("AllowAnyOrigin")]
        AllowAnyOrigin = 3
    }
}
