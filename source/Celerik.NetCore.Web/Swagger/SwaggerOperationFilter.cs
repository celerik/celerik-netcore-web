using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Celerik.NetCore.Web
{
    /// <summary>
    /// Implementation of the IOperationFilter interface to customize
    /// Swagger operations, in particular to allow adding the
    /// Authorization header paremeter to the endpoints that require
    /// authorization.
    /// </summary>
    public class SwaggerOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Adds all customized parameters to the passed-in operation
        /// object.
        /// </summary>
        /// <param name="operation">The swagger operation.</param>
        /// <param name="context">The operation filter context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            /*var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var filterList = filterDescriptors.Select(filter => filter.Filter);
            var requireAuthorization = filterList.Any(filter => filter is AuthorizeFilter);
            var requirePermission = filterList.Any(filter => filter is PermissionAttribute);
            var allowAnonymous = filterList.Any(filter => filter is IAllowAnonymousFilter);

            if ((requireAuthorization || requirePermission) && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "Access Token",
                    Required = true,
                    Type = "string",
                    Default = "Bearer "
                });
            }*/
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
