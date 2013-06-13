using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace DBI.Core.Web
{
    public class BaseUserControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Load User Control
        /// </summary>
        /// <param name="ucName"></param>
        /// <param name="ucPanel"></param>
        /// <param name="update"></param>
        public void LoadUserControl(string ucName, Ext.Net.Panel ucPanel, bool update = false)
        {
            if (update && ucName != null)
            {
                ucPanel.ContentControls.Clear();
            }

            System.Web.UI.UserControl userControl = (UserControl)this.LoadControl(string.Format("{0}.ascx", ucName));
            userControl.ID = "UC" + new Random();
            ucPanel.ContentControls.Add(userControl);

            if (update)
            {
                ucPanel.UpdateContent();
            }
        }
    }
}
