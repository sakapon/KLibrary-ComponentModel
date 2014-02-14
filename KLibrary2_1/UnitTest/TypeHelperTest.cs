using KLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class TypeHelperTest
    {
        [TestMethod]
        public void GetDefaultValue_1()
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
    }
}
