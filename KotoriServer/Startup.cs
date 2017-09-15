using KotoriServer.Security;
using Microsoft.AspNetCore.Authorization;
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Kotori", Version = "v1" });
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.AddSecurityDefinition("apiKey", new ApiKeyScheme
                {
                    Type = "apiKey",
                    Description = "API Key Authentication",
                    Name = "apiKey",
                    In = "header"
                });
                c.OperationFilter<AuthResponsesOperationFilter>();
            });

            services.AddAuthorization(options => options.AddPolicy("master", policy => policy.Requirements.Add(new MasterRequirement())));
            services.AddAuthorization(options => options.AddPolicy("project", policy => policy.Requirements.Add(new ProjectRequirement())));

            services.AddSingleton<IAuthorizationHandler, MasterHandler>();
            services.AddSingleton<IAuthorizationHandler, ProjectHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("index", "", new { controller = "Site", action = "Index" });
                routes.MapRoute("error", "error", new { controller = "Site", action = "Error" });                
            }
            );

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kotori");
                c.ShowRequestHeaders();
            });
        }
    }
}
