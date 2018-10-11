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
    string rut;
    string TOKEN;
    
    HtmlGenericControl table;

    protected void Page_Load(object sender, EventArgs e)
    {
        rut = Request.QueryString["RUT"];

        


        if (!string.IsNullOrEmpty(rut))
        {
            if (!IsPostBack)
            {
                DatosCliente cliente = new DatosCliente(rut);
                titulo.Text = cliente.Nombre;
                Page.Title = "Cotizaciones - " + cliente.Nombre; 
                Cotizacion.GetAll AllCot = new Cotizacion.GetAll(true, rut);
                FillTablaCot(AllCot.Lista);
            }
            
        }
    }


    protected void FillTablaCot(List<Cotizacion.GetRowInfo> Lista)
    {
        table = new HtmlGenericControl("table") { ID = "TablaDetalle" };
        table.Attributes.Add("class", "table card-border-onix");
        /*crear encabezado*/
        
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");

        string[] encabezados = new string[5];
        encabezados[0] = "N° Cotización";
        encabezados[1] = "Cliente";
        encabezados[2] = "Neto";
        encabezados[3] = "Validez";
        encabezados[4] = "";
        HtmlGenericControl thead = Thead(encabezados,"");
        table.Controls.Add(thead);

        //items
        HtmlGenericControl tr;
        foreach (var item in Lista)
        {

            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filas");
            HtmlGenericControl td;

            /*Correlativo*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Correlativo.ToString() };
            tr.Controls.Add(td);

            /*cliente*/
            td = new HtmlGenericControl("td") { InnerHtml = item.CLIENTEDOC };
            tr.Controls.Add(td);

            /*neto*/
            td = new HtmlGenericControl("td") { InnerHtml = item.NETO.ToString("C0",CultureInfo.CurrentCulture) };
            tr.Controls.Add(td);

            /*Validez*/
            td = new HtmlGenericControl("td") { InnerHtml = item.VALIDEZ.ToShortDateString() };
            tr.Controls.Add(td);

            /*Estado*/
            td = new HtmlGenericControl("td");
            HtmlGenericControl span = new HtmlGenericControl("i");
            if (item.ISORDERED)
            {
                span.Attributes.Add("class", "fas fa-cart-arrow-down fa-lg");
                span.Attributes.Add("style", "color:gray;");
                tr.Attributes.Add("title", "Ingresada como pedido");
            }
            else if (item.ACEPTADA)
            {
                span.Attributes.Add("class", "far fa-check-circle fa-lg");
                span.Attributes.Add("style", "color:green;");
                tr.Attributes.Add("title", "Aceptada por el cliente");
            }
            else if (item.ENVIADA)
            {
                span.Attributes.Add("class", "fas fa-share fa-lg");
                span.Attributes.Add("style", "color:#3cb371;");
                tr.Attributes.Add("title", "Cotización enviada al cliente");
            }
            else
            {
                span.Attributes.Add("class", "fas fa-eraser fa-lg");
                span.Attributes.Add("style", "color:blue;");
                tr.Attributes.Add("title", "Borrador");
            }
            td.Controls.Add(span);
            tr.Controls.Add(td);


            /*Id*/
            td = new HtmlGenericControl("td") { InnerHtml = item.ID };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*token*/
            td = new HtmlGenericControl("td") { InnerHtml = item.TOKENID };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);


            tbody.Controls.Add(tr);
        }

        table.Controls.Add(tbody);
        DivTablaCot.Controls.Clear();
        DivTablaCot.Controls.Add(table);
        


    }


    private HtmlGenericControl Thead(string[] Encabezados, string Clase)
    {
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        trhead.Attributes.Add("class", Clase);
        HtmlGenericControl th;

        /*item*/
        foreach (string item in Encabezados)
        {
            th = new HtmlGenericControl("th") { InnerHtml = item };
            trhead.Controls.Add(th);
        }

        thead.Controls.Add(trhead);

        return thead;




    }





    protected void SearchBtn_Click(object sender, EventArgs e)
    {

    }

    protected void BtnVerCot_Click(object sender, EventArgs e)
    {
        Encriptacion Enc = new Encriptacion(HdnTokenId.Value);


        Response.Redirect("~/View/Distribuidor/Cotizador/Formulario.aspx?RUT=" + rut + "&ID=" + HdnIdCot.Value + "&TOKEN=" + Enc.TokenEncriptado);

    }

    protected void BtnEliminarCot_Click(object sender, EventArgs e)
    {

    }
}