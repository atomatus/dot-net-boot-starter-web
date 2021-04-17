using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Swagger doc extensions.
    /// </summary>
    internal static class SwaggerDocExtensions
    {
        /// <summary>
        /// Resolve swaggerdoc values or request assembly info usage.
        /// </summary>
        /// <param name="configuration">current configuration values</param>
        /// <param name="assemblyCallback">assembly callback usage</param>
        /// <param name="candidateKeys">candidate configuration keys</param>
        /// <returns></returns>
        internal static string ResolveSwaggerDoc(this IConfiguration configuration, 
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

        /// <summary>
        /// Define swagger doc to options.
        /// </summary>
        /// <param name="options">swagger generation options</param>
        /// <param name="doc">swagger document values</param>
        internal static void SwaggerDoc(this SwaggerGenOptions options, SwaggerDoc doc)
        {
            options.SwaggerDoc(doc.Version, doc);
        }
    }
}