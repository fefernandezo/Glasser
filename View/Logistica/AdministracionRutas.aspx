<%@ Page Title="Administracion de Rutas" Language="C#" MasterPageFile="~/View/Logistica/SiteLog.Master" AutoEventWireup="true" CodeFile="AdministracionRutas.aspx.cs" Inherits="_AdmRutas" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        .headtable
        {
            background-color: rgba(29, 182, 65, 0.85);
            color: white;
           
            
        }
        .bodyDiv{
            padding-left:15px;
            padding-right:15px;
            padding-top:15px;
            padding-bottom:15px;
        }
        .item{
             padding-left:8px;
             padding-right:8px;
         }
         .headercol{
             margin-left:auto;
             margin-right:auto;
         }
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <asp:Panel ID="PanelGral" runat="server" Visible="false">
        <asp:HiddenField ID="HdnIdInventario" runat="server" />
        <asp:HiddenField ID="HdnKOSU" runat="server" />
        <div class="container">
            <div class="row text-center">
                <h1><asp:Label runat="server" ForeColor="Blue">Administración de Rutas</asp:Label></h1>
                <p></p>
                <h2><asp:Label runat="server" ID="LblInventName"></asp:Label></h2>
            </div>
            <div id="accordion">
                <!-- Asignacion-->
                  <div class="card">
                            <div class="card-header">
                                    <h3 class="card-title">
                                        <a data-toggle="collapse" data-parent="#accordion" href="#CollapseTres" class="glyphicon glyphicon-chevron-down"> Asignar rutas </a>
                                    </h3>
                            </div>
                                    <div id="CollapseTres" class="collapse">
                                            <div class="card-body">
                                                <asp:UpdatePanel runat="server" ID="UpdateDDlistbodega" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="form-inline">
                                                            <asp:Label runat="server" Text="Filtrar por bodega : "></asp:Label>
                                                            <asp:DropDownList OnSelectedIndexChanged="DDListBodega_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server" ID="DDListBodega"></asp:DropDownList>
                                                        </div>
                                                        <p></p>
                                                        <div class="text-center">
                                                            <asp:Label runat="server" ID="LblGrdRutas" ForeColor="Red" Visible="false" Font-Size="XX-Large"></asp:Label>
                                                        </div>
                                                            
                                                            <div class="table table-responsive">
                                                                    <asp:GridView runat="server" ID="GridAsingRutas" DataKeyNames="_Id" CssClass="table" AutoGenerateColumns="False" GridLines="Horizontal" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                                        <AlternatingRowStyle BackColor="#F7F7F7" />
                                                                        
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="CheckBAsigRutas" runat="server"  />
                                                                                    
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="text-center" />
                                                                            </asp:TemplateField>
                                                                           
                                                                            <asp:BoundField DataField="_Nombre" HeaderText="Ruta" >
                                                                                <ItemStyle CssClass="item" />
                                                                                <HeaderStyle CssClass="headercol" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="_Descripcion" HeaderText="Descripción" >
                                                                                <ItemStyle CssClass="item" />
                                                                                <HeaderStyle CssClass="headercol" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        
                                                                        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                        <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                                                        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                        <SortedAscendingCellStyle BackColor="#F4F4FD" />
                                                                        <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                                                                        <SortedDescendingCellStyle BackColor="#D8D8F0" />
                                                                        <SortedDescendingHeaderStyle BackColor="#3E3277" />
                                                                        
                                                                    </asp:GridView>
                                                            </div>
                                                        <div class="col-lg-6">
                                                            <asp:LinkButton ID="LinkToCrearRuta" runat="server" OnClick="LinkToCrearRuta_Click" Text="Crear una ruta"></asp:LinkButton>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="text-center">
                                                                <asp:Button ID="BtnAsigRutas" runat="server" CssClass="btn btn-success btn-lg" Text="Asignar" OnClick="BtnAsigRutas_Click" />
                                                            </div>
                                                        </div>
                                                        
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="DDListBodega" EventName="SelectedIndexChanged" />
                                                        <asp:PostBackTrigger ControlID="BtnAsigRutas" />
                                                        <asp:PostBackTrigger ControlID="LinkToCrearRuta" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                                                
                                                
                  
                                                
                 
                                            </div>
                                    </div>
                
                     </div>
                <!-- Rutas Asignadas-->
                  <div class="card">
                            <div class=" card-header">
                                    <h3 class=" card-title">
                                        <a data-toggle="collapse" data-parent="#accordion" href="#Collapsedos" class="glyphicon glyphicon-chevron-down"> Rutas Asignadas al inventario </a>
                                    </h3>
                            </div>
                                    <div id="Collapsedos" class="collapse show">
                                            <div class="card-body">
                                                    
                                                        <div class="row text-center">
                                                            <h3>Rutas Asignadas</h3>
                                                        </div>
                                                        <div class ="table table-responsive">
                                                            <div id="TablaRutas"></div>
                                                        </div>
                                                    
                                            </div>
                                    </div>
                
                     </div>
                     
            </div>

            

            
            
                 
                
        </div>
    </asp:Panel>

    <!--MODAL RUTA ITEM-->

    <div class="modal fade" id="modalrutaSelected" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <div class=" col-lg-6 text-left">
                        <h3 class="modal-title" id="gridSystemModalLabel"> <asp:Label runat="server" ID="LblNombreRuta"></asp:Label> </h3>
                    </div>
              
              
          </div>
            <div class="modal-body">
                <asp:HiddenField runat="server" ID="HdnIdAsignruta" />
                <asp:HiddenField runat="server" ID="HdnNameruta" />
                <div class="row bodyDiv">
                    <div class="col-lg-12 col-xs-12">
                        <h3><asp:Label runat="server" Text="Descripción :" Font-Bold="true"></asp:Label> <asp:Label runat="server" ID="LblDetalle"></asp:Label></h3>
                    </div>
                </div>
                <div class="row bodyDiv">
                    <div class="col-lg-12 col-xs-12 text-center">
                        <asp:Button runat="server" ID="BtnDetailRuta" OnClick="BtnDetailRuta_Click" CssClass="btn btn-primary  btn-lg" Text="Ver Detalle" /><span> </span>      
                   </div>
                </div>
              
               
                
            </div> 
            <div class="modal-footer">
                 <div class="row bodyModal">
                         <asp:Button runat="server" ID="BtnModalDeleteRuta" OnClick="BtnModalDeleteRuta_Click"  CssClass="btn btn-danger" Text="Eliminar Ruta" /><span> </span>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>  
               </div>
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>
        

    <!--MODAL Eliminar RUTA-->
    <div class="modal fade"  id="ModalMsg" tabindex="-1" role="dialog" aria-labelledby="TitleMsg">
      <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title" id="TitleMsg">Mensaje</h4>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                    <div class="row">
                            <h4><asp:Label runat="server" ID="Mdl_LblMsg"></asp:Label></h4>
                   </div>
              </div>
          </div>
          <div class="modal-footer">
              
                  <asp:Button CssClass="btn btn-primary" ID="MdlBtnEliminar" OnClick="BtnDeleteRuta_Click" runat="server" Text="SI" /><span> </span>
                  <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
              
              
          </div>
          
       </div>
  </div>
</div>
     
     <script type="text/javascript">
         $(document).ready(function () {
                 var IDINV = $('#<%=HdnIdInventario.ClientID %>').val();
                 LoadRutas(IDINV);
         });

            
             function LoadRutas(IdInventario) {
                  var datafrom = "{'IdInventario':'" + IdInventario + "'}";

                 var protocol = window.location.protocol;
                 var host = window.location.host;
                 var path = '/ServiciosWeb/WSlogistica.asmx/RutasXInventario';
                 var urldata = protocol + '//' + host + path;
                 
               $('#TablaRutas').html('');
                $.ajax({
                    type: 'post',
                    url: urldata,
                    data: datafrom,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (msg) {

                            var table = '<table id="TablaP" class="table table-hover">';
                                table += '<strong><thead ><tr class="text-uppercase text-info text-center">'
                                    + '<th class="cabeza"  id="hea1">Ruta</th>'
                                    + '<th class="cabeza" id="hea4">Descripción</th>'
                                    + '<th class="cabeza" id="hea2">Bodega</th>'
                                    + '<th class="cabeza" id="hea3">Sucursal</th>'
                                    + '</tr></thead></strong><tbody>';
                            $.each(msg.d, function () {
                                
                                
                               
                               
                                
                                table = table + '<tr class="filas">'+
                                    '<td> ' + this._Nombre + '</td> ' +
                                    '<td>' + this._Descripcion + '</td>' +
                                    '<td>' + this._CodBodega + '</td>' +
                                    '<td>' + this._CodSUCU + '</td>' +
                                    '<td style="display:none;">' + this.IdAsign + '</td>';
               
                            });
                                table = table + '</tbody></table>';

                            $('#TablaRutas').append(table);
                           
                            $('.cabeza').hover(function () {
                                $(this).css('cursor', 'pointer');
                                $(this).css('text-decoration', 'underline');
                            });
                            $('.cabeza').mouseleave(function () {
                                $(this).css('text-decoration', '');
                            });

                            $('#hea2').click(function () {
                                $(this).css('cursor', 'pointer');
                        });
                            $('.filas').hover(function () {
                                $(this).css('cursor', 'pointer');
                                $(this).css('text-decoration', 'underline');
                        });
                        $('.filas').mouseleave(function () {
                                $(this).css('text-decoration', '');
                            });

                            $('.filas').click(function () {
                                var _ID = $(this).find("td").eq(4).html();
                                var _Nombre = $(this).find("td").eq(0).html();
                                var _Descripcion=$(this).find("td").eq(1).html();
                             
                                $('#<%=HdnIdAsignruta.ClientID %>').val(_ID);
                                $('#<%=HdnNameruta.ClientID %>').val(_Nombre);
                                $('#<%=LblNombreRuta.ClientID %>').html(_Descripcion);
                                $('#<%=LblDetalle.ClientID %>').html(_Descripcion);
                                $("#modalrutaSelected").modal("show");

                            });

                                   
                            ///sorting
                             $('th').click(function () {
                            var table = $(this).parents('table').eq(0)
                            var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))
                            this.asc = !this.asc
                            if (!this.asc) {
                                rows = rows.reverse()
                            }
                            for (var i = 0; i < rows.length; i++) {
                                table.append(rows[i])
                            }
                            setIcon($(this), this.asc);
                        });

                            function comparer(index) {
                                return function (a, b) {
                                    var valA = getCellValue(a, index),
                                    valB = getCellValue(b, index)
                                    return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.localeCompare(valB)
                                }
                            }

                            function getCellValue(row, index) {
                                return $(row).children('td').eq(index).html()
                            }

                            function setIcon(element, asc) {
                                $("th").each(function (index) {
                                    $(this).removeClass("sorting");
                                    $(this).removeClass("asc");
                                    $(this).removeClass("desc");

                                });
                                element.addClass("sorting");
                                if (asc) element.addClass("asc");
                                else element.addClass("desc");
                            }

                           

                        

                        


            



                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert('Error :=> ' + textStatus + '--- ' + errorThrown);
                    }

                });
             }
         </script>
</asp:Content>