using System.Collections.Generic;
using System.Linq;
using KotoriServer.Helpers;
using KotoriServer.Security;

namespace KotoriServer.AuthServer
{
    public class LoginService
    {     
        readonly List<KeyUser> _users;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:KotoriServer.AuthServer.LoginService.InMemoryUserLoginService"/> class.
        /// </summary>
        /// <param name="users">Users.</param>
		public LoginService(List<KeyUser> users)
		{
			_users = users;
		}

	    /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <returns><c>true</c>, if credentials was validated, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="type">Type.</param>
		public bool ValidateCredentials(string key, Enums.KeyUserType type)
		{
			var user = FindByKeyAndType(key, type);

            return user != null;
		}

		/// <summary>
		/// Find a user by username
		/// </summary>
		public KeyUser FindByKeyAndType(string key, Enums.KeyUserType type)
		{
			return _users.FirstOrDefault(x => x.Key.Equals(key, System.StringComparison.OrdinalIgnoreCase) && x.Type == type);
		}

		/*public InMemoryUser FindByExternalProvider(string provider, string userId)
		{
			return _users.FirstOrDefault(x =>
				x.Provider == provider &&
				x.ProviderId == userId);
		}*/

		/// <summary>
		/// Sample auto-provision logic of new external users
		/// </summary>
		/*public InMemoryUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
		{
			// create a list of claims that we want to transfer into our store
			var filtered = new List<Claim>();

			foreach (var claim in claims)
			{
				// if the external system sends a display name - translate that to the standard OIDC name claim
				if (claim.Type == ClaimTypes.Name)
				{
					filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
				}
				// if the JWT handler has an outbound mapping to an OIDC claim use that
				else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
				{
					filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
				}
				// copy the claim as-is
				else
				{
					filtered.Add(claim);
				}
			}

			// if no display name was provided, try to construct by first and/or last name
			if (!filtered.Any(x => x.Type == JwtClaimTypes.Name))
			{
				var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
				var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
				if (first != null && last != null)
				{
					filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
				}
				else if (first != null)
				{
					filtered.Add(new Claim(JwtClaimTypes.Name, first));
				}
				else if (last != null)
				{
					filtered.Add(new Claim(JwtClaimTypes.Name, last));
				}
			}

			// create a new unique subject id
			var sub = CryptoRandom.CreateUniqueId();

			// check if a display name is available, otherwise fallback to subject id
			var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub;

			// create new user
			var user = new InMemoryUser()
			{
				Enabled = true,
				Subject = sub,
				Username = name,
				Provider = provider,
				ProviderId = userId,
				Claims = filtered
			};

			// add user to in-memory store
			_users.Add(user);

			return user;
		}*/
	}
}
