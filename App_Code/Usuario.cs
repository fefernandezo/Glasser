using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Security;
using GlobalInfo;

/// <summary>
/// Descripción breve de Usuario
/// </summary>
/// 

public class FuncUser : System.Web.UI.Page
{
    
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
    SqlCommand cmdPlabal;
    SqlCommand cmdAlfak;
    static SqlDataReader drPlabal;
    static SqlDataReader drAlfak;


    public int AppValidacion ()
    {
        string user = Context.User.Identity.Name;
        int valor = 0;
        string Select = "SELECT * FROM PLABAL.dbo.UsersInApp I, PLABAL.dbo.Users U WHERE I.UserId=U.UserId AND UserName = @User AND IDaplicacion=@IDApp ";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@User", user);
        cmdPlabal.Parameters.AddWithValue("@IDApp", 1);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            valor = 1;
        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();


        return valor;
    }

    public DataTable ListaPedidos(string Estado)
    {

        FuncUser Usu = new FuncUser();
        Infousuario DatosCliente = Usu.DatosUsuario();
        string Select = "SELECT e_Pedidos.nom_pedido AS 'Nombre', e_Pedidos.PedidoAlfak AS 'Numero', e_Pedidos.fecha_hora_pedido AS 'FechaIng', CONVERT(varchar,e_Pedidos.fecha_entrega,105) AS 'FechaEntr'," +
            " e_Pedidos.tipo_despacho AS 'TipoDes', e_Pedidos.observacion AS 'Observa'," +
            " e_Usuario.Usuario AS 'Usuario', e_Pedidos.Cantidad AS 'Cantidad', CAST(convert(int,ROUND(e_Pedidos.totalPedido,0)) AS VARCHAR) AS 'Total', e_Pedidos.Estado AS 'Estado'" +
            " FROM e_Pedidos INNER JOIN e_Usuario ON e_Pedidos.id_usuario = e_Usuario.ID" +
            " INNER JOIN e_TipoUsu ON e_Usuario.Id_Tipo = e_TipoUsu.ID" +
            " WHERE (e_TipoUsu.Rut = @rut) AND (e_Pedidos.Estado = @Estado)";
        DataTable ListaTotal = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                adapter.SelectCommand.Parameters.AddWithValue("@rut", DatosCliente.Rutempresa);
                adapter.SelectCommand.Parameters.AddWithValue("@Estado", Estado);
                adapter.Fill(ListaTotal);



            }
            finally
            {

            }
            

        }




        return ListaTotal;
    }

    public Infousuario DatosUsuario()
    {
        Infousuario Info;

        if (User.IsInRole("CLIENTE_ECOMMERCE"))
        {
            string Select = "Select U.UserId,T.ID, U.Nombre , U.Apellido , M.Email, T.Empresa, T.Rut, T.Factor, T.FactorRiesgo,T.TipoCliente,T.TipoxlsTP " +
            "FROM PLABAL.dbo.Users U, PLABAL.dbo.e_UserInEmp UIE, PLABAL.dbo.e_TipoUsu T, PLABAL.dbo.Memberships M" +
            " WHERE U.UserName = @User and U.UserId = UIE.UserId and UIE.IDTipoUsu= T.ID and U.UserId=M.UserId ";

            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@User", Page.User.Identity.Name);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                Info = new Infousuario
                {
                    Id = drPlabal["UserId"].ToString(),
                    IdEmpresa = drPlabal["ID"].ToString(),
                    Nombre = drPlabal["Nombre"].ToString(),
                    Apellido = drPlabal["Apellido"].ToString(),
                    Correo = drPlabal["Email"].ToString(),
                    Empresa = drPlabal["Empresa"].ToString(),
                    Rutempresa = drPlabal["Rut"].ToString(),
                    FactorMargen = drPlabal["Factor"].ToString(),
                    RiesgoPRecom = Convert.ToDouble(drPlabal["FactorRiesgo"].ToString()),
                    _TipoxlsTP = Convert.ToInt32(drPlabal["TipoxlsTP"].ToString()),
                    _TipoCliente = Convert.ToInt32(drPlabal["TipoCliente"].ToString()),
                };


            }
            else
            {
                Info = null;
            }
            drPlabal.Close();
            ConnPlabal.Close();
        }
        else
        {
            string Select = "Select U.UserId, U.Nombre + ' ' + U.Apellido, M.Email" +
           " FROM PLABAL.dbo.Users U, PLABAL.dbo.Memberships M" +
           " WHERE U.UserName = @User and U.UserId=M.UserId ";

            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@User", Page.User.Identity.Name);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                Info = new Infousuario
                {
                    Id = drPlabal[0].ToString(),
                    IdEmpresa = "",
                    Nombre = drPlabal[1].ToString(),
                    Correo = drPlabal[2].ToString(),
                    Empresa = "",
                    Rutempresa = "",
                    FactorMargen = "",
                    RiesgoPRecom = 0
                };


            }
            else
            {
                Info = null;
            }
            drPlabal.Close();
            ConnPlabal.Close();
        }
        
        

        return Info;
    }

    public Cliente DatosCliAlfak()
    {
        Infousuario Info = new Infousuario();
        Cliente cli = new Cliente();
        Info = DatosUsuario();
        string SelectAlfak = "SELECT TOP 1 ID,MCODE,NAME1, UST_ID, STRASSE, TLF1, MAIL, ZAHLBED, ADIENST  FROM SYSADM.KU_KUNDEN  WHERE UST_ID = @rut";
        ConnAlfak.Open();
        cmdAlfak = new SqlCommand(SelectAlfak, ConnAlfak);
        cmdAlfak.Parameters.AddWithValue("@rut", Info.Rutempresa);
        drAlfak = cmdAlfak.ExecuteReader();
        drAlfak.Read();

        if (drAlfak.HasRows)
        {
            cli.IdCliente = Convert.ToInt32(drAlfak[0]);
            cli.Mcode = Convert.ToString(drAlfak[1]);
            cli.NombreCliente = Convert.ToString(drAlfak[2]);
            cli.Rut = Convert.ToString(drAlfak[3]);
            cli.Direccion = Convert.ToString(drAlfak[4]);
            cli.Telefono = Convert.ToString(drAlfak[5]);
            cli.Correo = Convert.ToString(drAlfak[6]);
            cli.TipoPago = Convert.ToString(drAlfak[7]);
            cli.Vendedor = Convert.ToString(drAlfak[8]);
        }
        else
        {

        }
        drAlfak.Close();
        ConnAlfak.Close();

        return cli;
    }
}

public class Usuario : Page
{
    Coneccion Cn;
    static SqlDataReader drPlabal;
    public Infousuario Info { get; set; }

    public Usuario()
    {
        
        Cn = new Coneccion();
        

        if (Page.User.IsInRole("CLIENTE_ECOMMERCE"))
        {
            string Select = "Select U.UserId,T.ID, U.Nombre , U.Apellido , M.Email, T.Empresa, T.Rut, T.Factor, T.FactorRiesgo,T.TipoCliente,T.TipoxlsTP " +
            "FROM PLABAL.dbo.Users U, PLABAL.dbo.e_UserInEmp UIE, PLABAL.dbo.e_TipoUsu T, PLABAL.dbo.Memberships M" +
            " WHERE U.UserName = @User and U.UserId = UIE.UserId and UIE.IDTipoUsu= T.ID and U.UserId=M.UserId ";

            Cn.ConnPlabal.Open();
            Cn.CmdPlabal = new SqlCommand(Select, Cn.ConnPlabal);
            Cn.CmdPlabal.Parameters.AddWithValue("@User", Page.User.Identity.Name);
            drPlabal = Cn.CmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                Info = new Infousuario
                {
                    Id = drPlabal["UserId"].ToString(),
                    IdEmpresa = drPlabal["ID"].ToString(),
                    Nombre = drPlabal["Nombre"].ToString(),
                    Apellido = drPlabal["Apellido"].ToString(),
                    Correo = drPlabal["Email"].ToString(),
                    Empresa = drPlabal["Empresa"].ToString(),
                    Rutempresa = drPlabal["Rut"].ToString(),
                    FactorMargen = drPlabal["Factor"].ToString(),
                    RiesgoPRecom = Convert.ToDouble(drPlabal["FactorRiesgo"].ToString()),
                    _TipoxlsTP = Convert.ToInt32(drPlabal["TipoxlsTP"].ToString()),
                    _TipoCliente=Convert.ToInt32(drPlabal["TipoCliente"].ToString()),
                };


            }
            else
            {
                Info = null;
            }
            drPlabal.Close();
            Cn.ConnPlabal.Close();
        }
        else
        {
            string Select = "Select U.UserId, U.Nombre, U.Apellido, M.Email" +
           " FROM PLABAL.dbo.Users U, PLABAL.dbo.Memberships M" +
           " WHERE U.UserName = @User and U.UserId=M.UserId ";

            Cn.ConnPlabal.Open();
            Cn.CmdPlabal = new SqlCommand(Select, Cn.ConnPlabal);
            Cn.CmdPlabal.Parameters.AddWithValue("@User", Page.User.Identity.Name);
            drPlabal = Cn.CmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                Info = new Infousuario
                {
                    Id = drPlabal["UserId"].ToString(),
                    IdEmpresa = "",
                    Nombre = drPlabal["Nombre"].ToString(),
                    Apellido= drPlabal["Apellido"].ToString(),
                    Correo = drPlabal["Email"].ToString(),
                    Empresa = "",
                    Rutempresa = "",
                    FactorMargen = "",
                    RiesgoPRecom = 0
                };


            }
            else
            {
                Info = null;
            }
            drPlabal.Close();
            Cn.ConnPlabal.Close();
        }

    }
   
}

public class Cliente
{
    public int IdCliente { get; set; }
    public string Rut { get; set; }
    public string NombreCliente { get; set; }
    public string Mcode { get; set; }
    public string Direccion { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string TipoPago { get; set; }
    public string Tour { get; set; }
    public string Vendedor { get; set; }
}


public class Infousuario
{
    public string Id { get; set; }
    public string IdEmpresa { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string FactorMargen { get; set; }
    public string Rutempresa { get; set; }
    public string Empresa { get; set; }
    public double RiesgoPRecom { get; set; }
    public int _TipoxlsTP { get; set; }
    public int _TipoCliente { get; set; }
}
