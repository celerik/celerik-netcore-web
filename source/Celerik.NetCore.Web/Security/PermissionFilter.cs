using Microsoft.AspNetCore.Mvc.Filters;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Custom Filter to restrict access to an endpoint based on user permissions.
    /// </summary>
    public class PermissionFilter : IAuthorizationFilter
    {
        /// <summary>
        /// List of permissions that grant access to the endpoint.
        /// </summary>
        private readonly string[] _permissions;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="permissions">List of permissions that grant access to the endpoint.</param>
        public PermissionFilter(string[] permissions)
        {
            _permissions = permissions;
        }

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var action = context.ActionDescriptor.DisplayName;
            /*var userDataClaim = context.HttpContext?.User?.Claims?.FirstOrDefault(
                claim => claim.Type == UserClaims.USER_DATA
            );

            if (userDataClaim == null)
            {
                context.HttpContext.LogWarn(
                    $"Unauthorized call of '{action}'. Required permission(s): '{ToString()}'. The UserData claim is null"
                );
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = JsonConvert.DeserializeObject<UserDto>(userDataClaim.Value);
            if (user == null)
            {
                context.HttpContext.LogWarn(
                    $"Unauthorized call of '{action}'. Required permission(s): '{ToString()}'. The deserialized UserData claim is null"
                );
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasPermission = false;

            foreach (var permission in _permissions)
                if (user.Permissions.Contains(permission))
                {
                    hasPermission = true;
                    break;
                }

            if (!hasPermission)
            {
                context.HttpContext.LogWarn(
                    $"Unauthorized call of '{action}'. Required permission(s): '{ToString()}'. UserId: '{user.UserId}'"
                );
                context.Result = new UnauthorizedResult();
            }*/
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var toString = string.Join(',', _permissions);
            return toString;
        }
    }
}
