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
using Ecommerce;
using nsCliente;

public partial class Cliente_Default_Man : Page
{
    
    DatosCliente Cli;
    GetInfoClienteEcomm ClienteEcom;
    nsCliente.Usuario DUser;
    string rut;
    

    protected void Page_Load(object sender, EventArgs e)
    {

       

        string User = Page.User.Identity.Name;
        DUser = new nsCliente.Usuario(User);
        
        
        if (DUser.HasEmpresa)
        {
            if (DUser.InfoEmpresas.Count>1)
            {
                PnlBtnToMdlChangeCli.Visible = true;
            }
            else
            {
                PnlBtnToMdlChangeCli.Visible = false;
            }
            rut = Request.QueryString["RUT"];
            if (!string.IsNullOrEmpty(rut))
            {
                Cli = new DatosCliente(rut);
                ClienteEcom = new GetInfoClienteEcomm(rut);
            }
            else
            {
                Cli = DUser.InfoEmpresas.First();
                ClienteEcom = new GetInfoClienteEcomm(Cli.Rut);

            }
            if (!IsPostBack)
            {
                
                

                HdnRutCli.Value = Cli.Rut;
                FillInformation(Cli);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
                FechasExpress.Get get;
                get = new FechasExpress.Get("TER",ClienteEcom.DiasDeTrato,7);
                diastermo.Text = "(" + get.DiasEntrega.ToString() + " días hábiles)";
                FechaTermo.Text = "Entrega el " + get.FechaCorte.ToShortDateString();
                get = new FechasExpress.Get("LAMINA",7);
                DiasLaminas.Text = "(" +  get.DiasEntrega.ToString() + " días hábiles)";
                FechaLaminas.Text = "Entrega el " + get.FechaCorte.ToShortDateString();
                get = new FechasExpress.Get("ARQ",0,7);
                DiasArq.Text = "(" + get.DiasEntrega.ToString() + " días hábiles)";
                FechaArq.Text = "Entrega el " + get.FechaCorte.ToShortDateString();



            }
            else
            {


            }

        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, User + ", no tienes empresa asignada :(. Por favor comuníquese con el administrador."));
        }

        
        


        
              
        
        
    }

    protected void MdlBtnChangeCli_Click(object sender, EventArgs e)
    {
        

        Response.Redirect("~/View/Cliente/Default.aspx?RUT=" + DDLEmpresas.SelectedValue);
        
    }

    protected void FillInformation(DatosCliente _Cli)
    {
        FillDDlEmpresas(DUser.InfoEmpresas);
        
        Page.Title = _Cli.Nombre;
        lblCreditoDis.Text = (_Cli.EFinanciero.Credito + _Cli.EFinanciero.RiesgoPropio).ToString("C0");
        LabelDisponible.Text = _Cli.EFinanciero.Disponible.ToString("C0");
        Hiddendisponible.Value = _Cli.EFinanciero.Disponible.ToString();
        LblCliente.Text = "Empresa: " + Cli.Nombre;
        lblUtilizado.Text = _Cli.EFinanciero.Deuda.ToString("C0");
        CargarMensajes(Cli.Nombre);
        if (_Cli.EFinanciero.Disponible<=5000)
        {
            BtnCrearPedido.Enabled = false;
            BtnCrearPedido.ToolTip = "No tiene cupo para realizar pedido.";
        }
        else
        {
            BtnCrearPedido.Enabled = true;
        }
        if (_Cli.Bloqueado)
        {
            BtnCrearPedido.Enabled = false;
            BtnCrearPedido.ToolTip = "Cliente Bloqueado, comuníquese con el depto. de Créditos y Cobranzas.";
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
        Response.Redirect("~/View/Cliente/InfoEmpresa/Empresa.aspx?RUT=" + HdnRutCli.Value);
    }

    protected void BtnCotizar_Click(object sender, EventArgs e)
    {
        Cotizacion.FirstEntry entry = new Cotizacion.FirstEntry(HdnRutCli.Value, Page.User.Identity.Name);
        if (entry.IsSuccess)
        {
            Response.Redirect("~/View/Cliente/Cotizador/Formulario.aspx?RUT=" + HdnRutCli.Value + "&ID=" + entry.ID + "&TOKEN=" + entry.Token);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se asignó correctamente el canal seleccionado.'); window.location='" +
        Page.ResolveUrl("~/View/Cliente/") + "';", true);
        }
        
    }

    protected void BtnMisCotizaciones_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/View/Cliente/Cotizador/MisCotizaciones.aspx?RUT=" + HdnRutCli.Value );

    }

    protected void BtnCrearPedido_Click(object sender, EventArgs e)
    {
        /*abrir modal de creacion de pedido*/
    }

    protected void BtnNewOrder_Click(object sender, EventArgs e)
    {
        if (DDlOrderType.SelectedIndex==0)
        {
            LblDDLtypeordererror.Visible = true;
            LblDDLtypeordererror.Text = "Debe seleccionar el tipo de Pedido";
        }
        else if (DDlOrderType.SelectedIndex==1)
        {
            PedidoEcom.FirstEntry entry = new PedidoEcom.FirstEntry("TER", TxtOrderName.Text, TxtOrderObs.Text, User.Identity.Name, Cli);
            if (entry.IsSuccess)
            {
                Response.Redirect("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + Cli.Rut + "&ID=" + entry.ID + "&TOKEN=" + entry.Token);
            }
        }
        else if (DDlOrderType.SelectedIndex==2)
        {
            PedidoEcom.FirstEntry entry = new PedidoEcom.FirstEntry("TEM", TxtOrderName.Text, TxtOrderObs.Text, User.Identity.Name, Cli);
            if (entry.IsSuccess)
            {
                Response.Redirect("~/View/Cliente/IngresoPedidos/Templados.aspx?RUT=" + Cli.Rut + "&ID=" + entry.ID + "&TOKEN=" + entry.Token);
            }

        }
        else if (DDlOrderType.SelectedIndex == 3)
        {
            PedidoEcom.FirstEntry entry = new PedidoEcom.FirstEntry("DIM", TxtOrderName.Text, TxtOrderObs.Text, User.Identity.Name, Cli);
            if (entry.IsSuccess)
            {
                Response.Redirect("~/View/Cliente/IngresoPedidos/Dimensionados.aspx?RUT=" + Cli.Rut + "&ID=" + entry.ID + "&TOKEN=" + entry.Token);
            }
        }
    }



    protected void BtnMisPedidos_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/View/Cliente/MisPedidos/Default.aspx?RUT=" + HdnRutCli.Value);
    }
}