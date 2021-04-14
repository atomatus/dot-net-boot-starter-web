using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Com.Atomatus.Bootstarter.Web
{
    public static class BootstarterServiceExtensions
    {
        public static IMvcBuilder AddBootstarters([NotNull] this IServiceCollection services, [AllowNull] IConfiguration configuration = null)
        {
            return services
                .AddVersioning()
                .AddSwagger(configuration)
                .AddControllers();
        }

        public static IApplicationBuilder UseBootstarters([NotNull] this IApplicationBuilder app, [AllowNull] IApiVersionDescriptionProvider provider = null)
        {
            return app
                .UseVersionsing()
                .UseSwagger(provider)
                .UseHttpsRedirection()
                .UseAuthorization()
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
