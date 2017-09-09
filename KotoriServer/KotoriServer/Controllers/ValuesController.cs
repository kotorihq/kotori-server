using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace KotoriServer.Controllers
{
    [Route("api/[controller]")]
    [Obsolete]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [Obsolete]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Obsolete]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Obsolete]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Obsolete]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Obsolete]
        public void Delete(int id)
        {
        }
    }
}
