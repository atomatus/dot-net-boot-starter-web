using Com.Atomatus.Bootstarter.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private static IApplicationBuilder UseDeveloperExceptionPageInDevelopment(
            [NotNull] this IApplicationBuilder app,
            [AllowNull] ref IWebHostEnvironment env)
        {
            env ??= app.ApplicationServices.GetService<IWebHostEnvironment>();
            return env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app;
        }

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
        /// ● <see cref="SwaggerServiceExtensions.UseSwagger(IApplicationBuilder, IWebHostEnvironment, IApiVersionDescriptionProvider)"/><br/>
        /// ● <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection(IApplicationBuilder)"/><br/>
        /// ● <see cref="EndpointRoutingApplicationBuilderExtensions.UseRouting(IApplicationBuilder)"/><br/>
        /// ● <see cref="AuthAppBuilderExtensions.UseAuthentication(IApplicationBuilder)"/><br/>
        /// ● <see cref="AuthorizationAppBuilderExtensions.UseAuthorization(IApplicationBuilder)"/><br/>
        /// ● <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, System.Action{Microsoft.AspNetCore.Routing.IEndpointRouteBuilder})"/>
        /// </para>
        /// </summary>
        /// <param name="app">application configuration builder</param>
        /// <param name="env">web hosting environment for running application</param>
        /// <param name="provider">provider that discovers and describes API version information within an application.</param>
        /// <returns>application configuration builder</returns>
        public static IApplicationBuilder UseBootstarters(
            [NotNull] this IApplicationBuilder app,
            [AllowNull] IWebHostEnvironment env = null,
            [AllowNull] IApiVersionDescriptionProvider provider = null)
        {
            return app
                .UseDeveloperExceptionPageInDevelopment(ref env)
                .UseVersionsing()
                .UseSwagger(env, provider)
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        /// <typeparam name="TContext">target database context</typeparam>
        /// <param name="app">application configuration builder</param>
        /// <returns>application configuration builder</returns>/// <returns></returns>
        public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
            where TContext : ContextBase
        {
            app.ApplicationServices.RequireMigration<TContext>();
            return app;
        }
    }
}
