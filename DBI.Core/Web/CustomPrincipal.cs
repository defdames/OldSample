using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Core.Web
{
    public class CustomPrincipal : System.Security.Principal.IPrincipal
    {
        private CustomIdentity identityValue;

        public System.Security.Principal.IIdentity Identity
        {
            get { return identityValue; }
        }

        public bool IsInRole(string role)
        {
            return role == identityValue.Role.ToString();
        }

        public CustomPrincipal(string name, string password)
        {
	    identityValue = new CustomIdentity(name, password);
        }
    }
}
