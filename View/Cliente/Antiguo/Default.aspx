<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
         
         <div class="d-flex justify-content-center">
             <h1><asp:Label ID="LblCliente" runat="server"></asp:Label></h1>
         </div>
                <div id="accordion">
                     <div class="card mb-3">
                            <div class="card-header">
                                <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseFin" class="glyphicon glyphicon-chevron-down"> Estado Financiero</a></h3>    
                            </div>
                                    <div id="CollapseFin" class="collapse show">
                                            <div class="card-body">
                                                <div class="row">
                                                     <div class="col">
                                                                
                                                         <div class="card border-info">
                                                             <div class="card-header bg-info text-white">
                                                                 <h5 class="text-center">Crédito disponible</h5>
                                                             </div>
                                                            <div class="card-body">
                                                               
                                                                <h3 class=" text-center"><asp:Label ID="lblCreditoDis" runat="server" Text=""></asp:Label></h3>
                                                            </div>
                                                        </div>
                                                            <br />
                                                        </div>
                                                        <div class="col">
                                                                
                                                        <div class="card border-info">
                                                            <div class="card-header bg-info text-white">
                                                                <h5 class="text-center">Cupo utilizado</h5>
                                                            </div>
                                                                <div class="card-body">
                                                                    <h3 class="text-center"><asp:Label ID="lblDeuda" runat="server" Text=""></asp:Label></h3>
                                                                </div>
                                                        </div>
                                                            <br />
                                                        </div>
                                                        <div class="col">
                                                                
                                                        <div class="card border-info">
                                                            <div class="card-header bg-info text-white">
                                                                <h5 class="text-center ">Disponible para comprar</h5>
                                                            </div>
                                                                <div class="card-body">
                                                                    <h3 class="text-center"><asp:Label ID="LabelDisponible" runat="server" Text=""></asp:Label></h3>
                                                                    <asp:HiddenField ID="Hiddendisponible" runat="server" />
                                                                    
                                                                </div>
                                                        </div>
                                                            <br />
                        
                                                        </div>
                                                </div>
                                                       
                                            </div>
                                    </div>
                
                     </div>
                    
                    <div class="card mb-3">
                            <div class="card-header">
                                <h3 class="card-title"><a data-toggle="collapse" data-parent="#accordion" href="#CollapseInfo" class="glyphicon glyphicon-chevron-down">Plazos de Entrega</a></h3>    
                            </div>
                                    <div id="CollapseInfo" class="collapse show">
                                         <div class="card-body">
                                             <div class="row">
                                                 <div class="col">
                                                    <div class="card border-success">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Termopaneles</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="d-flex justify-content-center">
                                                                <h6><asp:Label ID="FechaTermo" runat="server"></asp:Label></h6>
                                                            </div>
                                                            <div class="d-flex justify-content-center">
                                                                <asp:Label ID="diastermo" runat="server"></asp:Label>*
                                                            </div>    
                                                        </div>
                                                    </div>
                                                </div>
                                                 <br />
                
                                                <div class="col">
                                                    <div class="card border-success">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Láminas</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="d-flex justify-content-center">
                                                                <h6><asp:Label ID="FechaLaminas" runat="server"></asp:Label></h6>
                                                            </div>
                                                            <div class="d-flex justify-content-center">
                                                                <asp:Label ID="DiasLaminas" runat="server"></asp:Label>*
                                                            </div>
                                                          
                                                                
                                                        </div>
                                                    </div>
                                                </div>
                                                 <br />
                                                <div class="col">
                                                    <div class="card border-success">
                                                        <div class="card-header headtable">
                                                            <h5 class="text-center">Arquitectura</h5>
                                                        </div>
                                                       <div class="card-body">
                                                           <div class="d-flex justify-content-center">
                                                               <h6><asp:Label ID="FechaArq" runat="server"></asp:Label></h6>
                                                           </div>
                                                           <div class="d-flex justify-content-center">
                                                               <asp:Label ID="DiasArq" runat="server"></asp:Label>*
                                                           </div>
                                                           
                                                                
                                                        </div>
                                                    </div>
                                                </div>
                                             </div>
                                                
                                            
                                         </div>
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