using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using Newtonsoft.Json;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umDetailSheet : System.Web.UI.Page
    {
        // Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                uxCloseDetailSheet.Disable();






            }
        }

        protected void deReadMaterialGridData(object sender, StoreReadDataEventArgs e)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailSheetID = long.Parse(Request.QueryString["detailSheetID"]);

            uxMaterialGridStore.DataSource = BUDGETBIDDING.MaterialGridData(projectID, detailSheetID);
        }

        [DirectMethod(Namespace = "SaveRecord")]
        public void deSaveMaterial(object recordData, long id)
        {
            long projectID = long.Parse(Request.QueryString["projectID"]);
            long detailTaskID = long.Parse(Request.QueryString["detailSheetID"]);
            string jsonText = recordData.ToString();
            BUDGETBIDDING.BUD_DETAIL_MATERIAL_DATA_V materialData = JsonConvert.DeserializeObject<BUDGETBIDDING.BUD_DETAIL_MATERIAL_DATA_V>(jsonText);
            BUD_BID_DETAIL_SHEET data;

            if (id == 0)
            {
                data = new BUD_BID_DETAIL_SHEET();
            }
            else
            {
                using (Entities _context = new Entities())
                {
                    data = _context.BUD_BID_DETAIL_SHEET.Where(x => x.DETAIL_SHEET_ID == id).Single();
                }
            }

            data.DESC_1 = materialData.DESC_1;
            data.AMT_1 = materialData.AMT_1;
            data.DESC_2 = materialData.DESC_2;
            data.AMT_2 = materialData.AMT_2;
            data.TOTAL = materialData.TOTAL;

            if (id == 0)
            {
                data.PROJECT_ID = projectID;
                data.DETAIL_TASK_ID = detailTaskID;
                data.REC_TYPE = "MATERIAL";
                data.CREATE_DATE = DateTime.Now;
                data.CREATED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now; ;
                data.MODIFIED_BY = "TEMP";
                GenericData.Insert<BUD_BID_DETAIL_SHEET>(data);
            }
            else
            {
                data.MODIFY_DATE = DateTime.Now; ;
                GenericData.Update<BUD_BID_DETAIL_SHEET>(data);
            }

            uxMaterialGridStore.CommitChanges();
            uxMaterialGridStore.Reload();
        }

        protected void deCheckAllowDetailSave(object sender, DirectEventArgs e)
        {
            char[] charsToTrim = { ' ', '\t' };
            string detailName = uxDetailName.Text.Trim(charsToTrim);

            if (detailName == "")
            {
                uxCloseDetailSheet.Disable();
            }
            else
            {
                uxCloseDetailSheet.Enable();
            }



        }

        protected void StandardMsgBox(string title, string msg, string msgIcon)
        {
            // msgIcon can be:  ERROR, INFO, QUESTION, WARNING
            X.Msg.Show(new MessageBoxConfig()
            {
                Title = title,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), msgIcon)
            });
        }
    }
}