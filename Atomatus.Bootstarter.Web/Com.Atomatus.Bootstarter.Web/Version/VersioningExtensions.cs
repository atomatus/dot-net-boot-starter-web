using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// API Versioning extensions.
    /// </summary>
    public static class VersioningExtensions
    {
        /// <summary>
        /// Adds service API versioning to the specified services collection
        /// with default api version as 1.0, and formated as "'v'VVV" (v1.0 or 1.0).
        /// </summary>
        /// <param name="services">specified services collection</param>
        /// <returns>current services collection</returns>
        public static IServiceCollection AddVersioning([NotNull] this IServiceCollection services)
        {
            return services
                .AddApiVersioning(p =>
                {
                    p.DefaultApiVersion = new ApiVersion(1, 0);
                    p.ReportApiVersions = true;
                    p.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddVersionedApiExplorer(p =>
                {
                    p.GroupNameFormat = "'v'VVV";
                    p.SubstituteApiVersionInUrl = true;
                });
        }

        /// <summary>
        /// Enable API versioning usage to application.
        /// </summary>
        /// <param name="app">application configuration builder</param>
        /// <returns>application configuration builder</returns>
        public static IApplicationBuilder UseVersionsing([NotNull] this IApplicationBuilder app)
        {
            return app.UseApiVersioning();
        }
    }
}
