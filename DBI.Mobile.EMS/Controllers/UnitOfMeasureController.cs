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

        [Authorize]
        [HttpGet]
        public IEnumerable<UNIT_OF_MEASURE_V> GetByDate(string fromDate)
        {
            Entities _context = new Entities();
            List<UNIT_OF_MEASURE_V> returnList = new List<UNIT_OF_MEASURE_V>();

            DateTime checkDate;
            if (DateTime.TryParse(fromDate, out checkDate))
            {
               List<UNIT_OF_MEASURE_V> pt = _context.UNIT_OF_MEASURE_V.Where(p => p.LAST_UPDATE_DATE >= checkDate).ToList();
                foreach (UNIT_OF_MEASURE_V uom in pt)
                {
                    UNIT_OF_MEASURE_V nUOM = new UNIT_OF_MEASURE_V();
                    nUOM.UOM_CODE = uom.UOM_CODE;
                    nUOM.UNIT_OF_MEASURE = uom.UNIT_OF_MEASURE;
                    nUOM.UOM_CLASS = uom.UOM_CLASS;
                    nUOM.BASE_UOM_FLAG = uom.BASE_UOM_FLAG;
                    nUOM.DISABLE_DATE = uom.DISABLE_DATE;
                    nUOM.LAST_UPDATE_DATE = uom.LAST_UPDATE_DATE;
                    nUOM.ACTIVE = uom.ACTIVE;
                    returnList.Add(nUOM);
                }
            }
            return returnList;
        }
    }
}
