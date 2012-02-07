using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxPro;

namespace InterfaceGraphiqueSMS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SMSBDDataContext dbContext = new SMSBDDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            //initialisation AJAX
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InterfaceGraphiqueSMS.WebForm1));


            if (!Page.IsPostBack)
            {

                //rafraichissement de la page chaque 15 secondes
                TimerRefresh.Interval = 15000;
                TimerRefresh.Enabled = true;

                //reinitialisation de la recherche
                Session["search"] = "";


                populateTableSMSEnvoyes();
            }
        }


      
        [AjaxPro.AjaxMethod]
        //fournit des details sur le SMS
        public void saveIdSMS(int idMessage)
        {
            Session["noSMS"] = idMessage;
        }


        [AjaxPro.AjaxMethod]
        //enregistre la recherche
        public void saveSearch(string recherche)
        {
            Session["search"] = recherche;
        }

        protected void buttonCache_clicked(object sender, EventArgs e)
        {
            populateSMSField(int.Parse(Session["noSMS"].ToString()));
          
        }



        protected void buttonSearch_clicked(object sender, EventArgs e)
        {
            populateTableSMSEnvoyes(Session["search"].ToString());
            //on update le panel
            UpdatePanel2.Update();

        }

        public void populateSMSField(int idMessage)
        {
            Message detailsMessage = (from mess in dbContext.Message where mess.idMessage == idMessage select mess).First();

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

        protected void rafraichirPage(object sender, EventArgs e)
        {
            populateTableSMSEnvoyes(Session["search"].ToString());
            UpdatePanel2.Update();
        }


        static string addslashes(string txt)
        {
            txt.Replace("'", "\\'");
            txt.Replace("\"", "\\\"");
            return (txt);
        }

        //supprime le message dont l'id est passe en parametre
        public void supprimerSMS(string idMessage)
        {

            //on recupere le message
            Message detailsMessage = (from mess in dbContext.Message where mess.idMessage == int.Parse(idMessage) select mess).First();


            //on le supprime
            dbContext.MessageEnvoi.DeleteOnSubmit(detailsMessage.MessageEnvoi);
            dbContext.Message.DeleteOnSubmit(detailsMessage);

            dbContext.SubmitChanges();


        }

        protected void buttonSupprimer_clicked(object sender, EventArgs e)
        {
            supprimerSMS(Session["noSMS"].ToString());
            populateTableSMSEnvoyes(Session["search"].ToString());
            //on update le panel
            UpdatePanel2.Update();

        }

        //remplit le tableau des SMS
        private void populateTableSMSEnvoyes(string elementRecherche="")
        {
            try{
                Message[] listeMessages;
            listeMessages = (from mess in dbContext.MessageEnvoi
                             where mess.Message.messageTexte.Contains(elementRecherche)
                             || mess.Message.noDestinataire.Contains(elementRecherche)
                             || mess.dateDemande.ToString().Contains(elementRecherche)
                             || mess.dateEnvoi.ToString().Contains(elementRecherche)
                             || mess.Statut.libelleStatut.Contains(elementRecherche)
                             orderby mess.idMessage descending 
                             select mess.Message).ToArray();

            //en tete du tableau
            TableHeaderRow ligneHeader = new TableHeaderRow();

            //Les cellules

            //destinaire
            TableHeaderCell headDest = new TableHeaderCell();
            headDest.Text = "Destinataire";
            ligneHeader.Cells.Add(headDest);

            //Message
            TableHeaderCell headMSG = new TableHeaderCell();
            headMSG.Text = "Message";
            ligneHeader.Cells.Add(headMSG);
            
            //Date demande Envoi
            TableHeaderCell headDemande = new TableHeaderCell();
            headDemande.Text = "Date Demande Envoi";
            ligneHeader.Cells.Add(headDemande);

            //Date d'envoi
            TableHeaderCell headEnvoi = new TableHeaderCell();
            headEnvoi.Text = "Date Envoi";
            ligneHeader.Cells.Add(headEnvoi);

            //Statut
            TableHeaderCell headStatut = new TableHeaderCell();
            headStatut.Text = "Statut";
            ligneHeader.Cells.Add(headStatut);

            //Actions
            TableHeaderCell headAction = new TableHeaderCell();
            headAction.Text = "Action";
            ligneHeader.Cells.Add(headAction);

            TableSMSEnvoyes.Rows.Add(ligneHeader);

            foreach (Message sms in listeMessages)
            {
                TableRow ligne = new TableRow();


                //no destinataire
                TableCell cDest = new TableCell();
                cDest.Text = sms.noDestinataire;
                cDest.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                ligne.Cells.Add(cDest);

                //Message
                //si message PDU seulement
                if (sms.messageTexte == null)
                {
                    TableCell cMsg = new TableCell();
                    cMsg.Text = "Trame PDU (Cliquer pour plus de details)";
                    cMsg.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                    ligne.Cells.Add(cMsg);
                }
                else
                {
                    TableCell cMsg = new TableCell();

                    //longueur du msg a afficher dans le tableau
                    if (sms.messageTexte.Length > 30)
                        cMsg.Text = sms.messageTexte.Substring(0, 29) + "...";
                    else
                        cMsg.Text = sms.messageTexte;

                    cMsg.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                    ligne.Cells.Add(cMsg);
                }


                //date de demande d'envoi
                TableCell cDemande = new TableCell();
                cDemande.Text = sms.MessageEnvoi.dateDemande.ToString();
                cDemande.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                ligne.Cells.Add(cDemande);

                //date d'envoi
                TableCell cEnvoi = new TableCell();
                cEnvoi.Text = sms.MessageEnvoi.dateEnvoi.ToString();
                cEnvoi.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                ligne.Cells.Add(cEnvoi);



                //Etat
                TableCell cStatut = new TableCell();
                cStatut.Text = sms.MessageEnvoi.Statut.libelleStatut;
                cStatut.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                ligne.Cells.Add(cStatut);

                //Action
                TableCell cAction = new TableCell();
                cAction.Text = "<img src='css\\images\\supprimer.gif' width='20' onclick='supprimerSMS(" + sms.idMessage + ")' style='cursor:pointer'/>";
                ligne.Cells.Add(cAction);

                //on ajoute un evenement javascript pour recuperer le click du tableau
                //ligne.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");


                //on ajoute la ligne au tableau
                TableSMSEnvoyes.Rows.Add(ligne);
           

            }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("transport"))
                {
                    populateTableSMSEnvoyes(elementRecherche);
                }
            }

        }

    }
}