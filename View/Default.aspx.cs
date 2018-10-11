using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        

            if(Page.User.Identity.IsAuthenticated)
            {
                LoginRedirectByRoleSection roleRedirectSection = (LoginRedirectByRoleSection)ConfigurationManager.GetSection("loginRedirectByRole");
                foreach (RoleRedirect roleRedirect in roleRedirectSection.RoleRedirects)
                {
                    if (Roles.IsUserInRole(Page.User.Identity.Name, roleRedirect.Role))
                    {
                        Response.Redirect(roleRedirect.Url);
                    }
                }
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            
       

    }
}