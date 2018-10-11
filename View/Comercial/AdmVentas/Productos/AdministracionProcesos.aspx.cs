using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Comercial;
using Alfak;

public partial class Com_Adm_Proc_ : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            
            GetMagnitudes Mag = new GetMagnitudes();
            FillDDL(DDLmagnitud, "MAGNITUD", "MAGNITUD", "Seleccionar Magnitud", Mag.Magnitudes());

            //cargar los 30 default
            GetListProcesos procesos = new GetListProcesos();
            FillingTable(procesos.ListaProcesos);

        }
        
       
    }

    protected void BtnCrearProc_Click(object sender, EventArgs e)
    {
        double _Costo;
        decimal _Merma;
        string _IdReturn;
        if (Double.TryParse(TxtPrecio.Text, out _Costo) && decimal.TryParse(HdnMerma.Value.Replace(".",","), out _Merma))
        {
            Random random = new Random();
            int _TokenId = random.Next(10000, 9999999);


            ProcesosClass._Proceso proceso = new ProcesosClass._Proceso
            {
                Nombre = TxtNombreProc.Text,
                Descripcion = TxtDescripcion.Text,
                ID_Magmed = DDLUnidadmed.SelectedValue,
                Costo_Unit = _Costo,
                Merma = Convert.ToDouble(_Merma),
                TokenId = _TokenId.ToString(),
                Estado=true,
            };

            ProcesosClass Procesos = new ProcesosClass();
            bool IsInserted = Procesos.Insert(proceso);
            if (IsInserted)
            {
                _IdReturn = Procesos.IdBeforeInsert;
                //abrir modal asociacion con alfak
                

                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "OpenModalAsocAlfak('asociar este proceso con uno de Alfak?','"+_IdReturn+"','El proceso ha sido creado')", true);

            }

            
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El costo no tiene el formato correcto, por favor intentelo nuevamente.');", true);
        }


        
        

    }

    HtmlGenericControl table;
    HtmlGenericControl thead;
    
    HtmlGenericControl strong;
    HtmlGenericControl th;
    HtmlGenericControl tbody;
    HtmlGenericControl td;


    protected void FillingTable(List<ProcesosClass._Proceso> ListaProcesos)
    {
        table = new HtmlGenericControl("table")
        {
            ID = "TablaProcesos",
            
        };
        table.Attributes.Add("class", "table card-border-lila");


        //armando la cabecera
        thead = new HtmlGenericControl("thead");
        strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        thead.Attributes.Add("class", "card-bg-uva text-white");
        tbody = new HtmlGenericControl("tbody");

        for (int i = 0; i <= 5; i++)
        {
            th = new HtmlGenericControl("th");
            if (i==0)
            {
                th.InnerHtml = "Nombre";
            }
            else if (i==1)
            {
                th.InnerHtml = "Descripción";
            }
            else if (i==2)
            {
                th.InnerHtml = "Costo";
            }
            else if (i == 3)
            {
                th.InnerHtml = "Merma";
            }
            else if (i==4)
            {
                th.InnerHtml = "Cod Alfak";
            }
            else if (i==5)
            {
                th.InnerHtml = "Última Actualización";
            }
            trhead.Controls.Add(th);

        }
        thead.Controls.Add(trhead);
        strong.Controls.Add(thead);

        //agregar la cabecera a la tabla
        table.Controls.Add(strong);


        //armando la informacion y agregandola a la tabla
        foreach (var item in ListaProcesos)
        {
            HtmlGenericControl tr = new HtmlGenericControl("tr");
            HtmlGenericControl td1 = new HtmlGenericControl("td");
            HtmlGenericControl td2 = new HtmlGenericControl("td");
            HtmlGenericControl td3 = new HtmlGenericControl("td");
            HtmlGenericControl td4 = new HtmlGenericControl("td");
            HtmlGenericControl td5 = new HtmlGenericControl("td");
            HtmlGenericControl td6 = new HtmlGenericControl("td");
            HtmlGenericControl tdhidden = new HtmlGenericControl("td");
            HtmlGenericControl tdhidde2 = new HtmlGenericControl("td");
            HtmlGenericControl tdhidde3 = new HtmlGenericControl("td");
            HtmlGenericControl tdhdnprecio = new HtmlGenericControl("td");
            HtmlGenericControl tdhdnunidad = new HtmlGenericControl("td");


            tr.Attributes.Add("class", "filas");

            //agrega col 1
            td1.InnerHtml = item.Nombre;
            tr.Controls.Add(td1);

            //agrega col 2
            td2.InnerHtml = item.Descripcion;
            tr.Controls.Add(td2);

            //agrega col 3
            if (item.Costo_Unit!=0)
            {
                td3.InnerHtml = item.Costo_Unit.ToString("C0",CultureInfo.CurrentCulture) + "/" + item._MagSimbolo;
            }
            else
            {
                td3.InnerHtml = "Sin Costo";
            }
            tr.Controls.Add(td3);

            //agrega col 4
            if (item.Merma!=0)
            {
                double Porc_merma = item.Merma * 100;
                td4.InnerHtml = Porc_merma.ToString() + " %";
            }
            else
            {
                td4.InnerHtml = "Sin Merma";
            }
            tr.Controls.Add(td4);

            //agrega asociacion alfak

            if (string.IsNullOrEmpty(item._CodAlfak))
            {
                td6.InnerHtml = "Sin Asociación";
            }
            else
            {
                td6.InnerHtml = item._CodAlfak;
            }
            tr.Controls.Add(td6);

            //agrega update
            td5.InnerHtml = item.F_Update.ToShortDateString();
            tr.Controls.Add(td5);

           

            //agrega las col escondida
            tdhidden.InnerHtml = item._ID;
            tdhidden.Attributes.Add("style","display:none;");
            tr.Controls.Add(tdhidden);

            tdhidde2.InnerHtml = item.TokenId;
            tdhidde2.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidde2);


            //agrega fila al tbody
            tbody.Controls.Add(tr);
        }

        table.Controls.Add(tbody);
        divtabla.Controls.Clear();
        divtabla.Controls.Add(table);
        


    }

    protected void BtnEditItem_Click(object sender, EventArgs e)
    {
        //cargar los valores y abrir el modal
        string _ID = HdnIDItem.Value;
        string TokenId = HdnTokenIdItem.Value;

        ProcesosClass Proc = new ProcesosClass();
        ProcesosClass._Proceso proceso = Proc.GetProceso(_ID, TokenId,true);
        TxtMdlEditNombre.Text = proceso.Nombre;
        TxtMdlEditDescr.Text = proceso.Descripcion;

        //cargar el DDLmag
        GetMagnitudes Mag = new GetMagnitudes();
        FillDDL(DDLMdlEditMag, "MAGNITUD", "MAGNITUD", "Seleccionar Magnitud", Mag.Magnitudes());
        DDLMdlEditMag.SelectedValue = proceso._Magnitud;

        //cargar el DDLUnidMed
        GetMagnitudes UnidMed = new GetMagnitudes(DDLMdlEditMag.SelectedValue);
        
            FillDDL(DDLMdlEditUnMed, "UNIDAD_MEDIDA", "_ID", "Seleccionar Unidad", UnidMed.AllMagnitudes);
        DDLMdlEditUnMed.SelectedValue = proceso.ID_Magmed;
        LblMdlEditSimbUnit.Text = " /" + proceso._MagSimbolo;
        TxtMdlEditCosto.Text = proceso.Costo_Unit.ToString();
        string mermax100 = (proceso.Merma * 100).ToString();
        if (string.IsNullOrEmpty(proceso._CodAlfak))
        {
            LblEditAsocAlfak.Text = "No tiene asociado Proceso de Alfak ";
            
            LinkTomdlAsocAlfak.InnerHtml = "Asociar";
        }
        else
        {
            ProductoAlfak alfak = new ProductoAlfak();
            ProductoAlfak.BA_PRODUKTE bA_PRODUKTE = alfak.GetByCod(proceso._CodAlfak);
            LblEditAsocAlfak.Text = "El código Alfak asociado es " + proceso._CodAlfak + " - " + bA_PRODUKTE.Descripcion;
            
            LinkTomdlAsocAlfak.InnerHtml = "Editar";
        }
        



        string Function = "OpenModalEdit('" + mermax100 + "');";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "CallMyFunction", Function, true);

        GetListProcesos procesos = new GetListProcesos();
        FillingTable(procesos.ListaProcesos);


    }

   

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        ProcesosClass Proc = new ProcesosClass();
        bool IsDelete = Proc.UpdateProceso(HdnIDItem.Value, ProcesosClass.COT_MPROCESOS.ESTADO, false);
        if (IsDelete)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El proceso " + lbltitleDetail.InnerHtml + " ha sido eliminado.'); window.location='" +
           Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/AdministracionProcesos.aspx") + "';", true);

        }
    }

    protected void Filtrar_Click(object sender, EventArgs e)
    {

    }

    protected void SearchBtn_Click(object sender, EventArgs e)
    {
        GetListProcesos GetP = new GetListProcesos(txtSearch.Text);
        FillingTable(GetP.ListaProcesos);
    }

    protected void DDLmagnitud_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLmagnitud.SelectedIndex != 0)
        {
            GetMagnitudes Mag = new GetMagnitudes(DDLmagnitud.SelectedValue);
            LblMedida1.Visible = true;
            DDLUnidadmed.Visible = true;
            FillDDLUnidadmed(Mag.AllMagnitudes);
            LblPrecioX.Visible = false;
            TxtPrecio.Visible = false;
            LblSimbolUn.Visible = false;


        }
        else
        {
            LblPrecioX.Visible = false;
            TxtPrecio.Visible = false;
            LblSimbolUn.Visible = false;
        }
    }

    protected void DDLUnidadmed_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLUnidadmed.SelectedIndex != 0)
        {
            GetMagnitudes Mag = new GetMagnitudes();
            bool Ismag = Mag.IsMagnitud(DDLUnidadmed.SelectedValue);
            if (Ismag)
            {
                LblPrecioX.Visible = true;
                TxtPrecio.Visible = true;
                LblSimbolUn.Visible = true;
                LblSimbolUn.Text = " /" + Mag._Magnitud.SIMBOLO;
            }
        }
        else
        {
            LblPrecioX.Visible = false;
            TxtPrecio.Visible = false;
            LblSimbolUn.Visible = false;
        }


    }

    protected void FillDDLUnidadmed(List<GetMagnitudes.Magnitud> ListaM)
    {
        FillDDL(DDLUnidadmed, "UNIDAD_MEDIDA", "_ID", "Seleccionar Unidad", ListaM);
        
    }

    

    protected void FillDDL(DropDownList DDL, string TextField, string ValueField, string FirstItem, object _Datasource)
    {
        DDL.DataSource = _Datasource;
        DDL.DataTextField = TextField;
        DDL.DataValueField = ValueField;
        DDL.DataBind();
        DDL.Items.Insert(0, new ListItem("--" + FirstItem + "--"));

    }

    protected void DDLMdlEditMag_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLMdlEditMag.SelectedIndex!=0)
        {
            DDLMdlEditUnMed.Enabled = true;
            GetMagnitudes Mag = new GetMagnitudes(DDLMdlEditMag.SelectedValue);
            FillDDL(DDLMdlEditUnMed, "UNIDAD_MEDIDA", "_ID", "Seleccionar Unidad", Mag.AllMagnitudes);
        }
        else
        {
            DDLMdlEditUnMed.Enabled = false;
        }
    }

    protected void DDLMdlEditUnMed_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLMdlEditUnMed.SelectedIndex!=0)
        {
            GetMagnitudes mAG = new GetMagnitudes();
            bool IsMAG = mAG.IsMagnitud(DDLMdlEditUnMed.SelectedValue);
            if (IsMAG)
            {
                LblMdlEditSimbUnit.Text ="/" + mAG._Magnitud.SIMBOLO;
            }
            

        }

    }

    protected void BtnMdlEditProc_Click(object sender, EventArgs e)
    {
        double _Costo;
        decimal _Merma;
        if (double.TryParse(TxtMdlEditCosto.Text,out _Costo) && decimal.TryParse(HdnMdlEditMerma.Value.Replace(".", ","), out _Merma))
        {
            ProcesosClass._Proceso proceso = new ProcesosClass._Proceso
            {
                _ID = HdnIDItem.Value,
                TokenId = HdnTokenIdItem.Value,
                Nombre = TxtMdlEditNombre.Text,
                Descripcion = TxtMdlEditDescr.Text,
                ID_Magmed = DDLMdlEditUnMed.SelectedValue,
                Costo_Unit = _Costo,
                Merma = Convert.ToDouble(_Merma),
                Estado=true,
            };

            ProcesosClass Proc = new ProcesosClass();
            bool IsEdited = Proc.UpdateProceso(proceso);

            if (IsEdited)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El proceso " + proceso.Nombre + " ha sido modificado.'); window.location='" +
           Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/AdministracionProcesos.aspx") + "';", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de editar el proceso, por favor intentelo nuevamente.');", true);
            }

            
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El costo no tiene el formato correcto, por favor intentelo nuevamente.'); window.location='" + 
            Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/AdministracionProcesos.aspx") +"';", true);
        }
        

        


    }

    protected void BtnNoAlfak_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/View/Comercial/AdmVentas/Productos/AdministracionProcesos.aspx");
    }

    protected void BtnAsociarAlfak_Click(object sender, EventArgs e)
    {
        PanelAsocAlfak.Visible = true;
        BtnAsociarAlfak.Enabled = false;
        
        FamiliasAlfak alfak = new FamiliasAlfak();
        List<FamiliasAlfak.Familia> familia = alfak.GetLbl2Familia("S");
        FillDDL(DDLTipoProcAsocAlfak, "Name", "ID", "Seleccionar Tipo", familia);
    }

    protected void DDLTipoProcAsocAlfak_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLTipoProcAsocAlfak.SelectedIndex!=0)
        {
            LblforDDLSubtipo.Visible = true;
            DDLSubTipoProcAsocAlfak.Visible = true;
            LblforDDLSubtipo.Text = "Sub Tipo";
            FamiliasAlfak alfak = new FamiliasAlfak("Procesos");
            List<FamiliasAlfak.Familia> familias = alfak.GetLbl3Familia(DDLTipoProcAsocAlfak.SelectedValue);
            FillDDL(DDLSubTipoProcAsocAlfak, "Name", "ID", "Seleccionar SubTipo", familias);
        }
        else
        {
            LblforDDLSubtipo.Visible = false;
            DDLSubTipoProcAsocAlfak.Visible = false;
        }
    }

    protected void DDLSubTipoProcAsocAlfak_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLSubTipoProcAsocAlfak.SelectedIndex!=0)
        {
            GrdListProcAlfak.Visible = true;
            TxtSearchAlfak.Visible = true;
            BtnSearchAlfak.Visible = true;
            
            ProductoAlfak productos = new ProductoAlfak(DDLSubTipoProcAsocAlfak.SelectedValue);
            GrdListProcAlfak.DataSource = productos.Listaproductos;
            GrdListProcAlfak.DataBind();
        }
        else
        {
            GrdListProcAlfak.Visible = false;
            TxtSearchAlfak.Visible = false;
            BtnSearchAlfak.Visible = false;
            
        }
        

    }

    protected void TxtSearchAlfak_TextChanged(object sender, EventArgs e)
    {
        ProductoAlfak productos = new ProductoAlfak(DDLSubTipoProcAsocAlfak.SelectedValue, TxtSearchAlfak.Text);
        GrdListProcAlfak.DataSource = productos.Listaproductos;
        GrdListProcAlfak.DataBind();
    }

    protected void GrdListProcAlfak_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdListProcAlfak.PageIndex = e.NewPageIndex;

    }

    protected void CheckBAsigacion_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GrdListProcAlfak.Rows)
        {
            var checkbox = row.FindControl("CheckBAsigacion") as CheckBox;
            if (checkbox.Checked)
            {
                
            }

        }
        
    }

    protected void LinkSelectAlfak_Click(object sender, EventArgs e)
    {
        ProcesosClass Proc = new ProcesosClass();
        string _CodigoAlfak = ((LinkButton)sender).Text;
        bool IsEdited = Proc.UpdateProceso(HdnIdbeforeCreate.Value,ProcesosClass.COT_MPROCESOS.COD_ALFAK,_CodigoAlfak);
        bool IsEdited2 = Proc.UpdateProceso(HdnIdbeforeCreate.Value,ProcesosClass.COT_MPROCESOS.KA_WGR,DDLSubTipoProcAsocAlfak.SelectedValue);
        if (IsEdited)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El proceso ha sido vinculado con el código " + _CodigoAlfak + " de Alfak'); window.location='" +
       Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/AdministracionProcesos.aspx") + "';", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de vincular el proceso con Alfak, por favor intentelo nuevamente.');", true);
        }
    }



    //sin uso
    protected void LinkToEditAsocAlfak_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction2", "OpenModalAsocAlfak('Editar el código de Alfak Asociado?','" + HdnIDItem.Value + "','Editar Código')", true);
    }
}