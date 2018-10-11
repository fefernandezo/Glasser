<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Account/SiteLogin.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<asp:Content runat="server" ID="HeaderContent" ContentPlaceHolderID="FeaturedContent" >

	<style>

     .loader {
    border: 16px solid #f3f3f3;
    border-radius: 50%;
    border-top: 16px solid #3498db;
    width: 120px;
    height: 120px;
    -webkit-animation: spin 2s linear infinite; /* Safari */
    animation: spin 2s linear infinite;
}
     .loader-bg{
         background: rgba(0, 0, 0, 0.58);
     }

/* Safari */
@-webkit-keyframes spin {
    0% {
        -webkit-transform: rotate(0deg);
    }

    100% {
        -webkit-transform: rotate(360deg);
    }
}

@keyframes spin {
    0% {
        transform: rotate(0deg);
    }

    100% {
        transform: rotate(360deg);
    }
}

.mdl-loading-bg{
    background: rgba(0, 21, 90, 0.55);
}

    
	
		img{
  display: block;
  margin: auto;
  width: 100%;
  height: auto;
}
		.phglass{
			margin-left:5%;
			margin-right:5%;
			margin-top: 60px;
            

		}
		.phglassopacity{
			opacity:0.4;
		}
		.error{
			color:red;
			margin-top:300px;
			padding-left:50%;
			background: rgba(255,255,255,.4);
		}

#login-button{
  cursor: pointer;
  position:absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  padding: 30px;
  margin: auto;
  width: 100px;
  height: 100px;
  border-radius: 50%;
  background: rgba(5, 113, 1, 0.95);
  overflow: hidden;
  opacity: 0.8;
  box-shadow: 15px 15px 30px #000;}
#login-button:hover{
	opacity:0.99;
}

.bg-modal-transp{

			background: rgba(0, 21, 90, 0.85);
			box-shadow:  10px 8px 10px rgba(0, 0, 0, 0.98);
}
	

/* Login container */
		#container {
			position: absolute;
			top: 10px;
			left: 0;
			right: 0;
			bottom: 0;
			margin: auto;
			width: 260px;
			height: 380px;
			padding-top: 30px;
			background: rgba(0, 21, 90, 0.8);
			box-shadow:  10px 8px 10px rgba(0, 0, 0, 0.98);
			
			display: none;
			
		}
	


.close-btn{
  position: absolute;
  cursor: pointer;
  
  
  color:white;
  top: 10px;
  right: 10px;
  width: 20px;
  height: 20px;
  text-align: center;
  border-radius: 10px;
  opacity: .5;
  -webkit-transition: all 2s ease-in-out;
  -moz-transition: all 2s ease-in-out;
  -o-transition: all 2s ease-in-out;
  transition: all 0.2s ease-in-out;
}

.close-btn:hover{
  opacity: 1;
}

/* Heading */
h1{
  font-family: 'Open Sans Condensed', sans-serif;
  position: relative;
  margin-top: 0px;
  text-align: center;
  font-size: 40px;
  color: #ddd;
  text-shadow: 3px 3px 10px #000;
}

/* Inputs */
a,
.input{
  font-family: 'Open Sans Condensed', sans-serif;
  text-decoration: none;
  position: relative;
  width: 80%;
  display: block;
  margin: 9px auto;
  font-size: 17px;
  color: #fff;
  padding: 8px;
  border-radius: 6px;
  border: none;
  background: rgba(255, 255, 255, 0.26);
  -webkit-transition: all 2s ease-in-out;
  -moz-transition: all 2s ease-in-out;
  -o-transition: all 2s ease-in-out;
  transition: all 0.2s ease-in-out;
}

.input:focus{
  outline: none;
  box-shadow: 3px 3px 10px #333;
  background: rgba(3,3,3,.18);
}

/* Placeholders */
::-webkit-input-placeholder {
   color: #ddd;  }
:-moz-placeholder { /* Firefox 18- */
   color: red;  }
::-moz-placeholder {  /* Firefox 19+ */
   color: red;  }
:-ms-input-placeholder {  
   color: #333;  }

/* Link */

.boton{
	padding-left:30px;
}



#remember-container{
  position: relative;
  margin: -5px 20px;
}





.remember{
  position: absolute;
  font-size: 13px;
  font-family: 'Hind', sans-serif;
  color: rgba(255,255,255,.9);
  top: 7px;
  left: 20px;
}

#forgotten{
  position: absolute;
  font-size: 12px;
  font-family: 'Hind', sans-serif;
  color: rgba(255,255,255,.9);
  right: 0px;
  top: 8px;
  cursor: pointer;
  -webkit-transition: all 2s ease-in-out;
  -moz-transition: all 2s ease-in-out;
  -o-transition: all 2s ease-in-out;
  transition: all 0.2s ease-in-out;
}

#forgotten:hover{
  color: rgba(255,255,255,.6);
}

#forgotten-container{
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  margin: auto;
  width: 260px;
  height: 180px;
  border-radius: 10px;
  background: rgba(3,3,3,0.25);
  box-shadow: 1px 1px 50px #000;
  display: none;
}

.orange-btn{
  background: rgba(87,198,255,.5);
}
.chebxtxt{
	color:white;
	border:inherit;
}

.mb-300{
    margin-top:300px;
}



	</style>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

	<div id="logoph" class=" text-center phglass">    
				<asp:Image ID="LogoLogin" CssClass="img-fluid center-block" Width="500" ImageUrl="~/Images/Logos/logo-phglass-w.png" runat="server" />
			   
			</div>
	<div id="login-button">
  <img src="https://dqcgrsy5v35b9.cloudfront.net/cruiseplanner/assets/img/icons/login-w-icon.png"/>
		
</div>

    
			<div class="contenedor" id="container">
				<h1>Iniciar sesión</h1>
					<span class="close-btn">
						<i class="fas fa-times-circle fa-lg"></i>
					</span>
                


				<div class="row">
                    <asp:Login runat="server" ID="ctlloggin" OnLoggedIn="ctlloggin_LoggedIn" OnLoginError="ctlloggin_LoginError" ViewStateMode="Disabled" RenderOuterTable="false">


		<LayoutTemplate>
					
                    

                    <div class="col-12">
                        
                            <asp:TextBox CssClass="input NombreUser" runat="server" ID="UserName" ValidationGroup="Login" placeholder="Nombre Usuario" />
							<asp:RequiredFieldValidator runat="server" ValidationGroup="Login" ControlToValidate="UserName" ErrorMessage="obligatorio." ForeColor="Red" />
                        </div>
                        
                        <div class="col-12">
                            <asp:TextBox CssClass="input" ValidationGroup="Login" runat="server" ID="Password" placeholder="Contraseña" TextMode="Password" />
							<asp:RequiredFieldValidator runat="server" ValidationGroup="Login" ControlToValidate="Password" ErrorMessage="obligatorio." ForeColor="Red" />
                        </div>
                        <div class="col-12 mb-3 text-center">
                            <asp:Button runat="server" CommandName="Login" ValidationGroup="Login" CssClass="btn btn-success" Text="Iniciar sesión" />
                        </div>

                    </LayoutTemplate>
	</asp:Login>


                        <div class="col-12 text-center">
                            <div class="form-check">
						   <asp:CheckBox runat="server" CssClass="form-check-input" Checked="true" ID="RememberMe"/>
							<label for="RememberMe" class="form-check-label text-white">¿Recordar cuenta?</label>
					   </div>
                        </div>
                    <div class="col-12 text-center">
                        <button type="button" id="ForgetPassBtn" class="btn btn-link text-white"  data-toggle="modal" data-target="#modalForgetPass">Olvidé mi contraseña</button>
                    </div>
                    
                   
				</div>
				
			


			</div>
    <div class="container">
        <div class="row mb-300">
            <div class="col">
                <asp:Label runat="server" ID="LblError" ForeColor="White" Font-Bold="true"></asp:Label>
            </div>
        </div>
    </div>

    
   
		
	
	



<!-- Forgotten Password Container -->
<!--Modal forgetpass-->
    <div class="modal fade"   id="modalForgetPass" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-dialog-centered" style="vertical-align:middle !important;" role="document">
        <div class="modal-content modal-sm bg-modal-transp">
          <div class="modal-header text-white">
            
            <h3> Recuperación de contraseña</h3>
              <span class="close-btn" data-dismiss="modal">
						<i class="fas fa-times-circle fa-lg"></i>
					</span>
          </div>
            <asp:UpdatePanel runat="server" ID="UPpanelforeget" UpdateMode="Conditional">
                <ContentTemplate>
                     <div class="modal-body">
              <div class="row">
                  <div class="col">
                      <asp:TextBox runat="server" ID="TxtEmal" CssClass="input" placeholder="Correo"></asp:TextBox>
                      <asp:Label runat="server" ID="LblMsjforeget" ForeColor="Red" Visible="false"></asp:Label>
                  </div>
              </div>
          </div>
          <div class="modal-footer">
              
                  <asp:Button runat="server" ID="RecuperarPass" OnClick="RecuperarPass_Click" OnClientClick="javascript:Loading();" CssClass="btn btn-primary" Text="Recuperar" />
              
          </div>
          

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="RecuperarPass" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
           
         
       </div>
  </div>
</div>

     <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="UPpanelforeget" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="modal"  id="modalLoading" tabindex="-1" role="dialog">
                                 <div class="modal-dialog modal-lg modal-dialog-centerd" role="document">
                                      <div class="overlay">
                           <div class="modalprogress">
                                   <div class="sk-fading-circle">
                                        <div class="sk-circle1 sk-circle"></div>
                                        <div class="sk-circle2 sk-circle"></div>
                                        <div class="sk-circle3 sk-circle"></div>
                                        <div class="sk-circle4 sk-circle"></div>
                                        <div class="sk-circle5 sk-circle"></div>
                                        <div class="sk-circle6 sk-circle"></div>
                                        <div class="sk-circle7 sk-circle"></div>
                                        <div class="sk-circle8 sk-circle"></div>
                                        <div class="sk-circle9 sk-circle"></div>
                                        <div class="sk-circle10 sk-circle"></div>
                                        <div class="sk-circle11 sk-circle"></div>
                                        <div class="sk-circle12 sk-circle"></div>
                                    </div>
                               <div class="text-center" id="TextProgress">Espere un momento...</div>      
                               <p></p>
                               <div class="text-center" id="TextProgress2"></div>
                            
                           </div>
                            
                        </div>
                                 </div>

                            </div>
                        
                            
                        </ProgressTemplate>
                      
                </asp:UpdateProgress> 
	<script>
		
		$('#login-button').click(function () {
			 
			$('#login-button').fadeOut('slow', function () {
			   $('#logoph').addClass('phglassopacity');
				$('#container').fadeIn();
				$('.NombreUser').focus();
            });
            
        });
        $('.close-btn').click(function () { CerrarModal(); });



/* Forgotten Password */
        $('#modalForgetPass').on('hide.bs.modal', CerrarModal());

        $('#ForgetPassBtn').click(function () {CerrarModal();});



        function Loading() {
            $('html').bind('keydown', function(e){
                    if(e.keyCode == 13 || e.keyCode==32){
                    return false;
                    }
            });
            $('#modalLoading').modal('show').css('pointer-events', 'none');

        }

        


        function CerrarModal() {
            
                    $('#logoph').removeClass('phglassopacity');
	                $("#container").fadeOut(300, function(){
			            $("#login-button").fadeIn(300);
            });
            $('html').bind('keydown', function(e){
                    
            });
            $('#modalLoading').modal('hide');
        }

        function OpenModalForeget() {
            $('#modalForgetPass').modal('show');
        }

	</script>
</asp:Content>
