using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestWebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = "Bienvenue !!!";
            Master.piedDePage = "Merci de prendre quelques instants pour répondre à notre <a href=EnqueteSatisfaction.aspx>enquête de satisfaction</a>";
        }

        protected void ActivationTest(object sender, EventArgs e)
        {
            //Response.Write("<script>alert('"+TextBox1.Text+"')</script>");
            MessageBox(TextBox1.Text);
            //Label1.Text = TextBox1.Text;
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }
    }
}