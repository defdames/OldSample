using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public class BudgetBiddingFunctions
    {
        public static List<object> AdamTest()
        {
            using (Entities context = new Entities())
            {
                List<object> dataSource;
                dataSource = (from d in context.PA_PERIODS_ALL
                              select new { END_DATE = d.END_DATE.Year }).Distinct().OrderBy(d => d.END_DATE).ToList<object>();
                return dataSource;
            }
        }

        public static List<PA_PERIODS_ALL> fiscalPeriods()
        {
            using (Entities _context = new Entities())
            {
                List<PA_PERIODS_ALL> _data = _context.PA_PERIODS_ALL.ToList();
                return _data;
            }
        }

        public static List<DateTime> distinctFiscalPeriods()
        {
            List<DateTime> _data = BudgetBiddingFunctions.fiscalPeriods().Select(x => x.END_DATE).Distinct().ToList();
            return _data;
        }

        //public static List<object> fiscalYears()
        //{
        //    List<object> _yearData = new List<object>();
        //    List<DateTime> _data = BudgetBiddingFunctions.distinctFiscalPeriods();
        //    foreach (var year in _data)
        //    {
        //        object x = new object();

        //    }

            

        //}

    }

   

}