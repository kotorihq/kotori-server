using System.Collections.Generic;
using KotoriServer.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace KotoriServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

			services.AddIdentityServer()
				.AddInMemoryClients(AuthServer.Config.Clients())
				.AddInMemoryApiResources(AuthServer.Config.ApiResources())
				.AddInMemoryUsers(AuthServer.Config.Users())
				.AddTemporarySigningCredential();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Kotori", Version = "v1" });
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
				c.AddSecurityDefinition("oauth2", new OAuth2Scheme
				{
					Type = "oauth2",
					Flow = "implicit",
					AuthorizationUrl = "http://petstore.swagger.io/oauth/dialog",
					Scopes = new Dictionary<string, string>
		{
			{ "readAccess", "Access read operations" },
			{ "writeAccess", "Access write operations" }
		}
				});
				// Assign scope requirements to operations based on AuthorizeAttribute
				c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kotori");
            });
        }
    }
}
