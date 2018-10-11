using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using nsCliente;

public partial class SiteCliente : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;
    nsCliente.Usuario DUser;
    DatosCliente Cli;

    protected void Page_Init(object sender, EventArgs e)
    {

        // El código siguiente ayuda a proteger frente a ataques XSRF
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Utilizar el token Anti-XSRF de la cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;

        
        
    }
    string rut;
    void master_Page_PreLoad(object sender, EventArgs e)
    {
        Usuario funcion = new Usuario();
        Infousuario usuario = funcion.Info;
        HtmlGenericControl span = new HtmlGenericControl("span")
        {
            InnerHtml = usuario.Nombre
        };
        HtmlGenericControl a;
        var divuser = loginview.FindControl("DivUsuario");
        var DropMenu = loginview.FindControl("DropMenu");
        divuser.Controls.Add(span);

        





        if (!IsPostBack)
        {
            // Establecer token Anti-XSRF
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validar el token Anti-XSRF
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
            }
        }

        rut = Request.QueryString["RUT"];

        Ainicio.Attributes.Clear();
        Ainicio.Attributes.Add("class", "nav-link text-white");
        Ainicio.Attributes.Add("id", "Ainicio");

        if (!string.IsNullOrEmpty(rut))
        {
            
            Ainicio.Attributes.Add("onclick", "Loading();");
            Ainicio.Attributes.Add("href", "~/View/Cliente/Default.aspx?RUT=" + rut);
           
            
        }
        else
        {
            DUser = new nsCliente.Usuario(Page.User.Identity.Name);
            Cli = DUser.InfoEmpresas.First();
            rut = Cli.Rut;
            
        }
        Ainicio.Attributes.Add("onclick", "Loading();");
        Ainicio.Attributes.Add("href", "~/View/Cliente/");
        a = new HtmlGenericControl("a") { InnerHtml="Información de la Empresa" };
        a.Attributes.Add("class", "dropdown-item dropdown-item-nav text-white");
        a.Attributes.Add("href", ResolveUrl("~/View/Cliente/Usuario/Info-empresa/Empresa.aspx") + "?RUT="+rut);
        DropMenu.Controls.Add(a);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        


    }
}