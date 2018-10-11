<%@ Page Title="Administrador" Language="C#" MasterPageFile="~/Administrador/SiteAdm.Master" AutoEventWireup="true" CodeFile="Asignar-Roles.aspx.cs" Inherits="_AsignarRoles" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <style>
        .fila{
            
            margin-bottom: 20px;
            margin-left:20px;
        }
        .ddlist{
            max-width: 300px;
        }
        .linkbtn{
            margin-left:30px;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">
        <Scripts>
             <asp:ScriptReference Name="jquery.ui.combined" />
        </Scripts>
    </asp:ScriptManager>
    <div class="container">
        <div class="row">
            <asp:Label ID="Mensaje" ForeColor="Red" runat="server"></asp:Label>
        </div>
        <div class="row">
            
            
                        <asp:Panel ID="PanelUser1" Visible="false" runat="server">
                            
                            <div class="text-center">
                                <h3><asp:Label runat="server" ID="Userheredado"></asp:Label></h3>
                            </div>
                            
                        </asp:Panel>
                        <asp:Panel ID="PanelUser" Visible="false" runat="server" >
                            <div class="form-inline fila">
                                <asp:Label runat="server" ID="label1" Font-Bold="true">Usuario </asp:Label>
                                <asp:DropDownList ID="DDListUser" CssClass="form-control ddlist" runat="server"></asp:DropDownList>
                                 
                            </div>
                            <div class="form-group">
                               
                            </div>
                        </asp:Panel>
                        
                    
            

        </div>
        <div class="row">
            <div class="form-inline fila">
                <asp:Label runat="server" Font-Bold="true">Tipo de usuario </asp:Label><asp:DropDownList ID="DDListroles" AutoPostBack="true" OnSelectedIndexChanged="DDListroles_SelectedIndexChanged" CssClass="form-control ddlist" runat="server"></asp:DropDownList>
            </div>
            
        </div>
        <div class="row">
            
                <asp:UpdatePanel ID="inempresa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelCliente" Visible="false" runat="server" >
                            <div class="form-inline fila">
                                <asp:Label runat="server" ID="label" Font-Bold="true">Cliente </asp:Label>
                                <asp:DropDownList ID="DDListcliente" CssClass="form-control ddlist" runat="server"></asp:DropDownList>
                                 <asp:LinkButton ID="CrearCliente" Text="Agregar un Cliente" CssClass="linkbtn" OnClick="CrearCliente_Click" runat="server"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                               
                            </div>
                        </asp:Panel>
                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DDListroles" EventName="SelectedIndexChanged" />
                       
                    </Triggers>
                </asp:UpdatePanel>
            

        </div>
        <div class="row">
           
                    <asp:Button runat="server" ID="BtnIngresar" Text="Asignar" CssClass="btn btn-success" OnClick="BtnIngresar_Click" />
                
        </div>
    </div>



</asp:Content>