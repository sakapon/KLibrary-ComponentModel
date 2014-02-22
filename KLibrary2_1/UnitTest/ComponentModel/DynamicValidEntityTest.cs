using KLibrary.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.ComponentModel
{
    [TestClass]
    public class DynamicValidEntityTest
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

            var entity = new DynamicValidEntity();
            entity.DefineProperty("Id", x => x is int && (int)x >= 0, 0);
            entity.DefineProperty("FirstName", x => x is string && x != null, "");
            entity.DefineProperty("LastName", x => x is string && x != null, "");
            entity.DefineProperty("Birthday", x => x is DateTime && (DateTime)x < DateTime.Now, DateTime.MinValue);
            entity.DefineGetProperty("FullName", x => string.Format("{0} {1}", x.FirstName, x.LastName).Trim());
            entity.DefinePropertiesDependency("FirstName", "FullName");
            entity.DefinePropertiesDependency("LastName", "FullName");

            dynamic target = entity;
            target.FirstName = "Ichiro";
            target.LastName = "Tokyo";
            target.Birthday = now1;

            entity.AddPropertyChangedHandler("Id", () => { idNotifiedCount++; });
            entity.AddPropertyChangedHandler("FirstName", () => { firstNameNotifiedCount++; });
            entity.AddPropertyChangedHandler("LastName", () => { lastNameNotifiedCount++; });
            entity.AddPropertyChangedHandler("FullName", () => { fullNameNotifiedCount++; });
            entity.AddPropertyChangedHandler("Birthday", () => { birthdayNotifiedCount++; });

            target.Id = 0;
            target.FirstName = "Ichiro";
            target.LastName = "Tokyo";
            target.Birthday = now1;

            Assert.AreEqual(0, target.Id);
            Assert.AreEqual("Ichiro", target.FirstName);
            Assert.AreEqual("Tokyo", target.LastName);
            Assert.AreEqual("Ichiro Tokyo", target.FullName);
            Assert.AreEqual(now1, target.Birthday);
            Assert.AreEqual(0, idNotifiedCount);
            Assert.AreEqual(0, firstNameNotifiedCount);
            Assert.AreEqual(0, lastNameNotifiedCount);
            Assert.AreEqual(0, fullNameNotifiedCount);
            Assert.AreEqual(0, birthdayNotifiedCount);

            target.Id = 123;
            target.FirstName = "Jiro";
            target.LastName = "Osaka";
            target.Birthday = now2;

            Assert.AreEqual(123, target.Id);
            Assert.AreEqual("Jiro", target.FirstName);
            Assert.AreEqual("Osaka", target.LastName);
            Assert.AreEqual("Jiro Osaka", target.FullName);
            Assert.AreEqual(now2, target.Birthday);
            Assert.AreEqual(1, idNotifiedCount);
            Assert.AreEqual(1, firstNameNotifiedCount);
            Assert.AreEqual(1, lastNameNotifiedCount);
            Assert.AreEqual(2, fullNameNotifiedCount);
            Assert.AreEqual(1, birthdayNotifiedCount);
        }
    }
}
