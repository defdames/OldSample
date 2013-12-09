using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class ImageLoader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                DAILY_ACTIVITY_FOOTER data;
                data = (from d in _context.DAILY_ACTIVITY_FOOTER
                        where d.HEADER_ID == HeaderId
                        select d).Single();

                if (data != null)
                {
                    MemoryStream imageStream = new MemoryStream();
                    byte[] imageContent;

                    if (Request.QueryString["type"] == "foreman")
                    {
                        imageContent = data.FOREMAN_SIGNATURE.ToArray();
                    }
                    else
                    {
                        imageContent = data.CONTRACT_REP.ToArray();
                    }

                    imageStream.Position = 0;
                    imageStream.Read(imageContent, 0, (int)imageStream.Length);
                    Response.ContentType = "image/jpeg";
                    Response.BinaryWrite(imageContent);
                    Response.End();
                }
                else
                {
                    // ERORR HANDLING NOT FOUND
                }
            }
        }
    }
}
