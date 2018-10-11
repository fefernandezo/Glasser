using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ecommerce;
using Microsoft.AspNet.Membership.OpenAuth;

public partial class _Ingresar_Pedido_Termo : System.Web.UI.Page
{

    string rut;
    string ID;
    string TOKEN;
    PedidoEcom Pedido;
    PedidoEcom.Get GetPedido;
    /*Obtener datos del cliente*/
    GetInfoClienteEcomm Cliente;
    nsCliente.DatosCliente DatosCliente;
    DistPoliticaChile.GetRegiones getvar;
    DistPoliticaChile.GetComunas getvarcom;



    protected void Page_Load()
    {
        rut = Request.QueryString["RUT"];
        ID = Request.QueryString["ID"];
        TOKEN = Request.QueryString["TOKEN"];

        

        

        if (!string.IsNullOrEmpty(rut) && !string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(TOKEN))
        {
            HtmlGenericControl a = new HtmlGenericControl("a")
            {
                InnerHtml = "Pedidos",
            };
            a.Attributes.Add("href", Page.ResolveUrl("/View/Cliente/MisPedidos/Default.aspx?RUT=") + rut);
            BreadPedido.Controls.Add(a);
            if (!IsPostBack)
            {
                PedidoEcom.UpdateMontos montos = new PedidoEcom.UpdateMontos(ID,TOKEN);
                if (montos.IsSuccess)
                {
                    Pedido = montos.Pedido;
                    Cliente = new GetInfoClienteEcomm(Pedido.RUT);
                    DatosCliente = montos.datosCliente;
                    FillInfoCabecera();
                    FillDetailTable();
                    FillDDLAddItem();
                    if (montos.TieneMensaje)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + montos.Mensaje + "');", true);

                    }
                }
                else
                {
                    Response.Redirect(Error404.Redireccion(MasterPageFile, montos.Mensaje));
                }
                

            }
            else
            {
                PedidoEcom.Get get = new PedidoEcom.Get(ID, TOKEN);
                if (get.IsSuccess)
                {
                    Pedido = get.Pedido;
                    Cliente = get.InfoCliente;
                    DatosCliente = get.Cliente;
                }
                else
                {
                    Response.Redirect(Error404.Redireccion(MasterPageFile, "Error en la Url."));
                }
            }

            



        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, " No tienes acceso a esta página."));
        }

        BtnSendOrder.Enabled = false;

    }


    #region Fill Info

    

    private void FillInfoCabecera()
    {

        TxtOrderName.Text = Pedido.Nombre;
        TxtOrderName.Enabled = false;
        TxtOrderObs.Text = Pedido.Observa;
        TxtOrderObs.Enabled = false;
        LblEmpresa.Text = DatosCliente.Nombre;
        if (Pedido.Neto > 0)
        {
            PnlTotalOrder.Visible = true;
            FillSummaryTable(Pedido);
        }
        else
        {
            PnlTotalOrder.Visible = false;
        }
        if (Pedido.F_Entrega > DateTime.Today)
        {
            BtnAddDeliver.Text = "Editar información de entrega";
            PnlDeliveryInfo.Visible = true;
            HtmlGenericControl label = new HtmlGenericControl("label");
            if (Pedido.EsDespacho)
            {
                getvarcom = new DistPoliticaChile.GetComunas(Pedido.COD_REGION);
                getvar = new DistPoliticaChile.GetRegiones(Pedido.COD_REGION);
                label.InnerHtml = "El despacho está programado para el día " + Pedido.F_Entrega.ToLongDateString() + " en " + Pedido.DirEntrega + ", comuna " + getvarcom.Comunas.Where(it=>it.Codigo==Pedido.COD_COMUNA).Select(it=>it.Nombre).First()
                    + ", " + getvar.Regiones.Select(it => it.Nombre).First();
                    ;
            }
            else
            {
                label.InnerHtml = "El retiro del pedido está programado para el " + Pedido.F_Entrega.ToLongDateString();
            }
            DivDeliveryInfo.Controls.Clear();
            DivDeliveryInfo.Controls.Add(label);

        }
        else
        {
            PnlDeliveryInfo.Visible = false;
            BtnAddDeliver.Text = "Agregar información de entrega";
            DisabledBtnTotermsandCond("El pedido no puede ser enviado, debe agregar información de entrega");
            
        }


       
    }

    private void FillDivTableDeliveryMdl()
    {
        HdnDivTableDeliveryPage.Value = "1";
        CostodelDia.Get get = new CostodelDia.Get(Pedido, 1, true);

        FillDeliveryTable(get.Lista);
        BtnDivTableDeliveryAnt.Visible = false;
        FillModalDeliveryAddress();
        

    }

    private void DisabledBtnTotermsandCond(string Tooltip)
    {
        BtnToTermandCond.Attributes.Add("disabled", "");
        BtnToTermandCond.Attributes.Add("title", Tooltip);
    }

    private void FillDeliveryTable(List<CostodelDia> Lista)
    {

        HtmlGenericControl table;

        table = new HtmlGenericControl("table")
        {
            ID = "DeliveryTable",

        };
        table.Attributes.Add("class", "table card-border-lila");


        /*Header*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        thead.Attributes.Add("class", " bg-opacity-blue-10");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");

        HtmlGenericControl th;
        HtmlGenericControl tr;
        HtmlGenericControl td;
        /*Fecha*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Fecha"
        };
        trhead.Controls.Add(th);
        /*Retiro*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Retiro"
        };
        trhead.Controls.Add(th);
        /*Despacho*/
        th = new HtmlGenericControl("th")
        {
            InnerHtml = "Despacho"
        };
        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        strong.Controls.Add(thead);

        //agregar la cabecera a la tabla
        table.Controls.Add(strong);

        /*Información*/
        int cont = 1;

        
        
        foreach (CostodelDia item in Lista)
        {
            string retiro = "retiro";
            string despacho = "despacho";
            tr = new HtmlGenericControl("tr");
            if (item.IsCorte)
            {
                tr.Attributes.Add("class", "bg-opacity-green-65");
                tr.Attributes.Add("data-toggle", "tootip");
                tr.Attributes.Add("title", "Recomendado");
            }

            if (DateTime.Now.Hour>Constantes.HoraCorte && item.Fecha == GetDiaHabil.Fecha(1))
            {
                retiro = "";
                despacho = "";
                tr.Attributes.Remove("class");
                tr.Attributes.Add("style", "text-decoration-line: line-through;color:grey;");
                tr.Attributes.Add("data-toggle", "tootip");
                tr.Attributes.Add("title", "No se puede entregar para el " + item.Fecha.ToLongDateString());
                
            }


            /*Fecha*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Fecha.ToLongDateString() };
            tr.Controls.Add(td);

            /*Retiro*/
            td = new HtmlGenericControl("td") { InnerHtml = item.PrEntrega.ToString("C0", CultureInfo.CurrentCulture) };
            if (!Pedido.EsDespacho && item.Fecha == Pedido.F_Entrega)
            {
                td.Attributes.Add("class", retiro+" selected");
                td.Attributes.Add("title", "Seleccionado");
            }
            else
            {
                td.Attributes.Add("class", retiro);
            }

            tr.Controls.Add(td);


            /*Despacho*/
            td = new HtmlGenericControl("td") { InnerHtml = item.PrDespacho.ToString("C0", CultureInfo.CurrentCulture) };
            if (Pedido.EsDespacho && item.Fecha == Pedido.F_Entrega)
            {
                td.Attributes.Add("class", despacho+" selected");
                td.Attributes.Add("title", "Seleccionado");
            }
            else
            {
                td.Attributes.Add("class", despacho);
            }

            tr.Controls.Add(td);

            /*Despacho*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Fecha.ToShortDateString() };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
            //agrega fila al tbody
            tbody.Controls.Add(tr);

            cont++;
        }

        table.Controls.Add(tbody);
        DivtableDeliverymodal.Controls.Clear();
        DivtableDeliverymodal.Controls.Add(table);
        

    }

    private void FillModalDeliveryAddress()
    {



        /*LLenar el de regiones*/
        getvar = new DistPoliticaChile.GetRegiones();
        List<string> Reg = new List<string> { "13","05"};
        var Regiones = from c in getvar.Regiones where Reg.Contains(c.Codigo) select c;
        FillDDL(DDLMdlDeliveryReg, "Nombre", "codigo", "--Seleccionar--", Regiones);
        
        DDLMdlDeliveryReg.SelectedValue = DatosCliente.Region;
        DDLMdlDeliveryReg.Enabled = true;

        PnlComuna.Visible = true;

        
        /*LLenar el de regiones*/
        if (Pedido.EsDespacho)
        {
            getvarcom = new DistPoliticaChile.GetComunas(Pedido.COD_REGION);
        }
        else
        {
            getvarcom = new DistPoliticaChile.GetComunas(DatosCliente.Region);
        }
        
        FillDDL(DDLMdlDeliveryCom, "Nombre", "codigo", "--Seleccionar--", getvarcom.Comunas);
        if (Pedido.EsDespacho)
        {
            RowmodalDeliveryAddress.Attributes.Add("class", "visible d-block");

            DDLMdlDeliveryReg.SelectedValue = Pedido.COD_REGION;
            TxtMdlDeliveryDir.Text = Pedido.DirEntrega;
            HdnTypeDeliverySelected.Value = "despacho";
            HdnDateSelected.Value = Pedido.F_Entrega.ToShortDateString();
            DDLMdlDeliveryCom.SelectedValue = Pedido.COD_COMUNA;


        }
        else
        {
            RowmodalDeliveryAddress.Attributes.Add("class", "invisible d-none");
            HdnTypeDeliverySelected.Value = "retiro";
        }

    }

    protected void FillDDL(DropDownList DDL, string TextField, string ValueField, string FirstItem, object _Datasource)
    {
        DDL.DataSource = _Datasource;
        DDL.DataTextField = TextField;
        DDL.DataValueField = ValueField;
        DDL.DataBind();
        DDL.Items.Insert(0, new ListItem(FirstItem));

    }


    private void FillDetailTable()
    {
        HtmlGenericControl table = new HtmlGenericControl("table") { ID = "ItemsTable" };
        table.Attributes.Add("class", "table bg-opacity-white-65");
        /*crear encabezado*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        trhead.Attributes.Add("class", "bg-opacity-white-25");
        HtmlGenericControl th;

        /*item*/
        th = new HtmlGenericControl("th") { InnerHtml = "Ítem" };
        trhead.Controls.Add(th);

        /*REFERENCIA*/
        th = new HtmlGenericControl("th") { InnerHtml = "Referencia" };
        trhead.Controls.Add(th);

        

        /*COMPOSICION*/
        th = new HtmlGenericControl("th") { InnerHtml = "Composición" };
        trhead.Controls.Add(th);

        /*Ancho*/
        th = new HtmlGenericControl("th") { InnerHtml = "Ancho" };
        trhead.Controls.Add(th);

        /*Alto*/
        th = new HtmlGenericControl("th") { InnerHtml = "Alto" };
        trhead.Controls.Add(th);

        /*Cant*/
        th = new HtmlGenericControl("th") { InnerHtml = "Cantidad" };
        
        trhead.Controls.Add(th);

        /*Neto un*/
        th = new HtmlGenericControl("th") { InnerHtml = "Neto/Un" };
        th.Attributes.Add("style", "min-width:100px;");
        trhead.Controls.Add(th);

        /*Neto*/
        th = new HtmlGenericControl("th") { InnerHtml = "Neto" };
        th.Attributes.Add("style","min-width:100px;");
        trhead.Controls.Add(th);

        thead.Controls.Add(trhead);
        /*agrega el thead*/
        table.Controls.Add(thead);



        DivListItems.Controls.Clear();
        PedDet.Get items;
        items = new PedDet.Get(ID, true);


        HtmlGenericControl tr;
        if (items.GotItems)
        {
            /*CheckDiccionario*/
            PedDet.CheckDiccionario checkDiccionario = new PedDet.CheckDiccionario(items.Items, Pedido, Cliente.Margen);

            if (checkDiccionario.HayCambios)
            {
                items = new PedDet.Get(ID, true);
                PedidoEcom.UpdateMontos montos = new PedidoEcom.UpdateMontos(Pedido, true, Cliente);
                FillInfoCabecera();

            }
            PnlItems.Visible = true;
            PnlDelivery.Visible = true;
            foreach (PedDet item in items.Items)
            {
                tr = new HtmlGenericControl("tr");

                HtmlGenericControl td;
                /*Item*/
                td = new HtmlGenericControl("td") { InnerHtml = item.NODO.ToString() };
                td.Attributes.Add("class", "editar");
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                tr.Controls.Add(td);

                /*Ref*/
                td = new HtmlGenericControl("td") { InnerHtml = item.REFERENCIA };
                td.Attributes.Add("class", "editar");
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                tr.Controls.Add(td);

                

                /*Composicion*/
                td = new HtmlGenericControl("td") { InnerHtml = item.DESCRIPCION };
                td.Attributes.Add("class", "editar");
                tr.Controls.Add(td);

                /*Ancho*/
                td = new HtmlGenericControl("td") { InnerHtml = item.ANCHO.ToString() };
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                td.Attributes.Add("class", "editar");
                tr.Controls.Add(td);

                /*Alto*/
                td = new HtmlGenericControl("td") { InnerHtml = item.ALTO.ToString() };
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                td.Attributes.Add("class", "editar");
                tr.Controls.Add(td);

                /*Cant*/
                td = new HtmlGenericControl("td") { InnerHtml = item.CANT.ToString() };
                td.Attributes.Add("class", "editar");
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                tr.Controls.Add(td);

                /*Neto/un*/
                td = new HtmlGenericControl("td") { InnerHtml = item.NETOUN.ToString("C0",CultureInfo.CurrentCulture) };
                td.Attributes.Add("class", "editar");
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                tr.Controls.Add(td);


                /*Neto*/
                td = new HtmlGenericControl("td") { InnerHtml = item.NETOITM.ToString("C0", CultureInfo.CurrentCulture) };
                td.Attributes.Add("class", "editar");
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                tr.Controls.Add(td);


                /*IDPEDDET*/
                td = new HtmlGenericControl("td") { InnerHtml = item.IDPEDDET };
                td.Attributes.Add("style", "display:none;");
                tr.Controls.Add(td);

                if (item.ISFROMFILE && !item.GOTFDICC)
                {
                    td = new HtmlGenericControl("td") { InnerHtml = item.TERMINOLOGIA };
                    td.Attributes.Add("style", "display:none;");
                    tr.Controls.Add(td);

                    tr.Attributes.Add("class", "filas bg-opacity-red-25");
                    DisabledBtnTotermsandCond("El pedido no puede ser enviado, hay ítems en rojo");
                    
                }
                else
                {
                    td = new HtmlGenericControl("td") { InnerHtml = "" };
                    td.Attributes.Add("style", "display:none;");
                    tr.Controls.Add(td);

                    tr.Attributes.Add("class", "filas");

                }
                /*Trash*/
                HtmlGenericControl span = new HtmlGenericControl("span") { InnerHtml = item.IDPEDDET };
                span.Attributes.Add("style", "display:none;");


                HtmlGenericControl trash;
                trash = new HtmlGenericControl("i");
                trash.Attributes.Add("class", "fas fa-trash-alt text-danger eliminar HighLight");
                trash.Controls.Add(span);
                td = new HtmlGenericControl("td");
                td.Attributes.Add("style", "vertical-align: middle; text-align:center;");
                td.Controls.Add(trash);
                tr.Controls.Add(td);



                if (item.NETOUN <= 0 || item.ALTO < 300 || item.ALTO < 300 || item.CANT < 1)
                {
                    DisabledBtnTotermsandCond("El pedido no puede ser enviado, hay items con valores que no corresponden");
                    
                    tr.Attributes.Add("class", "filas bg-opacity-red-25");
                    
                }




                /*agrega al tbody*/

                tbody.Controls.Add(tr);


            }

            table.Controls.Add(tbody);
            DivListItems.Controls.Add(table);

        }
        else
        {
            PnlItems.Visible = false;
            PnlDelivery.Visible = false;
            PnlTotalOrder.Visible = false;
        }


    }

    private void FillDDLAddItem()
    {
        ListasComponentesDVH Listas = new ListasComponentesDVH();
        DDLAddItemCREX.DataSource = Listas.Cristales;
        DDLAddItemCREX.DataTextField = "_Descripcion";
        DDLAddItemCREX.DataValueField = "_CodigoAlfak";
        DDLAddItemCREX.DataBind();
        DDLAddItemCREX.Items.Insert(0, new ListItem("--Seleccionar--"));

        DDLAddItemCRIN.DataSource = Listas.Cristales;
        DDLAddItemCRIN.DataTextField = "_Descripcion";
        DDLAddItemCRIN.DataValueField = "_CodigoAlfak";
        DDLAddItemCRIN.DataBind();
        DDLAddItemCRIN.Items.Insert(0, new ListItem("--Seleccionar--"));

        DDLAddItemSEP.DataSource = Listas.Separadores;
        DDLAddItemSEP.DataTextField = "_Descripcion";
        DDLAddItemSEP.DataValueField = "_CodigoAlfak";
        DDLAddItemSEP.DataBind();
        DDLAddItemSEP.Items.Insert(0, new ListItem("--Seleccionar--"));

    }

    private void FillDDLEditItem()
    {
        ListasComponentesDVH Listas = new ListasComponentesDVH();
        DDLEditCREX.DataSource = Listas.Cristales;
        DDLEditCREX.DataTextField = "_Descripcion";
        DDLEditCREX.DataValueField = "_CodigoAlfak";
        DDLEditCREX.DataBind();
        DDLEditCREX.Items.Insert(0, new ListItem("--Seleccionar--"));

        DDLEditCRIN.DataSource = Listas.Cristales;
        DDLEditCRIN.DataTextField = "_Descripcion";
        DDLEditCRIN.DataValueField = "_CodigoAlfak";
        DDLEditCRIN.DataBind();
        DDLEditCRIN.Items.Insert(0, new ListItem("--Seleccionar--"));

        DDLEditSEP.DataSource = Listas.Separadores;
        DDLEditSEP.DataTextField = "_Descripcion";
        DDLEditSEP.DataValueField = "_CodigoAlfak";
        DDLEditSEP.DataBind();
        DDLEditSEP.Items.Insert(0, new ListItem("--Seleccionar--"));

    }

    private void FillSummaryTable(PedidoEcom Ped)
    {
        HtmlGenericControl table = new HtmlGenericControl("table") { ID = "TablaTotales" };
        table.Attributes.Add("class", "table card-border-naranjo");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        HtmlGenericControl td;
        HtmlGenericControl tr;

        /*Neto Total*/
        tr = new HtmlGenericControl("tr");

        td = new HtmlGenericControl("td");
        td.Attributes.Add("style", "text-align:right;");
        strong = new HtmlGenericControl("strong") { InnerHtml = "Neto Total" };
        td.Controls.Add(strong);

        tr.Controls.Add(td);
        td = new HtmlGenericControl("td");
        strong = new HtmlGenericControl("strong") { InnerHtml = Ped.Neto.ToString("C0", CultureInfo.CurrentCulture) };
        td.Controls.Add(strong);
        tr.Controls.Add(td);
        /*agrega al tbody*/
        tbody.Controls.Add(tr);

        /*IVA*/
        tr = new HtmlGenericControl("tr");

        td = new HtmlGenericControl("td") { InnerHtml = "IVA (19%)" };
        td.Attributes.Add("style", "text-align:right;");


        tr.Controls.Add(td);
        td = new HtmlGenericControl("td") { InnerHtml = Ped.Iva.ToString("C0", CultureInfo.CurrentCulture) };
        tr.Controls.Add(td);
        /*agrega al tbody*/
        tbody.Controls.Add(tr);

        /*TOTAL*/
        tr = new HtmlGenericControl("tr");

        td = new HtmlGenericControl("td");
        td.Attributes.Add("style", "text-align:right;");
        strong = new HtmlGenericControl("strong") { InnerHtml = "TOTAL" };
        td.Controls.Add(strong);

        tr.Controls.Add(td);
        td = new HtmlGenericControl("td");
        strong = new HtmlGenericControl("strong") { InnerHtml = Ped.Bruto.ToString("C0", CultureInfo.CurrentCulture) };
        td.Controls.Add(strong);
        tr.Controls.Add(td);
        /*agrega al tbody*/
        tbody.Controls.Add(tr);
        table.Controls.Add(tbody);
        DivSummary.Controls.Clear();
        DivSummary.Controls.Add(table);



    }

    #endregion

    #region Botones click
    protected void BtnAddItem_Click(object sender, EventArgs e)
    {
        double Alto;
        double Ancho;
        int Cant;
        bool DoUpdate = true;
        if (!double.TryParse(TxtAddItemAlto.Text, out Alto))
        {
            DoUpdate = false;
        }

        if (!double.TryParse(TxtAddItemAncho.Text, out Ancho))
        {
            DoUpdate = false;
        }

        if (!int.TryParse(TxtAddItemCant.Text, out Cant))
        {
            DoUpdate = false;
        }

        if (DoUpdate)
        {
            PedidoEcom.AddItemDVH ItemDvh = new PedidoEcom.AddItemDVH(TxtRefAddItem.Text, new string[] { DDLAddItemCREX.SelectedValue, DDLAddItemSEP.SelectedValue, DDLAddItemCRIN.SelectedValue, "909" }, Cant, Ancho, Alto, Pedido);

            if (ItemDvh.IsSuccess)
            {
                Response.Redirect("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + rut + "&ID=" + ID + "&TOKEN=" + TOKEN);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Error.');", true);
            }
        }




    }

    protected void BtnDeleteItem_Click(object sender, EventArgs e)
    {
        PedDet.DeleteItem delete = new PedDet.DeleteItem(ID, HdnIdItemSelected.Value);
        if (delete.IsDeleted)
        {
            Response.Redirect("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + rut + "&ID=" + ID + "&TOKEN=" + TOKEN);
        }
    }

    protected void BtnEditItem1_Click(object sender, EventArgs e)
    {

        PnlEditFormFooter.Visible = true;
        ChargeEditItem(HdnIdItemSelected.Value);

    }

    protected void BtnEditItemGo_Click(object sender, EventArgs e)
    {
        double Alto;
        double Ancho;
        int Cant;
        bool DoUpdate = true;
        if (!double.TryParse(TxtEditAlto.Text, out Alto))
        {
            DoUpdate = false;
        }

        if (!double.TryParse(TxtEditAncho.Text, out Ancho))
        {
            DoUpdate = false;
        }

        if (!int.TryParse(TxtEditCant.Text, out Cant))
        {
            DoUpdate = false;
        }

        if (DoUpdate)
        {
            object[] Param;
            string[] ParamName;
            Getm2yPerim getm2 = new Getm2yPerim(Ancho, Alto);

            Param = new object[] { TxtEditRef.Text, Cant, Ancho, Alto, HdnIdItemSelected.Value, true, getm2.M2, getm2.M2 * Cant, getm2.Perimetro, getm2.Perimetro * Cant };
            ParamName = new string[] { "REFERENCIA", "CANT", "ANCHO", "ALTO", "IDPEDDET", "ESTADO", "M2UN", "M2ITM", "PERIMUN", "PERIMITM" };
            UpdateRow update = new UpdateRow("PLABAL", "ECOM_PEDDET", "REFERENCIA=@REFERENCIA,CANT=@CANT,ANCHO=@ANCHO,ALTO=@ALTO,M2UN=@M2UN,M2ITM=@M2ITM,PERIMUN=@PERIMUN,PERIMITM=@PERIMITM", "IDPEDDET=@IDPEDDET AND ESTADO=@ESTADO", Param, ParamName);

            if (update.Actualizado)
            {
                PedDet.GetItem getItem = new PedDet.GetItem(ID, HdnIdItemSelected.Value, true);
                if (!string.IsNullOrEmpty(HdnAddTerminologia.Value))
                {
                    string[] Codigos = new string[] { DDLEditCREX.SelectedValue, DDLEditSEP.SelectedValue, DDLEditCRIN.SelectedValue,"909" };
                    if (ChkAddTerminologia.Checked)
                    {
                        /*Agregar al diccionario*/

                        PedDet.AddDiccionario addDiccionario = new PedDet.AddDiccionario(HdnAddTerminologia.Value, Page.User.Identity.Name, rut, Codigos);
                    }

                    /*Ingreso de uno del diccionario*/

                    PedSubDet.InsertSub insertSub = new PedSubDet.InsertSub(getItem.Item, Codigos, Pedido, Cliente.Margen);
                    if (insertSub.Insertados)
                    {
                        UpdatePLABALRow updatePLABALRow = new UpdatePLABALRow("ECOM_PEDDET", "IDPEDDET", HdnIdItemSelected.Value, "GOTFDICC", true);
                    }
                }
                else
                {

                    /*Actualizacion*/

                    PedSubDet Sub;
                    bool IsChanged = false;
                    if (HdnCREXDET.Value != "")
                    {
                        IsChanged = true;
                        PedSubDet.CalcCostos costos = new PedSubDet.CalcCostos(DDLEditCREX.SelectedValue, getItem.Item);
                        Sub = costos.Subdets.First();
                        Param = new object[] { Sub.CNETOUN, Sub.CMERMAUN, Sub.CPROCUN, Sub.KGUN, Sub.ALFAKCODE, Sub.STL_PRODART, Sub.STL_PRODGRP, Sub.STL_WGR, HdnIDSUBDETCREEX.Value, Sub.DESCRIPCION };
                        ParamName = new string[] { "CNETOUN", "CMERMAUN", "CPROCUN", "KGUN", "ALFAKCODE", "STL_PRODART", "STL_PRODGRP", "STL_WGR", "IDPEDSUBDET", "DESCRIPCION" };

                        update = new UpdateRow("PLABAL", "ECOM_PEDSUBDET", "CNETOUN=@CNETOUN,CMERMAUN=@CMERMAUN,CPROCUN=@CPROCUN,KGUN=@KGUN,ALFAKCODE=@ALFAKCODE,STL_PRODART=@STL_PRODART,STL_PRODGRP=@STL_PRODGRP,STL_WGR=@STL_WGR,DESCRIPCION=@DESCRIPCION", "IDPEDSUBDET=@IDPEDSUBDET", Param, ParamName);
                    }

                    if (HdnCRINDET.Value != "")
                    {
                        IsChanged = true;
                        PedSubDet.CalcCostos costos = new PedSubDet.CalcCostos(DDLEditCRIN.SelectedValue, getItem.Item);
                        Sub = costos.Subdets.First();
                        Param = new object[] { Sub.CNETOUN, Sub.CMERMAUN, Sub.CPROCUN, Sub.KGUN, Sub.ALFAKCODE, Sub.STL_PRODART, Sub.STL_PRODGRP, Sub.STL_WGR, HdnIDSUBDETCRIN.Value, Sub.DESCRIPCION };
                        ParamName = new string[] { "CNETOUN", "CMERMAUN", "CPROCUN", "KGUN", "ALFAKCODE", "STL_PRODART", "STL_PRODGRP", "STL_WGR", "IDPEDSUBDET", "DESCRIPCION" };

                        update = new UpdateRow("PLABAL", "ECOM_PEDSUBDET", "CNETOUN=@CNETOUN,CMERMAUN=@CMERMAUN,CPROCUN=@CPROCUN,KGUN=@KGUN,ALFAKCODE=@ALFAKCODE,STL_PRODART=@STL_PRODART,STL_PRODGRP=@STL_PRODGRP,STL_WGR=@STL_WGR,DESCRIPCION=@DESCRIPCION", "IDPEDSUBDET=@IDPEDSUBDET", Param, ParamName);

                    }

                    if (HdnSEPDET.Value != "")
                    {
                        IsChanged = true;
                        PedSubDet.CalcCostos costos = new PedSubDet.CalcCostos(DDLEditSEP.SelectedValue, getItem.Item);
                        Sub = costos.Subdets.First();
                        Param = new object[] { Sub.CNETOUN, Sub.CMERMAUN, Sub.CPROCUN, Sub.KGUN, Sub.ALFAKCODE, Sub.STL_PRODART, Sub.STL_PRODGRP, Sub.STL_WGR, HdnIDSUBDETSEP.Value, Sub.DESCRIPCION };
                        ParamName = new string[] { "CNETOUN", "CMERMAUN", "CPROCUN", "KGUN", "ALFAKCODE", "STL_PRODART", "STL_PRODGRP", "STL_WGR", "IDPEDSUBDET", "DESCRIPCION" };

                        update = new UpdateRow("PLABAL", "ECOM_PEDSUBDET", "CNETOUN=@CNETOUN,CMERMAUN=@CMERMAUN,CPROCUN=@CPROCUN,KGUN=@KGUN,ALFAKCODE=@ALFAKCODE,STL_PRODART=@STL_PRODART,STL_PRODGRP=@STL_PRODGRP,STL_WGR=@STL_WGR,DESCRIPCION=@DESCRIPCION", "IDPEDSUBDET=@IDPEDSUBDET", Param, ParamName);
                    }

                    if (IsChanged)
                    {
                        Param = new object[] { HdnIdItemSelected.Value, ID, true };

                        ParamName = new string[] { "IDPEDDET", "IDPEDIDO", "ESTADO" };

                        SelectRows select = new SelectRows("PLABAL", "ECOM_PEDSUBDET", "DESCRIPCION", "IDPEDDET=@IDPEDDET AND IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO", Param, ParamName);
                        string[] Descrip = new string[select.Datos.Rows.Count];

                        for (int i = 0; i < 3; i++)
                        {
                            Descrip[i] = select.Datos.Rows[i][0].ToString();
                        }


                        UpdatePLABALRow updateP = new UpdatePLABALRow("ECOM_PEDDET", "IDPEDDET", HdnIdItemSelected.Value, "DESCRIPCION", string.Join(" || ", Descrip));


                    }

                    PedDet.Calculos calculos = new PedDet.Calculos(getItem.Item, Pedido, Cliente.Margen);
                }










                Response.Redirect("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + rut + "&ID=" + ID + "&TOKEN=" + TOKEN);



            }



        }

    }

    protected void BtnCargaMasiva_Click(object sender, EventArgs e)
    {
        string folio = DateTime.Now.ToLongTimeString().Replace(":", "");
        string path = Path.Combine(Server.MapPath("~/CargaMasiva/"));
        if (!string.IsNullOrEmpty(FileUpXls.FileName.ToString()))
        {
            string Filename = folio + FileUpXls.FileName.ToString();

            path = path + Filename;
            FileUpXls.SaveAs(path);
            object[] Param = new object[] { true, Filename, ID };
            string[] ParamName = new string[] { "HASFILE", "FILEXLS", "ID" };
            UpdateRow update = new UpdateRow("PLABAL", "ECOM_PEDIDOS", " HASFILE=@HASFILE , FILEXLS=@FILEXLS ", "ID=@ID", Param, ParamName);
            if (update.Actualizado)
            {
                LecturaxlsTP lecturaxls = new LecturaxlsTP(Filename, Cliente.TipoXlsDvh);
                if (lecturaxls.Lista.Count > 0)
                {
                    bool[] Ingresados = new bool[200];
                    PedidoEcom.AddItemDVH addItem;
                    int cont = 0;
                    foreach (LecturaxlsTP.Valores valor in lecturaxls.Lista)
                    {
                        addItem = new PedidoEcom.AddItemDVH(true, valor.Terminologia, valor.Referencia, valor.Cantidad, valor.Ancho, valor.Alto, Pedido);
                        Ingresados[cont] = addItem.IsSuccess;
                        cont++;
                    }
                    int x = Ingresados.Where(it => it).Count();
                    string Msg;
                    if (x < lecturaxls.Lista.Count)
                    {
                        Msg = "La lectura fue exitosa, pero el sistema solo pudo identificar en el diccionario " + x + " items. Los items que no han sido identifcado quedaron destacados en rojo.";
                    }
                    else
                    {
                        Msg = "La lectura del archivo fue exitosa.";
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + Msg + "'); window.location='" +
        Page.ResolveUrl("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + rut + "&ID=" + ID + "&TOKEN=" + TOKEN) + "';", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El sistema no pudo reconocer ningún item ingresado en el archivo, por favor intentelo nuevamente.'); window.location='" +
        Page.ResolveUrl("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + rut + "&ID=" + ID + "&TOKEN=" + TOKEN) + "';", true);

                }

            }
        }



    }
    #endregion

    #region Link Botones
    protected void LinkBtnOrderName_Click(object sender, EventArgs e)
    {
        if (LinkBtnOrderName.Text=="Editar")
        {
            LinkBtnOrderName.Text = "Actualizar";
            TxtOrderName.Enabled = true;
        }
        else
        {
            LinkBtnOrderName.Text = "Editar";
            TxtOrderName.Enabled = false;
            UpdatePLABALRow update = new UpdatePLABALRow("ECOM_PEDIDOS", "ID", ID, "NOMBRE", TxtOrderName.Text);
            if (update.Actualizado)
            {
                AlertInfoActualizada();
            }
        }
    }

    private void AlertInfoActualizada()
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('La información ha sido actualizada');", true);
    }

    protected void LinkBtnOrderObs_Click(object sender, EventArgs e)
    {
        if (LinkBtnOrderObs.Text=="Editar")
        {
            LinkBtnOrderObs.Text = "Actualizar";
            TxtOrderObs.Enabled = true;
        }
        else
        {
            LinkBtnOrderObs.Text = "Editar";
            TxtOrderObs.Enabled = false;
            UpdatePLABALRow update = new UpdatePLABALRow("ECOM_PEDIDOS", "ID", ID, "OBSERVA", TxtOrderObs.Text);
            if (update.Actualizado)
            {
                AlertInfoActualizada();
            }
        }
    }

    #endregion


    #region Delivery Info

    protected void BtnDivTableDeliverySig_Click(object sender, EventArgs e)
    {
        int Page;
        if (int.TryParse(HdnDivTableDeliveryPage.Value, out Page))
        {
            Page++;
            HdnDivTableDeliveryPage.Value = (Page).ToString();
            BtnDivTableDeliveryAnt.Visible = true;
        }
        PedDet.Get getitems = new PedDet.Get(Pedido.ID, true);
        var cmp = getitems.Items.Sum(iterator => iterator.CMPUN * iterator.CANT);
        var cpro = getitems.Items.Sum(iterator => iterator.CPROCUN * iterator.CANT);
        var cmer = getitems.Items.Sum(iterator => iterator.CMERMAUN * iterator.CANT);
        double[] KG = new double[getitems.Items.Count];
        int i = 0;
        foreach (var item in getitems.Items)
        {
            KG[i] = item.KGITM;
            i++;
        }

        CostodelDia.Get get = new CostodelDia.Get(Pedido, Page, true);

        FillDeliveryTable(get.Lista);

    }

    protected void BtnDivTableDeliveryAnt_Click(object sender, EventArgs e)
    {
        int Page;
        if (int.TryParse(HdnDivTableDeliveryPage.Value, out Page))
        {

            if (Page > 1)
            {
                Page--;
            }


        }
        if (Page == 1)
        {
            BtnDivTableDeliveryAnt.Visible = false;
        }
        HdnDivTableDeliveryPage.Value = (Page).ToString();
        PedDet.Get getitems = new PedDet.Get(Pedido.ID, true);
        var cmp = getitems.Items.Sum(iterator => iterator.CMPUN * iterator.CANT);
        var cpro = getitems.Items.Sum(iterator => iterator.CPROCUN * iterator.CANT);
        var cmer = getitems.Items.Sum(iterator => iterator.CMERMAUN * iterator.CANT);
        double[] KG = new double[getitems.Items.Count];
        int i = 0;
        foreach (var item in getitems.Items)
        {
            KG[i] = item.KGITM;
            i++;
        }

        CostodelDia.Get get = new CostodelDia.Get(Pedido, Page, true);

        FillDeliveryTable(get.Lista);



    }

    protected void LinkBtnMisdirecciones_Click(object sender, EventArgs e)
    {
        Misdirecciones.Get get = new Misdirecciones.Get(rut, true);
        if (get.IsGot)
        {
            PnlTableMisdir.Visible = true;
            FillTableMisDir(get.Lista);
        }

    }

    protected void FillTableMisDir(List<Misdirecciones> Dirs)
    {

        HtmlGenericControl table = new HtmlGenericControl("table") { ID = "DirTable" };
        /*crear encabezado*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        HtmlGenericControl tr;

        HtmlGenericControl th;

        /*dir*/
        th = new HtmlGenericControl("th") { InnerHtml = "Dirección" };
        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        /*agrega el thead*/
        table.Controls.Add(thead);
        foreach (Misdirecciones item in Dirs)
        {
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "selDir");
            HtmlGenericControl td;
            /*Item*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Direccion + ", " + item.Comuna + ", " + item.Region };
            tr.Controls.Add(td);

            /*Dirección*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Direccion };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
            /*Comuna*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Cod_com };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
            /*region*/
            td = new HtmlGenericControl("td") { InnerHtml = item.Cod_Reg };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
            /*agrega al tbody*/

            tbody.Controls.Add(tr);
        }
        table.Controls.Add(tbody);
        DivTableMisDir.Controls.Clear();
        DivTableMisDir.Controls.Add(table);
    }

    protected void DDLMdlDeliveryReg_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (DDLMdlDeliveryReg.SelectedIndex != 0)
        {
            PnlComuna.Visible = true;

            DistPoliticaChile.GetComunas getvar;
            /*LLenar el de regiones*/
            getvar = new DistPoliticaChile.GetComunas(DDLMdlDeliveryReg.SelectedValue);
            FillDDL(DDLMdlDeliveryCom, "Nombre", "codigo", "--Seleccionar--", getvar.Comunas);

        }
        else
        {
            PnlComuna.Visible = false;

        }
    }

    protected void BtnAddDeliveryMethod_Click(object sender, EventArgs e)
    {
        UpdateRow update;
        bool IsDelivewry;
        string Dir;
        string CCom;
        string CReg;
        if (HdnTypeDeliverySelected.Value == "despacho")
        {
            IsDelivewry = true;
            Dir = TxtMdlDeliveryDir.Text;
            CCom = DDLMdlDeliveryCom.SelectedValue;
            CReg = DDLMdlDeliveryReg.SelectedValue;
        }
        else if (HdnTypeDeliverySelected.Value == "retiro")
        {
            IsDelivewry = false;
            Dir = " ";
            CCom = " ";
            CReg = " ";
        }
        else
        {
            IsDelivewry = false;
            Dir = " ";
            CCom = " ";
            CReg = " ";
        }

        DateTime fecha;
        if (DateTime.TryParse(HdnDateSelected.Value, out fecha))
        {
            object[] Param = new object[] { IsDelivewry, Pedido.ID, Dir, CCom, CReg, fecha };
            string[] ParamName = new string[] { "ESDESPACHO", "IDPEDIDO", "DIRENTREGA", "COD_COMUNA", "COD_REGION", "F_ENTREGA" };
            update = new UpdateRow("PLABAL", "ECOM_PEDIDOS",
                "ESDESPACHO=@ESDESPACHO,DIRENTREGA=@DIRENTREGA,COD_COMUNA=@COD_COMUNA,COD_REGION=@COD_REGION,F_ENTREGA=@F_ENTREGA",
                "ID=@IDPEDIDO", Param, ParamName);

            if (update.Actualizado)
            {
                PedidoEcom.UpdateMontos updateMontos = new PedidoEcom.UpdateMontos(Pedido, true, Cliente);
                if (updateMontos.IsSuccess)
                {
                    Response.Redirect("~/View/Cliente/IngresoPedidos/Termopanel.aspx?RUT=" + rut + "&ID=" + ID + "&TOKEN=" + TOKEN);
                }
            }

        }
        else
        {
            IsDelivewry = false;
        }

    }
    #endregion




    private void ChargeEditItem(string IDPEDDET)
    {
        FillDDLEditItem();
        PedDet.GetItem getItem = new PedDet.GetItem(ID, IDPEDDET, true);
        if (getItem.IsGot)
        {
            object[] Param = new object[] { ID,IDPEDDET,true};
            string[] ParamName = new string[] { "IDPEDIDO","IDPEDDET","ESTADO" };
            SelectRows select = new SelectRows("PLABAL", "ECOM_PEDSUBDET", "IDPEDSUBDET,ALFAKCODE", "IDPEDDET=@IDPEDDET AND IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO",Param,ParamName);
            TxtEditRef.Text = getItem.Item.REFERENCIA;
            TxtEditAlto.Text = getItem.Item.ALTO.ToString();
            TxtEditAncho.Text = getItem.Item.ANCHO.ToString();
            TxtEditCant.Text = getItem.Item.CANT.ToString();

            if (select.IsGot)
            {
                PnlAddDicc.Visible = false;

                DataRow dr;
                dr = select.Datos.Rows[0];
                DDLEditCREX.SelectedValue = dr[1].ToString();
                HdnCREXDET.Value= dr[1].ToString();
                HdnIDSUBDETCREEX.Value = dr[0].ToString();

                dr = select.Datos.Rows[1];
                DDLEditSEP.SelectedValue = dr[1].ToString();
               HdnSEPDET.Value= dr[1].ToString();
                HdnIDSUBDETSEP.Value= dr[0].ToString();

                dr = select.Datos.Rows[2];
                DDLEditCRIN.SelectedValue = dr[1].ToString();
                HdnCRINDET.Value= dr[1].ToString();
                HdnIDSUBDETCRIN.Value= dr[0].ToString();

            }
            else
            {
                PnlAddDicc.Visible = true;
                ChkAddTerminologia.Text = "&nbsp;&nbsp;&nbsp;Agregar al diccionario el término \"" + getItem.Item.TERMINOLOGIA + "\" al Diccionario.";
                ChkAddTerminologia.Checked = true;
            }
        }
    }


    protected void DDLMdlDeliveryCom_SelectedIndexChanged(object sender, EventArgs e)
    {
        CostodelDia.Get get = new CostodelDia.Get(Pedido, 1, true,DDLMdlDeliveryReg.SelectedValue,DDLMdlDeliveryCom.SelectedValue);
        FillDeliveryTable(get.Lista);
    }

    protected void BtnSendOrder_Click(object sender, EventArgs e)
    {
        
        EnviarPedidoDVH enviarPedidoDVH = new EnviarPedidoDVH(Pedido,Cliente);
        string PreAlert;
        if (enviarPedidoDVH.NroPedido.Count()>1)
        {
            PreAlert = "El pedido ha sido ingresado con los siguientes números: " + string.Join(", ", enviarPedidoDVH.NroPedido);
        }
        else
        {
            PreAlert = "El pedido ha sido ingresado con el siguiente número: " + string.Join(", ", enviarPedidoDVH.NroPedido);
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('"+PreAlert+"');", true);
    }

    protected void BtnAddDeliver_Click(object sender, EventArgs e)
    {
        FillDivTableDeliveryMdl();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenDeliveryModal", "OpenDeliveryModal();", true);

    }

   
}