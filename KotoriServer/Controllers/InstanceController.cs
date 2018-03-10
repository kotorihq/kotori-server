using KotoriCore;
using KotoriServer.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Instance controller.
    /// </summary>
    [Route("api/instance")]
    public class InstanceController
    {
        readonly Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.InstanceController"/> class.
        /// </summary>
        /// <param name="kotori">Kotori.</param>
        public InstanceController(IKotori kotori)
        {
            _kotori = kotori as Kotori;
        }

        /// <summary>
        /// Gets the instance information.
        /// </summary>
        /// <returns>The instance information.</returns>
        [HttpGet]
        public InstanceResult Get()
        {
            return new InstanceResult(_kotori.Configuration);
        }
    }
}
