<%@ Page Title="Administracion de Inventarios" Language="C#"  MasterPageFile="~/View/Logistica/SiteLog.Master" AutoEventWireup="true" CodeFile="AdministracionInventario.aspx.cs" Inherits="_AdmInventario" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        .headtable
        {
            background-color: rgba(29, 182, 65, 0.85);
            color: white;
           
            
        }

        .item{
             padding-left:8px;
             padding-right:8px;
         }
        .headercol{
             margin-left:auto;
             margin-right:auto;
         }
        .StatusClassAct{
            background-color:green;
            color:white;
        }
        .statusClassCerr{

        }
        .filas{
            
            vertical-align:middle;
        }
        .bodyModal{
            padding-left:15px;
            padding-right:15px;
            padding-top:15px;
            padding-bottom:15px;
        }
        .btn-user{

        }
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
     <div class="container">
        


             <div id="accordion">
                 <!-- CREACION DE INVENTARIOS-->
                     <div class="card">
                            <div class="card-header">
                               <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseFin" class="glyphicon glyphicon-chevron-down"> Creación de Inventarios </a></h3>    
                            </div>

                                    <div id="CollapseFin" class="collapse">
                                            <div class="card-body">
                                                <asp:UpdatePanel runat="server" ID="UpdateCreacion" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row bodyModal">
                                                            <div class="col-lg-6 col-sm-12">
                                                                <div class="form-inline">
                                                                    <asp:Label runat="server" Text="Nombre*: "></asp:Label><asp:TextBox runat="server" CssClass="form-control" ID="TxtNombreInv"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredTxtNombreInv" runat="server" ErrorMessage="Debe Ingresar el nombre del Inventario" ForeColor="Red" ValidationGroup="Creacion" ControlToValidate="TxtNombreInv"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-sm-12">
                                                                <div class="form-inline">
                                                                    <asp:Label runat="server" Text="Sucursal: "></asp:Label><asp:DropDownList runat="server" ID="DDListSucursal" CssClass="form-control"></asp:DropDownList><asp:Label runat="server" ID="ErrorDDListSucursal" Visible="false" ForeColor="Red"></asp:Label>
                                                                </div>
                                                            </div>
                                                    
                                                        </div>

                                                        <div class="row bodyModal">
                                                            <div class="col-lg-6 col-sm-12">
                                                                <div class="form-inline">
                                                                    <asp:Label runat="server" Text="Inicio*: "></asp:Label><asp:TextBox runat="server" placeholder="dd-mm-yyyy" CssClass="form-control" ID="TxtStartDate"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredStartDate" runat="server" ErrorMessage="debe ingresar una fecha de Inicio" ForeColor="Red" ValidationGroup="Creacion" ControlToValidate="TxtStartDate"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6 col-sm-12">
                                                                <div class="form-inline">
                                                                    <asp:Label runat="server" Text="Término: "></asp:Label><asp:TextBox runat="server" placeholder="dd-mm-yyyy" CssClass="form-control" ID="TxtEndDate"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="ErrorEndDate" Visible="false" ForeColor="Red"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row bodyModal">
                                                            <div class=" form-group">
                                                                    <asp:Label runat="server" Text="Descripción*:"></asp:Label><asp:TextBox runat="server" CssClass="form-control" ID="TxtDescripInv" TextMode="MultiLine"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredDescripcion" runat="server" ErrorMessage="Debe ingresar una descripción para el Inventario" ForeColor="Red" ControlToValidate="TxtDescripInv" ValidationGroup="Creacion"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="row bodyModal">
                                                            <div class="form-group text-right">
                                                                <asp:Button runat="server" ID="BtnIngresarInvent" ValidationGroup="Creacion" Text="Siguiente" OnClick="BtnIngresarInvent_Click" CssClass="btn btn-success btn-lg" />
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnIngresarInvent" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                   
                 *Campos requeridos
                                            </div>
                                    </div>
                
                     </div>
                 <!-- INVENTARIOS ACTIVOS-->
                  <div class="card">
                            <div class="card-header">
                               <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseDos" class="glyphicon glyphicon-chevron-down"> Inventarios </a></h3>    
                            </div>
                                    <div id="CollapseDos" class="collapse show">
                                            <div class="card-body">
                                                            <div class ="table table-responsive">
                                                                  <div id="TablaInventarios"></div>
                                                            </div>
                                            </div>
                                    </div>
                
                  </div>
                 
             </div>

                  
     </div>
      <!--MODAL Inventario Item-->

    <div class="modal fade" id="modalInventario" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
                        <h3 class="modal-title" id="gridSystemModalLabel"> <asp:Label runat="server" ID="NombreInv"></asp:Label> </h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                <asp:HiddenField runat="server" ID="HdnIdInv" />
                <asp:HiddenField runat="server" ID="HdnNameInv" />
                <div class="row bodyModal">
                    <div class="col-lg-12 col-xs-12">
                        <h3><asp:Label runat="server" Text="Descripción :" Font-Bold="true"></asp:Label> <asp:Label runat="server" ID="LblDetalle"></asp:Label></h3>
                    </div>
                </div>
                <div class="row bodyModal">
                    <div class="col-lg-12 col-xs-12 text-center">
                        <p></p>
                        <asp:Button runat="server" ID="BtnDetail" OnClick="BtnDetail_Click" CssClass="btn btn-primary  btn-lg" Text="Ver Detalle" />
                        <p></p>
                   </div>
                    
                    <div class="col-lg-12 col-xs-12 text-center">
                        <p></p>
                        <asp:Button runat="server" ID="BtnAdmRutas" OnClick="BtnAdmRutas_Click" CssClass="btn btn-success  btn-lg" Text="Administrar Rutas" />     
                        <p></p>
                   </div>

                    <div class="col-lg-12 col-xs-12 text-center">
                        <p></p>
                        <asp:Button runat="server" ID="BtnAdmUsers" OnClick="BtnAdmUsers_Click"  CssClass="btn btn-user  btn-lg" Text="Administrar Usuarios" />     
                        <p></p>
                   </div>
                    
                </div>
              
               
                
            </div> 
            <div class="modal-footer">
                <div class="btn-group btn-group-justified" role="group" aria-label="...">
                    <div class="btn-group" role="group">
                        
                        <asp:Button runat="server" ID="BtnCerrar" OnClick="BtnCerrar_Click" OnClientClick="Confirm('Cerrar')" CssClass="btn btn-info" Text="Cerrar Inventario" />
                        <asp:Button runat="server" ID="BtnActivar" OnClick="BtnActivar_Click" OnClientClick="Confirm('Activar')" CssClass="btn btn-warning" Text="Activar Inventario" />
                    </div>

                    <div class="btn-group" role="group">
                        <asp:Button runat="server" ID="BtnEliminar" OnClick="BtnEliminar_Click" OnClientClick="Confirm('Eliminar')" CssClass="btn btn-danger" Text="Eliminar Inventario" />
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
                 LoadInventarios("todos");
             });

             function Eliminar(ID, Nombre) {
                
             }

             function Confirm(Tipo) {
                 var confirm_value = document.createElement("INPUT");

                 var stringg = "";
                 if (Tipo=="Eliminar") {
                     stringg='Está seguro que desea eliminar el inventario "' +  $('#<%=HdnNameInv.ClientID %>').val() + '" ?';
                 }
                 if (Tipo=="Cerrar") {
                     stringg='Está seguro que desea cerrar el inventario "' +  $('#<%=HdnNameInv.ClientID %>').val() + '" ?';
                 }
                 if (Tipo=="Activar") {
                     stringg='Está seguro que desea activar el inventario "' +  $('#<%=HdnNameInv.ClientID %>').val() + '" ?';
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

             function LoadInventarios(tipo) {

                 var datafrom;
                 if (tipo === "todos") {

                     datafrom = "{'status':'Todos'}";

                 }

                 else if (tipo === "Activos") {

                     datafrom = "{'status': '0'}";

                 }

                 else if (tipo === "Cerrados") {

                     datafrom = "{'status': '1'}";

                 }

                 


               $('#TablaInventarios').html('');
                $.ajax({
                    type: 'post',
                    url: '../ServiciosWeb/WSlogistica.asmx/AllInventories',
                    data: datafrom,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (msg) {

                            var table = '<table id="TablaP" class="table table-hover">';
                                table += '<strong><thead ><tr class="text-uppercase text-info text-center">'
                                    + '<th class="cabeza"  id="hea1">Inventario</th>'
                                    + '<th class="cabeza" id="hea0">Sucursal</th>'
                                    + '<th class="cabeza" id="hea2">Descripción</th>'
                                    + '<th class="cabeza" id="hea3">Fecha Inicio</th>'
                                    + '<th class="cabeza" id="hea4">Fecha Término</th>'
                                    + '<th class="cabeza" id="hea5">Estado</th>'
                                        + '</tr></thead></strong><tbody>';
                            $.each(msg.d, function () {
                                var Id = this._ID;
                                var StatusClass;
                               
                                if (this._Status==='Activo') {
                                    StatusClass = "StatusClassAct";
                                }
                                
                                table = table + '<tr class="filas">'+
                                    '<td> ' + this._Nombre + '</td> ' +
                                    '<td>' + this._Sucursal + '</td>' +
                                    '<td>' + this._Descripcion + '</td>' +
                                    '<td>' + this._Inicio + '</td>' +
                                    '<td>' + this._Termino + '</td>' +
                                    '<td class="' + StatusClass + '">' + this._Status + '</td>'+
                                    '<td style="display:none;">' + this._ID + '</td>';
               
                            });
                                table = table + '</tbody></table>';

                            $('#TablaInventarios').append(table);
                           
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
                                
                                $(this).css('text-decoration', 'underline');
                        });
                        $('.filas').mouseleave(function () {
                                $(this).css('text-decoration', '');
                            });


                        $('.filas').click(function () {
                            var _ID = $(this).find("td").eq(6).html();
                            var _Nombre = $(this).find("td").eq(0).html();
                            var _Descripcion = $(this).find("td").eq(2).html();
                            var _Estado = $(this).find("td").eq(5).html();
                             $('#<%=NombreInv.ClientID %>').html(_Nombre);
                            $('#<%=HdnIdInv.ClientID %>').val(_ID);
                            $('#<%=HdnNameInv.ClientID %>').val(_Nombre);
                            $('#<%=LblDetalle.ClientID %>').html(_Descripcion);
                            if (_Estado == 'Cerrado') {
                                $('#<%=BtnCerrar.ClientID%>').attr("style", "display:none");
                                $('#<%=BtnActivar.ClientID%>').attr("style", "display:block");
                                $('#<%=BtnAdmRutas.ClientID%>').attr("disabled", true);
                                $('#<%=BtnAdmUsers.ClientID%>').attr("disabled", true);
                                $('#<%=BtnEliminar.ClientID%>').attr("disabled", true);
                            }
                            else {
                                $('#<%=BtnActivar.ClientID%>').attr("style", "display:none");
                                $('#<%=BtnCerrar.ClientID%>').attr("style", "display:block");
                                 $('#<%=BtnAdmRutas.ClientID%>').attr("disabled", false);
                                $('#<%=BtnAdmUsers.ClientID%>').attr("disabled", false);
                                $('#<%=BtnEliminar.ClientID%>').attr("disabled", false);

                            }
                            $("#modalInventario").modal("show");

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

                            ///sorting
                            $("[id*='TablaInventarios'] tr:has(td)").hover(function () {

                                $(this).css("cursor", "pointer");

                            });

                        

                        


            



                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert('Error :=> ' + textStatus + '--- ' + errorThrown);
                    }

                });
             }
         </script>
</asp:Content>