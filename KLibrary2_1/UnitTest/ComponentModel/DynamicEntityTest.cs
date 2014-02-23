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
            var lastNameNotifiedCount = 0;
            var fullNameNotifiedCount = 0;
            var birthdayNotifiedCount = 0;

            var now1 = new DateTime(2001, 1, 1);
            var now2 = now1.AddDays(1);

            var entity = new DynamicEntity();
            entity.DefineProperty("Id", -1);
            entity.DefineProperty("FirstName", "");
            entity.DefineProperty("LastName", "");
            entity.DefineGetProperty("FullName", x => string.Format("{0} {1}", x.FirstName, x.LastName).Trim(), "FirstName", "LastName");
            entity.DefineProperty("Birthday", DateTime.MinValue);

            entity.AddPropertyChangedHandler("Id", () => { idNotifiedCount++; });
            entity.AddPropertyChangedHandler("FirstName", () => { firstNameNotifiedCount++; });
            entity.AddPropertyChangedHandler("LastName", () => { lastNameNotifiedCount++; });
            entity.AddPropertyChangedHandler("FullName", () => { fullNameNotifiedCount++; });
            entity.AddPropertyChangedHandler("Birthday", () => { birthdayNotifiedCount++; });

            dynamic target = entity;
            target.Id = -1;
            target.FirstName = "Ichiro";
            target.LastName = "Tokyo";
            target.Birthday = now1;

            Assert.AreEqual(-1, target.Id);
            Assert.AreEqual("Ichiro", target.FirstName);
            Assert.AreEqual("Tokyo", target.LastName);
            Assert.AreEqual("Ichiro Tokyo", target.FullName);
            Assert.AreEqual(now1, target.Birthday);
            Assert.AreEqual(0, idNotifiedCount);
            Assert.AreEqual(1, firstNameNotifiedCount);
            Assert.AreEqual(1, lastNameNotifiedCount);
            Assert.AreEqual(2, fullNameNotifiedCount);
            Assert.AreEqual(1, birthdayNotifiedCount);

            target["Id"] = 123;
            target["FirstName"] = "Jiro";
            target["LastName"] = "Osaka";
            target["Birthday"] = now2;

            Assert.AreEqual(123, target["Id"]);
            Assert.AreEqual("Jiro", target["FirstName"]);
            Assert.AreEqual(123, target.Id);
            Assert.AreEqual("Jiro", target.FirstName);
            Assert.AreEqual("Osaka", target.LastName);
            Assert.AreEqual("Jiro Osaka", target.FullName);
            Assert.AreEqual(now2, target.Birthday);
            Assert.AreEqual(1, idNotifiedCount);
            Assert.AreEqual(2, firstNameNotifiedCount);
            Assert.AreEqual(2, lastNameNotifiedCount);
            Assert.AreEqual(4, fullNameNotifiedCount);
            Assert.AreEqual(2, birthdayNotifiedCount);
        }

        [TestMethod]
        public void GetDynamicMemberNames_1()
        {
            dynamic target = new DynamicEntity();
            target.DefineProperty("Id", -1);
            target.DefineProperty("Name", "");
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
