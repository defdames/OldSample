using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.Generic
{
    /// <summary>
    /// Used to add a single field to an EXT combo - Value (string)
    /// </summary>
    public class SingleCombo
    {
        public string ID_NAME { get; set; }
    }

    /// <summary>
    /// Used to add two fields to an EXT combo - ID (string) and Value (string)
    /// </summary>
    public class DoubleComboStringID
    {
        public string ID { get; set; }
        public string ID_NAME { get; set; }
    }

    /// <summary>
    /// Used to add two fields to an EXT combo - ID (long) and Value (string)
    /// </summary>
    public class DoubleComboLongID : SingleCombo
    {
        public long ID { get; set; }
    }

    /// <summary>
    /// Adds a description field to an EXT Single Combo
    /// </summary>
    public class TripleCombo : DoubleComboStringID
    {
        public string DESCRIPTION { get; set; }
    }
}
