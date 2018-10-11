<%@ Page Title="Detalle del pedido" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.master" AutoEventWireup="true" CodeFile="Detalle-del-Pedido.aspx.cs" Inherits="Cliente_DetallePedido" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /*pasos del formulario de envio de pedido*/
/* Darkgrey */
/* Lightgrey */
/* Grey */
/* White */
/* Red */
/* Green */
/* Black */
/* blue */
* {
  box-sizing: border-box;
}

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

.panelcabecera {
    background-color: #ff6a00;
    border-color: #ff6a00;
    color: white;
}
.panelcuerpo {
    border-color: #ff6a00;
}
.panelcabecerainfped {
    background-color:  #2ecc71;
    border-color:  #2ecc71;
    color: white;
}
.panelcuerpoinfped {
    border-color:  #2ecc71;
}
.btnenviar{
    padding-top:50px;
    padding-bottom:50px;
}
.rowsteps{
    padding-left:25px;
    padding-right: 27px;
}
.observacion{
    padding-bottom:20px;
}
.neto{
    border: 1px solid;
    border-color:#2ecc71;
    border-bottom: 0px;
    border-left:0px;
    border-right:0px;
    
}
.adddirvisible{
    display: normal;
}
.adddirnotvisible{
    display:none;
}
.seleccionhover{
   
}
.seleccionhover:hover{
    cursor: pointer;
    background: rgba(1, 46, 220, 0.9);
    color:white;
    
}
.tablafechas{
    
    
    height:400px;
    overflow-y:auto;
    display:block;
    border:none;
}
cabecera{
    border:none;
    background: rgba(0, 139, 20, 0.75);
    color:white;
    
}
.seleccionado{
    background:rgba(1, 46, 220, 0.58);
    color:white;
}

thead{
    position:static;
}

  
  
  
.contenedortablafechas{
    padding-left:5%;
    padding-right:5%;
    
}
.recomendado{
    background:rgba(7, 161, 0, 0.95);
    color:white;
}

.popover{
    background:rgba(7, 161, 0, 0.95);
    color:white;
}
.invisible{
    display:none;
}
.visible{
    display:normal;
}
.express{
    background:rgba(173, 0, 0, 0.99);
    color:white;
}


    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <asp:HiddenField runat="server" ID="HiddenStatus" />
    <asp:HiddenField ID="expiration" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="Adddays" runat="server" />
    <asp:HiddenField ID="Tabla_tipoEntr" runat="server" />
    <div class="container">
        <div class="row rowsteps">
            <div class="col-lg-4">
                <br />
            </div>
            <div class="col-lg-4 col-xs-12">
                <div class="steps">
                    <ul>
                        <li><a id="InfPed" href="#"><div class="step done" data-desc="Información del Pedido">1</div></a></li>
                        <li><a id="InfDet" href="#"><div class="step active" data-desc="Detalle">2</div></a></li>  
                        <li><a id="InfDesp" href="#" data-toggle="modal"  data-target="#ModalIngPed2"><div id="divInfodes" class="step" data-desc="Información de entrega">3</div></a></li>          
                    </ul>
                </div>
            </div>
            <div id="divEnviar" class="col-lg-4 col-xs-12 text-center btnenviar invisible">
                <asp:Button ID="EnviarPedido" runat="server" Text="Enviar Pedido" CssClass="btn btn-lg btn-primary" />
            </div>
             
        </div>
        <div class="row">
            <div class="panel panelcuerpoinfped">
          <div class="panel-heading panelcabecerainfped">Información del pedido </div>
              <div class="panel-body">
                  
                       <div class="row">
                        <div class="col-lg-12 col-xs-12">
                            <strong><asp:Label ID="Label1" CssClass="control-label" runat="server" Text="Nombre: "></asp:Label></strong>
                            <asp:Label ID="lblNombrePedido" runat="server" Text=""></asp:Label>
                        </div>
                         <div class="col-lg-12 observacion">
                             <strong><asp:Label ID="Label2" CssClass="control-label" runat="server" Text="Observación: "></asp:Label></strong>
                             <asp:Label ID="lblObservacion" runat="server" Text=""></asp:Label>
                        </div>
                         <div class="col-lg-3 col-xs-6">
                                <asp:Label ID="LblKilos"  runat="server"></asp:Label>
                            </div>
                            <div class="col-lg-3 col-lg-1-xs-6">
                                <asp:Label ID="LblM2"  runat="server"></asp:Label>
                            </div>
                     </div>
                  <br />
                        <div class="row">
                            <div class="col-lg-12 col-xs-12 neto text-center">
                                <h3><asp:Label ID="NetodelPedido" Font-Bold="true" runat="server"></asp:Label></h3>
                            </div>
                           
                         
                       </div>
                 
                   
                  
              </div>
        </div>
            

        </div>
        <div id="rowinfodesp" class="row invisible">
            <div class="panel panelcuerpo">
                <div class="panel-heading panelcabecera">Información de entrega</div>
                <div class="panel-body">
                    
                         <div class="row">
                        <div class="col-lg-12 col-xs-12">
                            <p><asp:Label ID="AlertInfoEntrega" runat="server"></asp:Label><label id="countdown"></label></p>
                            
                        </div>
                        <div class="col-lg-12 col-xs-12">
                            <p><asp:Label ID="TipoEntrega" runat="server" Font-Bold="true"></asp:Label></p>
                            
                        </div>
                        <div class="col-lg-12 col-xs-12">
                            <p><asp:Label ID="Fechaentrega" runat="server" Font-Bold="true"></asp:Label></p>
                            
                        </div>
                        <div class="col-lg-12 col-xs-12">
                            <p><asp:Label ID="DirEntrega" runat="server" Font-Bold="true"></asp:Label></p>
                            
                        </div>
                    </div>
                    
                   
                </div>
            </div>
        </div>
        <div class="row">
            <div class="panel panel-primary">
                <div class="panel-heading">Detalle</div>
                <div class="panel-body">
                    <div class ="table table-responsive">
                        <asp:GridView ID="GridDetallePedido" CssClass="table table-hover table-bordered" runat="server"></asp:GridView>
                    </div>
                </div>
            </div>
           
        </div>
    </div>

    <!--MODAL INFO DE ENTREGA -->  
    <div class="modal fade"  id="ModalIngPed2" tabindex="-1" role="dialog" aria-labelledby="ModalIngPed2Label">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="ModalIngPed2Label">Información de entrega del pedido<asp:Label runat="server" ID="TitleModalIngPed2"></asp:Label></h4>
                    
          </div>
          <div class="modal-body">
                  <div class="row">
                                <div class="col-lg-4 col-xs-12">
                                   
                                    
                                </div>
                                
                                
                            </div>
                           
                            <br />
                            <!--Seleccion de fechas y codigo express-->
                            <div class="row">
                                
                                

                                <!--Codigo express-->
                                <div class="col-lg-12 col-xs-12">
                                    <div class="grupocodexpress">
                                        <asp:UpdatePanel ID="UpdatePanelExpress" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="panelExpress" Visible="false" runat="server">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-xs-12">
                                                            <asp:Label ID="LblExpress" ForeColor="GrayText" runat="server"></asp:Label>
                                                        </div>    
                                                        
                                                    </div>
                                                    <!--Solicitar codigo express-->
                                                   <div class="row">
                                                       <div class="col-lg-12">
                                                           <asp:Panel ID="PanelSinCod" runat="server">
                                                                <asp:Label ID="Notienecod" runat="server" ForeColor="Blue" Font-Bold="true" Text="¿No tiene código Express?"></asp:Label>
                                                               <div class="form-inline">
                                                                   <asp:UpdatePanel ID="UpdatePanelSolCodExp" runat="server" UpdateMode="Conditional">
                                                                       <ContentTemplate>
                                                                           <asp:HiddenField ID="id_temp" runat="server" />
                                                                           <asp:Button ID="BtnSolicitudCodExp" runat="server" OnClick="BtnSolicitudCodExp_Click" CssClass="btn btn-primary" Text="Solicitar" />
                                                                   <br /><asp:Label ID="ValidSolCodExp" runat="server"></asp:Label><br /><asp:Label ID="ValidSolCodExp2" runat="server"></asp:Label>
                                                                       </ContentTemplate>
                                                                       <Triggers>
                                                                           <asp:AsyncPostBackTrigger ControlID="BtnSolicitudCodExp" EventName="Click" />
                                                                       </Triggers>
                                                                   </asp:UpdatePanel>
                                                                   
                                                               </div>
                                                                
                                                           </asp:Panel>
                                                           
                                                       </div>
                                                   </div>                                                
                                                </asp:Panel>
                                                
                                            </ContentTemplate>
                                            <Triggers>
                                               
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                        
                                    
                                    </div>
                            </div>
              <div class="row">
                  <div class="col-lg-6 col-xs-12">
                      <asp:Panel ID="MOPanelFechas" CssClass="contenedortablafechas" runat="server">
                  </asp:Panel>
                  </div>
                  <div class="col-lg-6 col-xs-12 invisible" id="divdirecciones">
                      <!--Ingreso de direccion de despacho-->
                                <div >
                                         <asp:UpdatePanel ID="UpdatePanelDirecciones" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                               
                                                    <div class="row">
                                                        <div class="col-lg-12 col-xs-12">
                                                            <h4>Dirección de despacho</h4>
                                                        </div>
                                                        <div class="col-lg-9 col-xs-12">
                                                            <asp:TextBox ID="TextDireccion" onkeypress="showadddir()" placeholder="Ingresar Dirección de despacho"   CssClass="form-control" runat="server"></asp:TextBox>
                                                            
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="modalingped2" runat="server" ErrorMessage="Ingrese una dirección" ForeColor="Red" ControlToValidate="TextDireccion"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-lg-3 col-xs-12">
                                                            <asp:LinkButton ID="Adddir" runat="server" title="Agregar a mis direcciones guardadas" CssClass="adddirnotvisible" >Guardar en mis direcciones</asp:LinkButton>
                                                        </div>
                                                    
                                                        <div class="col-lg-12">
                                                            <asp:LinkButton ID="BtnMisdirecciones" OnClientClick="hideadddir()" runat="server" title="Seleccionar de mis direcciones guardadas" OnClick="BtnMisdirecciones_Click">Mis direcciones</asp:LinkButton>
                                                            <asp:DropDownList ID="DropDirecciones" AutoPostBack="true" OnSelectedIndexChanged="DropDirecciones_SelectedIndexChanged" Visible="false" CssClass="form-control" runat="server">

                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                               
                                            </ContentTemplate>
                                            <Triggers>
                                                
                                                <asp:AsyncPostBackTrigger ControlID="BtnMisdirecciones" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="DropDirecciones" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                     </div>
                  </div>
                  
              </div>
              <div class="row text-center">
                 <h4><asp:Label runat="server"  ID="Lblentrega"></asp:Label></h4> 
              </div>
                 
          </div>      
              <!--footer (botones)-->
          <div class="modal-footer">
               
                      <asp:Button CssClass="btn btn-success" ID="BtnGuardarInfoEntrega" ValidationGroup="modalingped2" CausesValidation="true" runat="server" Text="Aceptar" OnClick="BtnGuardarInfoEntrega_Click" />
                      <asp:Label ID="labelprueba" runat="server"></asp:Label>
          </div>
          
       </div>
     </div>
    </div>
    <script type="text/javascript">
        $('.recomendado').popover();
        $(document).ready(function () {
            Estado();
        });
        function Estado() {
            var valor = $('#<%= (HiddenStatus.ClientID)%>').val();

            if (valor == 1) {

                $('#divInfodes').removeClass().addClass('step entr');
                $('#rowinfodesp').removeClass('invisible').addClass('visible');
                $('#divEnviar').removeClass('invisible').addClass('visible');
            }
        }
        function showadddir() {
                $('#<%= (Adddir.ClientID)%>').removeClass('adddirnotvisible').addClass('adddirvisible');
        }
        function hideadddir() {
            $('#<%= (Adddir.ClientID)%>').removeClass('adddirvisible').addClass('adddirnotvisible');
        }
        function Seleccionfecha(adddays, tipo, idcontrol, fecha) {

            $.each($('.cuadros'), function () {
                $(this).removeClass().addClass('seleccionhover cuadros');
            });
            
            $('#' + idcontrol).removeClass().addClass('seleccionhover seleccionado cuadros');
            if (tipo == 'retiro') {
                $('#<%=Lblentrega.ClientID%>').html('El pedido puede ser retirado a partir del ' + fecha);
                $('#divdirecciones').removeClass().addClass('col-lg-6 col-xs-12 invisible');
            }
            else {
                $('#<%=Lblentrega.ClientID%>').html('El pedido será despachado el ' + fecha);
                $('#divdirecciones').removeClass().addClass('col-lg-6 col-xs-12 visible');
            }
            

        }

        
           

    </script>
    
   
</asp:Content>
