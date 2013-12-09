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
    public partial class umSubmitActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    uxSubmitReasonForNoWork.SetValue(data.REASON_FOR_NO_WORK.ToString());
                    uxSubmitHotel.SetValue(data.HOTEL_NAME.ToString());
                    uxSubmitCity.SetValue(data.HOTEL_CITY.ToString());
                    uxSubmitState.SetValue(data.HOTEL_STATE.ToString());
                    uxSubmitPhone.SetValue(data.HOTEL_PHONE.ToString());
                    string ForemanUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=foreman", HeaderId);
                    uxForemanSignatureImage.ImageUrl = ForemanUrl;

                    string ContractRepresentativeUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=contract", HeaderId);
                    uxContractRepresentativeImage.ImageUrl = ContractRepresentativeUrl;
                }
                catch (InvalidOperationException)
                {

                }
            }

        }

        protected void deStoreFooter(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                //Set HeaderId
                long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

                //Check if footer record exists
                DAILY_ACTIVITY_FOOTER existingRecord;

                existingRecord = (from d in _context.DAILY_ACTIVITY_FOOTER
                                      where d.HEADER_ID == HeaderId
                                      select d).Single();

                if (existingRecord != null)
                {
                    //Check for empty values
                    try
                    {
                        string ReasonForNoWork = uxSubmitReasonForNoWork.Value.ToString();
                        existingRecord.REASON_FOR_NO_WORK = ReasonForNoWork;
                    }
                    catch (NullReferenceException)
                    {
                    }

                    try
                    {
                        string Hotel = uxSubmitHotel.Value.ToString();
                        existingRecord.HOTEL_NAME = Hotel;
                    }
                    catch (NullReferenceException)
                    {
                    }

                    try
                    {
                        string HotelCity = uxSubmitCity.Value.ToString();
                        existingRecord.HOTEL_CITY = HotelCity;
                    }
                    catch(NullReferenceException)
                    {
                    }

                    try
                    {
                        string HotelState = uxSubmitState.Value.ToString();
                        existingRecord.HOTEL_STATE = HotelState;
                    }
                    catch
                    {
                    }

                    try
                    {
                        string HotelPhone = uxSubmitPhone.Value.ToString();
                        existingRecord.HOTEL_PHONE = HotelPhone;
                    }
                    catch
                    {
                    }

                    try
                    {
                        //file upload
                        HttpPostedFile ForemanSignatureFile = uxSubmitSignature.PostedFile;
                        byte[] ForemanSignatureArray = ImageToByteArray(ForemanSignatureFile);

                        existingRecord.FOREMAN_SIGNATURE = ForemanSignatureArray;
                    }
                    catch
                    {
                    }

                    try
                    {
                        //file upload
                        HttpPostedFile ContractRepFile = uxSubmitContract.PostedFile;
                        byte[] ContractRepArray = ImageToByteArray(ContractRepFile);

                        existingRecord.CONTRACT_REP = ContractRepArray;
                    }
                    catch
                    {
                    }
                    existingRecord.MODIFIED_BY = User.Identity.Name;
                    existingRecord.MODIFY_DATE = DateTime.Now;

                    GenericData.Update<DAILY_ACTIVITY_FOOTER>(existingRecord);
                }
                else
                {
                    DAILY_ACTIVITY_FOOTER data = new DAILY_ACTIVITY_FOOTER();

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
                }
            }
        }

        protected byte[] ImageToByteArray(HttpPostedFile ImageFile)
        {
            byte[] ImageArray = null;
            BinaryReader b = new BinaryReader(ImageFile.InputStream);
            ImageArray = b.ReadBytes(ImageFile.ContentLength);
            return ImageArray;
        }

        protected void deStoreFooterAndSubmit(object sender, DirectEventArgs e)
        {
            //Set HeaderId
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);

            //Store Footer to DB
            deStoreFooter(sender, e);

            //Get header
            using (Entities _context = new Entities())
            {
                DAILY_ACTIVITY_HEADER data = (from d in _context.DAILY_ACTIVITY_HEADER
                                              where d.HEADER_ID == HeaderId
                                              select d).Single();
                
                //Update status to Requires approval
                data.STATUS = 2;

                GenericData.Update<DAILY_ACTIVITY_HEADER>(data);
            }
        }
    }
}