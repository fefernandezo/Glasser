using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logistica;

public partial class _HomeInventario : Page
{
    string IDInv;
    string Token;
    LogClass logClass;
    protected void Page_Load(object sender, EventArgs e)
    {
        IDInv = Request.QueryString["ID"];
        Token = Request.QueryString["TOKEN"];

        if (!IsPostBack)
        {

           
            TokenClass token = new TokenClass();
            logClass = new LogClass();
            List<Bodega> bodegas;
            List<Sucursal> sucursals;
            if (!string.IsNullOrWhiteSpace(IDInv))
            {
                if (Token == token.TokenId)
                {
                    PanelSucInv.Visible = true;
                    PanelDDLSuc.Visible = false;
                    PanelGral.Visible = true;
                    Inventario inventario = logClass.GetVariables(IDInv);
                    sucursals = logClass.GetListSucursal();
                    Panelbodega.Visible = true;
                    var fff = (from a in sucursals where a._KOSU == inventario.KOSU select a).First();
                    LblSucursal.Text = fff._Name;
                    bodegas = logClass.GetListBodega(inventario.KOSU);
                    ChargeBodegas(bodegas);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('Error al tratar de ingresar a la página');});</script>", false);
                }


            }
            else
            {
                sucursals = logClass.GetListSucursal();
                ChargeSucursales(sucursals);
                PanelDDLSuc.Visible = true;
                PanelSucInv.Visible = false;
                PanelGral.Visible = true;
               

            }
        }
        
      
    }

    protected void ChargeBodegas (List<Bodega> bodegas)
    {
        DDListBodega.DataSource = bodegas;
        DDListBodega.DataTextField = "COMBINADO";
        DDListBodega.DataValueField = "_KOBO";
        DDListBodega.DataBind();
        DDListBodega.Items.Insert(0, new ListItem("--Seleccionar Bodega--"));
    }

    protected void ChargeSucursales(List<Sucursal> sucursales)
    {
        DDListSucu.DataSource = sucursales;
        DDListSucu.DataTextField = "COMBINADO";
        DDListSucu.DataValueField = "_KOSU";
        DDListSucu.DataBind();
        DDListSucu.Items.Insert(0, new ListItem("--Seleccionar Sucursal--"));
    }
    protected void BtnCrearRuta_Click(object sender, EventArgs e)
    {
        if(DDListBodega.SelectedIndex==0)
        {
            ErrorDDlistbodega.Visible = true;
            ErrorDDlistbodega.Text = "No se ha seleccionado Bodega";
        }
        else
        {
            LogClass logClass = new LogClass();
            List<Bodega> bodegas;
            bodegas = logClass.GetListBodega("ALL");
            var KOSU = bodegas.Where(it => it._KOBO == DDListBodega.SelectedValue).Select(g => g._KOSU.First());
            var fff = (from a in bodegas where a._KOBO == DDListBodega.SelectedValue select a).First();
            //creacion de ruta exitosa
            Ruta ruta = new Ruta {
                _Nombre = TxtNombreRuta.Text,
                _Descripcion = TxtDescripRuta.Text,
                _CodBodega = DDListBodega.SelectedValue,
                _CodSUCU = fff._KOSU,
            };

            bool IsInsert = logClass.InsertRuta(ruta);
            if(!IsInsert)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('Error al tratar de ingresar la ruta en la base de datos');});</script>", false);
            }
            else
            {
                Mdl_LblMsg.Text = "Creación de Ruta exitosa";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsg').modal('show');});</script>", false);
            }
            


        }
    }

    protected void MdlBtnOkMsg_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(IDInv))
        {
            Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + IDInv + "&TOKEN=" + Token);
        }
        else
        {
            Response.Redirect("~/View/Logistica/CreaciondeRutas.aspx");
        }
    }

    protected void DDListSucu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(DDListSucu.SelectedIndex==0)
        {
            Panelbodega.Visible = false;
        }
        else
        {
            logClass = new LogClass();
            Panelbodega.Visible = true;
            List<Bodega> bodegas;
            bodegas = logClass.GetListBodega(DDListSucu.SelectedValue);
            ChargeBodegas(bodegas);
        }
        
    }
}