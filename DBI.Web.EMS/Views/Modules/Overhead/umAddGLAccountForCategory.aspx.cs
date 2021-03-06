﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAddGLAccountForCategory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        public class GL_ACCOUNTS_V2
        {
            public string SEGMENT5 { get; set; }
            public string SEGMENT5_DESC { get; set; }

        }


        protected void uxGLAccountListStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                string sql = @"SELECT gccl.segment5 as SEGMENT5, b.description AS SEGMENT5_DESC
                             from gl.gl_code_combinations gccl
                             inner join APPLSYS.FND_ID_FLEX_SEGMENTS S on gccl.chart_of_accounts_id = s.id_flex_num and s.application_id = 101 and s.id_flex_code = 'GL#' and s.segment_num = 5 and s.enabled_flag = 'Y'
                             inner join APPS.FND_FLEX_VALUES_VL b on b.flex_value = gccl.segment5 and s.flex_value_set_id = b.flex_value_set_id GROUP BY gccl.segment5, b.description order by 2";
                var _data = _context.Database.SqlQuery<GL_ACCOUNTS_V2>(sql).AsQueryable();

                _data = (from dups in _data
                         where !_context.OVERHEAD_ACCOUNT_CATEGORY.Any(x => x.ACCOUNT_SEGMENT == dups.SEGMENT5)
                         select dups);

                int count;
                uxGLAccountListStore.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
            }
        }

        protected void deSaveAccountsToCategory(object sender, DirectEventArgs e)
        {
            RowSelectionModel rm = CheckboxSelectionModel1 as RowSelectionModel;

            var _category_id = long.Parse(Request.QueryString["category_id"]);

            long? _lastSortOrder = 0;
            //Get max sort order
            using (Entities _context = new Entities())
            {
                long? _temp = _context.OVERHEAD_ACCOUNT_CATEGORY.Select(x => x.SORT_ORDER).Max();
                if (_temp != null)
                    _lastSortOrder = _temp;
            }


                foreach(SelectedRow row in rm.SelectedRows)
                {
                    OVERHEAD_ACCOUNT_CATEGORY _record = new OVERHEAD_ACCOUNT_CATEGORY();
                    _record.ACCOUNT_SEGMENT = row.RecordID;
                    _record.CATEGORY_ID = _category_id;
                    _record.SORT_ORDER = 0;
                    _record.CREATE_DATE = DateTime.Now;
                    _record.MODIFY_DATE = DateTime.Now;
                    _record.CREATED_BY = User.Identity.Name;
                    _record.MODIFIED_BY = User.Identity.Name;
                    _record.SORT_ORDER = _lastSortOrder + 1;
                    GenericData.Insert<OVERHEAD_ACCOUNT_CATEGORY>(_record);
                }
        }
    }
}