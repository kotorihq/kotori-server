namespace KotoriServer.Tokens
{
    /// <summary>
    /// Instance result.
    /// </summary>
    public class InstanceResult : IResult
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.InstanceResult"/> class.
        /// </summary>
        public InstanceResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.InstanceResult"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public InstanceResult(KotoriCore.Configurations.IKotoriConfiguration configuration)
        {
            Instance = configuration.Instance;
            Version = configuration.Version;
        }
    }
}
