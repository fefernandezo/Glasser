using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comercial;

public partial class _Creacion_Variables : Page
{
    
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Convert.ToInt32(Request.QueryString["ID"]);
        string Token = Request.QueryString["Request"];
        Encriptacion encriptacion = new Encriptacion(Token);
        string TokenDes = encriptacion.TokenDesencriptado;

        if (!IsPostBack)
        {
            if (TokenDes == "Paseparairacreacionvar")
            {
                HdnId.Value = ID.ToString();
                if(ID==1)
                {
                    Lbltitle.Text = "Familias para los componentes y materiales";
                    LblTitle1.Text = "Familias";

                }
                else if (ID==2)
                {
                    Lbltitle.Text = "Categorías para los componentes y materiales";
                    LblTitle1.Text = "Categorías";
                }
                else if (ID==3)
                {
                    Lbltitle.Text = "Colores para los componentes y materiales";
                    LblTitle1.Text = "Colores";
                }
                else if (ID==4)
                {
                    MTABMAcontrols.Visible = true;
                    Lbltitle.Text = "Marcas para los componentes y materiales";
                    LblTitle1.Text = "Marcas";
                }
                else if (ID == 5)
                {
                    Lbltitle.Text = "Familias de productos";
                    LblTitle1.Text = "Familias";
                }
                else if (ID == 6)
                {
                    Lbltitle.Text = "Categorías de productos";
                    LblTitle1.Text = "Categorías";
                }
                else if (ID == 7)
                {
                    Lbltitle.Text = "Canales de Distribución";
                    LblTitle1.Text = "Canales";
                }


            }
        }

    }


    protected void BtnCrear_Click(object sender, EventArgs e)
    {
        Variables variables = new Variables {
            _Name = TxtNombre.Text,
            _Descrition=TxtDescripcion.Text,
            _estado=true,
            _CssClass="",
            _Procedencia=TxtProcedencia.Text,
            
        };
        SQLQueryVariables insert = new SQLQueryVariables(HdnId.Value);
        bool IsInsert = insert.InsertVar(variables);

        if (IsInsert)
        {
            if (HdnId.Value=="1" || HdnId.Value == "2")
            {
                Redireccionamiento("~/View/Comercial/AdmVentas/Productos/Variables/AdministracionVariables.aspx?ID=", HdnId.Value, "Paseparairaadmvariables");
            }
            else
            {
                Redireccionamiento("~/View/Comercial/AdmVentas/Productos/Variables/AdministracionVariables.aspx?ID=", HdnId.Value, "Paseparairaadmvariables");
                //Response.Redirect("~/View/Comercial/AdmVentas/Productos/Variables/");
            }
            
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(alert('Hubo un error al intentar crear la variables, intentelo nuevamente.'));</script>", false);
        }
    }

    private void Redireccionamiento(string url, string valor, string pase)
    {
        Encriptacion encriptacion = new Encriptacion(pase);
        string Token = encriptacion.TokenEncriptado;

        Response.Redirect(url + valor + "&Request=" + Token);
    }
}