using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterfaceGraphiqueSMS
{
    public partial class _Default : System.Web.UI.Page
    {

        //contexte de la base de donnees
        SMSBDDataContext dbContext = new SMSBDDataContext();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void EcrireSMS(object sender, EventArgs e)
        {
            //insertion d'un message
            Message msg = new Message();
            msg.messagePDU = contenuSMS.Text;
            msg.noDestinataire = numDestinataire.Text;

            dbContext.Message.InsertOnSubmit(msg);
            dbContext.SubmitChanges();

        }
    }
}