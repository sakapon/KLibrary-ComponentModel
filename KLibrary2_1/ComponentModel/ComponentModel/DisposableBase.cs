using System;

namespace KLibrary.ComponentModel
{
    public abstract class DisposableBase : IDisposable
    {
        ~DisposableBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // マネージ リソースを解放します。
            }

            // アンマネージ リソースを解放します。
        }
    }
}
