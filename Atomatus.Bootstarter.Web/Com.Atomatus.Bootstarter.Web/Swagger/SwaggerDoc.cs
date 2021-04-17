using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <para>
    /// Swagger doc informations.
    /// </para>
    /// <para>
    /// To create an instance use <see cref="Factory.Create(IConfiguration, string)"/>.
    /// </para>
    /// <para>
    /// Bellow an example how to implement swaggerdoc information in appsetting.json.<br/>
    /// Whether not defined it, the <see cref="Factory"/> will try to create it from assembly definition.
    /// </para>
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
    /// </summary>
    public sealed class SwaggerDoc
    {
        /// <summary>
        /// Implicit conversion of SwaggerDoc to OpenApiInfo.
        /// </summary>
        /// <param name="doc"></param>
        public static implicit operator OpenApiInfo(SwaggerDoc doc)
        {
            return new OpenApiInfo
            {
                Version = doc.Version,
                Title = doc.Title,
                Description = doc.Description,
                Contact = doc.Author
            };
        }

        /// <summary>
        /// <para>
        /// Factory to generate a swaggerdoc from appsettings.json or from assembly info.
        /// </para>
        /// <para>
        /// Bellow an example how to implement swaggerdoc information in appsetting.json.<br/>
        /// Whether not defined it, the Factory will try to create it from assembly definition.
        /// </para>
        /// <code>
        /// "SwaggerDoc": {<br/>
        /// "Title": "Your API Name",<br/>
        /// "Description": "Your API description",<br/>
        /// "Author": {<br/>
        /// "Name": "Author name",<br/>
        /// "Email": "author@mail.com",<br/>
        /// "Url": "https://yourprojectaddress"<br/>
        /// }}<br/>
        /// </code>
        /// </summary>
        public static class Factory
        {
            /// <summary>
            /// Create an instance of <see cref="SwaggerDoc"/> from
            /// appsettings.json definition or from current entry assembly definition.
            /// </summary>
            /// <param name="configuration">appsetings.json configuration values</param>
            /// <param name="version">swagger api version, when null will try to load from configuration or assembly</param>
            /// <returns></returns>
            public static SwaggerDoc Create([NotNull] IConfiguration configuration, string version = null)
            {
                return new SwaggerDoc
                {
                    Version = version ?? configuration.ResolveSwaggerDoc(
                        a => a.GetName().Version.ToString(), 
                        "Version"),

                    Title = configuration.ResolveSwaggerDoc(
                        a => a.GetName().Name, 
                        "Title"),                    


                    Description = configuration.ResolveSwaggerDoc(
                        a => a.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                              .OfType<AssemblyDescriptionAttribute>()
                              .FirstOrDefault()?
                              .Description,  
                        "Description", "Desc"),

                    Author = new SwaggerAuthor
                    {
                        Name = configuration.ResolveSwaggerDoc(
                            a => FileVersionInfo.GetVersionInfo(a.Location).CompanyName, 
                            "Author:Name", "AuthorName", "Author"),

                        Email = configuration.ResolveSwaggerDoc(null, 
                            "Author:Email", "AuthorEmail", "Email"),

                        Url = Uri.TryCreate(
                                configuration.ResolveSwaggerDoc(null, 
                                    "Author:Url", "AuthorUrl", "Url"), 
                                UriKind.Absolute, 
                                out Uri uri) ? uri : default
                    }                                        
                };
            }
        }

        /// <summary>
        /// Swagger author.
        /// </summary>
        public sealed class SwaggerAuthor
        {
            /// <summary>
            /// Author name.
            /// </summary>
            public string Name { get; set; }
            
            /// <summary>
            /// Author e-mail.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Author url page.
            /// </summary>
            public Uri Url { get; set; }
            
            /// <summary>
            /// Implicit conversion of SwaggerAuthor to OpenApiContact.
            /// </summary>
            /// <param name="author"></param>

            public static implicit operator OpenApiContact(SwaggerAuthor author)
            {
                return new OpenApiContact
                {
                    Name = author.Name,
                    Email = author.Email,
                    Url = author.Url,
                };
            }
        }

        /// <summary>
        /// Application title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// A short description of application.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The version of API.
        /// </summary>
        public string Version { get; private set; }
        
        /// <summary>
        /// The author infrormation.
        /// </summary>
        public SwaggerAuthor Author { get; private set; }

        private SwaggerDoc() { }        
    }
}