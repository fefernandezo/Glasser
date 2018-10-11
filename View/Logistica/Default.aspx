<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/View/Logistica/SiteLog.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
     <div class="container">
         <asp:Panel ID="PanelAlerta" runat="server" Visible="false">
            <div class="container ">
                <div class="row">
                    <div class="col-12">
                        <div class="alert alert-success alert-dismissible fade show" role="alert" id="AlertaBienvenida">
                            <h4>Notificaciones</h4>
             
                        <div runat="server" id="MsjPortal"></div>
             

             <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
         </div>
                    </div>
                    
                </div>
            </div>
            
        </asp:Panel>
         <div class="row text-center text-success">
             <h1>BIENVENIDOS AL MÓDULO LOGÍSTICA GLASSER</h1>
         </div>       
      </div>



    <nav class="navbar fixed-bottom navbar-dark bg-primary">
        <div class="navbar nav">
            <asp:Image ID="Android" runat="server" CssClass="img-thumbnail w-25 h-25" ImageUrl="~/Images/Logos/android-icon2.png" />
        <button type="button" class="btn btn-outline-light" id="MOpenModalEliminar" data-toggle="modal" data-target="#modalMsj">Dercargar App Android</button>
        </div>
        

    </nav>


    <!--Modal Mensaje Descargar App-->
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
                    <h4>Está seguro que desea descargar la App?</h4>
                      </div>
                    
                  </div>

              </div>
          </div>
          <div class="modal-footer">
                  <asp:HyperLink ID="lnkdwnload" runat="server" CssClass="btn btn-success" NavigateUrl="https://drive.google.com/uc?authuser=0&id=1XkEZevtP1OlglcDdp4tKoqKSihwpckK4&export=download">Descargar</asp:HyperLink>
                  <span> </span>
                  <button type="button" class="btn btn-default" data-dismiss="modal">NO</button>
              
              
          </div>
          
       </div>
  </div>
</div>
    <script type="text/javascript">
        
$(document).ready(function () {
            CerrarAlert();

        });

        function CerrarAlert() {
            setInterval(function () {
                $('#AlertaBienvenida').alert("close");
            }, 10000);
            
        }
        

    </script>
</asp:Content>