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
    public static class SwaggerServiceExtensions
    {
        private static readonly Lazy<ConcurrentBag<string>> versions;

        static SwaggerServiceExtensions()
        {
            versions = new Lazy<ConcurrentBag<string>>();
        }

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
