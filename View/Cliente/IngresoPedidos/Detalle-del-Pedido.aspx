<%@ Page Language="C#" Title="Detalle del pedido" AutoEventWireup="true" MasterPageFile="~/View/Cliente/SiteCliente.Master" CodeFile="Detalle-del-Pedido.aspx.cs" Inherits="View_Cliente_IngresoPedidos_Detalle_del_Pedido" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        
         .steps {
  max-width: 500px;
  
  margin: 50px auto;
  box-sizing: border-box;
  /* End #steps ul*/
}
.steps ul,
.steps li {
  margin: 0;
  padding: 0;
  list-style: none;
}
.steps ul {
  display: table;
  width: 100%;
  /* End #steps li*/
}
.steps ul li {
  display: table-cell;
  position: relative;
  /* End .step*/
}
.steps ul li:first-child {
  width: 1px;
}
.steps ul li:first-child .step {
  /* Hide line before first step*/
}
.steps ul li:first-child .step:before {
  content: none;
}
.steps ul li .step {
  width: 40px;
  height: 40px;
  border: 1px solid;
  border-radius: 50%;
  border-color: #dedede;
  line-height: 37px;
  font-size: 15px;
  text-align: center;
  color: #999999;
  background-color: #dedede;
  float: right;
}
.text-inactivo{
    color:#999999;
    background-color: #dedede;
}
.entr{
    background-color: #ff6a00;
  border-color: #ff6a00;
  color: white;

}
.border-entr{
    border-color: #ff6a00;
}
.steps ul li .step:before {
  height: 4px;
  display: block;
  background-color: #dedede;
  position: absolute;
  content: "";
  right: 40px;
  left: 0px;
  top: 50%;
  cursor: default;
}
.steps ul li .step:after {
  display: block;
  transform: translate(-42px, 10px);
  color: #999999;
  content: attr(data-desc);
  font-size: 13px;
  line-height: 15px;
  min-width: 120px;
}
.steps ul li .step.active {
  border-color: #0095db;
  color: white;
  background: #0095db;
}
.steps ul li .step.active:before {
  background: linear-gradient(to right, #2ecc71 0%, #0095db 100%);
}
.steps ul li .step.active:after {
  color: #0095db;
  font-weight: 600;
}
.steps ul li .step.done {
  background-color: #2ecc71;
  border-color: #2ecc71;
  color: white;
}
.steps ul li .step.done:after {
  color: #2ecc71;
}
.steps ul li .step.done:before {
  background-color: #2ecc71;
  
}
.steps ul li .step.entr {
  background-color: #ff6a00;
  border-color: #ff6a00;
  color: white;
}
.steps ul li .step.entr:after {
  color: #ff6a00;
}
.steps ul li .step.entr:before {
  background-color: #ff6a00;
  background: linear-gradient(to left, #ff6a00 0%, #0095db 100%);
}
/* End #steps*/
         .rowsteps{
    padding-left:25px;
    padding-right: 27px;
}


         .fade-in {
	opacity: 1;
	animation-name: fadeInOpacity;
	animation-iteration-count: 1;
	animation-timing-function: ease-in;
	animation-duration: 1s;
}

@keyframes fadeInOpacity {
	0% {
		opacity: 0;
	}
	100% {
		opacity: 1;
	}
}

.boton {
  border: 2px solid black;
  background-color: white;
  color: black;
  padding: 14px 28px;
  font-size: 16px;
  cursor: pointer;
  border-radius:5px;
}

/* Naranjo */
.outline-entr {
  border-color: #ff6a00;
  color: #ff6a00;
}

.outline-entr:hover {
  background-color: #ff6a00;
  color: white;
}
.cristalA{
   
   
    
    border-bottom-left-radius:5px;
    border-top-left-radius:5px;
    text-align:center;
    padding: 30px 0;
    height:100px;
     background-color: rgba(0, 174, 230, 0.6);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(0, 174, 230, 0.6) -webkit-gradient(linear, left top, left bottom, from(#E2E5E4), to(rgba(0, 174, 230, 0.6))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(0, 174, 230, 0.6) -moz-linear-gradient(top, #E2E5E4, rgba(0, 174, 230, 0.6)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=rgba(0, 174, 230, 0.6)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;
}
.cristalB{
    
    
    border-bottom-right-radius:5px;
    border-top-right-radius:5px;
    text-align:center;
    padding: 30px 0;
    height:100px;
    background-color: rgba(0, 174, 230, 0.6);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(0, 174, 230, 0.6) -webkit-gradient(linear, left top, left bottom, from(#E2E5E4), to(rgba(0, 174, 230, 0.6))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(0, 174, 230, 0.6) -moz-linear-gradient(top, #E2E5E4, rgba(0, 174, 230, 0.6)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=rgba(0, 174, 230, 0.6)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;
}
.separador{
    border-top: 10px solid rgba(107, 107, 107, 0.79);
    border-bottom: 10px solid black;
    background-color:rgba(0, 155, 245, 0.05);
    border-color:rgba(194, 193, 193, 0.3);
    padding:unset;
    height:100px;
}
.sep-br{
    border-color:#cd7f32;
}
.sep-ma{
    border-color:rgba(194, 193, 193, 0.67);

}
.sep-ti{
    border-color:rgba(114, 114, 114, 0.50);
}
.cri-inc{
    background-color: rgba(0, 174, 230, 0.73);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(0, 174, 230, 0.73) -webkit-gradient(linear, left top, left bottom, from(#E2E5E4), to(rgba(0, 174, 230, 0.73))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(0, 174, 230, 0.73) -moz-linear-gradient(top, #E2E5E4, rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;

}
.cri-br{
    background-color: rgba(233, 213, 54, 0.81);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(0, 174, 230, 0.73) -webkit-gradient(linear, left top, left bottom, from(rgba(233, 213, 54, 0.81)), to(rgba(0, 174, 230, 0.73))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(0, 174, 230, 0.73) -moz-linear-gradient(top, rgba(233, 213, 54, 0.81), rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=rgba(233, 213, 54, 0.81), endColorstr=rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;

}
.cri-blue{
    background-color: rgba(10, 0, 255, 0.81);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(0, 174, 230, 0.73) -webkit-gradient(linear, left top, left bottom, from(rgba(10, 0, 255, 0.81)), to(rgba(0, 174, 230, 0.73))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(0, 174, 230, 0.73) -moz-linear-gradient(top, rgba(10, 0, 255, 0.81), rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=rgba(10, 0, 255, 0.81), endColorstr=rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;

}
.cri-grey{
    background-color: rgba(126, 126, 126, 0.70);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(0, 174, 230, 0.73) -webkit-gradient(linear, left top, left bottom, from(rgba(126, 126, 126, 0.70)), to(rgba(0, 174, 230, 0.73))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(0, 174, 230, 0.73) -moz-linear-gradient(top, rgba(126, 126, 126, 0.70), rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=rgba(126, 126, 126, 0.70), endColorstr=rgba(0, 174, 230, 0.73)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;

}
.cri-sa{

    background-color: rgba(1, 1, 1, 0.1);
    /* For WebKit (Safari, Chrome, etc) */
    background: rgba(1, 1, 1, 0.1) -webkit-gradient(linear, left top, left bottom, from(rgba(1, 1, 1, 0.1)), to(rgba(1, 1, 1, 0.7))) no-repeat;
    /* Mozilla,Firefox/Gecko */
    background: rgba(1, 1, 1, 0.1) -moz-linear-gradient(top, rgba(1, 1, 1, 0.1), rgba(1, 1, 1, 0.7)) no-repeat;
    /* IE 5.5 - 7 */
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=rgba(1, 1, 1, 0.1), endColorstr=rgba(1, 1, 1, 0.7)) no-repeat;
    /* IE 8 */
    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr=#E2E5E4, endColorstr=#E2E5E4)" no-repeat;

}
.cri-sem{

}
.cri-lowe{

}
.cri-lam{
    background: linear-gradient(to right, 
	                              transparent 0%, 
	                              transparent calc(50% - 0.95px), 
	                              blue calc(50% - 0.8px), 
	                              blue calc(50% + 0.8px), 
	                              transparent calc(50% + 0.95px), 
	                              transparent 100%); 
    background-color:rgba(0, 155, 245, 0.20);
}

.cri-lamac{
    background: linear-gradient(to right, 
	                              transparent 0%, 
	                              transparent calc(50% - 2.5px), 
	                              blue calc(50% - 0.8px), 
	                              blue calc(50% + 0.8px), 
	                              transparent calc(50% + 2.5px), 
	                              transparent 100%); 
    background-color:rgba(0, 155, 245, 0.20);
}

.exterior{
    text-align:center;
    writing-mode:vertical-rl;
}
.interior{
    text-align:center;
    writing-mode:vertical-lr;
    
}

.item{
             padding-left:8px;
             padding-right:8px;
         }
         .headercol{
             margin-left:auto;
             margin-right:auto;
         }
         .noexistecode{
             background-color:red;
             color:white;

         }

#canvas1{
  
  height:80px ;
  width: inherit;
 
  
}





        
        
       
    </style>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="container">
        
        <div class="row rowsteps">
            <div class="col-lg-4">
                <br />
            </div>
            <div class="col-lg-5 col-xs-12">
                <div class="steps">
                    <ul>
                        <li><a id="InfPed"><div class="step done" data-desc="Información del Pedido">1</div></a></li>
                        <li><a id="InfDet"><div class="step active" data-desc="Detalle">2</div></a></li>  
                        <li><a id="InfDesp"><div id="divInfodes" class="step" data-desc="Información de entrega">3</div></a></li>
                        
                    </ul>
                </div>
            </div>
             
        </div>
        <div class="row mb-5 justify-content-end">
            <asp:Button runat="server" CssClass="btn btn-primary" ID="BtnEnviarPedido" Text="Enviar Pedido" Enabled="false" />
        </div>
    </div>
    <div class="container">
        <div id="accordion">
            <div class="card border-success mb-3">
                <div class="card-header bg-success text-white" id="headerInfPed" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseInfoPed">
                    <h5><i  class="fas fa-chevron-down"> Información del pedido</i></h5>
                </div>
                <div id="CollapseInfoPed" class="collapse">
                    <div class="card-body">
                        <div class="row mb-2">
                            <div class="col-md-8 col-12 mb-3">
                                <div class="col-md-8 col-12">
                                    <h6>Nombre del Pedido: <asp:Label runat="server" ID="LblNombrePedido"></asp:Label></h6>
                                </div>
                               

                            </div>
                            <div class="col-md-4 col-12">
                                <asp:Panel runat="server" ID="PanelTotales" Visible="false">
                                    <div class="card">
                                        <ul class="list-group list-group-flush">
                                            <li class="list-group-item"><asp:Label runat="server" ID="LblNetoTotal" Font-Bold="true">Neto:$180.000</asp:Label></li>
                                            <li class="list-group-item"><asp:Label runat="server" ID="LblIVAtotal" Font-Bold="true" >IVA:$20.000</asp:Label></li>
                                            <li class="list-group-item"><asp:Label runat="server" ID="LblMontoTotal" Font-Bold="true" >Total:$199.000</asp:Label></li>
                                        </ul>
                                    </div>
                                </asp:Panel>
                            </div>
                            
                            
                        </div>
                        <div class="row mb-2">
                            <div class="col">
                                <h6>Observaciones: </h6><asp:Label runat="server" ID="LblObservaciones"></asp:Label>
                            </div>
                            
                        </div>
                        
                    
                    </div>
                </div>
            </div>
            <div id ="MsgcardInfDesp"></div>
            <div class="card mb-3 " id="cardInfDesp">
                <div class="card-header" data-toggle="collapse" id="headerInfDesp" data-parent="#accordion" data-target="#CollapseDesp">
                    <h5><i  class="fas fa-chevron-down"> Información de entrega</i></h5>
                </div>
                <div id="CollapseDesp" class="collapse">
                    <div class="card-body">
                        <div class="row mb-3 justify-content-center">
                            <asp:Button runat="server" ID="BtnIngresoInfDesp" CssClass="boton outline-entr" />
                        </div>
                   
                        <asp:Panel runat="server" ID="PanelInfDespEnabled" Visible="false">
                            <div class="row mb-3">
                                <div class="col-12">
                                    <asp:Label runat="server" ID="LblFechaDesp" Font-Bold="true">Fecha de Entrega: 19-12-1985</asp:Label>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-12">
                                    <asp:Label runat="server" ID="LblTipoDesp" Font-Bold="true">Tipo de despacho: Despacho a domicilio</asp:Label>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-12">
                                    <asp:Label runat="server" ID="LblDireccion" Font-Bold="true">Dirección: Blas Vial 8683, Las Condes, RM</asp:Label>
                                </div>
                            </div>

                        </asp:Panel>

                    </div>
                </div>
            </div>
            <div class="card border-primary mb-3">
                <div class="card-header bg-primary text-white" id="headerDetPed" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseDetPed">
                    <h5><i  class="fas fa-chevron-down"> Detalle del pedido</i></h5>
                </div>

                <div id="CollapseDetPed" class="collapse show">
                    <div class="card-body">
                        
                        <div class="table-responsive">
                            <div id="DivTablaDetail"></div>
                           
                        </div>
                    
                    </div>
                </div>
            </div>

     </div>
        
    </div>
    <div class="table-responsive">
        <asp:GridView runat="server" ID="GridDetalle" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
    </div>
     

    <!--MODAL ERROR CODE-->
    <div class="modal fade" id="modalErrorCode" tabindex="-1" role="dialog" aria-labelledby="ModalLabelError" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
                        <h3 class="modal-title" id="ModalLabelError">Error en Item  </h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-12"><h6>El sistema no reconoce la composición "<asp:Label runat="server" ID="LblTermino"></asp:Label>"</h6></div>
                    <div class="col-12">Por favor seleccione y agregue el término a nuestro diccionario para registrarlo.</div>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdateModalItem" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row justify-content-center">
                            <div class="col-1 exterior">Exterior</div>
                            <div class="col-1 cristalA" runat="server" id="CristalA"></div>
                            <div class="col-1 separador" runat="server" id="Separador"><canvas id="canvas1" class="snow"></canvas></div>
                            <div class="col-1 cristalB" runat="server" id="CristalB"></div>
                            <div class="col-1 interior">Interior</div>
                        </div>
                        <div class="row mb-3 justify-content-center">
                            Corte transversal del Termopanel
                        </div>
                        <div class="row mb-4 justify-content-center">                    
                            <div class="col-4"><asp:DropDownList ID="DDLCristalA" OnSelectedIndexChanged="DDLCristalA_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList></div>
                            <div class="col-4"><asp:DropDownList ID="DDLSeparador" OnSelectedIndexChanged="DDLCristalA_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList></div>
                            <div class="col-4"><asp:DropDownList ID="DDLCristalB" OnSelectedIndexChanged="DDLCristalA_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList></div>
                        </div>
                        <div class="row mb-3 justify-content-start">
                            <div class="col-12">
                                <asp:CheckBox runat="server" CssClass="form-check" AutoPostBack="true" ID="ChkGasArgon" OnCheckedChanged="DDLCristalA_SelectedIndexChanged" Text=" Con Gas Argón" />
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ChkGasArgon" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="DDLCristalA" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="DDLCristalB" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="DDLSeparador" EventName="SelectedIndexChanged" />
                    </Triggers>

                </asp:UpdatePanel>
                
                
                
                
                
               
              
               
                
            </div> 
            <div class="modal-footer">
                <div class="btn-group btn-group-justified" role="group" aria-label="...">
                    
                    <div class="btn-group" role="group">
                        <asp:Button runat="server" ID="BtnAgregarTermino" CssClass="btn btn-outline-info" Text="Agregar término" />
                    </div>
                    <div class="btn-group" role="group">
                          <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>
                    </div>
                </div>
                
                
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>
    <script type="text/javascript">
        $(document).ready(function () {
            
            $('.card-header').hover(function () {
                $(this).css('cursor', 'pointer');
            });

            $('.card-header').click(function () {
                var _ID = $(this).attr('id');
                SelectFuncxHeader(_ID);
            });

            $('.step').hover(function () {
                $(this).css('cursor', 'pointer');
            });

            
            

            
            

            
            $('#InfPed').click(function () {
                SelectFuncxHeader('headerInfPed');
            });
            $('#InfDesp').click(function () {
                SelectFuncxHeader('headerInfDesp');
            });
            $('#InfDet').click(function () {
                SelectFuncxHeader('headerDetPed');
            });

            
        });

        function SetClassDVH(claseA,claseSep,claseB,Gas) {
            $('#<%= CristalA.ClientID%>').removeClass().addClass(claseA);
            $('#<%= CristalB.ClientID%>').removeClass().addClass(claseB);
            $('#<%= Separador.ClientID%>').removeClass().addClass(claseSep);
            if (Gas == '1') {
                AddArgon();
            }
            else {
                ClearArgon();
            }
            console.log("CristalA: " + claseA + " Separador: " + claseSep + " ClaseB: " + claseB);
        }
        

        function SelectFuncxHeader(HeaderId) {
            if (HeaderId=='headerDetPed') {
                AbreDetalle();
            }
            else if (HeaderId=='headerInfDesp') {
                AbreInfDesp();
            }
            else if (HeaderId=='headerInfPed') {
                 AbreInfPed();
            }
        }

        function AbreDetalle() {
            
                $('html, body').animate({
                    scrollTop: $('div.border-primary').offset().top},
                    1000
                );
                $('#CollapseDesp').collapse('hide');
                $('#CollapseInfoPed').collapse('hide');
                $('#CollapseDetPed').collapse('show');
            

        }

        function AbreInfPed() {
             
                $('html, body').animate({
                    scrollTop: $('.border-success').offset().top},
                    1000
                );
                $('#CollapseDesp').collapse('hide');
                $('#CollapseInfoPed').collapse('show');
                $('#CollapseDetPed').collapse('hide');
            

        }

        function AbreInfDesp() {
          
                
                $('html, body').animate({
                    scrollTop: $('#cardInfDesp').offset().top},
                    1000
                );
                 $('#CollapseDesp').collapse('show');
                $('#CollapseInfoPed').collapse('hide');
                $('#CollapseDetPed').collapse('hide');
                
           
        }

        function EnableInfDesp() {
            $('#divInfodes').addClass('entr');
            $('#headerInfDesp').addClass('entr');
            $('#cardInfDesp').addClass('border-entr');


        }


        function AddMsgInfDesp() {
            
            var elemento = '<div class="alert alert-warning alert-dismissible fade-in show" role="alert" id="AlertaBienvenida">'
                            +'<strong>Estimado cliente:</strong> debe ingresar la información de entrega del pedido.'
                + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>';

            setTimeout(function() {
                $('#MsgcardInfDesp').append(elemento);
            }, 1000);
           
                
           
            
        }

        //funciones de colores
         function Snow(){
  
	var canvas = document.getElementById("canvas1");
	var ctx = canvas.getContext("2d");
	var W = window.innerWidth;
	var H = window.innerHeight;
	canvas.width = W;
	canvas.height = H;
	var mp = 150;
	var particles = [];
  
             for (var i = 0; i < mp; i++) {
                 var l = Math.random();
        particles.push({

			x: Math.random()*W + l,
			y: Math.random()*H,
			r: Math.random()*4+2,
			d: Math.random()*mp+8
		})
	}
	
	// draw the snowflakes
	function draw() {
		ctx.clearRect(0, 0, W, H);
		ctx.fillStyle = "rgb(12, 5, 5)";
		ctx.beginPath();
		for(var i = 0; i < mp; i++) {
			var p = particles[i];
			ctx.moveTo(p.x, p.y);
			ctx.arc(p.x, p.y, p.r, 15, Math.PI*2, true);
		}
		ctx.fill();
		update();
	}
  
	var angle = 0;
  
  // move the snowflakes
	function update()  {
		angle += 1.5;
		for(var i = 0; i < mp; i++) {
			var p = particles[i];
			p.y += Math.cos(angle+p.d) + 1 + p.r;
			p.x += Math.sin(angle) * 3;
			if(p.x > W+5 || p.x < -5 || p.y > H) {
				if(i%3 > 0) {
					particles[i] = {x: Math.random()*W, y: -5, r: p.r, d: p.d};
				} else {
					if(Math.sin(angle) > 0) {
						particles[i] = {x: -5, y: Math.random()*H, r: p.r, d: p.d};
					} else {
						particles[i] = {x: W+5, y: Math.random()*H, r: p.r, d: p.d};
					}
				}
			}
		}
	}
	
	// animation loop
	setInterval(draw, 300);
}


        function AddArgon() {
            $('#canvas1').removeClass().addClass('visible');
            Snow();
            
            
        }
        function ClearArgon() {
            $('#canvas1').removeClass().addClass('invisible');
        }

        //funciones de colores---- end

        function GetDetail(IdPedido, TokenId) {

            $('#DivTablaDetail').html('');

            var datafrom = "{'ID':'" + IdPedido + "','TokenId':'" + TokenId + "'}";
            $.ajax({
                type: 'post',
                url: '../ServiciosWeb/DatosDeServicios.asmx/GetListTempDetail',
                data: datafrom,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var table = '<table id="TablaP" class="table table-bordered">';
                    table += '<strong><thead class="bg-primary" ><tr class="text-uppercase text-white text-center">'
                        + '<th class="cabeza" id="hea1">Referencia</th>'
                        + '<th class="cabeza" id="hea2">Terminología</th>'
                        + '<th class="cabeza" id="hea3">Cantidad</th>'
                        + '<th class="cabeza" id="hea4">Ancho</th>'
                        + '<th class="cabeza" id="hea6">Alto</th>'
                        + '<th class="cabeza" id="hea7">Neto</th>'
                        + '<th class="cabeza" id="hea8">Iva</th>'
                        + '<th class="cabeza" id="hea9">Total</th>'
                
                        + '</tr></thead></strong><tbody>';
                    $.each(msg.d, function () {
                        
                         var existe = this._ExisteCode;

                        var popover = '';

                        

                        if (!existe) {
                            table = table + '<tr class="filas noexistecode"><td>' + this._Referencia + '</td>';
                            popover = '<span class="d-inline-block" data-toggle="popover" title="Atención!" data-content="este item no encuentra una terminología asociada en el diccionario"></span>';
                            table = table + '<td>' + this._Terminologia + '</td>'+
                                '<td>' +popover+ this._Cantidad + '</td>' ;
                        }
                        else {
                            table = table + '<tr class="filas"><td>' + this._Referencia + '</td>';

                            table = table + '<td>' + this._Terminologia + '</td>'+
                                '<td>'+ this._Cantidad + '</td>' ;
                        }

                        table = table + '<td>' + this._Ancho + '</td>' +
                                        '<td>' + this._Alto + '</td>' +
                                        '<td>' + this.Neto + '</td>' +
                                        '<td>' + this.Iva + '</td>' +
                                        '<td>' + this.Total + '</td>';
                    
                
               
                        table = table + '<td style="display:none;">' + this.Id + '</td>';
                       
                       
                    });

                    table = table + '</tbody></table>';

                    $('#DivTablaDetail').append(table);
                    $("[data-toggle='popover']").popover('show');
                    
                    

                    $('.noexistecode').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
                    });
                    $('.noexistecode').mouseleave(function () {

                        $(this).css('text-decoration', '');
                    });

                    $('.noexistecode').click(function () {
                        var _IDItem = $(this).find("td").eq(8).html();
                        var _Termino = $(this).find("td").eq(1).html();
                        $('#<%=LblTermino.ClientID%>').html(_Termino);
                        $("#modalErrorCode").modal("show");
                        $("[data-toggle='popover']").popover('hide');

                    });

           


                },
                error: function (jqXHR, textStatus, errorThrown) {

                    alert('Error :=> ' + textStatus + '--- ' + errorThrown);
                }
            });
        }
    </script>
</asp:Content>


