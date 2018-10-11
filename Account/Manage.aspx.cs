﻿using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.UI.WebControls;

using Microsoft.AspNet.Membership.OpenAuth;

public partial class Account_Manage : System.Web.UI.Page
{
    protected string SuccessMessage
    {
        get;
        private set;
    }

    protected bool CanRemoveExternalLogins
    {
        get;
        private set;
    }

    protected void Page_Load()
    {
        if (!IsPostBack)
        {
            // Determine las secciones que se van a presentar
            var hasLocalPassword = OpenAuth.HasLocalPassword(User.Identity.Name);
            setPassword.Visible = !hasLocalPassword;
            changePassword.Visible = hasLocalPassword;

            CanRemoveExternalLogins = hasLocalPassword;

            // Presentar mensaje de operación correcta
            var message = Request.QueryString["m"];
            if (message != null)
            {
                // Seccionar la cadena de consulta desde la acción
                Form.Action = ResolveUrl("~/Account/Manage");

                SuccessMessage =
                    message == "ChangePwdSuccess" ? "Se ha cambiado la contraseña con éxito."
                    : message == "SetPwdSuccess" ? "Se estableció la contraseña."
                    : message == "RemoveLoginSuccess" ? "El inicio de sesión externo se ha quitado."
                    : String.Empty;
                successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
            }
        }
        

        // Vincular con datos la lista de las cuentas externas
        var accounts = OpenAuth.GetAccountsForUser(User.Identity.Name);
        CanRemoveExternalLogins = CanRemoveExternalLogins || accounts.Count() > 1;
       
        
    }

    protected void setPassword_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            var result = OpenAuth.AddLocalPassword(User.Identity.Name, password.Text);
            if (result.IsSuccessful)
            {
                Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
            }
            else
            {
                
                newPasswordMessage.Text = result.ErrorMessage;
                
            }
        }
    }

    
   

    protected T Item<T>() where T : class
    {
        return GetDataItem() as T ?? default(T);
    }
    

    protected static string ConvertToDisplayDateTime(DateTime? utcDateTime)
    {
        // Puede cambiar este método para convertir la hora y fecha UTC con el formato y el desfase
        // deseados. En este caso, se convertirá a la zona horaria del servidor y se asignará el formato
        // de cadena de hora larga y fecha corta mediante la cultura de subproceso actual.
        return utcDateTime.HasValue ? utcDateTime.Value.ToLocalTime().ToString("G") : "[nunca]";
    }
}