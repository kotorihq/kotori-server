namespace KotoriServer.Tokens
{
    /// <summary>
    /// Project key result.
    /// </summary>
    public class ProjectKeyResult : IResult
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:KotoriServer.Tokens.ProjectKeyResult"/> is readonly.
        /// </summary>
        /// <value><c>true</c> if is readonly; otherwise, <c>false</c>.</value>
        public bool IsReadonly { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.ProjectKeyResult"/> class.
        /// </summary>
        public ProjectKeyResult(string key, bool isReadonly)
        {
            Key = key;
            IsReadonly = isReadonly;
        }
    }
}
