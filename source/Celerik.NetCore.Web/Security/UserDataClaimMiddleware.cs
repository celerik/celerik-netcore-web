using System.Linq;
using System.Threading.Tasks;
using Celerik.NetCore.Services;
using Celerik.NetCore.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Middleware to include the data of the current authenticated user
    /// into the user claims. This data is a UserDto object which contains
    /// useful information such as the user permissions.
    /// 
    /// When we receive the Bearer token in the Authentication header,
    /// this token contains a claim with the UserId, so we use that id
    /// in order to retrieve the full information of the user.
    /// 
    /// This middleware is necessary because we only trust in the UserId
    /// comming from the Authentication header, but the rest of information
    /// of the user can change during the token lifecycle, so it is necesary
    /// to get a fresh copy of the user information for each request.
    /// </summary>
    /// <code>
    ///     // Add this middleware from the Startup.cs class
    ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    ///     {
    ///         app.UseMiddleware&lt;UserDataClaimMiddleware&gt;();
    ///     }
    /// </code>
    public class UserDataClaimMiddleware
    {
        /// <summary>
        ///  The object that can process the HTTP request.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Provides information about the web hosting environment.
        /// </summary>
        private readonly IHostingEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="next">The object that can process the HTTP request.</param>
        /// <param name="environment">Provides information about the web hosting environment.</param>
        public UserDataClaimMiddleware(RequestDelegate next, IHostingEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        /// <summary>
        /// Process the request for this middleware.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        /// <returns>The task result.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
                await ProcessAuthenticatedRequest(context);

            await _next.Invoke(context);
        }

        /// <summary>
        /// Process an authenticated request by adding the User Data into the
        /// user claims.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        private async Task ProcessAuthenticatedRequest(HttpContext context)
        {
            context.LogDebug("Processing an authenticated request");
            await Task.FromResult(0);
            /*
            var userIdClaim = context.User.Claims?.FirstOrDefault(claim => claim.Type == UserClaims.USER_ID);
            if (userIdClaim == null)
            {
                context.LogDebug("The UserId claim is null");
                return;
            }

            var userId = userIdClaim.Value?.ToInt();
            if (userId == 0)
            {
                context.LogDebug($"The UserId claim is invalid: '{userIdClaim.Value}'");
                return;
            }
            
            var securityService = context.RequestServices.GetRequiredService<ISecurityService>();
            var userSearch = new SearchUsers { UserId = userId };
            var userData = (await securityService.SearchUsersAsync(userSearch)).FirstOrDefault();
            if (userData == null)
            {
                context.LogDebug($"There is no user matching the UserId: '{userId}'");
                return;
            }

            var userJson = JsonConvert.SerializeObject(userData);
            var userClaims = new List<Claim> { new Claim(UserClaims.USER_DATA, userJson) };
            var userIdentity = new ClaimsIdentity(userClaims);

            context.User.AddIdentity(userIdentity);
            context.LogDebug("Added the UserData claim");
            */
        }
    }
}
