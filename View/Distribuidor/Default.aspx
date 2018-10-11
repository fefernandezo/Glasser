<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Distribuidor/SiteDistr.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Dis_Default_Man" %>

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
    
     <div class="container-fluid">
         <asp:HiddenField runat="server" ID="HdnRutCli" />
          <asp:Panel ID="PanelAlerta" runat="server" Visible="false">
            <div class="container ">
                <div class="row">
                    <div class="col-12">
                        <div class="alert alert-success alert-dismissible fade show" role="alert" id="AlertaBienvenida">
                            <h4>Notificaciones</h4>
             
                        <div runat="server" id="MsjPortal"></div>
             

             <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
         </div>
                    </div>
                    
                </div>
            </div>
            
        </asp:Panel>

         <div class="d-flex justify-content-center">
             <div class=" form-inline">
                 <h1><asp:Label ID="LblCliente" runat="server"></asp:Label></h1>
                 <asp:Panel runat="server" ID="PnlBtnToMdlChangeCli" Visible="false">
                     <button type="button" class="btn btn-link" data-toggle="modal" data-target="#modalChangeCli">Cambiar Empresa</button>
                 </asp:Panel>
                 
             </div>
             
         </div>
         <div class="row">
             <!--sidebar-->
             <div class=" col-sm-12 col-md-12 col-xl-3 col-lg-12 mb-2">
                 <div class="card mb-1">
                     <div class="card-body sidebar-bg-gris">
                         <div class="row">
                             <asp:Button runat="server" ID="BtnInfoEmpr" OnClick="BtnInfoEmpr_Click" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Información de la empresa" />
                         </div>
                     </div>
                 </div>
                 <div class="card mb-1">
                     <div class="card-body sidebar-bg-gris">
                         
                         <div class="row">
                             <asp:Button runat="server" ID="BtnMisCotizaciones" OnClick="BtnMisCotizaciones_Click" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Mis cotizaciones" />
                         </div>
                         <div class="row">
                             <asp:Button runat="server" ID="BtnCotizar" OnClick="BtnCotizar_Click" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Nueva Cotización" />
                         </div>
                     </div>
                 </div>
                 <asp:Panel runat="server" ID="PnlPedidos">
                     <div class="card mb-1">
                         <div class="card-body sidebar-bg-gris">
                             <div class="row">
                                 <asp:Button runat="server" ID="BtnMisPedidos" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Mis Pedido" />
                             </div>
                             <div class="row">
                             <asp:Button runat="server" ID="BtnCrearPedido" CssClass="btn btn-link text-azul text-uppercase font-weight-bold" Text="Nuevo Pedido" />
                                 
                         </div>
                             

                         </div>
                     </div>
                 </asp:Panel>
                 
                 

             </div>
               
             <div class="col-sm-12 col-md-12 col-xl-9 col-lg-12">
                 <div id="accordion"  >
                     <div class="card mb-3 border-primary">
                            <div class="card-header bg-light border-primary">
                                <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseFin" class="glyphicon glyphicon-chevron-down"> Estado Financiero</a></h3>    
                            </div>
                                    <div id="CollapseFin" class="collapse show">
                                            <div class="card-body">
                                                <div class="row">
                                                     <div class="col">
                                                                
                                                         <div class="card border-info">
                                                             <div class="card-header bg-info text-white">
                                                                 <h5 class="text-center">Crédito disponible</h5>
                                                             </div>
                                                            <div class="card-body">
                                                               
                                                                <h3 class=" text-center"><asp:Label ID="lblCreditoDis" runat="server" Text=""></asp:Label></h3>
                                                            </div>
                                                        </div>
                                                            <br />
                                                        </div>
                                                        <div class="col">
                                                                
                                                        <div class="card border-info">
                                                            <div class="card-header bg-info text-white">
                                                                <h5 class="text-center">Cupo utilizado</h5>
                                                            </div>
                                                                <div class="card-body">
                                                                    <h3 class="text-center"><asp:Label ID="lblUtilizado" runat="server" Text=""></asp:Label></h3>
                                                                </div>
                                                        </div>
                                                            <br />
                                                        </div>
                                                        <div class="col">
                                                                
                                                        <div class="card border-info">
                                                            <div class="card-header bg-info text-white">
                                                                <h5 class="text-center ">Disponible para comprar</h5>
                                                            </div>
                                                                <div class="card-body">
                                                                    <h3 class="text-center"><asp:Label ID="LabelDisponible" runat="server" Text=""></asp:Label></h3>
                                                                    <asp:HiddenField ID="Hiddendisponible" runat="server" />
                                                                    
                                                                </div>
                                                        </div>
                                                            <br />
                        
                                                        </div>
                                                </div>
                                                       
                                            </div>
                                    </div>
                
                     </div>
                    
                    <div class="card mb-3 border-success">
                            <div class="card-header bg-light border-success">
                                <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseInfo" class="glyphicon glyphicon-chevron-down">Plazos de Entrega</a></h3>    
                            </div>
                                    <div id="CollapseInfo" class="collapse show">
                                         <div class="card-body ">
                                             <div class="row">
                                                 <div class="col">
                                                    <div class="card border-success">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Termopaneles</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="d-flex justify-content-center">
                                                                <h6><asp:Label ID="FechaTermo" runat="server"></asp:Label></h6>
                                                            </div>
                                                            <div class="d-flex justify-content-center">
                                                                <asp:Label ID="diastermo" runat="server"></asp:Label>*
                                                            </div>    
                                                        </div>
                                                    </div>
                                                </div>
                                                 <br />
                
                                                <div class="col">
                                                    <div class="card border-success">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Láminas</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="d-flex justify-content-center">
                                                                <h6><asp:Label ID="FechaLaminas" runat="server"></asp:Label></h6>
                                                            </div>
                                                            <div class="d-flex justify-content-center">
                                                                <asp:Label ID="DiasLaminas" runat="server"></asp:Label>*
                                                            </div>
                                                          
                                                                
                                                        </div>
                                                    </div>
                                                </div>
                                                 <br />
                                                <div class="col">
                                                    <div class="card border-success">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Arquitectura</h5>
                                                        </div>
                                                       <div class="card-body">
                                                           <div class="d-flex justify-content-center">
                                                               <h6><asp:Label ID="FechaArq" runat="server"></asp:Label></h6>
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
    <script type="text/javascript">
        $(document).ready(function () {
            CerrarAlert();

        });

        function Loading() {

            $('#modalChangeCli').modal('hide');
            
            
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Espere un momento...');
            $('#modalLoading').modal('show').css('pointer-events','none');
        }

        function CerrarAlert() {
            setInterval(function () {
                $('#AlertaBienvenida').alert("close");
            }, 4000);
            
        }

    </script>
</asp:Content>