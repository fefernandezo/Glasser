<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="CreaciondeVariables.aspx.cs" Inherits="_Creacion_Variables" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        .headtable
        {
            background-color: rgba(29, 182, 65, 0.85);
            color: white;
           
            
        }
         .bodyRow{
            padding-left:15px;
            padding-right:15px;
            padding-top:15px;
            padding-bottom:15px;
        }
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
   <div class="container">
       <div class="row justify-content-center">
           <h1>Creación de Variables</h1>
       </div>
       <div class="row mb-3 justify-content-center">
           <h1><asp:Label runat="server" ID="LblTitle1"></asp:Label></h1>
       </div>
       
       <div class="row justify-content-center">
           <div class="card mb-3 card-border-mangotango">
                <div class="card-header card-bg-mangotango" id="headerInfPed">
                    <h5><asp:Label runat="server" ID="Lbltitle"></asp:Label></h5>
                </div>
                <div id="CollapseInfoPed" class="collapse show">
                    <div class="card-body ">
                        <asp:HiddenField runat="server" ID="HdnId" />
                        <asp:Panel runat="server" ID="CommonControls">
                            <div class="row mb-3">
                                <div class="col">
                                        Nombre : <asp:TextBox runat="server" ID="TxtNombre" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtNombre" ErrorMessage="El nombre es obligatorio" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                        </div>
                            <div class="row mb-3">
                                <div class="col">
                                        Descripción : <asp:TextBox runat="server" TextMode="MultiLine" ID="TxtDescripcion" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="MTABMAcontrols" Visible="false">
                            <div class="row mb-3">
                                <div class="col">
                                    Procedencia: <asp:TextBox runat="server" ID="TxtProcedencia" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row mb-2 justify-content-end">
                            <div class="col text-right">
                                <asp:Button runat="server" ID="BtnCrear" OnClick="BtnCrear_Click" CssClass=" boton outline-mangotango" Text="Crear" />
                            </div>
                            
                        </div>
                         
                        

                    </div>
                </div>
            </div>
       </div>
   </div>
</asp:Content>