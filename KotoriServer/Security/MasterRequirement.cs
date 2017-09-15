using KotoriServer.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class MasterRequirement : IAuthorizationRequirement, IRequirement
    {
        public Enums.ClaimType ClaimType => Enums.ClaimType.Master;
    }
}
