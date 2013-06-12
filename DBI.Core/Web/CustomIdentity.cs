using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;

using DBI.Core.Security;

namespace DBI.Core.Web
{
    public class CustomIdentity : System.Security.Principal.IIdentity
    {

        private string nameValue;
        private Boolean authenticatedValue;
        private BuiltInRole roleValue;

        public string AuthenticationType
        {
            get { return "Custom Authentication"; }
        }

        public bool IsAuthenticated
        {
            get { return authenticatedValue; }
        }

        public string Name
        {
            get { return nameValue; }
        }

        public BuiltInRole Role
        {
            get { return roleValue; }
        }

        private bool IsValidUser(string username, string password)
        {
            return Authentication.WindowsAuthenticate(username, password);
        }

        public CustomIdentity(string name, string password)
	    {
		if (IsValidUser(name, password)) {
			nameValue = name;
			authenticatedValue = true;
            roleValue = Microsoft.VisualBasic.ApplicationServices.BuiltInRole.User;
		} else {
			nameValue = "";
			authenticatedValue = false;
            roleValue = Microsoft.VisualBasic.ApplicationServices.BuiltInRole.Guest;
		}
	    }
    }
}
