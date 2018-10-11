<%@ Page Title="Registrar usuario" Language="C#" MasterPageFile="~/Administrador/SiteAdm.Master" AutoEventWireup="true" CodeFile="Registrar.aspx.cs" Inherits="Account_Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style>
        .formulario{
            margin: auto;
            
        }
        .textbox{
            max-width: 300px;
        }
    </style>
    <div class="container">
        <div class="row text-center">
            <hgroup class="title">
        <h1><%: Title %></h1>
        <h2>Para crear un usuario, debe llenar el siguiente formulario</h2>
    </hgroup>
        </div>
        
        <div class="row">
             
                            <div class=" form-inline">
                                <p>
                                    <asp:Label ID="lblname" runat="server" AssociatedControlID="Txtnombre" Text="Nombre :"></asp:Label>
                                    <asp:TextBox ID="Txtnombre" runat="server" CssClass="form-control textbox" placeholder="Nombre del usuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Txtnombre"
                                        CssClass="field-validation-error" ForeColor="Red" ErrorMessage="El campo Nombre del usuario es obligatorio." />
                                </p>
                            </div>
                            <div class=" form-inline">
                                <p>
                                    <asp:Label ID="lblapellido" AssociatedControlID="Txtapellido" runat="server" Text="Apellido :"></asp:Label>
                                    <asp:TextBox ID="Txtapellido" runat="server" CssClass="form-control textbox" placeholder="Apellido del usuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Txtapellido"
                                        CssClass="field-validation-error" ForeColor="Red" ErrorMessage="El campo Apellido es obligatorio." />
                                </p>
                            </div>
            <asp:CreateUserWizard runat="server" ID="RegisterUser" ViewStateMode="Disabled" OnCreatedUser="RegisterUser_CreatedUser">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="wizardStepPlaceholder" />
                    <asp:PlaceHolder runat="server" ID="navigationPlaceholder" />
                </LayoutTemplate>
                <WizardSteps>
                    <asp:CreateUserWizardStep runat="server" ID="RegisterUserWizardStep">
                        <ContentTemplate>
                            <p class="message-info">
                                Es necesario que las contraseñas tengan al menos <%: Membership.MinRequiredPasswordLength %> caracteres.
                            </p>

                            <p class="validation-summary-errors danger">
                                <asp:Literal runat="server" ID="ErrorMessage" />
                            </p>
                           
                            <div class="formulario">
                                <fieldset>
                        
                                <div class="form-inline">
                                    <p><asp:Label runat="server" AssociatedControlID="UserName">Usuario</asp:Label>
                                        <asp:TextBox CssClass="form-control textbox" runat="server" ID="UserName" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                        CssClass="field-validation-error" ForeColor="Red" ErrorMessage="El campo Usuario es obligatorio." />
                                    </p>
                                    
                                    
                                </div>
                                <div class="form-inline">
                                    <p>
                                        <asp:Label runat="server" AssociatedControlID="Email">Correo electrónico</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control textbox" ID="Email" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                        CssClass="field-validation-error" ForeColor="Red" ErrorMessage="El campo de dirección de correo es obligatorio." />
                                    </p>
                                    
                                </div>
                                <div class="form-inline">
                                    <p>
                                        <asp:Label runat="server" AssociatedControlID="Password">Contraseña</asp:Label>
                                        <asp:TextBox CssClass="form-control textbox" runat="server" ID="Password" TextMode="Password" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                        CssClass="field-validation-error" ForeColor="Red" ErrorMessage="El campo de contraseña es obligatorio." />
                                    </p>
                                    
                                </div>
                                <div class="form-inline">
                                    <p>
                                        <asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirmar contraseña</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control textbox" ID="ConfirmPassword" TextMode="Password" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                        CssClass="field-validation-error" ForeColor="Red" Display="Dynamic" ErrorMessage="El campo de confirmación de contraseña es obligatorio." />
                                    </p>
                                    
                                    <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                     CssClass="field-validation-error" ForeColor="Red" Display="Dynamic" ErrorMessage="La contraseña y la contraseña de confirmación no coinciden." />
                                </div>

                                <div class="row text-center">
                                    <asp:Button runat="server" CssClass="btn btn-success" CommandName="MoveNext" Text="Registrar Usuario" />
                                </div>
                        
                    </fieldset>
                            </div>
                            
                        </ContentTemplate>
                        <CustomNavigationTemplate />
                    </asp:CreateUserWizardStep>
                </WizardSteps>
            </asp:CreateUserWizard>
        </div>
    </div>
    

    
</asp:Content>