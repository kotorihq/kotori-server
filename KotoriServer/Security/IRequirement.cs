using KotoriServer.Helpers;

namespace KotoriServer.Security
{
    public interface IRequirement
    {
        Enums.ClaimType ClaimType { get; }
    }
}
