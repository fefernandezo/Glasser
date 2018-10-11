using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _AsignarRoles : Page
{
    string usuario;
    string role;
    protected void Page_Load(object sender, EventArgs e)
    {
        usuario = Request.QueryString["Usuario"];
        role = Request.QueryString["role"];
        Administrador adm = new Administrador();

        if (!string.IsNullOrEmpty(usuario))
        {
            
            if (!IsPostBack)
            {
                //BtnIngresar.Visible = false;
                PanelUser1.Visible = true;

                ConsultaUser _Usuario = adm.SelectUser(usuario);
                Userheredado.Text = _Usuario.Nombre + " " + _Usuario.Apellido;
                DataTable roles = adm.RolesList();
                DDListroles.DataSource = roles;
                DDListroles.DataTextField = "Description";
                DDListroles.DataValueField = "RoleId";
                DDListroles.DataBind();
                DDListroles.Items.Insert(0, new ListItem("Seleccionar", "0"));

                DataTable TablaClientes = adm.Clientes();
                DDListcliente.DataSource = TablaClientes;
                DDListcliente.DataTextField = "Empresa";
                DDListcliente.DataValueField = "ID";
                DDListcliente.DataBind();
                DDListcliente.Items.Insert(0, new ListItem("Seleccionar", "0"));

            }
            else
            {
                
               
            }
        }
        else
        {
            if(!IsPostBack)
            {
               // BtnIngresar.Visible = false;
                DataTable roles = adm.RolesList();
                DDListroles.DataSource = roles;
                DDListroles.DataTextField = "Description";
                DDListroles.DataValueField = "RoleId";
                DDListroles.DataBind();
                DDListroles.Items.Insert(0, new ListItem("Seleccionar", "0"));

                PanelUser.Visible = true;
                DataTable Usuarios = adm.ListaUsers();
                DDListUser.DataSource = Usuarios;
                DDListUser.DataTextField = "Usuario";
                DDListUser.DataValueField = "UserName";
                DDListUser.DataBind();
                DDListUser.Items.Insert(0, new ListItem("Seleccionar", "0"));

                DataTable TablaClientes = adm.Clientes();
                DDListcliente.DataSource = TablaClientes;
                DDListcliente.DataTextField = "Empresa";
                DDListcliente.DataValueField = "ID";
                DDListcliente.DataBind();
                DDListcliente.Items.Insert(0, new ListItem("Seleccionar", "0"));

            }
            else
            {
               

                
            }
            


        }

        
        
        
        
    }

    protected void DDListroles_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnIngresar.Visible = true;

        if(DDListroles.SelectedItem.Text=="Cliente Ecommerce")
        {
            PanelCliente.Visible = true;
        }
        else if(DDListroles.SelectedItem.Text == "Seleccionar")
        {
            BtnIngresar.Visible = false;
            PanelCliente.Visible = false;
        }
        else
        {
            PanelCliente.Visible = false;

        }
    }

    protected void CrearCliente_Click(object sender, EventArgs e)
    {
        string role = "";
        if(string.IsNullOrEmpty(usuario))
        {
            usuario = DDListUser.SelectedValue;
        }
        if(DDListroles.SelectedItem.Text=="Seleccionar")
        {
            role = "";
        }
        else
        {
            role = "&role=" + DDListroles.SelectedItem.Text;
        }
        Response.Redirect("~/Administrador/Registro-Usuarios/Agregar-Cliente.aspx?FromAsigroles=1" + "&User=" + usuario + role );
    }

    protected void BtnIngresar_Click(object sender, EventArgs e)
    {
        Administrador adm = new Administrador();
        if (!string.IsNullOrEmpty(usuario))
        {
            if (DDListroles.SelectedItem.Text == "Seleccionar")
            {
                string script = "Debe Seleccionar un Role";
                Mensaje.Text = script;
            }
            else if (DDListroles.SelectedItem.Text == "Cliente Ecommerce")
            {
                if (DDListcliente.SelectedItem.Text == "Seleccionar")
                {
                    string script = "Debe seleccionar el Cliente";
                    Mensaje.Text = script;
                }
                else
                {
                    adm.InsertUserInemp(usuario, DDListcliente.SelectedValue);
                    adm.InsertUserInRoles(usuario, DDListroles.SelectedValue);
                    Response.Redirect("~/Administrador/Registro-Usuarios/");
                }
            }
            else
            {

                adm.InsertUserInRoles(usuario, DDListroles.SelectedValue);
                Response.Redirect("~/Administrador/Registro-Usuarios/");

            }
        }
        else
        {
            if (DDListroles.SelectedItem.Text == "Seleccionar")
            {
                string script = "Debe Seleccionar un Role";
                Mensaje.Text = script;
            }
            else if (DDListroles.SelectedItem.Text == "Cliente Ecommerce")
            {
                if (DDListcliente.SelectedItem.Text == "Seleccionar")
                {
                    string script = "Debe seleccionar el Cliente";
                    Mensaje.Text = script;
                }
                else
                {
                    adm.InsertUserInemp(DDListUser.SelectedValue, DDListcliente.SelectedValue);
                    adm.InsertUserInRoles(DDListUser.SelectedValue, DDListroles.SelectedValue);
                    Response.Redirect("~/Administrador/Registro-Usuarios/");
                }
            }
            else
            {

                adm.InsertUserInRoles(DDListUser.SelectedValue, DDListroles.SelectedValue);
                Response.Redirect("~/Administrador/Registro-Usuarios/");

            }
        }

            
        
        

        
    }
}