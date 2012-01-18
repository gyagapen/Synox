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

                populateTableSMSEnvoyes();

                
            }
        }

        protected void ListMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Message detailsMessage = (from mess in dbContext.Message where mess.idMessage == int.Parse(ListMessages.SelectedValue) select mess).ToArray()[0];
            tbMessage.Text = detailsMessage.messageTexte;
            tbDestinataire.Text = detailsMessage.noDestinataire;
            tbEmetteur.Text = detailsMessage.noEmetteur;

            Statut statutMsg = (from st in dbContext.Statut where st.idStatut == detailsMessage.MessageEnvoi.idStatut select st).First();
            tbStatut.Text = statutMsg.libelleStatut;

            Encodage encodageMsg = (from enc in dbContext.Encodage where enc.idEncodage == detailsMessage.idEncodage select enc).First();
            tbEncodage.Text = encodageMsg.libelleEncodage;

            tbPDU.Text = detailsMessage.messagePDU;
            tbDateEnvoi.Text = detailsMessage.MessageEnvoi.dateEnvoi.ToString();
            tbDateDemande.Text = detailsMessage.MessageEnvoi.dateDemande.ToString();
        }


        private void populateTableSMSEnvoyes()
        {
            Message[] listeMessages;
            listeMessages = (from mess in dbContext.MessageEnvoi select mess.Message).ToArray();

            foreach (Message sms in listeMessages)
            {
                TableRow ligne = new TableRow();
                
                //reference envoi
                TableCell cRef = new TableCell();
                cRef.Text = sms.MessageEnvoi.referenceEnvoi;
                ligne.Cells.Add(cRef);

                //no destinataire
                TableCell cDest = new TableCell();
                cRef.Text = sms.noDestinataire;
                ligne.Cells.Add(cDest);

                //Message
                //si message PDU seulement
                if (sms.messageTexte == null)
                {
                    TableCell cMsg = new TableCell();
                    cMsg.Text = "Trame PDU";
                    ligne.Cells.Add(cMsg);
                }
                else
                {
                    TableCell cMsg = new TableCell();
                    cMsg.Text = sms.messageTexte;
                    ligne.Cells.Add(cMsg);
                }


                //Etat
                TableCell cStatut = new TableCell();
                cStatut.Text = sms.MessageEnvoi.Statut.libelleStatut;
                ligne.Cells.Add(cStatut);

                //on ajoute la ligne au tableau
                TableSMSEnvoyes.Rows.Add(ligne);
            }

        }
    }
}