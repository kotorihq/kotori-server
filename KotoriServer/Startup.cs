using Daifuku.Extensions;
using KotoriCore;
using KotoriServer.Filters;
using KotoriServer.Helpers;
using KotoriServer.Middleware;
using KotoriServer.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace KotoriServer
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="environment">Environment.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        IConfiguration Configuration { get; }
        IHostingEnvironment Environment { get; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">A collection of services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "KotoriServer.xml");
            //var filePath = Path.Combine(Environment.ContentRootPath, "KotoriServer.xml");

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
                        Name = "frohikey"
                    },
                    License = new License
                    {
                        Name = "MIT",
                        Url = "https://raw.githubusercontent.com/kotorihq/kotori-server/master/LICENSE"
                    }
                });

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();

                c.AddSecurityDefinition("masterKey", new ApiKeyScheme
                {
                    Type = "apiKey",
                    Description = "API Key Authentication",
                    Name = "masterKey",
                    In = "header"
                });

                c.AddSecurityDefinition("projectKey", new ApiKeyScheme
                {
                    Type = "apiKey",
                    Description = "API Key Authentication",
                    Name = "projectKey",
                    In = "header"
                });

                //c.CustomSchemaIds((type) =>
                //{
                //    if (type == typeof(InstanceResult))
                //        return "Instance";

                //    if (type == typeof(KotoriCore.Domains.SimpleProject))
                //        return "Project";

                //    if (type == typeof(KotoriCore.Domains.ProjectKey))
                //        return "ProjectKey";

                //    return type.FullName;
                //}
                //);
                
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<InternalServerErrorOperationFilter>();

                //c.IncludeXmlComments(filePath);
            });

            services.AddAuthorization(options => options.AddPolicy("master", policy => policy.Requirements.Add(new MasterRequirement())));
            services.AddAuthorization(options => options.AddPolicy("project", policy => policy.Requirements.Add(new ProjectRequirement(KotoriCore.Helpers.Enums.ClaimType.Project))));
            services.AddAuthorization(options => options.AddPolicy("readonlyproject", policy => policy.Requirements.Add(new ProjectRequirement(KotoriCore.Helpers.Enums.ClaimType.ReadonlyProject))));

            services.AddSingleton<IAuthorizationHandler, MasterHandler>();
            services.AddSingleton<IAuthorizationHandler, ProjectHandler>();
            services.AddSingleton<IKotori, Kotori>();

            services.AddCors
            (
                options => { options.AddPolicy("AllowAnyOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); }
            );

            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                options.InputFormatters.Insert(0, new RawRequestBodyFormatter());
            });
        }

        /// <summary>
        /// Configure the specified app and env.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Hosting environment.</param>
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

            app.UseCors("AllowAnyOrigin");

            app.UseMiddleware(typeof(ErrorHandling));

            app.UseMvc(routes =>
            {
                routes.MapRoute("index", "", new { controller = "Site", action = "Index" });
                routes.MapRoute("error", "error", new { controller = "Site", action = "Error" });
            }
            );

            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kotori");
            //    c.ShowRequestHeaders();
            //});
        }
    }
}