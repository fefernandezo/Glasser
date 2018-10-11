<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.Master" AutoEventWireup="true" CodeFile="Pedidos.aspx.cs" Inherits="_Pedidos" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>


         .centrado {
    
            vertical-align:central;
            text-align: center;
}
         .encabezado{
             padding-left:100px;
             padding-right:100px;
             margin-left:auto;
             margin-right:auto;
         }
         .buscar{
            
             margin-left:auto;
             margin-right:auto;
             max-width:500px;
         }
         
     .informe-fecha
        {
           
            background-color: blue;
        }
        .informe-estado
        {
            background-color: lightgrey;
        }
        
        .fabricacion
        {
            color:blue;
        }
        .paradesp{
            color: brown;
        }
        .despachado{
            color: yellowgreen;
        }
        table tr th {
  cursor: pointer;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
        .grupocodexpress{
            padding-top:20px;
        }

.sorting {
  background-color: #D4D4D4;
}

.asc:after {
  content: ' ↑';
}

.desc:after {
  content: " ↓";
}
.numpedido {
    border: 3px solid blue;
    position: relative;
    border-radius: 10px; 
    padding: 4px 4px 2px 4px;
    max-width:150px;
}




/* Form Progress */
.progress {
  max-width: 1000px;
  background-color: white;
  text-align: center;
  height: 100px;
  box-shadow: inset 0 0 0 rgba(0,0,0,.2);
 
}

/*linea al rededor de circulos y barra*/
.progress .circle,
.progress .bar {
  display: inline-block;
  background: #ffffff;
  width: 40px;
  height: 40px;
  border-radius: 40px;
  border: 1px solid #d5d5da;
}
.progress .bar {
  position: relative;
  max-width: 500px;
  height: 6px;
  top: -33px;
  margin-left: -5px;
  margin-right: -5px;
  border-left: none;
  border-right: none;
  border-radius: 0;
}
.progress .circle .label {
  display: inline-block;
  width: 32px;
  height: 32px;
  line-height: 30px;
  border-radius: 32px;
  margin-top: 2px;
  color: #b5b5ba;
  font-size: 17px;
}
.progress .circle .title {
  color: #b5b5ba;
  font-size: 12px;
  line-height: 30px;
  outline-width: 100px;
  margin-left: -5px;
  
}

/* Done / Active */
.progress .bar.done,
.progress .circle.done {
  background: #8bc435;
}
.progress .bar.active {
  background: linear-gradient(to right, #EEE 40%, #FFF 60%);
}
.progress .circle.done .label {
  color: #FFF;
  background: #8bc435;
  /*box-shadow: inset 0 0 2px rgba(0,0,0,.2);*/
  
}
.progress .circle.done .title {
  color: #8bc435;
}
.progress .circle.active .label {
  color: #FFF;
  background: #0c95be;
  box-shadow: inset 0 0 2px rgba(0,0,0,.2);
}
.progress .circle.active .title {
  color: #0c95be;
}
.cuadrotipodesp{
    padding-top: 5px;
    padding-bottom: 20px;
}
.clllegada {
    padding-left:50px;
    padding-top:-30px;
    display: block;
    font-size: 1em;
    margin-top: 1.33em;
    margin-bottom: 1.33em;
    margin-left: 0;
    margin-right: 0;
    font-weight: bold;
    
}

/*pasos del formulario de envio de pedido*/
/* Darkgrey */
/* Lightgrey */
/* Grey */
/* White */
/* Red */
/* Green */
/* Black */
/* blue */
* {
  box-sizing: border-box;
}

.steps {
  width: 100%;
  
  margin: 50px auto;
  box-sizing: border-box;
  /* End #steps ul*/
}
.steps ul,
.steps li {
  margin: 0;
  padding: 0;
  list-style: none;
}
.steps ul {
  display: table;
  width: 100%;
  /* End #steps li*/
}
.steps ul li {
  display: table-cell;
  position: relative;
  /* End .step*/
}
.steps ul li:first-child {
  width: 1px;
}
.steps ul li:first-child .step {
  /* Hide line before first step*/
}
.steps ul li:first-child .step:before {
  content: none;
}
.steps ul li .step {
  width: 40px;
  height: 40px;
  border: 1px solid;
  border-radius: 50%;
  border-color: #dedede;
  line-height: 37px;
  font-size: 15px;
  text-align: center;
  color: #999999;
  background-color: #dedede;
  float: right;
}
.steps ul li .step:before {
  height: 4px;
  display: block;
  background-color: #dedede;
  position: absolute;
  content: "";
  right: 40px;
  left: 0px;
  top: 50%;
  cursor: default;
}
.steps ul li .step:after {
  display: block;
  transform: translate(-42px, 10px);
  color: #999999;
  content: attr(data-desc);
  font-size: 13px;
  line-height: 15px;
  min-width: 120px;
}
.steps ul li .step.active {
  border-color: #0095db;
  color: white;
  background: #0095db;
}
.steps ul li .step.active:before {
  background: linear-gradient(to right, #2ecc71 0%, #0095db 100%);
}
.steps ul li .step.active:after {
  color: #0095db;
  font-weight: 600;
}
.steps ul li .step.done {
  background-color: #2ecc71;
  border-color: #2ecc71;
  color: white;
}
.steps ul li .step.done:after {
  color: #2ecc71;
}
.steps ul li .step.done:before {
  background-color: #2ecc71;
}
/* End #steps*/
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
   
    <asp:ScriptManager runat="server">
        <Scripts>
            
             
        </Scripts>
    </asp:ScriptManager>
    
    <br />
    <div class="container-fluid">
        <div class="row encabezado">
            <asp:HiddenField ID="ocultoRut" runat="server" />
            <asp:HiddenField ID="palabraBuscar" runat="server" />
            <div class="col-md-3">
                
                <p>
                    <button type="button" class="btn btn-primary" data-toggle="modal"  data-target="#ModalIngPed1"><i class=" fas fa-plus fa-2x" ></i> Ingresar Pedido</button>               
                </p>
            
            </div>
            
            <div class="col-md-3">
                <p>
                        <button type="button" class="btn btn-success" data-toggle="modal"  data-target="#modalInforme"><i class="fas fa-file-pdf fa-2x" ></i> Informe  </button>               
                </p>
                
            
            </div>
            <div  class="col-md-6">
                <div class="row">
                    <div id="ING" class="col-md-3 text-center" title="Filtrar los pedidos ingresados">
                   <i  class="fas fa-share-square"> </i><p> Ingresado</p>
               </div>
               <div id="PRG" class="col-md-3 text-center" title="Filtrar los pedidos en fabricación">
                   <i class="fas fa-calendar-alt fabricacion"> </i> <p>En fabricación</p>
               </div>
               <div id="DIS" class="col-md-3 text-center" title="Filtrar los pedidos disponible para despacho">
                    <i class="fas fa-warehouse paradesp"> </i> <p>Disponible para despacho</p>
               </div>
               <div id="DES" class="col-md-3 text-center" title="Filtrar los pedidos entregados">
                   <i class="fas fa-thumbs-up despachado"> </i> <p>Entregado</p>
               </div>    
                </div>
               
           </div> 
       
          
        </div>
         
    <br />
    <br />
    <div class="row">
        <div class="container">
            <div class="row buscar">
            <!--<asp:TextBox ID="search2" runat="server" placeholder="Buscar texto1" CssClass="form-control" Width="200"></asp:TextBox>-->
                
            <input type="text" id="search" size="10" placeholder="Buscar texto en pedidos" title="filtrar los pedidos que coincidan con el texto ingresado" class="form-control"/>
        </div>
        <br />
        <br />
        <div class="row">
            <div class ="table table-responsive">
        <div id="DivTabla">

        </div>
            </div>
        </div>
        
        </div>
        
    </div>
        </div>
       
     <!--MODAL INGRESO NUEVO PEDIDO 1 -->  
    <div class="modal fade"  id="ModalIngPed1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            
            <h4 class="modal-title" id="myModalLabel">Ingresar Nuevo Pedido</h4>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
              
              
          </div>
          <div class="modal-body">
              <div class="row center-block mb-3">
                  <div class="col-lg-3"></div>
                  <div class="col-lg-6">
                <div class="steps">
                    <ul>
                        <li><a id="InfPed" href="#"><div class="step active" data-desc="Información de Pedido">1</div></a></li>
                        <li><a id="InfDet" href="#"><div class="step" data-desc="Detalle del pedido">2</div></a></li>  
                        <li><a id="InfDesp" href="#"><div class="step" data-desc="Información de despacho">3</div></a></li>          
                        <li><a id="InfEnd" href="#"><div class="step" data-desc="Enviar">4</div></a></li>
                    </ul>
                </div>
            </div>
                 
              </div>  
               <div class="card border-primary mb-3">
                        <div class="card-header text-white bg-primary">
                            <h3 class="panel-title">Información del pedido</h3>
                        </div>
                        <div class="card-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col">
                                        <asp:TextBox ID="txtNomPedido" placeholder="Nombre del pedido" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorNombre" ValidationGroup="IngPed1" runat="server" ErrorMessage="Ingrese un nombre para el pedido" ForeColor="Red" ControlToValidate="txtNomPedido"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <asp:TextBox ID="txtObservacion" placeholder="Observación" CssClass="form-control" runat="server" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col">
                                        <label for="exampleInputFile">Adjuntar detalle del pedido</label><a class="btn" href="#" rel="popover" data-img="/Cliente/Diccionario/Plantillas/PlantillaPedido.PNG"><span class="glyphicon glyphicon-question-sign">?</span></a>
                                        <asp:FileUpload ID="fupArchivos" CssClass=" form-control-file" runat="server" />
                                        <asp:RequiredFieldValidator ID="ReqFieldArchivo" ValidationGroup="IngPed1" runat="server" ErrorMessage="Seleccione un archivo" ForeColor="Red" ControlToValidate="fupArchivos"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblMensajeAdj" CssClass="help-block" runat="server" Text="Adjunte archivo Excel" ></asp:Label>
                                    </div>
                                    <div class="col">
                                        <asp:LinkButton ID="LinkPlantilla" OnClick="LinkPlantilla_Click" title="Plantilla en formato xls" runat="server">Descargar Plantilla</asp:LinkButton>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                  </div>
              
          </div>      
              
          <div class="modal-footer">
      
              <asp:Button CssClass="btn btn-success" ID="btnModalIngPed1" ValidationGroup="IngPed1" CausesValidation="true" runat="server" Text="Siguiente" OnClick="IngPed2Open_Click" />
              
          </div>
          
       </div>
  </div>
</div>

     <!--MODAL INFORME EN PDF-->
        <div class="modal fade" id="modalInforme" tabindex="-1" role="dialog" aria-labelledby="ModalInformeLabel">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
                <h4 class="modal-title" id="ModalInformeLabel">Descarga de Informe</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
            <div class="modal-body">

                <div class="card border-info mb-3">
                    <div class=" card-header bg-info text-white">
                        Seleccionar Fecha
                    </div>
                    <div class="card-body text-info">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:UpdatePanel ID="UpdatePanelFEechDesde" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                            <h4>Desde</h4>
                                            <asp:TextBox title="Ingresar fecha" ID="TextFechDesde" CssClass="form-control" runat="server" Width="200" PlaceHolder="dd-mm-aaaa" ></asp:TextBox>
                                            <asp:Button runat="server" ID="BtnCalendarFechDesde" CssClass="btn btn-outline-info" ValidationGroup="GrupoFechDesde" OnClick="BtnCalendarFechDesde_Click" Text="Calendario" />
                                            <asp:Calendar ID="CalendarFechDesde" Visible="false"  OnSelectionChanged="CalendarFechDesde_SelectionChanged"  runat="server" TodayDayStyle-BackColor="Lime" FirstDayOfWeek="Monday" SelectedDayStyle-ForeColor="White">
                                                <TodayDayStyle BackColor="#666666" />
                                            </asp:Calendar>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnCalendarFechDesde" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                        </div>
                        <div class="col-md-6">
                                <asp:UpdatePanel ID="UpdatePanelFechHasta" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                            <h4>Hasta</h4>
                                            <asp:TextBox title="Ingresar fecha" ID="textFechHasta" CssClass="form-control" runat="server" Width="200" PlaceHolder="dd-mm-aaaa" ></asp:TextBox>
                                            <asp:Button runat="server" ID="BtnCalendarFechHasta" CssClass="btn btn-outline-info" ValidationGroup="GrupoFechDesde" OnClick="BtnCalendarFechHasta_Click" Text="Calendario" />
                                            <asp:Calendar ID="CalendarFechHasta" Visible="false" OnSelectionChanged="CalendarFechHasta_SelectionChanged"  runat="server" TodayDayStyle-BackColor="Lime" FirstDayOfWeek="Monday" SelectedDayStyle-ForeColor="White">
                                                <TodayDayStyle BackColor="#666666" />
                                            </asp:Calendar>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnCalendarFechHasta" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                        </div>
                        </div>
                        
                    </div>
                     </div>
                    <div class="card border-info mb-3">
                    <div class=" card-header  bg-info text-white">
                        Seleccionar estado de los pedidos
                    </div>
                    <div class="card-body text-info">
                        <div class="container">
                                    <div class="list-group">
                                        <asp:UpdatePanel ID="UpdatePanelCheckbox" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <p><asp:CheckBox ID="Es_todos" runat="server" OnCheckedChanged="Es_todos_CheckedChanged" AutoPostBack="true" Text="Todos" /></p>
                                                <p><asp:CheckBox ID="Es_Ingresados"  runat="server" Text="Ingresados" /></p>
                                                <p><asp:CheckBox ID="Es_Fabricacion" runat="server" Text="En Fabricación" /></p>
                                                <p><asp:CheckBox ID="Es_Disponible" runat="server" Text="Disponibles para despacho" /></p>
                                                <p><asp:CheckBox ID="Es_Entregados" runat="server" Text="Entregados" /></p>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Es_todos" EventName="CheckedChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                       
                                            
                                    </div>
                        </div>
                        
                    </div>
                    </div>


               
                
               
                
              
                
            </div> 
            <div class="modal-footer">

                <asp:LinkButton runat="server" ID="BtnDescagar" OnClick="BtnDescagarPDF_Click" CssClass="btn btn-success">
                    <i class=" fas fa-download"> Descargar PDF</i>
                </asp:LinkButton>
                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>
    
    <!--MODAL DETALLE DE PEDIDOS-->
    <div class="modal fade" id="modalDetallePedido" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <div class="modal-title">
                <h3 id="gridSystemModalLabel"></h3>
            </div>
                    
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
            <div class="modal-body">
                <div class="row justify-content-end">
                     <h4><label class="numpedido" id="Npedido"></label></h4>
                </div>
               
                    
                             
                   
                   <div class="row">
                       <p class="clllegada" id="llegada"></p>
                   </div>
                   
               
                
               <div class ="table table-responsive">
                  <div id="tablaCreada">
                  </div>    
                   </div>
                
            </div> 
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>


    <script type="text/javascript">
$(document).ready(function () {

    LoadPedidos("todos");

            $('a[rel=popover]').popover({
                html: true,
                trigger: 'hover',
                placement: 'top',
                container: 'body',
                content: function(){return '<img class="img" src="'+ $(this).data('img') + '" />';}
            });
            

           
           

    $('#ING').hover(function () {
        $(this).css('cursor', 'pointer');
    });

    $('#PRG').hover(function () {
        $(this).css('cursor', 'pointer');
    });
    $('#DIS').hover(function () {
        $(this).css('cursor', 'pointer');
    });
    $('#DES').hover(function () {
        $(this).css('cursor', 'pointer');
    });
    $('#TablaP').hover(function () {
        $(this).css('cursor', 'pointer');
    });

    $('#ING').click(function () {
        LoadPedidos("ING");
    });
    $('#PRG').click(function () {
        LoadPedidos("PRG");
    });
    $('#DIS').click(function () {
        LoadPedidos("DIS");
    });
    $('#DES').click(function () {
        LoadPedidos("DES");

    
    });
    
   
    $('#search').keyup(function () { searchTable($(this).val()); });



});

        
/**
 * 
 
 */
function LoadPedidos(tipo) {
    var datafrom;
    if (tipo === "todos") {
        datafrom = "{'Tipo': 'Todos'}";
    }
    else if (tipo === "ING") {
        datafrom = "{'Tipo': 'ING'}";
    }
    else if (tipo === "PRG") {
        datafrom = "{'Tipo': 'PRG'}";
    }
    else if (tipo === "DIS") {
        datafrom = "{'Tipo': 'DIS'}";
    }
    else if (tipo === "DES") {
        datafrom = "{'Tipo': 'DES'}";
    }

    $('#DivTabla').html('');
    
    $.ajax({
        type: 'post',
        url: '../ServiciosWeb/DatosDeServicios.asmx/TodosLosPedidos',
        data: datafrom,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            var table = '<table id="TablaP" class="table table-hover table-bordered">';
            table += '<strong><thead class="bg-primary" ><tr class="text-uppercase text-white text-center">'
                + '<th class="cabeza" id="hea1">Nombre Pedido</th>'
                + '<th class="cabeza" id="hea2">Número</th>'
                + '<th class="cabeza" id="hea3">Fecha Ingreso</th>'
                + '<th class="cabeza" id="hea4">Tipo Despacho</th>'
                + '<th class="cabeza" id="hea6">Observación</th>'
                + '<th class="cabeza" id="hea7">Usuario</th>'
                + '<th class="cabeza" id="hea8">Cantidad</th>'
                + '<th class="cabeza" id="hea9">Total</th>'
                + '<th class="cabeza" id="hea10">Estado</th>'
                + '</tr></thead></strong><tbody>';
            $.each(msg.d, function () {
                var Total = parseInt(this.Total);

                var T2 = Total.toLocaleString('it-IT');

                table = table + '<tr class="filas" ><td>' + this.Nombre + '</td>' +
                    '<td>' + this.Numero + '</td>' +
                    '<td>' + this.FechaIng + '</td>' +
                    '<td>' + this.TipoDes + '</td>' +
                    '<td>' + this.Observa + '</td>' +
                    '<td>' + this.Usuario + '</td>' +
                    '<td>' + this.Cantidad + '</td>' +
                    '<td>' + '$' + T2 + '</td>';
                    
                
                if (this.Estado === "ING") {
                    table = table + '<td class="ING centrado"><i class="fas fa-share-square"> </i></td></tr>';
                }
                if (this.Estado === "PRG") {
                    table = table + '<td class="PRG centrado"><i class="fas fa-calendar-alt fabricacion"> </i></td></tr>';
                }
                if (this.Estado === "DIS") {
                    table = table + '<td class="DIS centrado"><i class="fas fa-warehouse paradesp"> </i></td></tr>';
                }
                if (this.Estado === "DES") {
                    table = table + '<td class="DES centrado"><i class="fas fa-thumbs-up despachado"> </i></td></tr>';
                }
                table = table + '<td style="display:none;">' + this.FechaEntr + '</td>';
            });
            table = table + '</tbody></table>';

            $('#DivTabla').append(table);

            $('.cabeza').hover(function () {
                $(this).css('cursor', 'pointer');
                $(this).css('text-decoration', 'underline');
            });
            $('.cabeza').mouseleave(function () {

                $(this).css('text-decoration', '');
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
            })

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
            $("[id*='TablaP'] tr:has(td)").hover(function () {

                $(this).css("cursor", "pointer");

            });


            //carga Modal de detalle del pedido
            $('.filas').click(function () {

                console.log($(this).children().eq(0));
                var rowdata = $(this).children().eq(2);


                
                Npedido = $(this).find("td").eq(1).html();
                Nombre = $(this).find("td").eq(0).html();
                estatus = $(this).find("td").eq(8).attr("class");
                

               
                //detalle del pedido
                $.ajax({
                    type: 'post',
                    url: '../ServiciosWeb/DatosDeServicios.asmx/DetalleDelPedido',
                    data: "{'codigo':'" + Npedido + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msgd) {
                        var table = '<table id="TablaP2" class="table table-hover table-bordered">';
                        table += '<strong><thead ><tr class="text-uppercase text-info text-center">'
                            + '<th >Referencia</th>'
                            + '<th >Código</th>'
                            + '<th >Composición</th>'
                            + '<th >Cantidad</th>'
                            + '<th >Ancho</th>'
                            + '<th >Alto</th>'
                            + '<th >M2</th>'
                            + '<th >Precio Neto</th>'

                            + '</tr></thead></strong><tbody>';
                        $.each(msgd.d, function () {
                            var Total = parseInt(this.precio);

                            var T2 = Total.toLocaleString('it-IT');

                            table = table + '<tr ><td>' + this.modelo + '</td>' +
                                '<td>' + this.codPro + '</td>' +
                                '<td>' + this.compos + '</td>' +
                                '<td>' + this.cantidad + '</td>' +
                                '<td>' + this.ancho + '</td>' +
                                '<td>' + this.alto + '</td>' +
                                '<td>' + this.mt2 + '</td>' +
                                '<td>' + '$' + T2 + '</td>';



                        });
                        table = table + '</tbody></table>';
                        $("#gridSystemModalLabel").html(Nombre);
                        $("#Npedido").html("N°: " + Npedido);
                        
                        $.ajax({
                            type: 'post',
                            url: '../ServiciosWeb/DatosDeServicios.asmx/EncabezadoPedido',
                            data: "{'Npedido':'" + Npedido + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (ped) {

                                $.each(ped.d, function () {
                                    $("#llegada").html("Llegada estimada para el día: " + this.FechaEntrega);
                                });
                            }
                        });

                        
                        $("#tablaCreada").html(table);
                        if (estatus === "ING")
                        {
                            
                            
                            $("#span1").removeClass().addClass("label glyphicon glyphicon-send");
                            $("#span1").html("");
                            $("#span2").removeClass().addClass("label glyphicon glyphicon-calendar");
                            $("#span2").html("");
                            $("#span3").removeClass().addClass("label glyphicon glyphicon-home");
                            $("#span3").html("");
                            $("#circle1").removeClass().addClass("circle active");
                            $("#circle2").removeClass().addClass("circle");
                            $("#bar1").removeClass().addClass("bar");
                            $("#bar2").removeClass().addClass("bar");
                            $("#bar3").removeClass().addClass("bar");
                            $("#circle3").removeClass().addClass("circle");
                            $("#circle4").removeClass().addClass("circle");
                        }
                        if (estatus === "PRG")
                        {  
                            setInterval(function () {

                            }, 1000);

                            $("#span1").removeClass().addClass("label");
                            $("#span1").html("✓");
                            $("#span2").removeClass().addClass("label glyphicon glyphicon-calendar");
                            $("#span2").html("");
                            $("#span3").removeClass().addClass("label glyphicon glyphicon-home");
                            $("#span3").html("");
                            $("#bar1").removeClass().addClass("bar done");
                            $("#bar2").removeClass().addClass("bar");
                            $("#bar3").removeClass().addClass("bar");
                            $("#circle1").removeClass().addClass("circle done");
                            $("#circle2").removeClass().addClass("circle active");
                            $("#circle3").removeClass().addClass("circle");
                            $("#circle4").removeClass().addClass("circle");
                        }
                        if (estatus === "DIS")
                        {
                            $("#span1").removeClass().addClass("label");
                            $("#span1").html("✓");
                            $("#span2").removeClass().addClass("label");
                            $("#span2").html("✓");
                            $("#span3").removeClass().addClass("label glyphicon glyphicon-home");
                            $("#span3").html("");
                            $("#bar1").removeClass().addClass("bar done");
                            $("#bar2").removeClass().addClass("bar done");
                            $("#bar3").removeClass().addClass("bar");
                            $("#circle1").removeClass().addClass("circle done");
                            $("#circle2").removeClass().addClass("circle done");
                            $("#circle3").removeClass().addClass("circle active");
                            $("#circle4").removeClass().addClass("circle");
                        }
                        if (estatus === "DES")
                        {
                            $("#span1").removeClass().addClass("label");
                            $("#span1").html("✓");
                            $("#span2").removeClass().addClass("label");
                            $("#span2").html("✓");
                            $("#span3").removeClass().addClass("label");
                            $("#span3").html("✓");
                            $("#bar1").removeClass().addClass("bar done");
                            $("#bar2").removeClass().addClass("bar done");
                            $("#bar3").removeClass().addClass("bar done");
                            $("#circle1").removeClass().addClass("circle done");
                            $("#circle2").removeClass().addClass("circle done");
                            $("#circle3").removeClass().addClass("circle done");
                            $("#circle4").removeClass().addClass("circle active");
                        }
                       

                        $("#modalDetallePedido").modal("show");



                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        ///alert("Error detectado: " + textStatus + "\nExcepcion: " + errorThrown);
                        alert("Error detectado " + textStatus + "-" + errorThrown + "Error");
                    }
                });

            });



        }, error: function (jqXHR, textStatus, errorThrown) {
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