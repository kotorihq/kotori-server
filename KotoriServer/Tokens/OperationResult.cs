namespace KotoriServer.Tokens
{
    /// <summary>
    /// Operation result.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.OperationResult"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="url">URL.</param>
        public OperationResult(string id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}
