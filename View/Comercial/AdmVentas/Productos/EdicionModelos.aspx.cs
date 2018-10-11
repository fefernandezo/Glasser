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
                Modelos Model = new Modelos();
                bool IsModel = Model.IsModel(ID, true, TOKEN); 
                HdnIdModel.Value = ID;
                if (IsModel)
                {
                    //Llena el dropdown de las formulas predeterminadas
                    FillDDlFormulasPred(DDLFormulasPred);

                    //fill DDL piezas y copmponentes
                    FillDDLFirstPieceOrComp();

                    Imagen.ImageUrl = Model._modelo.IMAGE;
                    
                    TxtNombre.Text = Model._modelo.NOMBRE;
                    LblCreateDate.Text = Model._modelo.F_Created.ToLongDateString();
                    LblEditDate.Text = Model._modelo.F_Update.ToLongDateString();
                    TxtDescripcion.Text = Model._modelo.DESCRIPCION;
                    
                    FillDDlFamilia();

                    BomModel bomModel = new BomModel(ID, true);
                    FILLtablaBOM(bomModel.Lista);

                    //Fill DDr canales no asignaDOS y tabla


                    

                    CanalDis cdis = new CanalDis();
                    FillTablaCanales(cdis.GetCanalesAsign(ID,true));
                    FillDDL(DDlSelectChanel, "CHANNAME", "ID", "Seleccionar", cdis.GetCanalesNOAsign(ID, true));

                    // Habilita el ddl de la familia
                    if(string.IsNullOrEmpty(Model._modelo.Familia))
                    {
                        DDLFamilia.Enabled = true;
                        DDLCategoria.Enabled = false;
                        LinkBtnFamCat.Text = "Actualizar";
                    }
                    else
                    {
                        DDLFamilia.SelectedValue = Model._modelo.IdFamilia;
                    }
                    //habilita el ddl de categoria
                    if (string.IsNullOrEmpty(Model._modelo.Categoria))
                    {
                        DDLCategoria.Enabled = false;
                        LinkBtnFamCat.Text = "Actualizar";
                        GetListVariables getList = new GetListVariables(6);
                        FillDDlCategoria(getList.Listavariables);

                    }
                    else
                    {
                        GetListVariables Get = new GetListVariables();
                        Get.GetListModelCategory(Model._modelo.IdFamilia);
                        FillDDlCategoria(Get.ListCompCategory);
                        DDLCategoria.SelectedValue = Model._modelo.ID_PASIGFAMCAT.ToString();
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

    protected void LinkBtnNombre_Click(object sender, EventArgs e)
    {
        if (TxtNombre.Enabled)
        {
            TxtNombre.Enabled = false;
            LinkBtnNombre.Text = "Editar";
            //hacer un update a la tabla con el nombre
            Modelos mODEL = new Modelos();
            bool Isinserted = mODEL.UpdateModel(HdnIdModel.Value, Modelos.COT_PTABMODEL.NOMBRE, TxtNombre.Text);
            if (!Isinserted)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar Actualizar la información, intentelo nuevamente.'));</script>", false);
            }
            else
            {
                
            }
        }
        else
        {
            TxtNombre.Enabled = true;
            LinkBtnNombre.Text = "Actualizar";

        }

    }

    protected void LinkBtnFamCat_Click(object sender, EventArgs e)
    {
        bool Isinsert = false;
        if (!DDLCategoria.Enabled)
        {
            DDLCategoria.Enabled = true;
            LinkBtnFamCat.Text = "Actualizar";
        }
        else
        {
            DDLCategoria.Enabled = false;
            LinkBtnFamCat.Text = "Editar";
            Isinsert = true;

        }
        if (!DDLFamilia.Enabled)
        {
            DDLFamilia.Enabled = true;
            LinkBtnFamCat.Text = "Actualizar";
        }
        else
        {
            Isinsert = true;
            DDLFamilia.Enabled = false;
            LinkBtnFamCat.Text = "Editar";
        }

        if (DDLFamilia.SelectedIndex == 0 || DDLCategoria.SelectedIndex == 0)
        {
            Isinsert = false;
            AlertDDl.Visible = true;
            DDLFamilia.Enabled = true;
            DDLCategoria.Enabled = true;
            LinkBtnFamCat.Text = "Actualizar";
        }
        else
        {
            AlertDDl.Visible = false;
        }

        if (Isinsert)
        {
            //hacer el update a la tabla con la familia y la categoria
            Modelos mODEL = new Modelos();
            bool Isinserted = mODEL.UpdateModel(HdnIdModel.Value, Modelos.COT_PTABMODEL.ID_PASIGFAMCAT, DDLCategoria.SelectedValue);
            
            if (!Isinserted)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar Actualizar la información, intentelo nuevamente.'));</script>", false);
            }
        }
    }

    protected void LinkBtnDescripcion_Click(object sender, EventArgs e)
    {
        if (TxtDescripcion.Enabled)
        {
            TxtDescripcion.Enabled = false;
            LinkBtnDescripcion.Text = "Editar";
            //hacer un update a la tabla con la descripcion
            Modelos mODEL = new Modelos();
            bool Isinserted = mODEL.UpdateModel(HdnIdModel.Value, Modelos.COT_PTABMODEL.DESCRIPCION, TxtDescripcion.Text);
            
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
        string path = Path.Combine(Server.MapPath("~/Images/COT_PMODEL/"));

        if (!string.IsNullOrEmpty(FileUpFoto.FileName.ToString()))
        {
            string fileName = folio + FileUpFoto.FileName.ToString();
            string path_foto = "http://" + HttpContext.Current.Request.Url.Authority + "/Images/COT_PMODEL/" + fileName;
            path = path + fileName;
            FileUpFoto.SaveAs(path);

            //hacer un update a la tabla con la foto
            Modelos mODEL = new Modelos();
            bool Isinserted = mODEL.UpdateModel(HdnIdModel.Value, Modelos.COT_PTABMODEL.IMG_PATH, path_foto);
            
            if (!Isinserted)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Error al intentar Actualizar la información, intentelo nuevamente.'));</script>", false);
            }
            else
            {
                Response.Redirect("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN);
            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se seleccionó ningún archivo.'));</script>", false);
        }





    }

    HtmlGenericControl table;
    HtmlGenericControl thead;

    HtmlGenericControl strong;
    HtmlGenericControl tr;
    HtmlGenericControl i;
    

    protected void FILLtablaBOM(List<BomModel.Bom> boms)
    {
        string espacio = "&nbsp;";
        table = new HtmlGenericControl("table")
        {
            ID = "TablaProcesos",

        };
        

        foreach (var item in boms)
        {
            tr = new HtmlGenericControl("tr");
            HtmlGenericControl  td = new HtmlGenericControl("td");
            HtmlGenericControl td0 = new HtmlGenericControl("td");

            tr.Attributes.Add("class", "filas");
            i = new HtmlGenericControl("i");
            HtmlGenericControl tdhDN1 = new HtmlGenericControl("td");
            HtmlGenericControl tdhDN2 = new HtmlGenericControl("td");
            HtmlGenericControl tdFORMULA = new HtmlGenericControl("td");
            tdhDN1.InnerHtml = item.LEVEL;
            tdhDN1.Attributes.Add("style", "display:none;");
            tdhDN2.InnerHtml = item.ID;
            tdhDN2.Attributes.Add("style", "display:none;");
            if (item.LEVEL=="1")
            {
                tdFORMULA.InnerHtml = "";
                tdFORMULA.Attributes.Add("style", "display:none;");
                i.Attributes.Add("class", "fas fa-align-right");
                td.InnerHtml = espacio + item.NOMBRE.ToUpper();
                td.Attributes.Add("class", "font-weight-bold");
                
            }
            else
            {
                double formula;
                BomModel model = new BomModel();
                string[] UnMed = model.GetInfoItem(item.FROM_TAB,item.ID_FROM_TAB);
                string opcionaL = "";
                if (item.OPCIONAL)
                {
                    opcionaL = "(Opcional) ";
                }
                if (double.TryParse(item.FORMULA,out formula))
                {
                    tdFORMULA.InnerHtml =opcionaL+ espacio + formula + " " + UnMed[1] + " ; Costo: " + item.COSTO.ToString("C0", CultureInfo.CurrentCulture) + "/" + UnMed[0];
                }
                else
                {
                    tdFORMULA.InnerHtml =opcionaL+ espacio+" Formula:" + item.FORMULA + " " + UnMed[1] + " ; Costo: " + item.COSTO.ToString("C0", CultureInfo.CurrentCulture) + "/" + UnMed[0];
                }
                
                tdFORMULA.Attributes.Add("class", "text-dark");
                i.Attributes.Add("class", "fas fa-arrow-up ml-3");
                
                td.InnerHtml = espacio + espacio + espacio + item.NOMBRE;
            }
            td0.Controls.Add(i);
            tr.Controls.Add(td0);
            tr.Controls.Add(td);
            tr.Controls.Add(tdFORMULA);
            tr.Controls.Add(tdhDN1);
            tr.Controls.Add(tdhDN2);
            table.Controls.Add(tr);


        }
        divtabla.Controls.Clear();
        divtabla.Controls.Add(table);



    }
    
    protected void FillTablaCanales(List<CanalDis.Canal> canales)
    {
        string espacio = "&nbsp;";
       HtmlGenericControl table2 = new HtmlGenericControl("table")
        {
            ID = "TablaCanales",

        };

        
        
        int cont = 1;

        foreach (var item in canales)
        {

            tr = new HtmlGenericControl("tr");
            tr.Attributes.Add("class", "filasCanal");
            HtmlGenericControl td0 = new HtmlGenericControl("td");
            HtmlGenericControl td1 = new HtmlGenericControl("td");
            HtmlGenericControl td2 = new HtmlGenericControl("td");
            td0.InnerHtml = cont.ToString() +".-" +espacio;
            td1.InnerHtml = espacio + item.CHANNAME;
            td2.Attributes.Add("style", "display:none;");
            td2.InnerHtml = item.ID_Asign;
            tr.Controls.Add(td0);
            tr.Controls.Add(td1);
            tr.Controls.Add(td2);
            table2.Controls.Add(tr);
            cont++;
        }
        divtablaCanales.Controls.Clear();
        divtablaCanales.Controls.Add(table2);

    }
   

    protected void FillDDlFamilia()
    {
        GetListVariables getList = new GetListVariables(5);
        DDLFamilia.DataSource = getList.Listavariables;
        DDLFamilia.DataTextField = "_Name";
        DDLFamilia.DataValueField = "ID";
        DDLFamilia.DataBind();
        DDLFamilia.Items.Insert(0, new ListItem("--Seleccionar Familia--"));
    }

    

    

    protected void FillDDlCategoria(List<Variables> ListCategory)
    {
        FillDDL(DDLCategoria, "_Name", "_IDAsign", "Seleccionar Categoría", ListCategory);
        
    }



   

    protected void DDLFamilia_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!DDLCategoria.Enabled || DDLFamilia.SelectedIndex!=0)
        {
            DDLCategoria.Enabled = true;
            GetListVariables Get = new GetListVariables();
            Get.GetListModelCategory(DDLFamilia.SelectedValue);
            FillDDlCategoria(Get.ListCompCategory);
            
        }
        else
        {
            DDLCategoria.Enabled = false;
        }
    }

  

    
    protected void FillDDLFirstPieceOrComp()
    {
        DDLFirstPieceOrComp.Items.Insert(0, new ListItem("--Seleccionar--"));
        DDLFirstPieceOrComp.Items.Insert(1, new ListItem("Agregar Pieza"));
        DDLFirstPieceOrComp.Items.Insert(2, new ListItem("Agregar Componente"));
        DDLFirstPieceOrComp.Items.Insert(3, new ListItem("Agregar Proceso"));
        DDLFirstPieceOrComp.DataBind();
    }

    protected void FillDDL(DropDownList DDL, string TextField, string ValueField, string FirstItem, object _Datasource)
    {
        DDL.DataSource = _Datasource;
        DDL.DataTextField = TextField;
        DDL.DataValueField = ValueField;
        DDL.DataBind();
        DDL.Items.Insert(0, new ListItem("--" + FirstItem + "--"));

    }


    protected void DDLFirstPieceOrComp_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DDLFirstPieceOrComp_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (DDLFirstPieceOrComp.SelectedIndex!=0)
        {
            if (DDLFirstPieceOrComp.SelectedIndex==1)
            {
                PanelDivPiezas.Visible = true;
                PanelDivCompo.Visible = false;
                DDLFirstDivCompo.Visible = false;
                PanelDivProc.Visible = false;

            }
            else if (DDLFirstPieceOrComp.SelectedIndex==2)
            {
                PanelDivPiezas.Visible = false;
                PanelDivCompo.Visible = true;
                DDLFirstDivCompo.Visible = true;
                PanelDivProc.Visible = false;
                BomModel bom = new BomModel(HdnIdModel.Value, true, "1");
                FillDDL(DDLFirstDivCompo, "NOMBRE", "ID", "--Seleccionar--",bom.Lista);
                PanelSeleccionCompo.Visible = false;


            }
            else if (DDLFirstPieceOrComp.SelectedIndex==3)
            {
                PanelDivPiezas.Visible = false;
                PanelDivCompo.Visible = false;
                DDLFirstDivCompo2.Visible = true;
                BomModel bom = new BomModel(HdnIdModel.Value, true, "1");
                FillDDL(DDLFirstDivCompo2, "NOMBRE", "ID", "--Seleccionar--", bom.Lista);
                PanelDivProc.Visible = true;


            }
           
        }
        else
        {
            PanelDivPiezas.Visible = false;
            PanelDivCompo.Visible = false;
            PanelDivProc.Visible = false;

        }
    }

   

    protected void LinktoAsocAlfak_Click(object sender, EventArgs e)
    {
        PnlLinktoAsocAlfak.Visible = true;
        PnlTxtSelectedCode.Visible = false;
        FamiliasAlfak alfak = new FamiliasAlfak();
        List<FamiliasAlfak.Familia> familia = alfak.GetLbl3Familia("W1");
        FillDDL(DDLSubTipoProcAsocAlfak, "Name", "ID", "--Seleccionar tipo pieza--",familia);

    }

    protected void LinkSelectAlfak_Click(object sender, EventArgs e)
    {
        PnlLinktoAsocAlfak.Visible = false;
        PnlTxtSelectedCode.Visible = true;
        TxtSelectedAlfakCode.Text = ((LinkButton)sender).Text;
    }

    protected void TxtSearchAlfak_TextChanged(object sender, EventArgs e)
    {
        ProductoAlfak productos = new ProductoAlfak(DDLSubTipoProcAsocAlfak.SelectedValue, TxtSearchAlfak.Text);
        GrdListProcAlfak.DataSource = productos.Listaproductos;
        GrdListProcAlfak.DataBind();
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



    protected void BtnAddPieces_Click(object sender, EventArgs e)
    {
        BomModel model = new BomModel();
        bool IsInserted = false;
        BomModel.Bom bom;
        if (DDLFirstPieceOrComp.SelectedIndex==1)
        {
            double ancho;
            double largo;
            string error;
            if (double.TryParse(TxtMdlAPAnchoPiece.Text, out ancho))
            {
                if (double.TryParse(TxtMdlAPLargoPiece.Text, out largo))
                {
                    
                    bom = new BomModel.Bom
                    {
                        NOMBRE = TxtMdlAPNamePiece.Text,
                        ANCHO = ancho,
                        LARGO = largo,
                        ALFAK_CODE = TxtSelectedAlfakCode.Text,
                        CANT_PRED=0,
                        COSTO=0,
                        NODE= model.GetPos_Nr(HdnIdModel.Value, true, "1"),
                        ID_MODEL=HdnIdModel.Value,
                        LEVEL="1",
                        FROM_TAB="",
                        ID_FROM_TAB="0",
                        ESTADO=true,
                        POS_NR= model.GetPos_Nr(HdnIdModel.Value, true, "1"),
                        FORMULA ="",
                    };
                    IsInserted= model.InsertBom(bom);
                }
                else
                {
                    error = "Formato de Alto incorrecto";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error de formato: " + error + "'); window.location='" +
       Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);
                }
                
            }
            else
            {
                error = " Formato de Ancho incorrecto";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error de formato: " + error + "'); window.location='" +
       Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);
            }
            
        }
        else if (DDLFirstPieceOrComp.SelectedIndex==2)
        {
            
                ComponentesClass Item = new ComponentesClass(HdnIdSelectedCompo.Value, true);
            int IDItemBom = Convert.ToInt32(DDLFirstDivCompo.SelectedValue);

            BomModel ItemBom = new BomModel(IDItemBom,true);
            
            bom = new BomModel.Bom
            {
                NOMBRE = Item._Detalle.Nombre,
                ALFAK_CODE = Item._Detalle.COD_ALFAK,
                ANCHO = 0,
                LARGO = 0,
                FORMULA = TxtFormulaCompo.Text,
                CANT_PRED = 0,
                ESTADO = true,
                COSTO = Item._Detalle.PrecioUn,
                FROM_TAB = "COT_MCOMPONENTES",
                ID_FROM_TAB = Item._Detalle._ID,
                ID_MODEL = HdnIdModel.Value,
                LEVEL = "2",
                NODE = ItemBom.ItemBom.POS_NR,
                POS_NR = ItemBom.GetPos_Nr(HdnIdModel.Value, true, "2",ItemBom.ItemBom.POS_NR),
                OPCIONAL = ChkIsOpcional.Checked,
                
                
            };

            IsInserted=model.InsertBom(bom);


        }
        else if (DDLFirstPieceOrComp.SelectedIndex==3)
        {
            ProcesosClass ProcClass = new ProcesosClass();
            ProcesosClass._Proceso proceso = ProcClass.GetProceso(DDLTipoProcAsocAlfak.SelectedValue, true);
            int IDItemBom = Convert.ToInt32(DDLFirstDivCompo2.SelectedValue);
            BomModel ItemBom = new BomModel(IDItemBom, true);
            bom = new BomModel.Bom {
                NOMBRE = proceso.Nombre,
                ALFAK_CODE = proceso._CodAlfak,
                ANCHO = 0,
                LARGO = 0,
                FORMULA = TxtPnlProcSelectedFormula.Text,
                CANT_PRED = 0,
                ESTADO = true,
                COSTO = proceso.Costo_Unit,
                FROM_TAB = "COT_MPROCESOS",
                ID_FROM_TAB = proceso._ID,
                ID_MODEL = HdnIdModel.Value,
                LEVEL = "2",
                NODE = ItemBom.ItemBom.POS_NR,
                POS_NR = ItemBom.GetPos_Nr(HdnIdModel.Value, true, "2", ItemBom.ItemBom.POS_NR),
                OPCIONAL=ChkOpcionalProc.Checked,
            };

            IsInserted = model.InsertBom(bom);

        }
        else 
        {
            bom = new BomModel.Bom ();
        }
        
         

        if (IsInserted)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Asignación Completa.'); window.location='" +
       Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error en la asignación, por favor intentelo nuevamente.'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);
        }
    }

    protected void DDLFirstDivCompo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLFirstDivCompo.SelectedIndex!=0)
        {
            PanelSeleccionCompo.Visible = true;
            FamiliasAlfak alfak = new FamiliasAlfak();
            List<FamiliasAlfak.Familia> familias = alfak.GetLbl3Familia("W2*");
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
            FillDDL(DDLSelcTipoCompo, "Name", "ID", "Seleccionar Tipo", familias);

        }
        else
        {
            PanelSeleccionCompo.Visible = false;
        }

    }

    protected void DDLSelcTipoCompo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLSelcTipoCompo.SelectedIndex!=0)
        {
            PnlCompoSelected.Visible = false;
            GrdCompo.Visible = true;
            TxtSearchCompo.Visible = true;
            BtnSearchCompo.Visible = true;
            BomModel model = new BomModel();
            
            GrdCompo.DataSource = model.GetCompoByWGR(DDLSelcTipoCompo.SelectedValue, true);
            GrdCompo.DataBind();

        }
        else
        {
            GrdCompo.Visible = false;
            TxtSearchCompo.Visible = false;
            BtnSearchCompo.Visible = false;
        }

    }

    protected void TxtSearchCompo_TextChanged(object sender, EventArgs e)
    {
        _GetListComponentes _GetList = new _GetListComponentes(TxtSearchCompo.Text, DDLSelcTipoCompo.SelectedValue, true);
        GrdCompo.DataSource = _GetList.ListComponentes;
        GrdCompo.DataBind();
    }

    protected void LinkSelectCompo_Click(object sender, EventArgs e)
    {
        string ID = ((LinkButton)sender).CssClass;
        ComponentesClass CsC = new ComponentesClass(ID, true);
        if (!string.IsNullOrEmpty(CsC._Detalle._ID))
        {
            PnlCompoSelected.Visible = true;
            GrdCompo.Visible = false;
            TxtSearchCompo.Visible = false;
            BtnSearchCompo.Visible = false;
            LblResCodCompo.Text = CsC._Detalle.Nombre;
            HdnIdSelectedCompo.Value = CsC._Detalle._ID;
            LblCostoCompo.Text = CsC._Detalle.PrecioUn.ToString("C0", CultureInfo.CurrentCulture) + "/" + CsC._Detalle.UnMedSimbolo ;
            if (CsC._Detalle.Path_Photo.Length >1)
            {
                HtmlGenericControl img = new HtmlGenericControl("img");
                img.Attributes.Add("class", "img-thumbnail rounded mx-auto d-block ");
                img.Attributes.Add("alt", "Responsive image");
                img.Attributes.Add("src", CsC._Detalle.Path_Photo);
                PnlCompoDivImage.Controls.Add(img);
            }
            Encriptacion token = new Encriptacion(CsC._Detalle.TokeId);
            HtmlGenericControl a = new HtmlGenericControl("a");
            a.Attributes.Add("href", "http://" + HttpContext.Current.Request.Url.Authority + "/View/Comercial/AdmVentas/Productos/EdicionComponente.aspx?ID="
                + ID + "&TOKEN=" + token.TokenEncriptado);
            a.Attributes.Add("target", "_blank");
            a.Attributes.Add("rel", "noopener noreferrer");
            a.InnerHtml = "Ver más detalles";

            
            PnlCompoSelDivHasProc.Controls.Add(a);

            if (CsC._Detalle.HasProc)
            {
                LblHasProcCompo.Text = "Este componente tiene procesos asociados.";
                LblHasProcCompo.Font.Bold = true;
                
            }
            HtmlGenericControl i = new HtmlGenericControl("a");
            HtmlGenericControl span = new HtmlGenericControl("label");
            HtmlGenericControl label = new HtmlGenericControl("label");
            



            if (CsC._Detalle.UnMedSimbolo=="Un")
            {
                i.Attributes.Add("data-content", "Ingresar números enteros");
                

            }
            label.InnerHtml = "Rendimiento o Ecuación de cálculo: &nbsp;";
            span.InnerHtml = CsC._Detalle.UnidadMedida + " por pieza \"" + DDLFirstDivCompo.SelectedItem + "\"";
            span.Attributes.Add("class", "h6");

            DivPreInfoFormula.Controls.Add(label);
            DivInfoFormula.Controls.Add(span);
        }
        
    }

    protected void BtnEliminarItem_Click(object sender, EventArgs e)
    {
        BomModel bom = new BomModel(Convert.ToInt32(HdnMdlDelIdItem.Value),true);
        bool IsDelete;
        if (HdnMdlDelLevel.Value=="1")
        {
            IsDelete = bom.DeleteNode(HdnIdModel.Value, bom.ItemBom.NODE, false);
        }
        else
        {
            IsDelete = bom.DeleteItem(HdnIdModel.Value, HdnMdlDelIdItem.Value, false);
        }


        if (IsDelete)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('La Eliminación fue exitosa.'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de eliminar, por favor intentelo nuevamente.'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);

        }
    }

   
    protected void BtnAddCanal_Click(object sender, EventArgs e)
    {
        CanalDis dis = new CanalDis();
        bool IsAsign = dis.AsigModeltoChann(DDlSelectChanel.SelectedValue, HdnIdModel.Value, true);

        if (IsAsign)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se asignó correctamente el canal seleccionado.'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar asignar el canal seleccionado, por favor intentelo nuevamente.'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);
        }

    }

    protected void BtnMdlDeleteCanal_Click(object sender, EventArgs e)
    {
        CanalDis dis = new CanalDis();
        bool IsDelete = dis.DeleteAsign(HdnMdlDeleteCanalID.Value);

        if (IsDelete)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se ha eliminado el canal'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Se produjo un error al tratar de eliminar el canal, por favor intentelo nuevamente.'); window.location='" +
        Page.ResolveUrl("~/View/Comercial/AdmVentas/Productos/EdicionModelos.aspx?ID=" + HdnIdModel.Value + "&TOKEN=" + TOKEN) + "';", true);

        }

    }

    protected void DDLFormulasPred_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidationDDlFormulasPred(DDLFormulasPred, TxtFormulaCompo);
        

    }

    private void FillDDlFormulasPred( DropDownList DDL)
    {

        DDL.Items.Clear();
        DDL.Items.Insert(0, new ListItem("--Seleccionar formula--"));
        DDL.Items.Insert(1, new ListItem("Área"));
        DDL.Items.Insert(2, new ListItem("Perímetro"));

    }

    protected void DDLFirstDivCompo2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLFirstDivCompo2.SelectedIndex != 0)
        {
            PanelSelectProceso.Visible = true;
            ProcesosClass procesos = new ProcesosClass(true);
            
            FillDDL(DDLTipoProcAsocAlfak, "Nombre", "_ID", "Seleccionar Proceso", procesos.Procesos);

        }
        else
        {
            PanelSelectProceso.Visible = false;
        }

    }

    protected void DDLTipoProcAsocAlfak_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLTipoProcAsocAlfak.SelectedIndex!=0)
        {
            ProcesosClass ProcClass = new ProcesosClass();
            ProcesosClass._Proceso proceso = ProcClass.GetProceso(DDLTipoProcAsocAlfak.SelectedValue,true);
            PanelProcesoSelected.Visible = true;
            FillDDlFormulasPred(DDLPnlProcSelectedFormula);
            LblPnlProcSelectedCosto.Text = "El costo de este proceso es de " + proceso.Costo_Unit.ToString("C0", CultureInfo.CurrentCulture) + "/" + proceso._MagSimbolo;
            LblPnlProcSelectedCosto.Font.Bold = true;
            LblPnlProcSelectedMerma.Text = "con merma de " + proceso.Merma*100 + "% por " + proceso._MagSimbolo;
        }
        else
        {
            PanelProcesoSelected.Visible = false;
            LblPnlProcSelectedCosto.Text = "";
        }
    }

    private void ValidationDDlFormulasPred(DropDownList DDL, TextBox Txt)
    {
        if (DDL.SelectedIndex == 0)
        {

        }
        else if (DDL.SelectedIndex == 1)
        {
            Txt.Text = "(ANCHO/1000)*(ALTO/1000)";
        }
        else if (DDL.SelectedIndex == 2)
        {
            Txt.Text = "((ANCHO/1000)+(ALTO/1000))*2";
        }
        else
        {

        }

    }

    protected void DDLPnlProcSelectedFormula_SelectedIndexChanged(object sender, EventArgs e)
    {

        ValidationDDlFormulasPred(DDLPnlProcSelectedFormula,TxtPnlProcSelectedFormula);
        
    }

    protected void ValidarFormulacompo_Click(object sender, EventArgs e)
    {
        CalcEcuacion ecuacion = new CalcEcuacion(TxtFormulaCompo.Text,1300,1800);

        if (ecuacion.Resultado>0)
        {
            LblValidadorFormulaCompo.Text = "Ancho: 1300, Alto:1800 Resultado:" + ecuacion.Resultado;
        }
        else
        {
            LblValidadorFormulaCompo.Text = "Error en la ecuación o número ingresado";
        }
        
    }

    protected void Validadorformulaproceso_Click(object sender, EventArgs e)
    {
        CalcEcuacion ecuacion = new CalcEcuacion(TxtPnlProcSelectedFormula.Text, 1300, 1800);

        if (ecuacion.Resultado > 0)
        {
            LblValidadorFormulaproc.Text = "Ancho: 1300, Alto:1800 Resultado:" + ecuacion.Resultado;
        }
        else
        {
            LblValidadorFormulaproc.Text = "Error en la ecuación o número ingresado";
        }

    }
}