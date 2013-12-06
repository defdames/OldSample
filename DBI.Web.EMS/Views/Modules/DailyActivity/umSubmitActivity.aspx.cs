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
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    List<DAILY_ACTIVITY_FOOTER> data;
        //    var HeaderId = Request.QueryString["HeaderId"];
            
        //    //Get Footer Details
        //    using (Entities _context = new Entities())
        //    {
        //        data = (from d in _context.DAILY_ACTIVITY_FOOTER
        //                    where d.HEADER_ID = HeaderId
        //                    select d).ToList();
        //    }

        //    //Set Default Values
        //    try
        //    {
        //        uxSubmitReasonForNoWork.SetValue(data.REASON_FOR_NO_WORK.ToString());
        //    }
        //    //If it's empty do nothing(so the field is empty)
        //    catch(NullReferenceException)
        //    {

        //    }

        //    try
        //    {
        //        uxSubmitHotel.SetValue(data.HOTEL.ToString());
        //    }
        //    catch(NullReferenceException)
        //    {

        //    }

        //    try
        //    {
        //        uxSubmitCity.SetValue(data.HOTEL_CITY.ToString());
        //    }
        //    catch(NullReferenceException)
        //    {

        //    }

        //    try
        //    {
        //        uxSubmitState.SetValue(data.HOTEL_STATE.ToString());
        //    }
        //    catch (NullReferenceException)
        //    {

        //    }

        //    try
        //    {
        //        uxSubmitPhone.SetValue(data.HOTEL_PHONE.ToString());
        //    }
        //    catch (NullReferenceException)
        //    {

        //    }

        //    string ForemanUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}?type=foreman", HeaderId);
        //    uxForemanSignatureImage.ImageUrl = ForemanUrl;
            
        //    string ContractRepresentativeUrl = string.Format("ImageLoader/ImageLoader.aspx?headerId={0}&type=contract", HeaderId);
        //    uxContractRepresentativeImage.ImageUrl = ContractRepresentativeUrl;
                
           
        //}

        //protected void deStoreFooter(object sender, DirectEventArgs e)
        //{
        //    using (Entities _context = new Entities())
        //    {
        //        //Check for empty values
        //        try
        //        {
        //            string ReasonForNoWork = uxSubmitReasonForNoWork.Value.ToString();
        //            //todo data.REASON_FOR_NO_WORK = ReasonForNoWork;
        //        }
        //        catch (NullReferenceException)
        //        {
        //            //todo data.REASON_FOR_NO_WORK = null;
        //        }

        //        try
        //        {
        //            string Hotel = uxSubmitHotel.Value.ToString();
        //            //todo data.HOTEL_NAME = Hotel;
        //        }
        //        catch (NullReferenceException)
        //        {
        //            //todo data.HOTEL_NAME = null;
        //        }

        //        try
        //        {
        //            string HotelCity = uxSubmitCity.Value.ToString();
        //            //todo data.HOTEL_CITY = HotelCity;
        //        }
        //        catch
        //        {
        //            //todo data.HOTEL_CITY = null;
        //        }

        //        try
        //        {
        //            string HotelState = uxSubmitState.Value.ToString();
        //            //todo data.HOTEL_STATE = HotelState;
        //        }
        //        catch
        //        {
        //            //todo data.HOTEL_STATE = null;
        //        }

        //        try
        //        {
        //            string HotelPhone = uxSubmitPhone.Value.ToString();
        //            //todo data.HOTEL_PHONE = HotelPhone;
        //        }
        //        catch
        //        {
        //            //todo data.HOTEL_PHONE = null;
        //        }

        //        try
        //        {
        //            //file upload
        //            HttpPostedFile ForemanSignature = uxSubmitSignature.PostedFile;
        //            //todo data.FOREMAN_SIGNATURE = ForemanSignature
        //        }
        //        catch
        //        {
        //            //todo data.FOREMAN_SIGNATURE = null;
        //        }
        //    }
        //}

        protected void deStoreFooterAndSubmit(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {

            }
        }
    }
}