using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;
using KotoriServer.Exceptions;

namespace KotoriServer.Security
{
    public class MasterHandler : AuthorizationHandler<MasterRequirement>
    {
        KotoriCore.Configuration.Kotori _kotori;

        public MasterHandler(IConfiguration config)
        {
            _kotori = config.ToKotoriConfiguration();
        }

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
