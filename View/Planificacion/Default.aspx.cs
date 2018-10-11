using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RandomERP;
using RandomERP.ConsumosPPR;
using PlanificacionOT;
using GlobalInfo;
using System.Web.UI.HtmlControls;

public partial class _Default : Page
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
            UserList[2] = "ALLPLANIFICACION";
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


}