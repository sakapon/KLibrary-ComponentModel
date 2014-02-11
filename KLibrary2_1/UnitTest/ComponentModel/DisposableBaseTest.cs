using KLibrary.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.ComponentModel
{
    [TestClass]
    public class DisposableBaseTest
    {
        [TestMethod]
        public void Dispose_True1()
        {
            var isManagedResourceReleased = false;
            var isUnmanagedResourceReleased = false;

            using (var obj = new Disposable1())
            {
                obj.ReleaseManagedResource = () => isManagedResourceReleased = true;
                obj.ReleaseUnmanagedResource = () => isUnmanagedResourceReleased = true;
            }

            Assert.AreEqual(true, isManagedResourceReleased);
            Assert.AreEqual(true, isUnmanagedResourceReleased);
        }

        [TestMethod]
        public void Dispose_True2()
        {
            var isManagedResourceReleased = false;
            var isUnmanagedResourceReleased = false;

            var obj = new Disposable1();
            obj.ReleaseManagedResource = () => isManagedResourceReleased = true;
            obj.ReleaseUnmanagedResource = () => isUnmanagedResourceReleased = true;
            obj.Dispose();

            Assert.AreEqual(true, isManagedResourceReleased);
            Assert.AreEqual(true, isUnmanagedResourceReleased);
        }

        [TestMethod]
        public void Dispose_False()
        {
            var isManagedResourceReleased = false;
            var isUnmanagedResourceReleased = false;

            var obj = new Disposable1();
            obj.ReleaseManagedResource = () => isManagedResourceReleased = true;
            obj.ReleaseUnmanagedResource = () => isUnmanagedResourceReleased = true;

            Assert.AreEqual(false, isManagedResourceReleased);
            Assert.AreEqual(false, isUnmanagedResourceReleased);

            Assert.Inconclusive("デバッグして、Disposable1.Dispose メソッドを目視で確認してください。");
        }
    }

    public class Disposable1 : DisposableBase
    {
        public Action ReleaseManagedResource { get; set; }
        public Action ReleaseUnmanagedResource { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // マネージ リソースを解放します。
                if (ReleaseManagedResource != null)
                {
                    ReleaseManagedResource();
                }
            }

            // アンマネージ リソースを解放します。
            if (ReleaseUnmanagedResource != null)
            {
                ReleaseUnmanagedResource();
            }
        }
    }
}
