<%@ Page Title="Administrador" Language="C#" MasterPageFile="~/Administrador/SiteAdm.Master" AutoEventWireup="true" CodeFile="Agregar-Cliente.aspx.cs" Inherits="_Agregar_Cliente" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <style>
        .fila{
            
            margin-bottom: 20px;
            margin-left:20px;
        }
        .ddlist{
            max-width: 300px;
        }
        .linkbtn{
            margin-left:30px;
        }
        .txtsearch{
            max-width:300px;
        }
        .break{
            padding-top:30px;
            
        }
        .invisible{
            visibility:hidden;
            display:none;
        }
        .visible{
            visibility:visible;
            display:block;
        }

    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server">

    </asp:ScriptManager>
    <div class="container">
        <div class="row">
            <h2>Agregar Cliente a Ecommerce</h2>
        </div>
        <div class="form-inline">
            <asp:TextBox ID="TxtSearch" runat="server" Placeholder="Buscar Cliente en Random" title="búsqueda de cliente por nombre o rut en Random" CssClass=" form-control txtsearch"></asp:TextBox>
            <asp:Button ID="BtnseachCli" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="BtnseachCli_Click" />
        </div>
        
            <div id="Panelcliente" class="row break invisible">
               
                        <p><label id="lacliente"></label><asp:Label ID="TxtCliente" runat="server"></asp:Label></p>
                        <p><label id="larut"></label><asp:Label runat="server" ID="LBLrut"></asp:Label></p>
                <asp:HiddenField ID="Hddnombre" runat="server" />
                <asp:HiddenField ID="hddrut" runat="server" />
                <br />
                <div class="row">
                    <div class="col-lg-4 col-xs-12 col-md-6">
                        <div class="form-inline">
                            <div class="col-lg-12">
                                <label>Dirección:</label>&nbsp;
                            <asp:TextBox CssClass="form-control" runat="server" ID="Direccion"></asp:TextBox>
                            </div>
                            
                        </div>
                        
                    </div>
                </div><br /><br />
                <div class="row">
                    <div class="col-lg-4 col-xs-12 col-md-6">
                        <asp:DropDownList runat="server" CssClass=" form-control" ID="DDlRegion" AutoPostBack="true" OnSelectedIndexChanged="DDlRegion_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div><br />
                <asp:UpdatePanel ID="UpdatePanelComunas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-lg-4 col-xs-12 col-md-6">
                                <asp:DropDownList runat="server" CssClass=" form-control" ID="DDlComuna"></asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DDlRegion" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                
                
                    
            <br /><br />
                
                     <asp:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-inline">
                                Margen: <asp:DropDownList runat="server" CssClass="form-control" ID="DDlistMargen">
                                    </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-12">
                                <asp:CheckBox ID="ChkDVHM2" runat="server" Text="Precios de termopanel por metro cuadrado" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-12">
                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLTratodias">
                                    <asp:ListItem Text="Sin trato por días de entrega" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Plazo de entrega a 3 días" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Plazo de entrega a 4 días" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Plazo de entrega a 5 días" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Plazo de entrega a 6 días" Value="6"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
    
                        
                        </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Ingresar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                
            
            </div>
        
            <div id="PanelIngresar" class="row break invisible">
                <asp:Button runat="server" ID="Ingresar" CssClass="btn btn-success" Text="Ingresar" OnClick="Ingresar_Click" />
            </div>
            <div id="PanelGrid" class="row break">
            <div class="table-responsive">
                <div id="Grdclientes" runat="server"  CssClass="table table-hover"></div>
                    
                
            </div>
            
        </div>
        
        

    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('[id*=Grdclientes] tr:has(td)').hover(function () {
                $(this).css('cursor', 'pointer');
                
            }
            );
            $('[id*=Grdclientes] tr:has(td)').click(function () {
                var rut = $(this).find('td').eq(0).html();
                var dir = $(this).find('td').eq(2).html();
                var reg =$(this).find('td').eq(3).html();
                $('#Panelcliente').removeClass('invisible').addClass('visible');
                $('#PanelIngresar').removeClass('invisible').addClass('visible');
                $('#PanelGrid').hide();
                var cliente = $(this).find('td').eq(1).html();
                $('#lacliente').html('Cliente   :');
                $('#larut').html('Rut   :');
                $('#<%= TxtCliente.ClientID%>').html(cliente);
                $('#<%= Hddnombre.ClientID%>').val(cliente);
                $('#<%= LBLrut.ClientID%>').html(rut);
                $('#<%= hddrut.ClientID%>').val(rut);
                $('#<%= Direccion.ClientID%>').val(dir);
                
            });
        });
    </script>
</asp:Content>
