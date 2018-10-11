using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de Funciones
/// </summary>


public class INFOcliente
{
    FuncUser usuario = new FuncUser();
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnPlabal2 = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlCommand cmdGlasser;
    SqlCommand cmdPlabal;
    static SqlDataReader drPlabal;
    static SqlDataReader drGlasser;



    public int RiesgoPropio()
    {
        int valor = 0;

        SqlConnection conx;
        string consulta;
        Infousuario Datousuario = usuario.DatosUsuario();

        conx = ConnGlasser;
        ///conx = new SqlConnection(SqlDataSource1.SelectCommand);
        consulta = @"SELECT TOP 1 NOKOEN,DIEN,FOEN,CRTO,CRSD,CRCH,CRPA,CRLT,MOCTAEN FROM MAEEN WITH ( NOLOCK )  WHERE KOEN= @rut";
        SqlCommand cmd = new SqlCommand(consulta, conx);
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd.ExecuteReader();
            leerCadena.Read();

            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    valor = 0;
                }
                else
                {
                    valor = Convert.ToInt32(leerCadena[6]);
                }

            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }
        return valor;
    }

    public int CupoUtilizado()
    {
        int montoUtilizado = 0;
        int montoncr = 0;
        int montonvv = 0;
        Infousuario Datousuario = usuario.DatosUsuario();
        SqlConnection conx;
        string consulta = @"";
        conx = ConnGlasser;

        //notas de credito
        consulta = @"SELECT sum(VABRDO)   
                        FROM MAEEDO EDO WITH ( NOLOCK )  WHERE EDO.ENDO = @rut AND EDO.TIDO IN  ('NCV','ncv')  AND EDO.EMPRESA='01'  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";
        SqlCommand cmd0 = new SqlCommand(consulta, conx);
        try
        {
            cmd0.Connection.Open();
            cmd0.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd0.ExecuteReader();
            leerCadena.Read();

            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montoncr = 0;
                }
                else
                {
                    montoncr = Convert.ToInt32(leerCadena[0]);
                }
            }

            leerCadena.Close();
        }
        finally
        {
            cmd0.Connection.Close();
        }

        //notas de venta
        consulta = @"SELECT SUM(MAEDDO.VABRLI )
                        FROM MAEDDO MAEDDO WITH ( NOLOCK )  
                        INNER JOIN MAEEDO MAEEDO WITH ( NOLOCK ) ON MAEEDO.IDMAEEDO = MAEDDO.IDMAEEDO 
                        WHERE MAEDDO.ENDO = @rut AND MAEDDO.TIDO IN  ('NVV','RES','PRO','OCC','GDD','GDP','GDV','GRC','GRD','GRP')  AND MAEDDO.EMPRESA='01'  AND MAEDDO.ESLIDO=' '  AND MAEDDO.LILG IN ('SI','GR')  AND MAEEDO.ESDO<>'N' ";
        SqlCommand cmd = new SqlCommand(consulta, conx);
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd.ExecuteReader();
            leerCadena.Read();
            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montonvv = 0;
                }
                else
                {
                    montonvv = Convert.ToInt32(leerCadena[0]);
                }
            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }


        //facturas
        int montofact = 0;
        consulta = @"SELECT sum(VABRDO)   
                        FROM MAEEDO EDO WITH ( NOLOCK )  WHERE EDO.ENDO = @rut AND EDO.TIDO IN  ('FCV','fcv')  AND EDO.EMPRESA='01'  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";
        SqlCommand cmd2 = new SqlCommand(consulta, conx);
        try
        {
            cmd2.Connection.Open();
            cmd2.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd2.ExecuteReader();
            leerCadena.Read();
            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montofact = 0;
                }
                else
                {
                    montofact = Convert.ToInt32(leerCadena[0]);
                }
            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }


        //cheques
        int montochv = 0;
        consulta = @"SELECT sum(VABRDO)   
                            FROM MAEEDO EDO WITH ( NOLOCK )  WHERE EDO.ENDO = @rut AND EDO.TIDO IN  ('CHV')  AND EDO.EMPRESA='01'  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";
        SqlCommand cmd3 = new SqlCommand(consulta, conx);
        try
        {
            cmd3.Connection.Open();
            cmd3.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd3.ExecuteReader();
            leerCadena.Read();
            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montochv = 0;
                }
                else
                {
                    montochv = Convert.ToInt32(leerCadena[0]);
                }
            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }


        montoUtilizado = montonvv + montofact - montochv - montoncr;

        return montoUtilizado;
    }

    public int Credito()
    {
        int montoCredito = 0;
        Infousuario Datousuario = usuario.DatosUsuario();
        SqlConnection conx;
        string consulta = @"";
        conx = ConnGlasser;
        consulta = @"SELECT TOP 1 NOKOEN,DIEN,FOEN,CRTO,CRSD,CRCH,CRPA,CRLT,MOCTAEN From MAEEN WITH ( NOLOCK )  WHERE KOEN= @rut";
        SqlCommand cmd = new SqlCommand(consulta, conx);

        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd.ExecuteReader();
            leerCadena.Read();

            if (leerCadena.HasRows)
            {
                montoCredito = Convert.ToInt32(leerCadena[3]);
            }
            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();

        }

        return montoCredito;
    }

    public int CupoUtilizPHGlass()
    {
        int montoUtilizado = 0;
        int montoncr = 0;
        SqlConnection conx;
        Infousuario Datousuario = usuario.DatosUsuario();
        string consulta = @"";

        conx = new SqlConnection(ConfigurationManager.ConnectionStrings["FENIXPHGLASS"].ConnectionString);

        //notas de credito
        consulta = @"SELECT sum(VABRDO)   
                        FROM MAEEDO EDO WITH ( NOLOCK )  WHERE EDO.ENDO = @rut AND EDO.TIDO IN  ('NCV','ncv')  AND EDO.EMPRESA='01'  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";
        SqlCommand cmd = new SqlCommand(consulta, conx);
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd.ExecuteReader();
            leerCadena.Read();
            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montoncr = 0;
                }
                else
                {
                    montoncr = Convert.ToInt32(leerCadena[0]);
                }
            }
            leerCadena.Close();

        }
        finally
        {
            cmd.Connection.Close();
        }


        //facturas
        int montofact = 0;
        int resta = 0;
        consulta = @"SELECT sum(VABRDO), sum(VAABDO)   
                        FROM MAEEDO EDO WITH ( NOLOCK )  WHERE EDO.ENDO = @rut AND EDO.TIDO IN  ('FCV','fcv')  AND EDO.EMPRESA='01'  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";

        SqlCommand cmd2 = new SqlCommand(consulta, conx);


        try
        {
            cmd2.Connection.Open();
            cmd2.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd2.ExecuteReader();
            leerCadena.Read();

            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montofact = 0;
                }
                else
                {
                    montofact = Convert.ToInt32(leerCadena[0]);
                    resta = Convert.ToInt32(leerCadena[1]);
                }
            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }


        //cheques
        int montochv = 0;
        consulta = @"SELECT sum(VABRDO)   
                            FROM MAEEDO EDO WITH ( NOLOCK )  WHERE EDO.ENDO = @rut AND EDO.TIDO IN  ('CHV')  AND EDO.EMPRESA='01'  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";
        SqlCommand cmd3 = new SqlCommand(consulta, conx);
        try
        {
            cmd3.Connection.Open();
            cmd3.Parameters.AddWithValue("@rut", Datousuario.Rutempresa);
            SqlDataReader leerCadena = cmd3.ExecuteReader();
            leerCadena.Read();
            if (leerCadena.HasRows)
            {
                if (leerCadena[0] == DBNull.Value)
                {
                    montochv = 0;
                }
                else
                {
                    montochv = Convert.ToInt32(leerCadena[0]);
                }
            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }


        montoUtilizado = montofact - montoncr - resta;

        return montoUtilizado;
    }

    public double FactorMargen()
    {
        double valor = 0;
        Infousuario Datousuario = usuario.DatosUsuario();

        string Select = "Select Factor from PLABAL.dbo.e_TipoUsu where Rut=@rutcli";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@rutcli", Datousuario.Rutempresa);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            valor = Convert.ToDouble(drPlabal[0].ToString());
        }
        else
        {

        }

        drPlabal.Close();
        ConnPlabal.Close();



        return valor;
    }

    public void Actualizarestado(string Idempresa)
    {
        DataTable Datos = new DataTable();
        string Select = "Select P.PedidoAlfak, P.Estado, N.Nventa " +
            "FROM PLABAL.dbo.NVENTA N, PLABAL.dbo.e_pedidos P, PLABAL.dbo.e_Usuario U " +
            "WHERE P.id_usuario=U.ID AND N.OT=P.PedidoAlfak AND P.Estado IN ('ING','PRG','DIS')  AND U.Id_Tipo =@Idempresa";
        using (ConnPlabal2)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal2);
                adapter.SelectCommand.Parameters.AddWithValue("@Idempresa", Idempresa);
                adapter.Fill(Datos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        foreach (DataRow Row in Datos.Rows)
        {
            string Pedido = Row[0].ToString().Trim();
            string estado = Row[1].ToString().Trim();
            string NVV = Row[2].ToString().Trim();
            int Des = 0;
            int Bod = 0;
            string Fab = "";

            if (NVV != "")
            {
                string consulta = @"Ecomm_Traz_doc @nudo";
                ConnGlasser.Open();
                cmdGlasser = new SqlCommand(consulta, ConnGlasser);
                cmdGlasser.Parameters.AddWithValue("@nudo", NVV);
                drGlasser = cmdGlasser.ExecuteReader();
                drGlasser.Read();
                if (drGlasser.HasRows)
                {
                    Des = Convert.ToInt32(drGlasser[4].ToString());
                    Bod = Convert.ToInt32(drGlasser[3].ToString());
                    Fab = drGlasser[4].ToString();


                }
                drGlasser.Close();
                ConnGlasser.Close();
                if (Des > 0)
                {
                    UpdateActualizarEstado(Pedido, "DES");
                }
                else if (Bod > 0)
                {
                    UpdateActualizarEstado(Pedido, "DIS");
                }
                else if (Fab != "")
                {
                    UpdateActualizarEstado(Pedido, "PRG");
                }


            }


        }

    }

    public void UpdateActualizarEstado(string Pedido, string Estado)
    {

        ConnPlabal.Open();
        string Update = "UPDATE PLABAL.dbo.e_pedidos SET Estado = @Estado WHERE PedidoAlfak = @Pedido ";
        cmdPlabal = new SqlCommand(Update, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@Estado", Estado);
        cmdPlabal.Parameters.AddWithValue("@Pedido", Pedido);
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();

    }

    public Montos Montos()
    {
        FuncUser usuario = new FuncUser();
        Montos datos = new Montos();
        Infousuario datouser = usuario.DatosUsuario();

        datos.Credit = Convert.ToDouble(Credito() + RiesgoPropio() + datouser.RiesgoPRecom);
        datos.Utilizado = Convert.ToDouble(CupoUtilizado());
        datos.Disponible = datos.Credit - datos.Utilizado;

        return datos;
    }
}

public class Montos
{
    public double Credit { get; set; }
    public double Utilizado { get; set; }
    public double Disponible { get; set; }
}