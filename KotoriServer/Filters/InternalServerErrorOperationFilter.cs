using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using KotoriServer.Security;

namespace KotoriServer.Filters
{
    public class InternalServerErrorOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Responses.Add("500", new Response { Description = "Internal server error" });
        }
    }
}
