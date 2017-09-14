using KotoriCore.Configuration;

namespace KotoriServer.Models
{
    public class SiteIndex
    {
        public Kotori Kotori { get; }

        public SiteIndex(Kotori kotori)
        {            
            Kotori = kotori;
        }
    }
}
