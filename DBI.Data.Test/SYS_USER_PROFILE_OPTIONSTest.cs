using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for SYS_USER_PROFILE_OPTIONSTest and is intended
    ///to contain all SYS_USER_PROFILE_OPTIONSTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SYS_USER_PROFILE_OPTIONSTest
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
        ///A test for ProfileOptionsByType
        ///</summary>
        [TestMethod()]
        public void ProfileOptionsByTypeTest()
        {
            List<SYS_USER_PROFILE_OPTIONS.SYS_PROFILE_OPTIONS_V2> actual;
            actual = SYS_USER_PROFILE_OPTIONS.ProfileOptionsByType();
            Assert.IsTrue(actual.Count >= 0);
        }
    }
}
