using Ecommerce;
using nsCliente;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MPed_Default : Page
{
    string rut;
    DateTime DFrom;
    DateTime DTo;
    double MontoFrom;
    double MontoTo;
    string[] OrdersType;
    string[] OrdersStatus;
    DatosCliente Cliente;
    int CantItems;
    PedidoEcom.OrderFiltering Filtrado;

    protected void Page_Load(object sender, EventArgs e)
    {
        rut = Request.QueryString["RUT"];
        DateTime.TryParse(Request.QueryString["DFrom"], out DFrom);
        DateTime.TryParse(Request.QueryString["DTo"], out DTo);

        



        if (!string.IsNullOrEmpty(rut))
        {
            Random Rnd = new Random();
            if (!IsPostBack)
            {
                CantItems = 10;
                DoFiltering();

            }
            else
            {
                CantItems = Convert.ToInt32(DDlTopitems.SelectedValue);
            }
            
            string Fondo = Rnd.Next(1,11).ToString() + ".png";
            contenedor.Attributes.Add("style", "background: url(" + Page.ResolveUrl("/Images/Fondos/random/" + Fondo)  + ") no-repeat center center fixed;background-size: cover;");
            FillInfoCabecera();
            
                
            
        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, " Error al intentar entrar en la página."));
        }


            
       

    }

    protected void FillInfoCabecera()
    {
        Cliente = new DatosCliente(rut);
        LblEmpresa.Text = Cliente.Nombre;
        
    }

    protected void FillTabla(List<PedidoEcom> Items)
    {
        HtmlGenericControl table;

        table = new HtmlGenericControl("table")
        {
            ID = "OrdersTable",

        };
        table.Attributes.Add("class", "table card-border-lila");

        /*Header*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        trhead.Attributes.Add("class","CsCabecera");
        HtmlGenericControl th;
        HtmlGenericControl tr;
        HtmlGenericControl td;

        /*Nombre*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Pedido"
        };
        trhead.Controls.Add(th);

        /*Número*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Nro de Pedido"
        };
        trhead.Controls.Add(th);

        /*Tipo*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Tipo"
        };
        trhead.Controls.Add(th);

        /*Cantidad*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Cantidad"
        };
        trhead.Controls.Add(th);

        /*Total*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Total"
        };
        th.Attributes.Add("style", "min-width:150px;");
        trhead.Controls.Add(th);

        /*Ingresado por*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Ingresado por"
        };
        trhead.Controls.Add(th);

        /*Fecha de Entrega*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Fecha de entrega"
        };
        trhead.Controls.Add(th);

        /*Estado del pedido*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Estado"
        };
        trhead.Controls.Add(th);

        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        strong.Controls.Add(thead);

        //agregar la cabecera a la tabla
        table.Controls.Add(strong);

        var items= Items.OrderByDescending(iterator => iterator.F_Ingreso).Take(CantItems);

        foreach (PedidoEcom item in items)
        {
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "pedido");
            /*Nombre*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Nombre };
            tr.Controls.Add(td);

            /*Nombre*/
            td = new HtmlGenericControl("td") { InnerHtml = item.NroPedido };
            tr.Controls.Add(td);

            /*Tipo*/
            string Tipo;
            if (item.OrderType=="TER")
            {
                Tipo = "Termopanel";
            }
            else
            {
                Tipo = "";
            }
            td = new HtmlGenericControl("td") { InnerHtml = Tipo};
            td.Attributes.Add("style", "vertical-align: middle;");
            tr.Controls.Add(td);

            
            /*Cantidad*/
            td = new HtmlGenericControl("td") { InnerHtml = item.CanTotal.ToString() };
            td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
            tr.Controls.Add(td);

            /*Total*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Bruto.ToString("C0",CultureInfo.CurrentCulture) };
            td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
            tr.Controls.Add(td);

            GlasserUser.GetInfo GGI = new GlasserUser.GetInfo(item.UserId);
            string Usuario;
            if (GGI.IsSuccess)
            {
                Usuario = GGI.Datos.Nombre + " " + GGI.Datos.Apellido;
            }
            else
            {
                Usuario = item.UserId;
            }
            /*Usuario*/

            td = new HtmlGenericControl("td") { InnerHtml =  Usuario};
            tr.Controls.Add(td);

            string FechaE;
            if (item.F_Entrega<DateTime.Today.AddDays(-10000))
            {
                FechaE = "Sin Fecha";
            }
            else
            {
                FechaE = item.F_Entrega.ToShortDateString();
            }
            /*Fecha Entrega*/
            td = new HtmlGenericControl("td") { InnerHtml = FechaE };
            td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
            tr.Controls.Add(td);

            /*Estado*/
            td = new HtmlGenericControl("td");
            
            string Estado;
            HtmlGenericControl span = new HtmlGenericControl("span");
            HtmlGenericControl i = new HtmlGenericControl("i");
            if (item.Estado=="BOR")
            {
                Estado = " Borrador";
                i.Attributes.Add("class","fas fa-eraser");
                td.Attributes.Add("class", "text-mangotango");
            }
            else if (item.Estado=="DES")
            {
                Estado = " Entregado";
                i.Attributes.Add("class", "fas fa-thumbs-up");
                td.Attributes.Add("class", "text-success");
            }
            else if (item.Estado == "ING")
            {
                Estado = " Ingresado";
                i.Attributes.Add("class", "fas fa-clipboard-check");
                td.Attributes.Add("class", "text-primary");
            }
            else if (item.Estado == "PRG")
            {
                Estado = " En Fabricación";
                i.Attributes.Add("class", "fas fa-cogs");
                td.Attributes.Add("class", "text-lila");
            }
            else if (item.Estado == "DIS")
            {
                Estado = " Bodega";
                i.Attributes.Add("class", "fas fa-warehouse");
                td.Attributes.Add("class", "text-warning");
            }
            else
            {
                Estado = "";
            }
            
            span.InnerHtml = Estado;
            td.Controls.Add(i);
            td.Controls.Add(span);
            td.Attributes.Add("id",item.Estado);
            tr.Controls.Add(td);

            td = new HtmlGenericControl("td") { InnerHtml = item.ID };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            string ItemDetalle = "";
            if (item.OrderType=="TER")
            {
                ItemDetalle = "<p>Tipo de Orden: Termopanel.</p>";
            }
            else if (item.OrderType=="TEM")
            {
                ItemDetalle = "<p>Tipo de Orden: Templados.</p>";
            }

            if (!string.IsNullOrEmpty(item.NroPedido))
            {
                ItemDetalle += "<p>Números de pedidos: " + item.NroPedido + "</p>";
            }

            if (!string.IsNullOrWhiteSpace(item.Observa))
            {
                ItemDetalle += "<p>Observaciones: " + item.Observa + "</p>";
            }

            if (item.EsDespacho)
            {
                ItemDetalle += "<p>Despacho programado para el " + item.F_Entrega.ToLongDateString() + " en " + item.DirEntrega + "</p>";
            }
            else
            {
                ItemDetalle += "<p>Retiro programado para el " + item.F_Entrega.ToLongDateString() + "</p>";
            }

            ItemDetalle += "<div class='d-flex flex-row'>" +
                "<div class='p-2'>Neto : </div><div class='p-2'>" + item.Neto.ToString("C0", CultureInfo.CurrentCulture) + "</div>" +
                "</div>" +
                "<div class='d-flex flex-row'>" +
                "<div class='p-2'>IVA : </div><div class='p-2'>" + item.Iva.ToString("C0", CultureInfo.CurrentCulture) + "</div>" +
                "</div>" +
                "<div class='d-flex flex-row'>" +
                "<div class='p-2'>Total : </div><div class='p-2'>" + item.Bruto.ToString("C0", CultureInfo.CurrentCulture) + "</div>" +
                "</div>";
                

            td = new HtmlGenericControl("td") { InnerHtml = ItemDetalle };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            if (item.FileXLS=="OLD")
            {
                td = new HtmlGenericControl("td") { InnerHtml = "OLD" };
                
            }
            else
            {
                td = new HtmlGenericControl("td") { InnerHtml = "" };
            }
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            td = new HtmlGenericControl("td") { InnerHtml = item.TokenId };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            tbody.Controls.Add(tr);
        }

        table.Controls.Add(tbody);
        DivTPedidos.Controls.Clear();
        DivTPedidos.Controls.Add(table);


    }

    protected void SearchBtn_Click(object sender, EventArgs e)
    {
        PedidoEcom.OrderFiltering orderFiltering = new PedidoEcom.OrderFiltering(rut, txtSearch.Text);

        FillTabla(orderFiltering.Items);
      

    }

    protected void BtnFiltering_Click(object sender, EventArgs e)
    {

        DoFiltering();

    }

    private void DoFiltering()
    {

        GenItemsFiltered();

        FillTabla(Filtrado.Items);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CerrarModal", "CerrarMFilter();", true);



    }

    private void GenItemsFiltered()
    {
        double.TryParse(HdnRangeMin.Value, out MontoFrom);
        if (!string.IsNullOrWhiteSpace(HdnRangeMax.Value))
        {
            double.TryParse(HdnRangeMax.Value, out MontoTo);
        }

        List<ListItem> SelectedTipos = ChkList.Items.Cast<ListItem>().Where(it => it.Selected).Where(it => it.Value != "0").ToList();


        OrdersType = new string[SelectedTipos.Count];
        int cont = 0;
        foreach (var item in SelectedTipos)
        {
            OrdersType[cont] = item.Value;
            cont++;
        }

        List<ListItem> SelectedStatus = ChkListStatus.Items.Cast<ListItem>().Where(it => it.Selected).Where(it => it.Value != "0").ToList();


        OrdersStatus = new string[SelectedStatus.Count];
        cont = 0;
        foreach (var item in SelectedStatus)
        {
            OrdersStatus[cont] = item.Value;
            cont++;
        }

        if (DateTime.TryParse(HdnFilterDateFrom.Value, out DFrom) && DateTime.TryParse(HdnFilterDateTo.Value, out DTo))
        {

            Filtrado = new PedidoEcom.OrderFiltering(rut, DFrom, DTo, MontoFrom, MontoTo, OrdersType, OrdersStatus);
        }
        else if (!DateTime.TryParse(HdnFilterDateFrom.Value, out DFrom) && DateTime.TryParse(HdnFilterDateTo.Value, out DTo))
        {
            Filtrado = new PedidoEcom.OrderFiltering(rut, DTo, MontoFrom, MontoTo, OrdersType, OrdersStatus);
        }
        else if (!DateTime.TryParse(HdnFilterDateFrom.Value, out DFrom) && !DateTime.TryParse(HdnFilterDateTo.Value, out DTo))
        {
            Filtrado = new PedidoEcom.OrderFiltering(rut, MontoFrom, MontoTo, OrdersType, OrdersStatus);
        }
        else
        {
            Filtrado = new PedidoEcom.OrderFiltering(rut, MontoFrom, MontoTo, OrdersType, OrdersStatus);
        }

    }

    protected void MdlItemBtnEdit_Click(object sender, EventArgs e)
    {
        Encriptacion encriptacion = new Encriptacion(HdnTokenId.Value);
        string url = "~/View/Cliente/IngresoPedidos/";
        if (HdnTypeOrder.Value=="Termopanel")
        {
            url += "Termopanel.aspx?RUT=";
        }
        
        Response.Redirect(url+rut + "&ID=" + HdnOrderSelected.Value + "&TOKEN=" + encriptacion.TokenEncriptado);

    }

    protected void MdlItemBtnDelete_Click(object sender, EventArgs e)
    {
        PedidoEcom.Get getP = new PedidoEcom.Get(HdnOrderSelected.Value);
        string Anulacion = " (Pedido Anulado por " + User.Identity.Name + " el " + DateTime.Now.ToLongDateString() + " a las " + DateTime.Now.ToShortTimeString() + " )";
        object[] Param = new object[] { getP.Pedido.Observa + Anulacion,HdnOrderSelected.Value };
        string[] ParamName = new string[] {"OBSERVA","ID" };
        UpdateRow update = new UpdateRow("PLABAL", "UPDATE ECOM_PEDIDOS SET ESTADO='NUL',OBSERVA=@OBSERVA WHERE ID=@ID",Param,ParamName);
        DoFiltering();
        //Response.Redirect("~/View/Cliente/MisPedidos/Default.aspx?RUT=" + rut);
    }

    protected void MdlItemBtnDetails_Click(object sender, EventArgs e)
    {
        Encriptacion encriptacion = new Encriptacion(HdnTokenId.Value);
        string OLD;
        if (HdnOldSelected.Value=="OLD")
        {
            OLD = "True";
        }
        else
        {
            OLD = "False";
        }
        Response.Redirect("~/View/Cliente/MisPedidos/Detalle.aspx?RUT=" + rut + "&ID=" + HdnOrderSelected.Value + "&TOKEN=" + encriptacion.TokenEncriptado + "&OLD=" + OLD);
    }

    protected void DDlTopitems_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDL = sender as DropDownList;
        DDL.SelectedValue = DDlTopitems.SelectedValue;
        DoFiltering();
    }



    protected void BtnDescargaXls_Click(object sender, EventArgs e)
    {
        GenItemsFiltered();
        GlasserExcel.CrearExcel.PedidosEcomm Filtro = new GlasserExcel.CrearExcel.PedidosEcomm(Filtrado.Items,Cliente);
        Response.ClearContent();
        Response.BinaryWrite(Filtro.Excel);
        Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToShortDateString() + "-Informe Pedidos.xlsx");
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Flush();
        Response.End();
    }
}