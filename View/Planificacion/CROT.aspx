<%@ Page Title="Inicio" Language="C#" EnableEventValidation="true" MasterPageFile="~/View/Planificacion/SitePlan.Master" AutoEventWireup="true" CodeFile="CROT.aspx.cs" Inherits="View_Planificacion_CROT" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>


     
    </style>
    
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
     <div class="modal"  id="modalLoading" tabindex="-1" role="dialog">
                                 <div class="modal-dialog modal-lg modal-dialog-centerd" role="document">
                                      <div class="overlay">
                           <div class="modalprogress">
                                   <div class="sk-fading-circle">
                                        <div class="sk-circle1 sk-circle"></div>
                                        <div class="sk-circle2 sk-circle"></div>
                                        <div class="sk-circle3 sk-circle"></div>
                                        <div class="sk-circle4 sk-circle"></div>
                                        <div class="sk-circle5 sk-circle"></div>
                                        <div class="sk-circle6 sk-circle"></div>
                                        <div class="sk-circle7 sk-circle"></div>
                                        <div class="sk-circle8 sk-circle"></div>
                                        <div class="sk-circle9 sk-circle"></div>
                                        <div class="sk-circle10 sk-circle"></div>
                                        <div class="sk-circle11 sk-circle"></div>
                                        <div class="sk-circle12 sk-circle"></div>
                                    </div>
                               <div class="text-center" id="TextProgress">prueba</div>      
                               <p></p>
                               <div class="text-center" id="TextProgress2">pruebA2</div>
                            
                           </div>
                            
                        </div>
                                 </div>

                            </div>
     <div class="container">
         <div class="contenedor">
             <div class="row text-center">
             <h1>Creación de OT</h1>
         </div>
         <div class="row">
             <div class="form-inline">
                 <asp:TextBox ID="TxtBuscarNVV" runat="server" CssClass="form-control"  placeholder="Buscar NVV"></asp:TextBox>
                 <asp:Button runat="server" CssClass="btn btn-success" Text="Buscar" ID="BtnBuscarNvv" OnClick="BtnBuscarNvv_Click" />
                 
             </div>
         </div>


         <br />
         <br />
         <asp:Panel runat="server" ID="PanelDetail" Visible="false">
             <div class="row borde">
                 <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Rut:"></asp:Label></div>
                 <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Blue" ID="LblKOEN">99558220-1</asp:Label></div>

                 <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Cliente:"></asp:Label></div>
                 <div class="col-md-6 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Blue" ID="LblNOKOEN">VENTALPLASTIC SPA</asp:Label></div>
             </div>
             <br />
             <div class="row borde">
                    <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Orden de Producción:"></asp:Label></div>
                    <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Blue" ID="LblOP">141258</asp:Label></div>
                    
                    <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Nota de Venta:"></asp:Label></div>
                    <div class="col-md-2 col-6"><asp:Label runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Blue" ID="LblNVV">PH00061109</asp:Label></div>
             </div>
             
             <div class="row borde fechacont">
                 <div class="col-6 col-md-2">
                     <asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Fecha de Ingreso:"></asp:Label><span> </span>
                 </div>
                 <div class="col-6 col-md-3">
                     <asp:Label runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Blue" ID="LblFEEMLI">08-05-2018</asp:Label>
                 </div>
                 
             </div>
             
         
                              <asp:UpdatePanel ID="UpdatePanelSolCodExp" runat="server" UpdateMode="Conditional">
                                  <ContentTemplate>
                                      <div class="row borde fechacont">
                                      <div class="col-12 col-md-3">
                                                <asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Fecha de Entrega a Cliente:"></asp:Label><span> </span>
                                      </div>
                                      <div class="col-12 col-md-6">
                                          <div class="form-inline">
                                                <asp:TextBox Enabled="false" CssClass="form-control" runat="server" Font-Size="Large" placeholder="dd-mm-yyyy" Font-Bold="true" ForeColor="Blue" ID="LblFEERLI"></asp:TextBox>
                                                <asp:Button runat="server" ID="BtnCamFech1" OnClick="BtnCamFech_Click" Text="Cambiar" CssClass="btn btn-outline-info" />
                                          </div>
                                      </div>
                                          </div>
     
                                  </ContentTemplate>
                                     <Triggers>
                                           <asp:AsyncPostBackTrigger ControlID="BtnCamFech1" EventName="Click" />
                                     </Triggers>
                              </asp:UpdatePanel>                                  
                 
             
             
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                          <ContentTemplate>
                              <div class="row borde fechacont">
                                <div class="col-12 col-md-3">
                                    <asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Fecha de Entrega Producción:"></asp:Label>
                                </div>
                                <div class="col-12 col-md-6">
                                    <div class="form-inline">
                                        <asp:TextBox Enabled="false" CssClass="form-control" runat="server" Font-Size="Large" placeholder="dd-mm-yyyy" Font-Bold="true" ForeColor="Blue" ID="TextENTPROD"></asp:TextBox>
                                        <asp:Button runat="server" ID="BtnCamFech2"  OnClick="BtnCamFech2_Click" Text="Cambiar" CssClass="btn btn-outline-info" />
                                    </div>
                                </div>
                                  </div>
                          </ContentTemplate>
                                     <Triggers>
                                           <asp:AsyncPostBackTrigger ControlID="BtnCamFech2" EventName="Click" />
                                     </Triggers>
                  </asp:UpdatePanel>

             
             <br />
                <asp:UpdatePanel ID="PanelTIPOOT" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="VaoNO" />
                        <asp:HiddenField runat="server" ID="Komode" />
                        <asp:HiddenField runat="server" ID="Tipoop" />

                        <asp:Panel ID="paneltipoot2" runat="server" Visible="false">
                            <div class="form-inline">
                            <asp:DropDownList runat="server" ID="DrpTIPOOT" CssClass="form-control" ></asp:DropDownList>
                           
                            <asp:Button runat="server" ID="BtnDrTIPOOT" CssClass="btn btn-success" Text="Agregar" OnClick="BtnDrTIPOOT_Click" />
                        </div>
                        
                        <br />
                        <br />
                        </asp:Panel>


                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnDrTIPOOT" EventName="Click" />
                        
                        
                    </Triggers>
                </asp:UpdatePanel>
             
                   
                  <asp:UpdatePanel ID="GenerarOT" runat="server">
                      <ContentTemplate>
                          
                   
                          <div class="row justify-content-center" >
                        <asp:Button runat="server" ID="BtnGenOT"  CssClass="btn btn-primary btn-lg"   OnClientClick="javascript:Counter();" Text="Generar OT" ValidationGroup="GenerarOTPanel" OnClick="BtnGenOT_Click" />   
                              <div class="text-center"><asp:Label runat="server" ID="LblAdvance"></asp:Label></div>
                    </div>
                          

                                <div class="modal fade"  id="ModalIngOT" tabindex="-1" role="dialog" aria-labelledby="TitleMsg">
                                    <div class="modal-dialog modal-sm" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                
                                                <h4 class="modal-title" id="TitleIngOT">Mensaje</h4>
                                                <button type="button" class="close" onclick="Redirecciona()"><span aria-hidden="true">&times;</span></button>
                                            </div>
                                            <div class="modal-body">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <h4><asp:Label runat="server" ID="LblIngOT"></asp:Label></h4>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Panel runat="server" CssClass="text-center" ID="PanelIngOT"></asp:Panel>
             
                                        </div>
          
                                    </div>
                                    </div>
                                </div>
                      </ContentTemplate>
                      
                  </asp:UpdatePanel>
                 
                    
               
             <br />
             
                <div class="row">
                    <div class="table table-responsive">
                        <asp:GridView ID="TablaDetail" runat="server" CssClass="table" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="ITEM_ALFAK" HeaderText="Item" >
                                    <ItemStyle CssClass="item" />
                                    <HeaderStyle CssClass="headercol" />
                                </asp:BoundField>
                                <asp:BoundField DataField="KOPR" HeaderText="Código">
                                    <ItemStyle CssClass="item" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NOKOPR" HeaderText="Detalle">
                                    <ItemStyle CssClass="item" />
                                </asp:BoundField>
                                <asp:BoundField DataField="KOMODE" HeaderText="Modelo">
                                    <ItemStyle CssClass="item" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cant.">
                                    <ItemStyle CssClass="item" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ANCHO" HeaderText="Ancho">
                                    <ItemStyle CssClass="item" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LARGO" HeaderText="Largo" >
                                    <ItemStyle CssClass="item" />
                                </asp:BoundField>
                                
                                
                                
                                
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                    </div>
                </div>
         </asp:Panel>
            
         </div>
         
        
                
                 
                
     </div>


    <div class="modal fade"  id="ModalMsg" tabindex="-1" role="dialog" aria-labelledby="TitleMsg">
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
      
              <asp:Button CssClass="btn btn-info" ID="MdlBtnOkMsg" runat="server" Text="Aceptar"  OnClick="MdlBtnOkMsg_Click" />
             
          </div>
          
       </div>
  </div>
</div>

  

    <script type="text/javascript">
        $(document).ready(function () {

            
        });
        
       
        

        function Redirecciona() {
             url = "../Planificacion/CROT.aspx";
                $(location).attr("href", url);
                
        }

        function Counter() {

            $('html').bind('keydown', function(e){
                    if(e.keyCode == 13 || e.keyCode==32){
                    return false;
                    }
            });
            $('#modalLoading').modal('show').css('pointer-events', 'none');
            

                var Not= "0000" + $('#<%=LblOP.ClientID %>').html();
                var counter = 0;
                var c = 0;
            var MsgStatus = "";
            var Estado = 0;
            var min = 0;
            var seg = 0;
                var i = setInterval(function(){
                    seg = c / 10;
                    if (seg==60) {
                        min++;
                        c = 0;
                    }
                      
                    $("#TextProgress2").html('Tiempo: ' + min + ':' + Math.round(seg));
                    var datafrom = "{'NUMOT': '" + Not +"'}";
                    //AJAX
                    $.ajax({
                        type: 'post',
                        url: '../ServiciosWeb/WSPlanificacion.asmx/StatusOT',
                        data: datafrom,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (msg) {
                            $.each(msg.d, function () {

                                MsgStatus = this._MsgStatus;
                                Estado = this._Status;
                                
                                counter = Estado;
                            });
                            $("#TextProgress").html(MsgStatus + '   ' + Estado + '%');
                                //alert(MsgStatus + '   ' + Estado + '%');
                            
                        }, error: function (jqXHR, textStatus, errorThrown) { alert('Error :=> ' + textStatus + '--- ' + errorThrown); }

                    });
                    
                    c++;
      
                    if(counter == 100) {
                        clearInterval(i);
                    }
            }, 100);

            
        }



    </script>
</asp:Content>