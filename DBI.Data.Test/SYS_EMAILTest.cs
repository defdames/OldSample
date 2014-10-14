using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for SYS_EMAILTest and is intended
    ///to contain all SYS_EMAILTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SYS_EMAILTest
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
        ///A test for DARSubmittedEmail
        ///</summary>
        [TestMethod()]
        public void DARSubmittedEmailTest()
        {
             List<SYS_EMAIL> _list =  SYS_EMAIL.DARSubmittedEmail(3677);

             SYS_EMAIL.GenerateEmailAndProcess(_list);

             Assert.IsTrue(_list.Count > 0);
        }
    }
}
