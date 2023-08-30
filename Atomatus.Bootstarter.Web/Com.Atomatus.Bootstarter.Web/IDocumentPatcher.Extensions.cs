using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <see cref="IDocumentPatcher"/> extension methods.
    /// </summary>
    public static class IDocumentPatcherExtensions
    {
        /// <summary>
        /// <para>
        /// Copy all non null data from <paramref name="self"/> (IDocumentPatcher object) to <paramref name="target"/> object
        /// that are sharing all same Properties name.
        /// </para>
        /// <para>
        /// Property type must be compatible for one of follwing conditions: 
        /// Same type, 
        /// Derived type, 
        /// Derived interfaces or 
        /// Types with same Properties definition.
        /// </para>
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <param name="self">document patcher reference</param>
        /// <param name="target">target object to receive shared properties with current document patcher object</param>
        public static void ApplyTo<T>(this IDocumentPatcher self, [NotNull] T target) where T: class
        {
            var sourceProperties = self.GetType().GetProperties().Where(p => p.CanRead);
            var targetProperties = target.GetType().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                var value = sourceProperty.GetValue(self, null);
                if(value != null && targetProperties.FirstOrDefault(targetProperty => targetProperty.CanWrite && 
                        sourceProperty.Name.Equals(targetProperty.Name, StringComparison.InvariantCulture)) is PropertyInfo found)
                {
                    try
                    {
                        var targetValue = ObjectMapper.Parse(value, found.PropertyType);
                        found.SetValue(target, targetValue, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex, nameof(IDocumentPatcher));
#if DEBUG
                        throw;
#endif
                    }
                }
            }
        }
    }
}
