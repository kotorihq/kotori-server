using System.IO;
using Daifuku.Extensions;
using KotoriCore;
using KotoriServer.Filters;
using KotoriServer.Middleware;
using KotoriServer.Security;
using KotoriServer.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Examples;
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

            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "KotoriServer.xml");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                new Info 
                { 
                    Title = "Kotori", 
                    Description = "REST API for Kotori",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "frohikey",
                        Email = "frohikey@gmail.com"
                    },
                    License = new License
                    {
                        Name = "MIT",
                        Url = "https://raw.githubusercontent.com/kotorihq/kotori-server/master/LICENSE"
                    }
                });

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();

                c.AddSecurityDefinition("apiKey", new ApiKeyScheme
                {
                    Type = "apiKey",
                    Description = "API Key Authentication",
                    Name = "apiKey",
                    In = "header"
                });

                c.CustomSchemaIds((type) => {
                    if (type == typeof(InstanceResult))
                        return "Instance";
                    
                    return type.FullName; 
                }
                );

                c.OperationFilter<ExamplesOperationFilter>();
                c.OperationFilter<DescriptionOperationFilter>();

                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<InternalServerErrorOperationFilter>();

                c.IncludeXmlComments(filePath);
            });

            services.AddAuthorization(options => options.AddPolicy("master", policy => policy.Requirements.Add(new MasterRequirement())));
            services.AddAuthorization(options => options.AddPolicy("project", policy => policy.Requirements.Add(new ProjectRequirement(KotoriCore.Helpers.Enums.ClaimType.Project))));
            services.AddAuthorization(options => options.AddPolicy("readonlyproject", policy => policy.Requirements.Add(new ProjectRequirement(KotoriCore.Helpers.Enums.ClaimType.ReadonlyProject))));

            services.AddSingleton<IAuthorizationHandler, MasterHandler>();
            services.AddSingleton<IAuthorizationHandler, ProjectHandler>();
            services.AddSingleton<IKotori, Kotori>();
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
                // force https
                var options = new RewriteOptions().AddRedirectToHttpsPermanent();
                app.UseRewriter(options);

                //app.RedirectDomains(new System.Collections.Generic.Dictionary<string, string>
                //{
                //    { "from", "to" },
                //});

                app.UseExceptionHandler("/error");
            }

            app.UseDaifuku();
            app.UseServerHeader("WE <3 KOTORI");

            app.UseStaticFiles();

            app.UseMiddleware(typeof(ErrorHandling));

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
