using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DBI.Data;
using Newtonsoft.Json;

namespace DBI.Mobile.EMS.Controllers
{
    public class DARSubmitController : ApiController
    {
        [Authorize]
        [HttpPost]
        public void Post(HttpRequestMessage req)
        {
            var data = req.Content.ReadAsStringAsync().Result;
            DBI.Data.DAILY_ACTIVITY_HEADER headerRecord = JsonConvert.DeserializeObject<DBI.Data.DAILY_ACTIVITY_HEADER>(data);
            GenericData.Insert<DBI.Data.DAILY_ACTIVITY_HEADER>(headerRecord);

            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\temp\\post.json");
            file.Write(data);
            file.Close();
        }

    }
}
