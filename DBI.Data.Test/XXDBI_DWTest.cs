using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for XXDBI_DWTest and is intended
    ///to contain all XXDBI_DWTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XXDBI_DWTest
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
        ///A test for jcSummaryLineAmounts
        ///</summary>
        [TestMethod()]
        public void jcSummaryLineAmountsTest()
        {
            long projectId = 0; // TODO: Initialize to an appropriate value
            string weekEndingDate = string.Empty; // TODO: Initialize to an appropriate value
            XXDBI_DW.JOB_COST_V expected = null; // TODO: Initialize to an appropriate value
            XXDBI_DW.JOB_COST_V actual;
            actual = XXDBI_DW.jcSummaryLineAmounts(projectId, weekEndingDate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
