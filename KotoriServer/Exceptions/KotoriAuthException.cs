using KotoriServer.Helpers;

namespace KotoriServer.Exceptions
{
    public class KotoriAuthException : KotoriException
    {
        public Enums.ClaimType ClaimType { get; }
        public bool EmptyKey { get; }

        public KotoriAuthException(Enums.ClaimType claimType, string message, bool emptyKey) : base(message)
        {
            EmptyKey = emptyKey;
            ClaimType = claimType;
        }        
    }
}
