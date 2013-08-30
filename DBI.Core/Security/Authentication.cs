using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

///
/// <summary>
/// This namespace controls all ascepts
namespace DBI.Core.Security
{
    /// <summary>
    /// Authentication class used to verify usernames and passwords.
    /// </summary>
    /// <remarks>This class allows you to authenticate or verify your user account and password against all DBI systems, currently it's only windows domain supported.</remarks>
    public class Authentication
    {
        /// <summary>
        /// DBI Domain address used for authentication procedures
        /// </summary>
        public const string DomainAddress = "S0026.dbiservices.com";


        /// <summary>
        /// Execute authentication check against Active Directory.
        /// </summary>
        /// <param name="username">Windows active directory username</param>
        /// <param name="password">Windows active directory password</param>
        /// <returns>Boolean</returns>
        public static bool WindowsAuthenticate(string username, string password)
        {
            //First, create a new return variable
            bool returnValue = true;

            //Next, create a new context for the domain
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
            {
                //Next, attempt to validate the credentials
                returnValue = ctx.ValidateCredentials(username, password);
            }

            //Finally, return the return variable
            return returnValue;

        }

        public static bool IsUserAccountLockedOut(string username, ref bool bUserLocked)
        {
            //First, create a new return variable
            bool bReturn = true;

            //Default the user locked out flag
            bUserLocked = true;

            try
            {
                //Next, create the domain principal context object
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
                {
                    //Next, create the user, attempt to find the user by the user name
                    UserPrincipal usr = UserPrincipal.FindByIdentity(ctx, username);

                    //Next, check if the user was found
                    if ((usr != null))
                    {

                        //User found, check if the account is locked out or not
                        bUserLocked = usr.BadLogonCount < 12;

                        //Success, return true
                        bReturn = true;
                    }
                    else
                    {
                        //User not found, so return false
                        bReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Error, just return false and log 
                bReturn = false;
            }

            //Finally, return the return variable
            return bReturn;
        }

       

    }
}
