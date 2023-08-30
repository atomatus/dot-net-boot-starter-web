using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Com.Atomatus.Bootstarter.Model;
using Com.Atomatus.Bootstarter.Services;
using Microsoft.Extensions.Logging;

namespace Com.Atomatus.Bootstarter.Web
{
    /// <summary>
    /// <inheritdoc/>
    /// <br/>
    /// Use this controller base to create, read, update or delete any
    /// data using DTO objects to be converting between <typeparamref name="TModel"/>
    /// and DTOs.
    /// </summary>
    /// <typeparam name="TService">target service type to data persistence</typeparam>
    /// <typeparam name="TModel">target model type</typeparam>    
    public abstract class ControllerMapperBase<TService, TModel> : ControllerBase<TService, TModel>
        where TService : IService
        where TModel : class, IModel
    {
        /// <inheritdoc/>
        protected ControllerMapperBase(TService service) : base(service) { }

        /// <inheritdoc/>
        protected ControllerMapperBase(TService service, ILogger<ControllerMapperBase<TService, TModel>> logger) : base(service, logger) { }

        /// <summary>
        ///  Copies property values from the <paramref name="source"/> object to the <paramref name="target"/> object.
        ///  This method facilitates the transfer of property values between objects, applying
        ///  a series of strategies to handle different copying scenarios.
        /// </summary>
        /// <param name="source">source object</param>
        /// <param name="target">target object to accept the values</param>
        /// <returns>True, source properties was copied to target, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throws when source or target is null</exception>
        /// <exception cref="InvalidOperationException">Throws when is not possible copy value from source to target</exception>
        protected internal bool Copy([NotNull] object source, [NotNull] object target)
        {
            return ObjectMapper.Copy(source, target);
        }

        /// <summary>
        /// Copy all properties from <paramref name="sources"/> list to <paramref name="targets"/> list.
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="sources">source object</param>
        /// <param name="targets">target object to accept the values</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected internal void CopyList<TSource, TTarget>(
            [NotNull] IList<TSource> sources,
            [NotNull] IList<TTarget> targets)
            where TSource : class, new()
            where TTarget : class, new()
        {
            if(targets.Count != 0)
            {
                throw new InvalidOperationException("Target list must be empty!");
            }

            foreach (TSource source in sources) {
                TTarget target = new TTarget();
                this.Copy(source, target);
                targets.Add(target);
            }
        }

        /// <summary>
        /// Convert <paramref name="source"/> object to target type <typeparamref name="TTarget"/>
        /// coping all shared properties between both.
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="source">source object</param>
        /// <returns>target object within all shared properties to source</returns>
        protected internal TTarget Parse<TSource, TTarget>([NotNull] TSource source)
            where TSource : class, new()
            where TTarget : class, new()
        {
            TTarget target = new TTarget();
            this.Copy(source, target);
            return target;
        }

        /// <summary>
        /// Convert <paramref name="sources"/> list to target type <typeparamref name="TTarget"/> list
        /// coping all shared properties between both.
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="sources">source object</param>
        /// <returns>target object within all shared properties to source</returns>
        protected internal IReadOnlyList<TTarget> ParseList<TSource, TTarget>([NotNull] IList<TSource> sources)
            where TSource : class, new()
            where TTarget : class, new()
        {
            List<TTarget> targets = new List<TTarget>();
            this.CopyList(sources, targets);
            return targets.AsReadOnly();
        }

    }
}
