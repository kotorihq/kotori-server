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
        readonly string _instance;

        public InstanceController(IKotori kotori)
        {
            _kotori = kotori as Kotori;
            _instance = _kotori.Configuration.Instance;
        }

        /// <summary>
        /// Get instance name.
        /// </summary>
        /// <returns>The instance name.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(InstanceResult), 200)]
        public InstanceResult Get()
        {
            return new InstanceResult(_instance);
        }
    }
}
