using KotoriCore.Domains;

namespace KotoriServer.Tokens
{
    public class ProjectResult
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// The name.
        /// </summary>
        public readonly string Name;

        public ProjectResult(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static implicit operator ProjectResult(SimpleProject simpleProject)
        {
            return new ProjectResult
            (
                simpleProject.Identifier,
                simpleProject.Name
            );
        }
    }
}
