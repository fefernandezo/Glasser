<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="AdministracionProcesos.aspx.cs" Inherits="Com_Adm_Proc_" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>

         .range-slider-container {
  position: relative;
  top: 50%;
  left: 0;
  transform: translateY(-50%);
  padding-left:10px;
  padding-right:21px;
  margin-left:10px;
  margin-right:21px;
}
        

input[type=range] {
  -webkit-appearance: none;
  margin: 10px 0;
  width: 100%;
  position: absolute;
  top: 0;
  margin: 0;
}
#range-value-bar {
  width: 100%;
  content:"0";
  background-color: #5147c7;
  position: absolute;
  z-index: 10000;
  height: 25px;
  top: 0;
  margin: 0;
  border-radius: 5px;
}
#range-value-bar2 {
  width: 100%;
  content:"0";
  background-color: #5147c7;
  position: absolute;
  z-index: 10000;
  height: 25px;
  top: 0;
  margin: 0;
  border-radius: 5px;
}
#range-value {
  width: 25px;
  content:"0";
  background: rgba(233, 239, 244, 0.1);
  position: absolute;
  z-index: 10000;
  height: 25px;
  top: -40px;
  margin: 0;
  border-radius: 5px;
  left: 10%;
  transform: translateX(-50%);
  font-size: 20px;
  
  color: #41576B;
  box-shadow: 0 2px 10px 0 rgba(0,0,0,0.08);
  text-align: center;
  opacity: 0;
}
#range-value2 {
  width: 25px;
  content:"0";
  background: rgba(233, 239, 244, 0.1);
  position: absolute;
  z-index: 10000;
  height: 25px;
  top: -40px;
  margin: 0;
  border-radius: 5px;
  left: 10%;
  transform: translateX(-50%);
  font-size: 20px;
  
  color: #41576B;
  box-shadow: 0 2px 10px 0 rgba(0,0,0,0.08);
  text-align: center;
  opacity: 0;
}
input[type=range]:focus {
  outline: none;
}
input[type=range]::-webkit-slider-runnable-track {
  width: 100%;
  height: 25px;
  cursor: pointer;
  animate: 0.2s;
  box-shadow: 0px 0px 0px #000000, 0px 0px 0px #0d0d0d;
  background: #E9EFF4;
  border-radius: 5px;
  border: 0px solid #000101;
}
input[type=range]::-webkit-slider-thumb {
  box-shadow: 0 2px 10px 0 rgba(0,0,0,0.08);
  border: 14px solid #FFF;
  height: 53px;
  width: 53px;
  border-radius: 30px;
  background: #4600b9;
  cursor: pointer;
  -webkit-appearance: none;
  margin-top: -13.5px;
  position: relative;
  z-index: 1000000000;
}
input[type=range]::-moz-range-track {
  width: 100%;
  height: 12.8px;
  cursor: pointer;
  animate: 0.2s;
  box-shadow: 0px 0px 0px #000000, 0px 0px 0px #0d0d0d;
  background: #000;
  border-radius: 25px;
  border: 0px solid #000101;
}
input[type=range]::-moz-range-thumb {
  box-shadow: 0px 0px 0px #000000, 0px 0px 0px #0d0d0d;
  border: 0px solid #000000;
  height: 20px;
  width: 39px;
  border-radius: 7px;
  background: #000000;
  cursor: pointer;
}
input[type=range]::-ms-track {
  width: 100%;
  height: 12.8px;
  cursor: pointer;
  animate: 0.2s;
  background: transparent;
  border-color: transparent;
  border-width: 39px 0;
  color: transparent;
}
input[type=range]::-ms-fill-lower {
  background: #000;
  border: 0px solid #000101;
  border-radius: 50px;
  box-shadow: 0px 0px 0px #000000, 0px 0px 0px #0d0d0d;
}
input[type=range]::-ms-fill-upper {
  background: #000;
  border: 0px solid #000101;
  border-radius: 50px;
  box-shadow: 0px 0px 0px #000000, 0px 0px 0px #0d0d0d;
}
input[type=range]::-ms-thumb {
  box-shadow: 0px 0px 0px #000000, 0px 0px 0px #0d0d0d;
  border: 0px solid #000000;
  height: 20px;
  width: 39px;
  border-radius: 7px;
  background: #000;
  cursor: pointer;
}
       
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <div class="container">
        <div class="row mb-3">
            <div class="col text-center">
                <button type="button" id="BtnCrearComponente" onclick="OpenModal()" class="btn btn-lila btn-lg">Crear proceso</button>
            </div>
        </div>

        <div class="card mb-3 border-info">
            <div class="card-body">
                <div class="row mb-3" id="rowfilter">
            <div class="col-lg-8 mb-3 col-12">
                <div class="form-row">
                    <div class="col-8">
                        <asp:TextBox runat="server" ID="txtSearch" data-toggle="tooltip" CssClass="form-control" data-placement="bottom" title="Buscar por nombre o descripción" placeholder="Buscar por nombre o descripción" ></asp:TextBox>
                    </div>
                    <div class="col-2">
                        <asp:Button runat="server" ID="SearchBtn" OnClick="SearchBtn_Click" Text="Buscar" CssClass="btn btn-info" />
                    </div>
                    
                </div>
                
            </div>
            <div class="col-lg-4 col-12 text-right">
                <button type="button" class="btn btn-light" data-toggle="modal" data-target="#modalfilter"><i class="fas fa-filter fa-lg"></i></button>
                
            </div>
        </div>
            </div>
        </div>
        
        <div class="row mb-3" id="rowtable">
            <div class="table-responsive" runat="server" id="divtabla">

            </div>
        </div>
    </div>

    <!--MODAL CREAR PROCESO-->
    <div class="modal fade" id="modalCrearProc" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header card-bg-lila text-white">
                        <h3 class="modal-title" id="gridSystemModalLabel">Crear Proceso </h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                
                <div class="row mb-1">
                    <div class="col-12 col-lg-6">
                        <div class=" form-group">
                            <label for="TxtNombreProc">Nombre:</label><asp:TextBox runat="server" ID="TxtNombreProc" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" 
                                            ValidationGroup="CrearProcGroup" runat="server" ControlToValidate="TxtNombreProc"
                                            Text="El nombre es obligatorio" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    
                </div>
                <div class="row mb-3">
                    <div class="col">
                        <div class=" form-group">
                           <label for="TxtDescripcion">Descripción:</label><asp:TextBox runat="server" ID="TxtDescripcion" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!--precio y Magnitud-->
                
                    <asp:UpdatePanel runat="server" ID="UpdatePmodalprecio" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="row mb-1">
                                <div class="col-12 col-lg-6">
                                    <div class="form">
                                        <label>Magnitud:</label><asp:DropDownList runat="server" ValidationGroup="CrearProcGroup" ID="DDLmagnitud"
                                            OnSelectedIndexChanged="DDLmagnitud_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="--Seleccionar Magnitud--" ID="Req_ID" Display="Dynamic" 
                                            ValidationGroup="CrearProcGroup" runat="server" ControlToValidate="DDLmagnitud"
                                            Text="Debe Seleccionar Magnitud" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>   
                                </div>
                    
                          </div>
                          <div class="row mb-2">
                              <div class="col-12 col-lg-6">
                                  <div class="form">
                                      <asp:Label runat="server" ID="LblMedida1" Visible="false">Unidad de Medida:</asp:Label>
                                      <asp:DropDownList runat="server" ID="DDLUnidadmed" ValidationGroup="CrearProcGroup" CssClass="form-control" Visible="false" OnSelectedIndexChanged="DDLUnidadmed_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                      <asp:RequiredFieldValidator InitialValue="--Seleccionar Unidad--" ID="Req_2" Display="Dynamic" 
                                            ValidationGroup="CrearProcGroup" runat="server" ControlToValidate="DDLUnidadmed"
                                            Text="Debe Seleccionar Unidad" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </div>
                                  

                              </div>
                          </div>
                          <div class="row mb-4">
                              <div class="col-12 col-lg-6">
                                  <div class="form-inline">
                                      <asp:Label runat="server" ID="LblPrecioX" Visible="false" Text="Costo:"></asp:Label><span>&nbsp;</span>
                                      <asp:TextBox runat="server" ID="TxtPrecio" ValidationGroup="CrearProcGroup" CssClass="form-control" Visible="false"></asp:TextBox><span>&nbsp;</span>
                                      <asp:Label runat="server" ID="LblSimbolUn" Visible="false" Font-Bold="true"></asp:Label>
                                  </div>
                                  <asp:RequiredFieldValidator ID="Req_3" Display="Dynamic" 
                                            ValidationGroup="CrearProcGroup" runat="server" ControlToValidate="TxtPrecio"
                                            Text="Debe Ingresar el costo" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                          </div>

                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLmagnitud" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLUnidadmed" EventName="SelectedIndexChanged" />
                      </Triggers>
                  </asp:UpdatePanel>

                <div class="row mb-2 mb-5">
                    <div class="col-12 mb-4">
                        <label>Merma:</label>&nbsp;<strong><label id="Lblmerma"></label></strong>
                    </div>
                    <div class="col-12">
                        
                        <div class="range-slider-container">
                            <input type="range" class="range-slider" value="10" />
                            <span id="range-value-bar"></span>
                            <span id="range-value">0</span>
                        </div>

                    </div>
                </div>
                <asp:HiddenField runat="server" ID="HdnMerma" />
                
                <div class="row mb-2">
                    
                    <div class="col-12 text-right">
                        
                            <asp:Button runat="server" ID="BtnCrearComp" OnClick="BtnCrearProc_Click" ValidationGroup="CrearProcGroup" CssClass="boton outline-lila" Text="Crear Proceso" />
                        
                    </div>
                </div>
         
              
               
                
            </div> 
            
            </div>
        </div><!-- /.modal-content -->
      </div>

    <!--MODAL EDITAR PROCESO-->
    <div class="modal fade" id="modalEditProc" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalEdit" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header card-bg-lila text-white">
                        <h3 class="modal-title" id="gridSystemModalEdit">Crear Proceso </h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                
                <div class="row mb-1">
                    <div class="col-12 col-lg-6">
                        <div class=" form-group">
                            <label for="TxtMdlEditNombre">Nombre:</label><asp:TextBox runat="server" ID="TxtMdlEditNombre" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Rqmodaledit1" Display="Dynamic" 
                                            ValidationGroup="EditProcGroup" runat="server" ControlToValidate="TxtMdlEditNombre"
                                            Text="El nombre es obligatorio" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    
                </div>
                <div class="row mb-3">
                    <div class="col">
                        <div class=" form-group">
                           <label for="TxtMdlEditDescr">Descripción:</label><asp:TextBox runat="server" ID="TxtMdlEditDescr" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-12">
                         <div class="form-inline">
                                            <asp:Label runat="server" ID="LblEditAsocAlfak"></asp:Label> &nbsp;
                                        <div id="LinkTomdlAsocAlfak" style="cursor:pointer;color:dodgerblue;" runat="server"></div>
                                        
                                        </div>
                            
                        
                        
                    </div>
                </div>
                <!--precio y Magnitud-->
                
                    <asp:UpdatePanel runat="server" ID="UpPanelMdlEdit1" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="row mb-1">
                                <div class="col-12 col-lg-6">
                                    <div class="form">
                                        <label>Magnitud:</label><asp:DropDownList runat="server" ValidationGroup="CrearProcGroup" ID="DDLMdlEditMag"
                                            OnSelectedIndexChanged="DDLMdlEditMag_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="--Seleccionar Magnitud--" ID="RqMdlEdit3" Display="Dynamic" 
                                            ValidationGroup="EditProcGroup" runat="server" ControlToValidate="DDLMdlEditMag"
                                            Text="Debe Seleccionar Magnitud" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>   
                                </div>
                    
                          </div>
                          <div class="row mb-2">
                              <div class="col-12 col-lg-6">
                                  <div class="form">
                                      <asp:Label runat="server" ID="LblMdlEditLbl1" >Unidad de Medida:</asp:Label>
                                      <asp:DropDownList runat="server" ID="DDLMdlEditUnMed" ValidationGroup="EditProcGroup" CssClass="form-control" 
                                          OnSelectedIndexChanged="DDLMdlEditUnMed_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                      
                                      <asp:RequiredFieldValidator InitialValue="--Seleccionar Unidad--" ID="RqMdlEdit4" Display="Dynamic" 
                                            ValidationGroup="EditProcGroup" runat="server" ControlToValidate="DDLMdlEditUnMed"
                                            Text="Debe Seleccionar Unidad" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </div>
                                  

                              </div>
                          </div>
                          <div class="row mb-4">
                              <div class="col-12 col-lg-6">
                                  <div class="form-inline">
                                      <asp:Label runat="server" ID="LblMdlEditCost1" Text="Costo:"></asp:Label><span>&nbsp;</span>
                                      <asp:TextBox runat="server" ID="TxtMdlEditCosto" ValidationGroup="EditProcGroup" CssClass="form-control"></asp:TextBox><span>&nbsp;</span>
                                      <asp:Label runat="server" ID="LblMdlEditSimbUnit" Font-Bold="true"></asp:Label>
                                  </div>
                                  <asp:RequiredFieldValidator ID="RqMdlEdit5" Display="Dynamic" 
                                            ValidationGroup="EditProcGroup" runat="server" ControlToValidate="TxtMdlEditCosto"
                                            Text="Debe Ingresar el costo" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                          </div>

                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLMdlEditMag" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLMdlEditUnMed" EventName="SelectedIndexChanged" />
                      </Triggers>
                  </asp:UpdatePanel>

                <div class="row mb-2 mb-5">
                    <div class="col-12 mb-4">
                        <label>Merma:</label>&nbsp;<strong><label id="LblMdlEditMerma"></label></strong>
                    </div>
                    <div class="col-12">
                        
                        <div class="range-slider-container">
                            <input type="range" class="range-slider2" id="RangeMdlEdit" />
                            <span id="range-value-bar2"></span>
                            <span id="range-value2">0</span>
                        </div>

                    </div>
                </div>
                <asp:HiddenField runat="server" ID="HdnMdlEditMerma" />
                
                <div class="row mb-2">
                    
                    <div class="col-12 text-right">
                        
                            <asp:Button runat="server" ID="BtnMdlEditProc" OnClick="BtnMdlEditProc_Click"  ValidationGroup="EditProcGroup" CssClass="boton outline-lila" Text="Editar Proceso" />
                        
                    </div>
                </div>
         
              
               
                
            </div> 
            
            </div>
        </div><!-- /.modal-content -->
      </div>

    <!--MODAL Detalle-->
    <div class="modal fade" id="modalDetalle" tabindex="-1" role="dialog" aria-labelledby="tituloDetail" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
                        <h3 class="modal-title" runat="server" id="lbltitleDetail"></h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                <asp:HiddenField runat="server" ID="HdnIDItem" />
                <asp:HiddenField runat="server" ID="HdnTokenIdItem" />
                
                
               
               <div class="row mb-3">
                   <div class="col-11">
                       <div class="form">
                           <strong><label>Descripción:</label></strong>
                           <label id="LblDescrip"></label>
                       </div>
                   </div>

               </div>
                <div class="row mb-3">
                   <div class="col-11">
                       <div class="form">
                           <strong><label>Costo Proceso:</label></strong>
                           <label id="LblCosto"></label>
                       </div>
                   </div>

               </div>
                <div class="row mb-3">
                   <div class="col-11">
                       <div class="form">
                           <strong><label>Merma:</label></strong>
                           <label id="LblMerma"></label>
                       </div>
                   </div>

               </div>
                <div class="row mb-3">
                    <div class="col-11">
                        <div class="form">
                            <strong><label>Código Alfak:</label></strong>
                            <label id="lblAsocAlfak"></label>
                        </div>
                    </div>
                </div>
                
            </div> 
            <div class="modal-footer">
               
             <asp:Button runat="server" ID="BtnEditItem" OnClick="BtnEditItem_Click" CssClass="btn btn-onix" Text="Editar" />
                <button type="button" class="btn btn-danger" data-toggle="modal" data-target="#modaldeleteitem">Eliminar</button>
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>


     <!--Modal delete item-->
    <div class="modal fade"  id="modaldeleteitem" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Mensaje</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col">
                      <label>Seguro que desea eliminar este proceso?</label>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-dismiss="modal">NO</button>
                  <asp:Button runat="server" ID="BtnDelete" CssClass="btn btn-primary" OnClick="BtnDelete_Click" Text="SI" />
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>

     <!--Modal filter-->
    <div class="modal fade"  id="modalfilter" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Filtro</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col">
                      
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                  <asp:Button runat="server" ID="Filtrar" OnClick="Filtrar_Click" CssClass="btn btn-primary" Text="Listo" />
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>

    <!--Modal Vinculación con Alfak-->
    <div class="modal fade"  id="modalAsociarAlfak" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3><label id="TitleMdlAsocAlfak1"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row mb-1">
                  <div class="col">
                      <h6><label id="TituloModalAsocAlfak"></label></h6>
                      <asp:HiddenField runat="server" ID="HdnIdbeforeCreate" />
                  </div>
              </div>
              <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdateAsigAlfakCode">
                  <ContentTemplate>
                      <asp:Panel runat="server" ID="PanelAsocAlfak" Visible="false">
                          
                          <div class="row">
                                <div class="col-12">
                                    <div class="form">
                                        <label>Tipo</label>
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
                      <asp:AsyncPostBackTrigger ControlID="BtnAsociarAlfak" EventName="Click" />
                      <asp:AsyncPostBackTrigger ControlID="DDLSubTipoProcAsocAlfak" EventName="SelectedIndexChanged" />
                      <asp:AsyncPostBackTrigger ControlID="DDLTipoProcAsocAlfak" EventName="SelectedIndexChanged" />
                      <asp:AsyncPostBackTrigger ControlID="GrdListProcAlfak" EventName="PageIndexChanging" />
                      <asp:AsyncPostBackTrigger ControlID="BtnSearchAlfak" EventName="Click" />
                      <asp:AsyncPostBackTrigger ControlID="TxtSearchAlfak" EventName="TextChanged" />
                      
                  </Triggers>
              </asp:UpdatePanel>
          </div>
          <div class="modal-footer">
              <div class=" btn-group" role="group" aria-label="Alfak Asig">
                  <asp:Button runat="server" ID="BtnAsociarAlfak" OnClick="BtnAsociarAlfak_Click" CssClass="btn btn-success" Text="Si" />
                  <asp:Button runat="server" ID="BtnNoAlfak" OnClick="BtnNoAlfak_Click" CssClass="btn btn-secondary" Text="No" />
              </div>
              
          </div>
          
       </div>
  </div>
</div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=LinkTomdlAsocAlfak.ClientID%>').click(function () {
                var IdItem = $('#<%=HdnIDItem.ClientID%>').val();
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
                var ID = $(this).find("td").eq(6).html();
                var token = $(this).find("td").eq(7).html();
                
                var nombre = $(this).find("td").eq(0).html();
                var descr = $(this).find("td").eq(1).html();
                var costo = $(this).find("td").eq(2).html();
                var merma = $(this).find("td").eq(3).html();
                var Alfak = $(this).find("td").eq(4).html();

                
                $('#<%=HdnIDItem.ClientID%>').val(ID);
                $('#<%=HdnTokenIdItem.ClientID%>').val(token);
                $('#LblMerma').html(merma);
                $('#lblAsocAlfak').html(Alfak);

                $('#LblCosto').html(costo);
                $('#lbltitleDetail').html(nombre);
                $('#modalDetalle').modal('show');
                
                $('#LblDescrip').html(descr);
            });
        });
        
        function OpenModal() {
            $('#modalCrearProc').modal('show');
            
        }

        function OpenModalAsocAlfak(titulo, IdItem, titulo1) {
            $('#TituloModalAsocAlfak').html(titulo);
            $('#TitleMdlAsocAlfak1').html(titulo1);
            $('#modalAsociarAlfak').modal('show');
            $('#<%=HdnIdbeforeCreate.ClientID%>').val(IdItem);
        }
        function OpenModalEdit(merma2) {
            var _Mermai = merma2 / 100;
            $('#RangeMdlEdit').val(merma2);
            
            $('.range-slider2').val(merma2);
            
            rangeValueHandler2();
            $('#modalEditProc').modal('show');
            
            $('#<%=HdnMdlEditMerma.ClientID%>').val(_Mermai);

        }

        const rangeSlider = document.querySelector('.range-slider');
        const rangeValueBar = document.querySelector('#range-value-bar');
        const rangeValue = document.querySelector('#range-value');
        const mermaValue = document.querySelector('#Lblmerma');

        

        


let isDown = false;

function dragHandler() {
  isDown = !isDown;
  if (!isDown) {
    rangeValue.style.setProperty('opacity', '0');
  } else {
    rangeValue.style.setProperty('opacity', '1');
  }
}

function dragOn(e) {
  if (!isDown) return;
  rangeValueHandler();
}

function rangeValueHandler() {
  rangeValueBar.style.setProperty('width', `${rangeSlider.value}%`);
  rangeValue.style.setProperty('transform', `translateX(-${this.value}%)`);
    rangeValue.innerHTML = rangeSlider.value + '%';
    var _merma = rangeSlider.value / 100;
    mermaValue.innerHTML = rangeSlider.value + '%' ;
    $('#<%= HdnMerma.ClientID%>').val(_merma);
  rangeValue.style.setProperty('left', `${rangeSlider.value}%`);
}

rangeValueHandler();
rangeSlider.addEventListener('mousedown', dragHandler);
rangeSlider.addEventListener('mousemove', dragOn);
rangeSlider.addEventListener('mouseup', dragHandler);
rangeSlider.addEventListener('click', rangeValueHandler);



        const rangeValueBar2 = document.querySelector('#range-value-bar2');
        const rangeValue2 = document.querySelector('#range-value2');
        const mermaValue2 = document.querySelector('#LblMdlEditMerma');
        const rangeSlider2 = document.querySelector('.range-slider2');

        let Esbajo = false;

        function dragHandler2() {
            Esbajo = !Esbajo;
            if (!Esbajo) {
                rangeValue2.style.setProperty('opacity', '0');
            } else {
                rangeValue2.style.setProperty('opacity', '1');
            }
        }
        function dragOn2(e) {
            if (!Esbajo) return;
            rangeValueHandler2();
        }

        function rangeValueHandler2() {
            rangeValueBar2.style.setProperty('width', `${rangeSlider2.value}%`);
                rangeValue2.style.setProperty('transform', `translateX(-${this.value}%)`);
                rangeValue2.innerHTML = rangeSlider2.value + '%';
                var _merma = rangeSlider2.value / 100;
                mermaValue2.innerHTML = rangeSlider2.value + '%' ;
                $('#<%= HdnMdlEditMerma.ClientID%>').val(_merma);
                rangeValue2.style.setProperty('left', `${rangeSlider2.value}%`);
        }

        
rangeSlider2.addEventListener('mousedown', dragHandler2);
rangeSlider2.addEventListener('mousemove', dragOn2);
rangeSlider2.addEventListener('mouseup', dragHandler2);
rangeSlider2.addEventListener('click', rangeValueHandler2);



    </script>
</asp:Content>