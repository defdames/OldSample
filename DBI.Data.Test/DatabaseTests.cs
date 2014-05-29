using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DBI.Data.Test
{
    [TestClass]
    public class DatabaseTests 
    {

        [TestMethod]
        public void setProfileOptionTest()
        {
            try
            {
                string _profile_option_name = "UserCrossingSelectedValue";
                long _user_id = 1154;
                string _new_profile_value = "UP";

                using (TransactionScope _transaction = new TransactionScope())
                {
                    SYS_USER_PROFILE_OPTIONS.setProfileOption(_profile_option_name, _user_id, _new_profile_value);
                }
            }
            catch (DBICustomException ex)
            {
                Assert.IsTrue(ex is DBICustomException);
            }

            catch (Exception ex)
            {
                Assert.Fail(string.Format("Invalid Exception was thrown: {0}", ex.Message.ToString()));
            }
           
        }

        [TestMethod]
        public void userProfileOptionTest()
        {
            string _profile_option_name = "UserCrossingSelectedValue";
            long _user_id = 12501;
            string _value = SYS_USER_PROFILE_OPTIONS.userProfileOption(_profile_option_name,_user_id);
            Assert.IsTrue(_value.Length > 0);
        }

        [TestMethod]
        public void laborBurdenTest()
        {
            string _buid = "12231";
            string _fiscalYear = "2013";
            decimal _value = DBI.Data.Oracle.PA.laborBurden(_buid, _fiscalYear);
            Assert.IsTrue(_value > 0);
        }

    }
}
