<%@ Page Title="Usuario" Language="C#" MasterPageFile="~/View/Comercial/AdmVentas/SiteAdmVentas.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default_Variables" %>
<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
         .border-card1{
             border-color:rgba(0, 125, 189, 0.61);
         }
         .bg-card1{
             background-color:rgba(0, 125, 189, 0.61);
             text-shadow: -2px 0 rgba(0, 125, 189, 0.91),0 2px rgba(0, 125, 189, 0.98);
         }

         .border-card2{
             border-color:rgba(90, 0, 191, 0.70);
         }
         .bg-card2{
             background-color:rgba(90, 0, 191, 0.60);
             text-shadow: -2px 0 rgba(90, 0, 191, 0.91),0 2px rgba(90, 0, 191, 0.98);
         }


    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

   <div class="container">
       
       <div class="card card-border-onix mb-3">
                <div class="card-header card-bg-onix text-white" id="headerInfPed" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseInfoPed">
                    <h5><i  class="fas fa-chevron-down"> VARIABLES DE COMPONENTES Y MATERIALES</i></h5>
                </div>
                <div id="CollapseInfoPed" class="collapse show">
                    <div class="card-body ">
                         <div class="row mb-3 justify-content-center">
                             
                            <div class="col-6 text-center invisible">
                                <button type="button" id="BtnFamilias" onclick="OpenModal('1')" class="boton btn-onix">Familias</button>
                            </div>
                            <div class="col-6 text-center invisible">
                                <button type="button" id="BtnCategorias" onclick="OpenModal('2')" class="boton btn-onix">Categorías</button>
                            </div>
                        </div>
                        <div class="row mb-3 justify-content-center">
                            
                            <div class="col-6 text-center">
                                <button type="button" id="BtnColores" onclick="OpenModal('3')" class="boton btn-onix">Colores</button>
                            </div>
                            <div class="col-6 text-center">
                                <button type="button" id="BtnMarcas" onclick="OpenModal('4')" class="boton btn-onix">Marcas</button>
                            </div>
                        </div>

                    </div>
                    
                </div>
            </div>
       <div class="card card-border-lila mb-3">
                <div class="card-header card-bg-lila text-white" id="headerproductos" data-toggle="collapse" data-parent="#accordion" data-target="#CollapseProductos">
                    <h5><i  class="fas fa-chevron-down"> VARIABLES DE PRODUCTOS</i></h5>
                </div>
                <div id="CollapseProductos" class="collapse show">
                    <div class="card-body ">
                         <div class="row mb-3 justify-content-center">
                             <div class="col-6 text-center">
                                <button type="button" id="BtnFamiliasProd" onclick="OpenModal('5')" class="boton btn-uva">Familias de productos</button>
                            </div>

                            <div class="col-6 text-center">
                                <button type="button" id="BtnCategoriasProd" onclick="OpenModal('6')" class="boton btn-uva">Categorías de productos</button>
                            </div>

                            
                            
                        </div>
                        <div class="row mb-3 justify-content-center">
                             <div class="col-6 text-center">
                                <button type="button" id="BtnCanalDis" onclick="OpenModal('7')" class="boton btn-uva">Canales de Distribución</button>
                            </div>
                        </div>
                        

                    </div>
                   
                </div>
            </div>
       
   </div>
    <!--MODAL opciones en el boton-->
    <div class="modal fade" id="modalOpciones" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
                        <h3 class="modal-title" id="gridSystemModalLabel"><asp:Label runat="server" ID="ModalTitle"></asp:Label> </h3>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>  
              
          </div>
            <div class="modal-body">
                <asp:HiddenField runat="server" ID="HdnModal1" />
                <div class="row justify-content-center">
                    <div class="col-6 text-center">
                        <asp:Button runat="server" ID="BtnAdmvariable" OnClick="BtnAdmvariable_Click" Text="Administrar Variables" CssClass="btn btn-success" />
                    </div>
                    <div class="col-6 text-center">
                        <asp:Button runat="server" ID="BtnCrearVar" OnClick="BtnCrearVar_Click" Text="Crear Variables" CssClass="btn btn-info" />
                    </div>
                </div>
         
              
               
                
            </div> 
            <div class="modal-footer">
              
                
                
                
            </div>
            </div>
        </div><!-- /.modal-content -->
      </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.card-header').hover(function () {
                                
               
                $(this).css('cursor', 'pointer');
            });
            

        });

        function OpenModal(boton) {

            if (boton == '1') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Componentes y materiales: Familias ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Familias');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Familia');
            }
            else if (boton == '2') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Componentes y materiales: Categorías ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Categorías');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Categoría');

            }
            else if (boton=='3') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Componentes y materiales: Colores ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Colores');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Color');
            }
            else if (boton=='4') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Componentes y materiales: Marcas ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Marcas');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Marca');
            }
            else if (boton=='5') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Productos: Familias ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Familias');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Familia de producto');
            }
            else if (boton=='6') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Productos: Categorías ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Categorías');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Categoría de productos');
            }
            else if (boton=='7') {
                $('#modalOpciones').modal('show');
                $('#<%=HdnModal1.ClientID%>').val(boton);
                $('#<%=ModalTitle.ClientID%>').html('Canales de Distribución ');
                $('#<%=BtnAdmvariable.ClientID%>').val('Administrar Canales');
                $('#<%=BtnCrearVar.ClientID%>').val('Crear Canal de distribución');
            }
            

        }

    </script>
   
</asp:Content>
