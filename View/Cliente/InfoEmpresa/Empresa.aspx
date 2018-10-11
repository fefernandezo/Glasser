<%@ Page Title="Empresa" Language="C#" MasterPageFile="~/View/Cliente/SiteCliente.master" AutoEventWireup="true" CodeFile="Empresa.aspx.cs" Inherits="Cliente_Default_Man" %>

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
     <div class="container-fluid">
         <asp:HiddenField runat="server" ID="HdnRutCli" />
         
                 
                
            </div>

    <!--Modal-->
    <div class="modal fade"  id="modalChangeCli" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content modal-sm">
          <div class="modal-header">
            
            <h3>Seleccionar Empresa</h3>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col">
                      <asp:DropDownList runat="server" ID="DDLEmpresas"  CssClass="form-control"></asp:DropDownList>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              
                  
              
                  
              
              
          </div>
          
       </div>
  </div>
</div>
    <script type="text/javascript">
       

    </script>
</asp:Content>