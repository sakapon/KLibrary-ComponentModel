using KLibrary.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.ComponentModel
{
    [TestClass]
    public class DependentOnAttributeTest
    {
        [TestMethod]
        public void GetTargetToSourceMap_1()
        {
            var map = DependentOnAttribute.GetTargetToSourceMap(typeof(Person));

            Assert.AreEqual(1, map.Count);
            Assert.AreEqual(2, map["FullName"].Length);
            // MEMO: Reverse order in actual.
            CollectionAssert.AreEquivalent(new[] { "FirstName", "LastName" }, map["FullName"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTargetToSourceMap_Null()
        {
            DependentOnAttribute.GetTargetToSourceMap(null);
        }

        [TestMethod]
        public void GetSourceToTargetMap_1()
        {
            var map = DependentOnAttribute.GetSourceToTargetMap(typeof(Person));

            Assert.AreEqual(2, map.Count);
            Assert.AreEqual(1, map["FirstName"].Length);
            Assert.AreEqual(1, map["LastName"].Length);
            Assert.AreEqual("FullName", map["FirstName"][0]);
            Assert.AreEqual("FullName", map["LastName"][0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSourceToTargetMap_Null()
        {
            DependentOnAttribute.GetSourceToTargetMap(null);
        }
    }
}
