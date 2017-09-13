using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class MasterRequirement : IAuthorizationRequirement, IRequirement
    {
    }
}
