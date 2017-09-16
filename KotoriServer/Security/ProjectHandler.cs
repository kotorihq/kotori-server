using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class ProjectHandler : AuthorizationHandler<ProjectRequirement>
    {        
        /// <summary>
        /// Handles the requirement async.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="requirement">Requirement.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectRequirement requirement)
        {
            // TODO: implement
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
