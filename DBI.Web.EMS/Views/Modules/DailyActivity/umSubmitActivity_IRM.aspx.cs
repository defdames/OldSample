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
using DBI.Data.DataFactory;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umSubmitActivity_IRM : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }

            if (!X.IsAjaxRequest && !IsPostBack)
            {
                GetFooterData();
                uxStateList.Data = StaticLists.StateList;
            }

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
                    }

                    try
                    {
                        uxSubmitHotel.SetValue(data.HOTEL_NAME.ToString());
                    }
                    catch (NullReferenceException)
                    {
                    }

                    try
                    {
                        uxSubmitCity.SetValue(data.HOTEL_CITY.ToString());
                    }
                    catch (NullReferenceException)
                    {
                    }

                    try
                    {
                        uxSubmitState.SetValue(data.HOTEL_STATE.ToString());
                    }
                    catch (NullReferenceException)
                    {
                    }
                    try
                    {
                        uxSubmitPhone.SetValue(data.HOTEL_PHONE.ToString());
                    }
                    catch (NullReferenceException)
                    {
                    }
                    try
                    {
                        uxContractRepresentative.SetValue(data.CONTRACT_REP_NAME.ToString());
                    }
                    catch(NullReferenceException)
                    {
                    }

                    try
                    {
                        uxDotRepName.SetValue(data.DOT_REP_NAME.ToString());
                    }
                    catch (NullReferenceException)
                    {
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

                    if (data.DOT_REP.Length > 0)
                    {
                        string DotRepUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=dot", HeaderId);
                        uxDotRepImage.ImageUrl = DotRepUrl;
                        uxDotRepImage.Show();
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
            using (Entities _context = new Entities())
           {
                //Check if footer record exists
                data = (from d in _context.DAILY_ACTIVITY_FOOTER
                        where d.HEADER_ID == HeaderId
                        select d).SingleOrDefault();
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
                catch (NullReferenceException)
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
                    data.CONTRACT_REP_NAME = null;
                }

                try
                {
                    string DotRepName = uxDotRepName.Value.ToString();
                    data.DOT_REP_NAME = DotRepName;
                }
                catch
                {
                    data.DOT_REP_NAME = null;
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

                //file upload
                HttpPostedFile DotRepFile = uxSubmitDotRep.PostedFile;
                byte[] DotRepArray = ImageToByteArray(DotRepFile);

                if (DotRepFile.ContentLength > 0)
                {
                    data.DOT_REP = DotRepArray;
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

                try
                {
                    string DotRepName = uxDotRepName.Value.ToString();
                    data.DOT_REP_NAME = DotRepName;
                }
                catch
                {
                    data.DOT_REP_NAME = null;
                }

                try
                {
                    //file upload
                    HttpPostedFile DotRepFile = uxSubmitDotRep.PostedFile;
                    byte[] DotRepArray = ImageToByteArray(DotRepFile);

                    data.DOT_REP = DotRepArray;
                }
                catch
                {
                    data.DOT_REP = null;
                }

                data.CREATED_BY = User.Identity.Name;
                data.MODIFIED_BY = User.Identity.Name;
                data.CREATE_DATE = DateTime.Now;
                data.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<DAILY_ACTIVITY_FOOTER>(data);
                
            }
            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "Footer Added Successfully",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
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
    }
}