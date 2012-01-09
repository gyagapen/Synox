using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestWebApplication1
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public String piedDePage
        {
            get
            {
                return ltlPiedDePage.Text;
            }
            set
            {
                ltlPiedDePage.Text = value;
            }
        }
    }
}