using Microsoft.AspNetCore.Mvc.Filters;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Filter to log the lifetime of a controller action. We log the begining
    /// and end of the execution.
    /// </summary>
    /// <code>
    ///     // Add this filter from the Startup.cs class:
    ///     public void ConfigureServices(IServiceCollection services)
    ///     {
    ///         services.AddMvc(config => {
    ///             config.Filters.Add(typeof(LogRequestFilter));
    ///         });
    ///     }
    /// </code>
    public class LogRequestFilter : IActionFilter
    {
        /// <summary>
        /// Called before the action executes, after model binding is completed.
        /// </summary>
        /// <param name="context">The context of the action filter.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.LogInfo(new HttpContextLoggerConfig
            {
                Message = $"{context.HttpContext.Request.Path} Start",
                IncludeRequestInfo = true,
            });
        }

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The context of the action filter.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.LogInfo(new HttpContextLoggerConfig
            {
                Message = $"{context.HttpContext.Request.Path} End",
                IncludeResponseInfo = true,
                IsUnhandledException = context.Exception != null
            });
        }
    }
}
