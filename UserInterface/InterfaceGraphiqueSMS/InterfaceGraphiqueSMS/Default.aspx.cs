using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace InterfaceGraphiqueSMS
{
    public partial class _Default : System.Web.UI.Page
    {

        //contexte de la base de donnees
        SMSBDDataContext dbContext = new SMSBDDataContext();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //on charge tous les encodages
                Encodage[] listeEncodages = (from encs in dbContext.Encodage select encs).ToArray();
                //Response.Write(listeEncodages.First().libelleEncodage);
                //on peuple le drop down liste pour les encodages
                DropDownEncodage.DataSource = listeEncodages;
                DropDownEncodage.DataTextField = "libelleEncodage";
                DropDownEncodage.DataValueField = "idEncodage";
                DropDownEncodage.DataBind();
            }
        }

        protected void EcrireSMS(object sender, EventArgs e)
        {
            Message msg = new Message();

            if (ListeMode.SelectedValue == "Texte")
            {
                // Il s'agit d'un message texte

                //insertion d'un message
                msg.messageTexte = contenuSMS.Text;
                msg.noDestinataire = numDestinataire.Text;
                //on recupere l'encodage
                msg.Encodage = (from enc in dbContext.Encodage where enc.idEncodage == int.Parse(DropDownEncodage.SelectedValue) select enc).First();

                //demande accuse reception
                if (CheckBoxAccuse.Checked) // on a demande un accuse
                {
                    msg.accuseReception = 1;
                }
                else
                {
                    msg.accuseReception = 0;
                }
            }
            else
            {
                // Message PDU
                msg.messagePDU = contenuSMS.Text;

                // Encodage PDU, pas propre !!!
                msg.Encodage = (from enc in dbContext.Encodage where enc.idEncodage == 4 select enc).First(); 
            }

            dbContext.Message.InsertOnSubmit(msg);

            //on cree un message envoi
            MessageEnvoi smsEnvoi = new MessageEnvoi();
            smsEnvoi.Message = msg;
            smsEnvoi.dateDemande = DateTime.Now;
            //selectionne statut en attente
            Statut stat = (from st in dbContext.Statut where st.libelleStatut == "En attente" select st).First();
            smsEnvoi.Statut = stat;

            if (ListeMode.SelectedValue == "Texte")
            {
                int nbJours, nbHeures, nbMinutes;

                if (tbJours.Text == null || tbJours.Text=="")
                    nbJours = 0;
                else
                    nbJours = int.Parse(tbJours.Text);

                if (tbHeures.Text == null || tbHeures.Text=="")
                    nbHeures = 0;
                else
                    nbHeures = int.Parse(tbHeures.Text);

                if (tbMinutes.Text == null || tbMinutes.Text == "")
                    nbMinutes = 5;
                else
                {
                    nbMinutes = int.Parse(tbMinutes.Text);
                    if (nbMinutes < 5)
                        nbMinutes = 5;
                }

                //duree de validite
                TimeSpan duree = new TimeSpan(nbJours, nbHeures, nbMinutes, 0, 0);

                if (duree.Days > 30) //Up to 441 days
                    smsEnvoi.dureeValidite = (byte)(192 + (int)(duree.Days / 7));
                else if (duree.Days >= 1) //Up to 30 days
                    smsEnvoi.dureeValidite = (byte)(166 + duree.Days);
                else if (duree.Hours > 12) //Up to 24 hours
                    smsEnvoi.dureeValidite = (byte)(143 + (duree.Hours - 12) * 2 + duree.Minutes / 30);
                else if (duree.Hours >= 1 || duree.Minutes > 1) //Up to 12 hours
                    smsEnvoi.dureeValidite = (byte)(duree.Hours * 12 + duree.Minutes / 5 - 1);
                else
                    smsEnvoi.dureeValidite = 0;
            }

            dbContext.MessageEnvoi.InsertOnSubmit(smsEnvoi);

            dbContext.SubmitChanges();

            Response.Write("<script> $(\"#dialog\").dialog(); </script>"); 

        }

        protected void ListeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            IEnumerator ctrls = UpdatePanel1.Controls.GetEnumerator();
            while (ctrls.MoveNext())
            {
                if (ListeMode.SelectedValue == "PDU")
                {
                    ((Control)ctrls.Current).Visible = false;
                }
                else
                {
                    ((Control)ctrls.Current).Visible = true;
                }
            }
            //TextBox1.Visible = false;
        }

        protected void tbJours_TextChanged(object sender, EventArgs e)
        {
            int conv;
            if (int.TryParse(tbJours.Text, out conv))
            {
                if (conv > 441)
                {
                    tbJours.Text = "441";
                }
            }
            else
            {
                tbJours.Text = null;
            }
        }

        protected void DropDownEncodage_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DropDownEncodage.SelectedValue)
            {
                case "1":
                    // encodage 7 bits
                    contenuSMS.MaxLength = 160;
                    break;
                case "2":
                    // ecodage 8 bits
                    contenuSMS.MaxLength = 140;
                    break;
                case "3":
                    // encodage 16 bits
                    contenuSMS.MaxLength = 70;
                    break;
                case "4":
                    // Mode PDU, on ne fixe pas de limite
                    //contenuSMS.MaxLength = 0;
                    break;
            }
            contenuSMS.MaxLength = 5;
        }

        protected void tbHeures_TextChanged(object sender, EventArgs e)
        {
            int conv;
            if (int.TryParse(tbHeures.Text, out conv))
            {
                if (conv > 24)
                {
                    tbHeures.Text = "24";
                }
            }
            else
            {
                tbHeures.Text = null;
            }
        }

        protected void tbMinutes_TextChanged(object sender, EventArgs e)
        {
            int conv;
            if (int.TryParse(tbMinutes.Text, out conv))
            {
                if (conv > 60)
                {
                    tbMinutes.Text = "60";
                }
            }
            else
            {
                tbMinutes.Text = null;
            }
        }
    }
}