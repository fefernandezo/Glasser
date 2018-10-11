using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Comercial;
using Comercial.Cotizador;
using ConsumosAPI;
using Microsoft.AspNet.Membership.OpenAuth;

public partial class _Cotizador : System.Web.UI.Page
{
    Cotizacion.GetRowInfo Cot;
    string ID;
    string rut;
    string TOKEN;


  
    

    protected void Page_Load()
    {
        rut = Request.QueryString["RUT"];
        ID = Request.QueryString["ID"];
        TOKEN = Request.QueryString["TOKEN"];

        //UnidadFomento.GetUF getUF = new UnidadFomento.GetUF(DateTime.Now);


        //var ufff = getUF.Lista.UFs.First();
        
        if (ValidarIngreso(rut,ID,TOKEN))
        {
            Page.Title = "Cot N°" + Cot.Correlativo + "-" + Cot.Cliente.Nombre;
            if (!IsPostBack)
            {
               
                Encabezado();
                FormularioModelos.GetFamilias familias = new FormularioModelos.GetFamilias();
                FillDDL(DDlFamilia,"value","key","--Seleccionar--",familias.Datos);
                OTRosCostos(Cot);
                GastosGrales(Cot);
                Margen(Cot);
                Descuento(Cot);
                FillTablaDetalleyTotales(ID, true, Cot.MARGEN);


            }
            
            
            
        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, " la página no fue encontrada."));
        }
        
       
    }
    HtmlGenericControl table;
    HtmlGenericControl tr;

    private void FillTablaDetalleyTotales(string ID, bool Estado, double Margen)
    {
        DetalleCotizacion.Get Det = new DetalleCotizacion.Get(ID, Estado);
            if (Det.HasDetail)
            {
            Updatetotales();
            UpdateDetalle();

            Cot = new Cotizacion.GetRowInfo(ID, TOKEN, Estado);
            DetalleCotizacion.Get Det2 = new DetalleCotizacion.Get(ID, Estado);

            FillTablaDetalle(Det2.Detalle, Margen);
                double Subtotal = Cot.OTROSCOST + Cot.COSTDIR + Cot.COSTMO + Cot.BENECONOM + Cot.GASTOSGEN;
                FillTablaTotales(Cot.OTROSCOST, Cot.GASTOSGEN, Subtotal, Cot.DSCTOPORC);
            }

    }


    private void MargeyDescuento(Cotizacion.GetRowInfo Cot)
    {
        Margen(Cot);
        Descuento(Cot);
    }
    private void Margen(Cotizacion.GetRowInfo Cot)
    {
        /*string espacio = "&nbsp;";*/
        table = new HtmlGenericControl("table") { ID = "TablaMargen" };
        table.Attributes.Add("style", "text-align:center; width:100%;");
        
        /*crear encabezado*/

        HtmlGenericControl h3 = new HtmlGenericControl("H3");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        HtmlGenericControl td;
        

        if (Cot.MARGEN > 0)
        {
            /*txtmargen*/
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filasMargen");
            td = new HtmlGenericControl("td");
            td.Attributes.Add("style", "text-align:center;");
            h3 = new HtmlGenericControl("H3") { InnerHtml = "Editar Margen (" + (Cot.MARGEN * 100).ToString() + "%)" };
            td.Controls.Add(h3);
            tr.Controls.Add(td);
            /*titulo del modal*/
            td = new HtmlGenericControl("td") { InnerHtml = "Editar Margen"};
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*porcentaje*/
            td = new HtmlGenericControl("td") { InnerHtml = Cot.MARGEN.ToString() };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

        }
        else
        {
            /*txtmargen*/
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filasMargen");
            td = new HtmlGenericControl("td") ;
            td.Attributes.Add("style", "text-align:center;");
            h3 = new HtmlGenericControl("H3") { InnerHtml = "Agregar Margen" };
            td.Controls.Add(h3);
            tr.Controls.Add(td);
            /*titulo del modal*/
            td = new HtmlGenericControl("td") { InnerHtml = "Agregar Margen" };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*porcentaje*/
            td = new HtmlGenericControl("td") { InnerHtml = "0,2" };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
        }

        /*agrega al tbody*/
        tbody.Controls.Add(tr);
        table.Controls.Add(tbody);
        DivMargen.Controls.Clear();
        DivMargen.Controls.Add(table);



    }

    private void Descuento(Cotizacion.GetRowInfo Cot)
    {
        /*string espacio = "&nbsp;";*/
        table = new HtmlGenericControl("table") { ID = "TablaDscto" };
        table.Attributes.Add("style", "text-align:center; width:100%;");

        /*crear encabezado*/

        HtmlGenericControl h3 = new HtmlGenericControl("H3");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        HtmlGenericControl td;


        if (Cot.DSCTOPORC > 0)
        {
            /*txtmargen*/
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filasDscto");
            td = new HtmlGenericControl("td");
            td.Attributes.Add("style", "text-align:center;");
            h3 = new HtmlGenericControl("H3") { InnerHtml = "Editar Dscto  (" + (Cot.DSCTOPORC * 100).ToString() + "%)" };
            td.Controls.Add(h3);
            tr.Controls.Add(td);
            /*titulo del modal*/
            td = new HtmlGenericControl("td") { InnerHtml = "Editar Dscto  (" + (Cot.DSCTOPORC * 100).ToString() + "%)" };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*porcentaje*/
            td = new HtmlGenericControl("td") { InnerHtml = Cot.DSCTOPORC.ToString() };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

        }
        else
        {
            /*txtmargen*/
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filasDscto");
            td = new HtmlGenericControl("td");
            td.Attributes.Add("style", "text-align:center;");
            h3 = new HtmlGenericControl("H3") { InnerHtml = "Agregar Descuento" };
            td.Controls.Add(h3);
            tr.Controls.Add(td);
            /*titulo del modal*/
            td = new HtmlGenericControl("td") { InnerHtml = "Agregar Descuento" };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*porcentaje*/
            td = new HtmlGenericControl("td") { InnerHtml = "0,05" };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
        }

        /*agrega al tbody*/
        tbody.Controls.Add(tr);
        table.Controls.Add(tbody);
        DivDscto.Controls.Clear();
        DivDscto.Controls.Add(table);
        



    }

    private void GastosGrales(Cotizacion.GetRowInfo Cot)
    {
        TxtGGral.Text = Cot.GASTOSGEN.ToString();
        if (Cot.GASTOSGEN>0)
        {
            LblGGral.Visible = true;
            PanelGgral.Visible = false;
            LblGGral.Text = "Gastos Generales: " + Cot.GASTOSGEN.ToString("C0", CultureInfo.CurrentCulture);
            LinkBtnGGral.Text = "Editar Gastos Generales";

        }
        else
        {
            PanelGgral.Visible = false;
            LblGGral.Visible = false;
            LblGGral.Text = "";
            LinkBtnGGral.Text = "Agregar Gastos Generales a la cotización";
        }
    }

    private void OTRosCostos(Cotizacion.GetRowInfo Cot)
    {
        TxtOtrosCostos.Text = Cot.OTROSCOST.ToString();
        if (Cot.OTROSCOST > 0)
        {
            LblOtrosCostos.Visible = true;
            PanelOtrosCosto.Visible = false;
            LblOtrosCostos.Text = "Otros Costos: " + Cot.OTROSCOST.ToString("C0", CultureInfo.CurrentCulture);

            LinkBtnOtrosCostos.Text = "Editar Otros Costos";
        }
        else
        {
            PanelOtrosCosto.Visible = false;
            LblOtrosCostos.Visible = false;
            LblOtrosCostos.Text = "";
            LinkBtnOtrosCostos.Text = "Agregar otros costos a la cotización";
        }

    }

    protected void FillDDL(DropDownList DDL, string TextField, string ValueField, string FirstItem, object _Datasource)
    {
        DDL.DataSource = _Datasource;
        DDL.DataTextField = TextField;
        DDL.DataValueField = ValueField;
        DDL.DataBind();
        DDL.Items.Insert(0, new ListItem("--" + FirstItem + "--"));

    }

    protected void Encabezado()
    {
        TituloDoc.Text = "Cot N°" + Cot.Correlativo + "-" + Cot.Cliente.Nombre;
        TxtValidez.Text = Cot.VALIDEZ.ToShortDateString();
        if (string.IsNullOrEmpty(Cot.CLIENTEDOC))
        {
            TxtNombreCli.Enabled = true;
            LinkbtnNombreCli.Text = "Actualizar";

        }
        else
        {
            TxtNombreCli.Enabled = false;
            TxtNombreCli.Text = Cot.CLIENTEDOC;
            LinkbtnNombreCli.Text = "Editar";
        }
        
        
        if (string.IsNullOrEmpty(Cot.OBSERVA))
        {
            
            
            TxtObservacion.Enabled = true;
            LinkbtnObs.Text = "Actualizar";
        }
        else
        {

            TxtObservacion.Text = Cot.OBSERVA;
            TxtObservacion.Enabled = false;
            LinkbtnObs.Text = "Editar";
        }
    }

   

    protected bool ValidarIngreso(string _rut, string _id, string _token)
    {
        bool IsValid = false;

        if (!string.IsNullOrEmpty(_rut) || !string.IsNullOrEmpty(_id) || !string.IsNullOrEmpty(_token))
        {
           

            Cot = new Cotizacion.GetRowInfo(_id, _token, true);
            if (Cot.HasRow)
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }

        }
        else
        {
            IsValid = false;
        }


        return IsValid;
    }

    private void UpdateDetalle()
    {
        SubDetalleCotizacion.UpdateMontos updateMontos = new SubDetalleCotizacion.UpdateMontos(ID, "DISTRIBUCIÓN");
    }

    private void Updatetotales()
    {
        Cotizacion.MontosCotizacion Montos = new Cotizacion.MontosCotizacion(ID);
    }
    
    protected void FillTablaDetalle(List<DetalleCotizacion> _DETALLE, double Margen)
    {
        

        /*string espacio = "&nbsp;";*/
        table = new HtmlGenericControl("table") { ID="TablaDetalle"};
        table.Attributes.Add("class", "table card-border-onix");
        /*crear encabezado*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        trhead.Attributes.Add("class","fondo-mangotango text-white");

        HtmlGenericControl th;

        double totalMO = _DETALLE.Sum(it => it.COSTMOUN);
        bool Ismo = (totalMO > 0) ? true : false;

        double totalDIRECT = _DETALLE.Sum(it => it.COSTDIRUN);
        bool IsDirect = (totalDIRECT > 0) ? true : false;

        
        bool IsUtilidad = (Margen > 0) ? true : false;

        double NETO = _DETALLE.Sum(it => it.NETO);

        
        

        /*item*/
        th = new HtmlGenericControl("th") { InnerHtml = "Ítem" };
        trhead.Controls.Add(th);

        /*Nombre*/
        th = new HtmlGenericControl("th") { InnerHtml = "Nombre" };
        trhead.Controls.Add(th);

        /*Cantidad*/
        th = new HtmlGenericControl("th") { InnerHtml = "Cantidad" };
        trhead.Controls.Add(th);

        if (Ismo)
        {
            th = new HtmlGenericControl("th") { InnerHtml = "Costo MO/Un" };
            trhead.Controls.Add(th);
        }

        if (IsDirect)
        {
            th = new HtmlGenericControl("th") { InnerHtml = "Costo Directo/Un" };
            trhead.Controls.Add(th);
        }

        if (IsUtilidad)
        {
            th = new HtmlGenericControl("th") { InnerHtml = "Utilidad/Un" };
            trhead.Controls.Add(th);
        }


        /*NETO*/
        th = new HtmlGenericControl("th") { InnerHtml = "Neto/Un" };
        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);

        /*TOTAL*/
        th = new HtmlGenericControl("th") { InnerHtml = "Neto" };
        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        /*agrega el thead*/
        table.Controls.Add(thead);


        HtmlGenericControl tr;
        foreach (var item in _DETALLE)
        {
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filas");
            
            HtmlGenericControl td;

            /*IDENDODET*/
            td = new HtmlGenericControl("td") { InnerHtml = item.IDENDODET };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*foto*/
            td = new HtmlGenericControl("td") { InnerHtml = item.IMAGEN };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*n° item*/
            td = new HtmlGenericControl("td") { InnerHtml = item.POS_NR};
            td.Attributes.Add("rel", "popover");
            td.Attributes.Add("data-img", item.IMAGEN);
            tr.Controls.Add(td);

            /*Nombre*/
           

            td = new HtmlGenericControl("td") { InnerHtml = item.NOMBRE };
            td.Attributes.Add("rel", "popover");
            td.Attributes.Add("data-img", item.IMAGEN);
            tr.Controls.Add(td);

            /*Cantidad*/
            td = new HtmlGenericControl("td") { InnerHtml = item.CANTIDAD.ToString() };
            td.Attributes.Add("rel", "popover");
            td.Attributes.Add("data-img", item.IMAGEN);
            tr.Controls.Add(td);

            /*costo mo*/
            if (Ismo)
            {
                double ismo = item.COSTMOUN;
                td = new HtmlGenericControl("td") { InnerHtml = ismo.ToString("C0",CultureInfo.CurrentCulture) };
                
                tr.Controls.Add(td);
            }

            /*costo directo*/
            if (IsDirect)
            {
                double idirect = item.COSTDIRUN;
                td = new HtmlGenericControl("td") { InnerHtml = idirect.ToString("C0",CultureInfo.CurrentCulture) };
                
                tr.Controls.Add(td);
            }
            double UTILIDAD = 0;
            double TOTALCOST = item.COSTMOUN + item.COSTDIRUN;
            /*utilidad*/
            if (IsUtilidad)
            {
                  

                GetMargen uTILIDAD = new GetMargen(Margen, TOTALCOST);
                UTILIDAD = uTILIDAD.BenEcon;
                td = new HtmlGenericControl("td") { InnerHtml = UTILIDAD.ToString("C0",CultureInfo.CurrentCulture) };
                
                tr.Controls.Add(td);
            }

            /*NETO*/
            td = new HtmlGenericControl("td") { InnerHtml = (UTILIDAD + TOTALCOST).ToString("C0", CultureInfo.CurrentCulture) };
            
            tr.Controls.Add(td);

            /*total*/

            td = new HtmlGenericControl("td") { InnerHtml = (item.CANTIDAD*(UTILIDAD+TOTALCOST)).ToString("C0", CultureInfo.CurrentCulture) };
            
            tr.Controls.Add(td);

            

            /*agrega al tbody*/

            tbody.Controls.Add(tr);
        }

        table.Controls.Add(tbody);
        DivListaItem.Controls.Clear();
        DivListaItem.Controls.Add(table);
    }

    protected void FillTablaTotales(double OtrosC, double GGral, double Subtotal, double Dscto)
    {
        
        /*string espacio = "&nbsp;";*/
        table = new HtmlGenericControl("table") { ID = "TablaTotales" };
        table.Attributes.Add("class", "table card-border-naranjo");
        /*crear encabezado*/
        
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        HtmlGenericControl td;


        /*Otros Costos*/
        if (OtrosC>0)
        {
            tr = new HtmlGenericControl("tr");
            
            td = new HtmlGenericControl("td") { InnerHtml = "Otros Costos" };
            td.Attributes.Add("style", "text-align:right;");
            td.Attributes.Add("class", "text-primary");
            tr.Controls.Add(td);
            td = new HtmlGenericControl("td") { InnerHtml = OtrosC.ToString("C0", CultureInfo.CurrentCulture) };
            td.Attributes.Add("class", "text-primary");
            tr.Controls.Add(td);
            /*agrega al tbody*/
            tbody.Controls.Add(tr);
        }
        

        /*Gastos Generales*/
        if (GGral>0)
        {
            tr = new HtmlGenericControl("tr");
            
            td = new HtmlGenericControl("td") { InnerHtml = "Gastos Generales" };
            td.Attributes.Add("style", "text-align:right;");
            td.Attributes.Add("class", "text-primary");
            tr.Controls.Add(td);
            td = new HtmlGenericControl("td") { InnerHtml = GGral.ToString("C0", CultureInfo.CurrentCulture) };
            td.Attributes.Add("class", "text-primary");
            tr.Controls.Add(td);
            /*agrega al tbody*/
            tbody.Controls.Add(tr);
        }
        

        /*Subtotal*/
        tr = new HtmlGenericControl("tr");
        td = new HtmlGenericControl("td") { InnerHtml = "Subtotal" };
        td.Attributes.Add("style", "text-align:right;");
        
        
        tr.Controls.Add(td);
        
        td = new HtmlGenericControl("td") { InnerHtml = Subtotal.ToString("C0", CultureInfo.CurrentCulture) };
        
        tr.Controls.Add(td);
        /*agrega al tbody*/
        tbody.Controls.Add(tr);

        /*Descuento*/
        double DsctoTotal = 0;
        if (Dscto>0)
        {
            tr = new HtmlGenericControl("tr");
            
            td = new HtmlGenericControl("td");
            td.Attributes.Add("class", "text-danger");
            td.Attributes.Add("style", "text-align:right;");
            DsctoTotal = Subtotal * Dscto;
            
            strong = new HtmlGenericControl("strong") { InnerHtml = "Descuento (" + Math.Round(Dscto * 100, 0) + "%)" };
            td.Controls.Add(strong);
            

            tr.Controls.Add(td);
            td = new HtmlGenericControl("td");
            td.Attributes.Add("class", "text-danger");
            strong = new HtmlGenericControl("strong") { InnerHtml =DsctoTotal.ToString("C0", CultureInfo.CurrentCulture) };
            td.Controls.Add(strong);
            tr.Controls.Add(td);
            /*agrega al tbody*/
            tbody.Controls.Add(tr);
        }

        /*Neto Total*/
        tr = new HtmlGenericControl("tr");
        
        td = new HtmlGenericControl("td");
        td.Attributes.Add("style", "text-align:right;");
        strong = new HtmlGenericControl("strong") { InnerHtml = "Neto Total" };
        td.Controls.Add(strong);

        tr.Controls.Add(td);
        double SubT = Subtotal - DsctoTotal;
        td = new HtmlGenericControl("td");
        strong = new HtmlGenericControl("strong") { InnerHtml = SubT.ToString("C0", CultureInfo.CurrentCulture) };
        td.Controls.Add(strong);
        tr.Controls.Add(td);
        /*agrega al tbody*/
        tbody.Controls.Add(tr);

        /*IVA*/
        tr = new HtmlGenericControl("tr");
        
        td = new HtmlGenericControl("td") { InnerHtml = "IVA (19%)" };
        td.Attributes.Add("style", "text-align:right;");
        

        tr.Controls.Add(td);
        double IVA = Math.Round(SubT*0.19,0);
        td = new HtmlGenericControl("td") { InnerHtml = IVA.ToString("C0", CultureInfo.CurrentCulture) };
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
        double Bruto = IVA + SubT;
        td = new HtmlGenericControl("td");
        strong = new HtmlGenericControl("strong") { InnerHtml = Bruto.ToString("C0", CultureInfo.CurrentCulture) };
        td.Controls.Add(strong);
        tr.Controls.Add(td);
        /*agrega al tbody*/
        tbody.Controls.Add(tr);


        table.Controls.Add(tbody);
        DivListtotales.Controls.Clear();
        DivListtotales.Controls.Add(table);




    }

    

    protected void LinkbtnNombreCli_Click(object sender, EventArgs e)
    {
        if (TxtNombreCli.Enabled)
        {
            
            Cotizacion.UpdateRow row = new Cotizacion.UpdateRow(ID, "CLIENTE", TxtNombreCli.Text);
            if (row.Actualizado)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Información actualizada');", true);
                TxtNombreCli.Enabled = false;
                LinkbtnNombreCli.Text = "Editar";
            }

        }
        else
        {
            TxtNombreCli.Enabled = true;
            LinkbtnNombreCli.Text = "Actualizar";
        }
    }

    protected void LinkbtnObs_Click(object sender, EventArgs e)
    {
        if (TxtObservacion.Enabled)
        {

            Cotizacion.UpdateRow row = new Cotizacion.UpdateRow(ID, "OBSERVA", TxtObservacion.Text);
            if (row.Actualizado)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Información actualizada');", true);
                TxtObservacion.Enabled = false;
                LinkbtnObs.Text = "Editar";
            }

        }
        else
        {
            TxtObservacion.Enabled = true;
            LinkbtnObs.Text = "Actualizar";
        }

    }

    protected void LinkValidez_Click(object sender, EventArgs e)
    {
        if (LinkValidez.Text=="Cambiar")
        {
            LinkValidez.Text = "Actualizar";
            TxtValidez.Enabled = true;
        }
        else
        {
            TxtValidez.Enabled = false;
            LinkValidez.Text = "Cambiar";
            //hacer el update de la fecha
            DateTime date;
            if (DateTime.TryParse(TxtValidez.Text, out date))
            {
                Cotizacion.UpdateRow update = new Cotizacion.UpdateRow(ID, "VALIDEZ", date);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Error al actualizar la fecha.');", true);
            }
            
        }
    }

    protected void DDlFamilia_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDlFamilia.SelectedIndex!=0)
        {
            DDLCategoria.Visible = true;
            GetListVariables getList = new GetListVariables();
            getList.GetListModelCategory(DDlFamilia.SelectedValue);
            FillDDL(DDLCategoria,"_Name", "_IDAsign", "--Seleccionar--", getList.ListCompCategory);
            DDLModelos.Visible = false;
        }
        else
        {
            DDLCategoria.Visible = false;
            DDLModelos.Visible = false;
            PanelInfoModel.Visible = false;

        }
    }

    protected void DDLCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLCategoria.SelectedIndex!=0)
        {
            DDLModelos.Visible = true;
            Modelos modelos = new Modelos(Modelos.COT_PTABMODEL.ID_PASIGFAMCAT,DDLCategoria.SelectedValue,true);
            FillDDL(DDLModelos, "NOMBRE", "ID", "--Modelos--", modelos.Lista);
        }
        else
        {
            DDLModelos.Visible = false;
            PanelInfoModel.Visible = false;
        }
    }

    protected void DDLModelos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLModelos.SelectedIndex!=0)
        {
            PanelInfoModel.Visible = true;
            Modelos M = new Modelos(DDLModelos.SelectedValue,true);
            AddImage(M._modelo.IMAGE,M._modelo.NOMBRE);
            HdnIdModel.Value = M._modelo.ID;
            Adddetail(M._modelo);
        }
        else
        {
            PanelInfoModel.Visible = false;
        }
    }

    protected void Adddetail(Modelos._Modelo modelo)
    {
        BomModel bom = new BomModel(modelo.ID, true);
        
        HtmlGenericControl TablaBom = GettablaBom(bom.Lista);
        DivtablaBom.Controls.Clear();
        DivtablaBom.Controls.Add(TablaBom);
        if (!string.IsNullOrWhiteSpace(modelo.DESCRIPCION))
        {
            LblDetalle.Text = modelo.DESCRIPCION;
        }
        else
        {
            LblDetalle.Text = "";
        }

        
    }

    protected HtmlGenericControl GettablaBom(List<BomModel.Bom> boms)
    {
        HtmlGenericControl table = new HtmlGenericControl("table") { ID="Tbom"};
        HtmlGenericControl tr;
        HtmlGenericControl i;
        string espacio = "&nbsp;";
        foreach (var item in boms)
        {
            tr = new HtmlGenericControl("tr");
            HtmlGenericControl td = new HtmlGenericControl("td");
            HtmlGenericControl td0 = new HtmlGenericControl("td");

            tr.Attributes.Add("class", "filas");
            i = new HtmlGenericControl("i");
            
            
            
            if (item.LEVEL == "1")
            {
               
                i.Attributes.Add("class", "fas fa-align-right");
                td.InnerHtml = espacio + item.NOMBRE.ToUpper();
                td.Attributes.Add("class", "font-weight-bold");

            }
            else
            {
                
                
                string opcionaL = "";
                if (item.OPCIONAL)
                {
                    opcionaL = " (Opcional) ";
                }
                
                

                
                i.Attributes.Add("class", "fas fa-arrow-up ml-3");
                td.Attributes.Add("class", "text-lowercase");
                td.InnerHtml = espacio + espacio + espacio + opcionaL + item.NOMBRE ;
            }
            td0.Controls.Add(i);
            tr.Controls.Add(td0);
            tr.Controls.Add(td);
            
            table.Controls.Add(tr);


        }

        return table;

    }

    protected void AddImage(string Pathfoto, string Modelo)
    {
        DivImg.Controls.Clear();
        HtmlGenericControl img = new HtmlGenericControl("img");
        img.Attributes.Add("class","img-thumbnail rounded mx-auto d-block");
        img.Attributes.Add("alt", Modelo);
        img.Attributes.Add("src", Pathfoto);
        DivImg.Controls.Add(img);
    }

    protected void BtnSigAddModel_Click(object sender, EventArgs e)
    {

        string _IDMODELO =  HdnIdModel.Value;
        Modelos M = new Modelos(_IDMODELO, true);
        DetalleCotizacion detalle = new DetalleCotizacion {
            IDENDO = ID,
            IDMODELO = _IDMODELO,
            ESMODELO=true,
            NOMBRE = M._modelo.NOMBRE,
            CANTIDAD=0,
            OBSERVACION = M._modelo.DESCRIPCION,
            NETO=0,
            IVA=0,
            BRUTO=0,
            F_CREACION=DateTime.Now,
            IMAGEN=M._modelo.IMAGE,
            ESTADO=true,
            NETOUN=0,
            IVAUN=0,
            BRUTOUN=0,
            BENECONOMUN=0,
            COSTDIRUN=0,
            COSTMOUN=0,
            
        };
        detalle.GetPOS_NR(ID,true);

        DetalleCotizacion.Firstentry firstentry = new DetalleCotizacion.Firstentry(detalle);
        if (firstentry.IsInserted)
        {
            Response.Redirect("~/View/Distribuidor/Cotizador/Form-Item.aspx?RUT=" + rut + "&NAME=" + detalle.NOMBRE + "&IDENDODET=" + firstentry._IDENDODET + "&IDENDO=" + ID + "&TOKEN=" + TOKEN);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Error al tratar de ingresar el item.');", true);
        }
        
    }

    protected void BtnEditItem_Click(object sender, EventArgs e)
    {

        DetalleCotizacion.GetItem item = new DetalleCotizacion.GetItem(HdnIDENDODET.Value, true);
        

        Response.Redirect("~/View/Distribuidor/Cotizador/Form-Item.aspx?RUT=" + rut + "&NAME=" + item.Item.NOMBRE + "&IDENDODET=" + HdnIDENDODET.Value + "&IDENDO=" + ID +
        "&TOKEN=" + TOKEN);
    }

    protected void BtnEliminarItem_Click(object sender, EventArgs e)
    {
        string IDENDODET = HdnIDENDODET.Value;
        UpdatePLABALRow update;
        update= new UpdatePLABALRow("COT_ENDODET", "IDENDODET", IDENDODET, "ESTADO", false);
        bool UP1 = update.Actualizado;
        update = new UpdatePLABALRow("COT_ENDOSUBDET", "IDENDODET", IDENDODET, "ESTADO", false);
        bool UP2 = update.Actualizado;
        if ( UP1 && UP2)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('el item se ha eliminado.'); window.location='" +
        Page.ResolveUrl("~/View/Distribuidor/Cotizador/Formulario.aspx?RUT=" + rut +"&ID=" + ID +"&TOKEN=" + TOKEN) + "';", true);
        }
    }

    protected void LinkBtnOtrosCostos_Click(object sender, EventArgs e)
    {
        if (PanelOtrosCosto.Visible)
        {
            //hay que hacer el update del costo
            double otros;
            PanelOtrosCosto.Visible = false;
            LinkBtnOtrosCostos.Text = "Editar Otros Costos";

            if (double.TryParse(TxtOtrosCostos.Text, out otros))
            {
                LblOtrosCostos.Visible = true;
                LblOtrosCostos.Text = "Otros Costos: " + otros.ToString("C0", CultureInfo.CurrentCulture);
                Cotizacion.UpdateRow row = new Cotizacion.UpdateRow(ID, "OTROSCOST", otros);
                if (row.Actualizado)
                {
                    Cot = new Cotizacion.GetRowInfo(ID, TOKEN, true);
                    double Subtotal = otros + Cot.COSTDIR + Cot.COSTMO + Cot.BENECONOM + Cot.GASTOSGEN;
                    Updatetotales();
                    FillTablaTotales(otros, Cot.GASTOSGEN, Subtotal, Cot.DSCTOPORC);
                    
                    OTRosCostos(Cot);
                    Descuento(Cot);
                    Margen(Cot);
                }
                else
                {

                }

            }
            else
            {
                //mensaje de error formato de numeros
            }
            
            
        }
        else
        {
            PanelOtrosCosto.Visible = true;
            LinkBtnOtrosCostos.Text = "Actualizar";
            LblOtrosCostos.Visible = false;
        }
    }

    protected void LinkBtnGGral_Click(object sender, EventArgs e)
    {
        if (PanelGgral.Visible)
        {
            double gastos;
            PanelGgral.Visible = false;
            LinkBtnGGral.Text = "Editar gastos generales";
            if (double.TryParse(TxtGGral.Text,out gastos))
            {
                LblGGral.Visible = true;
                LblGGral.Text = "Gastos generales" +  gastos.ToString("C0", CultureInfo.CurrentCulture);
                Cotizacion.UpdateRow row = new Cotizacion.UpdateRow(ID, "GASTOSGEN", gastos);
                if (row.Actualizado)
                {
                    Cot = new Cotizacion.GetRowInfo(ID, TOKEN, true);
                    Updatetotales();
                    double Subtotal = Cot.OTROSCOST + Cot.COSTDIR + Cot.COSTMO + Cot.BENECONOM + gastos;
                    FillTablaTotales(Cot.OTROSCOST, gastos, Subtotal, Cot.DSCTOPORC);
                    
                    GastosGrales(Cot);
                    Descuento(Cot);
                    Margen(Cot);
                }
                else
                {

                }
            }
            else
            {

            }

        }
        else
        {
            PanelGgral.Visible = true;
            LinkBtnGGral.Text = "Actualizar";
            LblGGral.Visible = false;
        }
    }

    protected void BtnMdlDscto_Click(object sender, EventArgs e)
    {
        double Valor;
        string ValStr = HdnVarsliderdscto.Value.Replace(".",",");
        
        if (double.TryParse(ValStr, out Valor))
        {
            Cotizacion.UpdateRow row;
            row = new Cotizacion.UpdateRow(ID, "DSCTOPORC", Valor);
            if (row.Actualizado)
            {
                Cot = new Cotizacion.GetRowInfo(ID, TOKEN, true);
                
                double costos = Cot.COSTMO + Cot.COSTDIR + Cot.OTROSCOST + Cot.GASTOSGEN + Cot.BENECONOM;
                GetDescuento descuento = new GetDescuento(Valor,costos);
                row = new Cotizacion.UpdateRow(ID, "DSCTO", descuento.Descuento);
                Cot.DSCTOPORC = Valor;
                Cot.DSCTO = descuento.Descuento;
                double NetoTotal = descuento.Resultado;
                row = new Cotizacion.UpdateRow(ID, "NETO", NetoTotal);
                Cot.NETO = NetoTotal;
                double IvaTotal = Math.Round(NetoTotal * 0.19, 0);
                row = new Cotizacion.UpdateRow(ID, "IVA", IvaTotal);
                Cot.IVA = IvaTotal;
                double brutoTotal = NetoTotal + IvaTotal;
                row = new Cotizacion.UpdateRow(ID, "BRUTO", brutoTotal);
                Cot.BRUTO = brutoTotal;
                FillTablaDetalleyTotales(ID, true, Cot.MARGEN);
                GastosGrales(Cot);
                Descuento(Cot);
                Margen(Cot);

            }
            else
            {

            }
        }
        else
        {
            
        }

    }

    protected void BtnMdlMargen_Click(object sender, EventArgs e)
    {
        double Valor;
        string ValStr = hdnvarslidermargen.Value.Replace(".", ",");

        if (double.TryParse(ValStr, out Valor))
        {
            Cotizacion.UpdateRow row;
            row = new Cotizacion.UpdateRow(ID, "MARGEN", Valor);
            if (row.Actualizado)
            {

                
                Cot = new Cotizacion.GetRowInfo(ID, TOKEN, true);

                FillTablaDetalleyTotales(ID, true, Cot.MARGEN);
                GastosGrales(Cot);
                Descuento(Cot);
                Margen(Cot);

            }
            else
            {

            }
        }
        else
        {

        }

    }

    protected void BtnGenExcel_Click(object sender, EventArgs e)
    {
        ExcelCotizacion excel = new ExcelCotizacion(ID, TOKEN, true);
        Response.ClearContent();
        Response.BinaryWrite(excel.Excel);
        Response.AddHeader("content-disposition", "attachment; filename=Presupuesto " + Cot.Correlativo + " "+ Cot.CLIENTEDOC + ".xlsx");
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Flush();
        Response.End();
    }
}