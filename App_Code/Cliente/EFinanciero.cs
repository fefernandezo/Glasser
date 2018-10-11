using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de EFinanciero
/// </summary>
/// 


namespace nsCliente
{
    
    public class EFinanciero
    {
        public Utilizado CupoUtilizado { get; set; }
        public readonly double Deuda;
        public readonly double PagosNoefectivos;
        public double RiesgoPropio { get; set; }
        public double Credito { get; set; }
        public double Disponible { get; set; }
        

        Coneccion Conn;
        static SqlDataReader dr;

        private string Query { get; set; }
        private readonly string Rut;
        private readonly string Empresa;

        public EFinanciero(string _Rut, string _Empresa)
        {
            Rut = _Rut;
            Empresa = _Empresa;
            CupoUtilizado = new Utilizado {
                NVV = Math.Round(GetAmountNVV(),0),
                FCV= Math.Round(GetAmountFCV(),0),
                NCV= Math.Round(GetAmountNCV(),0),
                PedidosIng = Math.Round(GetAmountPedidos(),0),

            };
            GetRiesgoPropioNCredito();
            PagosNoefectivos =GetAmountCHEQUES();
            Deuda = CupoUtilizado.NVV + CupoUtilizado.PedidosIng +  CupoUtilizado.FCV- CupoUtilizado.NCV;


            Disponible = Credito + RiesgoPropio + PagosNoefectivos - Deuda;

            bool Is = true;
            
        }

        private void GetRiesgoPropioNCredito()
        {
            Conn = new Coneccion();
            Query = @"SELECT TOP 1 NOKOEN,DIEN,FOEN,CRTO,CRSD,CRCH,CRPA,CRLT,MOCTAEN FROM MAEEN WITH ( NOLOCK )  WHERE KOEN= @RUT";

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);
                



                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores validadores = new Validadores();
                    string Rpr = dr["CRPA"].ToString();
                    string Crtr = dr["CRTO"].ToString();
                    RiesgoPropio = Math.Round(validadores.ParseoDouble(Rpr),0);
                    Credito = Math.Round(validadores.ParseoDouble(Crtr),0);
                }
                dr.Close();
                Conn.ConnGlasser.Close();


            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut + " empresa:" + Empresa;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnGlasser.Close();
                


            }

        }

        private double GetAmountNVV()
        {
            
            Conn = new Coneccion();
            Query = @"SELECT SUM(MAEDDO.VABRLI ) " + 
                "FROM MAEDDO MAEDDO WITH ( NOLOCK )  " + 
                "INNER JOIN MAEEDO MAEEDO WITH ( NOLOCK ) ON MAEEDO.IDMAEEDO = MAEDDO.IDMAEEDO " + 
                "WHERE MAEDDO.ENDO = @RUT AND " + 
                "MAEDDO.TIDO IN ('NVV','RES','PRO','OCC','GDD','GDP','GDV','GRC','GRD','GRP') "+
                "AND MAEDDO.EMPRESA=@Empresa  AND MAEDDO.ESLIDO=' '  AND MAEDDO.LILG IN ('SI','GR')  AND MAEEDO.ESDO<>'N' ";

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);
                Conn.CmdPlabal.Parameters.AddWithValue("@Empresa", Empresa);



                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores validadores = new Validadores();
                    return validadores.ParseoDouble(dr[0].ToString());
                }
                else return 0;



                

            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut + " empresa:" + Empresa;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnGlasser.Close();
                return 0;


            }


        }

        private double GetAmountFCV()
        {
            Conn = new Coneccion();
            Query = @"SELECT sum(VABRDO) "+ 
                "FROM MAEEDO EDO WITH ( NOLOCK ) "+ 
                "WHERE EDO.ENDO = @RUT AND EDO.TIDO IN ('FCV','fcv')  AND EDO.EMPRESA=@Empresa "+
                "AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI=0 ";

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);
                Conn.CmdPlabal.Parameters.AddWithValue("@Empresa", Empresa);



                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores validadores = new Validadores();
                    return validadores.ParseoDouble(dr[0].ToString());
                }
                else return 0;



            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut + " empresa:" + Empresa;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnGlasser.Close();
                return 0;

            }


        }
        private double GetAmountNCV()
        {
            Conn = new Coneccion();
            Query = @"SELECT sum(VABRDO) "+
                "FROM MAEEDO EDO WITH ( NOLOCK ) " +
                "WHERE EDO.ENDO = @RUT AND EDO.TIDO IN  ('NCV','ncv')  AND " +
                "EDO.EMPRESA=@Empresa  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);
                Conn.CmdPlabal.Parameters.AddWithValue("@Empresa", Empresa);



                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores validadores = new Validadores();
                    return validadores.ParseoDouble(dr[0].ToString());
                }
                else return 0;



            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut + " empresa:" + Empresa;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnGlasser.Close();
                return 0;

            }


        }

        private double  GetAmountCHEQUES()
        {
            Conn = new Coneccion();
            Query = @"SELECT sum(VABRDO) "+
                "FROM MAEEDO EDO WITH ( NOLOCK ) "+
                "WHERE EDO.ENDO = @RUT AND EDO.TIDO IN  ('CHV')  AND " +
                "EDO.EMPRESA=@Empresa  AND EDO.ESPGDO = 'P'  AND EDO.ESDO<>'N'  AND EDO.NUDONODEFI = 0 ";

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);
                Conn.CmdPlabal.Parameters.AddWithValue("@Empresa", Empresa);



                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores validadores = new Validadores();
                    return Math.Round(validadores.ParseoDouble(dr[0].ToString()), 0);
                }
                else return 0;
                



            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut + " empresa:" + Empresa;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnGlasser.Close();
                return 0;
                


            }


        }


        /* Hay que arreglar esta funcion, no está devolviendo ningun valor*/
        private double GetAmountPedidos()
        {
            Conn = new Coneccion();
            Query = @"SELECT SUM (totalPedido) FROM e_Pedidos WHERE Estado='INGo' ";

            try
            {


                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);
                Conn.CmdPlabal.Parameters.AddWithValue("@Empresa", Empresa);



                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores validadores = new Validadores();
                    return validadores.ParseoDouble(dr[0].ToString());
                }
                else return 0;


            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut + " empresa:" + Empresa;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnPlabal.Close();
                return 0;


            }


        }

        public class Utilizado
        {
            public double NVV { get; set; }
            public double FCV { get; set; }
            
            public double NCV { get; set; }
            public double PedidosIng { get; set; }
        }
    }

}
