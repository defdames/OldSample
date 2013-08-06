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
        public const string DomainAddress = "dbiservices.com";


        /// <summary>
        /// Execute authentication check against Active Directory.
        /// </summary>
        /// <param name="username">Windows active directory username</param>
        /// <param name="password">Windows active directory password</param>
        /// <returns>Boolean</returns>
        public static bool WindowsAuthenticate(string username, String password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, DomainAddress))
            {
                return (pc.ValidateCredentials(username, password));
            }

        }

    }
}
