using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProRepo;

public partial class Account_Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                ctlloggin.UserNameLabelText = Request.Cookies["UserName"].Value;
                ctlloggin.PasswordLabelText= Request.Cookies["Password"].Value;
            }
        }

    }
    protected void ctlloggin_LoggedIn(object sender, EventArgs e)
    {
        RedirectLogin(ctlloggin.UserName);

        if (RememberMe.Checked)
        {
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
        }
        else
        {
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
        }

        Response.Cookies["UserName"].Value = ctlloggin.UserName;
        Response.Cookies["Password"].Value = ctlloggin.Password;
    }

    /// <summary>
    /// Redirect the user to a specific URL, as specified in the web.config, depending on their role.
    /// If a user belongs to multiple roles, the first matching role in the web.config is used.
    /// Prioritize the role list by listing higher-level roles at the top.
    /// </summary>
    /// <param name="username">Username to check the roles for</param>
    private void RedirectLogin(string username)
    {
        LoginRedirectByRoleSection roleRedirectSection = (LoginRedirectByRoleSection)ConfigurationManager.GetSection("loginRedirectByRole");
        foreach (RoleRedirect roleRedirect in roleRedirectSection.RoleRedirects)
        {
            if (Roles.IsUserInRole(username, roleRedirect.Role))
            {
                Response.Redirect(roleRedirect.Url);
            }
        }
    }



    protected void ctlloggin_LoginError(object sender, EventArgs e)
    {
        LblError.Text = "Error en el usuario o contraseña, por favor intentelo nuevamente.";
        
        

    }

    

    protected void RecuperarPass_Click(object sender, EventArgs e)
    {
        
        ResetPass();
        
    }

    private void ResetPass()
    {
        string Getuser = Membership.GetUserNameByEmail(TxtEmal.Text);

        if (string.IsNullOrEmpty(Getuser))
        {
            LblMsjforeget.Text = "El correo ingresado no existe en nuestros registros.";
            LblMsjforeget.Visible = true;
        }
        else
        {


            //recuperar contraseña
            string correo = TxtEmal.Text;
            string NewPass = Membership.GeneratePassword(7, 1);
            MembershipUser uSER = Membership.GetUser(Getuser, false);
            string OldPass;
            try
            {
                OldPass = uSER.ResetPassword();
                try
                {
                    uSER.ChangePassword(OldPass, NewPass);
                    string[] Para = { correo };
                    Correo _correo = new Correo();
                    _correo.Enviar(GetEmail(Getuser, NewPass, uSER.UserName), "Nueva contraseña Plataforma PHGlass", Para, "Plataforma PHGlass", "Normal");

                    TxtEmal.Visible = false;
                    RecuperarPass.Visible = false;
                    LblMsjforeget.ForeColor = System.Drawing.Color.White;
                    LblMsjforeget.Visible = true;
                    LblMsjforeget.Font.Size = 20;
                    LblMsjforeget.Text = "su contraseña ha sido enviada a su correo.";

                    

                }
                catch (MembershipPasswordException E1)
                {

                    throw E1;
                }
            }
            catch (MembershipPasswordException E2)
            {

                throw E2;
            }




        }

    }

    private string GetEmail(string Usuario, string Contraseña, string UserName)
    {
        string Cuerpo = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +

                              "<html xmlns = 'http://www.w3.org/1999/xhtml' >" +
                              "<head>" +
                                  "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />" +
                                  "<title> Plataforma Ecommerce PHGlass</title>" +
                                  "<meta name='viewport' content='width=device-width, initial-scale=1.0' />" +
                              "</head>" +
                              "<body style='margin: 0; padding: 0;'>" +
                              "<table border='0' cellpadding='0' cellspacing='0' max-width='700px' >" +
                                  "<tr>" +
                                      "<td>" +
                                          "<table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>" +
                                              "<tr>" +
                                                      "<td bgcolor='black' align='center' style='padding: 10px'>" +
                                                          "<span></span>" +
                                                      "</td>" +
                                               "</tr>" +
                                               "<tr>" +
                                                      "<td>" +
                                                              "<table border='1' cellpadding='0' cellspacing='0' width='100%'>" +
                                                                      "<tr>" +
                                                                              "<td colspan='3' style='color: #153643; font-family: Arial, sans-serif;padding: 5px 5px 5px 5px; text-align: center; font-weight: bold; font-size:20px'>Recuperación de Clave</td>" +
                                                                      "</tr>" +
                                                                      "<tr><td colspan='3' style='color: #153643; font-family: Arial, sans-serif;padding: 5px 5px 5px 5px; text-align: center;' >" +
                                                                      "Hola "+ Usuario +"!</br> A continuación te envío la nueva contraseña que has solicitado. por motivos de seguridad, debes ingresar con esta contraseña en el sistema y cambiarla. </td></tr>" +
                                                                      "<tr>" +
                                                                              "<td colspan='1'>Usuario:" +

                                                                              "</td>" +
                                                                              "<td colspan='2' style='color: #153643; font-family: Arial, sans-serif;padding: 5px 5px 5px 5px; text-align: left; font-weight: bold; font-size:16px'>" +
                                                                                UserName +
                                                                              "</td>" +
                                                                      "</tr>" +
                                                                      "<tr>" +
                                                                              "<td colspan='1'>Contraseña:" +

                                                                              "</td>" +
                                                                              "<td colspan='2' style='color: #153643; font-family: Arial, sans-serif;padding: 5px 5px 5px 5px; text-align: left; font-weight: bold; font-size:16px'>" +
                                                                                Contraseña +
                                                                              "</td>" +
                                                                      "</tr>" +
                                                                     
                                                                      
                                                                      
                                                               "</table>" +
                                                       "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                          "<td bgcolor='#22222b' style='padding: 10px 10px 10px 10px;'>" +
                                                          "       <table border='0' cellpadding='0' cellspacing='0' max-width='700%'>" +
                                                                      "<tr>" +
                                                                              "<td style='color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;' >&reg; PHGlass 2018 <br/>" +
                                                                                      "Este correo se genera de manera automática a través de la plataforma Ecommerce de PHGlass. Por favor no responder" +
                                                                              "</td>" +

                                                                       "<tr>" +
                                                                              "<td></td>" +
                                                                              "<td style='font-size: 0; line-height: 0;' width = '20' > &nbsp;</td>" +
                                                                              "<td></td>" +
                                                                       "</tr>" +

                                                                  "</table>" +
                                                           "</td>" +
                                                "</tr>" +
                                          "</table>" +
                                      "</td>" +
                                  "</tr>" +
                              "</table>" +
                              "</body>" +
                              "</html> ";

        return Cuerpo;
    }
}