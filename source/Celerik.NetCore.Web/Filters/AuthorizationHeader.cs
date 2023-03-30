using System;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Celerik.NetCore.Web
{
    public class AuthorizationHeader : Attribute, IOperationFilter
    {
        private readonly IConfiguration _config;

        public AuthorizationHeader(IConfiguration config)
        {
            _config = config;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "X-Authorization",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema() { Type = "string" },
                Example = new OpenApiString(_config["SwaggerConfig:BearerToken"]),
            });
        }
    }
}
