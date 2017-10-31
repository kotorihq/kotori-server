﻿namespace KotoriServer.Tokens
{
    /// <summary>
    /// Instance result.
    /// </summary>
    public class InstanceResult
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public string Instance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.InstanceResult"/> class.
        /// </summary>
        public InstanceResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.InstanceResult"/> class.
        /// </summary>
        /// <param name="instance">Instance.</param>
        public InstanceResult(string instance)
        {
            Instance = instance;
        }
    }
}