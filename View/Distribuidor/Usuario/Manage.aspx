<%@ Page Title="Administrar cuenta" Language="C#" MasterPageFile="~/View/Distribuidor/SiteDistr.master" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Dis_Account_Manage" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<%@ Import Namespace="Microsoft.AspNet.Membership.OpenAuth" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <h5><asp:Label ID="LblName" runat="server"></asp:Label></h5>
            </div>
        
    </div>
    </div>
    <section id="passwordForm">
       

        

        <asp:PlaceHolder runat="server" ID="setPassword" Visible="false">
            <p>
                No dispone de contraseña local para este sitio. Agregue una contraseña
                local para iniciar sesión sin que sea necesario ningún inicio de sesión externo.
            </p>
            <fieldset>
                <legend>Formulario para establecer contraseña</legend>
                <ol>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="password">Contraseña</asp:Label>
                        <asp:TextBox runat="server" ID="password" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="password"
                            CssClass="field-validation-error" ErrorMessage="El campo de contraseña es obligatorio."
                            Display="Dynamic" ValidationGroup="SetPassword" />
                        
                        <asp:Label runat="server" ID="newPasswordMessage" CssClass="field-validation-error"
                            AssociatedControlID="password" />
                        
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="confirmPassword">Confirmar contraseña</asp:Label>
                        <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="confirmPassword"
                            CssClass="field-validation-error" Display="Dynamic" ErrorMessage="El campo de confirmación de contraseña es obligatorio."
                            ValidationGroup="SetPassword" />
                        <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="confirmPassword"
                            CssClass="field-validation-error" Display="Dynamic" ErrorMessage="La contraseña y la contraseña de confirmación no coinciden."
                            ValidationGroup="SetPassword" />
                    </li>
                </ol>
                <asp:Button runat="server" Text="Establecer contraseña" ValidationGroup="SetPassword" OnClick="setPassword_Click" />
            </fieldset>
        </asp:PlaceHolder>

        <h3 class="text-center"> <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
                                        <p class="message-success"><%: SuccessMessage %></p>
                                       </asp:PlaceHolder></h3>

        <asp:PlaceHolder runat="server" ID="changePassword" Visible="false">
           
            <asp:ChangePassword runat="server" CancelDestinationPageUrl="~/" ViewStateMode="Disabled" RenderOuterTable="false" SuccessPageUrl="Manage?m=ChangePwdSuccess">
                <ChangePasswordTemplate>
                    <p class="validation-summary-errors">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                    <fieldset class="changePassword">
                        
                        <div class="container-fluid">
                            <div class="row center-block">
                                <legend class=" text-center">Cambiar contraseña</legend>
                                <div class="col-lg-4">

                                </div>
                                <div class="col-lg-4">
                                    
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword">Contraseña actual</asp:Label>
                                <asp:TextBox runat="server" ID="CurrentPassword" CssClass="passwordEntry form-control" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ForeColor="Red" ControlToValidate="CurrentPassword"
                                    CssClass="field-validation-error" ErrorMessage="El campo de contraseña actual es obligatorio." />
                                    </div>
                                    <div class="form-group">
                                         <asp:Label runat="server" ID="NewPasswordLabel" ForeColor="Blue" AssociatedControlID="NewPassword">Nueva contraseña</asp:Label>
                                <asp:TextBox runat="server" ID="NewPassword" CssClass="passwordEntry form-control" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ForeColor="Red" ControlToValidate="NewPassword"
                                    CssClass="field-validation-error" ErrorMessage="La contraseña nueva es obligatoria." />
                                    </div>
                                    <div class="form-group">
                                         <asp:Label runat="server" ID="ConfirmNewPasswordLabel" ForeColor="Blue" AssociatedControlID="ConfirmNewPassword">Confirmar la nueva contraseña</asp:Label>
                                <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry form-control" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                    CssClass="field-validation-error" Display="Dynamic" ForeColor="Red" ErrorMessage="La confirmación de la nueva contraseña es obligatoria." />
                                <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                    CssClass="field-validation-error" Display="Dynamic" ForeColor="Red" ErrorMessage="La nueva contraseña y la contraseña de confirmación no coinciden." />
                                    </div>
                                     <asp:Button runat="server" CssClass="btn btn-success" CommandName="ChangePassword" Text="Cambiar contraseña" />
                                </div>
                                <div class="col-lg-4">

                                </div>

                            </div>
                        </div>
                        
                       
                    </fieldset>
                </ChangePasswordTemplate>
            </asp:ChangePassword>
        </asp:PlaceHolder>
    </section>
    <style>
        h3 {
            color:blue;
        }
    </style>
   
</asp:Content>
