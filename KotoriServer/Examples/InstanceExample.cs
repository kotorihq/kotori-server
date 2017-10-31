using KotoriServer.Tokens;
using Swashbuckle.AspNetCore.Examples;

namespace KotoriServer.Examples
{
    /// <summary>
    /// Instance example.
    /// </summary>
    public class InstanceExample : IExamplesProvider
    {
        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns>The examples.</returns>
        public object GetExamples()
        {
            return new InstanceResult { Instance = "master-instance" };
        }
    }
}
