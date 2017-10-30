using KotoriServer.Tokens;
using Swashbuckle.AspNetCore.Examples;

namespace KotoriServer.Examples
{
    public class InstanceExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new InstanceResult { Instance = "master-instance" };
        }
    }
}
