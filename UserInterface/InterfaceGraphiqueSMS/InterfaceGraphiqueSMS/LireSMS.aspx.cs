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
                populateTableSMSRecus();


            }
        }


        [AjaxPro.AjaxMethod]
        //fournit des details sur le SMS
        public void saveIdSMS(int idMessage)
        {
            Session["noSMS"] = idMessage;
        }

        protected void buttonCache_clicked(object sender, EventArgs e)
        {
            //on va sauvegarde la date de premiere lecture si non renseigné
            populateSMSField(int.Parse(Session["noSMS"].ToString()), true);
            UpdatePanel2.Update();

        }

        //lorsque renseignerDateLecture est a vrai, on associe une date de premiere lecture au sms
        public void populateSMSField(int idMessage, Boolean renseignerDateLecture = false)
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
            }
        }

        //remplit le tableau des SMS
        private void populateTableSMSRecus()
        {
            Message[] listeMessages;
            listeMessages = (from mess in dbContext.MessageRecu orderby mess.idMessage descending select mess.Message).ToArray();

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


            TableSMSEnvoyes.Rows.Add(ligneHeader);

            foreach (Message sms in listeMessages)
            {
                TableRow ligne = new TableRow();


                //no emetteur
                TableCell cDest = new TableCell();
                cDest.Text = sms.noEmetteur;
                ligne.Cells.Add(cDest);

                //Message
                TableCell cMsg = new TableCell();
                cMsg.Text = sms.messageTexte;
                ligne.Cells.Add(cMsg);



                //date de reception
                TableCell cDemande = new TableCell();
                cDemande.Text = sms.MessageRecu.dateReception.ToString();
                ligne.Cells.Add(cDemande);

                //date de lecture
                TableCell cEnvoi = new TableCell();
                cEnvoi.Text = sms.MessageRecu.dateLecture.ToString();
                ligne.Cells.Add(cEnvoi);


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
                
                ligne.Cells.Add(cAccuse);




                //on ajoute un evenement javascript pour recuperer le click du tableau
                ligne.Attributes.Add("ondblclick", "selectTableSMS(" + sms.idMessage + ")");


                //on ajoute la ligne au tableau
                TableSMSEnvoyes.Rows.Add(ligne);

            }
        }

    }
}