<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Distribuidor/SiteDistr.master" AutoEventWireup="true" CodeFile="Formulario.aspx.cs" Inherits="_Cotizador" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
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
    <div class="container">
        <div class="row mb-5">
            <div class="col-12 text-right">
                <asp:Button runat="server" ID="BtnGenExcel" OnClick="BtnGenExcel_Click" CssClass="btn btn-outline-info" Text="Descargar Excel" />
            </div>
        </div>
        <div class="row mb-5">
            <div class="col-12">
                <h1 class="text-center"><asp:Label runat="server" ID="TituloDoc"></asp:Label></h1>
                
            </div>
        </div>
        <div class="row mb-5 ">
            <div class="col-12 col-lg-6"></div>
            <div class="col-12 col-lg-6">
                <asp:UpdatePanel runat="server" ID="updatevalidez" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-inline text-left">
                    <label>Cotización válida hasta &nbsp;</label> <asp:TextBox runat="server" CssClass="form-control" placeholder="dd-mm-yyyy" ID="TxtValidez" Enabled="false"></asp:TextBox>
                            <asp:LinkButton runat="server" ID="LinkValidez" OnClick="LinkValidez_Click" CssClass="btn btn-link" Text="Cambiar"></asp:LinkButton>
                </div>
                <!-- validez DEL DOC-->
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkValidez" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </div>
        </div>
        <div class="row mb-5">
            <div class="col-lg-12">
                <asp:UpdatePanel runat="server" ID="UpdatePnl1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form">
                    <label><strong>Cliente:</strong></label>&nbsp;&nbsp;<asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtNombreCli"></asp:TextBox>
                            <asp:LinkButton runat="server" ID="LinkbtnNombreCli" OnClick="LinkbtnNombreCli_Click" Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkbtnNombreCli" EventName="Click" />
                    </Triggers>
                    
                </asp:UpdatePanel>
                
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-lg-6 col-12">
                <div class="card fondo-uva">
                    <div class="row">
                        <div class="col-12 align-middle text-white">
                            <div runat="server" style="text-align:center;" id="DivMargen" >
                                
                                
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-12">
                <div class="card bg-info">
                    <div class="row">
                        <div class="col-12 text-center text-white">
                            <div runat="server" style="text-align:center;" id="DivDscto"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--CARD costos fijos cotización-->
        <div class="card border-info">
            <div class="row mt-3">
            <div class="col-lg-6 col-12">
                <asp:UpdatePanel runat="server" ID="UpdateCostFijos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-3 text-center">
             <div class="col-12">
                            <asp:Label runat="server" Font-Bold="true" ID="LblOtrosCostos" Visible="false"></asp:Label>
                        </div>
                        <div class="col-12">
                            
                                <asp:Panel ID="PanelOtrosCosto" Visible="false" runat="server">
                                    
                                    <div class="form-inline">
                                       Otros Costos: &nbsp;&nbsp; <asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtOtrosCostos"></asp:TextBox>
                                    </div>
                                </asp:Panel>
                            <asp:LinkButton runat="server" ID="LinkBtnOtrosCostos" OnClick="LinkBtnOtrosCostos_Click"  Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                            
                        </div>
            
           
        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnOtrosCostos" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </div>
            <div class="col-lg-6 col-12">
                <asp:UpdatePanel runat="server" ID="UpdateCostFijos2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-3 text-center">
             <div class="col-12">
                            <asp:Label runat="server" Font-Bold="true" ID="LblGGral" Visible="false"></asp:Label>
                        </div>
                        <div class="col-12">
                            
                                <asp:Panel ID="PanelGgral" Visible="false" runat="server">
                                    
                                    <div class="form-inline">
                                       Gastos Generales: &nbsp;&nbsp; <asp:TextBox runat="server" MaxLength="100" CssClass="form-control" ID="TxtGGral"></asp:TextBox>
                                    </div>
                                </asp:Panel>
                            <asp:LinkButton runat="server" ID="LinkBtnGGral" OnClick="LinkBtnGGral_Click"  Text="Editar" CssClass="btn btn-link"></asp:LinkButton>
                            
                        </div>
            
           
        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnGGral" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </div>
        </div>
        </div>
        

        

        

        
        
        <br />
        <div class="container">
            <div class="row mb-2">
                <div class="col-12 text-center">
                    <button type="button" runat="server" id="BtnMdlAdditem" data-toggle="modal" data-target="#modalAddItem" class=" boton outline-lila">Agregar Item</button>
                </div>
            </div>
            <div class="row">
                
                    <div id="DivListaItem" class="table-responsive" runat="server">

                    </div>
                
            </div>
        </div>

        <div class="container">
            
                
                <asp:UpdatePanel runat="server" ID="UpdateTotales" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                        <div class="col-lg-8 col-12"></div>
                        <div class="col-lg-4 col-12">
                            <div class="table-responsive" id="DivListtotales" runat="server"></div>
                        </div>
                             </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnGGral" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="LinkBtnOtrosCostos" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
                
           
        </div>
        <!--observaciones-->
        <div class="row mb-5">
            <div class="col-12">
            
                    <div id="AccObs">
                         <div class="card">
                        <div class="card-header" id="CardHeaderObs">
                            <h5 class="mb-0">
                                <a data-toggle="collapse" data-parent="#AccObs" href="#collapseObs" > Observaciones de la cotización</a>
                            </h5>
                        </div>
                        <div id="collapseObs" class="collapse" >
                            <div class="card-body">
                                <asp:UpdatePanel runat="server" ID="UpdatePnl2" UpdateMode="Conditional">
                <ContentTemplate>

                                <div class="form">
                                    <asp:TextBox TextMode="MultiLine" runat="server" ID="TxtObservacion" CssClass="form-control"></asp:TextBox>
                                <asp:LinkButton runat="server" ID="LinkbtnObs" OnClick="LinkbtnObs_Click" CssClass="btn btn-link" Text="Actualizar"></asp:LinkButton>
                                </div>
                    </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="LinkbtnObs" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
                                
                            </div>
                        </div>

                    </div>
                    </div>
                   
                
            </div>

        </div>
    </div>

    <!--Modal AddItem-->
    <div class="modal fade"  id="modalAddItem" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3>Agregar Item</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  
                  <div class="col">
              
                  <asp:UpdatePanel ID="updateselectfamcat" runat="server" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="cuadro-bordeado mb-3">
                              <div class="col-12 col-lg-12 mb-3">
                                  <asp:DropDownList runat="server" CssClass="form-control" OnSelectedIndexChanged="DDlFamilia_SelectedIndexChanged" AutoPostBack="true" ID="DDlFamilia"></asp:DropDownList>
                              </div>
                              <div class="col-12 col-lg-12 mb-3">
                                  <asp:DropDownList runat="server" ID="DDLCategoria" CssClass="form-control" OnSelectedIndexChanged="DDLCategoria_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:DropDownList>
                              </div>
                              <div class="col-12 col-lg-12 mb-3">
                                  <asp:DropDownList runat="server" ID="DDLModelos" CssClass="form-control" OnSelectedIndexChanged="DDLModelos_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:DropDownList>
                              </div>
                          </div>
                          <asp:Panel ID="PanelInfoModel" Visible="false" runat="server">
                              <asp:HiddenField ID="HdnIdModel" runat="server" />
                              <div class="row">
                                <div class="col-lg-12 col-12 mb-1">
                                  <div class="img-fluid">
                                      <div runat="server" id="DivImg"></div>
                                  </div>
                              </div>
                                  <div class="col-lg-12 col-12 cuadro-bordeado mb-3">
                                      <asp:Label runat="server" ID="LblDetalle"></asp:Label>
                                  </div>
                              <div class="col-lg-12 col-12 bg-light">
                                  <div runat="server" class="table-responsive" id="DivtablaBom"></div>
                              </div>
                              </div>
                          </asp:Panel>
                          
                          
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="DDlFamilia" EventName="SelectedIndexChanged" /> 
                          <asp:AsyncPostBackTrigger ControlID="DDLCategoria" EventName="SelectedIndexChanged" /> 
                          <asp:AsyncPostBackTrigger ControlID="DDLModelos" EventName="SelectedIndexChanged" /> 
                      </Triggers>
                  </asp:UpdatePanel>
              
                  </div>
              </div>
              
          </div>
          <div class="modal-footer">
              <asp:Button runat="server" ID="BtnSigAddModel" CssClass="btn btn-lila" Text="Siguiente" OnClick="BtnSigAddModel_Click" />
          </div>
          
       </div>
  </div>
</div>

    <!--Modal iTEM-->
    <div class="modal fade"  id="modalItem" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3><label id="ModalItemtitulo"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <asp:HiddenField runat="server" ID="HdnIDENDODET" />
              <div class="row mb-3">
                  <div class="col-lg-4 col-12"></div>
                  <div class="col-lg-4">
                      <asp:Button runat="server" ID="BtnEditItem" OnClick="BtnEditItem_Click" CssClass="btn btn-success" Text="Editar Item" />
                  </div>
              </div>
              <div class="row mb-3">
                  <div class="col-lg-4 col-12"></div>
                  <div class="col-lg-4">
                      <!--abrir modal eliminar item-->
                      <button type="button" class="btn btn-danger" data-toggle="modal" data-target="#modalDeleteItem">Eliminar Item</button>
                  </div>
              </div>
          </div>
          
          
       </div>
  </div>
</div>

    <!--Modal Eliminar Item-->
    <div class="modal fade"  id="modalDeleteItem" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          
          <div class="modal-body">
              
              <div class="row mb-3">
                  <div class="col">
                      <h3><label id="LblDeleteItem"></label> </h3>
                  </div>
                  <div class="col-lg-4 col-12"></div>
                  <div class="col-lg-4">
              
                  </div>
              </div>
             
          </div>
          <div class="modal-footer">
              <div class=" btn-group">
                  
                  <asp:Button runat="server" ID="BtnEliminarItem" OnClick="BtnEliminarItem_Click" CssClass="btn btn-danger" Text="SI" />
                  <button type="button" class="btn btn-secondary" data-dismiss="modal">NO</button>
              </div>
          </div>
          
       </div>
  </div>
</div>


    <!--Modal Margen-->
    <div class="modal fade"  id="modalMargen" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3><label id="ModalMargenTitle"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              
              <div class="row mb-2 mb-5">
                    <div class="col-12 mb-4">
                        <label>Margen:</label>&nbsp;<strong><label id="LblMargenEdit"></label></strong>
                    </div>
                    <div class="col-12">
                        <asp:HiddenField runat="server" ID="hdnvarslidermargen" />
                        <div class="range-slider-container">
                            <input type="range" class="range-slider" id="RangeMargenEdit" />
                            <span  id="range-value-bar"></span>
                            <span id="range-value">0</span>
                        </div>

                    </div>
                </div>
              
          </div>
            <div class=" modal-footer">
                <asp:Button runat="server" ID="BtnMdlMargen" OnClientClick="javascript:Loading();" OnClick="BtnMdlMargen_Click" Text="Aceptar" CssClass="btn btn-outline-success" />
            </div>
          
          
       </div>
  </div>
</div>
    
    <!--Modal Dscto-->
    <div class="modal fade"  id="modalDscto" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3><label id="ModalDsctoTitle"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              
              <div class="row mb-2 mb-5">
                    <div class="col-12 col-lg-12 mb-4">
                        <label>Descuento:</label>&nbsp;<strong><label id="LblDsctoEdit"></label></strong>
                    </div>
                    <div class="col-12 col-lg-12">
                        <asp:HiddenField runat="server" ID="HdnVarsliderdscto" />
                        <div class="range-slider-container">
                            <input type="range" class="range-slider" id="RangeDsctoEdit" />
                            <span  id="range-value-bar-2"></span>
                            <span id="range-value-2" ></span>
                        </div>

                    </div>
                </div>
          </div>
            <div class=" modal-footer">
                <asp:Button runat="server" ID="BtnMdlDscto" Text="Aceptar" OnClientClick="javascript:Loading();" CssClass="btn btn-outline-success" OnClick="BtnMdlDscto_Click" />
            </div>
          
          
       </div>
  </div>
</div>
    <script type="text/javascript">


        function Loading() {

            $('#modalMargen').modal('hide');
            $('#modalDscto').modal('hide');
            
            $('html').bind('keydown', function (e) {
                if (e.keyCode == 13 || e.keyCode == 32) {
                    return false;
                }
            });
            $('#LoadingMsj').html('Espere un momento...');
            $('#modalLoading').modal('show').css('pointer-events','none');
        }
        

        $(document).ready(function () {


            $('.filasMargen').hover(function () { $(this).css('cursor', 'pointer'); });
            $('.filasDscto').hover(function () { $(this).css('cursor', 'pointer'); });

            /*Margen*/
            $('.filasMargen').click(function () {
                $('#modalMargen').modal('show');
                $('#ModalMargenTitle').html($(this).find("td").eq(1).html());
                var _margen = $(this).find("td").eq(2).html();
                var Margenstr = _margen.replace(",", ".");
                var porc = Margenstr * 100;
                $('#LblMargenEdit').html(porc);
                console.log(Margenstr.toString() + ' - ' + porc.toString());
                $('.range-slider').val(porc);

                $('#<%= hdnvarslidermargen.ClientID%>').val(_margen);
                rangeValueHandler();
            });

            /*Descuento*/
            $('.filasDscto').click(function () {
                $('#modalDscto').modal('show');
                $('#ModalDsctoTitle').html($(this).find("td").eq(1).html());
                var _Dscto = $(this).find("td").eq(2).html();
                var Dsctostr = _Dscto.replace(",", ".");
                var porc = Dsctostr * 100;
                $('#LblDsctoEdit').html(porc);
                console.log(Dsctostr.toString() + ' - ' + porc.toString());
                $('.range-slider').val(porc);

                $('#<%= HdnVarsliderdscto.ClientID%>').val(_Dscto);
                rangeValueHandler();
            });



            $('.filasDscto').click(function () {
                $('#modalDscto').modal('show');
                $('#ModalDsctoTitle').html($(this).find("td").eq(1).html());
            });

            $('td[rel=popover]').popover({
                html: true,
                trigger: 'hover',
                placement: 'right',
                delay: {show:100, hide:0},
                content: function () {
                    return '<img class="img-fluid" src="' + $(this).data('img') + '" />';
                }
                });

            /*item*/
            $('.filas').hover(function () {
                        $(this).css('cursor', 'pointer');
                $(this).css('text-decoration', 'underline');
               
                
            });
            $('.filas').mouseleave(function () {

                $(this).css('text-decoration', '');
                $('[data-toggle="popover"]').popover('hide');  
            });
            $('.filas').click(function () {
                var IDENDODET = $(this).find("td").eq(0).html();
                var foto = $(this).find("td").eq(1).html();
                var Numero = $(this).find("td").eq(2).html();
                var nombre = $(this).find("td").eq(3).html();
                
                /*Abrir modal*/
                $('#modalItem').modal('show');
                $('#ModalItemtitulo').html("Item " + Numero + " - " + nombre);
                $('#LblDeleteItem').html("está seguro que desea eliminar el Item " + Numero + " - " + nombre);
                $('#<%= HdnIDENDODET.ClientID%>').val(IDENDODET);
                
                


            });

        });



        /*Margen*/
        const rangeSlider = document.querySelector('.range-slider');
        const rangeSlider2 = document.querySelector('#RangeDsctoEdit');

        const rangeValueBar = document.querySelector('#range-value-bar');
        const rangeValueBar2 = document.querySelector('#range-value-bar-2');

        const rangeValue = document.querySelector('#range-value');
        const rangeValue2 = document.querySelector('#range-value-2');

        const margenValue = document.querySelector('#LblMargenEdit');
        const dsctoValue = document.querySelector('#LblDsctoEdit');

        let isDown = false;

        function dragHandler() {
            isDown = !isDown;
            if (!isDown) {
                
                rangeValue.style.setProperty('opacity', '0');
                rangeValue2.style.setProperty('opacity', '0');
            } else {
                
                rangeValue.style.setProperty('opacity', '1');
                rangeValue2.style.setProperty('opacity', '1');
            }
        }

        function dragOn(e) {
            console.log('dragOn');
            if (!isDown) return;
            rangeValueHandler();
        }

        function rangeValueHandler() {
            

            rangeValueBar.style.setProperty('width', `${rangeSlider.value}%`);
            rangeValueBar2.style.setProperty('width', `${rangeSlider2.value}%`);

            rangeValue.style.setProperty('transform', `translateX(-${this.value}%)`);
            rangeValue2.style.setProperty('transform', `translateX(-${this.value}%)`);

            rangeValue.innerHTML = rangeSlider.value + '%';
            rangeValue2.innerHTML = rangeSlider2.value + '%';

            var _valor = rangeSlider.value / 100;
            var _valor2 = rangeSlider2.value / 100;
            margenValue.innerHTML = rangeSlider.value + '%';
            dsctoValue.innerHTML = rangeSlider2.value + '%';

            $('#<%= hdnvarslidermargen.ClientID%>').val(_valor);
            $('#<%= HdnVarsliderdscto.ClientID%>').val(_valor2);

            rangeValue.style.setProperty('left', `${rangeSlider.value}%`);
            rangeValue2.style.setProperty('left', `${rangeSlider2.value}%`);
        }

        rangeValueHandler();
        rangeSlider.addEventListener('mousedown', dragHandler);
        rangeSlider2.addEventListener('mousedown', dragHandler);
        
        rangeSlider.addEventListener('mousemove', dragOn);
        rangeSlider2.addEventListener('mousemove', dragOn);


        rangeSlider.addEventListener('mouseup', dragHandler);
        rangeSlider2.addEventListener('mouseup', dragHandler);

        rangeSlider.addEventListener('click', rangeValueHandler);
        rangeSlider2.addEventListener('click', rangeValueHandler);
        /******************************************************************************************************************/


            /*Descuento*/
            
            
            
            

            
            /******************************************************************************************************************/

    </script>
   
   
</asp:Content>
