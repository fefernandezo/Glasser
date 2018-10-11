using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Comercial.Cotizador;
using nsCliente;

public partial class Dis_Default_Man : Page
{
    EFinanciero fin;
    DatosCliente Cli;
    nsCliente.Usuario DUser;
    string rut;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        string User = Page.User.Identity.Name;
        DUser = new nsCliente.Usuario(User);


        if (DUser.HasEmpresa)
        {
            if (DUser.InfoEmpresas.Count > 1)
            {
                PnlBtnToMdlChangeCli.Visible = true;
            }
            else
            {
                PnlBtnToMdlChangeCli.Visible = false;
            }
            if (!IsPostBack)
            {
                rut = Request.QueryString["RUT"];
                if (!string.IsNullOrEmpty(rut))
                {
                    Cli = new DatosCliente(rut);
                }
                else
                {
                    Cli = DUser.InfoEmpresas.First();

                }
                HdnRutCli.Value = Cli.Rut;
                FillInformation(Cli);



            }
            else
            {


            }

        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, User + ", no tienes empresa asignada :(. Por favor comuníquese con el administrador."));
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

    protected void MdlBtnChangeCli_Click(object sender, EventArgs e)
    {
        

        Response.Redirect("~/View/Distribuidor/Default.aspx?RUT=" + DDLEmpresas.SelectedValue);
        
    }

    protected void FillInformation(DatosCliente _Cli)
    {
        FillDDlEmpresas(DUser.InfoEmpresas);
        fin = new EFinanciero(_Cli.Rut, "01");
        Page.Title = _Cli.Nombre;
        lblCreditoDis.Text = (fin.Credito + fin.RiesgoPropio).ToString("C0");
        LabelDisponible.Text = fin.Disponible.ToString("C0");
        Hiddendisponible.Value = fin.Disponible.ToString();
        LblCliente.Text = "Empresa: " + Cli.Nombre;
        lblUtilizado.Text = fin.Deuda.ToString("C0");
        CargarMensajes(Cli.Nombre);
        if (fin.Disponible<=5000)
        {
            BtnCrearPedido.Enabled = false;
            BtnCrearPedido.ToolTip = "No tiene cupo para realizar pedido";
        }
        else
        {
            BtnCrearPedido.Enabled = true;
        }
    }

    protected void FillDDlEmpresas(List<DatosCliente> empresas)
    {

        DDLEmpresas.DataSource = empresas;
        DDLEmpresas.DataTextField = "Nombre";
        DDLEmpresas.DataValueField = "Rut";
        DDLEmpresas.DataBind();
        DDLEmpresas.Items.Insert(0, new ListItem("--Seleccionar Empresa--"));
    }

    protected void CargarMensajes(string Empresa)
    {
        Mensaje Msj;
        HtmlGenericControl br;
        string[] UserList = new string[3];
        UserList[0] = "ALLCLIENTES";
        UserList[1] = Page.User.Identity.Name;
        UserList[2] = Empresa;
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
        else
        {
            PanelAlerta.Visible = false;
        }
    }

    protected void BtnInfoEmpr_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/View/Distribuidor/InfoEmpresa/Empresa.aspx?RUT=" + HdnRutCli.Value);
    }

    protected void BtnCotizar_Click(object sender, EventArgs e)
    {
        Cotizacion.FirstEntry entry = new Cotizacion.FirstEntry(HdnRutCli.Value, Page.User.Identity.Name);
        if (entry.IsSuccess)
        {
            Response.Redirect("~/View/Distribuidor/Cotizador/Formulario.aspx?RUT=" + HdnRutCli.Value + "&ID=" + entry.ID + "&TOKEN=" + entry.Token);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se asignó correctamente el canal seleccionado.'); window.location='" +
        Page.ResolveUrl("~/View/Distribuidor/") + "';", true);
        }
        
    }

    protected void BtnMisCotizaciones_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/View/Distribuidor/Cotizador/MisCotizaciones.aspx?RUT=" + HdnRutCli.Value );

    }
}