using System;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class Site1 : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var username = Session["Username"] as string;
                if (!string.IsNullOrEmpty(username))
                {
                    pnlLoggedIn.Visible = true;
                    pnlLoggedOut.Visible = false;
                    lblUser.Text = Server.HtmlEncode(username);
                }
                else
                {
                    pnlLoggedIn.Visible = false;
                    pnlLoggedOut.Visible = true;
                }
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Default.aspx");
        }
    }
}