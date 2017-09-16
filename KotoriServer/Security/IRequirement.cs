﻿using KotoriServer.Helpers;

namespace KotoriServer.Security
{
    /// <summary>
    /// Requirement inteface.
    /// </summary>
    public interface IRequirement
    {
        /// <summary>
        /// Gets the type of the claim.
        /// </summary>
        /// <value>The type of the claim.</value>
        Enums.ClaimType ClaimType { get; }
    }
}
