<%@ Page Title="Termopanel" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.master" AutoEventWireup="true" CodeFile="Termopanel.aspx.cs" Inherits="_Ingresar_Pedido_Termo" %>
<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
         .selected{
             background-color:rgba(0, 57, 245, 0.80);
             
         }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
   <div id="contenedor" class="container-fluid bg-fondo-hero-termopanel" >
       <br /><br /><br /><br /><br /><br />
      <div class="container-fluid">
          
          <!--titulo-->
          <div class="d-flex justify-content-center">
              <h1 class="text-light">Ingreso Pedidos Termopanel</h1>
          </div>
          <div class="d-flex justify-content-center mb-5">
              <h3 class="text-light">"<asp:Label runat="server" ID="LblEmpresa"></asp:Label>"</h3>
          </div>
          <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item" runat="server" id="BreadPedido"></li>
                    <li class="breadcrumb-item active">Pedido termopaneles</li>
                </ol>
            </nav>
          <!--Layout-->
          <div class="row mb-3">
              
              <div class="col-12 col-lg-9 col-md-12">
                        <!--OrderName-->
                    <div class="row mb-2">
                        <div class="col">
                            <!--card order name-->
                             <div class="card mb-1 bg-opacity-white-65">
                                <div class="card-body">
                                    
                                        
                                        <asp:UpdatePanel ID="UpdPnlName" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                <div class="row">
                                                    <div class="col-12 col-lg-12">
                                                        <label class="font-weight-bold">Nombre del Pedido:</label>&nbsp;&nbsp;
                                                    </div>
                                                    <div class="col-12 col-lg-10 col-md-8">
                                                        <asp:TextBox CssClass="form-control" ID="TxtOrderName" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-12 col-lg-2 col-md-2">
                                                        <asp:LinkButton runat="server" ID="LinkBtnOrderName" OnClick="LinkBtnOrderName_Click" CssClass=" BotonLink" Text="Editar"></asp:LinkButton>
                                                    </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="LinkBtnOrderName" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                        <!--OrderObs-->
                    <div class="row mb-2">
                        <div class="col">
                            <!--card order Obs-->
                             <div class="card mb-1 bg-opacity-white-65">
                                <div class="card-body">
                                    <asp:UpdatePanel runat="server" ID="UpDPnlObs" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                        <div class="col-12 col-lg-12">
                                                <label class="font-weight-bold">Observaciones del Pedido:</label>&nbsp;&nbsp;
                                        </div>
                                        <div class="col-12 col-lg-10">
                                            <asp:TextBox CssClass="form-control" ID="TxtOrderObs" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-12 col-lg-2">
                                            <asp:LinkButton runat="server" ID="LinkBtnOrderObs" OnClick="LinkBtnOrderObs_Click" CssClass=" BotonLink" Text="Editar"></asp:LinkButton>
                                        </div>
                             
                             
                                    </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="LinkBtnOrderObs" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    
                                </div>
                            </div>
                        </div>
                    </div>

                        <!--bTN ADD ITEMS-->
                    <div class="row mb-3">
                        <div class="col-12 col-lg-6 col-md-6 text-center mb-2">
                            <button type="button" class="btn btn-success" data-toggle="modal" data-target="#modalAddItem">Agregar Item</button>
                        </div>
                            <div class="col-12 col-lg-6 col-md-6 text-center mb-2">
                            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalCargaMasiva">Carga masiva desde Excel</button>
                        </div>
                    </div>

                    <!--items-->
                  <div class="row mb-3">
                      <div class="col">
                          <asp:Panel runat="server" ID="PnlItems">
                              <div class="card mb-1 bg-opacity-white-65">
                                  <div class="card-body">
                                      <div id="DivListItems" class="table-responsive" runat="server">

                                      </div>
                                  </div>
                              </div>
                          </asp:Panel>
                      </div>

                  </div>
              </div>
              <!--sidebar-->
              <div class="col-12 col-lg-3 col-md-12">
                  <asp:Panel runat="server" ID="PnlDelivery" Visible="false">
                      <div class="row mb-2">
                      <div class="col">
                           <!--card order Delivery-->
                            <div class="card mb-1 bg-opacity-white-65">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <h4>Información de entrega</h4>
                                        </div>
                                    </div>
                                    
                                    <asp:Panel runat="server" ID="PnlDeliveryInfo" Visible="false">
                                        <div class="row">
                                            <div class="col-12">
                                                <div id="DivDeliveryInfo" runat="server"></div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="row ml-2">
                                        
                                        <asp:LinkButton runat="server" ID="BtnAddDeliver" CssClass="BotonLink" OnClick="BtnAddDeliver_Click"></asp:LinkButton>
                                    </div>
                                    
                                    
                                </div>
                            </div>

                      </div>
                        </div>

                  </asp:Panel>
                  <asp:Panel ID="PnlTotalOrder" runat="server">
                      <div class="row mb-2">
                      <div class="col">
                          <!--card order Total-->
                            <div class="card mb-1 bg-opacity-white-65">
                                <div class="card-body">
                                    <div class="row mb-2">
                                        <div class="col">
                                            <h4>Resumen del pedido</h4>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div id="DivSummary" class="table-responsive" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col text-right">
                                            <!--poner boton link to terminos y condiciones-->
                                            <button type="button" runat="server" id="BtnToTermandCond" class="btn btn-lila" data-toggle="modal" data-target="#modalTerminosycond">Enviar Pedido</button>

                                        </div>
                                    </div>
                                    
                                </div>
                            </div>
                      </div>
                        </div>
                  </asp:Panel>
                  

              </div>
          </div>
          
          

          

      </div>
       
       
   </div>

    
    
    <!--Modal Additem-->
    <div class="modal fade"  id="modalAddItem" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content bg-opacity-white-25">
          <div class="modal-header text-white">
            
            <h3>Agregar Item</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body text-white">
              <div class="row mb-5">
                  <div class="col-12"><label class="font-weight-bold">Referencia</label></div>
                  <div class="col-lg-6 col-12">
                            <asp:TextBox runat="server" ID="TxtRefAddItem" placeholder="max 25 carácteres" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="TxtRefAddItem" ValidationGroup="GroupAddItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                      <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ValidationExpression="^[\s\S]{0,25}$" ControlToValidate="TxtRefAddItem" ValidationGroup="GroupAddItem" ErrorMessage="Supera el máximo de carácteres" SetFocusOnError="true" ></asp:RegularExpressionValidator>
                      </div>
              </div>
              <div class="row mb-4">
                  <div class="col-12 col-lg-4 mb-3">
                      <div class="container">
                          <div class="row justify-content-center"><label class="font-weight-bold">Cristal Exterior</label></div>
                          <div class="row justify-content-center"><asp:DropDownList runat="server" ID="DDLAddItemCREX" CssClass="form-control"></asp:DropDownList>
                              <asp:RequiredFieldValidator InitialValue="--Seleccionar--"
                                    ValidationGroup="GroupAddItem" runat="server" ControlToValidate="DDLAddItemCREX"  ErrorMessage="Debe seleccionar un item"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      
                  </div>
                  <div class="col-12 col-lg-4 mb-3">
                      <div class="container">
                          <div class="row justify-content-center"><label class="font-weight-bold">Separador</label></div>
                          <div class="row justify-content-center"><asp:DropDownList runat="server" ID="DDLAddItemSEP" CssClass="form-control"></asp:DropDownList>
                              <asp:RequiredFieldValidator InitialValue="--Seleccionar--"
                                    ValidationGroup="GroupAddItem" runat="server" ControlToValidate="DDLAddItemSEP"  ErrorMessage="Debe seleccionar un item"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      
                  </div>
                  <div class="col-12 col-lg-4">
                      <div class="container">
                          <div class="row justify-content-center"><label class="font-weight-bold">Cristal Interior</label></div>
                          <div class="row justify-content-center">
                              <asp:DropDownList runat="server" ID="DDLAddItemCRIN" CssClass="form-control"></asp:DropDownList>
                              <asp:RequiredFieldValidator InitialValue="--Seleccionar--"
                                    ValidationGroup="GroupAddItem" runat="server" ControlToValidate="DDLAddItemCRIN"  ErrorMessage="Debe seleccionar un item"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      
                  </div>
              </div>
              <div class="row mb-5 ml-2">
                  <div class="col-12">
                      <div class="form-inline">
                          <label class="font-weight-bold">Cantidad</label>&nbsp;&nbsp;
                          <asp:TextBox runat="server" ID="TxtAddItemCant" CssClass="form-control"></asp:TextBox>
                      </div>
                      <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="TxtAddItemCant"  ValidationGroup="GroupAddItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ValidationExpression="\b([1-9]|[1-8][0-9]|9[0-9]|100)\b" ControlToValidate="TxtAddItemCant" ValidationGroup="GroupAddItem" ErrorMessage="solo números enteros entre 1 y 100" SetFocusOnError="true" ></asp:RegularExpressionValidator>
                      
                  </div>
                  
              </div>
              <div class="row mb-2 ml-2">
                  <div class="col-12">
                      <div class="form-inline">
                          <label class="font-weight-bold">Ancho</label>&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:TextBox runat="server" ID="TxtAddItemAncho" placeholder="milímetros" CssClass="form-control"></asp:TextBox>
                      </div>
                      
                  </div>
                  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="TxtAddItemAncho"  ValidationGroup="GroupAddItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ValidationExpression="^(1[5-8][0-9]|19[0-9]|[2-9][0-9]{2}|1[0-9]{3}|2[0-3][0-9]{2}|24[0-8][0-9]|2490)$" ControlToValidate="TxtAddItemAncho" ValidationGroup="GroupAddItem" ErrorMessage="solo números enteros entre 300 y 2500" SetFocusOnError="true" ></asp:RegularExpressionValidator>
              </div>
              <div class="row ml-2">
                  <div class="col-12">
                      <div class="form-inline">
                          <label class="font-weight-bold">Alto</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:TextBox runat="server" ID="TxtAddItemAlto" placeholder="milímetros" CssClass="form-control"></asp:TextBox>
                      </div>

                      
                  </div>
                  <asp:RequiredFieldValidator runat="server" ID="ReqFieldAlto1" ControlToValidate="TxtAddItemAlto"  ValidationGroup="GroupAddItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                  <asp:RegularExpressionValidator runat="server" ID="ReqFieldAlto" ValidationExpression="^^(1[5-8][0-9]|19[0-9]|[2-9][0-9]{2}|[12][0-9]{3}|3[0-4][0-9]{2}|35[0-8][0-9]|3590)$" ControlToValidate="TxtAddItemAlto" ValidationGroup="GroupAddItem" ErrorMessage="solo números enteros entre 300 y 2500" SetFocusOnError="true" ></asp:RegularExpressionValidator>
                     
              </div>
             

          </div>
          <div class="modal-footer">
              <asp:Button runat="server" ID="BtnAddItem" ValidationGroup="GroupAddItem" OnClick="BtnAddItem_Click" OnClientClick="Loading();" CssClass="btn btn-success" Text="Agregar" />
          </div>
          
       </div>
  </div>
</div>

    <asp:HiddenField runat="server" ID="HdnIdItemSelected" />
    <!--Modal Delete-->
    <div class="modal fade"  id="modalDeleteItem" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content bg-opacity-white-25">
          <div class="modal-header text-white">
            
            <h3> <label id="DeleteItemTitle"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col-6 text-center">
                      
                      <asp:Button runat="server" ID="BtnDeleteItem" OnClick="BtnDeleteItem_Click" OnClientClick="Loading();" CssClass="btn btn-info" Text="Eliminar" />
                  </div>
                  <div class="col-6 text-center">
                      <button type="button" class="btn btn-danger" data-dismiss="modal" aria-label="Close">No</button>
                  </div>
              </div>
              
          </div>
            
       </div>
  </div>
</div>

    <!--Modal EditItem-->
    <div class="modal fade"  id="modalEditItem" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content bg-opacity-white-25">
          <div class="modal-header text-white">
            
            <h3> <label id="EditItemTitle"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <asp:UpdatePanel ID="UpdateMdlEdit" UpdateMode="Conditional" runat="server">
              <ContentTemplate>
                  <div class=" modal-body text-white">
                      <asp:Panel runat="server" ID="PnlBtnEditItem">
                          <div class="row mb-5">
                              <div class="col-12 text-center">
                                  <asp:Button runat="server" ID="BtnEditItem1" OnClick="BtnEditItem1_Click" CssClass="btn btn-naranjo" Text="Editar" />
                              </div>
                          </div>
                      </asp:Panel>
                      <asp:Panel runat="server" ID="PnlEditForm">
                          <div class="row mb-5">
                  <div class="col-12"><label class="font-weight-bold">Referencia</label></div>
                  <div class="col-lg-6 col-12">
                            <asp:TextBox runat="server" ID="TxtEditRef" placeholder="max 25 carácteres" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="TxtEditRef" ValidationGroup="GroupEditItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                      <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4" ValidationExpression="^[\s\S]{0,25}$" ControlToValidate="TxtEditRef" ValidationGroup="GroupEditItem" ErrorMessage="Supera el máximo de carácteres" SetFocusOnError="true" ></asp:RegularExpressionValidator>
                      </div>
              </div>
              <div class="row mb-1">
                  <div class="col-12 col-lg-4">
                      <div class="container">
                          <asp:HiddenField runat="server" ID="HdnIDSUBDETCREEX" />
                          <asp:HiddenField runat="server" ID="HdnCREXDET" />
                          <div class="row justify-content-center"><label class="font-weight-bold">Cristal Exterior</label></div>
                          <div class="row justify-content-center"><asp:DropDownList runat="server" ID="DDLEditCREX" CssClass="form-control"></asp:DropDownList>
                              <asp:RequiredFieldValidator InitialValue="--Seleccionar--"
                                    ValidationGroup="GroupEditItem" runat="server" ControlToValidate="DDLEditCREX"  ErrorMessage="Debe seleccionar un item"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      
                  </div>
                  <div class="col-12 col-lg-4">
                      <div class="container">
                          <asp:HiddenField runat="server" ID="HdnIDSUBDETSEP" />
                          <asp:HiddenField runat="server" ID="HdnSEPDET" />
                          <div class="row justify-content-center"><label class="font-weight-bold">Separador</label></div>
                          <div class="row justify-content-center"><asp:DropDownList runat="server" ID="DDLEditSEP" CssClass="form-control"></asp:DropDownList>
                              <asp:RequiredFieldValidator InitialValue="--Seleccionar--"
                                    ValidationGroup="GroupEditItem" runat="server" ControlToValidate="DDLEditSEP"  ErrorMessage="Debe seleccionar un item"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      
                  </div>
                  <div class="col-12 col-lg-4">
                      <div class="container">
                          <asp:HiddenField runat="server" ID="HdnIDSUBDETCRIN" />
                          <asp:HiddenField runat="server" ID="HdnCRINDET" />
                          <div class="row justify-content-center"><label class="font-weight-bold">Cristal Interior</label></div>
                          <div class="row justify-content-center">
                              <asp:DropDownList runat="server" ID="DDLEditCRIN" CssClass="form-control"></asp:DropDownList>
                              <asp:RequiredFieldValidator InitialValue="--Seleccionar--"
                                    ValidationGroup="GroupEditItem" runat="server" ControlToValidate="DDLEditCRIN"  ErrorMessage="Debe seleccionar un item"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      
                  </div>
              </div>
            <asp:HiddenField runat="server" ID="HdnAddTerminologia" />
                      <asp:Panel runat="server" ID="PnlAddDicc" Visible="false">
                          <div class="row ml-2">
                            <div class="col-12">
                                <asp:CheckBox runat="server" ID="ChkAddTerminologia" CssClass=" form-check-inline" Font-Bold="true" Text="Agregar al diccionario" />
                            </div>
                        </div>
                      </asp:Panel>

              <div class="row mb-5 mt-4 ml-2">
                  <div class="col-12">
                      <div class="form-inline">
                          <label class="font-weight-bold">Cantidad</label>&nbsp;&nbsp;
                          <asp:TextBox runat="server" ID="TxtEditCant" CssClass="form-control"></asp:TextBox>
                      </div>
                      <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="TxtEditCant"  ValidationGroup="GroupEditItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5" ValidationExpression="\b([1-9]|[1-8][0-9]|9[0-9]|100)\b" ControlToValidate="TxtEditCant" ValidationGroup="GroupEditItem" ErrorMessage="solo números enteros entre 1 y 100" SetFocusOnError="true" ></asp:RegularExpressionValidator>
                      
                  </div>
                  
              </div>
              <div class="row mb-2 ml-2">
                  <div class="col-12">
                      <div class="form-inline">
                          <label class="font-weight-bold">Ancho</label>&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:TextBox runat="server" ID="TxtEditAncho" placeholder="milímetros" CssClass="form-control"></asp:TextBox>
                      </div>
                      
                  </div>
                  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="TxtEditAncho"  ValidationGroup="GroupEditItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator6" ValidationExpression="([3-8][0-9]{2}|9[0-8][0-9]|99[0-9]|1[0-9]{3}|2[0-4][0-9]{2}|2500)\b" ControlToValidate="TxtEditAncho" ValidationGroup="GroupEditItem" ErrorMessage="solo números enteros entre 300 y 2500" SetFocusOnError="true" ></asp:RegularExpressionValidator>
              </div>
              <div class="row ml-2">
                  <div class="col-12">
                      <div class="form-inline">
                          <label class="font-weight-bold">Alto</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:TextBox runat="server" ID="TxtEditAlto" placeholder="milímetros" CssClass="form-control"></asp:TextBox>
                      </div>

                      
                  </div>
                  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="TxtEditAlto"  ValidationGroup="GroupEditItem" ErrorMessage="Campo requerido"></asp:RequiredFieldValidator>
                  <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator7" ValidationExpression="([3-8][0-9]{2}|9[0-8][0-9]|99[0-9]|1[0-9]{3}|2[0-4][0-9]{2}|2500)\b" ControlToValidate="TxtEditAlto" ValidationGroup="GroupEditItem" ErrorMessage="solo números enteros entre 300 y 2500" SetFocusOnError="true" ></asp:RegularExpressionValidator>
                     
              </div>

                           

                      </asp:Panel>
                      
                  </div>
                  <asp:Panel runat="server" ID="PnlEditFormFooter" Visible="false">
                      <div class="modal-footer">
                          <asp:Button runat="server" ID="BtnEditItemGo" OnClick="BtnEditItemGo_Click" ValidationGroup="GroupEditItem" CssClass="btn btn-success" Text="Actualizar" />
                        </div>
                  </asp:Panel>
                  
              </ContentTemplate>
              <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="BtnEditItem1" EventName="Click" />
                  <asp:PostBackTrigger ControlID="BtnEditItemGo" />
              </Triggers>
          </asp:UpdatePanel>
            
       </div>
  </div>
</div>

    <!--Modal CargaMasiva-->
    <div class="modal fade"  id="modalCargaMasiva" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content bg-opacity-white-25">
          <div class="modal-header text-white">
            
            <h3> Cargar Archivo Xls</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col-6 text-center">
                      <div class="row">
                      <div class="col-12">
                            <label for="FileUpXls">Adjuntar Archivo</label> <asp:FileUpload runat="server" ID="FileUpXls" CssClass="form-control-file" />
                           <asp:RegularExpressionValidator ID="RegularExpressionValidator8"
                               runat="server" ControlToValidate="FileUpXls"
                               ErrorMessage="Se permite solo archivos con formato .xls y .xlsx" ForeColor="Red"
                               ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$"
                               ValidationGroup="ValXls" SetFocusOnError="true"></asp:RegularExpressionValidator>
                          
                      </div>
                    
                  </div>
                      
                  </div>
                
              </div>
              
          </div>
            <div class=" modal-footer">
                <asp:Button runat="server" ID="BtnCargarXls" OnClick="BtnCargaMasiva_Click" OnClientClick="Loading();" CssClass="btn btn-success" Text="Cargar" />
            </div>
            
       </div>
  </div>
</div>

    <!--Modal Delivery info-->
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpDPnlmodalDelivery">
        <ContentTemplate>
            
            <div class="modal fade"  id="modalDelivery" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content bg-opacity-white-45 text-white">
          <div class="modal-header">
            
            <h3>Información de entrega</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
              
          </div>
          <div class="modal-body">
              <asp:UpdatePanel runat="server" ID="UpdatePnlDivTableDeliveryModal" UpdateMode="Conditional">
                  <ContentTemplate>
                      <div class="row"><div class=" table-responsive" runat="server" id="DivtableDeliverymodal"></div></div>
                      <div class="row mb-4 justify-content-between">
                          <asp:HiddenField runat="server" ID="HdnDivTableDeliveryPage" />
                  <div class="col-4">
                      <asp:LinkButton runat="server" ID="BtnDivTableDeliveryAnt" OnClick="BtnDivTableDeliveryAnt_Click" CssClass="btn btn-outline-light">Anterior</asp:LinkButton>
                  </div>
                  <div class="col-4">
                      <asp:LinkButton runat="server" ID="BtnDivTableDeliverySig" OnClick="BtnDivTableDeliverySig_Click" CssClass="btn btn-outline-light">Siguiente</asp:LinkButton>
                  </div>
              </div>
                  </ContentTemplate>
                  <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="BtnDivTableDeliverySig" EventName="Click" />
                      <asp:AsyncPostBackTrigger ControlID="BtnDivTableDeliveryAnt" EventName="Click" />
                      <asp:AsyncPostBackTrigger ControlID="DDLMdlDeliveryCom" EventName="SelectedIndexChanged" />

                  </Triggers>
              </asp:UpdatePanel>
              
              
              <div  runat="server" id="RowmodalDeliveryAddress">

                  <asp:UpdatePanel runat="server" ID="UpdatePnlModalDeliveryAddress" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="row ml-2">
                              <h6>Debe seleccionar la dirección de despacho</h6>
                          </div>
                          <div class="row ml-2 mr-1">
                              <div class="col-12 mb-2"><!--Region-->
                              <div class="form-inline">
                                  <label for="">Región</label>&nbsp;&nbsp;&nbsp;
                                  <asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLMdlDeliveryReg_SelectedIndexChanged" CssClass="form-control" ID="DDLMdlDeliveryReg"></asp:DropDownList>
                              </div>
                          </div>
                          <div class="col-12 mb-2"><!--Comuna-->
                              <asp:Panel ID="PnlComuna" runat="server" Visible="false">
                                  <div class="form-inline">
                                  <label for="DDLMdlDeliveryCom">Comuna</label>&nbsp;&nbsp;&nbsp;
                                  <asp:DropDownList runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLMdlDeliveryCom_SelectedIndexChanged" CssClass="form-control" ID="DDLMdlDeliveryCom"></asp:DropDownList>
                              </div>
                              </asp:Panel>
                              
                          </div>
                          
                          
                              
                          
                          </div>
                          <!--Direccion-->
                          <div class="row ml-2 mr-1" id="RowDireccion">
                              <div class="col-12 mb-2 mt-2">
                                  <label for="">Dirección</label>&nbsp;&nbsp;&nbsp;
                                  <asp:TextBox runat="server" ID="TxtMdlDeliveryDir" CssClass=" form-control"></asp:TextBox>
                          </div>
                              <div class="col-12">
                                  <asp:CheckBox runat="server" ID="ChkAddmisDir" CssClass="form-check" Text="Agregar a mis direcciones" />
                              </div>

                              <div class="d-flex justify-content-end">
                              <asp:LinkButton runat="server" OnClick="LinkBtnMisdirecciones_Click" ID="LinkBtnMisdirecciones" CssClass="btn btn-link text-white" Text="Seleccionar de mis direcciones"></asp:LinkButton>
                          </div>
                          
                          </div>
                          <asp:Panel runat="server" ID="PnlTableMisdir" Visible="false">
                              <div class="row ml-2 mr-1">
                              <div class="table-responsive" runat="server" id="DivTableMisDir"></div>
                                </div>
                          </asp:Panel>
                          
                          



                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLMdlDeliveryReg" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLMdlDeliveryCom" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="LinkBtnMisdirecciones" EventName="Click" />
                      </Triggers>
                  </asp:UpdatePanel>




                  
              </div>

              <asp:HiddenField runat="server" ID="HdnDateSelected" />
              <asp:HiddenField runat="server" ID="HdnTypeDeliverySelected" />
              
          </div>
            <div class=" modal-footer align-content-between">
                <label id="LblresumenDeliveryMth"></label>
                <asp:Button ID="BtnAddDeliveryMethod" OnClick="BtnAddDeliveryMethod_Click" runat="server" CssClass="btn btn-outline-success" Text="Aceptar" />
                
                
            </div>
            
       </div>
  </div>
</div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnAddDeliver" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    
    <!--Modal Terms&Conditions-->
    <div class="modal fade"  id="modalTerminosycond" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content bg-opacity-white-25">
          <div class="modal-header text-white">
            
            <h3> <label>Enviar Pedido</label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body text-white">
              <div class="row">
                  <div class="col-12 text-center">
                      <h5>Al enviar el pedido declaras haber leído y aceptas nuestros  
                          <asp:LinkButton runat="server" CssClass=" TermsAndCondition HighLight" ID="LinkToTermsandCond" Text="Términos y Condiciones."></asp:LinkButton>

                      </h5>
                  </div>
                  
              </div>
              
          </div>
            <div class=" modal-footer">
                <asp:Button runat="server" ID="BtnSendOrder" OnClick="BtnSendOrder_Click" Text="Enviar" CssClass="btn btn-lila" />
            </div>
            
       </div>
  </div>
</div>


    <asp:UpdateProgress ID="Updateprogress"  runat="server">
        <ProgressTemplate>
            <div class="LoadingContainer">
              <div class="overlay">
                           <div class="modalprogress">
                                   <div class="spinner">
                                        <div class="bounce1"></div>
                                        <div class="bounce2"></div>
                                        <div class="bounce3"></div>
                                    </div>
                               <h3 class="text-center" id="LoadingMsj">Espere un momento...</h3>
                            
                           </div>
                            
                        </div>
          </div>
          
        </ProgressTemplate>
    </asp:UpdateProgress>
   
    <script type="text/javascript">
        $(document).ready(function () {
            PageEvents();
            
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            PageEvents();
            console.log('add_end_rq');
        });

        /**
        function Loading() {

            
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Espere un momento...');
            $('#modalLoading').modal('show').css('pointer-events','none');
        }

        function Unloading() {
            $('#modalLoading').modal('toggle');
        }
         */

        function OpenDeliveryModal() {
            console.log('OpenDeliveryModal');
            $('#modalDelivery').modal('show');
            $('#contenedor').removeClass().addClass('container-fluid bg-fondo-hero-termopanel blur');
        }

        function PageEvents() {
            
            
            $('#<%=DDLMdlDeliveryCom.ClientID%>').change(function () {
                var sel = $(this).val();
                console.log(sel);
                if (sel=='--Seleccionar--') {
                    $('#RowDireccion').removeClass('visible d-block').addClass('invisible d-none');
                }
                else {
                    $('#RowDireccion').removeClass('invisible d-none').addClass('visible d-block');
                }
                $('#<%=TxtMdlDeliveryDir.ClientID%>').select();
                $('#<%=TxtMdlDeliveryDir.ClientID%>').focus();
            });

            $('.selDir').hover(function () {
                $(this).css('cursor', 'pointer');
                $(this).addClass('selected');

            });
            $('.selDir').mouseleave(function () { $(this).removeClass('selected'); });
            $('.selDir').click(function () {

                $('#<%=TxtMdlDeliveryDir.ClientID%>').val($(this).find("td").eq(1).html());
                
                $('#<%=DDLMdlDeliveryReg.ClientID%>').val($(this).find("td").eq(3).html());
                $('#<%=DDLMdlDeliveryCom.ClientID%>').val($(this).find("td").eq(2).html());
                $('#RowDireccion').removeClass('invisible d-none').addClass('visible d-block');

            });

            $('.modal').on('show.bs.modal', function (e) {
                $('#contenedor').removeClass().addClass('container-fluid bg-fondo-hero-termopanel blur');
            });
            $('.modal').on('hide.bs.modal', function (e) {
                $('#contenedor').removeClass().addClass('container-fluid bg-fondo-hero-termopanel');
            });

            $('.eliminar').hover(function () {
                $(this).css('cursor', 'pointer');
            });
            $('.filas').hover(function () {
                $(this).css('cursor', 'pointer');
                $(this).css('background','rgba(1, 70, 180, 0.10)');
            });

            $('.retiro').hover(function () { $(this).css('cursor', 'pointer');$(this).css('text-decoration', 'underline');$(this).css('font-weight', 'bold');  });
            $('.despacho').hover(function () { $(this).css('cursor', 'pointer'); $(this).css('text-decoration', 'underline'); $(this).css('font-weight', 'bold'); });

            $('.retiro').mouseleave(function () { $(this).css('text-decoration', '');$(this).css('font-weight', 'normal');  });
            $('.despacho').mouseleave(function () { $(this).css('text-decoration', ''); $(this).css('font-weight', 'normal'); });

            $('.retiro').click(function () {
                $('.retiro').removeClass('selected').addClass();
                $('.despacho').removeClass('selected').addClass();
                $(this).addClass('selected');
                $('.despacho').removeAttr('title');
                $('.retiro').removeAttr('title');
                $(this).attr('title','Seleccionado');
                $('#RowDireccion').removeClass('visible d-block').addClass('invisible d-none');
                $('#<%= RowmodalDeliveryAddress.ClientID%>').removeClass('visible d-block').addClass('invisible d-none');
                var tr = $(this).closest("tr");
                var fecha = tr.find("td").eq(0).html();
                $('#LblresumenDeliveryMth').html("Retiro para el día " + fecha + ". Costo: " + tr.find("td").eq(1).html());
                $('#<%=HdnDateSelected.ClientID%>').val(tr.find("td").eq(3).html());
                $('#<%=HdnTypeDeliverySelected.ClientID%>').val('retiro');
                
            });
            $('.despacho').click(function () {
                $('.retiro').removeClass('selected').addClass();
                $('.despacho').removeClass('selected').addClass();
                $(this).addClass('selected');
                $('.despacho').removeAttr('title');
                $('.retiro').removeAttr('title');
                $(this).attr('title','Seleccionado');
                
                $('#<%= RowmodalDeliveryAddress.ClientID%>').removeClass('invisible').addClass('visible d-block');
                var tr = $(this).closest("tr");
                var fecha = tr.find("td").eq(0).html();
                $('#LblresumenDeliveryMth').html("Despacho para el día " + fecha + ". Costo: " + tr.find("td").eq(2).html());
                $('#<%=HdnDateSelected.ClientID%>').val(tr.find("td").eq(3).html());
                $('#<%=HdnTypeDeliverySelected.ClientID%>').val('despacho');
                
            });

            $('.filas').mouseleave(function () {
                
                $(this).css('background','');
            });
            
            $('.eliminar').click(function () {
                 $('#modalDeleteItem').modal('show');
            });
            $('.filas').click(function () {
                
                $('#DeleteItemTitle').html("Seguro que desea eliminar el ítem " + $(this).find("td").eq(0).html() + "?");
                $('#EditItemTitle').html("Editar " + $(this).find("td").eq(0).html() + " - " + $(this).find("td").eq(1).html());
                $('#<%= HdnIdItemSelected.ClientID%>').val($(this).find("td").eq(8).html());
                $('#<%= HdnAddTerminologia.ClientID%>').val($(this).find("td").eq(9).html());

                
                
            });
           
            $('.editar').click(function () {
                $('#modalEditItem').modal('show');
                $('#<%=PnlBtnEditItem.ClientID%>').show();
                $('#<%=PnlBtnEditItem.ClientID%>').css('display','block');
                $('#<%=PnlEditForm.ClientID%>').hide();
                $('#<%=PnlEditFormFooter.ClientID%>').hide();
                $('#<%=PnlAddDicc.ClientID%>').hide();
            });



        }

        

        
    </script>
    
   
</asp:Content>
