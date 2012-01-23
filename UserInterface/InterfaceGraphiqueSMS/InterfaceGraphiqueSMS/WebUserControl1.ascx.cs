using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace InterfaceGraphiqueSMS
{
    [ParseChildren(false)]
    [PersistChildren(true)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    public partial class Controls_User_Controls_ScriptContainer : System.Web.UI.UserControl
    {
        string _innerText = null;

        public string InnerText
        {
            get
            {
                if (_innerText == null)
                {
                    StringBuilder sb = new StringBuilder();

                    using (StringWriter sWriter = new StringWriter(sb))
                    {
                        using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sWriter))
                        {
                            RenderChildren(htmlWriter);
                        }
                    }

                    _innerText = sb.ToString().Trim();
                }

                return _innerText;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Page.IsPostBack && this.Visible)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), String.Format("{0}_loadScripts", ClientID), InnerText, false);
            }
        }

    }
}  

