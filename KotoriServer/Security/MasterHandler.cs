using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class MasterHandler : AuthorizationHandler<MasterRequirement>
    {        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MasterRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
