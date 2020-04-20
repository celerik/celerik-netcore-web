using Microsoft.AspNetCore.Mvc;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Custom Attribute to authorize access to an endpoint based on User Permissions.
    /// </summary>
    public class PermissionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="permissions">List of permissions that grant access to the endpoint.
        /// E.g.: "AddUser".</param>
        public PermissionAttribute(params string[] permissions)
            : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { permissions };
        }
    }
}
