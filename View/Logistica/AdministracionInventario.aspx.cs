using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logistica;

public partial class _AdmInventario : Page
{
    string idinventario;
    
    TokenClass token;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            LogClass logClass = new LogClass();
            List<Sucursal> sucursals =logClass.GetListSucursal();
            ChargeSucursales(sucursals);
        }
       
    }

    protected void ChargeSucursales(List<Sucursal> sucursal)
    {
        DDListSucursal.DataSource = sucursal;
        DDListSucursal.DataTextField = "_Name";
        DDListSucursal.DataValueField = "_KOSU";
        DDListSucursal.DataBind();
        DDListSucursal.Items.Insert(0, new ListItem("--Seleccionar Sucursal--"));
    }

    protected void BtnEliminar_Click(object sender, EventArgs e)
    {
        
        string confirmValor = Request.Form["confirm_value"];
        if (confirmValor == "si")
        {
            token = new TokenClass();
            
            idinventario = HdnIdInv.Value;
            LogClass logClass = new LogClass();
            logClass.NullorCloseInventario(idinventario, token.TokenId,"Anular");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('El inventario " + HdnNameInv.Value + " ha sido \"ELIMINADO\" con éxito.');});</script>", false);
        }
        
    }

    protected void BtnDetail_Click(object sender, EventArgs e)
    {
        TokenClass token = new TokenClass();
        Response.Redirect("~/View/Logistica/DetalleInventario?ID=" + HdnIdInv.Value + "&TOKEN=" + token.TokenId);
    }

    

    protected void BtnIngresarInvent_Click(object sender, EventArgs e)
    {
        bool Insertar = true;
        DateTime StartDate = new DateTime();
        DateTime EndDate = new DateTime();
        try
        {
            StartDate = Convert.ToDateTime(TxtStartDate.Text);
            
        }
        catch
        {
            RequiredStartDate.Text = "Formato de fecha incorrecto";
            RequiredStartDate.IsValid = false;
            Insertar = false;
        }

        if(!string.IsNullOrWhiteSpace(TxtEndDate.Text))
        {
            try
            {
                EndDate = Convert.ToDateTime(TxtEndDate.Text);

            }
            catch
            {
                ErrorEndDate.Visible = true;
                ErrorEndDate.Text = "Formato de fecha incorrecto";
                Insertar = false;
            }
        }
        


        if (DDListSucursal.SelectedIndex == 0)
        {
            ErrorDDListSucursal.Visible = true;
            ErrorDDListSucursal.Text = "Debe seleccionar una sucursal";
            Insertar = false;
        }
        else
        {
            
        }

        if(Insertar)
        {

            //inserta en la tabla de inventarios y redirige a la asignacion de rutas
            Inventario inventario = new Inventario {
                Name = TxtNombreInv.Text,
                Description = TxtDescripInv.Text,
                DateCreate = DateTime.Now,
                KOSU=DDListSucursal.SelectedValue,
                Status="0",
                StartTime=StartDate,
                EndTime=EndDate,

            };
            LogClass logClass = new LogClass();

            int IdReturn = logClass.InsertInventario(inventario);

            if(IdReturn>0)
            {
                TokenClass token = new TokenClass();
                Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + IdReturn + "&TOKEN=" + token.TokenId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('Error al tratar de ingresar el inventario en la base de datos');});</script>", false);
            }
            
        }
    }

    

    protected void BtnAdmRutas_Click(object sender, EventArgs e)
    {
        token = new TokenClass();
        Response.Redirect("~/View/Logistica/AdministracionRutas?ID=" + HdnIdInv.Value + "&TOKEN=" + token.TokenId);
    }

    protected void BtnCerrar_Click(object sender, EventArgs e)
    {

        string confirmValor = Request.Form["confirm_value"];
        if (confirmValor == "si")
        {
            token = new TokenClass();
            idinventario = HdnIdInv.Value;
            LogClass logClass = new LogClass();
            logClass.NullorCloseInventario(idinventario, token.TokenId, "Cerrar");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('El inventario " + HdnNameInv.Value + " ha sido \"CERRADO\" con éxito.');});</script>", false);
        }
        
    }

    protected void BtnActivar_Click(object sender, EventArgs e)
    {

        string confirmValor = Request.Form["confirm_value"];
        if (confirmValor == "si")
        {
            token = new TokenClass();
            idinventario = HdnIdInv.Value;
            LogClass logClass = new LogClass();
            logClass.NullorCloseInventario(idinventario, token.TokenId, "Activar");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('El inventario " + HdnNameInv.Value + " ha sido \"ACTIVADO\" con éxito.');});</script>", false);
        }

    }

    protected void BtnAdmUsers_Click(object sender, EventArgs e)
    {
        token = new TokenClass();
        Response.Redirect("~/View/Logistica/AdministracionUsuarios?ID=" + HdnIdInv.Value + "&TOKEN=" + token.TokenId);


    }
}