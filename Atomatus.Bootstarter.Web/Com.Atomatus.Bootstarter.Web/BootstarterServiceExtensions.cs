using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// Bootstarter services extensions.
    /// A simplification of amount requests to service collection and
    /// application builder usage.
    /// </summary>
    public static class BootstarterServiceExtensions
    {
        /// <summary>
        /// <para>
        /// Add bootstarters services:
        /// </para>
        /// <para>
        /// ● <see cref="VersioningExtensions.AddVersioning(IServiceCollection)"/><br/>
        /// ● <see cref="SwaggerServiceExtensions.AddSwagger(IServiceCollection, IConfiguration)"/><br/>
        /// ● <see cref="MvcServiceCollectionExtensions.AddControllers(IServiceCollection)"/>
        /// </para>
        /// </summary>
        /// <param name="services">current service collection</param>
        /// <param name="configuration">optional configurations containing swagerDoc fields</param>
        /// <returns>mvc service builder</returns>
        public static IMvcBuilder AddBootstarters([NotNull] this IServiceCollection services, [AllowNull] IConfiguration configuration = null)
        {
            return services
                .AddLogService()
                .AddVersioning()
                .AddSwagger(configuration)
                .AddControllers();
        }

        /// <summary>
        /// <para>
        /// Enable usage of following (bootstarters) services to application.
        /// </para>
        /// <para>
        /// ● <see cref="VersioningExtensions.UseVersionsing(IApplicationBuilder)"/><br/>
        /// ● <see cref="SwaggerServiceExtensions.UseSwagger(IApplicationBuilder, IApiVersionDescriptionProvider)"/><br/>
        /// ● <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection(IApplicationBuilder)"/><br/>
        /// ● <see cref="AuthorizationAppBuilderExtensions.UseAuthorization(IApplicationBuilder)"/><br/>
        /// ● <see cref="EndpointRoutingApplicationBuilderExtensions.UseRouting(IApplicationBuilder)"/><br/>
        /// ● <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, System.Action{Microsoft.AspNetCore.Routing.IEndpointRouteBuilder})"/>
        /// </para>
        /// </summary>
        /// <param name="app">application configuration builder</param>
        /// <param name="provider">provider that discovers and describes API version information within an application.</param>
        /// <returns>application configuration builder</returns>
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
