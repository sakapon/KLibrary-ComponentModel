using System;

namespace KLibrary.ComponentModel
{
    /// <summary>
    /// Provides the base implementation for disposable objects.
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        /// <summary>
        /// Finalize this object.
        /// </summary>
        ~DisposableBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the managed and/or unmanaged resources used by this object.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Releases the managed resources.
            }

            // Releases the unmanaged resources.
        }
    }
}
