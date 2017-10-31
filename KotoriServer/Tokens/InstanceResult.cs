namespace KotoriServer.Tokens
{
    /// <summary>
    /// Instance result.
    /// </summary>
    public class InstanceResult
    {
        /// <value>The instance name.</value>
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
        /// <param name="instance">Instance name.</param>
        public InstanceResult(string instance)
        {
            Instance = instance;
        }
    }
}
