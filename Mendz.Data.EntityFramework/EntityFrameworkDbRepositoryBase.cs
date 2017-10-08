using Microsoft.EntityFrameworkCore;
using System;

namespace Mendz.Data.EntityFramework
{
    /// <summary>
    /// The base repository.
    /// </summary>
    /// <typeparam name="D">The database context.</typeparam>
    /// <remarks>
    /// If the DbContext instance is in its own class library, implementation projects should reference it.
    /// It is an option for implementation projects to contain/include the DbContext instance code.
    /// This is an overkill bastardization of EF usage that wraps the DbContext instance
    /// just so the application can follow Mendz.Data-based repository's coding pattern.
    /// There are pros and cons to doing that...
    /// If using EF, it is still recommended that you use it directly in your applications.
    /// Note that EF already implements the Unit of Work and Repository patterns internally.
    /// </remarks>
    public abstract class EntityFrameworkDbRepositoryBase<D> : IDisposable
        where D : DbContext, new()
    {
        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        protected D DbDataContext { get; set; }

        /// <summary>
        /// Gets or sets if the current instance is the owner of the database context.
        /// </summary>
        protected bool DbDataContextOwner { get; set; } = false;

        /// <summary>
        /// Creates a repository that owns a database context instance.
        /// </summary>
        protected EntityFrameworkDbRepositoryBase() => CreateDbDataContext();

        /// <summary>
        /// Creates a repository that shares a database context.
        /// </summary>
        /// <param name="dbDataContext">The database context to share.</param>
        protected EntityFrameworkDbRepositoryBase(D dbDataContext) => DbDataContext = dbDataContext;

        protected void CreateDbDataContext()
        {
            if (DbDataContext == null)
            {
                DbDataContext = new D();
                DbDataContextOwner = true;
            }
        }

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (DbDataContextOwner)
                    {
                        DbDataContext.Dispose();
                        disposed = true;
                    }
                }
                disposed = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
