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

            //selectionne ascii comme encodage
            Encodage enc = (from en in dbContext.Encodage select en).First();
            msg.Encodage = enc;

            //selectionne statut en attente
            Statut stat = (from st in dbContext.Statut where st.libelleStatut == "En attente" select st).First();
            msg.Statut = stat;

            dbContext.Message.InsertOnSubmit(msg);
            dbContext.SubmitChanges();

        }
    }
}