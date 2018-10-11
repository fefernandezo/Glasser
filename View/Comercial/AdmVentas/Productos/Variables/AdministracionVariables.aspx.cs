using Comercial;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _AdmVariablesProd : Page
{
    
    string RutaName;
    string IdAsignRuta;
    string IdInvent;

    protected void Page_Load(object sender, EventArgs e)
    {
       string ID = Request.QueryString["ID"];
        string Token = Request.QueryString["Request"];
        Encriptacion encriptacion = new Encriptacion(Token);
        string TokenDes = encriptacion.TokenDesencriptado;

        if(!IsPostBack)
        {
            List<Variables> Xasignar = new List<Variables>();
            SQLQueryVariables SQL = new SQLQueryVariables(ID);
            
            
            if (TokenDes == "Paseparairaadmvariables")
            {
                
                HdnId.Value = ID.ToString();
                if (SQL._ID == 1)
                {

                    LinktoCrearVar.Text = "Crear \"Familia\" de componentes";
                    Lbltitle.Text = "Asignar Categorías a Familias de componentes y materiales";
                    Lbltitle2.Text = "Categorías por asignar";
                    BtnAsignacion.Text = "Asignar Categoría";
                    LblVariable.Text = "Listado de Familias";
                    LblDDList.Text = "Familia";
                    Lbltitleasignadas.Text = "Categorías Asignadas";
                   

                }
                else if (SQL._ID == 2)
                {

                    LinktoCrearVar.Text = "Crear \"Categoría\" de componentes";
                    Lbltitle.Text = "Asignar colores a Categorías de componentes y materiales";
                    Lbltitle2.Text = "Colores por asignar";
                    BtnAsignacion.Text = "Asignar Color";
                    LblVariable.Text = "Listado de Categorías";
                    LblDDList.Text = "Categoría";
                    Lbltitleasignadas.Text = "Colores Asignados";


                }
                else if (SQL._ID == 3)
                {
                    LinktoCrearVar.Text = "Crear \"Color\" de componentes";
                    Lbltitle.Text = "Asignar colores a Categorías de componentes y materiales";
                    Lbltitle2.Text = "Colores por asignar";
                    BtnAsignacion.Text = "Asignar Color";
                    LblVariable.Text = "Listado de Colores";
                    LblDDList.Text = "Categoría";
                    Lbltitleasignadas.Text = "Colores Asignados";



                }
                else if (SQL._ID == 4)
                {
                    LinktoCrearVar.Text = "Crear \"Marca\" de componentes";
                    LblVariable.Text = "Listado de Marcas";
                    Card2.Visible = false;
                }
                else if (SQL._ID == 5)
                {
                    LinktoCrearVar.Text = "Crear \"Familia\" de productos";
                    Lbltitle.Text = "Asignar Categorías a Familias de productos";
                    Lbltitle2.Text = "Categorías por asignar";
                    BtnAsignacion.Text = "Asignar Categoría";
                    LblVariable.Text = "Listado de Familias";
                    LblDDList.Text = "Familia";
                    Lbltitleasignadas.Text = "Categorías Asignadas";

                }
                else if (SQL._ID == 6)
                {
                    LinktoCrearVar.Text = "Crear \"Categoría\" de productos";
                    Lbltitle.Text = "Asignar Categorías a Familias de productos";
                    Lbltitle2.Text = "Categorías por asignar";
                    BtnAsignacion.Text = "Asignar Categoría";
                    LblVariable.Text = "Listado de Categorías";
                    LblDDList.Text = "Familia";
                    Lbltitleasignadas.Text = "Categorías Asignadas";
                }
                else if (SQL._ID == 7)
                {
                    LinktoCrearVar.Text = "Crear \"Canal de distribución\"";
                    LblVariable.Text = "Canales de distribución";
                    Card2.Visible = false;

                }
                

                HdnListAsign.Value = SQL._CodeAsign;
                GetListVarAsign getList = new GetListVarAsign(SQL._CodeAsign);
                ChargeDDlist(getList.Listavariables);
            }
        }
        
        

        

    }

    

    protected void ChargeDDlist(List<Variables> variables)
    {
        DDlistVariables.DataSource = variables;
        DDlistVariables.DataTextField = "_Name";
        DDlistVariables.DataValueField = "ID";
        DDlistVariables.DataBind();
        DDlistVariables.Items.Insert(0, new ListItem("---Seleccionar----"));
    }

    protected void FillGridviewxAsign(List<Variables> variables)
    {
       

    }



    protected void DDlistVariables_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetListNoAsignadas Noasignadas;
        if (DDlistVariables.SelectedIndex !=0)
        {
            PanelAsignacion.Visible = true;
            Noasignadas = new GetListNoAsignadas(HdnListAsign.Value, DDlistVariables.SelectedValue);
            GridxAsignar.DataSource = Noasignadas.ListaVar;
            GridxAsignar.DataBind();
            string Function = "GetAsignacion('" + HdnListAsign.Value + "','" + DDlistVariables.SelectedValue + "');";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CallFunction", Function, true);

        }
        else
        {
            
            PanelAsignacion.Visible = false;
        }

    }

    protected void BtnVarDetailEliminar_Click(object sender, EventArgs e)
    {
        string _IdTipo = HdnId.Value;
        string IdVariable = HdnIdItem.Value;

        SQLQueryVariables SQL = new SQLQueryVariables(_IdTipo);
        bool Retorno = SQL.DeletetVar(IdVariable);

        if (Retorno)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('La Variable ha sido eliminada.'));</script>", false);
            Encriptacion encriptacion = new Encriptacion("Paseparairaadmvariables");

            Response.Redirect("~/View/Comercial/AdmVentas/Productos/Variables/AdministracionVariables.aspx?ID=" + HdnId.Value + "&Request=" + encriptacion.TokenEncriptado);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Hubo un error al intentar eliminar la variables, intentelo nuevamente.'));</script>", false);
        }

    }

    protected void BtnAsignacion_Click(object sender, EventArgs e)
    {
        if (GridxAsignar.Rows.Count > 0)
        {
            int y = 0;
            foreach (GridViewRow row in GridxAsignar.Rows)
            {
                var checkbox = row.FindControl("CheckBAsigacion") as CheckBox;
                if (checkbox.Checked)
                {
                    y++;
                }

            }
            if (y > 0)
            {
                SQLQueryVariables SQL = new SQLQueryVariables(HdnId.Value);
                bool Insert = false;
                foreach (GridViewRow row in GridxAsignar.Rows)
                {
                    var checkbox = row.FindControl("CheckBAsigacion") as CheckBox;
                    if (checkbox.Checked)
                    {
                        string IdITEM = GridxAsignar.DataKeys[row.RowIndex].Value.ToString();
                        Insert = SQL.InsertAsign(DDlistVariables.SelectedValue,IdITEM);
                        if (!Insert)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se pudo asignar'));</script>", false);
                        }
                    }

                }

                Encriptacion encriptacion = new Encriptacion("Paseparairaadmvariables"); 
                
                Response.Redirect("~/View/Comercial/AdmVentas/Productos/Variables/AdministracionVariables.aspx?ID=" + HdnId.Value + "&Request=" + encriptacion.TokenEncriptado);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se ha seleccionado ningún item'));</script>", false);
            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se ha seleccionado ningún item'));</script>", false);
        }
    }

    protected void LinktoCrearVar_Click(object sender, EventArgs e)
    {
        Encriptacion encriptacion = new Encriptacion("Paseparairacreacionvar");

        Response.Redirect("~/View/Comercial/AdmVentas/Productos/Variables/CreaciondeVariables.aspx?ID=" + HdnId.Value + "&Request=" + encriptacion.TokenEncriptado);

    }
}