using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for HRTest and is intended
    ///to contain all HRTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HRTest
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
        ///A test for OrganizationList
        ///</summary>
        [TestMethod()]
        public void OrganizationListTest()
        {
            List<HR.ORGANIZATION> actual;
            actual = HR.OrganizationList();
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        ///A test for ActiveOrganizationList
        ///</summary>
        [TestMethod()]
        public void ActiveOrganizationListTest()
        {
            List<HR.ORGANIZATION> actual;
            actual = HR.ActiveOrganizationList();
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
