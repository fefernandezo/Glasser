using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class SiteLog : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;
    Mensaje msj;
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

    void master_Page_PreLoad(object sender, EventArgs e)
    {
        Usuario funcion = new Usuario();
        Infousuario usuario = funcion.Info;
        HtmlGenericControl span = new HtmlGenericControl("span");
        span.InnerHtml = usuario.Nombre;
        var divuser = loginview.FindControl("DivUsuario");
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            string[] user = new string[1];
            user[0] = Page.User.Identity.Name;
            msj = new Mensaje(user);
            if (msj.HasMessages)
            {
                var mensaje = msj.Mensajes.First();
                ModalShow(mensaje.Cuerpo,mensaje.HeadMsj,mensaje.Id_Mensage);
                
            }
        }
        

        

    }

    protected void ModalShow(string Mensaje, string Titulo, string IdMsj)
    {
        HdnIdMsjMaster.Value = IdMsj;
        Mdl_LblTitleMaster.Text = "Mensaje de: " + Titulo;
        Mdl_LblMsgMaster.Text = Mensaje;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsgMaster').modal('show');});</script>", false);
    }

    protected void BtnMsjMaster_Click(object sender, EventArgs e)
    {
        msj = new Mensaje();
        msj.NullingMsj(HdnIdMsjMaster.Value);
        System.Threading.Thread.Sleep(300);

    }
}