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
        public KotoriCore.Helpers.Enums.ClaimType ClaimType => KotoriCore.Helpers.Enums.ClaimType.Project;
    }
}
