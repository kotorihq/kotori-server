namespace KotoriServer.Tokens
{
    /// <summary>
    /// Create project key request.
    /// </summary>
    public class CreateProjectKeyRequest
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:KotoriServer.Tokens.CreateProjectKeyRequest"/> is readonly.
        /// </summary>
        /// <value><c>true</c> if is readonly; otherwise, <c>false</c>.</value>
        public bool? IsReadonly { get; set; }
    }
}
