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
        }

        protected void ListMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Message DetailsMessage = (from mess in dbContext.Message where mess.idMessage == ListMessages.SelectedIndex select mess).ToArray()[0];
            tbMessage.Text = ListMessages.SelectedIndex.ToString();
            //tbMessage.Text += "a";
        }
    }
}