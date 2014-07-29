using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for OVERHEAD_BUDGET_TYPESTest and is intended
    ///to contain all OVERHEAD_BUDGET_TYPESTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OVERHEAD_BUDGET_TYPESTest
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
        ///A test for NextAvailBudgetTypeByOrganization
        ///</summary>
        [TestMethod()]
        public void NextAvailBudgetTypeByOrganizationTest()
        {
            long organizationID = 121; // TODO: Initialize to an appropriate value
            long fiscalYear = 2014; // TODO: Initialize to an appropriate value
            List<OVERHEAD_BUDGET_TYPE> actual;
            actual = OVERHEAD_BUDGET_TYPES.NextAvailBudgetTypeByOrganization(organizationID, fiscalYear);
            Assert.IsTrue(actual.Count >= 0);
        }
    }
}
