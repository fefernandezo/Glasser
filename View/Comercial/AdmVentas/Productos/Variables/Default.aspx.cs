using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.Web.UI.WebControls;

using Microsoft.AspNet.Membership.OpenAuth;

public partial class _Default_Variables : System.Web.UI.Page
{
  

    protected void Page_Load()
    {
       
    }






    protected void BtnAdmvariable_Click(object sender, EventArgs e)
    {
        Redireccionamiento("~/View/Comercial/AdmVentas/Productos/Variables/AdministracionVariables.aspx?ID=", HdnModal1.Value,"Paseparairaadmvariables");
        


    }

    private void Redireccionamiento(string url, string valor, string pase)
    {
        Encriptacion encriptacion = new Encriptacion(pase);
        string Token = encriptacion.TokenEncriptado;
        
        Response.Redirect(url + valor +"&Request="+ Token);
    }

    protected void BtnCrearVar_Click(object sender, EventArgs e)
    {
        Redireccionamiento("~/View/Comercial/AdmVentas/Productos/Variables/CreaciondeVariables.aspx?ID=", HdnModal1.Value,"Paseparairacreacionvar");
    }
}