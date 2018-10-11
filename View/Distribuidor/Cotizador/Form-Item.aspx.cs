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

public partial class Dis_Form_Item : Page
{
    string IDENDODET;
    string NOMBRE;
    string IDENDO;
    string TOKEN;
    Cotizacion.GetRowInfo Cot;

    protected void Page_Load(object sender, EventArgs e)
    {
        IDENDODET = Request.QueryString["IDENDODET"];
        IDENDO = Request.QueryString["IDENDO"];
        TOKEN = Request.QueryString["TOKEN"];
        NOMBRE = Request.QueryString["NAME"];

        if (ValidarIngreso())
        {
            Page.Title = "Cot N°" + Cot.Correlativo + "-" + Cot.Cliente.Nombre;
            if (!IsPostBack)
            {
                



            }

        }
        else
        {
            Response.Redirect(Error404.Redireccion(MasterPageFile, " la página no fue encontrada."));
        }




    }

    protected bool ValidarIngreso()
    {
        bool IsValid = false;

        if (!string.IsNullOrEmpty(IDENDODET) || !string.IsNullOrEmpty(IDENDO) || !string.IsNullOrEmpty(TOKEN))
        {
            Cot = new Cotizacion.GetRowInfo(IDENDO, TOKEN, true);
            if (Cot.HasRow)
            {
                DetalleCotizacion.GetItem ITEM = new DetalleCotizacion.GetItem(IDENDODET,true);
                if (ITEM.IsGetting)
                {
                    if (!IsPostBack)
                    {
                        FillInformation(ITEM.Item);
                        


                    }
                    SubDetalleCotizacion.Get get = new SubDetalleCotizacion.Get(1, IDENDO, IDENDODET, true);
                    if (get.HasDetail)
                    {

                        FilltablaPiezas(get.SubDetalle);
                    }
                    

                    IsValid = true;
                    
                }
                
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


    protected void FillInformation(DetalleCotizacion DetCot)
    {
        AddImage(DetCot.IMAGEN,DetCot.NOMBRE);
        TxtNombreProd.Enabled = false;
        TxtDescripProd.Enabled = false;
        TxtNombreProd.Text = DetCot.NOMBRE;
        TxtDescripProd.Text = DetCot.OBSERVACION;
        TxtCantidad.Text = DetCot.CANTIDAD.ToString();
        if (DetCot.CANTIDAD==0)
        {
            TxtCantidad.Enabled = true;
            
            LinkBtnCantidad.Text = "Actualizar";
        }
        else
        {
            TxtCantidad.Enabled = false;
            LinkBtnCantidad.Text = "Editar";
        }

        if (DetCot.COSTMOUN==0)
        {
            LinkBtnCostoMO.Text = "Agregar Mano de Obra de instalación.";
        }
        else
        {
            TxtCostoMO.Text = DetCot.COSTMOUN.ToString();
            LblCostoMO.Text = "Costo Instalación:" + DetCot.COSTMOUN.ToString("C0", CultureInfo.CurrentCulture) + " por unidad.";
            LblCostoMO.Visible = true;
            LinkBtnCostoMO.Text = "Editar Costo de Instalación";
        }
        
    }


    protected void AddImage(string Pathfoto, string NombreModelo)
    {
        ItemImg.Controls.Clear();
        HtmlGenericControl img = new HtmlGenericControl("img");
        img.Attributes.Add("class", "img-thumbnail rounded mx-auto d-block");
        img.Attributes.Add("alt", NombreModelo);
        img.Attributes.Add("src", Pathfoto);
        ItemImg.Controls.Add(img);
    }

    HtmlGenericControl table;
    HtmlGenericControl tr;

    protected void FilltablaPiezas(List<SubDetalleCotizacion> sub)
    {
        /*string espacio = "&nbsp;";*/
        table = new HtmlGenericControl("table") { ID = "TablaPiezas" };
        table.Attributes.Add("class", "table card-border-onix");
        /*crear encabezado*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        trhead.Attributes.Add("class", "fondo-uva text-white");

        HtmlGenericControl th;

        /*item*/
        th = new HtmlGenericControl("th") { InnerHtml = "Ítem" };
        trhead.Controls.Add(th);

        /*Nombre*/
        th = new HtmlGenericControl("th") { InnerHtml = "Nombre" };
        trhead.Controls.Add(th);

        /*Ancho*/
        th = new HtmlGenericControl("th") { InnerHtml = "Ancho" };
        trhead.Controls.Add(th);

        /*Alto*/
        th = new HtmlGenericControl("th") { InnerHtml = "Alto" };
        trhead.Controls.Add(th);

        /*Neto*/
        th = new HtmlGenericControl("th") { InnerHtml = "Neto" };
        trhead.Controls.Add(th);

        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        /*agrega el thead*/
        table.Controls.Add(thead);

        foreach (var item in sub)
        {
            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filas");
            HtmlGenericControl td;

            /*n° item*/
            td = new HtmlGenericControl("td") { InnerHtml = item.POS_NR };
            tr.Controls.Add(td);

            /*Nombre*/
            td = new HtmlGenericControl("td") { InnerHtml = item.NOMBRE };
            tr.Controls.Add(td);

            /*Ancho*/
            td = new HtmlGenericControl("td") { InnerHtml = item.ANCHO.ToString() };
            tr.Controls.Add(td);

            /*Alto*/
            td = new HtmlGenericControl("td") { InnerHtml = item.ALTO.ToString() };
            tr.Controls.Add(td);

            /*Neto*/
            td = new HtmlGenericControl("td") { InnerHtml = item.NETO.ToString("C0",CultureInfo.CurrentCulture) };
            tr.Controls.Add(td);

            /*IDSUBDET*/
            td = new HtmlGenericControl("td") { InnerHtml = item.IDSUBDET };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);
            
            /*IDMODELBOM*/
            td = new HtmlGenericControl("td") { InnerHtml = item.IDMODELBOM };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*agrega al tbody*/

            tbody.Controls.Add(tr);

        }

        table.Controls.Add(tbody);

        DivListaPiezas.Controls.Clear();
        DivListaPiezas.Controls.Add(table);
    }


    protected void LinkbtnNombreProd_Click(object sender, EventArgs e)
    {
        if (TxtNombreProd.Enabled)
        {
            Update("COT_ENDODET", "IDENDODET", IDENDODET, "NOMBRE", TxtNombreProd.Text,true);
            
            TxtNombreProd.Enabled = false;
            LinkbtnNombreProd.Text = "Editar";
        }
        else
        {
            TxtNombreProd.Enabled = true;
            LinkbtnNombreProd.Text = "Actualizar";
        }

    }

    protected void Update(string _tabla, string _rowIdname, object ID, string _Campo, object Valor, bool alert)
    {
        UpdatePLABALRow update = new UpdatePLABALRow(_tabla, _rowIdname, ID, _Campo, Valor);
        if (update.Actualizado)
        {
            if (alert)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Información actualizada');", true);
            }
            

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Error al tratar de actualizar la información.');", true);

        }

    }



    protected void LinkBtnDescripProd_Click(object sender, EventArgs e)
    {
        if (TxtDescripProd.Enabled)
        {
            Update("COT_ENDODET", "IDENDODET", IDENDODET, "OBSERVACION", TxtDescripProd.Text,true);
            
            TxtDescripProd.Enabled = false;
            LinkBtnDescripProd.Text = "Editar";
        }
        else
        {
            TxtDescripProd.Enabled = true;
            LinkBtnDescripProd.Text = "Actualizar";
        }

    }

    protected void UpdateVariablesPieza_Click(object sender, EventArgs e)
    {
        bool s1 = false;
        bool s2 = false;
        double aNCHO;
        if (double.TryParse(TxtAnchoPiezaSelected.Text,out aNCHO))
        {
            Update("COT_ENDOSUBDET", "IDSUBDET", HdnIDSUBDET.Value, "ANCHO", aNCHO,false);
            s1 = true;
        }

        double aLTO;
        if (double.TryParse(TxtAltoPiezaSelected.Text,out aLTO))
        {
            Update("COT_ENDOSUBDET", "IDSUBDET", HdnIDSUBDET.Value, "ALTO", aLTO,false);
            s2 = true;
        }

        if (s1 && s2)
        {
            SubDetalleCotizacion.GetItem item = new SubDetalleCotizacion.GetItem(HdnIDSUBDET.Value, true);
            SubDetalleCotizacion.Get Subdet = new SubDetalleCotizacion.Get(item.Datos.NODE, IDENDO, IDENDODET, true);
            SubDetalleCotizacion.CalcularPieza Calculos = new SubDetalleCotizacion.CalcularPieza(Subdet.SubDetalle,aNCHO,aLTO, "DISTRIBUCIÓN");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Información actualizada.'); window.location='" +
        Page.ResolveUrl("~/View/Distribuidor/Cotizador/Form-Item.aspx?NAME=" + NOMBRE + "&IDENDODET=" + IDENDODET + "&IDENDO=" + IDENDO +
        "&TOKEN=" + TOKEN) + "';", true);
        }

        
    }

    protected void LinkBtnCantidad_Click(object sender, EventArgs e)
    {

        if (TxtCantidad.Enabled)
        {
            double Cant;
            if (double.TryParse(TxtCantidad.Text, out Cant))
            {
                Update("COT_ENDODET", "IDENDODET", IDENDODET, "CANTIDAD", Cant, false);
                

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Información actualizada.'); window.location='" +
                Page.ResolveUrl("~/View/Distribuidor/Cotizador/Form-Item.aspx?NAME=" + NOMBRE + "&IDENDODET=" + IDENDODET + "&IDENDO=" + IDENDO +
                "&TOKEN=" + TOKEN) + "';", true);
            }
            

            TxtCantidad.Enabled = false;
            LinkBtnCantidad.Text = "Editar";
        }
        else
        {
            TxtCantidad.Enabled = true;
            LinkBtnCantidad.Text = "Actualizar";
        }
        
    }

    protected void LinkBtnCostoMO_Click(object sender, EventArgs e)
    {
        if (PanelTxtCostoMO.Visible)
        {
            double MO;
            if (double.TryParse(TxtCostoMO.Text, out MO))
            {
                Update("COT_ENDODET", "IDENDODET", IDENDODET, "COSTMOUN", MO, false);
                DetalleCotizacion.GetItem Item = new DetalleCotizacion.GetItem(IDENDODET, true);
                DetalleCotizacion.ActualizarMontositem actualizar = new DetalleCotizacion.ActualizarMontositem(Item.Item);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Información actualizada.'); window.location='" +
                Page.ResolveUrl("~/View/Distribuidor/Cotizador/Form-Item.aspx?NAME=" + NOMBRE + "&IDENDODET=" + IDENDODET + "&IDENDO=" + IDENDO +
                "&TOKEN=" + TOKEN) + "';", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El formato ingresado no es correcto.');", true);
            }
            
            LinkBtnCostoMO.Text = "Actualizar";
            PanelTxtCostoMO.Visible = false;

        }
        else
        {
            PanelTxtCostoMO.Visible = true;
            LinkBtnCostoMO.Text = "Actualizar";
        }
    }

    protected void BtnVolverFormulario_Click(object sender, EventArgs e)
    {
        Cot = new Cotizacion.GetRowInfo(IDENDO, TOKEN, true);
        Response.Redirect("~/View/Distribuidor/Cotizador/Formulario.aspx?RUT=" + Cot.Cliente.Rut  + "&ID=" + IDENDO + "&TOKEN=" + TOKEN);
    }
}