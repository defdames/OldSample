using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;
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

        public static object WindDirection
        {
            get
            {
                return new object[]
                {
                    new object[]{"N", "North"},
                    new object[]{"NE", "North East"},
                    new object[]{"NW", "North West"},
                    new object[]{"S", "South"},
                    new object[]{"SE", "South East"},
                    new object[]{"SW", "South West"},
                    new object[]{"E", "East"},
                    new object[]{"W", "West"},
                };
            }
        }

        public static object StateList
        {
            get
            {
                return new object[]
            {
                new object[] { "AL", "Alabama" },
                new object[] { "AK", "Alaska" },
                new object[] { "AZ", "Arizona" },
                new object[] { "AR", "Arkansas" },
                new object[] { "CA", "California" },
                new object[] { "CO", "Colorado" },
                new object[] { "CT", "Connecticut" },
                new object[] { "DE", "Delaware" },
                new object[] { "DC", "District of Columbia" },
                new object[] { "FL", "Florida" },
                new object[] { "GA", "Georgia" },
                new object[] { "HI", "Hawaii" },
                new object[] { "ID", "Idaho" },
                new object[] { "IL", "Illinois" },
                new object[] { "IN", "Indiana" },
                new object[] { "IA", "Iowa" },
                new object[] { "KS", "Kansas" },
                new object[] { "KY", "Kentucky" },
                new object[] { "LA", "Louisiana" },
                new object[] { "ME", "Maine" },
                new object[] { "MD", "Maryland" },
                new object[] { "MA", "Massachusetts" },
                new object[] { "MI", "Michigan" },
                new object[] { "MN", "Minnesota" },
                new object[] { "MS", "Mississippi" },
                new object[] { "MO", "Missouri" },
                new object[] { "MT", "Montana" },
                new object[] { "NE", "Nebraska" },
                new object[] { "NV", "Nevada" },
                new object[] { "NH", "New Hampshire" },
                new object[] { "NJ", "New Jersey" },
                new object[] { "NM", "New Mexico" },
                new object[] { "NY", "New York" },
                new object[] { "NC", "North Carolina" },
                new object[] { "ND", "North Dakota" },
                new object[] { "OH", "Ohio" },
                new object[] { "OK", "Oklahoma" },
                new object[] { "OR", "Oregon" },
                new object[] { "PA", "Pennsylvania" },
                new object[] { "RI", "Rhode Island" },
                new object[] { "SC", "South Carolina" },
                new object[] { "SD", "South Dakota" },
                new object[] { "TN", "Tennessee" },
                new object[] { "TX", "Texas" },
                new object[] { "UT", "Utah" },
                new object[] { "VT", "Vermont" },
                new object[] { "VA", "Virginia" },
                new object[] { "WA", "Washington" },
                new object[] { "WV", "West Virginia" },
                new object[] { "WI", "Wisconsin" },
                new object[] { "WY", "Wyoming" } 
            };
            }
        }
        public static object CrossingStateList
        {
            get
            {
                return new object[]
            {
                new object[] { "AL", "AL" },
                new object[] { "AK", "AK" },
                new object[] { "AZ", "AZ" },
                new object[] { "AR", "AR" },
                new object[] { "CA", "CA" },
                new object[] { "CO", "CO" },
                new object[] { "CT", "CT" },
                new object[] { "DE", "DE" },
                new object[] { "DC", "DC" },
                new object[] { "FL", "FL" },
                new object[] { "GA", "GA" },
                new object[] { "HI", "HI" },
                new object[] { "ID", "ID" },
                new object[] { "IL", "IL" },
                new object[] { "IN", "IN" },
                new object[] { "IA", "IA" },
                new object[] { "KS", "KS" },
                new object[] { "KY", "KY" },
                new object[] { "LA", "LA" },
                new object[] { "ME", "ME" },
                new object[] { "MD", "MD" },
                new object[] { "MA", "MA" },
                new object[] { "MI", "MI" },
                new object[] { "MN", "MN" },
                new object[] { "MS", "MS" },
                new object[] { "MO", "MO" },
                new object[] { "MT", "MT" },
                new object[] { "NE", "NE" },
                new object[] { "NV", "NV" },
                new object[] { "NH", "NH" },
                new object[] { "NJ", "NJ" },
                new object[] { "NM", "NM" },
                new object[] { "NY", "NY" },
                new object[] { "NC", "NC" },
                new object[] { "ND", "ND" },
                new object[] { "OH", "OH" },
                new object[] { "OK", "OK" },
                new object[] { "OR", "OR" },
                new object[] { "PA", "PA" },
                new object[] { "RI", "RI" },
                new object[] { "SC", "SC" },
                new object[] { "SD", "SD" },
                new object[] { "TN", "TN" },
                new object[] { "TX", "TX" },
                new object[] { "UT", "UT" },
                new object[] { "VT", "VT" },
                new object[] { "VA", "VA" },
                new object[] { "WA", "WA" },
                new object[] { "WV", "WV" },
                new object[] { "WI", "WI" },
                new object[] { "WY", "WY" } 
            };
            }
        }
        public static object SurfaceTypes
        {
            get
            {
                return new object[]{
                    new object[]{"Scratch", "Scratch"},
                    new object[]{"Millings", "Millings"},
                    new object[]{"Michigan Joint", "Michigan Joint"},
                    new object[]{"Top", "Top"},
                    new object[]{"Concrete", "Concrete"},
                    new object[]{"Slurry", "Slurry"},
                    new object[]{"Restripe", "Restripe"},
                    new object[]{"Ghost", "Ghost"}
                };
            }
        }
        public static object ServiceTypes
        {
            get
            {
                return new object[]{
                    new object[]{"SB Addl Cut", "SB Addl Cut"},
                    new object[]{"SB Addl Maintenance", "SB Addl Maintenance"},
                    new object[]{"SB EXTROW Cut", "SB EXTROW Cut"},
                    new object[]{"SB EXTROW Maintenance", "SB EXTROW Maintenance"}
                };
            }
        }
        public static object ApplicationRequested
             {
                 get
                 {
                     return new object[]{
                    new object[]{"1", "1"},
                    new object[]{"2", "2"},
                    new object[]{"3", "3"},
                };
                 }
             }
        public static object InvoiceChoice
        {
            get
            {
                return new object[]{
                    new object[]{"1", "1"},
                    new object[]{"2", "2"},
                    new object[]{"3", "3"},
                    new object[]{"Supplemental", "Supplemental"},
                };
            }
        }
        public static object PropertyType
             {
                 get
                 {
                     return new object[]{
                    new object[]{"PUB", "PUB"},
                    new object[]{"PRI", "PRI"},
                 
                };
                 }
             }
    }
}
