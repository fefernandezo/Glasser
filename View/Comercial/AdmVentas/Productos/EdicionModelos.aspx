<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="EdicionModelos.aspx.cs" Inherits="Com_Adm_Default_" %>

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
                <h1>Edición del Modelo</h1><asp:HiddenField runat="server" ID="HdnIdModel" />
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
        
       


        <!--CARD FAMILIA Y CATEGORIA-->
        <div class="card mb-3 text-white bg-secondary">
            <div class="card-body">
                
                <asp:UpdatePanel runat="server" ID="UPDateCategoria" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-3">
                                <div class="col-lg-6 col-12">
                                    <div class="form-inline">
                                        <h5>Familia y Categoría</h5><span>&nbsp; &nbsp;</span>
                                        <asp:LinkButton runat="server" ID="LinkBtnFamCat" OnClick="LinkBtnFamCat_Click" CssClass="btn btn-outline-light" Text="Editar"></asp:LinkButton>
                                    </div>
                                    
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-12 col-lg-6 mb-3">
                                <asp:DropDownList runat="server" ID="DDLFamilia" OnSelectedIndexChanged="DDLFamilia_SelectedIndexChanged" AutoPostBack="true" Enabled="false" CssClass="form-control"></asp:DropDownList>
                            </div>
                            
                            <div class="col-12 col-lg-6">
                                <asp:DropDownList runat="server" ID="DDLCategoria" Enabled="false" CssClass="form-control"></asp:DropDownList>

                            </div>
                        </div>
                        <asp:Label runat="server" ForeColor="Red" Visible="false" Text="Debe seleccionar familia y categoría" ID="AlertDDl"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLFamilia" EventName="SelectedIndexChanged" />
                           <asp:AsyncPostBackTrigger ControlID="LinkBtnFamCat" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
               
            </div>
            
            
        </div>
        <!--CARD Piezas y componentes-->
        <div class="card mb-3 text-white bg-info">
            <div class="card-body">
                <asp:UpdatePanel runat="server" ID="UpDatePrecio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-2">
                            <div class="col-lg-6 col-12">
                                    <div class="form-inline">
                                        <h5>Piezas, componentes y procesos</h5><span>&nbsp; &nbsp;</span>
                                        <button type="button" runat="server" id="BtnAddPieces" data-toggle="modal" data-target="#modalAddPieces" class="btn btn-outline-light">Agregar</button>
                                    </div>
                                    
                            </div>
                            
                        </div>
                        <div class="row mb-3 ml-5" id="rowtable">
                            
                            <div class="table-responsive" runat="server" id="divtabla"></div>
                            

                        </div>
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <!--CARD Canales de distribucion-->
        <div class="card mb-3 text-white bg-success">
        <div class="card-body">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-2">
                            <div class="col-lg-6 col-12">
                                    <div class="form-inline">
                                        <h5>Canales de distribución Asociados</h5><span>&nbsp; &nbsp;</span>
                                        <button type="button" runat="server" id="Button1" data-toggle="modal" data-target="#modalAddCanal" class="btn btn-outline-light">Agregar</button>
                                    </div>
                                    
                            </div>
                            
                        </div>
                        <div class="row mb-3 ml-5" >
                            
                            <div class="table-responsive" runat="server" id="divtablaCanales"></div>
                            

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

   

    <!--Modal AddPieces-->
    <div class="modal fade"  id="modalAddPieces" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header bg-info text-white">
            
            <h3>Agregar Pieza o Componente</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                  <asp:UpdatePanel runat="server" ID="UpdatePieceOrComp" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="row mb-3">
                              <div class="col">
                                  <asp:DropDownList runat="server" ID="DDLFirstPieceOrComp"
                                      AutoPostBack="true" OnSelectedIndexChanged="DDLFirstPieceOrComp_SelectedIndexChanged1" CssClass="form-control"></asp:DropDownList>
                                  <asp:RequiredFieldValidator InitialValue="--Seleccionar--" ID="RqmdlPiece1" Display="Dynamic" 
                                            ValidationGroup="modalAddPiece" runat="server" ControlToValidate="DDLFirstPieceOrComp"
                                            Text="Debe Seleccionar un item" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                          </div>
                          <!--Div de las piezas-->
      
                          
                                 <asp:Panel runat="server" ID="PanelDivPiezas" Visible="false">
                                     <div class="row mb-3">
                                         <div class="col-12">
                                             <div class="form-inline">
                                                 Nombre:&nbsp;<asp:TextBox CssClass="form-control" runat="server" ID="TxtMdlAPNamePiece"></asp:TextBox>
                                             </div>
                                         </div>
                                     </div>
                                     <div class="row mb-3">
                                         <div class="col-12 mb-3">
                                             <div class="form-inline">
                                                 Ancho Estándar:&nbsp;<asp:TextBox CssClass="form-control" runat="server" ID="TxtMdlAPAnchoPiece">0</asp:TextBox>mm.
                                             </div>
                                         </div>
                                         <div class="col-12 mb-3">
                                             <div class="form-inline">
                                                 Alto Estándar:&nbsp;<asp:TextBox CssClass="form-control" runat="server" ID="TxtMdlAPLargoPiece">0</asp:TextBox>mm.
                                             </div>
                                         </div>
                                     </div>
                                     <div class="row mb-3">
                                         <div class="col-12">
                                             <asp:LinkButton runat="server" ID="LinktoAsocAlfak" Visible="false" OnClick="LinktoAsocAlfak_Click">Asociar con pieza de Alfak</asp:LinkButton>
                                         </div>
                                     </div>
                                     <!--panel asociacion con Alfak-->
                                     <asp:Panel runat="server" ID="PnlLinktoAsocAlfak" Visible="false">
                                         <!--drop down familia-->
                                         
                          <div class="row mb-3">
                              <div class="col-12">
                                  <div class="form">
                                      <asp:Label runat="server" ID="LblforDDLSubtipo"></asp:Label>
                                      <asp:DropDownList runat="server" ID="DDLSubTipoProcAsocAlfak" CssClass="form-control"
                                          AutoPostBack="true" OnSelectedIndexChanged="DDLSubTipoProcAsocAlfak_SelectedIndexChanged"
                                          ></asp:DropDownList>
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
                                      <asp:GridView runat="server" AllowPaging="true"  ID="GrdListProcAlfak"  
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
                                     <asp:Panel runat="server" ID="PnlTxtSelectedCode" Visible="false">
                                         <div class="row mb-3">
                                             <div class="col-12">
                                                <label>Código Asociado: &nbsp;</label> <asp:Label runat="server" ID="TxtSelectedAlfakCode"></asp:Label>
                                             </div>
                                         </div>
                                     </asp:Panel>
                                 </asp:Panel>
                                  
                             
                          <!--Div de componentes-->
                          <asp:Panel runat="server" ID="PanelDivCompo" Visible="false">
                              <!--Seleccion de pieza-->
                              <div class="row">
                                  <div class="col">
                                      <label>a la pieza</label>
                                  </div>
                              </div>
                                     <div class="row mb-3">
                                         <div class="col">
                                            
                                             <asp:DropDownList runat="server" ID="DDLFirstDivCompo"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDLFirstDivCompo_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="--Seleccionar--" ID="ReqFieldValidator1" Display="Dynamic" 
                                                ValidationGroup="modalAddPiece" runat="server" ControlToValidate="DDLFirstDivCompo"
                                                Text="Debe Seleccionar un item" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>

                                         </div>

                                     </div>
                              <!--Seleccion Tipo de componente-->
                              <asp:Panel runat="server" ID="PanelSeleccionCompo" Visible="false">
                                  <div class="row mb-3">
                                      <div class="col-12">
                                          <div class="form">
                                              <label>Tipo Componente</label>
                                              <asp:DropDownList runat="server" ID="DDLSelcTipoCompo" CssClass="form-control" AutoPostBack="true" 
                                                  OnSelectedIndexChanged="DDLSelcTipoCompo_SelectedIndexChanged"></asp:DropDownList>
                                          </div>
                                      </div>
                                  </div>
                                   <div class="row mb-3">
                              <div class="col-12">
                                  <div class="form-inline">
                                      <asp:TextBox runat="server" ID="TxtSearchCompo" OnTextChanged="TxtSearchCompo_TextChanged" AutoPostBack="true" CssClass="form-control" Visible="false" 
                                         placeholder="texto" ValidationGroup="searchCompo"></asp:TextBox>
                                      <asp:Button ID="BtnSearchCompo" ValidationGroup="searchCompo" runat="server" OnClick="TxtSearchCompo_TextChanged" 
                                          Text="Buscar" CssClass="btn btn-outline-info" Visible="false" />
                                  </div>
                                  
                              </div>
                          </div>
                          <div class="row">
                              <div class="col-12">
                                  <div class="table-responsive">
                                      <asp:GridView runat="server" AllowPaging="true"  ID="GrdCompo"
                                          CssClass="table card-border-lila" AutoGenerateColumns="false" Visible="false">
                                      <Columns>
                                                        <asp:TemplateField HeaderText="Componentes" HeaderStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkSelectCompo" OnClick="LinkSelectCompo_Click" CssClass='<%#Eval("_ID")%>' Text='<%#Eval("Nombre")%>' runat="server"></asp:LinkButton>
                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="text-center" />

                                                        </asp:TemplateField>
                                                        

                                                        
                                                        

                                                    </Columns>
                                  </asp:GridView>
                                  </div>
                                  
                              </div>
                          </div>

                              </asp:Panel>
                              <!--Panel componente seleccionado-->
                              <asp:Panel ID="PnlCompoSelected" runat="server" Visible="false">
                                  <div class="row mb-3">
                                      
                                      <div class="col-12 mb-2">
                                          <asp:HiddenField runat="server" ID="HdnIdSelectedCompo" />
                                          <h4><asp:Label runat="server" ID="LblResCodCompo"></asp:Label> </h4> 
                                      </div>
                                      <div class="col-12 mb-3">
                                          <h5><asp:Label runat="server" ID="LblCostoCompo"></asp:Label></h5>
                                      </div>
                                      <div class="col-12 mb-3">
                                          <h5>
                                              <asp:CheckBox runat="server" ID="ChkIsOpcional" CssClass="form-check-inline" ForeColor="Red" Text="&nbsp; Componente Opcional" />
                                          </h5>
                                          
                                      </div>
                                      <div class="col-12 mb-2">
                                          <asp:Label runat="server" ID="LblForFormulaCompo"></asp:Label>
                                          
                                              
                                                  <div id="DivPreInfoFormula" runat="server"></div>
                                                  <asp:UpdatePanel runat="server" ID="UpdateFormula" UpdateMode="Conditional">
                                                      <ContentTemplate>
                                                          <asp:DropDownList runat="server" ID="DDLFormulasPred" CssClass="form-control" AutoPostBack="true" 
                                                      OnSelectedIndexChanged="DDLFormulasPred_SelectedIndexChanged"></asp:DropDownList>
                                                          <asp:TextBox runat="server" placeholder="Cantidad o Fórmula" ID="TxtFormulaCompo" 
                                                              title="Variables: ANCHO,LARGO (de la pieza seleccionada); Ej1: (ANCHO/1000)*(LARGO/1000)   Ej2: ((ANCHO/1000)+(LARGO/1000))*2" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                          <asp:LinkButton runat="server" ID="ValidarFormulacompo" OnClick="ValidarFormulacompo_Click" Text="Validar Formula"></asp:LinkButton>
                                                          <asp:Label runat="server" ID="LblValidadorFormulaCompo" ForeColor="DarkBlue"></asp:Label>
                                                      </ContentTemplate>
                                                      <Triggers>
                                                          <asp:AsyncPostBackTrigger ControlID="DDLFormulasPred" EventName="SelectedIndexChanged" />
                                                          <asp:AsyncPostBackTrigger ControlID="ValidarFormulacompo" EventName="Click" />
                                                      </Triggers>
                                                  </asp:UpdatePanel>
                                                  
                                              
                                              
                                              
                                              <div runat="server" id="DivInfoFormula"></div>
                                          
                                      </div>
                                      <div class="col-12 mb-2">
                                          
                                          <asp:Label runat="server" ID="LblHasProcCompo"></asp:Label>
                                      </div>
                                      <div class="col-12 mb-2">
                                          <div runat="server" id="PnlCompoSelDivHasProc"></div>
                                      </div>
                                  </div>
                                  <div class="row" runat="server" id="PnlCompoDivImage"></div>

                              </asp:Panel>
                              

                                 </asp:Panel>
                          
                          <!--div de procesos-->
                          <asp:Panel runat="server" ID="PanelDivProc" Visible="false">
                                    <div class="row">
                                        <div class="col">
                                            a la pieza
                                        </div>
                                    </div>
                                     <div class="row mb-3">
                                         <div class="col">
                                             <asp:DropDownList runat="server" ID="DDLFirstDivCompo2"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDLFirstDivCompo2_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="--Seleccionar--" ID="RequiredFieldValidator22" Display="Dynamic" 
                                                ValidationGroup="modalAddPiece" runat="server" ControlToValidate="DDLFirstDivCompo2"
                                                Text="Debe Seleccionar un item" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>

                                         </div>

                                     </div>
                              <asp:Panel runat="server" ID="PanelSelectProceso" Visible="false">
                                  <div class="row mb-3">
                                      <div class="col-12">
                                          <asp:DropDownList runat="server" CssClass="form-control"  
                                            AutoPostBack="true" ID="DDLTipoProcAsocAlfak" OnSelectedIndexChanged="DDLTipoProcAsocAlfak_SelectedIndexChanged"></asp:DropDownList>
                                      </div>
                                  </div>
                              </asp:Panel>
                              <asp:Panel runat="server" ID="PanelProcesoSelected" Visible="false">
                                  <div class="row mb-3">
                                      <div class="col-12">
                                          <asp:Label runat="server" ID="LblPnlProcSelectedCosto"></asp:Label>
                                      </div>
                                      <div class="col-12 mb-3">
                                          <asp:Label runat="server" ID="LblPnlProcSelectedMerma"></asp:Label>
                                      </div>
                                      <div class="col-12 mb-3">
                                          <h5>
                                              <asp:CheckBox runat="server" ID="ChkOpcionalProc" CssClass="form-check-inline" ForeColor="Red" Text="&nbsp; Proceso Opcional" />
                                          </h5>
                                      </div>
                                  </div>
                                  <asp:UpdatePanel ID="UpdatePnlProcSelected" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                          <div class="row mb-3">
                                              <div class="col">
                                                  <label>Rendimiento o ecuación de cálculo de cantidades:</label>
                                                  <asp:DropDownList runat="server" ID="DDLPnlProcSelectedFormula" CssClass="form-control" AutoPostBack="true" 
                                                       OnSelectedIndexChanged="DDLPnlProcSelectedFormula_SelectedIndexChanged" ></asp:DropDownList>
                                                          <asp:TextBox runat="server" placeholder="Cantidad o Fórmula" ID="TxtPnlProcSelectedFormula" 
                                                              title="Variables: ANCHO,LARGO (de la pieza seleccionada); Ej1: (ANCHO/1000)*(LARGO/1000)   Ej2: ((ANCHO/1000)+(LARGO/1000))*2" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                  <asp:LinkButton runat="server" ID="Validadorformulaproceso" OnClick="Validadorformulaproceso_Click"  Text="Validar Formula"></asp:LinkButton>
                                                          <asp:Label runat="server" ID="LblValidadorFormulaproc" ForeColor="DarkBlue"></asp:Label>
                                              </div>
                                          </div>
                                          
                                      </ContentTemplate>
                                      <Triggers>
                                          <asp:AsyncPostBackTrigger ControlID="DDLPnlProcSelectedFormula" EventName="SelectedIndexChanged" />
                                          <asp:AsyncPostBackTrigger ControlID="Validadorformulaproceso" EventName="Click" />
                                      </Triggers>
                                  </asp:UpdatePanel>
                              </asp:Panel>
                                 </asp:Panel>
                          
                         
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDLFirstPieceOrComp" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLFirstDivCompo" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLSelcTipoCompo" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="DDLTipoProcAsocAlfak" EventName="SelectedIndexChanged" />
                          
                          <asp:AsyncPostBackTrigger ControlID="DDLSubTipoProcAsocAlfak" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="LinktoAsocAlfak" EventName="Click" />
                          <asp:AsyncPostBackTrigger ControlID="BtnSearchAlfak" EventName="Click" />
                          
                      </Triggers>
                  </asp:UpdatePanel>
                    
                  

              </div>
          </div>
          <div class="modal-footer bg-info text-white">
                 <asp:Button runat="server" ID="BtnInsertPiece" OnClick="BtnAddPieces_Click" ValidationGroup="modalAddPiece" CssClass="btn btn-outline-light" Text="Agregar" />
          </div>
          
       </div>
  </div>
</div>


    <!--Modal Eliminar-->
    <div class="modal fade"  id="modalDeleteItem" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Eliminar</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <asp:HiddenField runat="server" ID="HdnMdlDelIdItem" />
                  <asp:HiddenField runat="server" ID="HdnMdlDelLevel" />
                  <div class="col">
                      <label id="LblMsgModalDelete"></label>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              <asp:Button runat="server" ID="BtnEliminarItem" CssClass="btn btn-danger" OnClick="BtnEliminarItem_Click" Text="Si" />
              
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                 
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>


    <!--Modal add Canal-->
    <div class="modal fade"  id="modalAddCanal" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header bg-success text-white">
            
            <h3>Eliminar</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <asp:UpdatePanel runat="server" ID="UpdatePAddCanal" UpdateMode="Conditional">
                  <ContentTemplate>
                      <div class="row">
                            <div class="col">
                                  <asp:DropDownList runat="server" ID="DDlSelectChanel"
                                       CssClass="form-control"></asp:DropDownList>
                                  <asp:RequiredFieldValidator InitialValue="--Seleccionar--" ID="RequiredFieldValidator1" Display="Dynamic" 
                                            ValidationGroup="modalAddCanal" runat="server" ControlToValidate="DDlSelectChanel"
                                            Text="Debe Seleccionar un item" ErrorMessage="ErrorMessage" ForeColor="Red"></asp:RequiredFieldValidator>
                              </div>
                  
                        </div>
                  </ContentTemplate>
                  <Triggers>
                      
                  </Triggers>
              </asp:UpdatePanel>
              
          </div>
          <div class="modal-footer bg-success text-white">
              <asp:Button runat="server" ID="BtnAddCanal" OnClick="BtnAddCanal_Click" ValidationGroup="modalAddCanal" CssClass="btn btn-outline-light" Text="Agregar" />
              
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                 
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>


    <!--Modal Eliminar Canal-->
    <div class="modal fade"  id="modalDeleteCanal" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Eliminar</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <asp:HiddenField runat="server" ID="HdnMdlDeleteCanalID" />
                  
                  <div class="col">
                      <label id="LblMsgModalDeleteCanal"></label>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              <asp:Button runat="server" ID="BtnMdlDeleteCanal" CssClass="btn btn-danger" OnClick="BtnMdlDeleteCanal_Click" Text="Si" />
              
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                 
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>
  


    <script type="text/javascript">
        $(document).ready(function(){
    $('[data-toggle="popover"]').popover();   
});
        jQuery(document).ready(function ($) {
            
            $('.filasCanal').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
            });
            $('.filasCanal').mouseleave(function () {

                        $(this).css('text-decoration', '');
            });
            $('.filasCanal').click(function () {

                var ID = $(this).find("td").eq(2).html();
                var nombre = $(this).find("td").eq(1).html();
                var mensaje = "Está seguro que desea eliminar el canal " + nombre + "?";

                $('#<%=HdnMdlDeleteCanalID.ClientID%>').val(ID);
                $('#LblMsgModalDeleteCanal').html(mensaje);
                
                $('#modalDeleteCanal').modal('show');
                
                
            });
            $('.filas').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
            });
            $('.filas').mouseleave(function () {

                        $(this).css('text-decoration', '');
            });
            $('.filas').click(function () {
                var ID = $(this).find("td").eq(4).html();
                var level = $(this).find("td").eq(3).html();
                var nombre = $(this).find("td").eq(1).html();
                var mensaje;

                if (level.trim()==="1") {
                    mensaje = "Está seguro que desea eliminar la pieza " + nombre + " junto con todos sus componentes y procesos asociados?";
                }

                if (level.trim()==="2") {
                    mensaje = "Está seguro que desea eliminar " + nombre + "?";
                }

                
                

                var nombrecomp = $('#<%= TxtNombre.ClientID%>').val();
                var msj = 'Desea eliminar el proceso "' + nombre + '" de "' + nombrecomp + '" ?';
                $('#<%=HdnMdlDelIdItem.ClientID%>').val(ID);
                $('#<%=HdnMdlDelLevel.ClientID%>').val(level);
                $('#LblMsgModalDelete').html(mensaje);
                
                $('#modalDeleteItem').modal('show');
                
                
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

       

    </script>
    
</asp:Content>