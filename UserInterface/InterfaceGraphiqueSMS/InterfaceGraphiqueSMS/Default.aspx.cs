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
            //insertion d'un message
            Message msg = new Message();
            msg.messageTexte = contenuSMS.Text;
            msg.noDestinataire = numDestinataire.Text;
            //on recupere l'encodage
            msg.Encodage = (from enc in dbContext.Encodage where enc.idEncodage == int.Parse(DropDownEncodage.SelectedValue) select enc).First();
            


            //selectionne statut en attente
            Statut stat = (from st in dbContext.Statut where st.libelleStatut == "En attente" select st).First();
            msg.Statut = stat;

            
            


            //demande accuse reception
            if (CheckBoxAccuse.Checked) // on a demande un accuse
            {
                msg.accuseReception = 1;
            }
            else
            {
                msg.accuseReception = 0;
            }

            dbContext.Message.InsertOnSubmit(msg);

            //on cree un message envoi
            MessageEnvoi smsEnvoi = new MessageEnvoi();
            smsEnvoi.Message = msg;
            smsEnvoi.dateDemande = DateTime.Now;

            //duree de validite
            smsEnvoi.dureeValidite = 0;
            
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
    }
}