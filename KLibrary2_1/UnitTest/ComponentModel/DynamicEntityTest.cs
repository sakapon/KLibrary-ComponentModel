using KLibrary.ComponentModel;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.ComponentModel
{
    [TestClass]
    public class DynamicEntityTest
    {
        [TestMethod]
        public void Person_1()
        {
            var idNotifiedCount = 0;
            var firstNameNotifiedCount = 0;

            dynamic target = new DynamicEntity();
            target.Id = -1;
            target.AddPropertyChangedHandler("Id", new Action(() => { idNotifiedCount++; }));
            target.AddPropertyChangedHandler("FirstName", new Action(() => { firstNameNotifiedCount++; }));

            target.Id = -1;
            target.FirstName = "Ichiro";

            Assert.AreEqual(-1, target.Id);
            Assert.AreEqual("Ichiro", target.FirstName);
            Assert.AreEqual(0, idNotifiedCount);
            Assert.AreEqual(1, firstNameNotifiedCount);

            target.Id = 123;
            target.FirstName = "Jiro";

            Assert.AreEqual(123, target.Id);
            Assert.AreEqual("Jiro", target.FirstName);
            Assert.AreEqual(1, idNotifiedCount);
            Assert.AreEqual(2, firstNameNotifiedCount);

            target["Id"] = 456;
            target["FirstName"] = "Saburo";

            Assert.AreEqual(456, target.Id);
            Assert.AreEqual("Saburo", target.FirstName);
            Assert.AreEqual(456, target["Id"]);
            Assert.AreEqual("Saburo", target["FirstName"]);
            Assert.AreEqual(2, idNotifiedCount);
            Assert.AreEqual(3, firstNameNotifiedCount);
        }

        [TestMethod]
        public void GetDynamicMemberNames_1()
        {
            dynamic target = new DynamicEntity();
            target.Id = null;
            target.Name = null;
            CollectionAssert.AreEquivalent(new[] { "Id", "Name" }, (System.Collections.ICollection)target.GetDynamicMemberNames());
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void TryGetMember_NotContains()
        {
            dynamic target = new DynamicEntity();
            var id = target.Id;
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void TryGetIndex_NotContains()
        {
            dynamic target = new DynamicEntity();
            var id = target["Id"];
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void TryGetIndex_InvalidIndex()
        {
            dynamic target = new DynamicEntity();
            var id = target[1];
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void TrySetIndex_InvalidIndex()
        {
            dynamic target = new DynamicEntity();
            target[1] = 123;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPropertyChangedHandler_Null()
        {
            dynamic target = new DynamicEntity();
            target.AddPropertyChangedHandler("", null);
        }
    }
}
