<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/View/Planificacion/SitePlan.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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