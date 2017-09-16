using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using KotoriServer.Security;

namespace KotoriServer.Filters
{
    /// <summary>
    /// Security requirements operation filter.
    /// </summary>
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        readonly IOptions<AuthorizationOptions> _authorizationOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Security.SecurityRequirementsOperationFilter"/> class.
        /// </summary>
        /// <param name="authorizationOptions">Authorization options.</param>
        public SecurityRequirementsOperationFilter(IOptions<AuthorizationOptions> authorizationOptions)
        {
            _authorizationOptions = authorizationOptions;
        }

        /// <summary>
        /// Apply the specified operation and context.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="context">Context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerPolicies = context.ApiDescription.ControllerAttributes()
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy);
            var actionPolicies = context.ApiDescription.ActionAttributes()
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy);
            var policies = controllerPolicies.Union(actionPolicies).Distinct();
            var requiredClaimTypes = policies
                .Select(x => _authorizationOptions.Value.GetPolicy(x))
                .SelectMany(x => x.Requirements)
                .OfType<MasterRequirement>()
                .Select(x => x.ClaimType);

            // any required claims? inject 401 + 403 reponses
            if (requiredClaimTypes.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "apiKey", requiredClaimTypes.Select(x => x.ToClaimString()) }
                    }
                };
            }
        }
    }
}
