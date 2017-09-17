using KotoriCore;

namespace KotoriServer.Models
{
    /// <summary>
    /// Site index model.
    /// </summary>
    public class SiteIndex
    {
        /// <summary>
        /// Gets kotori configuration.
        /// </summary>
        /// <value>The kotori configuration.</value>
        public Kotori Kotori { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Models.SiteIndex"/> class.
        /// </summary>
        /// <param name="kotori">Kotori configuration.</param>
        public SiteIndex(Kotori kotori)
        {            
            Kotori = kotori;
        }
    }
}
