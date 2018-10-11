<%@ Page Title="Administracion de Rutas" Language="C#" MasterPageFile="~/View/Logistica/SiteLog.Master" AutoEventWireup="true" CodeFile="AdministracionUsuarios.aspx.cs" Inherits="_AdmRutas" %>

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
         .panel-usuarios > .panel-heading{
             color: white;;
             background-color:rgba(91, 41, 216, 0.90);
             border-color:rgba(91, 41, 216, 0.90);
             
         }
         .panel-usuarios {
             border-color:rgba(91, 41, 216, 0.90);
             
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
                <h1><asp:Label runat="server" ForeColor="Blue">Administración de Usuarios</asp:Label></h1>
                <p></p>

                <h2><asp:Label runat="server" ID="LblInventName"></asp:Label></h2>
            </div>

            <div  id="accordion">
                <!-- Asignacion-->
                  <div class="card card-border-naranjo mb-2">
                            <div class="card-header card-bg-naranjo" id="headerVariable" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseTres">
                                <h5><i  class="fas fa-chevron-down">Asignar Usuarios</i></h5>
                                    
                            </div>
                                    <div id="CollapseTres" class="collapse">
                                            <div class="card-body">
                                                <asp:UpdatePanel runat="server" ID="UpdateDDlistbodega" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="form-inline">
                                                            <asp:Label runat="server" Text="Filtrar por bodega : "></asp:Label>
                                                            <asp:DropDownList OnSelectedIndexChanged="DDListUsuario_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server" ID="DDListUsuario"></asp:DropDownList>
                                                        </div>
                                                        <p></p>
                                                        <div class="text-center">
                                                            <asp:Label runat="server" ID="LblGrdRutas" ForeColor="Red" Visible="false" Font-Size="XX-Large"></asp:Label>
                                                        </div>
                                                            
                                                            <div class="table table-responsive">
                                                                    <asp:GridView runat="server" ID="GridAsingRutas" DataKeyNames="IdAsign" CssClass="table" AutoGenerateColumns="False" GridLines="Horizontal" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3">
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
                                                            <asp:LinkButton ID="LinkToAsignRuta" runat="server" OnClick="LinkToAsignRuta_Click" Text="Asignar Ruta al Inventario"></asp:LinkButton>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="text-center">
                                                                <asp:Button ID="BtnAsigRutas" runat="server" CssClass="btn btn-success btn-lg" Text="Asignar" OnClick="BtnAsigRutas_Click" />
                                                            </div>
                                                        </div>
                                                        
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="DDListUsuario" EventName="SelectedIndexChanged" />
                                                        <asp:PostBackTrigger ControlID="BtnAsigRutas" />
                                                        <asp:PostBackTrigger ControlID="LinkToAsignRuta" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                 
                                            </div>
                                    </div>
                
                     </div>
                <!-- Rutas Asignadas-->
                  <div class="card card-border-naranjo mb-2">
                            <div class="card-header card-bg-naranjo" id="headerAsign" data-toggle="collapse" data-parent="#accordion" data-target="Collapsedos">
                                <h5><i  class="fas fa-chevron-down">Usuarios Asignados</i></h5>
                                   
                            </div>
                                    <div id="Collapsedos" class="collapse show">
                                            <div class="card-body">
                                                    <div class="row mb-3">
                                                        <div class="col-12">
                                                            <h3>Rutas Asignadas</h3>
                                                        </div>
                                                        <div class="col-12">
                                                            <div class ="table table-responsive">
                                                            <div id="TablaRutas"></div>
                                                        </div>
                                                        </div>
                                                        
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
                <asp:HiddenField runat="server" ID="HdnIdAsignUser" />
                
                <div class="row bodyDiv">
                    <div class="col-lg-12 col-xs-12">
                        <h3> <asp:Label runat="server" ID="LblDetalle"></asp:Label></h3>
                    </div>
                </div>
               
              
               
                
            </div> 
            <div class="modal-footer">
                 <div class="bodyModal">
                         <asp:Button runat="server" ID="BtnModalDeleteuseronRuta" OnClientClick="Confirm('Eliminar')" OnClick="BtnModalDeleteUseronRuta_Click"  CssClass="btn btn-danger" Text="Eliminarlo de esta Ruta" /><span> </span>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>  
               </div>
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>
        

   
     
     <script type="text/javascript">
         $(document).ready(function () {
                 var IDINV = $('#<%=HdnIdInventario.ClientID %>').val();
                 LoadRutas(IDINV);
         });

          function Confirm(Tipo) {
                 var confirm_value = document.createElement("INPUT");

                 var stringg = "";
                 if (Tipo=="Eliminar") {
                     stringg='Está seguro que desea eliminar este usuario?';
                 }
                
                 confirm_value.type = "hidden";
                 confirm_value.name = "confirm_value";
                 if (confirm(stringg)) {
                     confirm_value.value = "si";
                 }
                 else {
                     confirm_value.value = "no";
                 }
                 document.forms[0].appendChild(confirm_value);
             }

            
             function LoadRutas(IdInventario) {
                  var datafrom = "{'Usuario':'ALL','ID_Inventory':'" + IdInventario + "'}";

              
                               


               $('#TablaRutas').html('');
                $.ajax({
                    type: 'post',
                    url: '../ServiciosWeb/WSlogistica.asmx/GetListRutasUserInv',
                    data: datafrom,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (msg) {

                            var table = '<table id="TablaP" class="table table-hover">';
                                table += '<strong><thead ><tr class="text-uppercase text-info text-center">'
                                    + '<th class="cabeza">Usuario</th>'
                                    + '<th class="cabeza">Ruta</th>'
                                    + '<th class="cabeza">Descripción Ruta</th>'
                                    + '<th class="cabeza">Bodega</th>'
                                    
                                    
                                    + '</tr></thead></strong><tbody>';
                            $.each(msg.d, function () {
                                var Id = this._ID;
                                var StatusClass;
                               
                                if (this._Status==='Activo') {
                                    StatusClass = "StatusClassAct";
                                }
                                
                                table = table + '<tr class="filas">'+
                                    '<td>' + this._UserName + '</td>' +
                                    '<td> ' + this._RutaName + '</td> ' +
                                    '<td> ' + this._DescripRuta + '</td> ' +
                                    '<td>' + this._Bodega + '</td>' +
                                    
                                    '<td style="display:none;">' + this._Id_Asign_user + '</td>';
               
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
                                var usuario = $(this).find("td").eq(0).html();
                                var ruta = $(this).find("td").eq(1).html();
                                var bodega =$(this).find("td").eq(3).html();
                                var detalle = "El usuario " + usuario + "tiene asignada la ruta " + ruta + " en la bodega " + bodega;
                            
                             
                                $('#<%=HdnIdAsignUser.ClientID %>').val(_ID);
                                $('#<%=LblDetalle.ClientID%>').html(detalle);

                                
                                
                            
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