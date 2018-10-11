<%@ Page Title="Componentes" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="AdministracionComponentes.aspx.cs" Inherits="Com_Adm_Comp_" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
       
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container">
        <div class="row mb-3">
            <div class="col text-center">
                <button type="button" id="BtnCrearComponente" onclick="OpenModal()" class="btn btn-onix btn-lg">Crear componente</button>
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

    <!--MODAL opciones en el boton-->
    <div class="modal fade" id="modalCrearComponente" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
                        <h3 class="modal-title" id="gridSystemModalLabel">Crear Componente </h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                
                <div class="row mb-2">
                    <div class="col-12 col-lg-6">
                        <div class=" form-group">
                            <label for="TxtNombreComp">Nombre:</label><asp:TextBox runat="server" ID="TxtNombreComp" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="TxtNombreComp" ValidationGroup="CreateComp" ErrorMessage="Campo obligatorio" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
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
                <div class="row mb-2">
                    <div class="col-12 col-lg-6">
                        <div class=" form-group">
                           <label for="FileUpFoto">Adjuntar Foto</label> <asp:FileUpload runat="server" ID="FileUpFoto" CssClass="form-control-file" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7"
                               runat="server" ControlToValidate="FileUpFoto"
                               ErrorMessage="Se permite solo archivos con formato .jpg y .png" ForeColor="Red"
                               ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.gif|.jpeg|.jpg|.png)$"
                               ValidationGroup="CreateComp" SetFocusOnError="true"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-12 col-lg-6 text-right">
                        <div class="form-group">
                            <asp:Button runat="server" ID="BtnCrearComp" ValidationGroup="CreateComp" OnClick="BtnCrearComp_Click" CssClass="btn btn-outline-info" Text="Siguiente" />
                        </div>
                    </div>
                </div>
         
              
               
                
            </div> 
            <div class="modal-footer">
              
                
                
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>


    <!--MODAL Detalle-->
    <div class="modal fade" id="modalDetalle" tabindex="-1" role="dialog" aria-labelledby="tituloDetail" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
                        <h3 class="modal-title" id="lbltitleDetail"></h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                <asp:HiddenField runat="server" ID="HdnIDItem" />
                <asp:HiddenField runat="server" ID="HdnTokenIdItem" />
                
                <img id="ImgItem" src="http://cproyecto.phglass.cl/Images/COT_COMPONENTES/180801producto%1.jpg" class=" img-fluid rounded mx-auto d-block" />
               
               <div class="row mb-3">
                   <div class="col-11">
                       <div class="form">
                           <strong><label>Descripción:</label></strong>
                           <label id="LblDescrip"></label>
                       </div>
                   </div>

               </div>
                <div class="row mb-3">
                    <div class="col-5">
                        <div class="form-inline">
                            <strong><label>Precio:</label></strong>
                            <span>&nbsp;</span>
                            <label id="lblPrecio"></label>
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
                      <label>Seguro que desea eliminar este componente?</label>
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

    <script type="text/javascript">
        $(document).ready(function () {
            $('.filas').hover(function () {
                        $(this).css('cursor', 'pointer');
                        $(this).css('text-decoration', 'underline');
            });
            $('.filas').mouseleave(function () {

                        $(this).css('text-decoration', '');
            });
            $('.filas').click(function () {
                var ID = $(this).find("td").eq(5).html();
                var token = $(this).find("td").eq(6).html();
                var srcimg = $(this).find("td").eq(7).html();
                var precio = $(this).find("td").eq(8).html();
                var unidad = $(this).find("td").eq(9).html();

                var nombre = $(this).find("td").eq(0).html();
                var descr = $(this).find("td").eq(1).html();

                if (precio>0) {
                    $('#lblPrecio').html('$' + precio.toLocaleString('it-IT',{ style: 'currency', currency: 'CLP' }) + ' por ' +  unidad.toLocaleString() + '.');
                }
                else {
                    $('#lblPrecio').html('No se ha definido el precio.');
                }
                $('#<%=HdnIDItem.ClientID%>').val(ID);
                $('#<%=HdnTokenIdItem.ClientID%>').val(token);
                $('#lbltitleDetail').html(nombre);
                $('#modalDetalle').modal('show');
                $('#ImgItem').attr('src', srcimg);
                $('#LblDescrip').html(descr);
            });
        });
        
        function OpenModal() {
            $('#modalCrearComponente').modal('show');
            
        }

    </script>
</asp:Content>