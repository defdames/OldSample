using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class ValuesController : ApiController
    {
        [Authorize]
        public IEnumerable<PROJECTS_V> Get()
        {
            Entities _context = new Entities();
            var pl = _context.PROJECTS_V.ToList().Take(25);
            return pl;
        }

        //[Authorize]
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}