using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Synox.Web.ServiceSms
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBoxMessage.Text = @"<@R><PASS>XXXX<\PASS><NUMEROTELSIM><\NUMEROTELSIM><IP01><\IP01><@\R>";

                HidePanels();
                ButtonEnvoyes_Click(null, null);
            }

        }
        private void HidePanels()
        {
            PanelTest.Visible = false;
            PanelSmsAttente.Visible = false;
            PanelSmsEnErreur.Visible = false;
            PanelSmsRecu.Visible = false;
            PanelSmsEnvoyes.Visible = false;
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Bind();
        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            HidePanels();
            Synox.Services.ServiceSMS.Helpers.SmsHelper.SaveSmsAEnvoyer(TextBoxGsm.Text, TextBoxMessage.Text);
            ButtonEnAttente_Click(null, null);
        }

        protected void ButtonTest_Click(object sender, EventArgs e)
        {
            HidePanels();
            PanelTest.Visible = true;
        }
        protected void ButtonEnAttente_Click(object sender, EventArgs e)
        {
            HidePanels();
            PanelSmsAttente.Visible = true;
            BindSmsEnAttente();
        }
        protected void ButtonEnErreur_Click(object sender, EventArgs e)
        {
            HidePanels();
            PanelSmsEnErreur.Visible = true;
            BindSmsEnErreur();
        }
        protected void ButtonEnvoyes_Click(object sender, EventArgs e)
        {
            HidePanels();
            PanelSmsEnvoyes.Visible = true;
            BindSmsEnvoyes();
        }
        protected void ButtonRecus_Click(object sender, EventArgs e)
        {
            HidePanels();
            PanelSmsRecu.Visible = true;
            BindSmsEnRecus();
        }

        #region Pager ListView
        protected void ListViewSmsEnAttente_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager pager = (DataPager)ListViewSmsEnAttente.Controls[0].FindControl("Pager");
            pager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindSmsEnAttente();
        }
        protected void ListViewSmsEnErreur_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager pager = (DataPager)ListViewSmsEnErreur.Controls[0].FindControl("Pager");
            pager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindSmsEnErreur();
        }
        protected void ListViewSmsRecus_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager pager = (DataPager)ListViewSmsEnErreur.Controls[0].FindControl("Pager");
            pager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindSmsEnRecus();
        }
        protected void ListViewSmsEnvoyes_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager pager = (DataPager)ListViewSmsEnvoyes.Controls[0].FindControl("Pager");
            pager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindSmsEnvoyes();
        }
        #endregion
        #region Bind
        protected void BindSmsEnvoyes()
        {

            ListViewSmsEnvoyes.DataSource = Synox.Services.ServiceSMS.Helpers.SmsHelper.GetSmsEnvoyes().OrderByDescending(s => s.Id).ToList();
            ListViewSmsEnvoyes.DataBind();
        }
        protected void BindSmsEnAttente()
        {

            ListViewSmsEnAttente.DataSource = Synox.Services.ServiceSMS.Helpers.SmsHelper.GetSmsEnAttente().OrderByDescending(s => s.Id).ToList();
            ListViewSmsEnAttente.DataBind();
        }
        protected void BindSmsEnErreur()
        {

            ListViewSmsEnErreur.DataSource = Synox.Services.ServiceSMS.Helpers.SmsHelper.GetSmsErreur().OrderByDescending(s => s.Id).ToList();
            ListViewSmsEnErreur.DataBind();
        }
        protected void BindSmsEnRecus()
        {

            ListViewSmsRecus.DataSource = Synox.Services.ServiceSMS.Helpers.SmsReceptionHelper.GetSmsRecus().OrderByDescending(s => s.Id).ToList();
            ListViewSmsRecus.DataBind();
        }
        #endregion

    }
}