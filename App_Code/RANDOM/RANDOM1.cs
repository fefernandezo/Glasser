using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PlanificacionOT;
using System.Data;
using RandomERP.Kocrypt;
using System.Collections;

/// <summary>
/// Descripción breve de RANDOM1
/// </summary>
namespace RandomERP
{
    public class ProcedimientosAlm
    {
        public static class Detalle_Pedido
        {
            public const string Name = "Ecomm_Detalle_Pedido @NRO";
            public static class Variables
            {
                public const string NRO = "NRO";
            }
            
        }

        
    }

    namespace Kocrypt
    {
        public class GenKocrypt
        {
            Coneccion Conn;
            static SqlDataReader dr;

            public bool IsSuccess { get; set; }
            public string CODIGO { get; set; }

            private string Cod1 { get; set; }
            private string Cod2 { get; set; }
            private string Cod3 { get; set; }
            private string Cod4 { get; set; }
            private string Cod5 { get; set; }
            private string Cod6 { get; set; }
            

            public GenKocrypt(string TIDO, string NUDO)
            {
                DATOSin NUDODesint = DesintegrarNudo(TIDO, NUDO);
                Cod1 = GetCode(CAMPOS.COD_TIDO, CAMPOS.TIDO, NUDODesint.TIDO);
                Cod2 = GetCode(CAMPOS.COD_PREF, CAMPOS.PREF, NUDODesint.PREF);
                Cod3 = GetCode(CAMPOS.COD_NUM_8, CAMPOS.NUM_8, NUDODesint.NUM_8);
                Cod4 = GetCode(CAMPOS.COD_NUM_6, CAMPOS.NUM_6, NUDODesint.NUM_6);
                Cod5 = GetCode(CAMPOS.COD_NUM_4, CAMPOS.NUM_4, NUDODesint.NUM_4);
                Cod6 = GetCode(CAMPOS.COD_NUM_2, CAMPOS.NUM_2, NUDODesint.NUM_2);

                if (!string.IsNullOrWhiteSpace(Cod1) && !string.IsNullOrWhiteSpace(Cod2)&& !string.IsNullOrWhiteSpace(Cod3))
                {
                    if (!string.IsNullOrWhiteSpace(Cod4) && !string.IsNullOrWhiteSpace(Cod5) && !string.IsNullOrWhiteSpace(Cod6))
                    {
                        DATOSout CODEDesint = new DATOSout
                        {
                            COD_TIDO = Cod1,
                            COD_PREF = Cod2,
                            COD_NUM_8 = Cod3,
                            COD_NUM_6 = Cod4,
                            COD_NUM_4 = Cod5,
                            COD_NUM_2 = Cod6,

                        };
                        CODIGO = CODEDesint.COD_TIDO + CODEDesint.COD_PREF + CODEDesint.COD_NUM_8 + CODEDesint.COD_NUM_6 + CODEDesint.COD_NUM_4 + CODEDesint.COD_NUM_2;
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }

                }
                else
                {
                    IsSuccess = false;
                }

            }


            private DATOSin DesintegrarNudo(string _TIDO,string NUDO)
            {
                
                DATOSin datos = new DATOSin {
                    TIDO = _TIDO,
                    PREF = NUDO.Substring(0, 3),
                    NUM_8 = NUDO.Substring(3, 1),
                    NUM_6 = NUDO.Substring(4, 2),
                    NUM_4 = NUDO.Substring(6, 2),
                    NUM_2 = NUDO.Substring(8, 2),

                };

                return datos;
            }

            private string GetCode(string CampoRetorno, string CampoConsulta, string Valor)
            {
                string retorno;
                string Select = @"SELECT " + CampoRetorno +" FROM GLA_KOCRYPT WHERE " + CampoConsulta + "=@Valor ";
                Conn = new Coneccion();


                try
                {


                    Conn.ConnPlabal.Open();
                    Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
                    Conn.CmdPlabal.Parameters.AddWithValue("@Valor", Valor);
                    
                    dr = Conn.CmdPlabal.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        retorno = dr[0].ToString();
                        
                    }
                    else
                    {
                        
                        retorno = " ";
                        
                    }


                    dr.Close();
                    Conn.ConnPlabal.Close();

                }
                catch (Exception ex)
                {
                    retorno = " ";
                    throw ex;
                    
                }

                return retorno;
            }




            private class DATOSin
            {
                public string TIDO { get; set; }
                
                public string PREF { get; set; }
                
                public string NUM_8 { get; set; }
                
                public string NUM_6 { get; set; }
                
                public string NUM_4 { get; set; }
               
                public string NUM_2 { get; set; }
                

            }

            private class DATOSout
            {
               
                public string COD_TIDO { get; set; }
                
                public string COD_PREF { get; set; }
                
                public string COD_NUM_8 { get; set; }
               
                public string COD_NUM_6 { get; set; }
                
                public string COD_NUM_4 { get; set; }
                
                public string COD_NUM_2 { get; set; }

            }

            private static class CAMPOS
            {
                public static string TIDO = "TIDO";
                public static string COD_TIDO = "COD_TIDO";
                public static string PREF = "PREF";
                public static string COD_PREF = "COD_PREF";
                public static string NUM_8 = "NUM_8";
                public static string COD_NUM_8 = "COD_NUM_8";
                public static string NUM_6 = "NUM_6";
                public static string COD_NUM_6 = "COD_NUM_6";
                public static string NUM_4 = "NUM_4";
                public static string COD_NUM_4 = "COD_NUM_4";
                public static string NUM_2 = "NUM_2";
                public static string COD_NUM_2 = "COD_NUM_2";

            }
        }

        
    }

    namespace ConsumosPPR
    {
        public class GetTop40OtSinConsumo
        {
            public List<OTsinConsumo> Lista { get; set; }

            public bool HasOts { get; set; }

            Coneccion Conn;

            public GetTop40OtSinConsumo()
            {
                GetLista();
            }

            public void GetLista()
            {

                Lista = new List<OTsinConsumo>();


                string Select = "SELECT TOP 40 POTE.NUMOT, POTE.FIOT,POTE.IDPOTE,POTE.REFERENCIA, POTE.TIPOOT,COUNT(POTD.IDPOTD) AS 'ITEMS' " +
                    "FROM POTE, POTD WITH(NOLOCK) " +
                    "WHERE POTD.NUMOT = POTE.NUMOT AND POTD.CANTIDADF > 0 AND POTD.NIVEL = '1' AND POTE.FIOT >= '2018-09-01' AND " + 
                    "POTD.IDPOTD NOT IN(SELECT IDRST FROM MAEDDO WHERE ARCHIRST = 'POTD' AND POTD.IDPOTD = IDRST) " +
                    "GROUP BY POTE.NUMOT, POTE.FIOT,POTE.IDPOTE,POTE.REFERENCIA, POTE.TIPOOT ORDER BY POTE.FIOT ASC";

                Conn = new Coneccion();

                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnGlasser)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnGlasser);
                        adaptador.Fill(TablaDetalle);
                        Conn.ConnGlasser.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        
                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {
                    HasOts = true;
                    foreach (DataRow drr in TablaDetalle.Rows)
                    {
                        Validadores val = new Validadores();
                        OTsinConsumo item = new OTsinConsumo
                        {
                            IDPOTE = drr["IDPOTE"].ToString(),
                            _NUMOT = drr["NUMOT"].ToString(),
                            _REFERENCIA = drr["REFERENCIA"].ToString(),
                            _FIOT = val.ParseoDateTime(drr["FIOT"].ToString()),
                            _TIPOOT = drr["TIPOOT"].ToString(),
                            _ITEMS = Convert.ToInt32(drr["ITEMS"].ToString()),

                        };
                        Lista.Add(item);
                        

                    }
                }
                else
                {
                    HasOts = false;
                }

                
            }


            public class OTsinConsumo
            {
                public string IDPOTE { get; set; }
                public string _NUMOT { get; set; }
                public string _REFERENCIA { get; set; }
                public DateTime _FIOT { get; set; }
                public string _TIPOOT { get; set; }
                public int _ITEMS { get; set; }
            }

        }

        public class GenerarMAEDDOyMAEEDO
        {

            private static string _IDMAEEDO;
            private static string _EMPRESA;
            private static string _TIDO;
            private static string _NUDO;

            public List<Tablas.MAEDDO> ListItems { get; set; }

            public Tablas.MAEEDO MAEEDO { get; set; }

            Coneccion Conn;
            static SqlDataReader dr;

            public GenerarMAEDDOyMAEEDO()
            {
                

            }
            public bool[] LuzCamaraAccion()
            {

                //update maeedo
                Tablas.UpdateMAEEDO Update = new Tablas.UpdateMAEEDO(MAEEDO);
                bool Isupdate = Update.IsUpdated;
                bool IsInsert = false;
                int Cont = 1;
                foreach (var item in ListItems)
                {
                    Tablas.InsertInMAEDDO Maeddo;
                    VerStockMaterial verStock = new VerStockMaterial(item.EMPRESA, item.SULIDO, item.BOSULIDO, item.KOPRCT,item.CAPRCO1);
                    bool Insertar = false;
                    if (!verStock.HasStock)
                    {
                        if (verStock.IsAlternativa)
                        {
                            item.KOPRCT = verStock.Datos.CODIGO;
                            //Insertar = true;
                        }
                        
                    }
                    else
                    {
                        Insertar = true;
                    }
                    if (Insertar)
                    {
                        string _NULIDO = Cont.ToString();
                        //int tr = Cont.ToString().Length;

                        
                        //for (int we = 0; we < 5 - tr; we++)
                        //{
                            //_NULIDO = "0" + _NULIDO;
                        //}
                        //item.NULIDO = _NULIDO;
                        item.NULIDO = _NULIDO.PadLeft(5, '0');
                        Maeddo = new Tablas.InsertInMAEDDO(item);

                        if (Maeddo.IsInserted)
                        {
                            //rebajar stock 
                            RebajarStock(item);
                            Cont++;

                        }
                        else
                        {
                            IsInsert = false;
                        }
                    }
                    
                    
                }
                if (Cont>1)
                {
                    IsInsert = true;
                }
                else
                {
                    IsInsert = false;
                }
                bool[] retorno = new bool[2];
                retorno[0] = Isupdate;
                retorno[1] = IsInsert;
                if (IsInsert)
                {
                    UpdateMAEEDObi();
                }

                return retorno;
            }

            private void UpdateMAEEDObi()
            {
                double[] values = gETvaluesFRMAEDDO();
                double _CAPRCO1 = values[0];
                double _CAPREX1 = values[1];
                double _VAIVLI = Math.Round(values[2],0);
                double _VANELI = Math.Round(values[3],0);
                double _VABRLI = Math.Round(values[4],0);

                #region Query
                string Query = "UPDATE MAEEDO SET CAPRCO=@CAPRCO1,CAPREX=@CAPREX1,VAIVDO=@VAIVLI,VANEDO=@VANELI,VABRDO=@VABRDO WHERE" +
                    " IDMAEEDO=@IDMAEEDO";
                #endregion

                Conn = new Coneccion();
                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnGlasser);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRCO1", _CAPRCO1);
                    Conn.Cmd.Parameters.AddWithValue("@CAPREX1", _CAPREX1);
                    Conn.Cmd.Parameters.AddWithValue("@VAIVLI", _VAIVLI);
                    Conn.Cmd.Parameters.AddWithValue("@VANELI", _VANELI);
                    Conn.Cmd.Parameters.AddWithValue("@VABRDO", _VABRLI);
                    Conn.Cmd.Parameters.AddWithValue("@IDMAEEDO", MAEEDO.IDMAEEDO);

                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnGlasser.Close();


                }
                catch
                {
                    string ERRORSTR = "Error al tratar de actualizar MAEEDO SET CAPRCO=" + _CAPRCO1+ ",CAPREX=" + _CAPREX1 + ",VAIVDO=" + _VAIVLI + ",VANEDO=" + _VANELI + ",VABRDO=" + _VABRLI + " WHERE" +
                    " IDMAEEDO="+MAEEDO.IDMAEEDO;
                    ErrorCatching gETerror = new ErrorCatching();
                    gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                    Conn.ConnGlasser.Close();


                }

            }

            private double[] gETvaluesFRMAEDDO()
            {
                double[] valores = new double[5];

                string Select = "SELECT SUM(CAPRCO1),SUM(CAPREX1), ROUND(SUM(VAIVLI),0), SUM(VANELI), SUM(VABRLI) FROM MAEDDO WHERE IDMAEEDO=@IDMAEEDO";
                Conn = new Coneccion();

                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                    Conn.CmdPlabal.Parameters.AddWithValue("@IDMAEEDO", MAEEDO.IDMAEEDO);
                    
                    dr = Conn.CmdPlabal.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores val = new Validadores();
                        for (int i = 0; i < 5; i++)
                        {
                            valores[i] = val.ParseoDouble(dr[i].ToString());
                        }

                        dr.Close();
                        Conn.ConnGlasser.Close();

                    }
                    else
                    {
                        dr.Close();
                        Conn.ConnGlasser.Close();



                    }

                }
                catch (Exception ex)
                {

                    throw ex;

                }



                return valores;
            }

            public void GetMAEEDO(Tablas.MAEEDO _mAEEDO)
            {
                MAEEDO = _mAEEDO;
                _IDMAEEDO = _mAEEDO.IDMAEEDO;
                _EMPRESA = _mAEEDO.EMPRESA;
                _TIDO = _mAEEDO.TIDO;
                _NUDO = _mAEEDO.NUDO;
                ListItems = new List<Tablas.MAEDDO>();

            }

            public void AddItemMAEDDO(DetalleConsumo detalle)
            {
                Tablas.MAEDDO mAEDDO;
                string _ENDO = "76829725-8";
                GetDatosMAEPR _MAEPR = new GetDatosMAEPR(detalle._CODIGO);
                Validadores vAL = new Validadores();
                double cANT2 = Math.Round(detalle._CANTIDADF/_MAEPR.Datos.RLUD, 3);
                double _PPPRBRLT = Math.Round(_MAEPR.Datos.PMIFRS*(1 + (_MAEPR.Datos.POIVPR/100)),5);
                double _VAIVLI = Math.Round(_MAEPR.Datos.PMIFRS * (_MAEPR.Datos.POIVPR / 100),2)* detalle._CANTIDADF;
                double _VANELI = Math.Round(_MAEPR.Datos.PMIFRS * detalle._CANTIDADF, 0);
                mAEDDO = new Tablas.MAEDDO {
                    IDMAEEDO = _IDMAEEDO,
                    EMPRESA = _EMPRESA,
                    TIDO = _TIDO,
                    NUDO = _NUDO,
                    NUDOPA = detalle._NUMOT,
                    ARCHIRST = "POTD",
                    ENDO = _ENDO,
                    SUENDO = "",
                    ENDOFI = "",
                    LILG = "SI",
                    SULIDO = detalle._SUOT,
                    LUVTLIDO = "",
                    BOSULIDO = "PPR",
                    KOFULIDO = detalle._KOFUCRE,
                    NULILG = "",
                    PRCT = false,
                    TICT = "",
                    TIPR = detalle._TIPO,
                    NUSEPR = "",
                    KOPRCT = detalle._CODIGO,
                    UDTRPR = vAL.ParseoDouble(detalle._TIPOUNI),
                    RLUDPR = _MAEPR.Datos.RLUD,
                    CAPRCO1 = Math.Round(detalle._CANTIDADF,5),
                    CAPRAD1 = 0,
                    CAPREX1 = detalle._CANTIDADF,
                    CAPRNC1 = 0,
                    UD01PR = detalle._UNIDAD,
                    CAPRCO2 = cANT2,
                    CAPRAD2 = 0,
                    CAPREX2 = cANT2,
                    CAPRNC2 = 0,
                    UD02PR = detalle._UNIDADC,
                    KOLTPR = "TABPP02C",
                    MOPPPR = detalle._MODO,
                    TIMOPPPR = detalle._TIMODO,
                    TAMOPPPR = 0,
                    PPPRNELT = Math.Round(_MAEPR.Datos.PMIFRS,5),
                    PPPRNE = Math.Round(_MAEPR.Datos.PMIFRS,5),
                    PPPRBRLT = _PPPRBRLT,
                    PPPRBR = _PPPRBRLT,
                    NUDTLI = 0,
                    PODTGLLI = 0,
                    VADTNELI = 0,
                    VADTBRLI = 0,
                    POIVLI = _MAEPR.Datos.POIVPR,
                    VAIVLI = Math.Round(_VAIVLI,2),
                    NUIMLI = 0,
                    POIMGLLI = 0,
                    VAIMLI = 0,
                    VANELI = Math.Round(_VANELI,0),
                    VABRLI = Math.Round(_PPPRBRLT * detalle._CANTIDADF,0),
                    TIGELI = "E",
                    EMPREPA = _EMPRESA,
                    TIDOPA = "OTD",
                    ENDOPA = _ENDO,
                    NULIDOPA = detalle._NIVELSUP,
                    LLEVADESP = 0,
                    FEEMLI = DateTime.Now,
                    FEERLI = DateTime.Now,
                    PPPRPM = Math.Round(_MAEPR.Datos.PMIFRS,5),
                    OPERACION = detalle._OPERACION,
                    CODMAQ = detalle._CODMAQ,
                    ESLIDO = "C",
                    PPPRNERE1 = Math.Round(_VANELI / detalle._CANTIDADF,2),
                    PPPRNERE2 = Math.Round(_VANELI / detalle._CANTIDADF, 2),
                    ESFALI = "",
                    CAFACO = 0,
                    CAFAAD = 0,
                    CAFAEX = 0,
                    CMLIDO = 0,
                    NULOTE = "",
                    FVENLOTE = vAL.ParseoDateTime("0"),
                    ARPROD="",
                    NULIPROD= detalle._SUBNREG,
                    NUCOCO="",
                    NULICO="",
                    FCRELOTE = vAL.ParseoDateTime("0"),
                    SUBLOTE="",
                    NOKOPR=_MAEPR.Datos.NOKOPR,
                    ALTERNAT="",
                    PRENDIDO=false,
                    OBSERVA="",
                    KOFUAULIDO="",
                    KOOPLIDO="",
                    MGLTPR=0,
                    PPOPPR=0,
                    TIPOMG=0,
                    ESPRODLI="",
                    CAPRODCO=0,
                    CAPRODAD=0,
                    CAPRODEX=0,
                    CAPRODRE=0,
                    TASADORIG=0,
                    CUOGASDIF="0",
                    SEMILLA="0",
                    PROYECTO="",
                    POTENCIA=0,
                    HUMEDAD=0,
                    IDTABITPRE=0,
                    IDODDGDV="0",
                    LINCONDESP=true,
                    PODEIVLI=0,
                    VADEIVLI=0,
                    PRIIDETIQ="0",
                    KOLORESCA="",
                    KOENVASE="",
                    PPPRPMSUC = Math.Round(detalle._PPPRPMSUC,5),
                    PPPRPMIFRS =Math.Round(_MAEPR.Datos.PMIFRS,5),
                    COSTOTRIB= _VANELI,
                    COSTOIFRS=_VANELI,
                    SUENDOFI="",
                    COMISION=0,
                    FLUVTCALZA="",
                    FEERLIMODI=DateTime.Now,
                    IDRST= detalle._IDPOTD,
                    PERIODO="",

                };

                ListItems.Add(mAEDDO);
            }

            public void RebajarStock(Tablas.MAEDDO item)
            {
                object[] Param = new object[] { item.CAPRCO1, item.CAPRCO2, item.EMPRESA,item.SULIDO,item.KOPRCT,item.BOSULIDO,item.IDRST,item.CAPRCO1 };
                string[] ParamName = new string[] {"STFI1", "STFI2","EMPRESA","KOSU","KOPR","KOBO","IDPOTD","CANTIDADR" };

                UpdateRow update = new UpdateRow("GLASSER",
                    "BEGIN TRANSACTION; UPDATE MAEPMSUC SET STFI1=STFI1-@STFI1,STFI2=STFI2-@STFI2 WHERE EMPRESA=@EMPRESA AND KOSU=@KOSU AND KOPR=@KOPR; " +
                    "UPDATE MAEPR SET STFI1=STFI1-@STFI1,STFI2=STFI2-@STFI2,STREQFAB1=STREQFAB1-@STFI1,STREQFAB2=STREQFAB2-@STFI2 WHERE KOPR=@KOPR; " +
                    "UPDATE MAEPREM SET STFI1=STFI1-@STFI1,STFI2=STFI2-@STFI2,STREQFAB1=STREQFAB1-@STFI1,STREQFAB2=STREQFAB2-@STFI2 WHERE EMPRESA=@EMPRESA AND KOPR=@KOPR; " +
                    "UPDATE MAEST SET STFI1=STFI1-@STFI1,STFI2=STFI2-@STFI2,STREQFAB1=STREQFAB1-@STFI1,STREQFAB2=STREQFAB2-@STFI2 WHERE EMPRESA=@EMPRESA AND KOSU=@KOSU AND KOBO=@KOBO AND KOPR=@KOPR; " +
                    "UPDATE POTD SET CANTIDADR=CANTIDADR + @CANTIDADR WHERE IDPOTD=@IDPOTD; COMMIT;", Param, ParamName);

                if (!update.Actualizado)
                {
                    string ERRORSTR = "error en RebajaStock() KOPR=" + item.KOPRCT +" &NUDO=" + item.NUDO;
                    ErrorCatching gETerror = new ErrorCatching();
                    gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());

                }

               

            }

            
        }

        public class DetalleConsumo
        {
            
            public string _NUMOT { get; set; }
            public string _CODIGO { get; set; }
            public double _CANTIDADF { get; set; }
            public string _UNIDADC { get; set; }
            public string _TIPO { get; set; }
            public string _NIVELSUP { get; set; }
            public string _SUBNREG { get; set; }
            public string _OPERACION { get; set; }
            public string _SUOT { get; set; }
            public string _KOFUCRE { get; set; }
            public string _IDPOTD { get; set; }
            public string _IDPOTL { get; set; }
            public string _MODO { get; set; }
            public string _TIMODO { get; set; }
            public string _TIPOUNI { get; set; }
            public string _UNIDAD { get; set; }
            public double _TAMODO { get; set; }
            public double _PPPRPMSUC { get; set; }
            public string _CODMAQ { get; set; }






        }

        public class GetPMSUCyCODMAQ
        {
            public double PMSUC;
            public string CODMAQ;

            Coneccion Conn;
            SqlDataReader dr;
            public GetPMSUCyCODMAQ(string KOPR,string KOSU, string IDPOTL, string OPERACION, string EMPRESA)
            {
                GetPmSuc(KOPR, KOSU, EMPRESA);
                GetCodMaq(IDPOTL, OPERACION);

            }
            private void GetPmSuc(string KOPR, string KOSU, string EMPRESA)
            {

                string Select = "SELECT TOP 1 PMSUC FROM MAEPMSUC WHERE KOPR=@KOPR AND EMPRESA=@EMPRESA AND KOSU=@KOSU";
                Conn = new Coneccion();

                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                    Conn.CmdPlabal.Parameters.AddWithValue("@KOPR", KOPR);
                    Conn.CmdPlabal.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                    Conn.CmdPlabal.Parameters.AddWithValue("@KOSU", KOSU);
                    dr = Conn.CmdPlabal.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {

                        Validadores vAL = new Validadores();
                        PMSUC= vAL.ParseoDouble(dr[0].ToString());
                        dr.Close();
                        Conn.ConnGlasser.Close();

                    }
                    else
                    {
                        dr.Close();
                        Conn.ConnGlasser.Close();
                        


                    }




                }
                catch (Exception ex)
                {

                    throw ex;

                }



            }

            private void GetCodMaq(string IDPOTL, string OPERACION)
            {

                string Select = "select TOP 1 CODMAQOT from POTPR WHERE IDPOTL=@IDPOTL AND OPERACION=@OPERACION";
                Conn = new Coneccion();

                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                    Conn.CmdPlabal.Parameters.AddWithValue("@IDPOTL", IDPOTL);
                    Conn.CmdPlabal.Parameters.AddWithValue("@OPERACION", OPERACION);
                    dr = Conn.CmdPlabal.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        CODMAQ= dr[0].ToString();

                        dr.Close();
                        Conn.ConnGlasser.Close();

                    }
                    else
                    {
                        dr.Close();
                        Conn.ConnGlasser.Close();
                        


                    }





                }
                catch (Exception ex)
                {

                    throw ex;

                }



            }
        }

        //Obtiene los datos de una OT
        public class GetDatosOtPconsumno
        {
            Coneccion Conn;
            static SqlDataReader dr;

            public List<TablaPOTD> POTD { get; set; }
            public TablaPOTE POTE { get; set; }

            

            public bool IsSuccess { get; set; }

            public GetDatosOtPconsumno(string NUMOT)
            {
                if (GetPOTE(NUMOT) && GetPOTD(NUMOT))
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }



            }

            private bool GetPOTE(string NUMOT)
            {
                bool IsValid = false;
                string Select = "SELECT * FROM POTE WHERE NUMOT=@OT";
                Conn = new Coneccion();

                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                    Conn.CmdPlabal.Parameters.AddWithValue("@OT", NUMOT);
                    dr = Conn.CmdPlabal.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores Val = new Validadores();
                        POTE = new TablaPOTE
                        {
                            IDPOTE = dr["IDPOTE"].ToString(),
                            _NUMOT = dr["NUMOT"].ToString(),
                            _ESTADO = dr["ESTADO"].ToString(),
                            _FIOT = Val.ParseoDateTime(dr["FIOT"].ToString()),
                            _FTOT = Val.ParseoDateTime(dr["FTOT"].ToString()),
                            _REFERENCIA = dr["REFERENCIA"].ToString(),
                            _SUOT = dr["SUOT"].ToString(),
                            _KOFUCRE = dr["KOFUCRE"].ToString(),
                            _HORAGRAB = dr["HORAGRAB"].ToString(),
                            _TIPOOT = dr["TIPOOT"].ToString(),
                            _MODO = dr["MODO"].ToString(),
                            _TIMODO = dr["TIMODO"].ToString(),
                            _EMPRESA = dr["EMPRESA"].ToString(),
                            _FTESPPROD = Val.ParseoDateTime(dr["FTESPPROD"].ToString()),
                            _TAMODO = Val.ParseoDouble(dr["TAMODO"].ToString()),
                            _FACTURAR = Val.ParseoBoolean(dr["FACTURAR"].ToString()),

                        };
                        IsValid = true;
                    }
                    else
                    {
                        IsValid = false;

                    }


                    dr.Close();
                    Conn.ConnGlasser.Close();

                }
                catch (Exception ex)
                {
                    IsValid = false;
                    throw ex;

                }

                return IsValid;

            }

            private bool GetPOTD(string NUMOT)
            {
                POTD = new List<TablaPOTD>();
                bool IsValid;

                string Select = "SELECT POTD.* FROM POTE " +
                    "LEFT OUTER JOIN POTD ON POTD.NUMOT = POTE.NUMOT AND POTD.CANTIDADF > 0 AND POTD.NIVEL = '1' AND " +
                    "POTD.IDPOTD NOT IN(SELECT IDRST FROM MAEDDO WHERE ARCHIRST = 'POTD' AND POTE.NUMOT = @OT AND POTD.IDPOTD = IDRST ) " +
                    "WHERE POTD.NUMOT = @OT " +
                    "ORDER BY POTE.FIOT DESC";

                Conn = new Coneccion();

                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnGlasser)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnGlasser);
                        adaptador.SelectCommand.Parameters.AddWithValue("@OT", NUMOT);
                        adaptador.Fill(TablaDetalle);
                        Conn.ConnGlasser.Close();
                    }
                    catch (Exception ex)
                    {
                        IsValid = false;
                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {
                    IsValid = true;
                    foreach (DataRow drr in TablaDetalle.Rows)
                    {
                        TablaPOTD pOTD = new TablaPOTD
                        {
                            IDPOTD = drr["IDPOTD"].ToString(),
                            _IDPOTL = drr["IDPOTL"].ToString(),
                            _EMPRESA = drr["EMPRESA"].ToString(),
                            _NUMOT = drr["NUMOT"].ToString(),
                            _NREG = drr["NREG"].ToString(),
                            _SUBNREG = drr["SUBNREG"].ToString(),
                            _ESTADO = drr["ESTADO"].ToString(),
                            _PERTENECE = drr["PERTENECE"].ToString(),
                            _NIVEL = drr["NIVEL"].ToString(),
                            _AUXI = drr["AUXI"].ToString(),
                            _CODIGO = drr["CODIGO"].ToString(),
                            _MARCANOM = drr["MARCANOM"].ToString(),
                            _CODNOMEN = drr["CODNOMEN"].ToString(),
                            _NUMDEEST = drr["NUMDEEST"].ToString(),
                            _GLOSA = drr["GLOSA"].ToString(),
                            _TIPOUNI = drr["TIPOUNI"].ToString(),
                            _DOBLEU = drr["DOBLEU"].ToString(),
                            _CANTIDADF = Convert.ToDouble(drr["CANTIDADF"].ToString()),
                            _CANTIDACF = drr["CANTIDACF"].ToString(),
                            _CANTIDADR = drr["CANTIDADR"].ToString(),
                            _CANTANTI = drr["CANTANTI"].ToString(),
                            _UNIDAD = drr["UNIDAD"].ToString(),
                            _UNIDADC = drr["UNIDADC"].ToString(),
                            _TIPO = drr["TIPO"].ToString(),
                            _OPERACION = drr["OPERACION"].ToString(),
                            _CALIDAD = drr["CALIDAD"].ToString(),
                            _NIVELSUP = drr["NIVELSUP"].ToString(),
                            _NUMPLAN = drr["NUMPLAN"].ToString(),
                            _LILG = drr["LILG"].ToString(),
                            _NUMODC = drr["NUMODC"].ToString(),
                            _NREGODC = drr["NREGODC"].ToString(),
                            _SULIOTD = drr["SULIOTD"].ToString(),
                            _BOLIOTD = drr["BOLIOTD"].ToString(),
                            _PNPDNREG = drr["PNPDNREG"].ToString(),
                            _C_SALIDA = Convert.ToDouble(drr["C_SALIDA"].ToString()),
                            _C_INSUMOS = Convert.ToDouble(drr["C_INSUMOS"].ToString()),
                            _P_INSUMOS = drr["P_INSUMOS"].ToString(),
                            _CCINSUMOS = drr["CCINSUMOS"].ToString(),
                            _PCINSUMOS = drr["PCINSUMOS"].ToString(),

                        };

                        POTD.Add(pOTD);

                    }
                }
                else
                {
                    IsValid = false;
                }


                return IsValid;

            }
        }

    }


    
    public class TINTERMO
    {

        #region Variables
        public string IDTINTERMO { get; set; }
        public string NRODOCU { get; set; }
        public DateTime FEMDOCU { get; set; }
        public string KOEN { get; set; }
        public string SUEN { get; set; }
        public string KOFU { get; set; }
        public string KOPR { get; set; }
        public string KOMODE { get; set; }
        public int CANTIDAD { get; set; }
        public double PRECIO { get; set; }
        public string MODO { get; set; }
        public DateTime FEERLI { get; set; }
        public string IDMAEEDO { get; set; }
        public string IDMAEDDO { get; set; }
        public string ESTADO { get; set; }
        public double PESOUBIC { get; set; }
        public double LTSUBIC { get; set; }
        public double ESPESOR { get; set; }
        public double ANCHO { get; set; }
        public double LARGO { get; set; }
        public string TIPOCOLOR { get; set; }
        public string ESPESO { get; set; }
        public string CODCLI { get; set; }
        public string PLANOPROD { get; set; }
        public double FORMA_BOTT { get; set; }
        public double PERFORA { get; set; }
        public double DESTAJE { get; set; }
        public string ALAMINA { get; set; }
        public string LLAMINA { get; set; }
        public string VIDRIO_EX { get; set; }
        public string TPSEPARA12 { get; set; }
        public double ANCHOSEP { get; set; }
        public string VIDRIO_CE { get; set; }
        public string TPSEPARA22 { get; set; }
        public double ANCHOSEP2 { get; set; }
        public string VIDRIO_IN { get; set; }
        public int FACT_PORTE { get; set; }
        public int SIPOLISUL { get; set; }
        public int SISILICON { get; set; }
        public int SIARGON { get; set; }
        public int SIVALVULA { get; set; }
        public int PED_ALFAK { get; set; }
        public int ITEM_ALFAK { get; set; }
        public double PDTO_ALFAK { get; set; }
        public double TP_TVH_COR { get; set; }
        public double TP_DVH_COR { get; set; }
        public double CORRE_ARQ { get; set; }
        public string MON_ALFAK { get; set; }
        public string FPDIDO_ALF { get; set; }
        public string SUC_ALFAK { get; set; }
        public string REFALFAK1 { get; set; }
        public string REFALFAK2 { get; set; }
        public string CODMP01 { get; set; }
        public string CODMP02 { get; set; }
        public string CODMP03 { get; set; }
        public double M2MP01 { get; set; }
        public double M2MP02 { get; set; }
        public double M2MP03 { get; set; }
        public string MOTIVOS { get; set; }
        public string SKUCLIE { get; set; }
        public string CODCOLORES { get; set; }
        public string KODCOLORES { get; set; }
        public string PROD_VPVC { get; set; }
        public double CORR_VPVC { get; set; }
        public string COLO_VPVC { get; set; }
        #endregion

        public class GetByNroDocu
        {
            public List<TINTERMO> Lista;
            public readonly bool IsSuccess;
            private SelectRows select;
            private TINTERMO item;
            public GetByNroDocu(string Numero)
            {
                Lista = new List<TINTERMO>();
                IsSuccess= Obtener(Numero);
            }

            public GetByNroDocu(string[] Numeros)
            {
                Lista = new List<TINTERMO>();
                foreach (var Nro in Numeros)
                {
                    Obtener(Nro);
                }
                
            }

            private bool Obtener(string _Numero)
            {
                Hashtable Ht = new Hashtable() { {"NRO",_Numero } };
                select = new SelectRows("RANDOM", "TINTERMO", "*", "NRODOCU=@NRO",Ht);
                if (select.IsGot)
                {
                    Validadores Val = new Validadores();
                    foreach (DataRow dr in select.Datos.Rows)
                    {

                        item = new TINTERMO {
                            ALAMINA= dr["ALAMINA"].ToString(),
                            ANCHO=Val.ParseoDouble(dr["ANCHO"].ToString()),
                            ANCHOSEP=Val.ParseoDouble(dr["ANCHOSEP"].ToString()),
                            ANCHOSEP2=Val.ParseoDouble(dr["ANCHOSEP2"].ToString()),
                            CANTIDAD=Val.ParseoInt(dr["CANTIDAD"].ToString()),
                            CODCLI= dr["CODCLI"].ToString(),
                            CODCOLORES= dr["CODCOLORES"].ToString(),
                            CODMP01= dr["CODMP01"].ToString(),
                            CODMP02= dr["CODMP02"].ToString(),
                            CODMP03= dr["CODMP03"].ToString(),
                            COLO_VPVC= dr["COLO_VPVC"].ToString(),
                            CORRE_ARQ=Val.ParseoDouble(dr["CORRE_ARQ"].ToString()),
                            CORR_VPVC=Val.ParseoDouble(dr["CORR_VPVC"].ToString()),
                            DESTAJE=Val.ParseoDouble(dr["DESTAJE"].ToString()),
                            ESPESO= dr["ESPESO"].ToString(),
                            ESPESOR=Val.ParseoDouble(dr["ESPESOR"].ToString()),
                            ESTADO= dr["ESTADO"].ToString(),
                            FACT_PORTE=Val.ParseoInt(dr["FACT_PORTE"].ToString()),
                            FEERLI=Val.ParseoDateTime(dr["FEERLI"].ToString()),
                            FEMDOCU=Val.ParseoDateTime(dr["FEMDOCU"].ToString()),
                            FORMA_BOTT=Val.ParseoDouble(dr["FORMA_BOTT"].ToString()),
                            FPDIDO_ALF= dr["FPEDIDO_ALF"].ToString(),
                            IDMAEDDO= dr["IDMAEDDO"].ToString(),
                            IDMAEEDO= dr["IDMAEEDO"].ToString(),
                            IDTINTERMO= dr["IDTINTERMO"].ToString(),
                            ITEM_ALFAK=Val.ParseoInt(dr["ITEM_ALFAK"].ToString()),
                            KODCOLORES= dr["KODCOLORES"].ToString(),
                            KOEN= dr["KOEN"].ToString(),
                            KOFU= dr["KOFU"].ToString(),
                            KOMODE= dr["KOMODE"].ToString(),
                            KOPR= dr["KOPR"].ToString(),
                            LARGO=Val.ParseoDouble(dr["LARGO"].ToString()),
                            LLAMINA= dr["LLAMINA"].ToString(),
                            LTSUBIC=Val.ParseoDouble(dr["LTSUBIC"].ToString()),
                            M2MP01=Val.ParseoDouble(dr["M2MP01"].ToString()),
                            M2MP02=Val.ParseoDouble(dr["M2MP02"].ToString()),
                            M2MP03=Val.ParseoDouble(dr["M2MP03"].ToString()),
                            MODO= dr["MODO"].ToString(),
                            MON_ALFAK= dr["MON_ALFAK"].ToString(),
                            MOTIVOS= dr["MOTIVOS"].ToString(),
                            NRODOCU= dr["NRODOCU"].ToString(),
                            PDTO_ALFAK=Val.ParseoDouble( dr["PDTO_ALFAK"].ToString()),
                            PED_ALFAK=Val.ParseoInt( dr["PED_ALFAK"].ToString()),
                            PERFORA=Val.ParseoDouble(dr["PERFORA"].ToString()),
                            PESOUBIC=Val.ParseoDouble(dr["PESOUBIC"].ToString()),
                            PLANOPROD= dr["PLANOPROD"].ToString(),
                            PRECIO= Val.ParseoDouble(dr["PRECIO"].ToString()),
                            
                        };
                        Lista.Add(item);
                    }

                    return select.IsGot;
                }
                else
                {
                    return select.IsGot;
                }

            }
        }
    }

    //busca el stock de cierto codigo en la bodega determinada, si no encuentra, busca el stock de la alternativa, si no retorna false.
    public class VerStockMaterial
    {
        Coneccion Conn;
        static SqlDataReader dr;

        public MAEST Datos { get; set; }
        public bool HasStock { get; set; }
        public bool IsAlternativa { get; set; }


        public VerStockMaterial(string EMPRESA, string KOSU, string KOBO, string KOPR,double STFI1)
        {
            MAEST Original = GetFromMAEST(EMPRESA, KOSU, KOBO, KOPR,STFI1);

            if (MAEST.Equals(Original,null))
            {
                string[] Alternativas = GetAlternativa(KOPR);

                if (Alternativas == null)
                {
                    HasStock = false;
                }
                else
                {
                    List<MAEST> ListAlternativas = new List<MAEST>();
                    foreach (var item in Alternativas)
                    {
                        MAEST mAEST = GetFromMAEST(EMPRESA,KOSU,KOBO,item,STFI1);
                        if (!MAEST.Equals(mAEST,null))
                        {
                            ListAlternativas.Add(mAEST);
                        }

                    }

                    if (ListAlternativas.Count>0)
                    {
                        double valMax = ListAlternativas.Max(x => x.STFI2);
                        Datos = ListAlternativas.Where(y=>y.STFI2==valMax).First();
                        IsAlternativa = true;
                    }
                    else
                    {
                        HasStock = false;
                    }
                }
            }
            else
            {
                Datos = Original;
                HasStock = true;
            }


        }

        private MAEST GetFromMAEST(string EMPRESA, string KOSU, string KOBO, string KOPR, double STFI1)
        {
            MAEST mAEST;
            string Select = @"SELECT TOP 1 * FROM MAEST WITH (NOLOCK) " +
                "WHERE EMPRESA=@EMPRESA AND KOSU=@KOSU AND KOBO=@KOBO AND KOPR=@KOPR AND STFI1>=@STFI1 AND STFI2>0";
            Conn = new Coneccion();


            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.CmdPlabal.Parameters.AddWithValue("@KOSU", KOSU);
                Conn.CmdPlabal.Parameters.AddWithValue("@KOBO", KOBO);
                Conn.CmdPlabal.Parameters.AddWithValue("@KOPR", KOPR);
                Conn.CmdPlabal.Parameters.AddWithValue("@STFI1", STFI1);

                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    mAEST = new MAEST
                    {
                        CODIGO= KOPR,
                        STFI1 = Convert.ToDouble(dr["STFI1"].ToString()),
                        STFI2 = Convert.ToDouble(dr["STFI2"].ToString()),
                        STREQFAB1 = Convert.ToDouble(dr["STREQFAB1"].ToString()),
                        STREQFAB2 = Convert.ToDouble(dr["STREQFAB2"].ToString()),
                    };

                }
                else
                {

                    mAEST = null;
                }


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {
                
                throw ex;

            }

            return mAEST;

        }

        public string[] GetAlternativa(string KOPR)
        {
            string[] Lista;
            bool Istrue = true;
            string Consulta = "SELECT * FROM PLA_KOPRALTERN WHERE KOPR=@KOPR AND ESTADO=@ESTADO";

            Conn = new Coneccion();

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@KOPR", KOPR);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Istrue);
                    adaptador.Fill(TablaDetalle);
                    Conn.ConnPlabal.Close();
                }
                catch (Exception ex)
                {
                   
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                int i = 0;
                Lista = new string[TablaDetalle.Rows.Count];
            
                foreach (DataRow drr in TablaDetalle.Rows)
                {
                    Lista[i] = drr["ALTERNATIVA"].ToString();
                  
                }
            }
            else
            {
                Lista = null;
                
            }

            return Lista;
        }

        public bool AlcanzaCon(double Cant2)
        {
            bool Retorno;
            if (Cant2< Datos.STFI2)
            {
                Retorno = true;

            }
            else
            {
                Retorno = false;
            }

            return Retorno;
        }


        public class MAEST
        {
            public string CODIGO { get; set; }
            public double STFI1 { get; set; }
            public double STFI2 { get; set; }
            public double STREQFAB1 { get; set; }
            public double STREQFAB2 { get; set; }

        }


    }

    //BUSCA STOCK CON HASH TABLES
    public class GetTotalStock
    {
        Coneccion Conn;
        static SqlDataReader dr;


        public Hashtable Stock { get; set; }

        private static string EMPRESA;
        private static string KOSU;
        private static string KOBO;

        public GetTotalStock(string _EMPRESA, string _KOSU, string _KOBO)
        {
            EMPRESA = _EMPRESA;
            KOSU = _KOSU;
            KOBO = _KOBO;
            GetData();

        }

        private void GetData()
        {
            string Select = @"SELECT * FROM MAEST WITH (NOLOCK) " +
                "WHERE EMPRESA=@EMPRESA AND KOSU=@KOSU AND KOBO=@KOBO AND STFI1>0 AND STFI2>0";
            Conn = new Coneccion();

            Stock = new Hashtable();

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.CmdPlabal.Parameters.AddWithValue("@KOSU", KOSU);
                Conn.CmdPlabal.Parameters.AddWithValue("@KOBO", KOBO);


                dr = Conn.CmdPlabal.ExecuteReader();
                while (dr.Read())
                {
                    Stock.Add(dr["KOPR"].ToString(), Convert.ToDouble(dr["STFI1"].ToString()));
                }
                


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {

                throw ex;

            }





        }
    }
    //Obtiene los datos de una OT
    public class GetDatosOt
    {
        Coneccion Conn;
        static SqlDataReader dr;

        public List<TablaPOTD> POTD { get; set; }
        public TablaPOTE POTE { get; set; }

        public bool IsSuccess {get;set;}

        public GetDatosOt(string NUMOT)
        {
            if (GetPOTE(NUMOT) && GetPOTD(NUMOT))
            {
                IsSuccess = true;
            }
            else
            {
                IsSuccess = false;
            }
            
            

        }

        private bool GetPOTE(string NUMOT)
        {
            bool IsValid = false;
            string Select = "select * from POTE WHERE NUMOT=@OT";
            Conn = new Coneccion();

            try
            {
                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@OT", NUMOT);
                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores Val = new Validadores();
                    POTE = new TablaPOTE
                    {
                        IDPOTE = dr["IDPOTE"].ToString(),
                        _NUMOT = dr["NUMOT"].ToString(),
                        _ESTADO = dr["ESTADO"].ToString(),
                        _FIOT = Val.ParseoDateTime(dr["FIOT"].ToString()),
                        _FTOT = Val.ParseoDateTime(dr["FTOT"].ToString()),
                        _REFERENCIA = dr["REFERENCIA"].ToString(),
                        _SUOT = dr["SUOT"].ToString(),
                        _KOFUCRE = dr["KOFUCRE"].ToString(),
                        _HORAGRAB = dr["HORAGRAB"].ToString(),
                        _TIPOOT = dr["TIPOOT"].ToString(),
                        _MODO = dr["MODO"].ToString(),
                        _TIMODO = dr["TIMODO"].ToString(),
                        _EMPRESA = dr["EMPRESA"].ToString(),
                        _FTESPPROD = Val.ParseoDateTime(dr["FTESPPROD"].ToString()),
                        _TAMODO = Val.ParseoDouble(dr["TAMODO"].ToString()),
                        _FACTURAR = Val.ParseoBoolean(dr["FACTURAR"].ToString()),

                    };
                    IsValid = true;
                }
                else
                {
                    IsValid = false;

                }


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {
                IsValid = false;
                throw ex;

            }

            return IsValid;

        }

        private bool GetPOTD(string NUMOT)
        {
            POTD = new List<TablaPOTD>();
            bool IsValid;

            string Select = "SELECT * FROM POTD WHERE NUMOT=@OT  ORDER BY CODIGO,SUBNREG ASC";

            Conn = new Coneccion();

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnGlasser)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnGlasser);
                    adaptador.SelectCommand.Parameters.AddWithValue("@OT", NUMOT);
                    adaptador.Fill(TablaDetalle);
                    Conn.ConnGlasser.Close();
                }
                catch (Exception ex)
                {
                    IsValid = false;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                IsValid = true;
                foreach (DataRow drr in TablaDetalle.Rows)
                {
                    TablaPOTD pOTD = new TablaPOTD
                    {
                        IDPOTD = drr["IDPOTD"].ToString(),
                        _IDPOTL = drr["IDPOTL"].ToString(),
                        _EMPRESA = drr["EMPRESA"].ToString(),
                        _NUMOT = drr["NUMOT"].ToString(),
                        _NREG = drr["NREG"].ToString(),
                        _SUBNREG = drr["SUBNREG"].ToString(),
                        _ESTADO = drr["ESTADO"].ToString(),
                        _PERTENECE = drr["PERTENECE"].ToString(),
                        _NIVEL = drr["NIVEL"].ToString(),
                        _AUXI = drr["AUXI"].ToString(),
                        _CODIGO = drr["CODIGO"].ToString(),
                        _MARCANOM = drr["MARCANOM"].ToString(),
                        _CODNOMEN = drr["CODNOMEN"].ToString(),
                        _NUMDEEST = drr["NUMDEEST"].ToString(),
                        _GLOSA = drr["GLOSA"].ToString(),
                        _TIPOUNI = drr["TIPOUNI"].ToString(),
                        _DOBLEU = drr["DOBLEU"].ToString(),
                        _CANTIDADF = Convert.ToDouble(drr["CANTIDADF"].ToString()),
                        _CANTIDACF = drr["CANTIDACF"].ToString(),
                        _CANTIDADR = drr["CANTIDADR"].ToString(),
                        _CANTANTI = drr["CANTANTI"].ToString(),
                        _UNIDAD = drr["UNIDAD"].ToString(),
                        _UNIDADC = drr["UNIDADC"].ToString(),
                        _TIPO = drr["TIPO"].ToString(),
                        _OPERACION = drr["OPERACION"].ToString(),
                        _CALIDAD = drr["CALIDAD"].ToString(),
                        _NIVELSUP = drr["NIVELSUP"].ToString(),
                        _NUMPLAN = drr["NUMPLAN"].ToString(),
                        _LILG = drr["LILG"].ToString(),
                        _NUMODC = drr["NUMODC"].ToString(),
                        _NREGODC = drr["NREGODC"].ToString(),
                        _SULIOTD = drr["SULIOTD"].ToString(),
                        _BOLIOTD = drr["BOLIOTD"].ToString(),
                        _PNPDNREG = drr["PNPDNREG"].ToString(),
                        _C_SALIDA = Convert.ToDouble(drr["C_SALIDA"].ToString()),
                        _C_INSUMOS = Convert.ToDouble(drr["C_INSUMOS"].ToString()),
                        _P_INSUMOS = drr["P_INSUMOS"].ToString(),
                        _CCINSUMOS = drr["CCINSUMOS"].ToString(),
                        _PCINSUMOS = drr["PCINSUMOS"].ToString(),

                    };

                    POTD.Add(pOTD);

                }
            }
            else
            {
                IsValid = false;
            }


            return IsValid;

        }
    }

    


    // se obtiene el nro del documento segun modalidad y empresa, además hace el insert de linea en MAEEDO
    public class GenerarDocumento
    {
        Coneccion Conn;
        static SqlDataReader dr;

        public string NroDocumento { get; set; }

        public string KOCRYPT { get; set; }

        public string IDMAEEDO { get; set; }

        public bool IsSuccess { get; set; }

        private static string EMPRESA;
        private static string MODALIDAD;
        private static string TIDO;

        public GenerarDocumento(string _EMPRESA, string _MODALIDAD, string _TIDO)
        {
            EMPRESA = _EMPRESA;
            MODALIDAD = _MODALIDAD;
            TIDO = _TIDO;
            string NroInTable = GetNroToUse();
            string NroToUse = GetNroNotUsed(NroInTable);

            if (!string.IsNullOrWhiteSpace(NroToUse))
            {
                string SigNro = GetNroNotUsed(GetSigNro(NroToUse));
                if (!IsInUse(SigNro))
                {
                    bool IsUpdated = UpdateConfiest(SigNro,NroInTable);
                    if (IsUpdated)
                    {
                        

                        GenKocrypt kocrypt = new GenKocrypt(TIDO, NroToUse);
                        if (kocrypt.IsSuccess)
                        {
                            
                            if (InsertLineInMaeedo(NroToUse, kocrypt.CODIGO))
                            {
                                NroDocumento = NroToUse;
                                KOCRYPT = kocrypt.CODIGO;
                                GetIDMAEEDO();
                                IsSuccess = true;
                            }
                            
                        }
                    }
                    else
                    {
                        IsSuccess = false;
                    }

                }
                else
                {
                    IsSuccess = false;
                }
            }
            else
            {
                IsSuccess = false;
            }
            
        }

        private bool InsertLineInMaeedo(string NroDocu, string _Kocrypt)
        {
            bool IsInserted = false;
            #region Query
            string Query = "INSERT INTO MAEEDO (EMPRESA,TIDO,NUDO,KOCRYPT) VALUES (@EMPRESA,@TIDO,@NUDO,@KOCRYPT)";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnGlasser.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnGlasser);
                Conn.Cmd.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.Cmd.Parameters.AddWithValue("@TIDO", TIDO);
                Conn.Cmd.Parameters.AddWithValue("@NUDO", NroDocu);
                Conn.Cmd.Parameters.AddWithValue("@KOCRYPT", _Kocrypt);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnGlasser.Close();

                IsInserted = true;

            }
            catch (Exception ex)
            {
                throw ex;
                
            }


            return IsInserted;


        }

        private string GetNroNotUsed(string Nudo)
        {
            string NroObtenido = Nudo;
                
            if (!string.IsNullOrWhiteSpace(Nudo))
            {
                while (IsInUse(NroObtenido))
                {
                    NroObtenido = GetSigNro(NroObtenido);
                    
                }

                
            }
            else
            {
                
            }

            return NroObtenido;

        }

        private bool UpdateConfiest(string _SigNro, string _NroToUse)
        {
            bool IsUpdated = false;
            #region Query
            string Query = "UPDATE CONFIEST SET " + TIDO + "=@SigNro WHERE " + TIDO + "=@NroToUse AND EMPRESA=@EMPRESA AND MODALIDAD=@MODALIDAD";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnGlasser.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnGlasser);
                Conn.Cmd.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.Cmd.Parameters.AddWithValue("@MODALIDAD", MODALIDAD);
                Conn.Cmd.Parameters.AddWithValue("@SigNro", _SigNro);
                Conn.Cmd.Parameters.AddWithValue("@NroToUse", _NroToUse);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnGlasser.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

        private string GetSigNro(string _Nudo)
        {
            
            string Prefijo = _Nudo.Substring(0,3);
            int Correlativo = Convert.ToInt32(_Nudo.Substring(3));
            Correlativo++;
            string sufijo = Correlativo.ToString();
            //validar si esta en uso
            Correlativo = 7- sufijo.Length;
            string NroToGet = null;
            for (int i = 0; i < Correlativo; i++)
            {
                sufijo = "0" + sufijo;
            }

            NroToGet = Prefijo + sufijo;

            return NroToGet;
        }
        private bool IsInUse(string _Nudo)
        {
            bool retorno;
            string Select = @"SELECT TOP 1 * FROM MAEEDO WHERE EMPRESA=@EMPRESA AND NUDO=@NUDO AND TIDO=@TIDO";

            
            Conn = new Coneccion();


            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.CmdPlabal.Parameters.AddWithValue("@NUDO", _Nudo);
                Conn.CmdPlabal.Parameters.AddWithValue("@TIDO", TIDO);

                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    retorno = true;

                }
                else
                {
                    retorno = false;
                    

                }


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {
               
                throw ex;

            }





            return retorno;

        }


        private string GetNroToUse()
        {
            string retorno;
            string Select = @"SELECT TOP 1 * FROM CONFIEST  WHERE EMPRESA=@EMPRESA AND MODALIDAD=@MODALIDAD";
            Conn = new Coneccion();


            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.CmdPlabal.Parameters.AddWithValue("@MODALIDAD", MODALIDAD);

                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    retorno = dr[TIDO].ToString();

                }
                else
                {

                    retorno = " ";

                }


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {
                retorno = " ";
                throw ex;

            }


            
            

            return retorno;

        }

        private void GetIDMAEEDO()
        {
            string Select = @"SELECT TOP 1 * FROM MAEEDO WHERE EMPRESA=@EMPRESA AND NUDO=@NUDO AND TIDO=@TIDO";


            Conn = new Coneccion();


            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                Conn.CmdPlabal.Parameters.AddWithValue("@NUDO", NroDocumento);
                Conn.CmdPlabal.Parameters.AddWithValue("@TIDO", TIDO);

                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    IDMAEEDO = dr["IDMAEEDO"].ToString();

                }
                else
                {
                    


                }


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {

                throw ex;

            }


        }


    }


    //Obtiene datos de la tabla maestra de productos segun codigo de producto
    public class GetDatosMAEPR
    {
        Coneccion Conn;
        static SqlDataReader dr;

        public bool Existe { get; set; }

        public Tablas.MAEPR Datos { get; set; }

        public GetDatosMAEPR(string KOPR)
        {
            string Select = @"SELECT TOP 1 * FROM MAEPR WITH ( NOLOCK )  WHERE KOPR=@KOPR";
            Conn = new Coneccion();


            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@KOPR", KOPR);
                

                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores Val = new Validadores();
                    Datos = new Tablas.MAEPR {
                        TIPR = dr["TIPR"].ToString(),
                        KOPR = dr["KOPR"].ToString(),
                        NOKOPR = dr["NOKOPR"].ToString(),
                        KOPRRA = dr["KOPRRA"].ToString(),
                        NOKOPRRA = dr["NOKOPRRA"].ToString(),
                        KOPRTE = dr["KOPRTE"].ToString(),
                        KOGE = dr["KOGE"].ToString(),
                        ANALIZABLE = Val.ParseoBoolean(dr["ANALIZABLE"].ToString()),
                        ATPR = dr["ATPR"].ToString(),
                        BLOQUEAPR = dr["BLOQUEAPR"].ToString(),
                        CAMBIOSUJ = Val.ParseoBoolean(dr["CAMBIOSUJ"].ToString()),
                        CAMICO = Val.ParseoDouble(dr["CAMICO"].ToString()),
                        CAMIFA = Val.ParseoDouble(dr["CAMIFA"].ToString()),
                        CLALIBPR = dr["CLALIBPR"].ToString(),
                        COLEGPR = dr["COLEGPR"].ToString(),
                        CONSALCLI1 = Val.ParseoDouble(dr["CONSALCLI1"].ToString()),
                        CONSALCLI2 = Val.ParseoDouble(dr["CONSALCLI2"].ToString()),
                        CONSDEPRO1 = Val.ParseoDouble(dr["CONSDEPRO1"].ToString()),
                        CONSDEPRO2 = Val.ParseoDouble(dr["CONSDEPRO2"].ToString()),
                        CONTROLADO = Val.ParseoBoolean(dr["CONTROLADO"].ToString()),
                        CONUBIC = Val.ParseoBoolean(dr["CONUBIC"].ToString()),
                        DESPNOFAC1 = Val.ParseoDouble(dr["DESPNOFAC1"].ToString()),
                        DESPNOFAC2 = Val.ParseoDouble(dr["DESPNOFAC2"].ToString()),
                        DEVENGNCC1 = Val.ParseoDouble(dr["DEVENGNCC1"].ToString()),
                        DEVENGNCC2 = Val.ParseoDouble(dr["DEVENGNCC2"].ToString()),
                        DEVENGNCV1 = Val.ParseoDouble(dr["DEVENGNCV1"].ToString()),
                        DEVENGNCV2 = Val.ParseoDouble(dr["DEVENGNCV2"].ToString()),
                        DEVSINNCC1 = Val.ParseoDouble(dr["DEVSINNCC1"].ToString()),
                        DEVSINNCC2 = Val.ParseoDouble(dr["DEVSINNCC2"].ToString()),
                        DEVSINNCV1 = Val.ParseoDouble(dr["DEVSINNCV1"].ToString()),
                        DEVSINNCV2 = Val.ParseoDouble(dr["DEVSINNCV2"].ToString()),
                        DIVISIBLE = dr["DIVISIBLE"].ToString(),
                        DIVISIBLE2 = dr["DIVISIBLE2"].ToString(),
                        ESACTFI = Val.ParseoBoolean(dr["ESACTFI"].ToString()),
                        EXENTO = Val.ParseoBoolean(dr["EXENTO"].ToString()),
                        FECRPR = Val.ParseoDateTime(dr["FECRPR"].ToString()),
                        FEMOPR = Val.ParseoDateTime(dr["FEMOPR"].ToString()),
                        FEPM = Val.ParseoDateTime(dr["FEPM"].ToString()),
                        FEPMIFRS = Val.ParseoDateTime(dr["FEPMIFRS"].ToString()),
                        FEPMME = Val.ParseoDateTime(dr["FEPMME"].ToString()),
                        FEUL = Val.ParseoDateTime(dr["FEUL"].ToString()),
                        FEULOC = Val.ParseoDateTime(dr["FEULOC"].ToString()),
                        FEVALI = Val.ParseoDateTime(dr["FEVALI"].ToString()),
                        FMPR = dr["FMPR"].ToString(),
                        FUNCLOTE = dr["FUNCLOTE"].ToString(),
                        HFPR = dr["HFPR"].ToString(),
                        KOENPR = dr["KOENPR"].ToString(),
                        KOFUPR = dr["KOFUPR"].ToString(),
                        KOMODE = dr["KOMODE"].ToString(),
                        KOPRDIM = dr["KOPRDIM"].ToString(),
                        LISCOSTO = dr["LISCOSTO"].ToString(),
                        LOMAFA = Val.ParseoDouble(dr["LOMAFA"].ToString()),
                        LOMIFA = Val.ParseoDouble(dr["LOMIFA"].ToString()),
                        LOTECAJA = Val.ParseoBoolean(dr["LOTECAJA"].ToString()),
                        LTSUBIC = Val.ParseoDouble(dr["LTSUBIC"].ToString()),
                        METRCO = dr["METRCO"].ToString(),
                        MIKOPR = dr["MIKOPR"].ToString(),
                        MORGPR = dr["MORGPR"].ToString(),
                        MOUL = dr["MOUL"].ToString(),
                        MOULOC = dr["MOULOC"].ToString(),
                        MOVETIQ = Val.ParseoBoolean(dr["MOVETIQ"].ToString()),
                        MRPR = dr["MRPR"].ToString(),
                        MUDEFA = Val.ParseoDouble(dr["MUDEFA"].ToString()),
                        NFPRRG = dr["NFPRRG"].ToString(),
                        NIPRRG = dr["NIPRRG"].ToString(),
                        NMARCA = dr["NMARCA"].ToString(),
                        NODIM1 = dr["NODIM1"].ToString(),
                        NODIM2 = dr["NODIM2"].ToString(),
                        NODIM3 = dr["NODIM3"].ToString(),
                        NOKOPRAMP = dr["NOKOPRAMP"].ToString(),
                        NUIMPR = Val.ParseoDouble(dr["NUIMPR"].ToString()),
                        PESOUBIC = Val.ParseoDouble(dr["PESOUBIC"].ToString()),
                        PFPR = dr["PFPR"].ToString(),
                        PLANO = dr["PLANO"].ToString(),
                        PM = Val.ParseoDouble(dr["PM"].ToString()),
                        PMIFRS = Val.ParseoDouble(dr["PMIFRS"].ToString()),
                        PMIN = Val.ParseoDouble(dr["PMIN"].ToString()),
                        PMME = Val.ParseoDouble(dr["PMME"].ToString()),
                        PODEIVPR = Val.ParseoDouble(dr["PODEIVPR"].ToString()),
                        POIVPR = Val.ParseoDouble(dr["POIVPR"].ToString()),
                        PPUL01 = Val.ParseoDouble(dr["PPUL01"].ToString()),
                        PPUL01OC = Val.ParseoDouble(dr["PPUL01OC"].ToString()),
                        PPUL02 = Val.ParseoDouble(dr["PPUL02"].ToString()),
                        PPUL02OC = Val.ParseoDouble(dr["PPUL02OC"].ToString()),
                        PRDESRES = Val.ParseoBoolean(dr["PRDESRES"].ToString()),
                        PRESALCLI1 = Val.ParseoDouble(dr["PRESALCLI1"].ToString()),
                        PRESALCLI2 = Val.ParseoDouble(dr["PRESALCLI2"].ToString()),
                        PRESDEPRO1 = Val.ParseoDouble(dr["PRESDEPRO1"].ToString()),
                        PRESDEPRO2 = Val.ParseoDouble(dr["PRESDEPRO2"].ToString()),
                        PRRG = Val.ParseoBoolean(dr["PRRG"].ToString()),
                        RECENOFAC1 = Val.ParseoDouble(dr["RECENOFAC1"].ToString()),
                        RECENOFAC2 = Val.ParseoDouble(dr["RECENOFAC2"].ToString()),
                        REPUESTO = dr["REPUESTO"].ToString(),
                        RGPR = dr["RGPR"].ToString(),
                        RLUD = Val.ParseoDouble(dr["RLUD"].ToString()),
                        RUPR = dr["RUPR"].ToString(),
                        STDV1 = Val.ParseoDouble(dr["STDV1"].ToString()),
                        STDV1C = Val.ParseoDouble(dr["STDV1C"].ToString()),
                        STDV2 = Val.ParseoDouble(dr["STDV2"].ToString()),
                        STDV2C = Val.ParseoDouble(dr["STDV2C"].ToString()),
                        STENFAB1 = Val.ParseoDouble(dr["STENFAB1"].ToString()),
                        STENFAB2 = Val.ParseoDouble(dr["STENFAB2"].ToString()),
                        STFI1 = Val.ParseoDouble(dr["STFI1"].ToString()),
                        STFI2 = Val.ParseoDouble(dr["STFI2"].ToString()),
                        STMAPR = Val.ParseoDouble(dr["STMAPR"].ToString()),
                        STMIPR = Val.ParseoDouble(dr["STMIPR"].ToString()),
                        STOCKASEG = Val.ParseoBoolean(dr["STOCKASEG"].ToString()),
                        STOCNV1 = Val.ParseoDouble(dr["STOCNV1"].ToString()),
                        STOCNV1C = Val.ParseoDouble(dr["STOCNV1C"].ToString()),
                        STOCNV2 = Val.ParseoDouble(dr["STOCNV2"].ToString()),
                        STOCNV2C = Val.ParseoDouble( dr["STOCNV2C"].ToString()),
                        STREQFAB1 = Val.ParseoDouble(dr["STREQFAB1"].ToString()),
                        STREQFAB2=Val.ParseoDouble(dr["STREQFAB2"].ToString()),
                        STTR1 = Val.ParseoDouble(dr["STTR1"].ToString()),
                       

                    };
                    Existe = true;

                }
                else
                {
                    Existe = false;

                }


                dr.Close();
                Conn.ConnGlasser.Close();

            }
            catch (Exception ex)
            {
                Existe = false;
                throw ex;

            }

        }
    }

    public class Tablas
    {
        public class InsertInMAEDDO
        {
            public bool IsInserted;

            Coneccion Conn;

            public InsertInMAEDDO(Tablas.MAEDDO _MAEDDO)
            {
                #region Query
                string Query = "INSERT INTO MAEDDO VALUES(@IDMAEEDO ,@ARCHIRST ,@IDRST ,@EMPRESA ,@TIDO ,@NUDO ,@ENDO ," +
                    "@SUENDO ,@ENDOFI ,@LILG ,@NULIDO ,@SULIDO ,@LUVTLIDO ,@BOSULIDO ,@KOFULIDO ,@NULILG ,@PRCT ,@TICT ,@TIPR ,@NUSEPR ," +
                    "@KOPRCT ,@UDTRPR ,@RLUDPR ,@CAPRCO1 ,@CAPRAD1 ,@CAPREX1 ,@CAPRNC1 ,@UD01PR ,@CAPRCO2 ,@CAPRAD2 ,@CAPREX2 ,@CAPRNC2 ," +
                    "@UD02PR ,@KOLTPR ,@MOPPPR ,@TIMOPPPR ,@TAMOPPPR ,@PPPRNELT ,@PPPRNE ,@PPPRBRLT ,@PPPRBR ,@NUDTLI ,@PODTGLLI ,@VADTNELI ," +
                    "@VADTBRLI ,@POIVLI ,@VAIVLI ,@NUIMLI ,@POIMGLLI ,@VAIMLI ,@VANELI ,@VABRLI ,@TIGELI ,@EMPREPA ,@TIDOPA ,@NUDOPA ,@ENDOPA ," +
                    "@NULIDOPA ,@LLEVADESP ,@FEEMLI ,@FEERLI ,@PPPRPM ,@OPERACION ,@CODMAQ ,@ESLIDO ,@PPPRNERE1 ,@PPPRNERE2 ,@ESFALI ,@CAFACO ," +
                    "@CAFAAD ,@CAFAEX ,@CMLIDO ,@NULOTE ,@FVENLOTE ,@ARPROD ,@NULIPROD ,@NUCOCO ,@NULICO ,@PERIODO ,@FCRELOTE ,@SUBLOTE ,@NOKOPR ," +
                    "@ALTERNAT ,@PRENDIDO ,@OBSERVA ,@KOFUAULIDO ,@KOOPLIDO ,@MGLTPR ,@PPOPPR ,@TIPOMG ,@ESPRODLI ,@CAPRODCO ,@CAPRODAD ,@CAPRODEX ," +
                    "@CAPRODRE ,@TASADORIG ,@CUOGASDIF ,@SEMILLA ,@PROYECTO ,@POTENCIA ,@HUMEDAD ,@IDTABITPRE ,@IDODDGDV ,@LINCONDESP ,@PODEIVLI ," +
                    "@VADEIVLI ,@PRIIDETIQ ,@KOLORESCA ,@KOENVASE ,@PPPRPMSUC ,@PPPRPMIFRS ,@COSTOTRIB ,@COSTOIFRS ,@SUENDOFI ,@COMISION ,@FLUVTCALZA ," +
                    "@FEERLIMODI)";
                #endregion


                Conn = new Coneccion();
                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnGlasser);
                    #region Parametros
                    
                    Conn.Cmd.Parameters.AddWithValue("@IDMAEEDO", _MAEDDO.IDMAEEDO);
                    Conn.Cmd.Parameters.AddWithValue("@ARCHIRST", _MAEDDO.ARCHIRST);
                    Conn.Cmd.Parameters.AddWithValue("@IDRST", _MAEDDO.IDRST);
                    Conn.Cmd.Parameters.AddWithValue("@EMPRESA", _MAEDDO.EMPRESA);
                    Conn.Cmd.Parameters.AddWithValue("@TIDO", _MAEDDO.TIDO);
                    Conn.Cmd.Parameters.AddWithValue("@NUDO", _MAEDDO.NUDO);
                    Conn.Cmd.Parameters.AddWithValue("@ENDO", _MAEDDO.ENDO);
                    Conn.Cmd.Parameters.AddWithValue("@SUENDO", _MAEDDO.SUENDO);
                    Conn.Cmd.Parameters.AddWithValue("@ENDOFI", _MAEDDO.ENDOFI);
                    Conn.Cmd.Parameters.AddWithValue("@LILG", _MAEDDO.LILG);
                    Conn.Cmd.Parameters.AddWithValue("@NULIDO", _MAEDDO.NULIDO);
                    Conn.Cmd.Parameters.AddWithValue("@SULIDO", _MAEDDO.SULIDO);
                    Conn.Cmd.Parameters.AddWithValue("@LUVTLIDO", _MAEDDO.LUVTLIDO);
                    Conn.Cmd.Parameters.AddWithValue("@BOSULIDO", _MAEDDO.BOSULIDO);
                    Conn.Cmd.Parameters.AddWithValue("@KOFULIDO", _MAEDDO.KOFULIDO);
                    Conn.Cmd.Parameters.AddWithValue("@NULILG", _MAEDDO.NULILG);
                    Conn.Cmd.Parameters.AddWithValue("@PRCT", _MAEDDO.PRCT);
                    Conn.Cmd.Parameters.AddWithValue("@TICT", _MAEDDO.TICT);
                    Conn.Cmd.Parameters.AddWithValue("@TIPR", _MAEDDO.TIPR);
                    Conn.Cmd.Parameters.AddWithValue("@NUSEPR", _MAEDDO.NUSEPR);
                    Conn.Cmd.Parameters.AddWithValue("@KOPRCT", _MAEDDO.KOPRCT);
                    Conn.Cmd.Parameters.AddWithValue("@UDTRPR", _MAEDDO.UDTRPR);
                    Conn.Cmd.Parameters.AddWithValue("@RLUDPR", _MAEDDO.RLUDPR);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRCO1", _MAEDDO.CAPRCO1);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRAD1", _MAEDDO.CAPRAD1);
                    Conn.Cmd.Parameters.AddWithValue("@CAPREX1", _MAEDDO.CAPREX1);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRNC1", _MAEDDO.CAPRNC1);
                    Conn.Cmd.Parameters.AddWithValue("@UD01PR", _MAEDDO.UD01PR);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRCO2", _MAEDDO.CAPRCO2);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRAD2", _MAEDDO.CAPRAD2);
                    Conn.Cmd.Parameters.AddWithValue("@CAPREX2", _MAEDDO.CAPREX2);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRNC2", _MAEDDO.CAPRNC2);
                    Conn.Cmd.Parameters.AddWithValue("@UD02PR", _MAEDDO.UD02PR);
                    Conn.Cmd.Parameters.AddWithValue("@KOLTPR", _MAEDDO.KOLTPR);
                    Conn.Cmd.Parameters.AddWithValue("@MOPPPR", _MAEDDO.MOPPPR);
                    Conn.Cmd.Parameters.AddWithValue("@TIMOPPPR", _MAEDDO.TIMOPPPR);
                    Conn.Cmd.Parameters.AddWithValue("@TAMOPPPR", _MAEDDO.TAMOPPPR);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRNELT", _MAEDDO.PPPRNELT);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRNE", _MAEDDO.PPPRNE);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRBRLT", _MAEDDO.PPPRBRLT);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRBR", _MAEDDO.PPPRBR);
                    Conn.Cmd.Parameters.AddWithValue("@NUDTLI", _MAEDDO.NUDTLI);
                    Conn.Cmd.Parameters.AddWithValue("@PODTGLLI", _MAEDDO.PODTGLLI);
                    Conn.Cmd.Parameters.AddWithValue("@VADTNELI", _MAEDDO.VADTNELI);
                    Conn.Cmd.Parameters.AddWithValue("@VADTBRLI", _MAEDDO.VADTBRLI);
                    Conn.Cmd.Parameters.AddWithValue("@POIVLI", _MAEDDO.POIVLI);
                    Conn.Cmd.Parameters.AddWithValue("@VAIVLI", _MAEDDO.VAIVLI);
                    Conn.Cmd.Parameters.AddWithValue("@NUIMLI", _MAEDDO.NUIMLI);
                    Conn.Cmd.Parameters.AddWithValue("@POIMGLLI", _MAEDDO.POIMGLLI);
                    Conn.Cmd.Parameters.AddWithValue("@VAIMLI", _MAEDDO.VAIMLI);
                    Conn.Cmd.Parameters.AddWithValue("@VANELI", _MAEDDO.VANELI);
                    Conn.Cmd.Parameters.AddWithValue("@VABRLI", _MAEDDO.VABRLI);
                    Conn.Cmd.Parameters.AddWithValue("@TIGELI", _MAEDDO.TIGELI);
                    Conn.Cmd.Parameters.AddWithValue("@EMPREPA", _MAEDDO.EMPREPA);
                    Conn.Cmd.Parameters.AddWithValue("@TIDOPA", _MAEDDO.TIDOPA);
                    Conn.Cmd.Parameters.AddWithValue("@NUDOPA", _MAEDDO.NUDOPA);
                    Conn.Cmd.Parameters.AddWithValue("@ENDOPA", _MAEDDO.ENDOPA);
                    Conn.Cmd.Parameters.AddWithValue("@NULIDOPA", _MAEDDO.NULIDOPA);
                    Conn.Cmd.Parameters.AddWithValue("@LLEVADESP", _MAEDDO.LLEVADESP);
                    Conn.Cmd.Parameters.AddWithValue("@FEEMLI", _MAEDDO.FEEMLI);
                    Conn.Cmd.Parameters.AddWithValue("@FEERLI", _MAEDDO.FEERLI);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRPM", _MAEDDO.PPPRPM);
                    Conn.Cmd.Parameters.AddWithValue("@OPERACION", _MAEDDO.OPERACION);
                    Conn.Cmd.Parameters.AddWithValue("@CODMAQ", _MAEDDO.CODMAQ);
                    Conn.Cmd.Parameters.AddWithValue("@ESLIDO", _MAEDDO.ESLIDO);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRNERE1", _MAEDDO.PPPRNERE1);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRNERE2", _MAEDDO.PPPRNERE2);
                    Conn.Cmd.Parameters.AddWithValue("@ESFALI", _MAEDDO.ESFALI);
                    Conn.Cmd.Parameters.AddWithValue("@CAFACO", _MAEDDO.CAFACO);
                    Conn.Cmd.Parameters.AddWithValue("@CAFAAD", _MAEDDO.CAFAAD);
                    Conn.Cmd.Parameters.AddWithValue("@CAFAEX", _MAEDDO.CAFAEX);
                    Conn.Cmd.Parameters.AddWithValue("@CMLIDO", _MAEDDO.CMLIDO);
                    Conn.Cmd.Parameters.AddWithValue("@NULOTE", _MAEDDO.NULOTE);
                    Conn.Cmd.Parameters.AddWithValue("@FVENLOTE", _MAEDDO.FVENLOTE);
                    Conn.Cmd.Parameters.AddWithValue("@ARPROD", _MAEDDO.ARPROD);
                    Conn.Cmd.Parameters.AddWithValue("@NULIPROD", _MAEDDO.NULIPROD);
                    Conn.Cmd.Parameters.AddWithValue("@NUCOCO", _MAEDDO.NUCOCO);
                    Conn.Cmd.Parameters.AddWithValue("@NULICO", _MAEDDO.NULICO);
                    Conn.Cmd.Parameters.AddWithValue("@PERIODO", _MAEDDO.PERIODO);
                    Conn.Cmd.Parameters.AddWithValue("@FCRELOTE", _MAEDDO.FCRELOTE);
                    Conn.Cmd.Parameters.AddWithValue("@SUBLOTE", _MAEDDO.SUBLOTE);
                    Conn.Cmd.Parameters.AddWithValue("@NOKOPR", _MAEDDO.NOKOPR);
                    Conn.Cmd.Parameters.AddWithValue("@ALTERNAT", _MAEDDO.ALTERNAT);
                    Conn.Cmd.Parameters.AddWithValue("@PRENDIDO", _MAEDDO.PRENDIDO);
                    Conn.Cmd.Parameters.AddWithValue("@OBSERVA", _MAEDDO.OBSERVA);
                    Conn.Cmd.Parameters.AddWithValue("@KOFUAULIDO", _MAEDDO.KOFUAULIDO);
                    Conn.Cmd.Parameters.AddWithValue("@KOOPLIDO", _MAEDDO.KOOPLIDO);
                    Conn.Cmd.Parameters.AddWithValue("@MGLTPR", _MAEDDO.MGLTPR);
                    Conn.Cmd.Parameters.AddWithValue("@PPOPPR", _MAEDDO.PPOPPR);
                    Conn.Cmd.Parameters.AddWithValue("@TIPOMG", _MAEDDO.TIPOMG);
                    Conn.Cmd.Parameters.AddWithValue("@ESPRODLI", _MAEDDO.ESPRODLI);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRODCO", _MAEDDO.CAPRODCO);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRODAD", _MAEDDO.CAPRODAD);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRODEX", _MAEDDO.CAPRODEX);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRODRE", _MAEDDO.CAPRODRE);
                    Conn.Cmd.Parameters.AddWithValue("@TASADORIG", _MAEDDO.TASADORIG);
                    Conn.Cmd.Parameters.AddWithValue("@CUOGASDIF", _MAEDDO.CUOGASDIF);
                    Conn.Cmd.Parameters.AddWithValue("@SEMILLA", _MAEDDO.SEMILLA);
                    Conn.Cmd.Parameters.AddWithValue("@PROYECTO", _MAEDDO.PROYECTO);
                    Conn.Cmd.Parameters.AddWithValue("@POTENCIA", _MAEDDO.POTENCIA);
                    Conn.Cmd.Parameters.AddWithValue("@HUMEDAD", _MAEDDO.HUMEDAD);
                    Conn.Cmd.Parameters.AddWithValue("@IDTABITPRE", _MAEDDO.IDTABITPRE);
                    Conn.Cmd.Parameters.AddWithValue("@IDODDGDV", _MAEDDO.IDODDGDV);
                    Conn.Cmd.Parameters.AddWithValue("@LINCONDESP", _MAEDDO.LINCONDESP);
                    Conn.Cmd.Parameters.AddWithValue("@PODEIVLI", _MAEDDO.PODEIVLI);
                    Conn.Cmd.Parameters.AddWithValue("@VADEIVLI", _MAEDDO.VADEIVLI);
                    Conn.Cmd.Parameters.AddWithValue("@PRIIDETIQ", _MAEDDO.PRIIDETIQ);
                    Conn.Cmd.Parameters.AddWithValue("@KOLORESCA", _MAEDDO.KOLORESCA);
                    Conn.Cmd.Parameters.AddWithValue("@KOENVASE", _MAEDDO.KOENVASE);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRPMSUC", _MAEDDO.PPPRPMSUC);
                    Conn.Cmd.Parameters.AddWithValue("@PPPRPMIFRS", _MAEDDO.PPPRPMIFRS);
                    Conn.Cmd.Parameters.AddWithValue("@COSTOTRIB", _MAEDDO.COSTOTRIB);
                    Conn.Cmd.Parameters.AddWithValue("@COSTOIFRS", _MAEDDO.COSTOIFRS);
                    Conn.Cmd.Parameters.AddWithValue("@SUENDOFI", _MAEDDO.SUENDOFI);
                    Conn.Cmd.Parameters.AddWithValue("@COMISION", _MAEDDO.COMISION);
                    Conn.Cmd.Parameters.AddWithValue("@FLUVTCALZA", _MAEDDO.FLUVTCALZA);
                    Conn.Cmd.Parameters.AddWithValue("@FEERLIMODI", _MAEDDO.FEERLIMODI);
                    #endregion


                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnGlasser.Close();
                    IsInserted = true;

                }
                catch (Exception ex)
                {
                    throw ex;

                }


            }


        }
        public class UpdateMAEEDO
        {
            public bool IsUpdated;
            Coneccion Conn;
            public UpdateMAEEDO(MAEEDO _MAEEDO)
            {
                #region MyRegion
                string Query = "UPDATE MAEEDO SET ENDO=@ENDO,SUENDO=@SUENDO,ENDOFI=@ENDOFI,TIGEDO=@TIGEDO,SUDO=@SUDO,LUVTDO=@LUVTDO," +
                                "FEEMDO=@FEEMDO,KOFUDO=@KOFUDO,ESDO=@ESDO,ESPGDO=@ESPGDO,CAPRAD=@CAPRAD,MEARDO=@MEARDO,MODO=@MODO,TIMODO=@TIMODO," +
                                "TAMODO=@TAMODO,NUCTAP=@NUCTAP,VACTDTNEDO=@VACTDTNEDO,NUIVDO=@NUIVDO,POIVDO=@POIVDO,NUIMDO=@NUIMDO,VAIMDO=@VAIMDO," +
                                "POPIDO=@POPIDO,VAPIDO=@VAPIDO,FE01VEDO=@FE01VEDO,FEULVEDO=@FEULVEDO,FEER=@FEER,NUVEDO=@NUVEDO,VAABDO=@VAABDO,MARCA=@MARCA," +
                                "NUTRANSMI=@NUTRANSMI,NUCOCO=@NUCOCO,KOTU=@KOTU,LIBRO=@LIBRO,LCLV=@LCLV,ESFADO=@ESFADO,KOTRPCVH=@KOTRPCVH,NULICO=@NULICO," +
                                "PERIODO=@PERIODO,NUDONODEFI=@NUDONODEFI,TRANSMASI=@TRANSMASI,POIVARET=@POIVARET,VAIVARET=@VAIVARET,RESUMEN=@RESUMEN," +
                                "LAHORA=@LAHORA,KOFUAUDO=@KOFUAUDO,KOOPDO=@KOOPDO,ESPRODDO=@ESPRODDO,DESPACHO=@DESPACHO,HORAGRAB=@HORAGRAB,RUTCONTACT=@RUTCONTACT," +
                                "SUBTIDO=@SUBTIDO,TIDOELEC=@TIDOELEC,ESDOIMP=@ESDOIMP,CUOGASDIF=@CUOGASDIF,BODESTI=@BODESTI,PROYECTO=@PROYECTO,FECHATRIB=@FECHATRIB," +
                                "NUMOPERVEN=@NUMOPERVEN,BLOQUEAPAG=@BLOQUEAPAG,VALORRET=@VALORRET,FLIQUIFCV=@FLIQUIFCV,VADEIVDO=@VADEIVDO,KOCANAL=@KOCANAL,LEYZONA=@LEYZONA," +
                                "KOSIFIC=@KOSIFIC,LISACTIVA=@LISACTIVA,KOFUAUTO=@KOFUAUTO,SUENDOFI=@SUENDOFI,VAIVDOZF=@VAIVDOZF,ENDOMANDA=@ENDOMANDA,FLUVTCALZA=@FLUVTCALZA," +
                                "ARCHIXML=@ARCHIXML,IDXML=@IDXML,SERIENUDO=@SERIENUDO,VALORAJU=@VALORAJU WHERE IDMAEEDO=@IDMAEEDO";
                #endregion

                Conn = new Coneccion();
                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnGlasser);
                    #region Parametros
                    Conn.Cmd.Parameters.AddWithValue("@IDMAEEDO", _MAEEDO.IDMAEEDO);
                    Conn.Cmd.Parameters.AddWithValue("@ENDO", _MAEEDO.ENDO);
                    Conn.Cmd.Parameters.AddWithValue("@SUENDO", _MAEEDO.SUENDO);
                    Conn.Cmd.Parameters.AddWithValue("@ENDOFI", _MAEEDO.ENDOFI);
                    Conn.Cmd.Parameters.AddWithValue("@TIGEDO", _MAEEDO.TIGEDO);
                    Conn.Cmd.Parameters.AddWithValue("@SUDO", _MAEEDO.SUDO);
                    Conn.Cmd.Parameters.AddWithValue("@LUVTDO", _MAEEDO.LUVTDO);
                    Conn.Cmd.Parameters.AddWithValue("@FEEMDO", _MAEEDO.FEEMDO);
                    Conn.Cmd.Parameters.AddWithValue("@KOFUDO", _MAEEDO.KOFUDO);
                    Conn.Cmd.Parameters.AddWithValue("@ESDO", _MAEEDO.ESDO);
                    Conn.Cmd.Parameters.AddWithValue("@ESPGDO", _MAEEDO.ESPGDO);
                    Conn.Cmd.Parameters.AddWithValue("@CAPRAD", _MAEEDO.CAPRAD);
                    Conn.Cmd.Parameters.AddWithValue("@MEARDO", _MAEEDO.MEARDO);
                    Conn.Cmd.Parameters.AddWithValue("@MODO", _MAEEDO.MODO);
                    Conn.Cmd.Parameters.AddWithValue("@TIMODO", _MAEEDO.TIMODO);
                    Conn.Cmd.Parameters.AddWithValue("@TAMODO", _MAEEDO.TAMODO);
                    Conn.Cmd.Parameters.AddWithValue("@NUCTAP", _MAEEDO.NUCTAP);
                    Conn.Cmd.Parameters.AddWithValue("@VACTDTNEDO", _MAEEDO.VACTDTNEDO);
                    Conn.Cmd.Parameters.AddWithValue("@NUIVDO", _MAEEDO.NUIVDO);
                    Conn.Cmd.Parameters.AddWithValue("@POIVDO", _MAEEDO.POIVDO);
                    Conn.Cmd.Parameters.AddWithValue("@NUIMDO", _MAEEDO.NUIMDO);
                    Conn.Cmd.Parameters.AddWithValue("@VAIMDO", _MAEEDO.VAIMDO);
                    Conn.Cmd.Parameters.AddWithValue("@POPIDO", _MAEEDO.POPIDO);
                    Conn.Cmd.Parameters.AddWithValue("@VAPIDO", _MAEEDO.VAPIDO);
                    Conn.Cmd.Parameters.AddWithValue("@FE01VEDO", _MAEEDO.FE01VEDO);
                    Conn.Cmd.Parameters.AddWithValue("@FEULVEDO", _MAEEDO.FEULVEDO);
                    Conn.Cmd.Parameters.AddWithValue("@FEER", _MAEEDO.FEER);
                    Conn.Cmd.Parameters.AddWithValue("@NUVEDO", _MAEEDO.NUVEDO);
                    Conn.Cmd.Parameters.AddWithValue("@VAABDO", _MAEEDO.VAABDO);
                    Conn.Cmd.Parameters.AddWithValue("@MARCA", _MAEEDO.MARCA);
                    Conn.Cmd.Parameters.AddWithValue("@NUTRANSMI", _MAEEDO.NUTRANSMI);
                    Conn.Cmd.Parameters.AddWithValue("@NUCOCO", _MAEEDO.NUCOCO);
                    Conn.Cmd.Parameters.AddWithValue("@KOTU", _MAEEDO.KOTU);
                    Conn.Cmd.Parameters.AddWithValue("@LIBRO", _MAEEDO.LIBRO);
                    Conn.Cmd.Parameters.AddWithValue("@LCLV", _MAEEDO.LCLV);
                    Conn.Cmd.Parameters.AddWithValue("@ESFADO", _MAEEDO.ESFADO);
                    Conn.Cmd.Parameters.AddWithValue("@KOTRPCVH", _MAEEDO.KOTRPCVH);
                    Conn.Cmd.Parameters.AddWithValue("@NULICO", _MAEEDO.NULICO);
                    Conn.Cmd.Parameters.AddWithValue("@PERIODO", _MAEEDO.PERIODO);
                    Conn.Cmd.Parameters.AddWithValue("@NUDONODEFI", _MAEEDO.NUDONODEFI);
                    Conn.Cmd.Parameters.AddWithValue("@TRANSMASI", _MAEEDO.TRANSMASI);
                    Conn.Cmd.Parameters.AddWithValue("@POIVARET", _MAEEDO.POIVARET);
                    Conn.Cmd.Parameters.AddWithValue("@VAIVARET", _MAEEDO.VAIVARET);
                    Conn.Cmd.Parameters.AddWithValue("@RESUMEN", _MAEEDO.RESUMEN);
                    Conn.Cmd.Parameters.AddWithValue("@LAHORA", _MAEEDO.LAHORA);
                    Conn.Cmd.Parameters.AddWithValue("@KOFUAUDO", _MAEEDO.KOFUAUDO);
                    Conn.Cmd.Parameters.AddWithValue("@KOOPDO", _MAEEDO.KOOPDO);
                    Conn.Cmd.Parameters.AddWithValue("@ESPRODDO", _MAEEDO.ESPRODDO);
                    Conn.Cmd.Parameters.AddWithValue("@DESPACHO", _MAEEDO.DESPACHO);
                    Conn.Cmd.Parameters.AddWithValue("@HORAGRAB", _MAEEDO.HORAGRAB);
                    Conn.Cmd.Parameters.AddWithValue("@RUTCONTACT", _MAEEDO.RUTCONTACT);
                    Conn.Cmd.Parameters.AddWithValue("@SUBTIDO", _MAEEDO.SUBTIDO);
                    Conn.Cmd.Parameters.AddWithValue("@TIDOELEC", _MAEEDO.TIDOELEC);
                    Conn.Cmd.Parameters.AddWithValue("@ESDOIMP", _MAEEDO.ESDOIMP);
                    Conn.Cmd.Parameters.AddWithValue("@CUOGASDIF", _MAEEDO.CUOGASDIF);
                    Conn.Cmd.Parameters.AddWithValue("@BODESTI", _MAEEDO.BODESTI);
                    Conn.Cmd.Parameters.AddWithValue("@PROYECTO", _MAEEDO.PROYECTO);
                    Conn.Cmd.Parameters.AddWithValue("@FECHATRIB", _MAEEDO.FECHATRIB);
                    Conn.Cmd.Parameters.AddWithValue("@NUMOPERVEN", _MAEEDO.NUMOPERVEN);
                    Conn.Cmd.Parameters.AddWithValue("@BLOQUEAPAG", _MAEEDO.BLOQUEAPAG);
                    Conn.Cmd.Parameters.AddWithValue("@VALORRET", _MAEEDO.VALORRET);
                    Conn.Cmd.Parameters.AddWithValue("@FLIQUIFCV", _MAEEDO.FLIQUIFCV);
                    Conn.Cmd.Parameters.AddWithValue("@VADEIVDO", _MAEEDO.VADEIVDO);
                    Conn.Cmd.Parameters.AddWithValue("@KOCANAL", _MAEEDO.KOCANAL);
                    Conn.Cmd.Parameters.AddWithValue("@LEYZONA", _MAEEDO.LEYZONA);
                    Conn.Cmd.Parameters.AddWithValue("@KOSIFIC", _MAEEDO.KOSIFIC);
                    Conn.Cmd.Parameters.AddWithValue("@LISACTIVA", _MAEEDO.LISACTIVA);
                    Conn.Cmd.Parameters.AddWithValue("@KOFUAUTO", _MAEEDO.KOFUAUTO);
                    Conn.Cmd.Parameters.AddWithValue("@SUENDOFI", _MAEEDO.SUENDOFI);
                    Conn.Cmd.Parameters.AddWithValue("@VAIVDOZF", _MAEEDO.VAIVDOZF);
                    Conn.Cmd.Parameters.AddWithValue("@ENDOMANDA", _MAEEDO.ENDOMANDA);
                    Conn.Cmd.Parameters.AddWithValue("@FLUVTCALZA", _MAEEDO.FLUVTCALZA);
                    Conn.Cmd.Parameters.AddWithValue("@ARCHIXML", _MAEEDO.ARCHIXML);
                    Conn.Cmd.Parameters.AddWithValue("@IDXML", _MAEEDO.IDXML);
                    Conn.Cmd.Parameters.AddWithValue("@SERIENUDO", _MAEEDO.SERIENUDO);
                    Conn.Cmd.Parameters.AddWithValue("@VALORAJU", _MAEEDO.VALORAJU);
                    #endregion


                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnGlasser.Close();
                    IsUpdated = true;

                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
        }
        public class Update
        {
            public bool IsUpdated { get; set; }

            Coneccion Conn;

            public Update(string _TABLA, string _SET, string _WHERE)
            {
                #region Query
                string Query = "UPDATE " + _TABLA + " SET " + _SET + " WHERE " + _WHERE;
                #endregion

                Conn = new Coneccion();
                try
                {
                    Conn.ConnGlasser.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnGlasser);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnGlasser.Close();
                    IsUpdated = true;

                }
                catch (Exception ex)
                {
                    throw ex;
                    
                }

            }

        }
        
        public class MAEEDO
        {
            public string IDMAEEDO { get; set; }
            public string EMPRESA { get; set; }
            public string TIDO { get; set; }
            public string NUDO { get; set; }
            public string ENDO { get; set; }
            public string SUENDO { get; set; }
            public string ENDOFI { get; set; }
            public string TIGEDO { get; set; }
            public string SUDO { get; set; }
            public string LUVTDO { get; set; }
            public DateTime FEEMDO { get; set; }
            public string KOFUDO { get; set; }
            public string ESDO { get; set; }
            public string ESPGDO { get; set; }
            public double CAPRCO { get; set; }
            public double CAPRAD { get; set; }
            public double CAPREX { get; set; }
            public double CAPRNC { get; set; }
            public string MEARDO { get; set; }
            public string MODO { get; set; }
            public string TIMODO { get; set; }
            public double TAMODO { get; set; }
            public double NUCTAP { get; set; }
            public double VACTDTNEDO { get; set; }
            public double VACTDTBRDO { get; set; }
            public double NUIVDO { get; set; }
            public double POIVDO { get; set; }
            public double VAIVDO { get; set; }
            public double NUIMDO { get; set; }
            public double VAIMDO { get; set; }
            public double VANEDO { get; set; }
            public double VABRDO { get; set; }
            public double POPIDO { get; set; }
            public double VAPIDO { get; set; }
            public DateTime FE01VEDO { get; set; }
            public DateTime FEULVEDO { get; set; }
            public double NUVEDO { get; set; }
            public double VAABDO { get; set; }
            public string MARCA { get; set; }
            public DateTime FEER { get; set; }
            public string NUTRANSMI { get; set; }
            public string NUCOCO { get; set; }
            public string KOTU { get; set; }
            public string LIBRO { get; set; }
            public DateTime LCLV { get; set; }
            public string ESFADO { get; set; }
            public string KOTRPCVH { get; set; }
            public string NULICO { get; set; }
            public string PERIODO { get; set; }
            public bool NUDONODEFI { get; set; }
            public string TRANSMASI { get; set; }
            public double POIVARET { get; set; }
            public double VAIVARET { get; set; }
            public string RESUMEN { get; set; }
            public DateTime LAHORA { get; set; }
            public string KOFUAUDO { get; set; }
            public string KOOPDO { get; set; }
            public string ESPRODDO { get; set; }
            public int DESPACHO { get; set; }
            public int HORAGRAB { get; set; }
            public string RUTCONTACT { get; set; }
            public string SUBTIDO { get; set; }
            public bool TIDOELEC { get; set; }
            public string ESDOIMP { get; set; }
            public string CUOGASDIF { get; set; }
            public string BODESTI { get; set; }
            public string PROYECTO { get; set; }
            public DateTime FECHATRIB { get; set; }
            public string NUMOPERVEN { get; set; }
            public string BLOQUEAPAG { get; set; }
            public double VALORRET { get; set; }
            public DateTime FLIQUIFCV { get; set; }
            public double VADEIVDO { get; set; }
            public string KOCANAL { get; set; }
            public string KOCRYPT { get; set; }
            public string LEYZONA { get; set; }
            public string KOSIFIC { get; set; }
            public string LISACTIVA { get; set; }
            public string KOFUAUTO { get; set; }
            public string SUENDOFI { get; set; }
            public double VAIVDOZF { get; set; }
            public string ENDOMANDA { get; set; }
            public string FLUVTCALZA { get; set; }
            public string ARCHIXML { get; set; }
            public string IDXML { get; set; }
            public string SERIENUDO { get; set; }
            public string VALORAJU { get; set; }




        }

        public class MAEDDO
        {
            public string IDMAEDDO {get;set;}
            public string IDMAEEDO {get;set;}
            public string    ARCHIRST {get;set;}
            public string IDRST {get;set;}
            public string EMPRESA {get;set;}
            public string TIDO {get;set;}
            public string NUDO {get;set;}
            public string ENDO {get;set;}
            public string SUENDO {get;set;}
            public string ENDOFI {get;set;}
            public string LILG {get;set;}
            public string NULIDO {get;set;}
            public string SULIDO {get;set;}
            public string LUVTLIDO {get;set;}
            public string BOSULIDO {get;set;}
            public string KOFULIDO {get;set;}
            public string NULILG {get;set;}
            public bool PRCT {get;set;}
            public string TICT {get;set;}
            public string TIPR {get;set;}
            public string NUSEPR {get;set;}
            public string KOPRCT {get;set;}
            public double UDTRPR {get;set;}
            public double RLUDPR {get;set;}
            public double CAPRCO1 {get;set;}
            public double CAPRAD1 {get;set;}
            public double CAPREX1 {get;set;}
            public double CAPRNC1 {get;set;}
            public string UD01PR {get;set;}
            public double CAPRCO2 {get;set;}
            public double CAPRAD2 {get;set;}
            public double CAPREX2 {get;set;}
            public double CAPRNC2 {get;set;}
            public string UD02PR {get;set;}
            public string KOLTPR {get;set;}
            public string MOPPPR {get;set;}
            public string TIMOPPPR {get;set;}
            public double TAMOPPPR {get;set;}
            public double PPPRNELT {get;set;}
            public double PPPRNE {get;set;}
            public double PPPRBRLT {get;set;}
            public double PPPRBR {get;set;}
            public double NUDTLI {get;set;}
            public double PODTGLLI {get;set;}
            public double VADTNELI {get;set;}
            public double VADTBRLI {get;set;}
            public double POIVLI {get;set;}
            public double VAIVLI {get;set;}
            public double NUIMLI {get;set;}
            public double POIMGLLI {get;set;}
            public double VAIMLI {get;set;}
            public double VANELI {get;set;}
            public double VABRLI {get;set;}
            public string TIGELI {get;set;}
            public string EMPREPA {get;set;}
            public string TIDOPA {get;set;}
            public string NUDOPA {get;set;}
            public string ENDOPA {get;set;}
            public string NULIDOPA {get;set;}
            public double LLEVADESP {get;set;}
            public DateTime FEEMLI {get;set;}
            public DateTime FEERLI {get;set;}
            public double PPPRPM {get;set;}
            public string OPERACION {get;set;}
            public string CODMAQ {get;set;}
            public string ESLIDO {get;set;}
            public double PPPRNERE1 {get;set;}
            public double PPPRNERE2 {get;set;}
            public string ESFALI {get;set;}
            public double CAFACO {get;set;}
            public double CAFAAD {get;set;}
            public double CAFAEX {get;set;}
            public double CMLIDO {get;set;}
            public string NULOTE {get;set;}
            public DateTime FVENLOTE {get;set;}
            public string ARPROD {get;set;}
            public string NULIPROD {get;set;}
            public string NUCOCO {get;set;}
            public string NULICO {get;set;}
            public string PERIODO {get;set;}
            public DateTime FCRELOTE {get;set;}
            public string SUBLOTE {get;set;}
            public string NOKOPR {get;set;}
            public string ALTERNAT {get;set;}
            public bool PRENDIDO {get;set;}
            public string OBSERVA {get;set;}
            public string KOFUAULIDO {get;set;}
            public string KOOPLIDO {get;set;}
            public double MGLTPR {get;set;}
            public double PPOPPR {get;set;}
            public double TIPOMG { get;set;}
            public string ESPRODLI {get;set;}
            public double CAPRODCO { get;set;}
            public double CAPRODAD { get;set;}
            public double CAPRODEX { get;set;}
            public double CAPRODRE { get;set;}
            public double TASADORIG { get;set;}
            public string CUOGASDIF {get;set;}
            public string SEMILLA {get;set;}
            public string PROYECTO {get;set;}
            public double POTENCIA { get;set;}
            public double HUMEDAD { get;set;}
            public double IDTABITPRE { get;set;}
            public string IDODDGDV {get;set;}
            public bool LINCONDESP {get;set;}
            public double PODEIVLI { get;set;}
            public double VADEIVLI { get;set;}
            public string PRIIDETIQ {get;set;}
            public string KOLORESCA {get;set;}
            public string KOENVASE {get;set;}
            public double PPPRPMSUC { get;set;}
            public double PPPRPMIFRS { get;set;}
            public double COSTOTRIB { get;set;}
            public double COSTOIFRS { get;set;}
            public string SUENDOFI {get;set;}
            public double COMISION { get;set;}
            public string FLUVTCALZA {get;set;}
            public DateTime FEERLIMODI { get; set; }

        }

        public class MAEEDOOB
        {
            public string IDMAEEDO { get; set; }
            public string OBDO { get; set; }
            public string CPDO { get; set; }
            public string OCDO { get; set; }
            public string DIENDESP { get; set; }
        }

        public class MAEPR
        {
            public string TIPR { get; set; }
            public string KOPR { get; set; }
            public string NOKOPR { get; set; }
            public string KOPRRA { get; set; }
            public string NOKOPRRA { get; set; }
            public string KOPRTE { get; set; }
            public string KOGE { get; set; }
            public string NMARCA { get; set; }
            public string UD01PR { get; set; }
            public string UD02PR { get; set; }
            public double RLUD { get; set; }
            public double POIVPR { get; set; }
            public double NUIMPR { get; set; }
            public string RGPR { get; set; }
            public double STMIPR { get; set; }
            public double STMAPR { get; set; }
            public string MRPR { get; set; }
            public string ATPR { get; set; }
            public string RUPR { get; set; }
            public double STFI1 { get; set; }
            public double STDV1 { get; set; }
            public double STOCNV1 { get; set; }
            public double STFI2 { get; set; }
            public double STDV2 { get; set; }
            public double STOCNV2 { get; set; }
            public double PPUL01 { get; set; }
            public double PPUL02 { get; set; }
            public string MOUL { get; set; }
            public string TIMOUL { get; set; }
            public double TAUL { get; set; }
            public DateTime FEUL { get; set; }
            public double PM { get; set; }
            public DateTime FEPM { get; set; }
            public string FMPR { get; set; }
            public string PFPR { get; set; }
            public string HFPR { get; set; }
            public double VALI { get; set; }
            public DateTime FEVALI { get; set; }
            public double TTREPR { get; set; }
            public bool PRRG { get; set; }
            public string NIPRRG { get; set; }
            public string NFPRRG { get; set; }
            public double PMIN { get; set; }
            public double CAMICO { get; set; }
            public double CAMIFA { get; set; }
            public double LOMIFA { get; set; }
            public string PLANO { get; set; }
            public double STDV1C { get; set; }
            public double STOCNV1C { get; set; }
            public double STDV2C { get; set; }
            public double STOCNV2C { get; set; }
            public string METRCO { get; set; }
            public double DESPNOFAC1 { get; set; }
            public double DESPNOFAC2 { get; set; }
            public double RECENOFAC1 { get; set; }
            public double RECENOFAC2 { get; set; }
            public bool TRATALOTE { get; set; }
            public string DIVISIBLE { get; set; }
            public double MUDEFA { get; set; }
            public bool EXENTO { get; set; }
            public string KOMODE { get; set; }
            public bool PRDESRES { get; set; }
            public string LISCOSTO { get; set; }
            public bool STOCKASEG { get; set; }
            public bool ESACTFI { get; set; }
            public string CLALIBPR { get; set; }
            public string KOFUPR { get; set; }
            public string KOPRDIM { get; set; }
            public string NODIM1 { get; set; }
            public string NODIM2 { get; set; }
            public string NODIM3 { get; set; }
            public string BLOQUEAPR { get; set; }
            public string ZONAPR { get; set; }
            public bool CONUBIC { get; set; }
            public double LTSUBIC { get; set; }
            public double PESOUBIC { get; set; }
            public string FUNCLOTE { get; set; }
            public double LOMAFA { get; set; }
            public string COLEGPR { get; set; }
            public string MORGPR { get; set; }
            public DateTime FECRPR { get; set; }
            public DateTime FEMOPR { get; set; }
            public bool LOTECAJA { get; set; }
            public double STTR1 { get; set; }
            public double STTR2 { get; set; }
            public double PODEIVPR { get; set; }
            public string DIVISIBLE2 { get; set; }
            public bool MOVETIQ { get; set; }
            public string REPUESTO { get; set; }
            public double VIDAMEDIA { get; set; }
            public bool TRATVMEDIA { get; set; }
            public double PRESALCLI1 { get; set; }
            public double PRESALCLI2 { get; set; }
            public double PRESDEPRO1 { get; set; }
            public double PRESDEPRO2 { get; set; }
            public double CONSALCLI1 { get; set; }
            public double CONSALCLI2 { get; set; }
            public double CONSDEPRO1 { get; set; }
            public double CONSDEPRO2 { get; set; }
            public double DEVENGNCV1 { get; set; }
            public double DEVENGNCV2 { get; set; }
            public double DEVENGNCC1 { get; set; }
            public double DEVENGNCC2 { get; set; }
            public double DEVSINNCV1 { get; set; }
            public double DEVSINNCV2 { get; set; }
            public double DEVSINNCC1 { get; set; }
            public double DEVSINNCC2 { get; set; }
            public double STENFAB1 { get; set; }
            public double STENFAB2 { get; set; }
            public double STREQFAB1 { get; set; }
            public double STREQFAB2 { get; set; }
            public double PMME { get; set; }
            public DateTime FEPMME { get; set; }
            public double VALUNFLEKM { get; set; }
            public bool ANALIZABLE { get; set; }
            public double TOLELOTE { get; set; }
            public double PMIFRS { get; set; }
            public DateTime FEPMIFRS { get; set; }
            public string KOENPR { get; set; }
            public string MIKOPR { get; set; }
            public bool CONTROLADO { get; set; }
            public bool CAMBIOSUJ { get; set; }
            public double PPUL01OC { get; set; }
            public double PPUL02OC { get; set; }
            public string MOULOC { get; set; }
            public string TIMOULOC { get; set; }
            public double TAULOC { get; set; }
            public DateTime FEULOC { get; set; }
            public string NOKOPRAMP { get; set; }


        }

    }

    
}