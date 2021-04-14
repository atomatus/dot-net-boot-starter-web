using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;

namespace Com.Atomatus.Bootstarter.Web
{
    internal static class SwaggerDocExtensions
    {
        public static string ResolveSwaggerDoc(this IConfiguration configuration, 
            Func<Assembly, string> assemblyCallback,
            params string[] candidateKeys)
        {
            string[] prefixes = new[] { "SwaggerDoc", "Swagger" };
            
            foreach(string suffix in candidateKeys)
            {
                foreach(string prefix in prefixes)
                {
                    string key = string.Join(':', prefix, suffix);
                    string value = configuration[key];

                    if(!string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }
                }
            }

            return assemblyCallback?.Invoke(Assembly.GetEntryAssembly());
        }

        public static void SwaggerDoc(this SwaggerGenOptions options, SwaggerDoc doc)
        {
            options.SwaggerDoc(doc.Version, doc);
        }
    }
}