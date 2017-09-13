using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class ProjectHandler : AuthorizationHandler<ProjectRequirement>
    {        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
