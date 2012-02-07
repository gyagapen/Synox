using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxPro;

namespace InterfaceGraphiqueSMS
{
    public partial class LireSMS : System.Web.UI.Page
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

                populateTableSMSRecus();
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
            //on va sauvegarde la date de premiere lecture si non renseigné
            populateSMSField(int.Parse(Session["noSMS"].ToString()), true);
        }

        
        protected void buttonSearch_clicked(object sender, EventArgs e)
        {
            populateTableSMSRecus(Session["search"].ToString());
            //on update le panel
            UpdatePanel2.Update();

        }

        protected void buttonSupprimer_clicked(object sender, EventArgs e)
        {
            supprimerSMS(Session["noSMS"].ToString());
            populateTableSMSRecus(Session["search"].ToString());
            //on update le panel
            UpdatePanel2.Update();

        }


        protected void rafraichirPage(object sender, EventArgs e)
        {
            try
            {
                populateTableSMSRecus((Session["search"].ToString()));
            }
            catch
            {
                populateTableSMSRecus();
            }

            UpdatePanel2.Update();
        }

        //supprime le message dont l'id est passe en parametre
        public void supprimerSMS(string idMessage)
        {
            
                //on recupere le message
                Message detailsMessage = (from mess in dbContext.Message where mess.idMessage == int.Parse(idMessage) select mess).First();
           

            //on le supprime
            dbContext.MessageRecu.DeleteOnSubmit(detailsMessage.MessageRecu);
            dbContext.Message.DeleteOnSubmit(detailsMessage);

            dbContext.SubmitChanges();
            

        }

        //lorsque renseignerDateLecture est a vrai, on associe une date de premiere lecture au sms
        public void populateSMSField(int idMessage, Boolean renseignerDateLecture = false)
        {
            try
            {
                Message detailsMessage = (from mess in dbContext.Message where mess.idMessage == idMessage select mess).First();
                tbMessage.Text = detailsMessage.messageTexte;
                tbDestinataire.Text = detailsMessage.noDestinataire;
                tbEmetteur.Text = detailsMessage.noEmetteur;

                tbDateReception.Text = detailsMessage.MessageRecu.dateReception.ToString();
                tbDateLecture.Text = detailsMessage.MessageRecu.dateLecture.ToString();

                if (detailsMessage.accuseReception == 0)
                {
                    tbAccuse.Text = "Non";
                }
                else
                {
                    tbAccuse.Text = "Oui";
                }

                if (renseignerDateLecture)
                {
                    //on va sauvegarde la date de premiere lecture si non renseigné
                    detailsMessage.MessageRecu.dateLecture = DateTime.Now;
                    dbContext.SubmitChanges();
                    populateTableSMSRecus(Session["search"].ToString());
                    UpdatePanel2.Update();
                }
            }
            catch (Exception ex)
            {
                populateSMSField(idMessage, renseignerDateLecture);
            }
        }

        //remplit le tableau des SMS
        private void populateTableSMSRecus(string elementRecherche = "")
        {
            try
            {
                Message[] listeMessages;
                listeMessages = (from mess in dbContext.MessageRecu
                                 where mess.Message.messageTexte.Contains(elementRecherche)
                                 || mess.Message.noEmetteur.Contains(elementRecherche)
                                 || mess.dateReception.ToString().Contains(elementRecherche)
                                 orderby mess.idMessage
                                 descending
                                 select mess.Message).ToArray();

                //en tete du tableau
                TableHeaderRow ligneHeader = new TableHeaderRow();

                //Les cellules

                //destinaire
                TableHeaderCell headDest = new TableHeaderCell();
                headDest.Text = "Emetteur";
                ligneHeader.Cells.Add(headDest);

                //Message
                TableHeaderCell headMSG = new TableHeaderCell();
                headMSG.Text = "Message";
                ligneHeader.Cells.Add(headMSG);

                //Date reception
                TableHeaderCell headDemande = new TableHeaderCell();
                headDemande.Text = "Date Reception";
                ligneHeader.Cells.Add(headDemande);

                //Date lecture
                TableHeaderCell headLecture = new TableHeaderCell();
                headLecture.Text = "Date Lecture";
                ligneHeader.Cells.Add(headLecture);

                //Accuse demandé
                TableHeaderCell headAccuse = new TableHeaderCell();
                headAccuse.Text = "Accuse demandé";
                ligneHeader.Cells.Add(headAccuse);

                //Actions
                TableHeaderCell headAction = new TableHeaderCell();
                headAction.Text = "Action";
                ligneHeader.Cells.Add(headAction);

                TableSMSEnvoyes.Rows.Add(ligneHeader);

                foreach (Message sms in listeMessages)
                {
                    TableRow ligne = new TableRow();


                    //no emetteur
                    TableCell cDest = new TableCell();
                    cDest.Text = sms.noEmetteur;
                    cDest.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                    ligne.Cells.Add(cDest);

                    //Message
                    TableCell cMsg = new TableCell();
                    
                    //longueur du msg a afficher dans le tableau
                    if (sms.messageTexte.Length > 30)
                        cMsg.Text = sms.messageTexte.Substring(0, 29) + "...";
                    else
                        cMsg.Text = sms.messageTexte;

                    cMsg.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                    ligne.Cells.Add(cMsg);



                    //date de reception
                    TableCell cDemande = new TableCell();
                    cDemande.Text = sms.MessageRecu.dateReception.ToString();
                    cDemande.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                    ligne.Cells.Add(cDemande);

                    //date de lecture
                    TableCell cEnvoi = new TableCell();
                    cEnvoi.Text = sms.MessageRecu.dateLecture.ToString();
                    ligne.Cells.Add(cEnvoi);
                    cEnvoi.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");

                    //Accusse demande
                    TableCell cAccuse = new TableCell();

                    if (sms.accuseReception == 0)
                    {
                        cAccuse.Text = "Non";
                    }
                    else
                    {
                        cAccuse.Text = "Oui";
                    }
                    cAccuse.Attributes.Add("onclick", "selectTableSMS(" + sms.idMessage + ")");
                    ligne.Cells.Add(cAccuse);


                    //Action
                    TableCell cAction = new TableCell();
                    cAction.Text = "<img src='css\\images\\repondre.jpg' width='25' onclick='repondreSMS(" + sms.noEmetteur + ")' style='cursor:pointer'/><img src='css\\images\\supprimer.gif' width='20' onclick='supprimerSMS(" + sms.idMessage + ")' style='cursor:pointer'/>";
                    ligne.Cells.Add(cAction);



                    //on ajoute la ligne au tableau
                    TableSMSEnvoyes.Rows.Add(ligne);
                }
            }

            catch (Exception ex)
            {
                //populateTableSMSRecus(elementRecherche);
                throw ex;

            }
        }

    }
}