using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using Ecommerce;
using System.Threading;


public partial class View_Cliente_IngresoPedidos_Detalle_del_Pedido : System.Web.UI.Page
{
    GetCssClassComponente ClaseA;
    GetCssClassComponente ClaseB;
    GetCssClassComponente ClaseSep;

    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!IsPostBack)
        {
            string IDPedido = Request.QueryString["ID"];
            string TokenId = Request.QueryString["TOKENID"];
            ChargeDDLs();

            if (!string.IsNullOrEmpty(TokenId))
            {
                Encriptacion Token = new Encriptacion(TokenId);
                Pedidotemporal pedido = new Pedidotemporal(IDPedido, Token.DesEncriptado);
                if (pedido.IsTrue)
                {
                    
                    if(pedido.Datos._Estado=="0")
                    {
                        Usuario usuario = new Usuario();

                        if (pedido.Datos._FechaDesp.HasValue)
                        {
                            //si hay fecha de despacho es porque el pedido está valorizado y por lo tanto puede ser enviado
                            BtnEnviarPedido.Enabled = true;
                            BtnIngresoInfDesp.Text = "Modificar Información de despacho";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "EnableInfDesp()", true);
                            PanelInfDespEnabled.Visible = true;
                            PanelTotales.Visible = true;
                            if (!string.IsNullOrEmpty(pedido.Datos._DireccionDesp))
                            {
                                LblDireccion.Text = pedido.Datos._DireccionDesp;
                            }
                            LblFechaDesp.Text = pedido.Datos._FechaDesp.Value.ToShortDateString();
                            LblTipoDesp.Text = pedido.Datos._TipoDespacho;
                        }
                        else
                        {
                            //si no hay fecha de despacho es porque no se ha seleccionado la informacion de despacho
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "AddMsgInfDesp();", true);
                            BtnIngresoInfDesp.Text = "Ingresar Información de despacho";
                            BtnEnviarPedido.CssClass = "btn btn btn-outline-primary";

                            string Function = "";
                            if (pedido.HasDetail)
                            {
                                Function = "GetDetail('" + IDPedido + "','" + TokenId + "');";
                            }
                            else
                            {
                                /*LecturaxlsTP lecturaxlsTP = new LecturaxlsTP(IDPedido, usuario.Info._TipoxlsTP, Token.DesEncriptado);

                                if (lecturaxlsTP.EsValido)
                                {
                                    bool ingresa = pedido.Insert_temp_det_pedido(lecturaxlsTP.Lista,IDPedido);
                                    if (ingresa)
                                    {
                                        Function = "GetDetail('" + IDPedido + "','" + TokenId + "');";
                                    }


                                }
                                else
                                {

                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert(' ERROR(ES):" + string.Join(", ", lecturaxlsTP.ErrorString) + "'));</script>", false);
                                    
                                }
                                */

                            }
                            
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "CallMySecondFunction", Function, true);
                            ChargeGridPedido(pedido.Detalle);


                        }
                        LblNombrePedido.Text = pedido.Datos._OrderName;
                        LblObservaciones.Text = pedido.Datos._Observacion;
                        

                        

                        


                    }
                }
            }



            
        }
        
        

    }

    protected void ChargeGridPedido(List<T_temp_DetPedido> Datos)
    {
        GridDetalle.DataSource = Datos;
        GridDetalle.DataBind();
    }

    protected void ChargeDDLs()
    {
        ListasComponentesDVH Componentes = new ListasComponentesDVH();
        DDLCristalA.DataSource = Componentes.Cristales;
        DDLCristalA.DataTextField = "_Descripcion";
        DDLCristalA.DataValueField = "_ID";
        DDLCristalA.DataBind();
        DDLCristalA.Items.Insert(0, new ListItem("--Seleccionar Cristal Extr--"));

        DDLCristalB.DataSource = Componentes.Cristales;
        DDLCristalB.DataTextField = "_Descripcion";
        DDLCristalB.DataValueField = "_ID";
        DDLCristalB.DataBind();
        DDLCristalB.Items.Insert(0, new ListItem("--Seleccionar Cristal Int--"));

        DDLSeparador.DataSource = Componentes.Separadores;
        DDLSeparador.DataTextField = "_Descripcion";
        DDLSeparador.DataValueField = "_ID";
        DDLSeparador.DataBind();
        DDLSeparador.Items.Insert(0, new ListItem("--Seleccionar Separador--"));


    }


    protected void ChkGasArgon_CheckedChanged(object sender, EventArgs e)
    {
        


        
    }

    protected void DDLCristalA_SelectedIndexChanged(object sender, EventArgs e)
    {
        string A = "";
        string B = "";
        string S = "";
        int gas = 0;
        if (ChkGasArgon.Checked)
        {
            gas = 1;
        }
        else
        {
            gas = 0;
        }

        if (DDLCristalA.SelectedIndex!=0)
        {
            ClaseA = new GetCssClassComponente(DDLCristalA.SelectedValue);
            A = ClaseA.CssClass;

        }
        if (DDLCristalB.SelectedIndex!=0)
        {
            ClaseB = new GetCssClassComponente(DDLCristalB.SelectedValue);
            B = ClaseB.CssClass;
        }
        if(DDLSeparador.SelectedIndex!=0)
        {
            ClaseSep = new GetCssClassComponente(DDLSeparador.SelectedValue);
            S = ClaseSep.CssClass;
        }
        
        
        
        
        SetClass("col-1 cristalA " + A, S, "col-1 cristalB " + B,gas);



    }


    protected void SetClass(string claseA, string ClaseSep, string ClaseB, int Gas)
    {
        string Function = "SetClassDVH('" + claseA + "','" + ClaseSep + "','" + ClaseB + "','" + Gas +"');";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "CallFunctionsep", Function, true);

    }
}