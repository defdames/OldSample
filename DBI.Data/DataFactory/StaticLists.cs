using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ext.Net;

namespace DBI.Data.DataFactory
{
    /// <summary>
    /// This data class returns static values that can be used in programming web systems for DBI
    /// </summary>
    public class StaticLists
    {
        /// <summary>
        /// This function returns a list of supported languages currently being used for the DBI Enterprise Management System.
        /// </summary>
        /// <returns>object</returns>
        public static object GetSupportedLanguages()
        {
            object languages = new object[]
            {
                new object[] { ResourceManager.GetIconClassName(Icon.FlagCa), "Canada"},
                new object[] { ResourceManager.GetIconClassName(Icon.FlagFr), "French Canadian"},
                new object[] { ResourceManager.GetIconClassName(Icon.FlagGb), "Great Britain"},
                new object[] { ResourceManager.GetIconClassName(Icon.FlagUs), "United States"},
                new object[] { ResourceManager.GetIconClassName(Icon.FlagAr), "United Arab Emirates"},
            };
            return languages;
        }


    }
}
