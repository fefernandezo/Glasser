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
    protected void Page_PreInit(object sender, EventArgs e)
    {
        string Master = Request.QueryString["Request"];
        
        MasterPageFile = Master;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string mensaje = Request.QueryString["Msj"];
        Mensaje.Text = mensaje;

    }
}