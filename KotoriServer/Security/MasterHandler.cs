using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using KotoriCore;
using KotoriCore.Exceptions;

namespace KotoriServer.Security
{
    /// <summary>
    /// Handler of master keys.
    /// </summary>
    public class MasterHandler : AuthorizationHandler<MasterRequirement>
    {
        readonly Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Security.MasterHandler"/> class.
        /// </summary>
        /// <param name="kotori">Kotori.</param>
        public MasterHandler(IKotori kotori)
        {
            _kotori = kotori as Kotori;
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
                var apiKey = mvcContext.ToHttpHeaderValue("masterKey");

                if (_kotori.Configuration.MasterKeys.Any(key => key.Key.Equals(apiKey)))
                    context.Succeed(requirement);
                else
                {
                    context.Fail();

                    var emptyKey = string.IsNullOrEmpty(apiKey);

                    throw new KotoriAuthException(KotoriCore.Helpers.Enums.ClaimType.Master, emptyKey ? "No master key provided." : "Invalid master key.", emptyKey);
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
