using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Swagger service extensions.
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        private static readonly Lazy<ConcurrentBag<string>> versions;

        static SwaggerServiceExtensions()
        {
            versions = new Lazy<ConcurrentBag<string>>();
        }

        /// <summary>
        /// <para>
        /// Add swagger API documentation to current API usage.
        /// </para>
        /// 
        /// <para>
        /// Bellow an example how to implement swaggerdoc information in appsetting.json.<br/>
        /// Whether not defined it, the SwaggerDoc.Factory will try to create it from assembly definition fields.
        /// </para>
        /// 
        /// <para>
        /// ● From appsettings.json:<br/><br/>
        /// <code>
        /// "SwaggerDoc": {<br/>
        /// "Title": "Your API Name",<br/>
        /// "Description": "Your API description",<br/>
        /// "Author": {<br/>
        /// "Name": "Author name",<br/>
        /// "Email": "author@mail.com",<br/>
        /// "Url": "https://yourprojectaddress"<br/>
        /// }}
        /// </code>
        /// </para>
        /// 
        /// <para>
        /// ● From Assembly Fields:<br/><br/>
        /// <code>
        /// //Title<br/>
        /// Assembly.GetName().Name<br/><br/>
        /// //Versions<br/>
        /// Assembly.GetName().Version<br/><br/>
        /// //Description<br/>
        /// Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)<br/><br/>
        /// //Author Name<br/>
        /// FileVersionInfo.GetVersionInfo(Assembly.Location).CompanyName<br/><br/>
        /// </code>
        /// </para>
        /// 
        /// <para>
        /// About Swagger:<br/>
        /// <i>
        /// Swagger is an Interface Description Language for describing RESTful APIs 
        /// expressed using JSON. Swagger is used together with a set of open-source 
        /// software tools to design, build, document, and use RESTful web services. <br/>
        /// Swagger includes automated documentation, code generation, and test-case
        /// </i>
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger([NotNull] this IServiceCollection services, [AllowNull] IConfiguration configuration = null)
        {
            configuration ??= services.BuildServiceProvider().GetService<IConfiguration>();
            return services.AddSwaggerGen(options =>
            {
                while (versions.Value.TryTake(out string v))
                {
                    SwaggerDoc doc = SwaggerDoc.Factory.Create(configuration, v);
                    options.SwaggerDoc(doc);
                }
            });
        }

        /// <summary>
        /// <para>
        /// Enable Swagger usage for application configuration.
        /// </para>
        /// <i>
        /// Do not forget to define <see cref="AddSwagger(IServiceCollection, IConfiguration)"/>
        /// in <see cref="IServiceCollection"/> before request this method.
        /// </i>
        /// </summary>
        /// <param name="app">application configuration builder</param>
        /// <param name="provider">provider that discovers and describes API version information within an application.</param>
        /// <returns>application configuration builder</returns>
        public static IApplicationBuilder UseSwagger([NotNull] this IApplicationBuilder app, [AllowNull] IApiVersionDescriptionProvider provider = null)
        {            
            return SwaggerBuilderExtensions.UseSwagger(app)
               .UseSwaggerUI(c =>
               {
                   provider ??= app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                   foreach (var desc in provider.ApiVersionDescriptions)
                   {
                       c.SwaggerEndpoint(
                       $"/swagger/{desc.GroupName}/swagger.json",
                       desc.GroupName.ToUpperInvariant());
                       versions.Value.Add(desc.GroupName);
                   }
                   c.DocExpansion(DocExpansion.List);
               });
        }
    }
}
