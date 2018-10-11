using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GlobalInfo;
using Comercial;
using Comercial.Cotizador;


public partial class Com_Adm_Default_ : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Usuario Usuario = new Usuario();
        INFOcliente DatosCli = new INFOcliente();
        Infousuario infousu = Usuario.Info;
        if (!IsPostBack)
        {
            Mensaje Msj;
            HtmlGenericControl br;
            string[] UserList = new string[3];
            UserList[0] = "ALLGLASSER";
            UserList[1] = Page.User.Identity.Name;
            UserList[2] = "ALLCOMERCIAL";
            Msj = new Mensaje(UserList);
            if (Msj.HasMessages)
            {
                PanelAlerta.Visible = true;
                foreach (var item in Msj.Mensajes)
                {
                    HtmlGenericControl HeadMsj = new HtmlGenericControl("label") { InnerHtml = item.From + " : &nbsp;" };
                    HeadMsj.Attributes.Add("class", "font-weight-bold");
                    MsjPortal.Controls.Add(HeadMsj);
                    HtmlGenericControl Mensaje = new HtmlGenericControl("label") { InnerHtml = item.Cuerpo };
                    MsjPortal.Controls.Add(Mensaje);
                    br = new HtmlGenericControl("br");
                    MsjPortal.Controls.Add(br);

                }


            }


        }



    }


    protected void btnon_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnAcc_Click(object sender, EventArgs e)
    {
        GetPrecioUnit getPrecio = new GetPrecioUnit("DISTRIBUCIÓN",Convert.ToInt32(Txt1.Text), 1500, 1800);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Neto:" + getPrecio.Neto  + " Ben:" +
            getPrecio.Beneficio + " iva:" + getPrecio.Iva + " " + getPrecio.Bruto + "'); ", true);
    }
}