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

        string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName == value) return;
                firstName = value;
                NotifyPropertyChanged();
            }
        }

        string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName == value) return;
                lastName = value;
                NotifyPropertyChanged();
            }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
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

        public Person()
        {
            AddPropertyChangedHandler("FirstName", () => NotifyPropertyChanged("FullName"));
            AddPropertyChangedHandler("LastName", () => NotifyPropertyChanged("FullName"));
        }
    }
}
