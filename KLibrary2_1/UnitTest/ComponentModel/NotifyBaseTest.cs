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
            var firstNameNotifiedCount = 0;
            var lastNameNotifiedCount = 0;
            var fullNameNotifiedCount = 0;
            var birthdayNotifiedCount = 0;

            var now1 = DateTime.Now;
            var now2 = now1.AddSeconds(1);

            var target = new Person
            {
                Id = 1,
                FirstName = "Ichiro",
                LastName = "Tokyo",
                Birthday = now1,
            };
            target.AddPropertyChangedHandler("Id", () => { idNotifiedCount++; });
            target.AddPropertyChangedHandler("FirstName", () => { firstNameNotifiedCount++; });
            target.AddPropertyChangedHandler("LastName", () => { lastNameNotifiedCount++; });
            target.AddPropertyChangedHandler("FullName", () => { fullNameNotifiedCount++; });
            target.AddPropertyChangedHandler("Birthday", () => { birthdayNotifiedCount++; });

            target.Id = 1;
            target.FirstName = "Ichiro";
            target.LastName = "Tokyo";
            target.Birthday = now1;

            Assert.AreEqual(1, target.Id);
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
        int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id == value) return;
                _Id = value;
                NotifyPropertyChanged();
            }
        }

        string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName == value) return;
                _FirstName = value;
                NotifyPropertyChanged();
            }
        }

        string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName == value) return;
                _LastName = value;
                NotifyPropertyChanged();
            }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
        }

        DateTime _Birthday;
        public DateTime Birthday
        {
            get { return _Birthday; }
            set
            {
                if (_Birthday == value) return;
                _Birthday = value;
                NotifyPropertyChanged();
            }
        }

        public Person()
        {
            AddPropertyChangedHandler("FirstName", () => NotifyPropertyChanged("FullName"));
            AddPropertyChangedHandler("LastName", () => NotifyPropertyChanged("FullName"));
        }
    }
}
