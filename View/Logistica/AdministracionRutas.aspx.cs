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
    string RutaName;
    string IdAsignRuta;
    string IdInvent;

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
                List<Bodega> bodegas;
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
                    
                    rutasP_Asignar = logClass.GetListRuta("ALL", IDInv, inventario.KOSU);
                    ChargeRutas_P_Asignar(rutasP_Asignar);
                    bodegas = logClass.GetListBodega(inventario.KOSU);
                    ChargeBodegas(bodegas);


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

    protected void ChargeBodegas(List<Bodega> bodegas)
    {
        DDListBodega.DataSource = bodegas;
        DDListBodega.DataTextField = "COMBINADO";
        DDListBodega.DataValueField = "_KOBO";
        DDListBodega.DataBind();
        DDListBodega.Items.Insert(0, new ListItem("Todas las Bodegas de la sucursal"));
    }

    protected void ChargeRutas_P_Asignar(List<Ruta> rutas)
    {
        if (rutas.Count > 0)
        {
            BtnAsigRutas.Enabled = true;
            LblGrdRutas.Visible = false;
            
        }
        else
        {
            BtnAsigRutas.Enabled = false;
            LblGrdRutas.Text = "La bodega seleccionada no tiene Rutas";
            
            LblGrdRutas.Visible = true;

        }
        GridAsingRutas.DataSource = rutas;
        GridAsingRutas.DataBind();

    }

    protected void DDListBodega_SelectedIndexChanged(object sender, EventArgs e)
    {
        logClass = new LogClass();
        List<Ruta> rutas = new List<Ruta>();
        if (DDListBodega.SelectedIndex==0)
        {
            rutas = logClass.GetListRuta("ALL", HdnIdInventario.Value,HdnKOSU.Value);
        }
        else
        {
            rutas = logClass.GetListRuta(DDListBodega.SelectedValue, HdnIdInventario.Value,null);
        }
        
         
        ChargeRutas_P_Asignar(rutas);
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
                        string IdRuta = GridAsingRutas.DataKeys[row.RowIndex].Value.ToString();
                        bool Insert = logClass.InsertAsignRuta(HdnIdInventario.Value, IdRuta);
                    }

                }

                TokenClass token = new TokenClass();
                Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
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

    protected void LinkToCrearRuta_Click(object sender, EventArgs e)
    {
        TokenClass token = new TokenClass();
        Response.Redirect("~/View/Logistica/CreaciondeRutas?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
    }

    protected void BtnDeleteRuta_Click(object sender, EventArgs e)
    {
        TokenClass token = new TokenClass();
        logClass = new LogClass();
        logClass.AnularRutaAsign( HdnIdAsignruta.Value,token.TokenId);
        Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
    }

    protected void BtnDetailRuta_Click(object sender, EventArgs e)
    {

    }

    protected void BtnModalDeleteRuta_Click(object sender, EventArgs e)
    {
        RutaName = HdnNameruta.Value;
        IdAsignRuta = HdnIdAsignruta.Value;
        IdInvent = HdnIdInventario.Value;
        Mdl_LblMsg.Text = "Está seguro que desea eliminar la Ruta <strong>\"" + RutaName + "\"</strong>?";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsg').modal('show');});</script>", false);
    }
}