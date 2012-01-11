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
            //on charge tous les encodages
            Encodage[] listeEncodages = (from encs in dbContext.Encodage select encs).ToArray();

            //on peuple le drop down liste pour les encodages
            DropDownEncodage.DataSource = listeEncodages;
            DropDownEncodage.DataTextField = "libelleEncodage";
            DropDownEncodage.DataValueField = "idEncodage";
            DropDownEncodage.DataBind();
        }

        protected void EcrireSMS(object sender, EventArgs e)
        {
            //insertion d'un message
            Message msg = new Message();
            msg.messagePDU = contenuSMS.Text;
            msg.noDestinataire = numDestinataire.Text;
            //on recupere l'encodage
            msg.Encodage = (from enc in dbContext.Encodage where enc.idEncodage == int.Parse(DropDownEncodage.SelectedValue) select enc).First();


            //selectionne statut en attente
            Statut stat = (from st in dbContext.Statut where st.libelleStatut == "En attente" select st).First();
            msg.Statut = stat;

            dbContext.Message.InsertOnSubmit(msg);
            dbContext.SubmitChanges();

            Response.Write("<script> $(\"#dialog\").dialog(); </script>"); 

        }
    }
}