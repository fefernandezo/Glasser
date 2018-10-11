<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Cliente_Default_Man" %>

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
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    
     <div class="container-fluid bg-texture-glasser">
         <br /><br /><br />
         <asp:HiddenField runat="server" ID="HdnRutCli" />
          <asp:Panel ID="PanelAlerta" runat="server" Visible="false">
            <div class="container bg-opacity-white-65">
                <div class="row">
                    <div class="col-12">
                        <div class="alert alert-dismissible fade show" role="alert" id="AlertaBienvenida">
                            <h4>Notificaciones</h4>
             
                        <div runat="server" id="MsjPortal"></div>
             

             <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
         </div>
                    </div>
                    
                </div>
            </div>
            
        </asp:Panel>

         <div class="d-flex justify-content-center mb-3">
             <div class=" form-inline ">
                 <h1><asp:Label ID="LblCliente" runat="server"></asp:Label></h1>
                 <asp:Panel runat="server" ID="PnlBtnToMdlChangeCli" Visible="false">
                     &nbsp;&nbsp;&nbsp;&nbsp;<button type="button" class="btn btn-outline-dark" data-toggle="modal" data-target="#modalChangeCli">Cambiar Empresa</button>
                 </asp:Panel>
                 
             </div>
             
         </div>
         <div class="row">
             <!--sidebar-->
             <div class=" col-sm-12 col-md-12 col-xl-3 col-lg-12 mb-2">
                 <!--card info empresa-->
                 <div class="card mb-1 bg-opacity-white-65">
                     <div class="card-body">
                         <div class="row">
                             <div class="col-12">
                                 <asp:Button runat="server" ID="BtnInfoEmpr" OnClick="BtnInfoEmpr_Click" CssClass="btn btn-link HighLight-1 text-azul text-uppercase font-weight-bold" Text="Información de la empresa" />
                             </div>
                             
                         </div>
                     </div>
                 </div>

                 <!--card cotizaciones-->
                 <asp:Panel runat="server" ID="PnlCotizaciones" Visible="false">
                     <div class="card mb-1 bg-opacity-white-65">
                     <div class="card-body sidebar-bg-gris">
                         
                         <div class="row">
                             <asp:Button runat="server" ID="BtnMisCotizaciones" OnClick="BtnMisCotizaciones_Click" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Mis cotizaciones" />
                         </div>
                         <div class="row">
                             <asp:Button runat="server" ID="BtnCotizar" OnClick="BtnCotizar_Click" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Nueva Cotización" />
                         </div>
                     </div>
                 </div>
                 </asp:Panel>
                 
                 <asp:Panel runat="server" ID="PnlPedidos">
                     <div class="card mb-1 bg-opacity-white-65">
                         <div class="card-body">
                             <div class="row">
                                 <asp:Button runat="server" ID="BtnMisPedidos" OnClick="BtnMisPedidos_Click" CssClass="btn btn-link HighLight-1 text-azul text-uppercase font-weight-bold" Text="Mis Pedido" />
                             </div>
                             <div class="row">
                                 

                                 <asp:UpdatePanel runat="server" ID="UpdatepnlNewOrder" UpdateMode="Conditional">
                                     <ContentTemplate>
                                         <asp:Button runat="server" ID="BtnCrearPedido"  OnClientClick="javascript:OpenModalNewOrder();" CssClass="btn btn-link HighLight-1 text-azul text-uppercase font-weight-bold" Text="Nuevo Pedido" />
                                     </ContentTemplate>
                                     <Triggers>
                                         <asp:AsyncPostBackTrigger ControlID="BtnCrearPedido" EventName="Click" />
                                     </Triggers>
                                 </asp:UpdatePanel>
                             

                             </div>
                             

                         </div>
                     </div>
                 </asp:Panel>
                 
                 

             </div>
              <!--content--> 
             <div class="col-sm-12 col-md-12 col-xl-9 col-lg-12">
                 <div id="accordion"  >
                     <!--estados financieros-->
                     <div class="card mb-3 border-primary bg-opacity-white-75 ">
                            <div class="card-header bg-opacity-blue-10 border-primary">
                                <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseFin" class=" text-primary"> Estado Financiero</a></h3>    
                            </div>
                                    <div id="CollapseFin" class="collapse show">
                                            <div class="card-body">
                                                <div class="row">
                                                     <div class="col-12 col-lg-4 col-md-4 mb-3">
                                                                
                                                         <div class="card border-info bg-opacity-white-10 HighLight-1">
                                                             <div class="card-header bg-info text-white">
                                                                 <h5 class="text-center">Crédito disponible</h5>
                                                             </div>
                                                            <div class="card-body">
                                                               
                                                                <h3 class=" text-center"><asp:Label ID="lblCreditoDis" runat="server" Text=""></asp:Label></h3>
                                                            </div>
                                                        </div>
                                                           
                                                        </div>
                                                        <div class="col-12 col-lg-4 col-md-4 mb-3">
                                                                
                                                        <div class="card border-info bg-opacity-white-10 HighLight-1">
                                                            <div class="card-header bg-info text-white">
                                                                <h5 class="text-center">Cupo utilizado</h5>
                                                            </div>
                                                                <div class="card-body">
                                                                    <h3 class="text-center"><asp:Label ID="lblUtilizado" runat="server" Text=""></asp:Label></h3>
                                                                </div>
                                                        </div>
                                                            
                                                        </div>
                                                        <div class="col-12 col-lg-4 col-md-4 mb-3">
                                                                
                                                        <div class="card border-info bg-opacity-white-10 HighLight-1">
                                                            <div class="card-header bg-info text-white">
                                                                <h5 class="text-center ">Disponible para comprar</h5>
                                                            </div>
                                                                <div class="card-body">
                                                                    <h3 class="text-center"><asp:Label ID="LabelDisponible" runat="server" Text=""></asp:Label></h3>
                                                                    <asp:HiddenField ID="Hiddendisponible" runat="server" />
                                                                    
                                                                </div>
                                                        </div>
                                                            
                        
                                                        </div>
                                                </div>
                                                       
                                            </div>
                                    </div>
                
                     </div>
                    <!--plazos de entrega-->
                    <div class="card mb-3 border-success bg-opacity-white-75">
                            <div class="card-header bg-opacity-green-10 border-success">
                                <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseInfo" class=" text-success">Plazos de Entrega</a></h3>    
                            </div>
                                    <div id="CollapseInfo" class="collapse show">
                                         <div class="card-body ">
                                             <div class="row">
                                                 <div class="col-12 col-lg-4 col-md-4 mb-3">
                                                    <div class="card border-success bg-opacity-green-10 HighLight-1">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Termopaneles</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="d-flex justify-content-center">
                                                                <h5><asp:Label ID="FechaTermo" runat="server"></asp:Label></h5>
                                                            </div>
                                                            <div class="d-flex justify-content-center">
                                                                <asp:Label ID="diastermo" runat="server"></asp:Label>*
                                                            </div>    
                                                        </div>
                                                    </div>
                                                </div>
                                                 
                
                                                <div class="col-12 col-lg-4 col-md-4 mb-3">
                                                    <div class="card border-success bg-opacity-green-10 HighLight-1">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Láminas</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="d-flex justify-content-center">
                                                                <h5><asp:Label ID="FechaLaminas" runat="server"></asp:Label></h5>
                                                            </div>
                                                            <div class="d-flex justify-content-center">
                                                                <asp:Label ID="DiasLaminas" runat="server"></asp:Label>*
                                                            </div>
                                                          
                                                                
                                                        </div>
                                                    </div>
                                                </div>
                                                 
                                                <div class="col-12 col-lg-4 col-md-4 mb-3">
                                                    <div class="card border-success bg-opacity-green-10 HighLight-1">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Arquitectura</h5>
                                                        </div>
                                                       <div class="card-body">
                                                           <div class="d-flex justify-content-center">
                                                               <h5><asp:Label ID="FechaArq" runat="server"></asp:Label></h5>
                                                           </div>
                                                           <div class="d-flex justify-content-center">
                                                               <asp:Label ID="DiasArq" runat="server"></asp:Label>*
                                                           </div>
                                                           
                                                                
                                                        </div>
                                                    </div>
                                                </div>
                                             </div>
                                                
                                            
                                         </div>
                                    </div>
                    </div>
                </div>
             </div>

         
    </div>
                
                 
                
            </div>

    <!--Modal ChagenCli-->
    <div class="modal fade"  id="modalChangeCli" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3>Seleccionar Empresa</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col">
                      <asp:DropDownList runat="server" ID="DDLEmpresas"  CssClass="form-control"></asp:DropDownList>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              
                  <asp:Button runat="server" ID="MdlBtnChangeCli" OnClientClick="javascript:Loading();" CssClass="btn btn-outline-primary" OnClick="MdlBtnChangeCli_Click" Text="Cambiar" />
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>

    <!--Modal Nuevo pedido-->
    <div class="modal fade"  id="modalNewOrder" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3>Nuevo Pedido</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
            <asp:UpdatePanel runat="server" ID="UpdatepnlMdlNewOrder" UpdateMode="Conditional">
                  <ContentTemplate>
                      <div class="modal-body">
              <div class="row mb-5">
                  <div class="col-12 col-lg-12">
                      <div class="form-inline">
                          <h6>Tipo de pedido </h6>&nbsp;<asp:DropDownList runat="server" ID="DDlOrderType" CssClass="form-control">
                              <asp:ListItem Text="<--Seleccionar tipo-->" Value="0"></asp:ListItem>
                              <asp:ListItem Text="Termopanel" Value="1"></asp:ListItem>
                              <asp:ListItem Text="Templados" Value="2"></asp:ListItem>
                              <asp:ListItem Text="Dimensionados" Value="3"></asp:ListItem>
                                                              </asp:DropDownList>
                      </div>
                      
                  </div>
                  <div class="col-12 col-lg-12">
                      <asp:Label runat="server" ID="LblDDLtypeordererror" ForeColor="Red" Visible="false"></asp:Label>
                  </div>
              </div>
              
              <div class="row mb-2">
                  <div class="col-12 col-lg-12">
                      <h5>Nombre del Pedido*:</h5>
                  </div>
                  <div class="col-12 col-lg-12">
                      <asp:TextBox runat="server" ID="TxtOrderName" CssClass="form-control"></asp:TextBox>
                      
                  </div>
                  <div class="col-12 col-lg-12">
                      <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtOrderName" ValidationGroup="NewOrder" ForeColor="Red" Text="El nombre del pedido es obligatorio"></asp:RequiredFieldValidator>
                  </div>
              </div>
              <div class="row mb-2">
                  <div class="col-12 col-lg-12">
                      <h5>Observaciones:</h5>
                  </div>
                  <div class="col-12 col-lg-12">
                      <asp:TextBox runat="server" ID="TxtOrderObs" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                  </div>
              </div>

          </div>
          <div class="modal-footer">
                  <asp:Button runat="server" ID="BtnNewOrder" ValidationGroup="NewOrder" CssClass="btn btn-outline-primary" OnClick="BtnNewOrder_Click" Text="Siguiente" />
          </div>
                  </ContentTemplate>
                  <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="BtnNewOrder" EventName="Click" />
                  </Triggers>
              </asp:UpdatePanel>
          
          
       </div>
  </div>
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            CerrarAlert();

        });

        function Loading() {

            $('#modalChangeCli').modal('hide');
            $('#modalNewOrder').modal('hide');
            
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Espere un momento...');
            $('#modalLoading').modal('show').css('pointer-events','none');
        }

        function OpenModalNewOrder() {
            $('#modalNewOrder').modal('show');
        }

        function CerrarAlert() {
            setInterval(function () {
                $('#AlertaBienvenida').alert("close");
            }, 6000);
            
        }

    </script>
</asp:Content>