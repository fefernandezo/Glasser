using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

public partial class _Pedidos : Page
{
    
    FuncUser Usuario = new FuncUser();
    FunDetPed Funcion = new FunDetPed();
    int diascorridos;
    protected void Page_Load(object sender, EventArgs e)
    {
        Infousuario Datosusu = Usuario.DatosUsuario();

        string rutEmpresa = Datosusu.Rutempresa;
        ocultoRut.Value = rutEmpresa;


        if (!IsPostBack)
        {
            textFechHasta.Text = DateTime.Now.ToShortDateString();
            TextFechDesde.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            CalendarFechDesde.SelectedDate = DateTime.Now.AddDays(-30);
            CalendarFechDesde.VisibleDate = DateTime.Now.AddDays(-30);
        }
        PlazoEntrega plazo = Funcion.PlazoEntrega("Termo");
        diascorridos = plazo.DiasCorridos;
    }

    //modal informe PDF
    protected void CalendarFechDesde_SelectionChanged(object sender, EventArgs e)
    {
        string FechDesde = CalendarFechDesde.SelectedDate.ToShortDateString();

        TextFechDesde.Text = DateTime.Parse(FechDesde).ToString("dd/MM/yyyy");
        CalendarFechDesde.Visible = false;
    }

    protected void BtnCalendarFechDesde_Click(object sender, EventArgs e)
    {
        CalendarFechDesde.Visible = true;
        TextFechDesde.Text = "";
    }

    protected void BtnCalendarFechHasta_Click(object sender, EventArgs e)
    {
        CalendarFechHasta.Visible = true;
        textFechHasta.Text = "";
    }



    protected void CalendarFechHasta_SelectionChanged(object sender, EventArgs e)
    {
        string FechHasta = CalendarFechHasta.SelectedDate.ToShortDateString();
        textFechHasta.Text = DateTime.Parse(FechHasta).ToString("dd/MM/yyyy");
        CalendarFechHasta.Visible = false;

    }



    protected void BtnDescagarPDF_Click(object sender, EventArgs e)
    {
        DateTime Desde = Convert.ToDateTime(TextFechDesde.Text);
        DateTime Hasta = Convert.ToDateTime(textFechHasta.Text);
        var ListaEstaods = new List<string>();
        if (Es_todos.Checked)
        {
            ListaEstaods.Add("ING");
            ListaEstaods.Add("PRG");
            ListaEstaods.Add("DIS");
            ListaEstaods.Add("DES");
        }
        else
        {
            if (Es_Disponible.Checked)
            {
                ListaEstaods.Add("DIS");
            }
            if (Es_Entregados.Checked)
            {
                ListaEstaods.Add("DES");
            }
            if (Es_Fabricacion.Checked)
            {
                ListaEstaods.Add("PRG");
            }
            if (Es_Ingresados.Checked)
            {
                ListaEstaods.Add("ING");
            }
        }
        string estados = "'" + string.Join("','", ListaEstaods) + "'";

        Response.Redirect("~/PDFConvert/PDFpedidos.aspx?Desde=" + Desde.ToString() + "&Hasta=" + Hasta.ToString() + "&Estados=" + estados);
    }

    protected void Es_todos_CheckedChanged(object sender, EventArgs e)
    {
        if (Es_todos.Checked)
        {
            Es_Disponible.Checked = true;
            Es_Entregados.Checked = true;
            Es_Fabricacion.Checked = true;
            Es_Ingresados.Checked = true;
        }
        else
        {
            Es_Disponible.Checked = false;
            Es_Entregados.Checked = false;
            Es_Fabricacion.Checked = false;
            Es_Ingresados.Checked = false;
        }
    }

    //modal ingreso pedidos
    protected void LinkPlantilla_Click(object sender, EventArgs e)
    {
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.ContentType = "text/plain";
        response.AddHeader("Content-Disposition", "attachment; filename=Plantilla.xls");
        response.TransmitFile(Server.MapPath("~/Cliente/Diccionario/Plantillas/Plantilla-DetalledePedido.xls"));
        response.Flush();
        response.End();

    }

    string id_temp_pedido;

    //abre el segundo modal de ingreso pedidos y guarda la información en la tabla de pedidos pendientes
    protected void IngPed2Open_Click(object sender, EventArgs e)
    {
        string folio = DateTime.Now.ToLongTimeString().Replace(":", "");
        string path = Path.Combine(Server.MapPath("~/ArchivosAdjuntosPro/ArchivosProductos/"));
        string fileName = folio + fupArchivos.FileName.ToString();
        path = path + fileName;
        fupArchivos.SaveAs(path);

        //encrypta el token
        Random random = new Random();
        string token =random.Next(10000,99999).ToString();
        Encriptacion Palabra = new Encriptacion(token);
        string tokenID = Palabra.Encriptado;




        //inserta una linea en e_temp_pedidos y devuelve el id de la linea
        id_temp_pedido = Funcion.IngPedPaso1(txtNomPedido.Text, txtObservacion.Text, fileName,token);


        Response.Redirect("~/View/Cliente/IngresoPedidos/Detalle-del-Pedido.aspx?ID=" + id_temp_pedido + "&TOKENID="+tokenID);


    }
    
}