using KotoriServer.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class ProjectRequirement : IAuthorizationRequirement, IRequirement
    {
        public Enums.ClaimType ClaimType => Enums.ClaimType.Project;
    }
}
