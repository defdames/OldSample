using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for OVERHEADTest and is intended
    ///to contain all OVERHEADTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OVERHEADTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for OverheadGLRangeByOrganizationId
        ///</summary>
        [TestMethod()]
        public void OverheadGLRangeByOrganizationIdTest()
        {
            long organizationID = 0; // TODO: Initialize to an appropriate value
            Entities context = null; // TODO: Initialize to an appropriate value
            IQueryable<OVERHEAD_GL_RANGE_V> expected = null; // TODO: Initialize to an appropriate value
            IQueryable<OVERHEAD_GL_RANGE_V> actual;
            actual = OVERHEAD.OverheadGLRangeByOrganizationId(organizationID, context);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
