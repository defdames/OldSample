using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for TIME_CLOCKTest and is intended
    ///to contain all TIME_CLOCKTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TIME_CLOCKTest
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


        ///// <summary>
        /////A test for EmployeeTime
        /////</summary>
        //[TestMethod()]
        //public void EmployeeTimeTest()
        //{
        //    List<TIME_CLOCK.Employee> actual;
        //    actual = TIME_CLOCK.EmployeeTime();
        //    Assert.IsTrue(actual.Count > 0);
        //}

        ///// <summary>
        /////A test for EmployeeTimeCompletedUnapproved
        /////</summary>
        //[TestMethod()]
        //public void EmployeeTimeCompletedUnapprovedTest()
        //{
        //    Decimal supervisorId = new Decimal(301); // TODO: Initialize to an appropriate value
           
        //    List<TIME_CLOCK.Employee> actual;
        //    actual = TIME_CLOCK.EmployeeTimeCompletedUnapproved(supervisorId);
        //    Assert.IsTrue(actual.Count > 0);
        //}

        ///// <summary>
        /////A test for EmployeeTimeCompletedUnapprovedPayroll
        /////</summary>
        //[TestMethod()]
        //public void EmployeeTimeCompletedUnapprovedPayrollTest()
        //{
        
        //    List<TIME_CLOCK.Employee> actual;
        //    actual = TIME_CLOCK.EmployeeTimeCompletedUnapprovedPayroll();
        //    Assert.IsTrue(actual.Count > 0);
            
        //}
    }
}
