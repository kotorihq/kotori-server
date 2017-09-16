using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;
using KotoriServer.Exceptions;

namespace KotoriServer.Security
{
    /// <summary>
    /// Handler of master keys.
    /// </summary>
    public class MasterHandler : AuthorizationHandler<MasterRequirement>
    {
        KotoriCore.Configuration.Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Security.MasterHandler"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public MasterHandler(IConfiguration config)
        {
            _kotori = config.ToKotoriConfiguration();
        }

        /// <summary>
        /// Handles the requirement async.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="requirement">Requirement.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MasterRequirement requirement)
        {            
            if (context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext)
            {            
                var apiKey = mvcContext.ToHttpHeaderValue("apiKey");

                if (_kotori.MasterKeys.Any(key => key.Key.Equals(apiKey)))
                    context.Succeed(requirement);
                else
                {
                    context.Fail();

                    var emptyKey = string.IsNullOrEmpty(apiKey);

                    throw new KotoriAuthException(Helpers.Enums.ClaimType.Master, emptyKey ? "No master key provided." : "Invalid master key.", emptyKey);
                }
            }
            else
            {
                context.Fail();
            }                       

            return Task.CompletedTask;
        }
    }
}
