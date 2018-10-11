<%@ Page Title="Administracion de Rutas" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="AdministracionVariables.aspx.cs" Inherits="_AdmVariablesProd" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
         .tabla-bg-naranjo {
    border-color: #ff6a00;
}
         .tabla-bg-naranjo td {
             text-align:center;
             border-color: #ff6a00;

         }
         .tabla-bg-naranjo th {
             background-color: #ff6a00;
             border-color: #ff6a00;
             color: white;
         }
    
    .tabla-bg-uva th {
             background-color: #6F2DA8;
             border-color: #6F2DA8;
             color: white;
         }
    .tabla-bg-uva {
    border-color: #6F2DA8;
}
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
  <div class="container-fluid">
      <div class="row justify-content-center">
          <div id="accordion">
              <asp:HiddenField runat="server" ID="HdnId" />
              <div class="card card-border-naranjo mb-2">
                <div class="card-header card-bg-naranjo" id="headerVariable" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseVariable">
                    <h5><i  class="fas fa-chevron-down"><asp:Label runat="server" ID="LblVariable" Text="titulo"></asp:Label></i></h5>
                </div>
                <div id="CollapseVariable" class="collapse show">
                    <div class="card-body">
                        <div class="row justify-content-end">
                            <div class="col text-right">
                                <asp:LinkButton runat="server" ID="LinktoCrearVar" OnClick="LinktoCrearVar_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col">
                                <label id="label1"></label>
                               <div class="table-responsive" id="DivTablaVariable"></div>
                                

                            </div>
                           
                            
                            
                        </div>
                        
                        
                    
                    </div>
                </div>
            </div>
              <asp:Panel runat="server" ID="Card2" Visible="true">
                  <div class="card card-border-uva mb-3">
                <div class="card-header card-bg-uva" id="headerInfPed" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseInfoPed">
                    <h5><i  class="fas fa-chevron-down"><asp:Label runat="server" ID="Lbltitle" Text="titulo"></asp:Label></i></h5>
                </div>
                <div id="CollapseInfoPed" class="collapse">
                    <div class="card-body">
                        <div class="row mb-2">
                            <div class="col">
                                <div class="form-inline">
                                    <h6><asp:Label runat="server" ID="LblDDList"></asp:Label></h6>
                                    <asp:DropDownList runat="server" OnSelectedIndexChanged="DDlistVariables_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" ID="DDlistVariables"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server" ID="UpPanelAsignacion" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="PanelAsignacion" Visible="false">
                                    <div class="card card-border-uva mb-3">
                                <div class="card-header card-bg-uva" id="headerxasign" data-toggle="collapse" data-parent="#accordion" data-target="#Collapsexasign">
                                    <h5><i  class="fas fa-chevron-down"><asp:Label runat="server" ID="Lbltitle2" Text="titulo"></asp:Label></i></h5>
                                </div>
                                <div id="Collapsexasign" class="collapse show">
                                    <div class="card-body">
                                        <asp:HiddenField runat="server" ID="HdnListAsign" />
                                        
                                        <div class="row mb-2">
                                            <div class="col-12">
                                                <asp:GridView runat="server" ID="GridxAsignar" DataKeyNames="ID" CssClass="table" AutoGenerateColumns="False" GridLines="Horizontal" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBAsigacion" runat="server"  />

                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="text-center" />

                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="_Name" HeaderText="Nombre" >

                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="_Descrition" HeaderText="Descripción" >

                                                        </asp:BoundField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div class="col-12 text-center">
                                                <asp:Button runat="server" ID="BtnAsignacion" Text="Asignar" OnClick="BtnAsignacion_Click" CssClass="boton outline-uva" />
                                            </div>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card card-border-uva mb-3">
                                <div class="card-header card-bg-uva" id="headerasigandos" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseAsignados">
                                    <h5><i  class="fas fa-chevron-down"><asp:Label runat="server" ID="Lbltitleasignadas" Text="titulo"></asp:Label></i></h5>
                                </div>
                                <div id="CollapseAsignados" class="collapse show">
                                    <div class="card-body">
                                        <asp:HiddenField runat="server" ID="HiddenField1" />
                                        <div class="row mb-2">
                                            <div class="col">
                                                <div class="table-responsive" id="DivTablaAsignados"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                </asp:Panel>
                                

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="DDlistVariables" EventName="SelectedIndexChanged" />
                                <asp:PostBackTrigger ControlID="BtnAsignacion" />
                            </Triggers>
                                                    </asp:UpdatePanel>

                        
                        
                        
                    
                    </div>
                </div>
            </div>
              </asp:Panel>
              
              
          </div>
      </div>
  </div>

    <!--Modal Edicion de variable-->
    <div class="modal fade"  id="modalVarDetail" tabindex="-1" role="dialog" aria-labelledby="TitleMsg">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h3><asp:Label runat="server" ID="LblModalEdName"></asp:Label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                    <asp:HiddenField runat="server" ID="HdnIdItem" />
                  <div class="row">
                      <div class="col-12">
                          <h4>Descripción:</h4>
                      </div>
                      <div class="col-12">
                          <h4><asp:Label runat="server" ID="LblModalEdDescription"></asp:Label></h4>
                      </div>
                  </div>

              </div>
          </div>
          <div class="modal-footer">
              <div class="btn-group">
                  <asp:Button CssClass="btn btn-primary" ID="BtnVarDetaileditar" runat="server" Text="EDITAR" />
                  <button type="button" class="btn btn-danger" id="MOpenModalEliminar" data-toggle="modal" data-target="#modalMsj">ELIMINAR</button>
              </div>
                  <span> </span>
                  <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>
              
              
          </div>
          
       </div>
  </div>
</div>

    <!--Modal Mensaje Eliminar-->
    <div class="modal fade"  id="modalMsj" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Mensaje</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="container-fluid">
                    
                  <div class="row">
                      <div class="col-12">
                    <h4><label id="LblMsj"></label></h4>
                      </div>
                    
                  </div>

              </div>
          </div>
          <div class="modal-footer">
                  <asp:Button CssClass="btn btn-danger" ID="BtnVarDetailEliminar" OnClick="BtnVarDetailEliminar_Click" runat="server" Text="SI" />
                  <span> </span>
                  <button type="button" class="btn btn-default" data-dismiss="modal">NO</button>
              
              
          </div>
          
       </div>
  </div>
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            var ID = $('#<%=HdnId.ClientID%>').val();
            GetVarDetail(ID);
            
             });



        function GetAsignacion(TypeOfasign,IDfirst) {

            $('#DivTablaAsignados').html('');

            var protocol = window.location.protocol;
                 var host = window.location.host;
                 var path = '/ServiciosWeb/WSComercial.asmx/GetListVarAsignadas';
                 var urldata = protocol + '//' + host + path;

            var datafrom = "{'TypeOfasign':'"+TypeOfasign+"','IDfirst':'"+IDfirst+"'}";
            $.ajax({
                type: 'post',
                url: urldata,
                data: datafrom,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {

                    var table = '<table id="TablaAsign" class="table tabla-bg-uva">';
                    table += '<strong><thead class="bg-primary" ><tr class="text-uppercase text-white text-center">'
                        + '<th class="cabeza" id="hea1"><%=LblDDList.Text%></th>'
                        + '<th class="cabeza" id="hea2"><%=Lbltitleasignadas.Text%></th>'
                        + '<th class="cabeza" id="hea3">Fecha Asignación</th>'
                        
                
                        + '</tr></thead></strong><tbody>';
                    $.each(msg.d, function () {
                        
                        table = table + '<tr class="filas"><td>' + this._NameVarA + '</td>';
                        table = table + '<td>' + this. _NameVarB + '</td>'+
                                        '<td>'+ this._AsignDate + '</td>' ;
                        table = table + '<td style="display:none;">' + this._ID + '</td>';
                       
                       
                    });

                    table = table + '</tbody></table>';

                    $('#DivTablaAsignados').append(table);
                   
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    alert('Error :=> ' + textStatus + '--- ' + errorThrown);
                }
            });
        }

         function GetVarDetail(TypeId) {

             $('#DivTablaVariable').html('');
             

            var protocol = window.location.protocol;
                 var host = window.location.host;
                 var path = '/ServiciosWeb/WSComercial.asmx/GetListVariables';
                 var urldata = protocol + '//' + host + path;
             
            var datafrom = "{'TypeofVar':'" + TypeId + "'}";
            $.ajax({
                type: 'post',
                url: urldata,
                data: datafrom,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    
                    var table = '<table id="Tablavariable" class="table tabla-bg-naranjo">';
                    table += '<strong><thead ><tr class="text-uppercase text-center">'
                        + '<th class="cabeza" id="hea1">Nombre</th>'
                        + '<th class="cabeza" id="hea2">Descripción</th>';
                    if (TypeId==4) {
                        table += '<th class="cabeza" id="hea3">Procedencia</th>';
                    }
                    
                
                        table += '</tr></thead></strong><tbody>';
                    $.each(msg.d, function () {
                        
                        table = table + '<tr class="filasVar"><td>' + this._Name + '</td>'
                            + '<td>' + this._Descrition + '</td>';
                        
                        if (TypeId==4) {
                            table = table + '<td>' + this._Procedencia + '</td>';
                        }
                        
                               
                        table = table + '<td style="display:none;">' + this.ID + '</td>';
                       
                    });
                    
                    table = table + '</tbody></table>';

                    $('#DivTablaVariable').append(table);



                    $('.filasVar').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
                    });
                     $('.filasVar').mouseleave(function () {
                                $(this).css('text-decoration', '');
                    });

                    $('.filasVar').click(function () {
                        var _ID = '';
                        if (TypeId==4) {
                            _ID = $(this).find("td").eq(3).html();
                        }
                        else {
                            _ID=$(this).find("td").eq(2).html();
                        }
                        var _Nombre = $(this).find("td").eq(0).html();
                        var _Descripcion=$(this).find("td").eq(1).html();
                        $('#<%=LblModalEdName.ClientID%>').html(_Nombre);
                        $('#<%=LblModalEdDescription.ClientID%>').html(_Descripcion);
                        $('#<%=HdnIdItem.ClientID%>').val(_ID);
                        $('#LblMsj').html('Está seguro que desea eliminar ' + _Nombre + '?')
                            
                            $("#modalVarDetail").modal("show");

                    });

                    
                   
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    alert('Error :=> ' + textStatus + '--- ' + errorThrown);
                }
            });
        }

        function searchTable(inputVal) {
    var table = $('#TablaP');
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal, 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    return false;
                }
            });
            if (found === true) $(row).show(); else $(row).hide();
        }
    });
}

    </script>
</asp:Content>