using System.Threading.Tasks;
using KotoriCore;
using KotoriCore.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace KotoriServer.Security
{
    public class ProjectHandler : AuthorizationHandler<ProjectRequirement>
    {        
        readonly Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Security.ProjectHandler"/> class.
        /// </summary>
        /// <param name="kotori">Kotori.</param>
        public ProjectHandler(IKotori kotori)
        {
            _kotori = kotori as Kotori;
        }

        /// <summary>
        /// Handles the requirement async.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="requirement">Requirement.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectRequirement requirement)
        {
            if (context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext)
            {
                var projectId = mvcContext.RouteData.Values["projectId"]?.ToString();
                var apiKey = mvcContext.ToHttpHeaderValue("projectKey");

                if (!string.IsNullOrEmpty(apiKey) &&
                   !string.IsNullOrEmpty(projectId))
                {
                    try
                    {
                        var keys = _kotori.GetProjectKeys(_kotori.Configuration.Instance, projectId);
                        var key = keys.FirstOrDefault(k => k.Key.Equals(apiKey));
                        var isOk = false;

                        if (key != null)
                        {
                            if (requirement.ClaimType == KotoriCore.Helpers.Enums.ClaimType.Project)
                            {
                                isOk |= !key.IsReadonly;
                            }

                            if (requirement.ClaimType == KotoriCore.Helpers.Enums.ClaimType.ReadonlyProject)
                            {
                                isOk = true;
                            }
                        }
                        else
                        {
                            throw new KotoriAuthException(requirement.ClaimType, "Invalid project key provided.", string.IsNullOrEmpty(apiKey));
                        }

                        if (isOk)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                            throw new KotoriAuthException(requirement.ClaimType, "Invalid project key provided.", string.IsNullOrEmpty(apiKey));
                        }
                    }
                    catch(KotoriException)
                    {
                        context.Fail();    
                        throw new KotoriAuthException(requirement.ClaimType, "Invalid project key provided.", string.IsNullOrEmpty(apiKey));
                    }
                }
                else
                {
                    context.Fail();
                    throw new KotoriAuthException
                    (
                        KotoriCore.Helpers.Enums.ClaimType.Master,
                        string.IsNullOrEmpty(apiKey) ? "No project key provided." : "Invalid project key / project identifier.", 
                        string.IsNullOrEmpty(apiKey)
                    );
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
