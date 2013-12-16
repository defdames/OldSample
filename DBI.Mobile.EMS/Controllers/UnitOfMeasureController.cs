using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBI.Data;

namespace DBI.Mobile.EMS.Controllers
{
    public class UnitOfMeasureController : ApiController
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<UNIT_OF_MEASURE_V> Get()
        {
            Entities _context = new Entities();
            List<UNIT_OF_MEASURE_V> uomnvl = _context.UNIT_OF_MEASURE_V.ToList();
            return _context.UNIT_OF_MEASURE_V.ToList();   
        }
    }
}
