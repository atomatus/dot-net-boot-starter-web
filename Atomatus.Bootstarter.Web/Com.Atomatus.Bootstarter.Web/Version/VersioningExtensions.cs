using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Com.Atomatus.Bootstarter.Web
{
    public static class VersioningExtensions
    {
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

        public static IApplicationBuilder UseVersionsing([NotNull] this IApplicationBuilder app)
        {
            return app.UseApiVersioning();
        }
    }
}
