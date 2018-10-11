<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Distribuidor/SiteDistr.master" AutoEventWireup="true" CodeFile="MisCotizaciones.aspx.cs" Inherits="Dis_Default_Man" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
     <style>
        .headtable
        {
            background-color: rgba(29, 182, 65, 0.85);
            color: white;
           
            
        }
       
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <div class="container">
        <div class="row mb-5">
            <div class="col text-center">
                <h2><asp:Label runat="server" ID="titulo"></asp:Label></h2>
            </div>
        </div>
        
    </div>
    <div class="container">
        <div class="row mb-3">
            <div class="col text-center">
                <button type="button" id="BtnCrearCot" onclick="OpenModal()" class="btn btn-lila btn-lg">Nueva Cotización</button>
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
            <div class="table-responsive" runat="server" id="DivTablaCot">

            </div>
        </div>
    </div>

    <!--Modal Cotización-->
    <div class="modal fade"  id="modalCotizacion" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content ">
          <div class="modal-header">
            
            <h3><label id="LblCotizacionTitle"></label></h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <asp:HiddenField runat="server" ID="HdnIdCot" />
              <asp:HiddenField runat="server" ID="HdnTokenId" />
              <div class="row">
                  <div class="col-12 col-lg-6 text-center">
                      <asp:Button runat="server" ID="BtnVerCot" OnClick="BtnVerCot_Click" CssClass="btn btn-primary" Text="Ver Cotización" />
                  </div>
                  <div class="col-12 col-lg-6 text-center">
                      <asp:Button runat="server" ID="BtnEliminarCot" OnClick="BtnEliminarCot_Click" CssClass="btn btn-danger" Text="Eliminar" />
                  </div>
              </div>
          </div>
          
          
       </div>
  </div>
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.filas').hover(function () {
                $(this).css('cursor', 'pointer');
                $(this).css('background-color', 'rgba(121, 121, 121, 0.73)');
                $(this).css('color', 'white');
            });
            $('.filas').mouseleave(function () {
                $(this).css('color', 'black');
                $(this).css('background-color','');
            });
            $('.filas').click(function () {
                var nro = $(this).find("td").eq(0).html();
                var nombre = $(this).find("td").eq(1).html();
                var id = $(this).find("td").eq(5).html();
                var tokenid = $(this).find("td").eq(6).html();
                $('#LblCotizacionTitle').html(nro + ' - ' + nombre);
                $('#<%=HdnIdCot.ClientID%>').val(id);
                $('#<%=HdnTokenId.ClientID%>').val(tokenid);
                $('#modalCotizacion').modal('show');

            });
        });
       

    </script>
</asp:Content>