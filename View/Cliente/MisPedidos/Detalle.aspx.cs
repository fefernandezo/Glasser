using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ecommerce;
using nsCliente;

public partial class Cliente_MisPedidos_Detalle : Page
{
    string rut;
    string ID;
    string Token;
    string OLD;
    PedidoEcom.Get GetPedido;
    PedidoEcom.GetOld GetOldPedido;
    PedidoEcom Pedido;
    PedidoEcom.GetItems getItems;
    List<PedidoEcom.Detalle> Items;
    HtmlGenericControl table;
    HtmlGenericControl tr;
    HtmlGenericControl td;
    HtmlGenericControl strong;

    protected void Page_Load(object sender, EventArgs e)
    {
        rut = Request.QueryString["RUT"];
        Token = Request.QueryString["TOKEN"];
        ID = Request.QueryString["ID"];
        OLD = Request.QueryString["OLD"];

        if (!string.IsNullOrEmpty(rut) && !string.IsNullOrEmpty(OLD) && !string.IsNullOrEmpty(ID))
        {
            HtmlGenericControl a = new HtmlGenericControl("a") {
                InnerHtml = "Pedidos",
            };
            a.Attributes.Add("href", Page.ResolveUrl("/View/Cliente/MisPedidos/Default.aspx?RUT=") + rut);
            BreadPedido.Controls.Add(a);
            Random Rnd = new Random();
            string Fondo = Rnd.Next(1, 11).ToString() + ".png";
            contenedor.Attributes.Add("style", "background: url(" + Page.ResolveUrl("/Images/Fondos/random/" + Fondo) + ") no-repeat center center fixed;background-size: cover;");
            if (Convert.ToBoolean(OLD))
            {
                GetOldPedido = new PedidoEcom.GetOld(ID, rut);
                Pedido = GetOldPedido.Pedido;
                
                getItems = new PedidoEcom.GetItems(Pedido.NroPedido);
                if (getItems.TieneItems)
                {
                    Items = getItems.Lista;
                    FillTabla();
                    FillEncabezado();
                }
                
                
            }
            else
            {
                GetPedido = new PedidoEcom.Get(ID,Token);
                Pedido = GetPedido.Pedido;
                getItems = new PedidoEcom.GetItems(Pedido.NroPedido.Split(','));
                if (getItems.TieneItems)
                {
                    Items = getItems.Lista;
                    FillTabla();
                    FillEncabezado();
                }
            }
            
            
        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, " Error al intentar acceder a la página."));
        }


    }

    protected void FillEncabezado()
    {
        HtmlGenericControl RowPrin;
        HtmlGenericControl ColPrin1;
        HtmlGenericControl ColPrin2;
        HtmlGenericControl row;
        HtmlGenericControl container;
        HtmlGenericControl col;
        HtmlGenericControl Label;
        container = new HtmlGenericControl("div");
        container.Attributes.Add("class", "container");

        RowPrin = new HtmlGenericControl("div");
        RowPrin.Attributes.Add("class", "row");

        ColPrin1 = new HtmlGenericControl("div");
        ColPrin1.Attributes.Add("class","col-lg-9");

        ColPrin2 = new HtmlGenericControl("div");
        ColPrin2.Attributes.Add("class", "col-lg-3");

        #region Colprin1
        row = new HtmlGenericControl("div");
        row.Attributes.Add("class", "row mt-3");

        col = new HtmlGenericControl("div");
        col.Attributes.Add("class", "col-12");
        Label = new HtmlGenericControl("H3") { InnerHtml = "Nombre: " + Pedido.Nombre };
        col.Controls.Add(Label);
        row.Controls.Add(col);

        if (!string.IsNullOrWhiteSpace(Pedido.Observa))
        {
            col = new HtmlGenericControl("div");
            col.Attributes.Add("class", "col-12");
            Label = new HtmlGenericControl("H4") { InnerHtml = "Observaciones: " + Pedido.Observa };
            col.Controls.Add(Label);
            row.Controls.Add(col);
        }

        if (Pedido.F_Entrega > DateTime.Today.AddDays(-2000))
        {
            col = new HtmlGenericControl("div");
            col.Attributes.Add("class", "col-12 mt-3 mb-2");
            Label = new HtmlGenericControl("H6") { InnerHtml = "Fecha de entrega programada: " + Pedido.F_Entrega.ToShortDateString() };
            col.Controls.Add(Label);
            row.Controls.Add(col);
        }

        ColPrin1.Controls.Add(row);

        #endregion

        #region Colprin2
        row = new HtmlGenericControl("div");
        row.Attributes.Add("class", "row mt-5 mb-2");

        string Doc_Venta = null;
        if (Items.Select(it => it.DocEntrega).Distinct() !=null)
        {
            Doc_Venta = string.Join(", ", Items.Select(it => it.DocEntrega).Distinct());
        }

        string NroPedidos = null;
        if (Items.Select(it => it.NroPedido).Distinct() != null)
        {
             NroPedidos= string.Join(", ", Items.Select(it => it.NroPedido).Distinct());
        }


        if (!string.IsNullOrWhiteSpace(NroPedidos))
        {
            col = new HtmlGenericControl("div");
            col.Attributes.Add("class", "col-12");
            Label = new HtmlGenericControl("H6") { InnerHtml = "N° de Pedidos : " + NroPedidos };
            col.Controls.Add(Label);
            row.Controls.Add(col);
        }
        

        if (!string.IsNullOrWhiteSpace(Doc_Venta))
        {
            col = new HtmlGenericControl("div");
            col.Attributes.Add("class", "col-12");
            Label = new HtmlGenericControl("H6") { InnerHtml = "Doc venta : " + Doc_Venta };
            col.Controls.Add(Label);
            row.Controls.Add(col);
        }
        ColPrin2.Controls.Add(row);

        #endregion


        



        #region Tablatotales
        /*Totales*/
        double Neto = Items.Sum(it => it.Neto);
        double Iva = Math.Round(Neto * 0.19, 0);
        double Total = Neto + Iva;
        table = new HtmlGenericControl("table")
        {
            ID = "TotalesTable",

        };
        table.Attributes.Add("class", "table");

        /*Neto*/
        tr = new HtmlGenericControl("tr");
        tr.Attributes.Add("class", "font-weight-bold text-right");
        td = new HtmlGenericControl("td") { InnerHtml = "NETO" };
        tr.Controls.Add(td);
        td = new HtmlGenericControl("td") { InnerHtml = Neto.ToString("C0", CultureInfo.CurrentCulture) };
        tr.Controls.Add(td);
        table.Controls.Add(tr);

        /*Iva*/
        tr = new HtmlGenericControl("tr");
        tr.Attributes.Add("class", "font-weight-bold text-right");
        td = new HtmlGenericControl("td") { InnerHtml = "IVA" };
        tr.Controls.Add(td);
        td = new HtmlGenericControl("td") { InnerHtml = Iva.ToString("C0", CultureInfo.CurrentCulture) };
        tr.Controls.Add(td);
        table.Controls.Add(tr);

        /*Total*/
        tr = new HtmlGenericControl("tr");
        tr.Attributes.Add("class", "font-weight-bold text-right");
        td = new HtmlGenericControl("td") { InnerHtml = "TOTAL" };
        tr.Controls.Add(td);
        td = new HtmlGenericControl("td") { InnerHtml = Total.ToString("C0", CultureInfo.CurrentCulture) };
        tr.Controls.Add(td);
        table.Controls.Add(tr);

        Divtotales.Controls.Add(table);
        #endregion

        RowPrin.Controls.Add(ColPrin1);
        RowPrin.Controls.Add(ColPrin2);
        container.Controls.Add(RowPrin);
        DivEncabezado.Controls.Add(container);
    }


    protected void FillTabla()
    {
        

        table = new HtmlGenericControl("table")
        {
            ID = "ItemsTable",

        };
        table.Attributes.Add("class", "table card-border-lila");

        /*Header*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        trhead.Attributes.Add("class", "CsCabecera");
        HtmlGenericControl th;
        


        /*Item*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Item"
        };
        trhead.Controls.Add(th);

        /*Nro Pedido*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Nro Pedido"
        };
        trhead.Controls.Add(th);

        /*Referencia*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Referencia"
        };
        trhead.Controls.Add(th);

        /*Codigo de producto*/
        bool Codigo = false;
        if (!string.IsNullOrWhiteSpace(Items.Select(it=>it.CodRandom).First()))
        {
            th = new HtmlGenericControl("th")
            {
                InnerHtml = "Código Producto"
            };
            trhead.Controls.Add(th);
            Codigo = true;
            
        }

        /*Descripción*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Descripción"
        };
        trhead.Controls.Add(th);

        /*Dimensión*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Dimensión"
        };
        trhead.Controls.Add(th);


        /*Cantidad*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Cantidad"
        };
        trhead.Controls.Add(th);

        /*Neto*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Neto"
        };
        trhead.Controls.Add(th);

        /*Estado*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Estado"
        };
        trhead.Controls.Add(th);

        thead.Controls.Add(trhead);
        strong.Controls.Add(thead);

        //agregar la cabecera a la tabla
        table.Controls.Add(strong);

        

        foreach (PedidoEcom.Detalle item in Items)
        {
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "Detalle");
            /*Index*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Index.ToString() };
            tr.Controls.Add(td);

            /*Nro pedido*/
            td = new HtmlGenericControl("td") { InnerHtml = item.NroPedido};
            tr.Controls.Add(td);

            /*Ref*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Referencia };
            tr.Controls.Add(td);

            /*Codigo Random*/
            if (Codigo)
            {
                
                td = new HtmlGenericControl("td") { InnerHtml = item.CodRandom };
                tr.Controls.Add(td);

            }


            /*Descripcion*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Descripcion };
            tr.Controls.Add(td);

            /*Dim*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Ancho.ToString() + " x " + item.Alto.ToString() };
            tr.Controls.Add(td);

            /*Cantidad*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Cantidad.ToString() };
            tr.Controls.Add(td);

            /*Neto*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Neto.ToString("C0", CultureInfo.CurrentCulture) };
            td.Attributes.Add("title", " Neto Un : " + item.NetoUn.ToString("C0",CultureInfo.CurrentCulture));
            tr.Controls.Add(td);

            /*Estado item*/
            td = new HtmlGenericControl("td");
            if (item.Entregado)
            {
                td.InnerHtml = "Entregado";
                td.Attributes.Add("title",item.DocEntrega);
            }
            else if (item.En_Bodega)
            {
                td.InnerHtml = "En Bodega";
            }
            else if (item.Fabricado)
            {
                td.InnerHtml = "Fabricado";
            }
            else
            {
                td.InnerHtml = "Ingresado";
            }
            tr.Controls.Add(td);

            tbody.Controls.Add(tr);
        }

        table.Controls.Add(tbody);
        DivDetalle.Controls.Clear();
        DivDetalle.Controls.Add(table);


    }




}