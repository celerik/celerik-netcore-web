using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Middleware to process OPTIONS requests when CORS is enabled.
    /// 
    /// This middleware is necessary because POST request don´t work even
    /// with CORS enabled. This is because browsers send an OPTIONS request before
    /// the POST request which is not proccesed by the Api.
    /// 
    /// This middleware is added by the CorsHandler class, so you don´t have to
    /// worry for adding it from the Startup.cs class.
    /// </summary>
    /// <code>
    ///     void SomeMethod(IApplicationBuilder app)
    ///     {
    ///         app.UseMiddleware&lt;CorsOptionsMiddleware&gt;();
    ///     }
    /// </code>
    public class CorsOptionsMiddleware
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
        public CorsOptionsMiddleware(RequestDelegate next, IHostingEnvironment environment)
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
            if (context.Request.Method == "OPTIONS")
                await ProcessOptionsRequest(context);
            else
                await _next.Invoke(context);
        }

        /// <summary>
        /// Process an OPTIONS request by allowing CORS for the incoming origin.
        /// </summary>
        /// <param name="context">Object with all HTTP-specific information.</param>
        private async Task ProcessOptionsRequest(HttpContext context)
        {
            context.LogDebug("Processing an OPTIONS request, adding response headers");

            context.Response.Headers.Add("Access-Control-Allow-Origin",
                new[] { (string)context.Request.Headers["Origin"] });
            context.Response.Headers.Add("Access-Control-Allow-Headers",
                new[] { "Origin, X-Requested-With, Content-Type, Accept, Authorization" });
            context.Response.Headers.Add("Access-Control-Allow-Methods",
                new[] { "DELETE, GET, OPTIONS, PATCH, POST, PUT" });
            context.Response.Headers.Add("Access-Control-Allow-Credentials",
                new[] { "true" });

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync(HttpStatusCode.OK.ToString());
        }
    }
}
