using KotoriCore.Domains;

namespace KotoriServer.Tokens
{
    public class ProjectResult : IResult
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// The name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.ProjectResult"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
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
