using KotoriServer.Helpers;

namespace KotoriServer.Security
{
    /// <summary>
    /// Key user.
    /// </summary>
    public class KeyUser
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Enums.KeyUserType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Security.KeyUser"/> class.
        /// </summary>
        public KeyUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Security.KeyUser"/> class.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="type">Type.</param>
        public KeyUser(string key, Enums.KeyUserType type)
        {
            Key = key;
            Type = type;
        }
    }
}
