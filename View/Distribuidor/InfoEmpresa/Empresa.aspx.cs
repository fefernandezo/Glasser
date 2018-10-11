using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nsCliente;

public partial class Dis_Default_Man : Page
{
    EFinanciero fin;
    DatosCliente Cli;
    nsCliente.Usuario DUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        string rut = Request.QueryString["RUT"];
        if (!string.IsNullOrEmpty(rut))
        {
            HdnRutCli.Value = rut;
            string User = Page.User.Identity.Name;
            DUser = new nsCliente.Usuario(User);
            Cli = DUser.InfoEmpresas.Where(it => it.Rut == rut).First();
        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, "Hubo un error al tratar de buscar la empresa, intentelo en otro momento."));
        }
        


    }


    protected void FillInformation(DatosCliente _Cli)
    {
        
    }

   

   
}