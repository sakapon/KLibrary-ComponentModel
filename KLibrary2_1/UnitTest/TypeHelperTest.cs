using KLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Reflection;

namespace UnitTest
{
    [TestClass]
    public class TypeHelperTest
    {
        [TestMethod]
        public void GetDefaultValue_Type_1()
        {
            Assert.AreEqual(default(bool), typeof(bool).GetDefaultValue());
            Assert.AreEqual(default(byte), typeof(byte).GetDefaultValue());
            Assert.AreEqual(default(short), typeof(short).GetDefaultValue());
            Assert.AreEqual(default(int), typeof(int).GetDefaultValue());
            Assert.AreEqual(default(long), typeof(long).GetDefaultValue()); // 0L
            Assert.AreEqual(default(char), typeof(char).GetDefaultValue()); // '\0'
            Assert.AreEqual(default(float), typeof(float).GetDefaultValue()); // 0F
            Assert.AreEqual(default(double), typeof(double).GetDefaultValue()); // 0D
            Assert.AreEqual(default(IntPtr), typeof(IntPtr).GetDefaultValue());
            Assert.AreEqual(default(DayOfWeek), typeof(DayOfWeek).GetDefaultValue());
            Assert.AreEqual(default(decimal), typeof(decimal).GetDefaultValue()); // 0M
            Assert.AreEqual(default(DateTime), typeof(DateTime).GetDefaultValue());
            Assert.AreEqual(default(object), typeof(object).GetDefaultValue());
            Assert.AreEqual(default(string), typeof(string).GetDefaultValue());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDefaultValue_Type_Null()
        {
            ((Type)null).GetDefaultValue();
        }

        [TestMethod]
        public void GetDefaultValue_PropertyInfo_Int1()
        {
            var property = typeof(Class1).GetProperty("Int1");
            Assert.AreEqual(0, property.GetDefaultValue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetDefaultValue_PropertyInfo_Int2()
        {
            var property = typeof(Class1).GetProperty("Int2");
            property.GetDefaultValue();
        }

        [TestMethod]
        public void GetDefaultValue_PropertyInfo_Int3()
        {
            var property = typeof(Class1).GetProperty("Int3");
            Assert.AreEqual(123, property.GetDefaultValue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetDefaultValue_PropertyInfo_Int4()
        {
            var property = typeof(Class1).GetProperty("Int4");
            property.GetDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetDefaultValue_PropertyInfo_Int5()
        {
            var property = typeof(Class1).GetProperty("Int5");
            property.GetDefaultValue();
        }

        [TestMethod]
        public void GetDefaultValue_PropertyInfo_String1()
        {
            var property = typeof(Class1).GetProperty("String1");
            Assert.AreEqual(null, property.GetDefaultValue());
        }

        [TestMethod]
        public void GetDefaultValue_PropertyInfo_String2()
        {
            var property = typeof(Class1).GetProperty("String2");
            Assert.AreEqual(null, property.GetDefaultValue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetDefaultValue_PropertyInfo_String3()
        {
            var property = typeof(Class1).GetProperty("String3");
            property.GetDefaultValue();
        }

        [TestMethod]
        public void GetDefaultValue_PropertyInfo_String4()
        {
            var property = typeof(Class1).GetProperty("String4");
            Assert.AreEqual("abc", property.GetDefaultValue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetDefaultValue_PropertyInfo_String5()
        {
            var property = typeof(Class1).GetProperty("String5");
            property.GetDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDefaultValue_PropertyInfo_Null()
        {
            TypeHelper.GetDefaultValue((PropertyInfo)null);
        }

        class Class1
        {
            public int Int1 { get; set; }
            [DefaultValue(null)]
            public int Int2 { get; set; }
            [DefaultValue(123)]
            public int Int3 { get; set; }
            [DefaultValue(123.45)]
            public int Int4 { get; set; }
            [DefaultValue("abc")]
            public int Int5 { get; set; }

            public string String1 { get; set; }
            [DefaultValue(null)]
            public string String2 { get; set; }
            [DefaultValue(123)]
            public string String3 { get; set; }
            [DefaultValue("abc")]
            public string String4 { get; set; }
            [DefaultValue(typeof(object))]
            public string String5 { get; set; }
        }
    }
}
