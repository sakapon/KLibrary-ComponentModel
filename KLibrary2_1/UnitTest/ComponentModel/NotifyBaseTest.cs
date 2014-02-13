using KLibrary.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.ComponentModel
{
    [TestClass]
    public class NotifyBaseTest
    {
        [TestMethod]
        public void Person_1()
        {
            var idNotifiedCount = 0;
            var nameNotifiedCount = 0;
            var birthdayNotifiedCount = 0;

            var now = DateTime.Now;
            var now2 = DateTime.Now.AddSeconds(1);

            var target = new Person
            {
                Id = 1,
                Name = "Taro",
                Birthday = now,
            };
            target.AddPropertyChangedHandler("Id", () => { idNotifiedCount++; });
            target.AddPropertyChangedHandler("Name", () => { nameNotifiedCount++; });
            target.AddPropertyChangedHandler("Birthday", () => { birthdayNotifiedCount++; });

            Assert.AreEqual(1, target.Id);
            Assert.AreEqual("Taro", target.Name);
            Assert.AreEqual(now, target.Birthday);

            target.Id = 123;
            target.Name = "Jiro";
            target.Birthday = now2;

            Assert.AreEqual(123, target.Id);
            Assert.AreEqual("Jiro", target.Name);
            Assert.AreEqual(now2, target.Birthday);

            target.Id = 123;
            target.Name = "Jiro";
            target.Birthday = now2;

            Assert.AreEqual(123, target.Id);
            Assert.AreEqual("Jiro", target.Name);
            Assert.AreEqual(now2, target.Birthday);

            Assert.AreEqual(1, idNotifiedCount);
            Assert.AreEqual(1, nameNotifiedCount);
            Assert.AreEqual(1, birthdayNotifiedCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPropertyChangedHandler_ArgumentNull()
        {
            var target = new Person();
            target.AddPropertyChangedHandler("", null);
        }
    }

    public class Person : NotifyBase
    {
        int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                NotifyPropertyChanged();
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                NotifyPropertyChanged();
            }
        }

        DateTime birthday;
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                if (birthday == value) return;
                birthday = value;
                NotifyPropertyChanged();
            }
        }
    }
}
