using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Base class for all services implementing EntityFramework data access.
    /// </summary>
    /// <typeparam name="TLoggerCategory">The type who's name is used
    /// for the logger category name.</typeparam>
    /// <typeparam name="TDbContext">The type of DbContext.</typeparam>
    public abstract class ApiServiceEF<TLoggerCategory, TDbContext>
        : ApiService<TLoggerCategory>
            where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="args">Encapsulates the properties to initialize a new
        /// ApiService&lt;TLoggerCategory&gt;.</param>
        public ApiServiceEF(ApiServiceArgs<TLoggerCategory> args)
            : base(args) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="args">Encapsulates the properties to initialize a new
        /// ApiServiceEF&lt;TLoggerCategory, TDbContext&gt;.</param>
        public ApiServiceEF(ApiServiceArgsEF<TLoggerCategory, TDbContext> args)
            : base(args)
            => DbContext = args.DbContext;

        /// <summary>
        /// Reference to the current DbContext instance.
        /// </summary>
        protected TDbContext DbContext { get; private set; }

        /// <summary>
        /// Performs an Add/Insert/Delete of an entity into the DbContext.
        /// </summary>
        /// <param name="operation">The type of operation to perform
        /// on the DbContext.</param>
        /// <param name="entity">The entity to Add/Insert/Delete.</param>
        /// <param name="commit">Indicates wheather changes should be
        /// commited immediately.</param>
        /// <returns>The task object representing the asynchronous operation.
        /// </returns>
        protected virtual async Task SaveAsync(
            ApiOperationType operation,
            object entity,
            bool commit = true)
        {
            switch (operation)
            {
                case ApiOperationType.Insert:
                case ApiOperationType.BulkInsert:
                    DbContext.Add(entity);
                    break;
                case ApiOperationType.Update:
                case ApiOperationType.BulkUpdate:
                    DbContext.Update(entity);
                    break;
                case ApiOperationType.Delete:
                case ApiOperationType.BulkDelete:
                    DbContext.Remove(entity);
                    break;
            }

            if (commit && operation != ApiOperationType.Read)
                await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Detaches all tracked entities from the DbContext.
        /// </summary>
        /// <param name="excludedEntities">List of entities to be excluded in the
        /// detaching proccess.</param>
        protected void DetachEntities(IEnumerable<object> excludedEntities = null)
        {
            if (DbContext != null)
                foreach (var entry in DbContext.ChangeTracker.Entries().ToList())
                    if (entry.Entity != null && (excludedEntities == null || !excludedEntities.Contains(entry)))
                        entry.State = EntityState.Detached;
        }
    }
}
