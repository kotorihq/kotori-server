using KotoriServer.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    /// <summary>
    /// Project requirement.
    /// </summary>
    public class ProjectRequirement : IAuthorizationRequirement, IRequirement
    {
        /// <summary>
        /// Gets the type of the claim.
        /// </summary>
        /// <value>The type of the claim.</value>
        public Enums.ClaimType ClaimType => Enums.ClaimType.Project;
    }
}
