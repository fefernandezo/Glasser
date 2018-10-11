<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="EdicionComponente.aspx.cs" Inherits="Com_Adm_Default_" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        .zoom-image-container {
  
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  
}

.zoom-image {
  
  position: relative;
  
}

.zoom-image img {
  
  cursor: none;
  
  border-radius: 5px;
  box-shadow: 0 18px 5px -15px rgba( 0, 0, 0, .5 );
  
}

.hover-image {
  
  position: fixed;
  width: 300px;
  height: 300px;
  
  
  border-radius: 50%;
  
  transform: translate( 10%, -20% );
  
  pointer-events: none;
  
  box-shadow: 0 0 10px rgba( 0, 0, 0, .5 );
  
}
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="container">
        <div class="row">
            <div class="col text-center">
                <h1>Edición de Componente</h1><asp:HiddenField runat="server" ID="HdnIdComp" />
            </div>
        </div>
        <div class="row justify-content-center mb-3">
            <div class="zoom-image" >
                
            <asp:Image runat="server" ID="Imagen" CssClass="img-fluid" ImageUrl="http://cproyecto.phglass.cl/Images/COT_COMPONENTES/174632producto%201.jpg" />
  </div>
        </div>
        <div class="row">
            
            <div class="col-12 col-lg-6">
                
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UPDatenombre">
                        <ContentTemplate>
                            <div class="form-group">
                               <label for="TxtNombre">Nombre:</label>  <asp:TextBox runat="server" ID="TxtNombre" Enabled="false" CssClass="form-control" Text="Producto1"></asp:TextBox><asp:LinkButton runat="server" ID="LinkBtnNombre" OnClick="LinkBtnNombre_Click" Text="Editar"></asp:LinkButton>
                            </div>
                            
                        </ContentTemplate>
                        <Triggers>
                            
                            <asp:AsyncPostBackTrigger ControlID="LinkBtnNombre" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    
                
            </div>
            <div class="col-12 col-lg-6 text-center">
                <div class="row">
                    <div class="col">
                        <strong>Creado</strong> el <asp:Label runat="server" ID="LblCreateDate"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <strong>Editado</strong> el <asp:Label runat="server" ID="LblEditDate"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-12">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UPDdescripcion">
                    <ContentTemplate>
                        <div class="form-group">
                            <label for="">Descripción:</label><asp:TextBox runat="server" CssClass="form-control" Enabled="false" ID="TxtDescripcion" TextMode="MultiLine"></asp:TextBox><asp:LinkButton runat="server" ID="LinkBtnDescripcion" OnClick="LinkBtnDescripcion_Click" CssClass=" btn-link" Text="Editar"></asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnDescripcion" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </div>
        </div>
        <!--CARD VINCULACIón CON ALFAK-->
        <div class="card mb-3 text-white card-bg-naranjo">
            <div class="card-body">
                 <div class="row mb-3">
            <div class="col-12 ml-3">
                <div class="form-inline">
                    <asp:Label runat="server" ID="LblEditAsocAlfak"></asp:Label> &nbsp;
                    <div id="LinkTomdlAsocAlfak" class="btn btn-outline-light"  runat="server"></div>

                </div>
            </div>
        </div>
                <div class="row mb-3">
                    <div class="col-12 ml-2">
                        <asp:Label runat="server" ID="lblFamilyAlfak"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
       


        
        <!--CARD PRECIO-->
        <div class="card mb-3 text-white bg-info">
            <div class="card-body">
                <asp:UpdatePanel runat="server" ID="UpDatePrecio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-2">
                            <div class="col-lg-6 col-12">
                                    <div class="form-inline">
                                        <h5>Precio y Unidad de medición</h5><span>&nbsp; &nbsp;</span>
                                        <button type="button" runat="server" id="BtnEditPrecio" data-toggle="modal" data-target="#modalEditPrecio" class="btn btn-outline-light">Editar</button>
                                    </div>
                                    
                            </div>
                            
                        </div>
                        <div class="row mb-2">
                            <div class="col-12 ml-3">
                                <asp:Label runat="server" ID="LblPrecioydim"></asp:Label>
                            </div>
                            <div class="col-12 ml-3">
                                <asp:Label runat="server" ID="LblCantEmb"></asp:Label>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <!--CARD PROCESOS-->
        <div class="card mb-3 text-white fondo-lila">
            <div class="card-body">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-2">
                            <div class="col-lg-6 col-12">
                                    <div class="form-inline">
                                        <h5>Procesos Asociados</h5><span>&nbsp; &nbsp;</span>
                                        <button type="button" runat="server" id="BtnOpenModalProc" data-toggle="modal" data-target="#modalEditProceso" class="btn btn-outline-light">Agregar</button>
                                    </div>
                                    
                            </div>
                            
                        </div>
                        <div class="row mb-2">
                            <div class="col-12 ml-3">
                                <asp:Label runat="server" ID="LblProcesos"></asp:Label>
                                <div class="table-responsive" runat="server" id="divTablaProc"></div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>

    <!--Modal Editar imagen-->
    <div class="modal fade"  id="modalEditImage" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Cambiar imagen</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                    
                  <div class="row">
                      <div class="col-12">
                            <label for="FileUpFoto">Adjuntar Foto</label> <asp:FileUpload runat="server" ID="FileUpFoto" CssClass="form-control-file" />
                           <asp:RegularExpressionValidator ID="RegularExpressionValidator7"
                               runat="server" ControlToValidate="FileUpFoto"
                               ErrorMessage="Se permite solo archivos con formato .jpg y .png" ForeColor="Red"
                               ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$"
                               ValidationGroup="ValFoto" SetFocusOnError="true"></asp:RegularExpressionValidator>
                          
                          
                      </div>
                    
                  </div>

              </div>
          </div>
          <div class="modal-footer">
                  
                  <span> </span>
                  <asp:Button runat="server" ID="BtnEditImage" ValidationGroup="ValFoto" OnClick="BtnEditImage_Click" CssClass="btn btn-outline-info" Text="Actualizar" />
              
              
          </div>
          
       </div>
  </div>
</div>

    <!--Modal precio-->
    <div class="modal fade"  id="modalEditPrecio" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header bg-info text-white">
            
            <h3>Editar Precio y Unidad de medición</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                  <asp:UpdatePanel runat="server" ID="UpdatePmodalprecio" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="row mb-3">
                                <div class="col-lg-6 col-12">
                                    <div class="form">
                                        <label>Magnitud:</label><asp:DropDownList runat="server" ValidationGroup="modalprecio" ID="DDLmagnitud" OnSelectedIndexChanged="DDLmagnitud_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="--Seleccionar Magnitud--" ID="Req_ID" Display="Dynamic" 
                                            ValidationGroup="modalprecio" runat="server" ControlToValidate="DDLmagnitud"
                                            Text="Debe Seleccionar Magnitud" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>   
                                </div>
                    
                          </div>
                          <div class="row mb-3">
                              <div class="col-lg-6 col-12">
                                  <div class="form">
                                      <asp:Label runat="server" ID="LblMedida1" Visible="false">Unidad de Medida:</asp:Label>
                                      <asp:DropDownList runat="server" ID="DDLUnidadmed" ValidationGroup="modalprecio" CssClass="form-control" Visible="false" OnSelectedIndexChanged="DDLUnidadmed_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                      <asp:RequiredFieldValidator InitialValue="--Seleccionar Unidad--" ID="Req_2" Display="Dynamic" 
                                            ValidationGroup="modalprecio" runat="server" ControlToValidate="DDLUnidadmed"
                                            Text="Debe Seleccionar Unidad" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </div>
                                  

                              </div>
                          </div>
                          <!--PRECIO POR UNIDAD DE MEDIDA-->
                          <div class="row mb-3">
                              <div class="col-12">
                                  <div class="form-inline">
                                      <asp:Label runat="server" ID="LblPrecioX" Visible="false" Text="Precio:"></asp:Label><span>&nbsp;</span>
                                      <asp:TextBox runat="server" ID="TxtPrecio" ValidationGroup="modalprecio" ToolTip="ej: 5000 o 4999,5" CssClass="form-control" Visible="false"></asp:TextBox><span>&nbsp;</span>
                                      <asp:Label runat="server" ID="LblSimbolUn" Visible="false" Font-Bold="true"></asp:Label>
                                  </div>
                                  <asp:RequiredFieldValidator ID="Req_3" Display="Dynamic" 
                                            ValidationGroup="modalprecio" runat="server" ControlToValidate="TxtPrecio"
                                            Text="Debe Ingresar precio" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                          </div>
                          <!--cANTIDAD DE EMBALAJE-->
                          <div class="row mb-3">
                              <div class="col-12">
                                  <div class="form-inline">
                                      <asp:Label runat="server" ID="LblCantX" Visible="false" Text="Cantidad:"></asp:Label><span>&nbsp;</span>
                                      <asp:TextBox runat="server" ID="TxtCant" ValidationGroup="modalprecio" CssClass="form-control" ToolTip="ej: 0,5 o 1" Visible="false"></asp:TextBox><span>&nbsp;</span>
                                      <asp:Label runat="server" ID="LblCant2" Visible="false" Font-Bold="true"></asp:Label>
                                  </div>
                                  <asp:RequiredFieldValidator ID="Req_4" Display="Dynamic" 
                                            ValidationGroup="modalprecio" runat="server" ControlToValidate="TxtCant"
                                            Text="Debe Ingresar la cantidad" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                          </div>

                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLmagnitud" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLUnidadmed" EventName="SelectedIndexChanged" />
                      </Triggers>
                  </asp:UpdatePanel>
                    
                  

              </div>
          </div>
          <div class="modal-footer bg-info text-white">
                  
                  <span> </span>
                  <asp:Button runat="server" ID="Btnmodalprecio" ValidationGroup="modalprecio" OnClick="Btnmodalprecio_Click" CssClass="btn btn-outline-light" Text="Actualizar" />
              
              
          </div>
          
       </div>
  </div>
</div>

    <!--Modal proceso-->
    <div class="modal fade"  id="modalEditProceso" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header fondo-lila text-white">
            
            <h3><asp:Label runat="server" ID="LblMdltitleProc"></asp:Label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                  <asp:UpdatePanel runat="server" ID="UpdatePanelproc" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="row mb-3">
                              <div class="col">
                                  <asp:DropDownList runat="server" ID="DDLProcesosxasignar" AutoPostBack="true" CssClass=" form-control" OnSelectedIndexChanged="DDLProcesosxasignar_SelectedIndexChanged"></asp:DropDownList>
                                  <asp:RequiredFieldValidator InitialValue="--Seleccionar Proceso--" ID="RqmdlProc1" Display="Dynamic" 
                                            ValidationGroup="modalProc" runat="server" ControlToValidate="DDLProcesosxasignar"
                                            Text="Debe Seleccionar un proceso" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                          </div>
                          <div class="row mb-3">
                              <div class="col">
                                  <div class="form-inline">
                                      <strong><asp:label runat="server" ID="LbldescripProcAsign1" Visible="false">Descripción:</asp:label></strong>&nbsp;<asp:Label runat="server" ID="LblDescripProcAsign"></asp:Label>
                                  </div>
                                  
                              </div>
                          </div>
                          <div class="row mb-3">
                              <div class="col">
                                  <div class="form-inline">
                                      <strong><asp:label runat="server" ID="LblCostoProcAsign1" Visible="false">Costo:</asp:label></strong>&nbsp;<asp:Label runat="server" ID="LblCostoProcAsign"></asp:Label>
                                  </div>
                                  
                              </div>
                          </div>
                          <div class="row mb-3">
                              <div class="col">
                                  <div class="form-check" data-toggle="tooltip" data-placement="bottom" title="Los procesos asignados a un componente son obligatorios.">
                                      <asp:RadioButton runat="server" ID="RaProcObligatorio" Checked="true" Enabled="false" Text="&nbsp;&nbsp;-Obligatorio" Visible="false"  />
                                  </div>
                              </div>
                          </div>
                         
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLProcesosxasignar" EventName="SelectedIndexChanged" />
                      </Triggers>
                  </asp:UpdatePanel>
                    
                  

              </div>
          </div>
          <div class="modal-footer fondo-lila text-white">
                  
                  <span> </span>
                  <asp:Button runat="server" ID="BtnAddProc" ValidationGroup="modalProc" OnClick="BtnAddProc_Click" CssClass="btn btn-outline-light" Text="Agregar" />
              
              
          </div>
          
       </div>
  </div>
</div>


    <!--Modal Eliminar Proceso-->
    <div class="modal fade"  id="modalDeleteProc" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Eliminar Proceso</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <asp:HiddenField runat="server" ID="HdnIdAsignProc" />
                  <div class="col">
                      <label id="LblMsgModalDelete"></label>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              <asp:Button runat="server" ID="BtnDeleteProc" CssClass="btn btn-success" Text="Eliminar" OnClick="BtnDeleteProc_Click" />
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                 
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>

    <!--Modal Vinculación con Alfak-->
    <div class="modal fade"  id="modalAsociarAlfak" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3><label id="TitleMdlAsocAlfak1" runat="server"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row mb-1">
                  <div class="col">
                      <h6><label id="TituloModalAsocAlfak" runat="server"></label></h6>
                      <asp:HiddenField runat="server" ID="HdnIdbeforeCreate" />
                  </div>
              </div>
              <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdateAsigAlfakCode">
                  <ContentTemplate>
                      <asp:Panel runat="server" ID="PanelAsocAlfak">
                          
                          <div class="row mb-3">
                                <div class="col-12">
                                    <div class="form">
                                        <label>Familia Alfak</label>
                                        <asp:DropDownList runat="server" CssClass="form-control" OnSelectedIndexChanged="DDLTipoProcAsocAlfak_SelectedIndexChanged" 
                                            AutoPostBack="true" ID="DDLTipoProcAsocAlfak"></asp:DropDownList>
                                    </div>

                                </div>
                           </div>
                          <div class="row mb-3">
                              <div class="col-12">
                                  <div class="form">
                                      <asp:Label runat="server" ID="LblforDDLSubtipo" Visible="false"></asp:Label>
                                      <asp:DropDownList runat="server" ID="DDLSubTipoProcAsocAlfak" CssClass="form-control"
                                          AutoPostBack="true" OnSelectedIndexChanged="DDLSubTipoProcAsocAlfak_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                                  </div>
                              </div>

                          </div>
                          
                          <div class="row mb-3">
                              <div class="col-12">
                                  <div class="form-inline">
                                      <asp:TextBox runat="server" ID="TxtSearchAlfak" OnTextChanged="TxtSearchAlfak_TextChanged" AutoPostBack="true" CssClass="form-control" Visible="false" 
                                         placeholder="texto" ValidationGroup="searchAlfak"></asp:TextBox>
                                      <asp:Button ID="BtnSearchAlfak" ValidationGroup="searchAlfak" runat="server" OnClick="TxtSearchAlfak_TextChanged" Text="Buscar" CssClass="btn btn-outline-info" Visible="false" />
                                  </div>
                                  
                              </div>
                          </div>
                          <div class="row">
                              <div class="col-12">
                                  <div class="table-responsive">
                                      <asp:GridView runat="server" AllowPaging="true"  ID="GrdListProcAlfak" OnPageIndexChanging="GrdListProcAlfak_PageIndexChanging" 
                                          DataKeyNames="CodigoAlfak" CssClass="table card-border-lila" AutoGenerateColumns="false" Visible="false">
                                      <Columns>
                                                        <asp:TemplateField HeaderText="Código">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkSelectAlfak" OnClick="LinkSelectAlfak_Click" Text='<%#Eval("CodigoAlfak")%>' runat="server"></asp:LinkButton>
                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="text-center" />

                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="EAN" HeaderText="Cod. Random" >

                                                        </asp:BoundField>
                                                        

                                                        
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" >

                                                        </asp:BoundField>

                                                    </Columns>
                                  </asp:GridView>
                                  </div>
                                  
                              </div>
                          </div>
                          
                      </asp:Panel>
                      

                  </ContentTemplate>
                  <Triggers>
                      
                      <asp:AsyncPostBackTrigger ControlID="DDLSubTipoProcAsocAlfak" EventName="SelectedIndexChanged" />
                      <asp:AsyncPostBackTrigger ControlID="DDLTipoProcAsocAlfak" EventName="SelectedIndexChanged" />
                      
                      <asp:AsyncPostBackTrigger ControlID="GrdListProcAlfak" EventName="PageIndexChanging" />
                      <asp:AsyncPostBackTrigger ControlID="BtnSearchAlfak" EventName="Click" />
                      <asp:AsyncPostBackTrigger ControlID="TxtSearchAlfak" EventName="TextChanged" />
                      
                  </Triggers>
              </asp:UpdatePanel>
          </div>
         
          
       </div>
  </div>
</div>


    <script type="text/javascript">
        jQuery(document).ready(function ($) {
            $('#<%=LinkTomdlAsocAlfak.ClientID%>').click(function () {
                var IdItem = $('#<%=HdnIdComp.ClientID%>').val();
                OpenModalAsocAlfak('Editar el código de Alfak Asociado?', IdItem, 'Editar Código');
                

            });

            $('.filas').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
            });
            $('.filas').mouseleave(function () {

                        $(this).css('text-decoration', '');
            });
            $('.filas').click(function () {
                var ID_ASIGN = $(this).find("td").eq(5).html();
                
                
                var nombre = $(this).find("td").eq(0).html();
                

                var nombrecomp = $('#<%= TxtNombre.ClientID%>').val();
                var msj = 'Desea eliminar el proceso "' + nombre + '" de "' + nombrecomp + '" ?';
                $('#<%=HdnIdAsignProc.ClientID%>').val(ID_ASIGN);
                $('#LblMsgModalDelete').html(msj);
                
                $('#modalDeleteProc').modal('show');
                
                
            });
  
  $('.zoom-image img').click(function(event){
    $('#modalEditImage').modal('show');
  })

  $('.zoom-image img').hover(function(){

      var img = $(this).attr('src');
      $(this).css('cursor', 'crosshair');

    $(this).after("<div class='hover-image' style='background-image: url(" + img + "); background-size: 1200px;'></div>");

    $(this).mousemove(function(event){

      // Mouse Position
      var mx = event.pageX;
      var my = event.pageY;

      // Image Position
      var ix = $(this).offset().left;
      var iy = $(this).offset().top;

      // Mouse Position Relavtive to Image
      var x = mx - ( ix );
      var y = my - ( iy );

      // Image Height and Width
      var w = $(this).width();
      var h = $(this).height();

      // Mouse Position Relative to Image, in %
      var xp = ( -x / w ) * -100;
      var yp = ( -y / h ) * -100;

      $(this).parent().find('.hover-image').attr('style',

      "background-image: url(" + img + "); background-size: 1200px; background-repeat: no-repeat; background-position: " + xp + "% " + yp + "%; top: " + y + "px; left: " + x + "px;");

    });

  }, function(){

    $(this).parent().find('.hover-image').remove();

  });

        });

        function OpenModalAsocAlfak(titulo, IdItem, titulo1) {
            
            
            $('#modalAsociarAlfak').modal('show');
            $('#<%=HdnIdbeforeCreate.ClientID%>').val(IdItem);
        }

    </script>
    
</asp:Content>