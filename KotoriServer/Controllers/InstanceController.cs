﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Instance controller.
    /// </summary>
    [Route("api/instance")]
    public class InstanceController
    {
        private KotoriCore.Configuration.Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.InstanceController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public InstanceController(IConfiguration config)
        {
            _kotori = config.ToKotoriConfiguration();
        }

        /// <summary>
        /// Get instance name.
        /// </summary>
        /// <returns>The instance name.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get()
        {
            return "?";
            //return _kotori.Instance;
        }
    }
}
