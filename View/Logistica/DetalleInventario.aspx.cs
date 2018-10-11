using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logistica;


public partial class _AdmRutas : Page
{
    LogClass logclass;
    TokenClass token;
    protected void Page_Load(object sender, EventArgs e)
    {
        string IdInv = Request.QueryString["ID"];
        HdnIdInventario.Value = IdInv;

        if(!IsPostBack)
        {
           
            
        }

    }

   


    private void ExportarExcel(List<DetalleInv> detalleInvs, string InvName)
    {
        DetalleInvExcel Excel = new DetalleInvExcel();
        Response.ClearContent();
        Response.BinaryWrite(Excel.GenerateExcel(detalleInvs,InvName));
        Response.AddHeader("content-disposition", "attachment; filename=Inventario " + InvName + ".xlsx");
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Flush();
        Response.End();
        
    }

    protected void BtnModalDeleteItem_Click(object sender, EventArgs e)
    {
        token = new TokenClass();
        logclass = new LogClass();
        logclass.AnularItemInv(HdnIdItem.Value,token.TokenId);
        string funcion = "LoadDetail(" + HdnIdInventario.Value + "," + HdnPagina.Value + ");";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "mostrarPagina",funcion, true);

    }

    protected void BtnDownLoadExcel_Click(object sender, EventArgs e)
    {
        token = new TokenClass();
        logclass = new LogClass();
        Inventario inventario = logclass.GetVariables(HdnIdInventario.Value);
        ExportarExcel(logclass.GetDetailInventory(HdnIdInventario.Value,token.TokenId),inventario.Name);
        Response.Redirect("~/View/Logistica/DetalleInventario?ID=" + HdnIdInventario.Value + "&TOKEN=" + token.TokenId);
    }

   
}