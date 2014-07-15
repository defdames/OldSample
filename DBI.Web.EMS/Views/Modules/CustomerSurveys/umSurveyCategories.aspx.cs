using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;
using System.Data.Entity;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umSurveyCategories : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadCategories(object sender, StoreReadDataEventArgs e)
        {
            int count;
            using (Entities _context = new Entities())
            {
                uxCategoriesStore.DataSource = GenericData.ListFilterHeader<CUSTOMER_SURVEYS.CustomerSurveyCategoryStore>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], CUSTOMER_SURVEYS.GetCategories(_context), out count);
                e.Total = count;
            }
        }

        protected void deLoadEditCategoryWindow(object sender, DirectEventArgs e)
        {
            uxFormType.Value = "Edit";
            List<CUSTOMER_SURVEY_CAT> ToBeEdited = JSON.Deserialize<List<CUSTOMER_SURVEY_CAT>>(e.ExtraParams["CategoryInfo"]);
            uxCategoryId.Value = ToBeEdited[0].CATEGORY_ID;
            uxCategoryName.Value = ToBeEdited[0].NAME;
            uxDescription.Value = ToBeEdited[0].DESCRIPTION;

            uxAddEditCategoryWindow.Show();
        }

        protected void deSaveCategory(object sender, DirectEventArgs e)
        {
            CUSTOMER_SURVEY_CAT ToBeSaved;
            if (uxFormType.Text == "Add")
            {
                ToBeSaved = new CUSTOMER_SURVEY_CAT();
                ToBeSaved.NAME = uxCategoryName.Text;
                ToBeSaved.DESCRIPTION = uxDescription.Text;
                ToBeSaved.CREATE_DATE = DateTime.Now;
                ToBeSaved.CREATED_BY = User.Identity.Name;
                ToBeSaved.MODIFIED_BY = User.Identity.Name;
                ToBeSaved.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<CUSTOMER_SURVEY_CAT>(ToBeSaved);
            }
            else
            {
                using (Entities _context = new Entities())
                {
                    ToBeSaved = CUSTOMER_SURVEYS.GetCategory(decimal.Parse(uxCategoryId.Text), _context);    
                }
                ToBeSaved.CATEGORY_ID = decimal.Parse(uxCategoryId.Text);
                ToBeSaved.NAME = uxCategoryName.Text;
                ToBeSaved.DESCRIPTION = uxDescription.Text;
                ToBeSaved.MODIFIED_BY = User.Identity.Name;
                ToBeSaved.MODIFY_DATE = DateTime.Now;

                GenericData.Update<CUSTOMER_SURVEY_CAT>(ToBeSaved);
            }

            uxCategoriesStore.Reload();
            uxAddEditCategoryWindow.Hide();
            uxCategoryForm.Reset();
        }
        protected void deDeleteCategory(object sender, DirectEventArgs e)
        {
            decimal CategoryId = decimal.Parse(e.ExtraParams["CategoryId"]);
            CUSTOMER_SURVEY_CAT ToBeDeleted;
            int FormCount;
            using (Entities _context = new Entities())
            {
                ToBeDeleted = CUSTOMER_SURVEYS.GetCategory(CategoryId, _context);
                FormCount = ToBeDeleted.CUSTOMER_SURVEY_FORMS.Count;
            }
            if (FormCount > 0)
            {
                X.Msg.Alert("Error", "This Category has forms within it.  Please remove any existing forms before deleting the Category").Show();
            }
            else
            {
                GenericData.Delete<CUSTOMER_SURVEY_CAT>(ToBeDeleted);
            }

            uxCategoriesStore.Reload();
        }
    }
}