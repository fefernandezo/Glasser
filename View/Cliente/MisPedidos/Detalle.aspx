<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.master" AutoEventWireup="true" CodeFile="Detalle.aspx.cs" Inherits="Cliente_MisPedidos_Detalle" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ScriptManager runat="server">

    </asp:ScriptManager>

    <div id="contenedor" class="container-fluid" runat="server">
        <br /><br /><br /><br /><br /><br />
        <div class="container-fluid">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item" runat="server" id="BreadPedido"></li>
                    <li class="breadcrumb-item active">Detalle</li>
                </ol>
            </nav>
            <div class="card mt-3 mb-3 bg-opacity-white-80">
                <div runat="server" id="DivEncabezado"></div>
            </div>
            <div class="card mb-3 bg-opacity-white-80">
                <div runat="server" id="DivDetalle" class="table-responsive"></div>
            </div>
            <div class="d-flex flex-row-reverse">
                <div class="d-flex flex-column-reverse">
                    <div class="card mb-3 bg-opacity-white-80">
                <div runat="server" id="Divtotales"></div>
            </div>
                </div>
            </div>
            

        </div>

    </div>

    <!--Modal-->
    <div class="modal fade"  id="modal" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              
          </div>
          <div class="modal-footer">
              
          </div>
          
       </div>
  </div>
</div>
    <script type="text/javascript">
       

    </script>
</asp:Content>