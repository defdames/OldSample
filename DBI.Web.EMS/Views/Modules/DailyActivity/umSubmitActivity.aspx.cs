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
    public partial class umSubmitActivity : BasePage
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
                        uxSubmitReasonForNoWork.SetValue(data.REASON_FOR_NO_WORK.ToString());
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
                    data.REASON_FOR_NO_WORK = ReasonForNoWork;
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
                    data.REASON_FOR_NO_WORK = ReasonForNoWork;
                }
                catch (NullReferenceException)
                {
                    data.REASON_FOR_NO_WORK = null;
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

            checkPerDiem(HeaderId);

            if (AreMetersMissing(HeaderId))
            {
                X.MessageBox.Confirm("Meters Missing", "There are equipment entries without meter entries.  Continue?", new MessageBoxButtonsConfig()
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

        /// <summary>
        /// Validation check to see if equipment meter readings are missing
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns></returns>
        protected bool AreMetersMissing(long HeaderId)
        {
            //Get List of equipment
            List<DAILY_ACTIVITY_EQUIPMENT> EquipmentList;

            using (Entities _context = new Entities())
            {
                EquipmentList = (from d in _context.DAILY_ACTIVITY_EQUIPMENT
                                 where d.HEADER_ID == HeaderId
                                 select d).ToList();
            }
            int NumberOfMissingMeters = 0;
            foreach (DAILY_ACTIVITY_EQUIPMENT Equipment in EquipmentList)
            {
                if (Equipment.ODOMETER_START == null || Equipment.ODOMETER_END == null)
                {
                    NumberOfMissingMeters++;
                }
            }

            if (NumberOfMissingMeters > 0)
            {
                return true;
            }
            return false;
        }

        protected void checkPerDiem(long HeaderId)
        {
            using (Entities _context = new Entities())
            {
                //Get List of Employees for this header with Per-Diem active
                var EmployeeList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                    where d.HEADER_ID == HeaderId && d.PER_DIEM == "Y"
                                    select new { d.PERSON_ID, d.DAILY_ACTIVITY_HEADER.DA_DATE }).ToList();
                
                //Check for Additional active PerDiems on that day
                foreach (var Employee in EmployeeList)
                {
                    var HeaderList = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                      join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                      join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
                                      where h.DA_DATE == Employee.DA_DATE && d.PERSON_ID == Employee.PERSON_ID
                                      select new HeaderDetails { HEADER_ID = h.HEADER_ID, LONG_NAME = p.LONG_NAME, PERSON_ID = d.PERSON_ID }).ToList();

                    if (HeaderList.Count > 1)
                    {
                        X.Js.Call("parent.App.direct.dmLoadPerDiemPicker(" + Ext.Net.JSON.Serialize(HeaderList) + ")");
                    }

                }
            }
        }
    }
}