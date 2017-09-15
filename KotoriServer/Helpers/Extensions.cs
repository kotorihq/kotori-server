using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using KotoriServer.Helpers;
using System.Collections.Generic;
using Sushi2;

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
                Instance = section.GetValue<string>("Instance"),
                Version = section.GetValue<string>("Version"),
                MasterKeys = section.GetSection("MasterKeys").GetChildren().Select(x => x.Get<KotoriCore.Configuration.MasterKey>()),                
            };

            return kotori;
        }

        /// <summary>
        /// Convert auth filter context to a header value of particular field.
        /// </summary>
        /// <param name="context">Auth filter context.</param>
        /// <param name="key">Http header key.</param>
        /// <returns>Value or empty string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        public static string ToHttpHeaderValue(this AuthorizationFilterContext context, string key)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var header = ((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)((Microsoft.AspNetCore.Http.DefaultHttpContext)context.HttpContext).Request).Headers;

            var foundKey = header.Keys.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (foundKey == null)
                return string.Empty;

            return header[foundKey].ToString();
        }

        /// <summary>
        /// Get scope string from <paramref name="claimType">the claim type</paramref>.
        /// </summary>
        /// <param name="claimType">The claim type.</param>
        /// <returns>Scope string.</returns>
        public static string ToClaimString(this Enums.ClaimType claimType) => claimType.ToString().ToLower();       
    }
}
