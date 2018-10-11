using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Agregar_Cliente : System.Web.UI.Page
{
    string FromAsigroles;
    string Usuario;
    string Role;
    protected void Page_Load(object sender, EventArgs e)
    {
        FromAsigroles = Request.QueryString["FromAsigroles"];
        Usuario = Request.QueryString["User"];
        Role = Request.QueryString["role"];

        if (!IsPostBack)
        {
            
            DataTable tabla = new DataTable();
            int y = 0;
            for (int i = 10; i < 50; i++)
            {
                DDlistMargen.Items.Insert(y, new ListItem( i.ToString() + "%" , "1," + i.ToString() ));
                y++;
            }

            DDlistMargen.Items.Insert(0, new ListItem("Seleccionar", "0"));
            DistPoliticaChile.GetRegiones GetReg = new DistPoliticaChile.GetRegiones();
            FillDDL(DDlRegion, "Nombre", "codigo", "Seleccionar región", GetReg.Regiones);
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

    protected void BtnseachCli_Click(object sender, EventArgs e)
    {
        Administrador adm = new Administrador();
        DataTable ResultadoSearch = adm.ClientesRandom(TxtSearch.Text);
        fillTablaClientes(adm.ClientesRandom(TxtSearch.Text));
        
    }


    protected void fillTablaClientes(DataTable datos)
    {
        HtmlGenericControl table = new HtmlGenericControl("table") { ID = "DirTable" };
        table.Attributes.Add("class", "table table-hover");
        /*crear encabezado*/
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl strong = new HtmlGenericControl("strong");
        HtmlGenericControl trhead = new HtmlGenericControl("tr");
        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        HtmlGenericControl tr;

        HtmlGenericControl th;

        /*Rut*/
        th = new HtmlGenericControl("th") { InnerHtml = "RUT" };
        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        /*Nombre*/
        th = new HtmlGenericControl("th") { InnerHtml = "Nombre" };
        trhead.Controls.Add(th);
        thead.Controls.Add(trhead);
        
        /*agrega el thead*/
        table.Controls.Add(thead);

        foreach (DataRow dr in datos.Rows)
        {
            tr = new HtmlGenericControl("tr");
            HtmlGenericControl td;

            /*Rut*/
            td = new HtmlGenericControl("td") { InnerHtml = dr["RUT"].ToString() };
            tr.Controls.Add(td);
            /*Nombre*/
            td = new HtmlGenericControl("td") { InnerHtml = dr["CLIENTE"].ToString() };
            tr.Controls.Add(td);
            /*Dir*/
            td = new HtmlGenericControl("td") { InnerHtml = dr["DIR"].ToString() };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            /*CodReg*/
            
            td = new HtmlGenericControl("td") { InnerHtml = Convert.ToInt32(dr["REG"].ToString()).ToString() };
            td.Attributes.Add("style", "display:none;");
            tr.Controls.Add(td);

            tbody.Controls.Add(tr);
        }
        table.Controls.Add(tbody);
        Grdclientes.Controls.Clear();
        Grdclientes.Controls.Add(table);

    }
   



    protected void Ingresar_Click(object sender, EventArgs e)
    {
        Administrador adm = new Administrador();
        double factor = Convert.ToDouble(DDlistMargen.SelectedValue);
        if (adm.CreacionCliente(Hddnombre.Value.Trim(), hddrut.Value.Trim(), factor, Direccion.Text, DDlRegion.SelectedValue, DDlComuna.SelectedValue, ChkDVHM2.Checked, DDLTratodias.SelectedValue))
        {
            //redirecciona
            if (FromAsigroles == "1")
            {
                Response.Redirect("~/Administrador/Registro-Usuarios/Asignar-Roles.aspx?Usuario=" + Usuario + "&role=" + Role);

            }
            else
            {
                Response.Redirect("~/Administrador/");
            }
        }
        else
        {

        }
        
       
        
        
    }

    protected void DDlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DistPoliticaChile.GetComunas GetComunas = new DistPoliticaChile.GetComunas(DDlRegion.SelectedValue);
        FillDDL(DDlComuna, "Nombre", "codigo", "Seleccionar comuna", GetComunas.Comunas);

    }
}