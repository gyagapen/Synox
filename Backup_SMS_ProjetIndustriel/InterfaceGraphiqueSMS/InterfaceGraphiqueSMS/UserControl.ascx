<% @ Control Language="C#" ClassName="InLineScript" %>

<script runat="server">

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

                    using (System.IO.StringWriter sWriter = new System.IO.StringWriter(sb))
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

</script>