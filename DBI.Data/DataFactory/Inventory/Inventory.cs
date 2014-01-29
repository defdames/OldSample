using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class INVENTORY_V
    {
        public static List<INVENTORY_V> Inventory()
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<INVENTORY_V>().ToList();
            }
        }

        public static List<SUBINVENTORY_V> SubInventory()
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<SUBINVENTORY_V>().ToList();
            }
        }

        public static List<MOBILE_SUBINVENTORY_V> MobileSubInventoryList()
        {
            using (Entities _context = new Entities())
            {

                List<SUBINVENTORY_V> SubInventoryList = SubInventory();
                List<MOBILE_SUBINVENTORY_V> mobileSubInventoryList = new List<MOBILE_SUBINVENTORY_V>();

                foreach (SUBINVENTORY_V item in SubInventoryList)
                {
                    MOBILE_SUBINVENTORY_V rItem = new MOBILE_SUBINVENTORY_V();
                    rItem.ORG_ID = item.ORG_ID;
                    rItem.SECONDARY_INV_NAME = item.SECONDARY_INV_NAME;
                    rItem.SUBINVENTORY_DESCRIPTION = item.DESCRIPTION;
                    mobileSubInventoryList.Add(rItem);
                    rItem = null;
                }
                return mobileSubInventoryList;
            }
        }

        public static List<MOBILE_INVENTORY_V> MobileInventoryList()
        {
            using (Entities _context = new Entities())
            {

                List<INVENTORY_V> inventoryList = Inventory();
                List<MOBILE_INVENTORY_V> mobileInventoryList = new List<MOBILE_INVENTORY_V>();

                foreach (INVENTORY_V item in inventoryList)
                {
                    MOBILE_INVENTORY_V rItem = new MOBILE_INVENTORY_V();
                    rItem.ITEM_ID = item.ITEM_ID;
                    rItem.SEGMENT1 = item.SEGMENT1;
                    rItem.ITEM_DESCRIPTION = item.DESCRIPTION;
                    rItem.UOM_CODE = item.UOM_CODE;
                    rItem.ENABLED_FLAG = item.ENABLED_FLAG;
                    rItem.ACTIVE = item.ACTIVE;
                    rItem.ITEM_COST = item.ITEM_COST;
                    rItem.LE = item.LE;
                    rItem.INV_NAME = item.INV_NAME;
                    rItem.INV_LOCATION = item.INV_LOCATION;
                    rItem.ORGANIZATION_ID = item.ORGANIZATION_ID;
                    mobileInventoryList.Add(rItem);
                    rItem = null;
                }
                return mobileInventoryList;
            }
        }

        public static List<INVENTORY_V> GetActiveInventory(long OrgId = 0)
        {
            using (Entities _context = new Entities())
            {
                if (OrgId == 0)
                {
                    return _context.Set<INVENTORY_V>().Where(i => i.ACTIVE == "Y").ToList();
                }
                else
                {
                    return _context.Set<INVENTORY_V>().Where(i => i.ACTIVE == "Y").Where(i => i.ORGANIZATION_ID == OrgId).ToList();
                }
            }
        }
    }





}


