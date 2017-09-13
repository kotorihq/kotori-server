using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KotoriServer.Security
{
    public class MasterHandler : AuthorizationHandler<MasterRequirement>
    {        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MasterRequirement requirement)
        {            
            if (context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext)
            {
                var header = ((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)((Microsoft.AspNetCore.Http.DefaultHttpContext)mvcContext.HttpContext).Request).Headers;

                if (!header.Keys.Contains("apiKey"))
                    context.Fail();

                var val = header["apiKey"].ToString();

                // mega hack
                if (val.Equals("x"))
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
