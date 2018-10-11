<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/View/Logistica/SiteLog.Master" AutoEventWireup="true" CodeFile="CreaciondeRutas.aspx.cs" Inherits="_HomeInventario" %>

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
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <asp:Panel runat="server" ID="PanelGral" Visible="false">

         <div class="container">
            <div class="text-center">
                <h2>Creación de rutas</h2>
            </div>
            <div class="bodyRow">
                <div class=" form-inline">
                    <asp:Label runat="server" Text="Nombre ruta: "></asp:Label><asp:TextBox ID="TxtNombreRuta" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredTxtNombreRuta" runat="server" ForeColor="Red" ControlToValidate="TxtNombreRuta" ErrorMessage="Debe ingresar un nombre de Ruta"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="bodyRow">
                <div class=" form-group">
                    <asp:Label runat="server" Text="Descripción ruta: "></asp:Label><asp:TextBox ID="TxtDescripRuta" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="bodyRow">
                <asp:Panel ID="PanelSucInv" runat="server" Visible="false">
                    <div class="form-inline">
                        <asp:Label runat="server" Text="Sucursal: "></asp:Label><asp:Label ID="LblSucursal" runat="server"></asp:Label>
                    </div>
                    <p></p>
                </asp:Panel>
                <asp:Panel ID="PanelDDLSuc" runat="server" Visible="false">
                    <div class="form-inline">
                        <asp:Label runat="server" Text="Sucursal: "></asp:Label>
                        <asp:DropDownList runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="DDListSucu_SelectedIndexChanged" ID="DDListSucu"></asp:DropDownList>
                    </div>
                    <p></p>
                </asp:Panel>
                
                    <asp:UpdatePanel ID="UpdateBodega" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panelbodega" runat="server" Visible="false">
                                <div class="form-inline">
                                        <asp:Label runat="server" Text="Bodega: "></asp:Label><asp:DropDownList runat="server" CssClass="form-control" ID="DDListBodega"></asp:DropDownList>
                                </div>
                                <p></p>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DDListSucu" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    
                
                
                <asp:Label runat="server" ID="ErrorDDlistbodega" ForeColor="Red" Visible="false"></asp:Label>
            </div>
             <div class="bodyRow text-center">
                 <asp:Button runat="server" ID="BtnCrearRuta" Text="Crear Ruta" OnClick="BtnCrearRuta_Click" CssClass="btn btn-success btn-lg" />
             </div>
                
                 
                
         </div>
    </asp:Panel>
    

    <!--MODAL MENSAJE-->
    <div class="modal fade"  id="ModalMsg" tabindex="-1" role="dialog" aria-labelledby="TitleMsg" aria-hidden="true">
      <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
          <div class="modal-header">
              <h4 class="modal-title" id="TitleMsg">Mensaje</h4>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                    <div class="row">
                            <h4><asp:Label runat="server" ID="Mdl_LblMsg"></asp:Label></h4>
                   </div>
              </div>
          </div>
          <div class="modal-footer">
              <asp:Button CssClass="btn btn-info" ID="MdlBtnOkMsg" runat="server" Text="Aceptar" OnClick="MdlBtnOkMsg_Click" />
          </div>
          
       </div>
  </div>
</div>

    <script type="text/javascript">
       

        
    </script>
</asp:Content>