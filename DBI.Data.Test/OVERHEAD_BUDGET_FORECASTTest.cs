﻿using DBI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DBI.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for OVERHEAD_BUDGET_FORECASTTest and is intended
    ///to contain all OVERHEAD_BUDGET_FORECASTTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OVERHEAD_BUDGET_FORECASTTest
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
        ///A test for ImportActualForBudgetVersion
        ///</summary>
        [TestMethod()]
        public void ImportActualForBudgetVersionTest2()
        {
            //Entities context = new Entities(); // TODO: Initialize to an appropriate value
            //List<string> periodsToImport = new List<string>();
            //string monthToTest1 = "1";
            //string monthToTest2 = "2";
            //string monthToTest3 = "3";
            //string monthToTest4 = "4";
            //string monthToTest5 = "5";
            //string monthToTest6 = "6";
            //string monthToTest7 = "7";
            //string monthToTest8 = "8";
            //string monthToTest9 = "9";
            //string monthToTest10 = "10";
            //string monthToTest11 = "11";
            //string monthToTest12 = "12";
            //periodsToImport.Add(monthToTest1);
            //periodsToImport.Add(monthToTest2);
            //periodsToImport.Add(monthToTest3);
            //periodsToImport.Add(monthToTest4);
            //periodsToImport.Add(monthToTest5);
            //periodsToImport.Add(monthToTest6);
            //periodsToImport.Add(monthToTest7);
            //periodsToImport.Add(monthToTest8);
            //periodsToImport.Add(monthToTest9);
            //periodsToImport.Add(monthToTest10);
            //periodsToImport.Add(monthToTest11);
            //periodsToImport.Add(monthToTest12);
            //long budgetid = 187; // TODO: Initialize to an appropriate value
            //string lockImportData = "Y";
            //string loggedInUser = "LJANKOWSKI";// TODO: Initialize to an appropriate value
            //bool actual;
            //actual = OVERHEAD_BUDGET_FORECAST.ImportActualForBudgetVersion(context, periodsToImport, budgetid, loggedInUser, lockImportData);
        }



        /// <summary>
        ///A test for OrganizationBudgetsByHierarchy
        ///</summary>
        [TestMethod()]
        public void OrganizationBudgetsByHierarchyTest()
        {
            Entities context = new Entities(); // TODO: Initialize to an appropriate value
            long hierarchyID = 64; // TODO: Initialize to an appropriate value
            long leOrganizationID = 121; // TODO: Initialize to an appropriate value
            bool withSecurity = true; // TODO: Initialize to an appropriate value
            bool withEMSSecurity = false; // TODO: Initialize to an appropriate value
            List<OVERHEAD_BUDGET_FORECAST.BUDGET_VERSION> actual;
            actual = OVERHEAD_BUDGET_FORECAST.OrganizationBudgetsByHierarchy(context, hierarchyID, leOrganizationID, withSecurity, withEMSSecurity);
            Assert.AreEqual(1, 1);
        }

        
    }
}
