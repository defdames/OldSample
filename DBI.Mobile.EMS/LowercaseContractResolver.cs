using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBI.Mobile.EMS
{
    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}