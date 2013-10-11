using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
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
        /// Execute authentication check against Active Directory.
        /// </summary>
        /// <param name="username">Windows active directory username</param>
        /// <param name="password">Windows active directory password</param>
        /// <returns>Boolean</returns>
        public static bool Authenticate(string username, string password)
        {
            //First, create a new return variable
            bool _authenticated = true;

            //Next, create a new context for the domain
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
            {
                //Next, attempt to validate the credentials
                _authenticated = ctx.ValidateCredentials(username, password);
            }

            //Finally, return the return variable
            return _authenticated;
        }

        /// <summary>
        /// Generates a security token based on claims security
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static SessionSecurityToken GenerateSessionSecurityToken(List<Claim> claims)
        {
            var _id = new ClaimsIdentity(claims, "Forms");
            var _cp = new ClaimsPrincipal(_id);

            var token = new SessionSecurityToken(_cp);
            SessionSecurityToken _token = new SessionSecurityToken(_cp);
            return _token;
        }

    }
}
