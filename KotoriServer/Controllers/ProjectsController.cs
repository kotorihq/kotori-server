﻿using System.Collections.Generic;
using KotoriCore;
using KotoriCore.Commands;
using KotoriCore.Configurations;
using KotoriCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KotoriServer.Controllers
{
    /// <summary>
    /// Projects controller.
    /// </summary>
    [Route("api/projects")]
    public class ProjectsController
    {
        Kotori _kotori;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Controllers.ProjectController"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public ProjectController(IConfiguration config)
        {
            _kotori = new Kotori(config);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize("master")]
        public string Post(string name, string identifier, [FromBody]List<ProjectKey> projectKeys = null)
        {
            var result = _kotori.Process(new CreateProject(_kotori.Configuration.Instance, name, identifier, projectKeys));

            return "Project has been created.";
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KotoriCore.Domains.SimpleProject>), 200)]
        [Authorize("master")]
        public IEnumerable<KotoriCore.Domains.SimpleProject> Get()
        {
            var result = _kotori.Process(new GetProjects(_kotori.Configuration.Instance));

            return result.ToDataList<KotoriCore.Domains.SimpleProject>();
        }
    }
}