using System;
using System.Linq;
using System.Web;
using DBI.Data;
using DBI.Data.Generic;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAddGLAccount : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deFilterEvents(object sender, DirectEventArgs e)
        {
            try
            {
                uxGlAccountSecurityStore.RemoveAll();
                uxGlAccountSecurityStore.ClearFilter();
                uxGlAccountSecurityStore.Reload();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }

        }

        protected void deLoadCompanyCodes(object sender, StoreReadDataEventArgs e)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    var _glCodes = (from accounts in _context.GL_ACCOUNTS_V
                                           group accounts by new { accounts.SEGMENT1, accounts.SEGMENT1_DESC} into grp
                                           select new TripleCombo { ID = grp.Key.SEGMENT1, ID_NAME = grp.Key.SEGMENT1, DESCRIPTION = grp.Key.SEGMENT1_DESC}).OrderBy(o => o.ID).AsQueryable();

                    uxCompanyNameStore.DataSource = _glCodes.ToList();

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        protected void deReadGLSecurityCodes(object sender, StoreReadDataEventArgs e)
        {
            try
            {
                long _organizationID = long.Parse(Request.QueryString["OrgID"]);
                string _segment1 = e.Parameters["SEGMENT1"].ToString();
                string _segment2 = e.Parameters["SEGMENT2"].ToString();
                string _segment3 = e.Parameters["SEGMENT3"].ToString();
                string _segment4 = e.Parameters["SEGMENT4"].ToString();

                var _filteredData = GL_ACCOUNTS_V.AccountsFiltered(_segment1,_segment2,_segment3,_segment4,_organizationID);
                int count;
                uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _filteredData, out count);
                e.Total = _filteredData.Count();
            }
            catch (Exception)
            {
                
                throw;
            }
            

        }

        protected void deAddSelectedGlCodes(object sender, DirectEventArgs e)
        {
            try
            {
                long organizationID = long.Parse(Request.QueryString["OrgID"]);

                RowSelectionModel model = uxGlAccountSecurityGridSelectionModel;

                foreach (SelectedRow row in model.SelectedRows)
                {
                    OVERHEAD_GL_ACCOUNT account = new OVERHEAD_GL_ACCOUNT();
                    account.CODE_COMBO_ID = long.Parse(row.RecordID);
                    account.OVERHEAD_ORG_ID = organizationID;
                    account.CREATE_DATE = DateTime.Now;
                    account.MODIFY_DATE = DateTime.Now;
                    account.CREATED_BY = HttpContext.Current.User.Identity.Name.ToString();
                    account.MODIFIED_BY = HttpContext.Current.User.Identity.Name.ToString();
                    GenericData.Insert<OVERHEAD_GL_ACCOUNT>(account);
                }

                uxGlAccountSecurityStore.RemoveAll();
                uxGlAccountSecurityStore.ClearFilter();
                uxGlAccountSecurityStore.Reload();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }
           

        }

    }
}