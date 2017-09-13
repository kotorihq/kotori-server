﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace KotoriServer.Security
{
    public class MasterHandler : AuthorizationHandler<MasterRequirement>
    {
        KotoriCore.Configuration.Kotori _kotori;

        public MasterHandler(IConfiguration config)
        {
            _kotori = config.ToKotoriConfiguration();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MasterRequirement requirement)
        {            
            if (context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext)
            {
                var header = ((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)((Microsoft.AspNetCore.Http.DefaultHttpContext)mvcContext.HttpContext).Request).Headers;

                if (!header.Keys.Contains("apiKey"))
                    context.Fail();

                var val = header["apiKey"].ToString();
                
                if (_kotori.MasterKeys.Any(key => key.Key.Equals(val)))
                    context.Succeed(requirement);
                else
                    context.Fail();
            }
            else
            {
                context.Fail();
            }                       

            return Task.CompletedTask;
        }
    }
}
