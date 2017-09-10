using Microsoft.Extensions.Configuration;

namespace KotoriServer
{
    public static class Extensions
    {
        /// <summary>
        /// Convert configuration to the kotori configuration.
        /// </summary>
        /// <returns>The kotori configuration.</returns>
        /// <param name="configuration">Configuration.</param>
        public static KotoriCore.Configuration.Kotori ToKotoriConfiguration(this IConfiguration configuration)
        {
            var section = configuration.GetSection("Kotori").GetSection("Configuration");

            var kotori = new KotoriCore.Configuration.Kotori
            {
                Instance = section.GetValue<string>("Instance")
            };

            return kotori;
        }
    }
}
