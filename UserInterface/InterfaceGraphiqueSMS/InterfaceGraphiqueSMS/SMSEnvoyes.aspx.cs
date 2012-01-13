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
            if (!Page.IsPostBack)
            {
                Message[] listeMessages;
                listeMessages = (from mess in dbContext.MessageEnvoi select mess.Message).ToArray();
                ListMessages.DataSource = listeMessages;
                ListMessages.DataTextField = "messageTexte";
                ListMessages.DataValueField = "idMessage";
                ListMessages.DataBind();
            }
        }

        protected void ListMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Message detailsMessage = (from mess in dbContext.Message where mess.idMessage == int.Parse(ListMessages.SelectedValue) select mess).ToArray()[0];
            tbMessage.Text = detailsMessage.messageTexte;
            tbDestinataire.Text = detailsMessage.noDestinataire;
            tbEmetteur.Text = detailsMessage.noEmetteur;
            Statut statutMsg = (from st in dbContext.Statut where st.idStatut == detailsMessage.idStatut select st).First();
            tbStatut.Text = statutMsg.libelleStatut;
            Encodage encodageMsg = (from enc in dbContext.Encodage where enc.idEncodage == detailsMessage.idEncodage select enc).First();
            tbEncodage.Text = encodageMsg.libelleEncodage;
            tbPDU.Text = detailsMessage.messagePDU;
            tbDateEnvoi.Text = detailsMessage.MessageEnvoi.dateEnvoi.ToString();
            tbDateDemande.Text = detailsMessage.MessageEnvoi.dateDemande.ToString();
        }
    }
}