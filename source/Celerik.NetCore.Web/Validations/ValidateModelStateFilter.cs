using Celerik.NetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Filter to validate the ModelState. In case the model state is invalid, we
    /// send out a custom ApiResponse&lt;TData&gt; and log a propper message.
    /// </summary>
    /// <code>
    ///     // Add this filter from the Startup.cs class:
    ///     public void ConfigureServices(IServiceCollection services)
    ///     {
    ///         services.AddMvc(config => {
    ///             config.Filters.Add(typeof(ValidateModelStateFilter));
    ///         });
    ///     }
    /// </code>
    public class ValidateModelStateFilter : IActionFilter
    {
        /// <summary>
        /// Called before the action executes, after model binding is completed.
        /// </summary>
        /// <param name="context">The context of the action filter.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(modelState => modelState.Value.Errors.Count > 0)
                    .SelectMany(modelState => modelState.Value.Errors);

                var firstError = errors.First();

                context.HttpContext.LogWarn(new HttpContextLoggerConfig
                {
                    Message = "ModelState is invalid",
                    Details = errors,
                    Jsonify = true
                });

                /*context.Result = new OkObjectResult(new ApiResponse<object>
                {
                    Message = firstError.ErrorMessage,
                    MessageType = ApiMessageType.Error,
                    Success = false
                });*/
            }
        }

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The context of the action filter.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
