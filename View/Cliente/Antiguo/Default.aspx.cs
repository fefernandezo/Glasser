using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
            UserList[0] = "ALLCLIENTES";
            UserList[1] = Page.User.Identity.Name;
            UserList[2] = infousu.Empresa.Trim();
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
        

     
        

        DatosCli.Actualizarestado(infousu.IdEmpresa);
        string nombre = infousu.Empresa;

        Page.Title = nombre;
        if (infousu.Rutempresa != "" && infousu.Rutempresa != null)
        {
            Montos InfoFin = DatosCli.Montos();


            lblCreditoDis.Text = InfoFin.Credit.ToString("C0");

            lblDeuda.Text = InfoFin.Utilizado.ToString("C0");
            LabelDisponible.Text = InfoFin.Disponible.ToString("C0");
            Hiddendisponible.Value = InfoFin.Disponible.ToString();
            LblCliente.Text = "Cliente: " + infousu.Empresa;
            
        }
        else
        {

        }
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
        FunDetPed Funcion = new FunDetPed();
        PlazoEntrega Plazoentregatermo = Funcion.PlazoEntrega("Termo");
        diastermo.Text = "(" + Plazoentregatermo.Dias.ToString() + " días hábiles)";
        FechaTermo.Text = "Entrega el " + Plazoentregatermo.Fecha.ToShortDateString();
        PlazoEntrega Plazolamina = Funcion.PlazoEntrega("Lamina");
        DiasLaminas.Text = "(" + Plazolamina.Dias.ToString() + " días hábiles)";
        FechaLaminas.Text = "Entrega el " + Plazolamina.Fecha.ToShortDateString();
        PlazoEntrega Plazoarq = Funcion.PlazoEntrega("Arq");
        DiasArq.Text = "(" + Plazoarq.Dias.ToString() + " días hábiles)";
        FechaArq.Text = "Entrega el " + Plazoarq.Fecha.ToShortDateString();
    }
}