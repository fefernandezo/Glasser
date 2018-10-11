<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Distribuidor/SiteDistr.master" AutoEventWireup="true" CodeFile="Form-Item.aspx.cs" Inherits="Dis_Form_Item" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        .headtable
        {
            background-color: rgba(29, 182, 65, 0.85);
            color: white;
           
            
        }
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <div class="modal"  id="modalLoading" tabindex="-1" role="dialog">
                                 <div class="modal-dialog modal-lg modal-dialog-centerd" role="document">
                                     <div class="overlay">
                           <div class="modalprogress">
                                   <div class="spinner">
                                        <div class="bounce1"></div>
                                        <div class="bounce2"></div>
                                        <div class="bounce3"></div>
                                    </div>
                               <h3 class="text-center" id="LoadingMsj"></h3>
                            
                           </div>
                            
                        </div>
                                 </div>

    </div>

    <div class="container">
        <div class="row">
            <div class="col-12">
                <asp:Button runat="server" ID="BtnVolverFormulario" OnClientClick="javascript:Loading();" CssClass="btn btn-outline-danger" OnClick="BtnVolverFormulario_Click" Text="Volver" />
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4 col-12"></div>
            <div class="col-lg-4 col-12">
                <div class="img-fluid">
                    <div runat="server" id="ItemImg"></div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-lg-12 col-12 mb-2">
                <asp:UpdatePanel runat="server" ID="UpdatePnl1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form">
                    <label><strong>Producto:</strong></label>&nbsp;&nbsp;<asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtNombreProd"></asp:TextBox>
                            <asp:LinkButton runat="server" ID="LinkbtnNombreProd" OnClick="LinkbtnNombreProd_Click" Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkbtnNombreProd" EventName="Click" />
                    </Triggers>
                    
                </asp:UpdatePanel>
                
            </div>
            <div class="col-lg-12 col-12 mb-2">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form">
                    <label><strong>Descripción:</strong></label>&nbsp;&nbsp;<asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtDescripProd"></asp:TextBox>
                            <asp:LinkButton runat="server" ID="LinkBtnDescripProd" OnClick="LinkBtnDescripProd_Click" Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnDescripProd" EventName="Click" />
                    </Triggers>
                    
                </asp:UpdatePanel>
                
            </div>
            <div class="col-lg-12 col-12 mb-2">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-inline">
                    <label><strong>Cantidad:</strong></label>&nbsp;&nbsp;<asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtCantidad"></asp:TextBox>
                            <asp:LinkButton runat="server" ID="LinkBtnCantidad" OnClick="LinkBtnCantidad_Click" Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnCantidad" EventName="Click" />
                    </Triggers>
                    
                </asp:UpdatePanel>
                
            </div>
        </div>
        <div class="row mb-2">
            
                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="col-12">
                            <asp:Label runat="server" Font-Bold="true" ID="LblCostoMO" Visible="false"></asp:Label>
                        </div>
                        <div class="col-12">
                            
                                <asp:Panel ID="PanelTxtCostoMO" Visible="false" runat="server">
                                    
                                    <div class="form-inline">
                                       Mano de Obra Instalación: &nbsp;&nbsp; <asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtCostoMO"></asp:TextBox>/Unidad &nbsp; &nbsp;
                                    </div>
                                </asp:Panel>
                            <asp:LinkButton runat="server" ID="LinkBtnCostoMO" OnClick="LinkBtnCostoMO_Click"  Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                            
                        </div>


                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnCostoMO" EventName="Click" />
                    </Triggers>
                    
                </asp:UpdatePanel>
            
        </div>
    </div>
     <div class="container">
         <!--piezas del producto-->
         <div class="row mb-2">
             <div class="col text-center"><h3>Piezas del Producto</h3></div>
         </div>
         <div class="row">
             <div id="DivListaPiezas" class="table-responsive" runat="server"></div>
         </div>
         
         
                 
                
            </div>

    <!--Modal visualizar pieza-->
    <div class="modal fade"  id="modalPieza" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3><label id="ModalPiezatitulo"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <asp:HiddenField runat="server" ID="HdnIDSUBDET" />
              <div class="row mb-3">
                  <div class="col">
                      <div class="form-inline">
                          <label>Ancho: </label>&nbsp;<asp:TextBox CssClass="form-control" runat="server" ID="TxtAnchoPiezaSelected"></asp:TextBox>
                      </div>
                  </div>
              </div>
              <div class="row mb-3">
                  <div class="col">
                      <div class="form-inline">
                          <label>Alto:</label>&nbsp;<asp:TextBox CssClass="form-control" runat="server" ID="TxtAltoPiezaSelected"></asp:TextBox>
                      </div>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              <asp:Button runat="server" ID="UpdateVariablesPieza" CssClass="btn btn-outline-primary" Text="Aceptar" OnClick="UpdateVariablesPieza_Click" />
          </div>
          
       </div>
  </div>
</div>
    <script type="text/javascript">

        function Loading() {

            
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Espere un momento...');
            $('#modalLoading').modal('show').css('pointer-events','none');
        }

        $(document).ready(function () {
            $('.filas').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
            });
            $('.filas').mouseleave(function () {

                        $(this).css('text-decoration', '');
            });
            $('.filas').click(function () {
                var IDSUBDET = $(this).find("td").eq(5).html();
                var IDMODELBOM = $(this).find("td").eq(6).html();
                var nombre = $(this).find("td").eq(1).html();
                var ancho = $(this).find("td").eq(2).html();
                var alto =$(this).find("td").eq(3).html();
                /*Abrir modal*/
                $('#modalPieza').modal('show');
                $('#ModalPiezatitulo').html(nombre);
                $('#<%= TxtAltoPiezaSelected.ClientID%>').val(alto);
                $('#<%= TxtAnchoPiezaSelected.ClientID%>').val(ancho);
                $('#<%= HdnIDSUBDET.ClientID%>').val(IDSUBDET);


            });
        });

    </script>
</asp:Content>