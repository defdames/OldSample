﻿using DBI.Data.GMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for ServiceUnitDataTest and is intended
    ///to contain all ServiceUnitDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceUnitDataTest
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
        ///A test for ServiceUnitTypes
        ///</summary>
        [TestMethod()]
        public void ServiceUnitTypesTest()
        {
            string rrType = "UP"; // TODO: Initialize to an appropriate value
            List<ServiceUnitResponse> actual;
            actual = ServiceUnitData.ServiceUnitTypes(rrType);
            Assert.IsTrue(actual.Count > 0);
        }



        /// <summary>
        ///A test for ServiceUnitTypes
        ///</summary>
        [TestMethod()]
        public void ServiceUnitTypesTest1()
        {
            List<ServiceUnitResponse> actual;
            actual = ServiceUnitData.ServiceUnitTypes();
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        ///A test for ServiceUnitUnits
        ///</summary>
        [TestMethod()]
        public void ServiceUnitUnitsTest()
        {
            string proj = "UP"; // TODO: Initialize to an appropriate value
            List<ServiceUnitResponse> actual;
            actual = ServiceUnitData.ServiceUnitUnits(proj);
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
