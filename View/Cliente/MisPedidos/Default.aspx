<%@ Page Title="Mis Pedidos" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="MPed_Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <style>
         .table th,
.table td {
  padding: 10px 30px;
}



.table th.asc:after {
  display: inline;
  content: '↓';
}

.table th.desc:after {
  display: inline;
  content: '↑';
}

    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>

    <div id="contenedor" class="container-fluid" runat="server">
        <br /><br /><br /><br /><br /><br />
        <div class="container-fluid">
            <!--titulo-->
          <div class="d-flex justify-content-center">
              <h1 class="text-light">Pedidos</h1>
          </div>
          <div class="d-flex justify-content-center mb-5">
              <h3 class="text-light">"<asp:Label runat="server" ID="LblEmpresa"></asp:Label>"</h3>
          </div>

            <div class="card mb-3 border-info bg-opacity-white-45" style="border-radius:20px;">
            <div class="card-body">
                <div class="row mb-3" id="rowfilter">
            <div class="col-lg-8 mb-3 col-12">
                <div class="form-row">
                    <div class="col-8">
                        <asp:TextBox runat="server" ID="txtSearch" data-toggle="tooltip" CssClass="form-control" data-placement="bottom" title="Buscar por nombre o descripción" placeholder="Buscar por nombre o descripción" ></asp:TextBox>
                    </div>
                    <div class="col-2">
                        <asp:Button runat="server" ID="SearchBtn" OnClick="SearchBtn_Click" Text="Buscar" CssClass="btn btn-info Borde-Radio-20px" />
                    </div>
                    
                </div>
                
            </div>
            <div class="col-lg-4 col-12 text-right">
                <button type="button" class="btn btn-light" data-toggle="modal" data-target="#modalfilter"><i class="fas fa-filter fa-lg"></i></button>
                
            </div>
        </div>
            </div>
        </div>
            <asp:UpdatePanel runat="server" ID="UpPnlTable" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card mb-3 bg-opacity-white-75">
                        <div class="container">
                            <div class=" d-flex flex-row-reverse mb-2 mt-2">
                                
                                <asp:DropDownList runat="server" Width="200" ID="DDlTopitems" OnSelectedIndexChanged="DDlTopitems_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                                    <asp:ListItem Text="Visualizar 10" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Visualizar 20" Value="20" ></asp:ListItem>
                                    <asp:ListItem Text="Visualizar 50" Value="50" ></asp:ListItem>
                                    <asp:ListItem Text="Visualizar 90" Value="90" ></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                <div runat="server" id="DivTPedidos" class="table-responsive"></div>
            </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnFiltering" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="DDlTopitems" EventName="SelectedIndexChanged" />
                    <asp:PostBackTrigger ControlID="BtnEliminarGo" />
                </Triggers>
            </asp:UpdatePanel>
            
            <br />
        <br />

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

    <!--Modal Filter-->
    <div class="modal fade"  id="modalfilter" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content bg-opacity-dark-35">
          <div class="modal-header text-white"><h4>Filtros</h4>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true" class="text-white">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="accordion" id="FilterAccord">
                  <div class="card bg-opacity-white-25 mb-1">
                      <div class="card-header bg-opacity-white-25 text-white" id="HeadingFilterDates">
                          <a  data-toggle="collapse" data-target="#FilterCollapseDates" aria-expanded="false" aria-controls="FilterCollapseDates">
                             <label style="cursor:pointer;" class="HighLight-1">Fechas</label> 
                          </a>
                      </div>
                      <div id="FilterCollapseDates" class="collapse" aria-labelledby="HeadingFilterDates" data-parent="FilterAccord">
                          <div class="card-body bg-opacity-white-65">
                              <div class="row mb-2">
                                <div class="col-lg-6 col-12 mb-2">
                                    <div class="form-inline">
                                        <label for="FilterDateFrom">Desde &nbsp;&nbsp;</label>
                                        <input type="date" id="FilterDateFrom" onchange="FuFilterDateFrom()" class="form-control" />
                                    </div>
                                    <asp:HiddenField runat="server" ID="HdnFilterDateFrom" />
                                </div>
                                <div class="col-lg-6 col-12 mb-2">
                                    <div class=" form-inline">
                                        <label for="FilterDateTo">Hasta &nbsp;&nbsp;</label>
                                        <input type="date" id="FilterDateTo" onchange="FuFilterDateTo()" class="form-control" />
                                    </div>
                                    <asp:HiddenField runat="server" ID="HdnFilterDateTo" />
                                </div>

                              </div>
                          </div>
                      </div>
                  </div>

                  <div class="card bg-opacity-white-25 mb-1">
                      <div class="card-header bg-opacity-white-25 text-white" id="HeadingFilterAmount">
                          <a  data-toggle="collapse" data-target="#FilterCollapseAmount" aria-expanded="false" aria-controls="FilterCollapseAmount">
                             <label style="cursor:pointer;" class="HighLight-1">Montos</label> 
                          </a>
                      </div>
                      <div id="FilterCollapseAmount" class="collapse" aria-labelledby="HeadingFilterAmount" data-parent="FilterAccord">
                          <div class="card-body bg-opacity-white-65">
                              <div class="row mb-2">
                                <div class="col-lg-12 col-12 mb-5">
                                    
                                        <div class="range-slider-container">
                                            <input type="range" class="range-slider" id="RangeMin" value="0" />
                                            <input type="range" class="range-slider" id="RangeMax" value="100"/>
                                            <span  id="range-value-bar-Max" class="range-value-bar" ></span>
                                            
                                        </div>
                                    <asp:HiddenField runat="server" ID="HdnRangeMin" Value="0" />
                                    <asp:HiddenField runat="server" ID="HdnRangeMax" Value="30000000" />
                                        
                                    
                                </div>
                                  <div class="col-lg-12 col-12 mb-2">
                                      <label id="LblRangePrecios"></label>
                                  </div>
                                

                              </div>
                          </div>
                      </div>
                  </div>

                  <div class="card bg-opacity-white-25">
                      <div class="card-header bg-opacity-white-25 text-white" id="HeadingFilterType">
                          <a  data-toggle="collapse" data-target="#FilterCollapseType" aria-expanded="false" aria-controls="FilterCollapseType">
                             <label style="cursor:pointer;" class="HighLight-1">Tipo de Pedido</label> 
                          </a>
                      </div>
                      <div id="FilterCollapseType" class="collapse" aria-labelledby="HeadingFilterType" data-parent="FilterAccord">
                          <div class="card-body bg-opacity-white-65">
                              <div class="row mb-2">
                                <div class="col-lg-6 col-12 mb-2">
                                            <asp:CheckBoxList runat="server" ID="ChkList">
                                        <asp:ListItem Selected="True" Text="Todos" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Termopaneles" Value="'TER'"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Templados" Value="'TEM'"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                                
                              </div>
                          </div>
                      </div>
                  </div>

                  <div class="card bg-opacity-white-25">
                      <div class="card-header bg-opacity-white-25 text-white" id="HeadingFilterStatus">
                          <a  data-toggle="collapse" data-target="#FilterCollapseStatus" aria-expanded="false" aria-controls="FilterCollapseStatus">
                             <label style="cursor:pointer;" class="HighLight-1">Estado de Pedidos</label> 
                          </a>
                      </div>
                      <div id="FilterCollapseStatus" class="collapse" aria-labelledby="HeadingFilterStatus" data-parent="FilterAccord">
                          <div class="card-body bg-opacity-white-65">
                              <div class="row mb-2">
                                <div class="col-lg-6 col-12 mb-2">
                                    <asp:CheckBoxList runat="server" ID="ChkListStatus">
                                        <asp:ListItem Selected="True" Text="Todos" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Borradores" Value="'BOR'"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Ingresados" Value="'ING'"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="En Fabricación" Value="'PRG'"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="En Bodega" Value="'DIS'"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Entregados" Value="'DES'"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                                
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
              
          </div>
            <div class="modal-footer text-white">
                <asp:Button ID="BtnDescargaXls" CssClass="btn btn-primary" OnClick="BtnDescargaXls_Click" runat="server" Text="Descargar en Excel" />
                <asp:Button ID="BtnFiltering" CssClass="btn btn-success" OnClick="BtnFiltering_Click" runat="server" Text="Aplicar Filtros" />
            </div>
       </div>
  </div>
</div>

    <!--Modal ItemPedido-->
    <div class="modal fade"  id="modalItemPedido" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content bg-opacity-green-25">
          <div class="modal-header text-white"><h5><label id="MdlItemtitle"></label></h5></div>
          <div class="modal-body text-white">
              <asp:HiddenField runat="server" ID="HdnOrderSelected" />
              <asp:HiddenField runat="server" ID="HdnOldSelected" />
              <asp:HiddenField runat="server" ID="HdnTypeOrder" />
              <asp:HiddenField runat="server" ID="HdnTokenId" />
              <div class="row">
                  <div class="col mb-3"><div id="DivItemDetalles"></div></div>
              </div>

          </div>
            <div class="modal-footer text-white">
                <div class="btn-group" role="group">
                    <asp:Button runat="server" ID="MdlItemBtnEdit" OnClick="MdlItemBtnEdit_Click" CssClass="btn btn-primary" Text="Editar" />
                    <button type="button" class="btn btn-danger" runat="server" id="MdlItemBtnDelete" data-toggle="modal" data-target="#modalSeguro">Eliminar</button>
                    <button type="button" class="btn btn-danger" runat="server" id="MdlItemBtnNull" data-toggle="modal" data-target="#modalSeguro">Anular</button>
                    <asp:Button runat="server" ID="MdlItemBtnDetails" OnClick="MdlItemBtnDetails_Click" CssClass="btn btn-primary" Text="Ver Detalle" />
                </div>
            </div>
       </div>
  </div>
</div>

    <!--Modal seguro?-->
    <div class="modal fade"  id="modalSeguro" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header"><h5><label id="MdlSegurotitle"></label></h5></div>
          <div class="modal-body">
              <div class="container">
                  <div class="row"><h6><label id="MdlBodySeguro"></label></h6></div>
                  <div class="row">
                      <div class="col align-self-end">
                          <div class=" btn-group align-content-center" role="group">
                    <asp:Button runat="server" ID="BtnEliminarGo" OnClick="MdlItemBtnDelete_Click" CssClass="btn btn-danger" Text="Si" />
                    <asp:Button runat="server" ID="BtnNullingGo" CssClass="btn btn-danger" Text="Si" />
                      <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                </div>
                      </div>
                      
                  </div>
                  
              </div>
             
              
          </div>
            
       </div>
  </div>
</div>


    <script type="text/javascript">
        $(document).ready(function () {
            PageEvents();
            
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            PageEvents();
            
        });


        function PageEvents() {
            $('#<%=ChkList.ClientID%>').on('click', function () {
                
                var Todos = $('#<%=ChkList.ClientID%> input:checkbox').eq(0);
                
                Todos.on('click', function () {
                    if (Todos.prop('checked')) {
                        $('#<%=ChkList.ClientID%> input:checkbox').each(function () {
                            $(this).prop('checked', true);
                            
                        });
                
                    }
                    else {
                        $('#<%=ChkList.ClientID%> input:checkbox').each(function () {
                            $(this).prop('checked', false);

                        });
                
                    }
                });
                  
                   
            });

            $('#<%=ChkListStatus.ClientID%>').on('click', function () {
                var Todos = $('#<%=ChkListStatus.ClientID%> input:checkbox').eq(0);
                Todos.on('click', function () {
                    if (Todos.prop('checked')) {
                        $('#<%=ChkListStatus.ClientID%> input:checkbox').each(function () {
                            $(this).prop('checked',true);
                        });
                    }
                    else {
                        $('#<%=ChkListStatus.ClientID%> input:checkbox').each(function () {
                            $(this).prop('checked',false);
                        });
                    }
                });
                  
                   
            });



            $('th').hover(function () { $(this).css('cursor','pointer')});
            SortingTable();

            rangeValueHandler();
        rangeMin.addEventListener('mousedown', dragHandler);
        rangeMax.addEventListener('mousedown', dragHandler);
        
            rangeMin.addEventListener('mousemove', dragOn);
            rangeMax.addEventListener('mousemove', dragOn);
        


        rangeMin.addEventListener('mouseup', dragHandler);
        rangeMax.addEventListener('mouseup', dragHandler);

            rangeMin.addEventListener('click', rangeValueHandler);
            rangeMax.addEventListener('click', rangeValueHandler);


            $('.modal').on('show.bs.modal', function (e) {
                $('#<%=contenedor.ClientID%>').addClass('blur');
            });
            $('.modal').on('hide.bs.modal', function (e) {
                $('#<%=contenedor.ClientID%>').removeClass('blur');
            });

            $('.pedido').click(function () {
                var itemDetails = $(this).find("td").eq(9).html();
                var ID = $(this).find("td").eq(8).html();
                var TYPE = $(this).find("td").eq(2).html();
                var TOKENID = $(this).find("td").eq(11).html();
                $('#<%=HdnOrderSelected.ClientID%>').val(ID);
                $('#<%=HdnTokenId.ClientID%>').val(TOKENID);
                $('#<%=HdnTypeOrder.ClientID%>').val(TYPE);

                var Estado = $(this).find("td").eq(7).attr('id');
                console.log(Estado);

                if (Estado=='BOR') {
                    $('#<%=MdlItemBtnDetails.ClientID%>').hide();
                    $('#<%=MdlItemBtnNull.ClientID%>').hide();
                    $('#<%=BtnNullingGo.ClientID%>').hide();
                    $('#<%=MdlItemBtnDelete.ClientID%>').show();
                    $('#MdlSegurotitle').html('Eliminar');
                    $('#MdlBodySeguro').html('Seguro que desea Eliminar el Pedido ' + $(this).find("td").eq(0).html() + '?');
                    
                    $('#<%=BtnEliminarGo.ClientID%>').show();
                    $('#<%=MdlItemBtnEdit.ClientID%>').show();
                }
                else if (Estado=='ING') {
                    $('#<%=MdlItemBtnDetails.ClientID%>').show();
                    $('#<%=MdlItemBtnNull.ClientID%>').show();
                    $('#<%=BtnNullingGo.ClientID%>').show();
                    $('#MdlSegurotitle').html('Anular');
                    $('#MdlBodySeguro').html('Seguro que desea Anular el Pedido ' + $(this).find("td").eq(0).html() + '?');
                    $('#<%=MdlItemBtnDelete.ClientID%>').hide();
                    $('#<%=BtnEliminarGo.ClientID%>').hide();
                    $('#<%=MdlItemBtnEdit.ClientID%>').hide();
                }
                else {
                        $('#<%=MdlItemBtnDetails.ClientID%>').show();
                    $('#<%=MdlItemBtnNull.ClientID%>').hide();
                    $('#<%=BtnNullingGo.ClientID%>').hide();
                    $('#<%=MdlItemBtnDelete.ClientID%>').hide();
                    $('#<%=BtnEliminarGo.ClientID%>').hide();
                    $('#<%=MdlItemBtnEdit.ClientID%>').hide();
                }
                var Old = $(this).find("td").eq(10).html();
                $('#<%=HdnOldSelected.ClientID%>').val(Old);
                $('#MdlItemtitle').html($(this).find("td").eq(0).html());
                $('#DivItemDetalles').empty();
                $('#DivItemDetalles').append(itemDetails);
                $('#modalItemPedido').modal('show');

            });

             
            

            $('.pedido').hover(function () {
                $(this).css('cursor', 'pointer');
                $(this).css('text-decoration', 'underline');
                $(this).addClass('bg-opacity-dark-35 text-white');
            });
            
            $('.pedido').mouseleave(function () {
                $(this).css('text-decoration', '');
                $(this).removeClass('bg-opacity-dark-35 text-white');
            });

        }

        const rangeMin = document.querySelector('#RangeMin');
        const rangeMax = document.querySelector('#RangeMax');
        const rangeValueBar = document.querySelector('#range-value-bar-Max');
        
        

        let isDown = false;

        function dragHandler() {
            isDown = !isDown;
           
        }

        function dragOn(e) {
            if (!isDown) return;
            rangeValueHandler();
        }

        function rangeValueHandler() {
            
            var width = rangeMax.value - rangeMin.value;
            var margin = rangeMin.value;
            rangeValueBar.style.setProperty('width', width.toString() +'%');
            rangeValueBar.style.setProperty('margin-left', margin.toString() + '%');
            var Desde = rangeMin.value * 300000;
            var Hasta = rangeMax.value * 300000;
            $('#LblRangePrecios').html('Desde $' + Desde.toLocaleString() + ' Hasta $' + Hasta.toLocaleString());
            $('#<%=HdnRangeMin.ClientID%>').val(Desde);
            $('#<%=HdnRangeMax.ClientID%>').val(Hasta);
            
        }

        let SelectedFirstdate = true;

        function FuFilterDateFrom() {
            
            var DateFrom = $('#FilterDateFrom').val();
            
            $('#<%=HdnFilterDateFrom.ClientID%>').val(DateFrom);
            if (SelectedFirstdate) {
                var today = new Date();
                $('#FilterDateTo').val(DateFrom);
                $('#<%=HdnFilterDateTo.ClientID%>').val(DateFrom);
                    SelectedFirstdate = false;
            }
            
            
            
        }

        function FuFilterDateTo() {
            var DateTo = $('#FilterDateTo').val();
            $('#<%=HdnFilterDateTo.ClientID%>').val(DateTo);
        }

        function CerrarMFilter() { $('#modalfilter').modal('hide');}

        function SortingTable() {
           $('.CsCabecera th').click(function () {
                var index = $(this).index(),
                    rows = [],
                    thClass = $(this).hasClass('asc') ? 'desc' : 'asc';
                
                
                
                $(this).removeClass().addClass(thClass);
                
                $('#MainContent_OrdersTable tbody tr').each(function (index, row) {
              
                    rows.push($(row).detach());
                });
                
                rows.sort(function (a, b) {
                    var aValue = $(a).find('td').eq(index).html(),
                        bValue = $(b).find('td').eq(index).html();
                    var aV;
                    var bV;
                    var retorno;
                    if ($.isNumeric(aValue.replace('$',''))) {
                        aV = parseFloat(aValue.replace('$',''));
                        bV = parseFloat(bValue.replace('$',''));
                        retorno = aV > bV
                            ? 1 : aV < bV ? -1 : 0;
                        console.log('es 1 '+aV);
                    }
                    else {
                        retorno = aValue > bValue
                            ? 1 : aValue < bValue ? -1 : 0;
                        console.log('dos ' + aValue);
                    }
                    
                    return retorno;
                });
                
                if ($(this).hasClass('desc')) {
                    rows.reverse();
                }
                
               $.each(rows, function (index, row) {
                   $('#MainContent_OrdersTable tbody').append(row);
               });
            });
        }
            
        
        

    </script>
</asp:Content>