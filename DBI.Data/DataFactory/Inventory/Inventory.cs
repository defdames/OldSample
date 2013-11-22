﻿using System;
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

         public static List<INVENTORY_V> GetActiveInventory(long OrgId=0)
            {
                using (Entities _context = new Entities())
                {
                    if (OrgId == 0)
                    {
                        return _context.Set<INVENTORY_V>().Where(i => i.ACTIVE == "Y").ToList();
                    }
                    else
                    {
                        return _context.Set<INVENTORY_V>().Where(i => i.ACTIVE == "Y").Where(i=> i.ORGANIZATION_ID == OrgId).ToList();
                    }
                }
            }
    }





}


