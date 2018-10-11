<%@ Page Title="Consumo" Language="C#" MasterPageFile="~/View/Planificacion/SitePlan.Master" AutoEventWireup="true" CodeFile="Consumo.aspx.cs" Inherits="_Consumo" %>

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
    <asp:UpdatePanel ID="UpdatepnlGenDetail" runat="server" >
                  <ContentTemplate>
    
    <div class="row mb-5">
        <div class="col-12 text-center">
             <h1>Consumos</h1>
        </div>
            
         </div>
     <div class=" container">
         
         <div class="row mb-4">
             <div class="col-12 text-center mb-2">
                 <asp:Button runat="server" ID="BtnListOts" OnClick="BtnListOts_Click" ToolTip="Ultimas 20 OT sin consumo" Text="Ver listado OT sin consumo" CssClass="btn btn-naranjo" />
             </div>
             <div class="col-12 text-center mb-2">
                 <asp:Button runat="server" ID="BtnGenConsumo" OnClientClick="javascript:GenConsumo();" OnClick="BtnGenConsumo_Click" Text="Generar Consumo para estos items" CssClass="btn btn-lila" Visible="false" />
             </div>
         </div>

         <asp:Panel ID="panelDetalle" runat="server" Visible="false" >
             <asp:GridView ID="GrdDetalle" runat="server" CssClass="table table-bordered">

             </asp:GridView>
         </asp:Panel>
         
     </div>
                     
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnGetDetailsOT" />
        </Triggers>
                </asp:UpdatePanel>
    <!--Modal Listado OT-->
    <asp:UpdatePanel ID="Updatemodal" runat="server">
        <ContentTemplate>
            <div class="modal fade"  id="modalListaOT" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header fondo-naranjo">
            
            <h3>Listado de OT sin consumos</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
            
          <div class="modal-body">
              
              
                      <div class="row">
                          <div id="DivListaOT" runat="server" class=" table-responsive">
                              <asp:GridView runat="server" ID="GrdOT" CssClass="table card-border-naranjo" AutoGenerateColumns="false">
                                  <Columns>
                                      <asp:TemplateField HeaderText="N° OT">
                                          <ItemTemplate>
                                              <asp:CheckBox runat="server" ID="ChkOT" Text='<%#Eval("_NUMOT")%>' />
                                          </ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:BoundField DataField="_REFERENCIA" HeaderText="Cliente" />
                                      <asp:BoundField DataField="_TIPOOT" HeaderText="Tipo" />
                                      <asp:BoundField DataField="_ITEMS" HeaderText="Cant Items" />
                                      <asp:BoundField DataField="_FIOT" HeaderText="Fecha Ingreso" DataFormatString="{0:dd-MM-yyyy}" />
                                  </Columns>
                              </asp:GridView>
                          </div>
                      
              </div>
                      
              
          </div>
            
          <div class="modal-footer  fondo-naranjo">
              
              
                      <asp:Button runat="server"  OnClientClick="javascript:GenDetail();" ID="BtnGetDetailsOT" OnClick="BtnGetDetailsOT_Click" CssClass=" btn btn-outline-light" Text="Ir a Detalle" />
                 

              
              
              
          </div>
          
       </div>
  </div>
</div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnListOts" />
        </Triggers>
    </asp:UpdatePanel>
    
                       
                      
                  
              
    <!--prgress spinner del detalle
    <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="UpdatepnlGenDetail" DynamicLayout="true" >
                        <ProgressTemplate>
                            <div class="modal show"  id="modalLista4OT" tabindex="-1" role="dialog">
                                 <div class="modal-dialog modal-lg" role="document">
                                     <div class="modal-content">
                                         
                           
                                   <div class="spinner">
                                        <div class="bounce1"></div>
                                        <div class="bounce2"></div>
                                        <div class="bounce3"></div>
                                    </div>
                               <h3 class="text-center">Espere un momento... estamos generando el listado</h3>
                               
                            
                           
                            
                        
                                     </div>
                                 </div>

                            </div>
                            
                        
                            
                        </ProgressTemplate>
        
                      
                </asp:UpdateProgress> 
    -->
    
     
    <script type="text/javascript">

        $(document).ready(function () {
             /*$('#LoadingMsj').html('Espere un momento... estamos generando el listado');
                 $('#modalLoading').modal('show');*/
             });
        function OpenListaOT() {
            $('#modalListaOT').modal('show');
            
        }

        function GenDetail() {
            $('#modalListaOT').modal('hide');
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Espere un momento... se está generando el listado');
            $('#modalLoading').modal('show').css('pointer-events','none');
        }
        function GenConsumo() {
            
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Realizando cálculos...');
            $('#modalLoading').modal('show').css('pointer-events', 'none');
            var x = true;
            setInterval(function () {
                if (x) {
                    $('#LoadingMsj').html('Espere un momento... se está generando el consumo');
                    x = false;
                }
                else {
                    $('#LoadingMsj').html('Realizando cálculos...');
                    x = true;
                }

                
            }, 2500);

        }

    </script>


</asp:Content>