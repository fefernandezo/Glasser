using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logistica;

public partial class _AdmRutas : Page
{
    LogClass logClass;
    
    string IdAsignRuta;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       string IDInv = Request.QueryString["ID"];
        string Token = Request.QueryString["TOKEN"];
        
        TokenClass token = new TokenClass();
        if(!IsPostBack)
        {
            if (Token == token.TokenId)
            {

                logClass = new LogClass();
                Inventario inventario = logClass.GetVariables(IDInv);
                List<UsuariosInv> usuarios;
                List<Ruta> rutasP_Asignar;
                List<Sucursal> sucursals;

                if (inventario.Status == "0")
                {
                    //por aquí va la cosa
                    HdnIdInventario.Value = IDInv;
                    //obtener lista sucursales
                    sucursals = logClass.GetListSucursal();
                    var fff = (from a in sucursals where a._KOSU == inventario.KOSU select a).First();
                    PanelGral.Visible = true;
                    HdnKOSU.Value = inventario.KOSU;
                    LblInventName.Text = "\"" + inventario.Name + ", Sucursal " + fff._Name + "\"";
                    
                    rutasP_Asignar = logClass.GetListRutasxAsign("ALL", IDInv);
                    
                    ChargeRutas_P_Asignar(rutasP_Asignar, false);
                    usuarios = logClass.GetListUsuariosInv();
                    ChargeUsuarios(usuarios);


                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('El inventario al cual está tratando de ingresar no se encuentra disponible');});</script>", false);
                }


            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('Error al tratar de ingresar a la página');});</script>", false);
            }
        }
        

    }

    protected void ChargeUsuarios(List<UsuariosInv> ruta)
    {
        DDListUsuario.DataSource = ruta;
        DDListUsuario.DataTextField = "_Name";
        DDListUsuario.DataValueField = "_Id";
        DDListUsuario.DataBind();
        DDListUsuario.Items.Insert(0, new ListItem("---Seleccionar Usuario---"));
    }

    protected void ChargeRutas_P_Asignar(List<Ruta> rutas, bool BtnAsig)
    {
        if (rutas.Count > 0)
        {
            BtnAsigRutas.Enabled = BtnAsig;
            LblGrdRutas.Visible = false;
            
        }
        else
        {
            BtnAsigRutas.Enabled = false;
            LblGrdRutas.Text = "El usuario seleccionado tiene todas las rutas del inventario asignadas";
            
            LblGrdRutas.Visible = true;

        }
        GridAsingRutas.DataSource = rutas;
        GridAsingRutas.DataBind();

    }

    protected void DDListUsuario_SelectedIndexChanged(object sender, EventArgs e)
    {
        logClass = new LogClass();
        List<Ruta> rutas = new List<Ruta>();
        bool btnAsig;
        if (DDListUsuario.SelectedIndex==0)
        {
            rutas = logClass.GetListRutasxAsign("ALL", HdnIdInventario.Value);
            btnAsig = false;
        }
        else
        {
            rutas = logClass.GetListRutasxAsign(DDListUsuario.SelectedValue, HdnIdInventario.Value);
            btnAsig = true;
        }
        
         
        ChargeRutas_P_Asignar(rutas, btnAsig);
    }

    protected void BtnAsigRutas_Click(object sender, EventArgs e)
    {
       logClass = new LogClass();

        if (GridAsingRutas.Rows.Count>0)
        {
            int y = 0;
            foreach (GridViewRow row in GridAsingRutas.Rows)
            {
                var checkbox = row.FindControl("CheckBAsigRutas") as CheckBox;
                if (checkbox.Checked)
                {
                    y++;
                }

            }
            if(y>0)
            {
                foreach (GridViewRow row in GridAsingRutas.Rows)
                {
                    var checkbox = row.FindControl("CheckBAsigRutas") as CheckBox;
                    if (checkbox.Checked)
                    {
                        string IdAsignRuta = GridAsingRutas.DataKeys[row.RowIndex].Value.ToString();
                        bool Insert = logClass.InsertAsignoperario(IdAsignRuta, DDListUsuario.SelectedValue);
                    }

                }

                TokenClass token = new TokenClass();
                Response.Redirect("~/View/Logistica/AdministracionUsuarios?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se ha seleccionado ninguna ruta'));</script>", false);
            }
            
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('No se ha seleccionado ruta'));</script>", false);
        }
        
        
    }

    protected void LinkToAsignRuta_Click(object sender, EventArgs e)
    {
        TokenClass token = new TokenClass();
        Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
    }

    protected void BtnDeleteRuta_Click(object sender, EventArgs e)
    {
        TokenClass token = new TokenClass();
        logClass = new LogClass();
        //logClass.AnularRutaAsign( HdnIdAsignruta.Value,token.TokenId);
        Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
    }

    protected void BtnDetailRuta_Click(object sender, EventArgs e)
    {

    }

    protected void BtnModalDeleteUseronRuta_Click(object sender, EventArgs e)
    {

        string confirmValor = Request.Form["confirm_value"];
        if (confirmValor == "si")
        {
            TokenClass token = new TokenClass();
            logClass = new LogClass();

            if (logClass.AnularAsignUser(HdnIdAsignUser.Value, token.TokenId))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Se ha eliminado con éxito'));</script>", false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Hubo un error, favor intentelo más tarde.'));</script>", false);
            }
        }

        
        

    }
}