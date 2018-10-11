using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using Comercial;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.HtmlControls;
using Alfak;

public partial class Com_Adm_Default_ : Page
{
    string TOKEN;
    protected void Page_Load(object sender, EventArgs e)
    {
        string ID = Request.QueryString["ID"];
        TOKEN = Request.QueryString["TOKEN"];

        if(!IsPostBack)
        {
            if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(TOKEN))
            {
                ComponentesClass CsC = new ComponentesClass();
                bool IsComponente = CsC.IsComponente(ID, TOKEN);
                HdnIdComp.Value = ID;
                if (IsComponente)
                {
                    
                    Imagen.ImageUrl = CsC._Detalle.Path_Photo;
                    
                    TxtNombre.Text = CsC._Detalle.Nombre;
                    LblCreateDate.Text = CsC._Detalle.F_Creacion.ToLongDateString();
                    LblEditDate.Text = CsC._Detalle.F_Actualizacion.ToLongDateString();
                    TxtDescripcion.Text = CsC._Detalle.Descripcion;
                    
                   
                    FillDDLMagnitud();
                    FamiliasAlfak alfak = new FamiliasAlfak();
                    List<FamiliasAlfak.Familia> familias = alfak.familias;
                    FamiliasAlfak.Familia Item1 = new FamiliasAlfak.Familia { ID = "VI", Name = "VIDRIOS" };
                    familias.Add(Item1);

                    FillDDL(DDLTipoProcAsocAlfak, "Name", "ID", "Seleccionar Tipo", familias);

                    if (string.IsNullOrEmpty(CsC._Detalle.COD_ALFAK))
                    {
                        LblEditAsocAlfak.Text = "El componente no tiene asociado ningún código en Alfak";
                        LinkTomdlAsocAlfak.InnerHtml = "Asociar";
                        TitleMdlAsocAlfak1.InnerHtml = "Asociar Código Alfak";
                    }
                    else
                    {
                        ProductoAlfak alfakP = new ProductoAlfak();
                        ProductoAlfak.BA_PRODUKTE producto = alfakP.GetByCod(CsC._Detalle.COD_ALFAK);
                        LblEditAsocAlfak.Text = "El componente tiene asociado el código Alfak \"" + producto.CodigoAlfak + "\" - " + producto.Descripcion;
                        lblFamilyAlfak.Text = "Familia Alfak " + producto.Familia_Alfak;
                        LinkTomdlAsocAlfak.InnerHtml = "Editar";
                        TitleMdlAsocAlfak1.InnerHtml = "Editar Código Alfak";
                    }

                    

                    //habilita el textbox si es que no tiene precio
                    if (CsC._Detalle.PrecioUn==0)
                    {

                    }
                    //habilita el textbox para actualizar la cantidad del componente
                    if (CsC._Detalle.CantEmb==0)
                    {

                    }

                   
                    

                    //verifica si tiene precio
                    if (CsC._Detalle.PrecioUn == 0)
                    {
                        BtnEditPrecio.InnerHtml = "Ingresar";
                        LblPrecioydim.Text = "Debe ingresar precio y dimensión.";
                    }
                    else
                    {
                        BtnEditPrecio.InnerHtml = "Editar";
                        GetMagnitudes Mag = new GetMagnitudes();

                        LblPrecioydim.Text = "Precio por " + CsC._Detalle.UnMedSimbolo + " : " + CsC._Detalle.PrecioUn.ToString("C0",CultureInfo.CurrentCulture) ;
                        LblCantEmb.Text = CsC._Detalle.CantEmb + " " + CsC._Detalle.UnidadMedida + " por unidad de entrega."; 
                        TxtPrecio.Text = CsC._Detalle.PrecioUn.ToString();
                        TxtCant.Text = CsC._Detalle.CantEmb.ToString();
                        DDLmagnitud.SelectedValue = CsC._Detalle.Magnitud;
                        DDLUnidadmed.Visible = true;
                        GetMagnitudes Mag2 = new GetMagnitudes(CsC._Detalle.Magnitud);
                        
                        FillDDLUnidadmed(Mag2.AllMagnitudes);
                        
                        DDLUnidadmed.SelectedValue = CsC._Detalle.ID_Magnitud;
                        LblPrecioX.Visible = true;
                        LblCantX.Visible = true;
                        TxtCant.Visible = true;
                        TxtPrecio.Visible = true;
                        LblCant2.Visible = true;
                        
                        LblCantX.Text = CsC._Detalle.UnidadMedida + " por unidad de entrega:";
                        LblMedida1.Visible = true;
                        LblSimbolUn.Visible = true;
                        LblPrecioX.Text = "Precio por " + CsC._Detalle.UnMedSimbolo + ":";


                    }

                    //verifica si tiene procesos asociados
                    ProcesosClass.GetProcNoAsign procesos = new ProcesosClass.GetProcNoAsign(ID, true);
                    FillDDlProcesos(procesos.Procesos);
                    if (!CsC._Detalle.HasProc)
                    {
                        
                        LblProcesos.Text = "No tiene procesos asociados";
                        LblMdltitleProc.Text = "Agregar Procesos";
                    }
                    else
                    {
                        LblMdltitleProc.Text = "Agregar Procesos";
                        LblProcesos.Visible=false;
                        ProcesosClass.GetProcAsign procAsign = new ProcesosClass.GetProcAsign(ID,true);
                        FillingTable(procAsign.Procesos);

                    }
                    

                }
                else
                {
                    
                    Response.Redirect(Error404.Redireccion(MasterPageFile, "El componente no fue encontrado"));
                }
                


            }
            else
            {
                Response.Redirect(Error404.Redireccion(MasterPageFile, "Hubo un error al tratar de buscar el componente, intentelo nuevamente"));
                
            }


        }
        
       
    }

    HtmlGenericControl table;
    HtmlGenericControl thead;

    HtmlGenericControl strong;
    HtmlGenericControl th;
    HtmlGenericControl tbody;
    HtmlGenericControl td;


    protected void FillingTable(List<ProcesosClass.GetProcAsign.ProcAsign> ListaProcesos)
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
        thead.Attributes.Add("class", "fondo-uva text-white");
        tbody = new HtmlGenericControl("tbody");

        for (int i = 0; i <= 4; i++)
        {
            th = new HtmlGenericControl("th");
            if (i == 0)
            {
                th.InnerHtml = "Nombre";
            }
            else if (i == 1)
            {
                th.InnerHtml = "Costo";
            }
            else if (i == 2)
            {
                th.InnerHtml = "Merma";
            }
            else if (i == 3)
            {
                th.InnerHtml = "Tipo";
            }
            else if (i == 4)
            {
                th.InnerHtml = "Fecha Asignación";
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
            if (item.Costo_Unit != 0)
            {
                td3.InnerHtml = item.Costo_Unit.ToString("C0", CultureInfo.CurrentCulture) + "/" + item._MagSimbolo;
            }
            else
            {
                td3.InnerHtml = "Sin Costo";
            }
            tr.Controls.Add(td3);

            //agrega col 3
            if (item.Merma != 0)
            {
                double Porc_merma = item.Merma * 100;
                td4.InnerHtml = Porc_merma.ToString() + " %";
            }
            else
            {
                td4.InnerHtml = "Sin Merma";
            }
            tr.Controls.Add(td4);

            //agrega col tipo
            if (item.OBLIGATORIO)
            {
                td2.InnerHtml = "Obligatorio";
            }
            else
            {
                td2.InnerHtml = "No Obligatorio";
            }
            
            tr.Controls.Add(td2);

            

            

            //agrega col fecha
            td5.InnerHtml = item.F_Asign.ToShortDateString();
            tr.Controls.Add(td5);

            //agrega las col escondida
            tdhidden.InnerHtml = item.ID_Asign;
            tdhidden.Attributes.Add("style", "display:none;");
            tr.Controls.Add(tdhidden);

            


            //agrega fila al tbody
            tbody.Controls.Add(tr);
        }

        table.Controls.Add(tbody);
        divTablaProc.Controls.Clear();
        divTablaProc.Controls.Add(table);



    }

    protected void LinkBtnNombre_Click(object sender, EventArgs e)
    {
        if(TxtNombre.Enabled)
        {
            TxtNombre.Enabled = false;
            LinkBtnNombre.Text = "Editar";
            //hacer un update a la tabla con el nombre
            ComponentesClass cOMPO = new ComponentesClass();
            bool Isinserted = cOMPO.UpdateComponente(HdnIdComp.Value, COT_MCOMPONENTES.Campos.NOMBRE(), TxtNombre.Text);
            if(!Isinserted)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar Actualizar la información, intentelo nuevamente.'));</script>", false);
            }
        }
        else
        {
            TxtNombre.Enabled = true;
            LinkBtnNombre.Text = "Actualizar";

            
            


        }
        
    }

   

    protected void FillDDLUnidadmed(List<GetMagnitudes.Magnitud> ListaM)
    {
        FillDDL(DDLUnidadmed, "UNIDAD_MEDIDA", "_ID", "Seleccionar Unidad", ListaM);
        
    }

    protected void FillDDlProcesos(List<ProcesosClass._Proceso> Procesos)
    {
        FillDDL(DDLProcesosxasignar, "Nombre", "_ID", "Seleccionar Proceso", Procesos);
    }

   

    protected void FillDDLMagnitud()
    {
        GetMagnitudes Mag = new GetMagnitudes();
        FillDDL(DDLmagnitud, "MAGNITUD", "MAGNITUD", "Seleccionar Magnitud", Mag.Magnitudes());
    }

    protected void LinkBtnDescripcion_Click(object sender, EventArgs e)
    {
        if (TxtDescripcion.Enabled)
        {
            TxtDescripcion.Enabled = false;
            LinkBtnDescripcion.Text = "Editar";
            //hacer un update a la tabla con la descripcion
            ComponentesClass cOMPO = new ComponentesClass();
            bool Isinserted= cOMPO.UpdateComponente(HdnIdComp.Value, COT_MCOMPONENTES.Campos.DESCRIPCION(), TxtDescripcion.Text);
            if (!Isinserted)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar Actualizar la información, intentelo nuevamente.'));</script>", false);
            }
        }
        else
        {
            TxtDescripcion.Enabled = true;
            LinkBtnDescripcion.Text = "Actualizar";

            


        }

    }

   

    protected void BtnEditImage_Click(object sender, EventArgs e)
    {
        string folio = DateTime.Now.ToLongTimeString().Replace(":", "");
        string path = Path.Combine(Server.MapPath("~/Images/COT_COMPONENTES/"));

        if (!string.IsNullOrEmpty(FileUpFoto.FileName.ToString()))
        {
            string fileName = folio + FileUpFoto.FileName.ToString();
            string path_foto = "http://" + HttpContext.Current.Request.Url.Authority + "/Images/COT_COMPONENTES/" + fileName;
            path = path + fileName;
            FileUpFoto.SaveAs(path);

            //hacer un update a la tabla con la foto
            ComponentesClass cOMPO = new ComponentesClass();
            bool Isinserted = cOMPO.UpdateComponente(HdnIdComp.Value, COT_MCOMPONENTES.Campos.IMG_PATH(), path_foto);
            if (!Isinserted)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar Actualizar la información, intentelo nuevamente.'));</script>", false);
            }
            else
            {
                Response.Redirect("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + HdnIdComp.Value + "&TOKEN=" + TOKEN);
            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se seleccionó ningún archivo.'));</script>", false);
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


    protected void LinkBtnPrecio_Click(object sender, EventArgs e)
    {

    }

    protected void DDLmagnitud_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLmagnitud.SelectedIndex!=0)
        {
            GetMagnitudes Mag = new GetMagnitudes(DDLmagnitud.SelectedValue);
            LblMedida1.Visible = true;
            DDLUnidadmed.Visible = true;
            FillDDLUnidadmed(Mag.AllMagnitudes);
            LblPrecioX.Visible = false;
            TxtPrecio.Visible = false;
            LblSimbolUn.Visible = false;
            LblCantX.Visible = false;
            TxtCant.Visible = false;
            LblCant2.Visible = false;


        }
        else
        {
            LblPrecioX.Visible = false;
            TxtPrecio.Visible = false;
            LblSimbolUn.Visible = false;
            DDLUnidadmed.Visible = false;
            LblMedida1.Visible = false;
            LblCantX.Visible = false;
            TxtCant.Visible = false;
            LblCant2.Visible = false;
        }
    }

    protected void DDLUnidadmed_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLUnidadmed.SelectedIndex!=0)
        {
            GetMagnitudes Mag = new GetMagnitudes();
            bool Ismag = Mag.IsMagnitud(DDLUnidadmed.SelectedValue);
            if (Ismag)
            {
                LblPrecioX.Visible = true;
                TxtPrecio.Visible = true;
                LblSimbolUn.Visible = true;
                LblPrecioX.Text = "Precio por " + Mag._Magnitud.SIMBOLO + ":";

                LblCantX.Visible = true;
                TxtCant.Visible = true;
                LblCant2.Visible = true;
                
                LblCantX.Text = Mag._Magnitud.SIMBOLO + " por unidad de entrega:"; ;
            }
        }
        else
        {
            LblCantX.Visible = false;
            TxtCant.Visible = false;
            LblCant2.Visible = false;
            LblPrecioX.Visible = false;
            TxtPrecio.Visible = false;
            LblSimbolUn.Visible = false;
        }
        
        
    }

    protected void Btnmodalprecio_Click(object sender, EventArgs e)
    {
        double number;
        double cantidad;
        bool Isupdateprecio = false;
        bool IsUpdateUnMed = false;
        bool IsUpdateCant = false;
        string ErrorA = "";
        string ErrorB = "";
        string ErrorC = "";

        ComponentesClass Comp = new ComponentesClass();
        if (Double.TryParse(TxtPrecio.Text, out number))
        {
            
           Isupdateprecio = Comp.UpdateComponente(HdnIdComp.Value, COT_MCOMPONENTES.Campos.PRECIO_UNIT(), number);
           
        }
        else
        {
            ErrorB = " Formato de precio no es correcto ";
        }
        
        if (Double.TryParse(TxtCant.Text, out cantidad))
        {
            IsUpdateCant = Comp.UpdateComponente(HdnIdComp.Value, COT_MCOMPONENTES.Campos.CANT_UNIT(), cantidad);
        }
        else
        {
            ErrorA = " Formato de cantidad no es correcto ";
        }

       IsUpdateUnMed = Comp.UpdateComponente(HdnIdComp.Value, COT_MCOMPONENTES.Campos.ID_MAGMED(), DDLUnidadmed.SelectedValue);


        if (!IsUpdateCant)
        {
            ErrorA = " Error en la cantidad ";
        }
        if (!Isupdateprecio)
        {
            ErrorB = " Error en el precio ";
        }
        if (!IsUpdateUnMed)
        {
            ErrorC = " Error en la unidad de medida ";
        }

        if (IsUpdateCant && Isupdateprecio && IsUpdateUnMed)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('La actualización ha sido exitosa.'); window.location='" +
                Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + HdnIdComp.Value + "&TOKEN=" + TOKEN) + "';", true);

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Hubo un error al intentar actualizar , intentelo de nuevo. Errores " + ErrorA + ErrorB + ErrorC + "'));</script>", false);
        }

    }

    protected void DDLProcesosxasignar_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLProcesosxasignar.SelectedIndex!=0)
        {
            ProcesosClass Proc = new ProcesosClass();
            ProcesosClass._Proceso proceso = Proc.GetProceso(DDLProcesosxasignar.SelectedValue,true);
            LblCostoProcAsign1.Visible = true;
            LbldescripProcAsign1.Visible = true;
            RaProcObligatorio.Visible = true;
            LblDescripProcAsign.Text = proceso.Descripcion;
            LblCostoProcAsign.Text =  proceso.Costo_Unit.ToString("C0", CultureInfo.CurrentCulture) + "/" + proceso._MagSimbolo + " (" + proceso._Unidad_Medida + ")"; 
        }
        else
        {
            RaProcObligatorio.Visible = false;
            LblCostoProcAsign1.Visible = false;
            LbldescripProcAsign1.Visible = false;
        }
    }

    protected void BtnAddProc_Click(object sender, EventArgs e)
    {
        ProcesosClass proceso = new ProcesosClass();
        bool IsAsigned =  proceso.InsertAsignProc(true, DDLProcesosxasignar.SelectedValue, HdnIdComp.Value, true);

        if (IsAsigned)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El proceso " + DDLProcesosxasignar.SelectedItem + " ha sido asignado.'); window.location='" +
           Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + HdnIdComp.Value + "&TOKEN="+TOKEN) + "';", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de asignar el proceso, por favor intentelo nuevamente.');", true);

        }

    }

    protected void BtnDeleteProc_Click(object sender, EventArgs e)
    {
        ProcesosClass Proceso = new ProcesosClass();
        bool IsDelete = Proceso.UpdateAsignProc(HdnIdAsignProc.Value, ProcesosClass.COT_MASIGPROC.ESTADO, false);

        if (IsDelete)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El proceso ha sido eliminado.'); window.location='" +
          Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + HdnIdComp.Value + "&TOKEN=" + TOKEN) + "';", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de eliminar el proceso, por favor intentelo nuevamente.');", true);
        }
    }

    protected void DDLTipoProcAsocAlfak_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLTipoProcAsocAlfak.SelectedIndex != 0)
        {
            LblforDDLSubtipo.Visible = true;
            DDLSubTipoProcAsocAlfak.Visible = true;
            LblforDDLSubtipo.Text = "Tipo";
            FamiliasAlfak alfak = new FamiliasAlfak();
            List<FamiliasAlfak.Familia> familias = new List<FamiliasAlfak.Familia>();
            if (DDLTipoProcAsocAlfak.SelectedValue=="VI")
            {
                FamiliasAlfak.Familia Item1 = new FamiliasAlfak.Familia { ID = "B11", Name = "VIDRIOS INCOLOROS" };
                familias.Add(Item1);
                FamiliasAlfak.Familia Item2 = new FamiliasAlfak.Familia { ID = "B21", Name = "VIDRIOS EXTRACLAROS" };
                familias.Add(Item2);
                FamiliasAlfak.Familia Item3 = new FamiliasAlfak.Familia { ID = "B31", Name = "VIDRIOS DE COLORES" };
                familias.Add(Item3);
                FamiliasAlfak.Familia Item4 = new FamiliasAlfak.Familia { ID = "B41", Name = "ESPEJOS" };
                familias.Add(Item4);
                FamiliasAlfak.Familia Item5 = new FamiliasAlfak.Familia { ID = "B51", Name = "VIDRIOS MATEADOS" };
                familias.Add(Item5);
                FamiliasAlfak.Familia Item6 = new FamiliasAlfak.Familia { ID = "B61", Name = "VIDRIOS CATEDRAL" };
                familias.Add(Item6);
                FamiliasAlfak.Familia Item7 = new FamiliasAlfak.Familia { ID = "B71", Name = "VIDRIOS CONTROL SOLAR" };
                familias.Add(Item7);
                FamiliasAlfak.Familia Item8 = new FamiliasAlfak.Familia { ID = "C11", Name = "LAMINADOS INCOLOROS" };
                familias.Add(Item8);
                FamiliasAlfak.Familia Item9 = new FamiliasAlfak.Familia { ID = "C21", Name = "LAMINADOS COLOR" };
                familias.Add(Item9);
                FamiliasAlfak.Familia Item10 = new FamiliasAlfak.Familia { ID = "C31", Name = "LAMINADOS ACÚSTICOS" };
                familias.Add(Item10);

            }
            else
            {
                familias = alfak.GetLbl3Familia(DDLTipoProcAsocAlfak.SelectedValue);
            }
             

            
            
            FillDDL(DDLSubTipoProcAsocAlfak, "Name", "ID", "Seleccionar Tipo", familias);
        }
        else
        {
            LblforDDLSubtipo.Visible = false;
            DDLSubTipoProcAsocAlfak.Visible = false;
        }
    }

    protected void DDLSubTipoProcAsocAlfak_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLSubTipoProcAsocAlfak.SelectedIndex != 0)
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

    protected void LinkSelectAlfak_Click(object sender, EventArgs e)
    {
        ComponentesClass cOMPO = new ComponentesClass();
        string _CodigoAlfak = ((LinkButton)sender).Text;
        string WGR = DDLSubTipoProcAsocAlfak.SelectedValue;
        bool IsEdited = cOMPO.UpdateComponente(HdnIdbeforeCreate.Value, COT_MCOMPONENTES.Campos.COD_ALFAK(), _CodigoAlfak);
        bool IsEdited2 = cOMPO.UpdateComponente(HdnIdbeforeCreate.Value, COT_MCOMPONENTES.Campos.KA_WGR(), WGR);

        if (WGR.Contains("B") || WGR.Contains("C") || WGR.Contains("D"))
        {
            cOMPO.UpdateComponente(HdnIdbeforeCreate.Value, "IsGlass", true);
        }

        if (IsEdited)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('El componente ha sido vinculado con el código " + _CodigoAlfak + " de Alfak'); window.location='" +
       Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID=" + HdnIdbeforeCreate.Value + "&TOKEN=" + TOKEN ) + "';", true);
        }
        else
        {
           ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de vincular el proceso con Alfak, por favor intentelo nuevamente.');", true);
        }
    }
    
}