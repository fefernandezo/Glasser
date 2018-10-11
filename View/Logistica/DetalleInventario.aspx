<%@ Page Title="Administracion de Rutas" Language="C#" MasterPageFile="~/View/Logistica/SiteLog.Master" AutoEventWireup="true" CodeFile="DetalleInventario.aspx.cs" Inherits="_AdmRutas" %>

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
         .bodyDiv{
            padding-left:15px;
            padding-right:15px;
            padding-top:15px;
            padding-bottom:15px;
        }

       .pagination-ys {
    /*display: inline-block;*/
    padding-left: 5px;
    margin: 20px 0;
    border-radius: 4px;
}

.pagination-ys table > tbody > tr > td {
    display: inline;
}

.pagination-ys table > tbody > tr > td > a,
.pagination-ys table > tbody > tr > td > span {
    position: relative;
    float: left;
    padding: 8px 12px;
    line-height: 1.42857143;
    text-decoration: none;
    color: #dd4814;
    background-color: #ffffff;
    border: 1px solid #dddddd;
    margin-left: -1px;
}

.pagination-ys table > tbody > tr > td > span {
    position: relative;
    float: left;
    padding: 8px 12px;
    line-height: 1.42857143;
    text-decoration: none;    
    margin-left: -1px;
    z-index: 2;
    color: #aea79f;
    background-color: #f5f5f5;
    border-color: #dddddd;
    cursor: default;
}

.pagination-ys table > tbody > tr > td:first-child > a,
.pagination-ys table > tbody > tr > td:first-child > span {
    margin-left: 0;
    border-bottom-left-radius: 4px;
    border-top-left-radius: 4px;
}

.pagination-ys table > tbody > tr > td:last-child > a,
.pagination-ys table > tbody > tr > td:last-child > span {
    border-bottom-right-radius: 4px;
    border-top-right-radius: 4px;
}

.pagination-ys table > tbody > tr > td > a:hover,
.pagination-ys table > tbody > tr > td > span:hover,
.pagination-ys table > tbody > tr > td > a:focus,
.pagination-ys table > tbody > tr > td > span:focus {
    color: #97310e;
    background-color: #eeeeee;
    border-color: #dddddd;
}
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <div class="container">
         <asp:HiddenField runat="server" ID="HdnIdInventario" />
         <asp:HiddenField runat="server" ID="CantItems" />
         <asp:Panel ID="PanelGral" runat="server">
             <div class="text-right">
                 <p></p>
                 <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalDownload"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span> Descargar Excel</button>
                 <p></p>
             </div>
            
                 
                 
                     <div id="divPaginas" ></div>
                 
             
             <div class="row">
                 <div class="table table-responsive">
                     <div id="Tabladetail"></div>
                 </div>
             </div>
         </asp:Panel>
                        
                 
                
     </div>

    <!--MODAL ITEM SELECTED-->

    <div class="modal fade" id="modalItemSelected" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
                    <div class=" col-lg-6 text-left">
                        <h3 class="modal-title" id="gridSystemModalLabel">Código :  <asp:Label runat="server" ID="LblCodigo"></asp:Label> </h3>
                    </div>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
              
              
          </div>
            <div class="modal-body">
                <asp:HiddenField runat="server" ID="HdnIdItem" />
                <asp:HiddenField runat="server" ID="HdnPagina" />
                <div class="row bodyDiv">
                    <div class="col-lg-12 col-xs-12">
                        <h4><asp:Label runat="server" Text="Descripción :" Font-Bold="true"></asp:Label> <asp:Label runat="server" ID="LblDescripcion"></asp:Label></h4>
                    </div>
                    
                </div>
                <div class="row bodyDiv">
                    <div class="col-lg-12 col-xs-12">
                        <h4><asp:Label runat="server" Text="Ingresado por :" Font-Bold="true"></asp:Label> <asp:Label runat="server" ID="LblOperario"></asp:Label></h4>
                    </div>
                    <div class="col-lg-12 col-xs-12">
                        <h4><asp:Label runat="server" Text="Fecha de Ingreso :" Font-Bold="true"></asp:Label> <asp:Label runat="server" ID="LblFechaCont"></asp:Label></h4>
                    </div>
                </div>
              
               
                
            </div> 
            <div class="modal-footer">
                 
                <div class="btn-group mr-2" role="group">
                    <button type="button" data-toggle="modal" data-target="#modalEliminarItem" class="btn btn-danger">Eliminar Item</button><span> </span>
                </div>
                <div class="btn-group mr-2" role="group">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>  
                </div>
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>


    <!--MODAL Excel Download-->

    <div class="modal fade" id="modalDownload" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalexceld">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
           
                    <div class=" col-lg-6 text-left">
                        <h3><asp:Label runat="server">Descargar Excel</asp:Label> </h3>
                    </div>
               <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
              
              
          </div>
            <div class="modal-body">
               
               
                    <div class="btn-group mr-2" role="group">
                         <asp:Button runat="server" ID="BtnDownloadExcel" OnClick="BtnDownLoadExcel_Click"  CssClass="btn btn-success" Text="Descargar" /><span> </span>
                    </div>
                <div class="btn-group mr-2" role="group">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>  
                </div>
                        
                    
               
              
               
                
            </div> 
            
            </div>
        </div><!-- /.modal-content -->
      </div>

    <!--MODAL Eliminar Item-->
     <div class="modal fade" id="modalEliminarItem" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalelimnaritem">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
              <div class=" col-lg-6 text-left">
                        <h3><asp:Label CssClass="modal-title" runat="server">Mensaje</asp:Label> </h3>
                    </div>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
              
              
          </div>
            <div class="modal-body">
               
               <asp:Label runat="server" Text="Está seguro que desea elimnar este item?"></asp:Label>
              
               
                
            </div> 
            <div class="modal-footer">
                 <div class="btn-group">
                         <asp:Button runat="server" ID="BtnModalDeleteItem" OnClick="BtnModalDeleteItem_Click" CssClass="btn btn-danger" Text="SI" /><span> </span>
                        <button type="button" class="btn btn-default" data-dismiss="modal">NO</button>  
               </div>
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>

    <script type="text/javascript">
         $(document).ready(function () {
                 var IDINV = $('#<%=HdnIdInventario.ClientID %>').val();
             LoadFirst(IDINV);
             
             
        });

        function LoadFirst(idInv) {
            LoadDetail(idInv, 1);
             
            LoadPage(idInv,1);
            
           
            
        }

        function LoadPage(idinventario,pagina) {
            

             var datafrom = "{'IdInventario':'" + idinventario + "','Token':'BaPRiThWJLYvy3KOn04K43we4XtyMdJIiYzqFwvLH0'}";
            $.ajax({
                    type: 'post',
                    url: '../ServiciosWeb/WSlogistica.asmx/DetalleinvItems',
                    data: datafrom,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                success: function (msg) {

                    var _paginas;
                    var table2 = '<nav id="tabla2" class="Page navigation"><ul class="pagination justify-content-center">';

                    $.each(msg.d, function () {

                    
                        _paginas = Math.ceil(this._Items / 20);
                       
                         for (var i = 1; i <= 10; i++)
                         {
                             
                             if (pagina == 1 && i==1) {
                                 table2 = table2 + '<li class="pagina page-item active"><a class="page-link">' + i.toString() + '</a></li>';
                             }
                             else {
                                 table2 = table2 + '<li class="pagina page-item"><a class="page-link">' + i.toString() + '</a></li>';
                             }
                             

                        }
                        table2 = table2 + '</ul></nav>';

                       
                                
                    });

                    function SetPaginas(first, totalpag) {
                         var tpaginas = '<nav id="tabla2" class="Page navigation"><ul class="pagination justify-content-center">';
                        var last = first + 9;
                        if (totalpag >= (last) && first > 1) {
                            tpaginas = tpaginas + '<li class="anterior page-item"><a class="page-link" href="#">Siguiente</a></li>';
                            for (var i = first; i <= last; i++) {
                                tpaginas = tpaginas + '<li class="pagina page-item active"><a class="page-link">' + i.toString() + '</a></li>';
                            }
                            tpaginas = tpaginas + '<li class="siguiente page-item"><a class="page-link" href="#">Siguiente</a></li>';
                        }
                        else {

                        }
                    }
                    

                    $('#divPaginas').append(table2);

                           
                          
                    $('.pagina').hover(function () {

                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');

                    });

                    $('.pagina').mouseleave(function () {

                        $(this).css('text-decoration', '');

                    });


                    $('.pagina').click(function () {

                        var _ID = $(this).find('a').html();
                        
                        $('.pagina').removeClass().addClass('pagina page-item');

                        $(this).addClass('active');

                        LoadDetail(idinventario, _ID);
                    });

                           
                },

                error: function (jqXHR, textStatus, errorThrown) {

                    alert('Error :=> ' + textStatus + '--- ' + errorThrown);

                }


            });


        }   

        function LoadDetail(IdInv, pagina) {
            var datafrom = "{'IdInventario':'" + IdInv + "','Token':'BaPRiThWJLYvy3KOn04K43we4XtyMdJIiYzqFwvLH0','Page':'" + pagina + "'}";

              
                               


               $('#Tabladetail').html('');
                $.ajax({
                    type: 'post',
                    url: '../ServiciosWeb/WSlogistica.asmx/DetalleInventario',
                    data: datafrom,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (msg) {

                            var table = '<table id="TablaP" class="table table-hover">';
                                table += '<strong><thead ><tr class="text-uppercase text-info text-center">'
                                    + '<th class="cabeza"  id="hea1">Código</th>'
                                    + '<th class="cabeza" id="hea4">Descripción</th>'
                                    + '<th class="cabeza" id="hea2">Cantidad</th>'
                                    + '<th class="cabeza" id="hea3">Un</th>'
                                    + '<th class="cabeza" id="hea5">Bodega</th>'
                                     
                                    + '</tr></thead></strong><tbody>';
                            $.each(msg.d, function () {
                                
                                
                                table = table + '<tr class="filas">'+
                                    '<td> ' + this._KOPR + '</td> ' +
                                    '<td>' + this._RNDDescripcion + '</td>' +
                                    '<td>' + this._Cant + '</td>' +
                                    '<td>' + this._Unidad + '</td>' +
                                    '<td>' + this._Bodega + '</td>' +
                                    '<td style="display:none;">' + this._Id + '</td>'+
                                    '<td style="display:none;">' + this._CodBodega + '</td>'+
                                    '<td style="display:none;">' + this._Operario + '</td>'+
                                    '<td style="display:none;">' + this._Fecha + '</td>';
               
                            });
                                table = table + '</tbody></table>';

                            $('#Tabladetail').append(table);
                           
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
                            var _ID = $(this).find("td").eq(5).html();
                            var _codigo = $(this).find("td").eq(0).html();
                                var _descripcion = $(this).find("td").eq(1).html();
                                var _codbodega = $(this).find("td").eq(6).html();
                                var _operario = $(this).find("td").eq(7).html();
                                var _fecha = $(this).find("td").eq(8).html();
                                
                             
                                $('#<%=HdnIdItem.ClientID %>').val(_ID);
                                $('#<%=HdnPagina.ClientID %>').val(pagina);

                                $('#<%=LblCodigo.ClientID %>').html(_codigo);
                                $('#<%=LblDescripcion.ClientID %>').html(_descripcion);
                                $('#<%=LblOperario.ClientID %>').html(_operario);
                                $('#<%=LblFechaCont.ClientID %>').html(_fecha);
                            
                            $("#modalItemSelected").modal("show");

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