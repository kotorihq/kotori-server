﻿using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    /// <summary>
    /// Master requirement.
    /// </summary>
    public class MasterRequirement : IAuthorizationRequirement, IRequirement
    {
        /// <summary>
        /// Gets the type of the claim.
        /// </summary>
        /// <value>The type of the claim.</value>
        public KotoriCore.Helpers.Enums.ClaimType ClaimType => KotoriCore.Helpers.Enums.ClaimType.Master;
    }
}
