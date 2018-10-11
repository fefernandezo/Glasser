<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error-Pass-Form.aspx.cs" MasterPageFile="~/Account/SiteLogin.Master" Inherits="Account_ErrorAplicacion" %>
<asp:Content runat="server" ID="HeaderContent" ContentPlaceHolderID="FeaturedContent" >
	<style>
	
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
  opacity: 0.6;
  box-shadow: 10px 10px 30px #000;}
#login-button:hover{
	opacity:0.8;
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
			border-radius: 5px;
			background: rgba(0, 21, 90, 0.55);
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
h3{
  font-family: 'Open Sans Condensed', sans-serif;
  position: relative;
  margin-top: 0px;
  text-align: center;
  font-size: 30px;
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

    </asp:Content>
   
