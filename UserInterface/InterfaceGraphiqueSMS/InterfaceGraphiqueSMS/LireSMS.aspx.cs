using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterfaceGraphiqueSMS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SMSBDDataContext dbContext = new SMSBDDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Message[] listeMessages;
            listeMessages = (from mess in dbContext.Message select mess).ToArray();
            ListMessages.DataSource = listeMessages;
            ListMessages.DataTextField = "messageTexte";
            ListMessages.DataValueField = "idMessage";
            ListMessages.DataBind();
            tbMessage.Text = "troc";
        }

        protected void ListMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbMessage.Text = "truc";
        }
    }
}