using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umSubmitActivity_DBI : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View"))
            {
                X.Redirect("~/Views/uxDefault.aspx");
            }
            
            GetFooterData();

        }

        /// <summary>
        /// Gets existing footer data for display in form
        /// </summary>
        protected void GetFooterData()
        {
            DAILY_ACTIVITY_FOOTER data;
            var HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Get Footer Details
            using (Entities _context = new Entities())
            {
                try
                {
                    data = (from d in _context.DAILY_ACTIVITY_FOOTER
                            where d.HEADER_ID == HeaderId
                            select d).Single();
                    try
                    {
                        uxSubmitReasonForNoWork.SetValue(data.COMMENTS.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        uxSubmitReasonForNoWork.SetValue("");
                    }

                    try
                    {
                        uxSubmitHotel.SetValue(data.HOTEL_NAME.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        uxSubmitHotel.SetValue("");
                    }

                    try
                    {
                        uxSubmitCity.SetValue(data.HOTEL_CITY.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        uxSubmitCity.SetValue("");
                    }

                    try
                    {
                        uxSubmitState.SetValue(data.HOTEL_STATE.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        uxSubmitState.SetValue("");
                    }
                    try
                    {
                        uxSubmitPhone.SetValue(data.HOTEL_PHONE.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        uxSubmitPhone.SetValue("");
                    }

                    if (data.FOREMAN_SIGNATURE.Length > 0)
                    {
                        string ForemanUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=foreman", HeaderId);
                        uxForemanSignatureImage.ImageUrl = ForemanUrl;
                        uxForemanSignatureImage.Show();
                    }

                    if (data.CONTRACT_REP.Length > 0)
                    {
                        string ContractRepresentativeUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=contract", HeaderId);
                        uxContractRepresentativeImage.ImageUrl = ContractRepresentativeUrl;
                        uxContractRepresentativeImage.Show();
                    }
                }
                catch (InvalidOperationException)
                {

                }
            }
        }

        /// <summary>
        /// Direct event to store footer to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreFooter(object sender, DirectEventArgs e)
        {
            DAILY_ACTIVITY_FOOTER data;

            //Set HeaderId
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            try
            {
                using (Entities _context = new Entities())
                {
                    //Check if footer record exists
                    data = (from d in _context.DAILY_ACTIVITY_FOOTER
                            where d.HEADER_ID == HeaderId
                            select d).Single();
                }
            }
            catch (InvalidOperationException)
            {
                data = null;
            }

            if (data != null)
            {
                //Check for empty values
                try
                {
                    string ReasonForNoWork = uxSubmitReasonForNoWork.Value.ToString();
                    data.COMMENTS = ReasonForNoWork;
                }
                catch (NullReferenceException)
                {
                }

                try
                {
                    string Hotel = uxSubmitHotel.Value.ToString();
                    data.HOTEL_NAME = Hotel;
                }
                catch (NullReferenceException)
                {
                }

                try
                {
                    string HotelCity = uxSubmitCity.Value.ToString();
                    data.HOTEL_CITY = HotelCity;
                }
                catch(NullReferenceException)
                {
                }

                try
                {
                    string HotelState = uxSubmitState.Value.ToString();
                    data.HOTEL_STATE = HotelState;
                }
                catch
                {
                }

                try
                {
                    string HotelPhone = uxSubmitPhone.Value.ToString();
                    data.HOTEL_PHONE = HotelPhone;
                }
                catch
                {
                }

                try
                {
                    string ContractRepName = uxContractRepresentative.Value.ToString();
                    data.CONTRACT_REP_NAME = ContractRepName;
                }
                catch
                {

                }
                //file upload
                HttpPostedFile ForemanSignatureFile = uxSubmitSignature.PostedFile;
                byte[] ForemanSignatureArray = ImageToByteArray(ForemanSignatureFile);
                if (ForemanSignatureFile.ContentLength > 0)
                {
                    data.FOREMAN_SIGNATURE = ForemanSignatureArray;
                }

                //file upload
                HttpPostedFile ContractRepFile = uxSubmitContract.PostedFile;
                byte[] ContractRepArray = ImageToByteArray(ContractRepFile);

                if (ContractRepFile.ContentLength > 0)
                {
                    data.CONTRACT_REP = ContractRepArray;
                }

                data.MODIFIED_BY = User.Identity.Name;
                data.MODIFY_DATE = DateTime.Now;

                GenericData.Update<DAILY_ACTIVITY_FOOTER>(data);
            }
            else
            {
                data = new DAILY_ACTIVITY_FOOTER();

                data.HEADER_ID = HeaderId;

                //Check for empty values
                try
                {
                    string ReasonForNoWork = uxSubmitReasonForNoWork.Value.ToString();
                    data.COMMENTS = ReasonForNoWork;
                }
                catch (NullReferenceException)
                {
                    data.COMMENTS = null;
                }

                try
                {
                    string Hotel = uxSubmitHotel.Value.ToString();
                    data.HOTEL_NAME = Hotel;
                }
                catch (NullReferenceException)
                {
                    data.HOTEL_NAME = null;
                }

                try
                {
                    string HotelCity = uxSubmitCity.Value.ToString();
                    data.HOTEL_CITY = HotelCity;
                }
                catch
                {
                    data.HOTEL_CITY = null;
                }

                try
                {
                    string HotelState = uxSubmitState.Value.ToString();
                    data.HOTEL_STATE = HotelState;
                }
                catch
                {
                    data.HOTEL_STATE = null;
                }

                try
                {
                    string HotelPhone = uxSubmitPhone.Value.ToString();
                    data.HOTEL_PHONE = HotelPhone;
                }
                catch
                {
                    data.HOTEL_PHONE = null;
                }

                try
                {
                    string ContractRepName = uxContractRepresentative.Value.ToString();
                    data.CONTRACT_REP_NAME = ContractRepName;
                }
                catch
                {
                    data.CONTRACT_REP_NAME = null;
                }

                try
                {
                    //file upload
                    HttpPostedFile ForemanSignatureFile = uxSubmitSignature.PostedFile;
                    byte[] ForemanSignatureArray = ImageToByteArray(ForemanSignatureFile);

                    data.FOREMAN_SIGNATURE = ForemanSignatureArray;
                }
                catch
                {
                    data.FOREMAN_SIGNATURE = null;
                }

                try
                {
                    //file upload
                    HttpPostedFile ContractRepFile = uxSubmitContract.PostedFile;
                    byte[] ContractRepArray = ImageToByteArray(ContractRepFile);

                    data.CONTRACT_REP = ContractRepArray;
                }

                catch
                {
                    data.CONTRACT_REP = null;
                }

                data.CREATED_BY = User.Identity.Name;
                data.MODIFIED_BY = User.Identity.Name;
                data.CREATE_DATE = DateTime.Now;
                data.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<DAILY_ACTIVITY_FOOTER>(data);

                uxSubmitActivityForm.Reset();
            }
        }

        /// <summary>
        /// Converts uploaded image file to byte array for DB storage
        /// </summary>
        /// <param name="ImageFile"></param>
        /// <returns></returns>
        protected byte[] ImageToByteArray(HttpPostedFile ImageFile)
        {
            byte[] ImageArray = null;
            BinaryReader b = new BinaryReader(ImageFile.InputStream);
            ImageArray = b.ReadBytes(ImageFile.ContentLength);
            return ImageArray;
        }

        /// <summary>
        /// Updates Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deStoreFooterAndSubmit(object sender, DirectEventArgs e)
        {
            //Set HeaderId
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Store Footer to DB
            deStoreFooter(sender, e);

            List<EmployeeData> HoursOver14 = ValidationChecks.checkEmployeeTime("Hours over 14");
            List<long> BusinessUnitEquipment = ValidationChecks.EquipmentBusinessUnitCheck();
            List<long> BusinessUnitEmployees = ValidationChecks.EmployeeBusinessUnitCheck();
            bool HasWarnings = false;
            string MessageBoxMessage = string.Empty;
            if (HoursOver14.Count > 0)
            {
                if (HoursOver14.Exists(x => x.HEADER_ID == HeaderId))
                {
                    List<EmployeeData> EmployeeList = HoursOver14.FindAll(x => x.HEADER_ID == HeaderId);
                    foreach (EmployeeData Employee in EmployeeList)
                    {
                        MessageBoxMessage += string.Format("{0} has 14 or more hours logged on {1:MM-dd-yy}. <br />", Employee.EMPLOYEE_NAME, Employee.DA_DATE);
                    }
                    HasWarnings = true;
                }
            }

            if (BusinessUnitEquipment.Count > 0)
            {
                if (BusinessUnitEquipment.Exists(x => x == HeaderId))
                {
                    MessageBoxMessage += "This activity contains Equipment outside of the project's business unit. <br />";
                    HasWarnings = true;
                }
            }

            if (BusinessUnitEmployees.Count > 0)
            {
                if (BusinessUnitEmployees.Exists(x => x == HeaderId))
                {
                    MessageBoxMessage += "This activity contans Employees outside of the project's business unit. <br />";
                    HasWarnings = true;
                }
            }
            if (ValidationChecks.AreMetersMissing(HeaderId))
            {
                MessageBoxMessage += "There are equipment entries without meter entries. ";
                HasWarnings = true;
            }

            if (HasWarnings)
            {
                X.MessageBox.Confirm("Meters Missing", MessageBoxMessage + "Continue?", new MessageBoxButtonsConfig()
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Text = "Yes",
                        Handler = "App.direct.dmSubmitForApproval()"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Text = "No"
                    }
                }).Show();
            }
            else
            {
                dmSubmitForApproval();
            }
            
        }

        /// <summary>
        /// Submits header for approval
        /// </summary>
        [DirectMethod]
        public void dmSubmitForApproval()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"].ToString());

            //Get header
            DAILY_ACTIVITY_HEADER HeaderData;
            DAILY_ACTIVITY_FOOTER FooterData;
            using (Entities _context = new Entities())
            {
                HeaderData = (from d in _context.DAILY_ACTIVITY_HEADER
                              where d.HEADER_ID == HeaderId
                              select d).Single();

                FooterData = (from d in _context.DAILY_ACTIVITY_FOOTER
                              where d.HEADER_ID == HeaderId
                              select d).Single();
            }

            if (FooterData.FOREMAN_SIGNATURE.Length > 0)
            {
                //Update status to Requires approval
                HeaderData.STATUS = 2;
                GenericData.Update<DAILY_ACTIVITY_HEADER>(HeaderData);
                X.Js.Call("parent.App.direct.dmHideWindowUpdateGrid()");
            }
            else
            {
                X.Js.Call("parent.App.direct.dmSubmitNotification()");

            }
        }     
        
    }
}