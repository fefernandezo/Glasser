using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de Plan1
/// 
/// </summary>
/// 


namespace PlanificacionOT
{
    public class VALIDADOR
    {
        #region Constantes
        string stringConeccion = "GLASSERConnection";
        SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
        static SqlDataReader drPlabal;
        static SqlDataReader drGlasser1;
        SqlCommand cmdGlasser1;
        SqlCommand cmdPlabal;
        #endregion

        
        public string TIPOOT(string KOMODE)
        {
            string Is;
            string Select = "SELECT TIPOOT FROM PLA_ASIGTYPEOT WHERE KOMODE=@KOMODE";
            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@KOMODE", KOMODE);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                Is = drPlabal["TIPOOT"].ToString().Trim();
            }
            else
            {
                Is = "NO";
            }

            drPlabal.Close();
            ConnPlabal.Close();


            return Is;
        }

       

        public void InsertASIGTYPEOT(string KOMODE, string TIPOOT)
        {
            #region InsertString
            string Insert = "Insert INTO PLA_ASIGTYPEOT VALUES (@KOMODE,@TIPOOT)";
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@KOMODE", KOMODE);
                cmdPlabal.Parameters.AddWithValue("@TIPOOT", TIPOOT);


                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        

    }

    public class PLA_OTING
    {
        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
        static SqlDataReader drPlabal;
        
       SqlCommand cmdPlabal;

        public void InsertPLA_OTING(PLA_OTINGList Lista, string UserName)
        {
            #region GetUserId
            Usuario usuario = new Usuario();
            string UserId = usuario.Info.Id;
            #endregion


            #region InsertString
            string Insert = "Insert INTO PLA_OTING VALUES (@NUMOT,GETDATE(),@Status,@MsgStatus,@UserId)";
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@NUMOT", Lista._NUMOT);
                cmdPlabal.Parameters.AddWithValue("@Status", Lista._Status);
                cmdPlabal.Parameters.AddWithValue("@MsgStatus", Lista._MsgStatus);
                cmdPlabal.Parameters.AddWithValue("@UserId", UserId);


                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public void UpdatePLA_OTING(PLA_OTINGList Lista)
        {
            #region InsertString
            string Insert = "UPDATE PLA_OTING SET Status=@Status,MsgStatus=@MsgStatus WHERE NUMOT=@NUMOT";
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@NUMOT", Lista._NUMOT);
                cmdPlabal.Parameters.AddWithValue("@Status", Lista._Status);
                cmdPlabal.Parameters.AddWithValue("@MsgStatus", Lista._MsgStatus);


                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

    }
   

    public class OTGeneric
    {
        string stringConeccion = "GLASSERConnection";
        SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
        static SqlDataReader drGlasser1;
        SqlCommand cmdGlasser1;
        VarEncNVV varEnc;

        public VarEncNVV VarEncNVV(string NVV)
        {
            string Select = "SELECT TOP 1 EDO.ESPRODDO, TI.NRODOCU AS 'NUMOP',DDO.FEEMLI, DDO.FEERLI," +
                " DDO.IDMAEDDO,DDO.IDMAEEDO, DDO.SULIDO, EEN.KOEN, EEN.NOKOEN, EDO.EMPRESA,EDO.TIDO,EDO.NUDO, EDO.LISACTIVA," +
                "EDO.SUDO, EDO.MODO, EDO.TIMODO, EDO.TAMODO,EDO.HORAGRAB,TI.KOMODE " +
                "FROM TINTERMO TI, MAEEDO EDO, MAEDDO DDO, MAEEN EEN WITH (NOLOCK) WHERE EDO.ENDO = EEN.KOEN AND" +
                " TI.IDMAEDDO = DDO.IDMAEDDO AND TI.IDMAEEDO = EDO.IDMAEEDO AND EDO.NUDO LIKE @NUDO AND EDO.TIDO='NVV'";
            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Select, ConnGlasser);
                cmdGlasser1.Parameters.AddWithValue("@NUDO", NVV);
                drGlasser1 = cmdGlasser1.ExecuteReader();
                drGlasser1.Read();

                if (drGlasser1.HasRows)
                {
                    varEnc = new VarEncNVV
                    {
                        IsTrue = true,
                        NUMOP = drGlasser1["NUMOP"].ToString().Trim(),
                        FEEMLI = Convert.ToDateTime(drGlasser1["FEEMLI"].ToString()),
                        SULIDO = drGlasser1["SULIDO"].ToString(),
                        FEERLI = Convert.ToDateTime(drGlasser1["FEERLI"].ToString()),
                        IDMAEEDO = drGlasser1["IDMAEEDO"].ToString(),
                        KOEN = drGlasser1["KOEN"].ToString(),
                        NOKOEN = drGlasser1["NOKOEN"].ToString(),
                        EMPRESA = drGlasser1["EMPRESA"].ToString(),
                        TIDO = drGlasser1["TIDO"].ToString(),
                        NUDO = drGlasser1["NUDO"].ToString().Trim(),
                        LISACTIVA = drGlasser1["LISACTIVA"].ToString(),
                        ESPRODO = drGlasser1["ESPRODDO"].ToString(),
                        SUDO = drGlasser1["SUDO"].ToString(),
                        MODO = drGlasser1["MODO"].ToString(),
                        TIMODO = drGlasser1["TIMODO"].ToString(),

                        HORAGRAB = drGlasser1["HORAGRAB"].ToString(),
                        KOMODE = drGlasser1["KOMODE"].ToString().Trim(),
                        NAPROV = 0.3,
                    };

                }
                else
                {
                    varEnc = new VarEncNVV { IsTrue = false, MsgIstrue = "El número ingresado no coincide con ninguna Nota de Venta." };
                }

                drGlasser1.Close();
                ConnGlasser.Close();

                if (varEnc.ESPRODO == "O")
                {
                    Select = "select TOP 1 NUMOT from POTLCOM WITH (NOLOCK) where NUMECOTI=@NUDO";
                    ConnGlasser.Open();
                    cmdGlasser1 = new SqlCommand(Select, ConnGlasser);
                    cmdGlasser1.Parameters.AddWithValue("@NUDO", varEnc.NUDO);
                    drGlasser1 = cmdGlasser1.ExecuteReader();
                    drGlasser1.Read();

                    if (drGlasser1.HasRows)
                    {
                        varEnc.NUMOT = drGlasser1["NUMOT"].ToString();
                        varEnc.MsgIstrue = "La Nota de Venta " + varEnc.NUDO + "<br/> tiene creada <p> la OT N° " + varEnc.NUMOT + "</p>";
                    }
                    drGlasser1.Close();
                    ConnGlasser.Close();
                }
                else if (string.IsNullOrWhiteSpace(varEnc.ESPRODO))
                {
                    varEnc.IsTrue = false;
                    varEnc.MsgIstrue = " La Nota de Venta" + varEnc.NUDO + "<br/> no tiene <strong>solicitud de producción</strong>";
                }
            }

            catch (Exception ex)
            {
                varEnc = new VarEncNVV { IsTrue = false, MsgIstrue = "Error en la conección : " + ex };
            }
            string Selectdolar = "select VAMO FROM TABMO WITH (NOLOCK) WHERE KOMO='US$'";
            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Selectdolar, ConnGlasser);
                
                drGlasser1 = cmdGlasser1.ExecuteReader();
                drGlasser1.Read();

                if (drGlasser1.HasRows)
                {

                    varEnc.TAMODO = Convert.ToDouble(drGlasser1["VAMO"].ToString());
                }
                else
                {
                  
                }

                drGlasser1.Close();
                ConnGlasser.Close();

               
               
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }

            return varEnc;
        }

        public List<VarItemNVV> VarItemNVVs(string NVV)
        {
            List<VarItemNVV> varItems = new List<VarItemNVV>();

            string consulta = "SELECT MA.IDMAEDDO AS '_IDMAEDDO', PDI.APROVECHAM, PDI.LARGOG, PDI.TP_DVH_COR AS '_TP_DVH_COR',"+
                 "PDI.BYTETOTAL, PDI.FACT_PORTE AS 'PDIFACT', PDI.CORR_VPVC AS '_CORR_VPVC',"+
                 "PDI.M_ANCHO, PDI.CORREINDS, PDI.CORRE_ARQ AS '_CORRE_ARQ', PDI.TP_TVH_COR AS '_TP_TVH_COR', PDI.M2MP01 AS '_M2MP01', "+
                "PDI.M2MP02 AS '_M2MP02', PDI.M2MP03 AS '_M2MP03', MA.NULIDO AS 'NREG', MA.NOKOPR, MA.UD01PR, MA.CAPRCO2, TI.IDTINTERMO, "+
                "TI.KOPR, PR.KOMODE, TI.CANTIDAD, TI.PRECIO, TI.PESOUBIC, TI.LTSUBIC, TI.ESPESOR, TI.ANCHO, TI.LARGO, TI.TIPOCOLOR, TI.ESPESO, "+
                "TI.FORMA_BOTT, TI.PERFORA, TI.DESTAJE, TI.ALAMINA, TI.LLAMINA, TI.VIDRIO_EX, TI.TPSEPARA12, TI.ANCHOSEP2, TI.ANCHOSEP, TI.VIDRIO_IN, "+
                "TI.SIPOLISUL, TI.SISILICON, TI.SIARGON, TI.SIVALVULA, TI.ITEM_ALFAK, TI.CODMP01, TI.CODMP02, TI.CODMP03, TI.CODCOLORES, TI.KODCOLORES "+
                "FROM MAEDDO MA, TINTERMO TI, PDIMEN PDI, MAEPR PR WITH(NOLOCK) WHERE TI.IDTINTERMO = MA.IDRST AND "+
                "MA.KOPRCT = TI.KOPR AND PDI.CODIGO = TI.KOPR AND TI.KOPR = PR.KOPR AND TI.IDMAEDDO = MA.IDMAEDDO AND MA.TIDO = 'NVV' AND TI.ANCHO <> 0 AND MA.NUDO LIKE @NUDO ORDER BY MA.IDMAEDDO ASC";

            DataTable TablaDetalle2 = new DataTable();

            using (SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[stringConeccion].ConnectionString))
            {
                

                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(consulta, Conn);
                    adaptador2.SelectCommand.Parameters.AddWithValue("@NUDO", NVV);

                    adaptador2.Fill(TablaDetalle2);
                    Conn.Close();

                }
                catch (Exception EX)
                {
                    
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + consulta, HttpContext.Current.Request.Url.ToString());
                }
                if (TablaDetalle2.Rows.Count > 0)
                {
                    foreach (DataRow drRAN in TablaDetalle2.Rows)
                    {
                        VarItemNVV varItemNVV = new VarItemNVV {
                            IDMAEDDO = drRAN["_IDMAEDDO"].ToString(),
                            APROVECHAM = drRAN["APROVECHAM"].ToString(),
                            CORR_VPVC= drRAN["_CORR_VPVC"].ToString(),
                            LARGOG = drRAN["LARGOG"].ToString(),
                            TP_DVH_COR = drRAN["_TP_DVH_COR"].ToString(),
                            BYTETOTAL = drRAN["BYTETOTAL"].ToString(),
                            M_ANCHO = drRAN["M_ANCHO"].ToString(),
                            CORREINDS = drRAN["CORREINDS"].ToString(),
                            NREG = drRAN["NREG"].ToString(),
                            NOKOPR = drRAN["NOKOPR"].ToString().Trim(),
                            IDTINTERMO = drRAN["IDTINTERMO"].ToString(),
                            KOPR = drRAN["KOPR"].ToString(),
                            KOMODE = drRAN["KOMODE"].ToString(),
                            CANTIDAD = Convert.ToInt32(drRAN["CANTIDAD"].ToString()),
                            PRECIO = drRAN["PRECIO"].ToString(),
                            PESOUBIC = drRAN["PESOUBIC"].ToString(),
                            LTSUBIC = drRAN["LTSUBIC"].ToString(),
                            ESPESOR = drRAN["ESPESOR"].ToString(),
                            ANCHO = drRAN["ANCHO"].ToString(),
                            LARGO = drRAN["LARGO"].ToString(),
                            TIPOCOLOR = drRAN["TIPOCOLOR"].ToString(),
                            ESPESO = drRAN["ESPESO"].ToString(),
                            
                            FORMA_BOTT = drRAN["FORMA_BOTT"].ToString(),
                            PERFORA = drRAN["PERFORA"].ToString(),
                            DESTAJE = drRAN["DESTAJE"].ToString(),
                            ALAMINA = drRAN["ALAMINA"].ToString(),
                            LLAMINA = drRAN["LLAMINA"].ToString(),
                            VIDRIO_EX = drRAN["VIDRIO_EX"].ToString(),
                            TPSEPARA12 = drRAN["TPSEPARA12"].ToString(),
                            ANCHOSEP2 = drRAN["ANCHOSEP2"].ToString(),
                            ANCHOSEP = drRAN["ANCHOSEP"].ToString(),
                            VIDRIO_IN = drRAN["VIDRIO_IN"].ToString(),
                            FACT_PORTE = drRAN["PDIFACT"].ToString(),
                            SIPOLISUL = drRAN["SIPOLISUL"].ToString(),
                            SISILICON = drRAN["SISILICON"].ToString(),
                            SIARGON = drRAN["SIARGON"].ToString(),
                            SIVALVULA = drRAN["SIVALVULA"].ToString(),
                            ITEM_ALFAK = drRAN["ITEM_ALFAK"].ToString(),
                            TP_TVH_COR = drRAN["_TP_TVH_COR"].ToString(),
                            CORRE_ARQ = drRAN["_CORRE_ARQ"].ToString(),
                            
                            
                            CODMP01 = drRAN["CODMP01"].ToString(),
                            CODMP02 = drRAN["CODMP02"].ToString(),
                            CODMP03 = drRAN["CODMP03"].ToString(),
                            M2MP01 = Convert.ToDouble(drRAN["_M2MP01"].ToString()),
                            M2MP02 = Convert.ToDouble(drRAN["_M2MP02"].ToString()),
                            M2MP03 = Convert.ToDouble(drRAN["_M2MP03"].ToString()),
                           
                            CODCOLORES = drRAN["CODCOLORES"].ToString(),
                            KODCOLORES = drRAN["KODCOLORES"].ToString(),
                            UD01PR = drRAN["UD01PR"].ToString(),
                            CAPRCO2= Convert.ToDouble(drRAN["CAPRCO2"].ToString()),
                        };
                        varItemNVV.CODNOMEN = BuscaCodNomen(varItemNVV.KOPR, varItemNVV.KOMODE);

                        varItems.Add(varItemNVV);

                    }
                }
                else
                {

                }
            }

            return varItems;
        }

        public List<VarInsumos> GetCostInsumos(string Codtipo, string IDTINTERMO, int CANTIDAD)
        {
            List<VarInsumos> Insumos = new List<VarInsumos>();
            #region SQLString
            string Select = "SELECT PNPD.*, MAEPR.NOKOPR,MAEPR.UD01PR,MAEPR.UD02PR,MAEPR.TIPR,MAEPR.DIVISIBLE," +
                "MAEPR.LOMIFA,MAEPR.LOMAFA,MAEPR.MUDEFA, MAEPR.KOPRDIM,MAEPR.NODIM1,MAEPR.NODIM2,PCOMODI.COMODIN AS XXCOMODIN " +
                "FROM PNPD WITH (NOLOCK) LEFT JOIN MAEPR   ON PNPD.ELEMENTO = MAEPR.KOPR " +
                "LEFT JOIN PCOMODI ON PNPD.ELEMENTO = PCOMODI.KOCOMO " +
                "LEFT JOIN PRELA ON PRELA.CODNOMEN = PNPD.CODIGO " +
                "LEFT JOIN TINTERMO ON TINTERMO.KOMODE = PRELA.CODIGO " +
                "WHERE TINTERMO.IDTINTERMO = @IDTINTERMO  ORDER BY PNPD.NREG";
            #endregion

            DataTable TablaDetalle2 = new DataTable();

            using (SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[stringConeccion].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(Select, Conn);
                    adaptador2.SelectCommand.Parameters.AddWithValue("@IDTINTERMO", IDTINTERMO);

                    adaptador2.Fill(TablaDetalle2);
                    Conn.Close();
                }
                catch (Exception EX)
                {
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " SELECT PNPD.*:IDTINTERMO:" + IDTINTERMO, HttpContext.Current.Request.Url.ToString());
                }
                if (TablaDetalle2.Rows.Count > 0)
                {
                    
                    foreach (DataRow drRAN in TablaDetalle2.Rows)
                    {
                        int index = TablaDetalle2.Rows.IndexOf(drRAN) + 1;
                        #region ReplaceinECUTPU
                        string ecuCANT;
                        if (drRAN["ECUCANT"].ToString().Contains("#7"))
                        {
                            ecuCANT = drRAN["ECUCANT"].ToString().Replace("#7", " ");
                        }
                        else if (drRAN["ECUCANT"].ToString().Contains("#E"))
                        {
                            ecuCANT = drRAN["ECUCANT"].ToString().Replace("#E", " ");
                        }
                        else
                        {
                            ecuCANT = drRAN["ECUCANT"].ToString();
                        }
                        #endregion

                        string SubNreg = index.ToString().PadLeft(5, '0');

                        VarInsumos varInsumos = new VarInsumos {
                            _SUBNREG = SubNreg,
                            _NREG = drRAN["NREG"].ToString(),
                            _OPERACION = drRAN["OPERACION"].ToString().Trim(),
                            _ECUCANT = ecuCANT,
                            _TIPO = drRAN["TIPO"].ToString(),
                            _ESMODELO = drRAN["ESMODELO"].ToString(),
                            _GLOSA = drRAN["NOKOPR"].ToString(),
                            _UNIDAD = drRAN["UD01PR"].ToString(),
                            _UNIDADC = drRAN["UD02PR"].ToString(),


                        };
                        

                        string ELEMENTO = drRAN["ELEMENTO"].ToString();




                        //cálculo de precio de insumos
                        string CodComp;
                        if (varInsumos._ESMODELO == "S")
                        {


                            string FUKOPR = GETfunokopr(ELEMENTO);


                            Char Delmm = '+';
                            String[] SUBFUKOPR = FUKOPR.Split(Delmm);
                            int Y = SUBFUKOPR.Count();
                            List<string> Lista = new List<string>();
                            foreach (var Substt in SUBFUKOPR)
                            {
                                Lista.Add(SubStt(Substt.Trim(), IDTINTERMO));

                            }

                            CodComp = string.Join("", Lista);
                            varInsumos._ELEMENTO = CodComp.Trim();



                        }
                        else
                        {
                            varInsumos._ELEMENTO = ELEMENTO;
                        }

                        
                        //CALCULO DEL C_INSUMO
                        DataTable TablaPreD = PrecioIFRS(varInsumos._ELEMENTO);
                        if (TablaPreD.Rows.Count > 0)
                        {
                            DataRow RowT = TablaPreD.Rows[0];
                            varInsumos.PMIFRS = Convert.ToDouble(RowT["PMIFRS"].ToString());
                            varInsumos._GLOSA = RowT["NOKOPR"].ToString();
                            varInsumos._UNIDAD= RowT["UD01PR"].ToString();
                            varInsumos._UNIDADC = RowT["UD02PR"].ToString();
                           

                        }

                        if (varInsumos._UNIDAD != varInsumos._UNIDADC)
                        {
                            varInsumos.DOBLEU = "1";
                        }
                        else
                        {
                            varInsumos.DOBLEU = "0";
                        }

                        //cálculo de cantidades de insumos
                        if (varInsumos._UNIDAD == "UN" && varInsumos._OPERACION!="115")
                        {
                            decimal variable = Math.Ceiling(Convert.ToDecimal(CalcEcuacion(ecuCANT, IDTINTERMO)));
                            varInsumos.CANTUNIT = variable;

                            variable = CANTIDAD * variable;
                            varInsumos.CANTIDADDF = Convert.ToDouble(variable.ToString());
                        }
                        else
                        {
                            decimal variable = Math.Round(Convert.ToDecimal(CalcEcuacion(ecuCANT, IDTINTERMO)), 3);
                            varInsumos.CANTUNIT = variable;

                            variable = CANTIDAD * variable;
                            varInsumos.CANTIDADDF = Convert.ToDouble(variable.ToString());
                        }


                        

                        varInsumos.STREQFAB2 = Math.Round(Convert.ToDouble(varInsumos.CANTIDADDF) / RLUD(varInsumos._ELEMENTO),2);


                        Insumos.Add(varInsumos);

                    }


                }
                else
                {

                }
            }


            return Insumos;
        }

        //OBTIENE UNA LISTA POR ITEM DE LA NVV CON LAS VARIABLES DE MAQUINAS
        public List<VarMaq> GetCostMOyMAQ(string CodTipo, string IDTINTERMO)
        {
            List<VarMaq> Maquina = new List<VarMaq>();
            #region SQLString
            string Select = "SELECT PNPR.CODIGO, PNPR.OPERACION,PNPR.ORDEN, PNPR.POROPANT, PNPR.PORREQCOMP, PNPR.SALPROXJOR, " +
                "POPER.UDAD,POPER.TIPOOP,POPER.CODMAQ,POPER.ECUTPU, POPER.ECUCUP,POPER.OBREROS, POPER.ECUCHMOP " +
                "FROM PNPR WITH(NOLOCK ) LEFT OUTER JOIN POPER ON PNPR.OPERACION = POPER.OPERACION " +
                "WHERE PNPR.CODIGO = @Codigo ORDER BY PNPR.CODIGO,PNPR.ORDEN";
            #endregion

            DataTable TablaDetalle2 = new DataTable();

            using (SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[stringConeccion].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(Select, Conn);
                    adaptador2.SelectCommand.Parameters.AddWithValue("@Codigo", CodTipo);

                    adaptador2.Fill(TablaDetalle2);
                    Conn.Close();
                }
                catch (Exception EX)
                {
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " SELECT PNPR.CODIGO:@Codigo"+ CodTipo, HttpContext.Current.Request.Url.ToString());
                }
                if (TablaDetalle2.Rows.Count > 0)
                {
                    foreach (DataRow drRAN in TablaDetalle2.Rows)
                    {
                        #region ReplaceinECUTPU
                        string ecutpu;
                        if (drRAN["ECUTPU"].ToString().Contains("#7"))
                        {
                            ecutpu = drRAN["ECUTPU"].ToString().Replace("#7", " ");
                        }
                        else if (drRAN["ECUTPU"].ToString().Contains("#E"))
                        {
                            ecutpu = drRAN["ECUTPU"].ToString().Replace("#E", " ");
                        }
                        else
                        {
                            ecutpu = drRAN["ECUTPU"].ToString();
                        }
                        #endregion

                        #region ReplaceinECUCUP
                        string ecucup;
                        if (drRAN["ECUCUP"].ToString().Contains("#7"))
                        {
                            ecucup = drRAN["ECUCUP"].ToString().Replace("#7", " ");
                        }
                        else if (drRAN["ECUCUP"].ToString().Contains("#E"))
                        {
                            ecucup = drRAN["ECUCUP"].ToString().Replace("#E", " ");
                        }
                        else
                        {
                            ecucup = drRAN["ECUCUP"].ToString();
                        }
                        #endregion

                        #region replaceenECUCHMOP
                        string ecuchmop;
                        if (drRAN["ECUCHMOP"].ToString().Contains("#7"))
                        {
                            ecuchmop = drRAN["ECUCHMOP"].ToString().Replace("#7", " ");
                        }
                        else if (drRAN["ECUCHMOP"].ToString().Contains("#E"))
                        {
                            ecuchmop = drRAN["ECUCHMOP"].ToString().Replace("#E", " ");
                        }
                        else
                        {
                            ecuchmop = drRAN["ECUCHMOP"].ToString();
                        }
                        #endregion


                        VarMaq varMaq = new VarMaq
                        {
                            CODIGO = drRAN["CODIGO"].ToString(),
                            OPERACION = drRAN["OPERACION"].ToString().Trim(),
                            ORDEN = drRAN["ORDEN"].ToString(),
                            POROPANT = drRAN["POROPANT"].ToString(),
                            PORREQCOMP = drRAN["PORREQCOMP"].ToString(),
                            SALPROXJOR = drRAN["SALPROXJOR"].ToString(),
                            UDAD = drRAN["UDAD"].ToString(),
                            TIPOOP = drRAN["TIPOOP"].ToString(),
                            CODMAQOT = drRAN["CODMAQ"].ToString(),
                            ECUTPU = ecutpu,
                            ECUCUP = ecucup,
                            OBREROS = drRAN["OBREROS"].ToString(),
                            ECUCHMOP = ecuchmop,

                        };
                        if (!string.IsNullOrWhiteSpace(varMaq.ECUCUP))
                        {
                            varMaq.C_MAQUINAS = Convert.ToDouble(CalcEcuacion(varMaq.ECUCUP, IDTINTERMO));
                        }
                        else
                        {
                            varMaq.C_MAQUINAS = 0;
                        }

                        if (!string.IsNullOrWhiteSpace(varMaq.ECUCHMOP))
                        {
                            double valorMO = Convert.ToDouble(varMaq.OBREROS) * Convert.ToDouble(CalcEcuacion(varMaq.ECUCHMOP, IDTINTERMO));
                            varMaq.C_M_OBRA = valorMO;
                        }
                        else
                        {
                            varMaq.C_M_OBRA = 0;
                        }
                        varMaq.C_FABRIC = varMaq.C_MAQUINAS + varMaq.C_M_OBRA + varMaq.C_INSUMOS;

                        Maquina.Add(varMaq);
                    }


                }
                else
                {

                }
            }



            return Maquina;
        }

        public string CalcEcuacion(string Ecuacion, string IDTINTERMO)
        {

            string Result;
            #region SQLString
            string StringSQL = "DECLARE @TEMP_TINTERMO_TABLE TABLE ( IDTINTERMO NVARCHAR(50), ANCHO FLOAT, LARGO FLOAT, ANCHOSEP FLOAT, " +
                "SISILICON FLOAT, " +
                "SIARGON FLOAT, " +
                "SIPOLISUL FLOAT, " +
                "M2MP01 FLOAT, " +
                "M2MP02 FLOAT, " +
                "M2MP03 FLOAT, " +
                "ESPESOR FLOAT, " +
                "FORMA_BOTT FLOAT, " +
                "NAPROV FLOAT NULL, " +
                "CODMP01 NVARCHAR(50), " +
                "CODMP02 NVARCHAR(50), " +
                "CODMP03 NVARCHAR(50), " +
                "TPSEPARA12 NVARCHAR(50), " +
                "SIVALVULA FLOAT, " +
                "DESTAJE FLOAT, " +
                "FACT_PORTE FLOAT, " +
                "PERFORA FLOAT) " +
                "INSERT INTO @TEMP_TINTERMO_TABLE " +
                "SELECT TI.IDTINTERMO,TI.ANCHO,TI.LARGO,TI.ANCHOSEP,TI.SISILICON, "+
                "TI.SIARGON,TI.SIPOLISUL,MEN.M2MP01, MEN.M2MP02,MEN.M2MP03,TI.ESPESOR, TI.FORMA_BOTT, "+
                "(SELECT TOP 1 VALOR FROM PNOMDIM WITH(NOLOCK )  WHERE CODIGO = 'NAPROV') AS 'NAPROV', "+
                "TI.CODMP01,TI.CODMP02,TI.CODMP03,TI.TPSEPARA12,TI.SIVALVULA,TI.DESTAJE, TI.FACT_PORTE,TI.PERFORA "+
                "FROM TINTERMO TI, PDIMEN MEN WHERE TI.KOPR = MEN.CODIGO AND TI.IDTINTERMO =@IDtintermo " +
                "SELECT (" + Ecuacion + ") FROM @TEMP_TINTERMO_TABLE ";
            #endregion

            if (!string.IsNullOrWhiteSpace(Ecuacion))
            {
                try
                {
                    ConnGlasser.Open();
                    cmdGlasser1 = new SqlCommand(StringSQL, ConnGlasser);
                    cmdGlasser1.Parameters.AddWithValue("@IDtintermo", IDTINTERMO);
                    drGlasser1 = cmdGlasser1.ExecuteReader();
                    drGlasser1.Read();

                    if (drGlasser1.HasRows)
                    {
                        Result = drGlasser1[0].ToString();

                    }
                    else
                    {
                        Result = "0";
                    }

                    drGlasser1.Close();
                    ConnGlasser.Close();
                }
                catch
                {
                    Result = "0";
                    drGlasser1.Close();
                    ConnGlasser.Close();
                }
            }
            else
            {
                Result = "0";
            }



            return Result;
        }

        public string SubStt(string subfukopr, string IDTINTERMO)
        {
            string result = "";
            if (subfukopr == "CODCOLOR01")
            {
                subfukopr = "CODCOLORES";
            }

            string StringSQL = "SELECT " + subfukopr + " FROM TINTERMO WITH (NOLOCK) WHERE IDTINTERMO =@IDtintermo ";

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(StringSQL, ConnGlasser);
                cmdGlasser1.Parameters.AddWithValue("@IDtintermo", IDTINTERMO);


                drGlasser1 = cmdGlasser1.ExecuteReader();
                drGlasser1.Read();

                if (drGlasser1.HasRows)
                {

                    result = drGlasser1[0].ToString();
                    DataTable subfuvar = GetVARcomp(subfukopr);
                    int pnlargo = 0;
                    if (subfuvar.Rows.Count > 0)
                    {
                        DataRow ROW = subfuvar.Rows[0];
                        string pnvalor = ROW["A"].ToString();
                        string PNlargo = ROW["B"].ToString();
                        pnlargo = Convert.ToInt32(PNlargo.Trim());
                    }


                    if (subfukopr == "ANCHOSEP")
                    {
                        int y = pnlargo - result.Length;
                        if (y > 0)
                        {
                            for (int i = 0; i < y; i++)
                            {
                                result = "0" + result;
                            }
                        }

                    }


                }
                else
                {
                    result = subfukopr;

                }

                drGlasser1.Close();
                ConnGlasser.Close();
            }
            catch
            {
                drGlasser1.Close();
                ConnGlasser.Close();
                if (subfukopr == "CODACEIT01")
                {
                    result = "QAPD00016";
                }
                else
                {
                    result = subfukopr;
                }
            }





            return result;

        }

        private DataTable GetVARcomp(string subfukopr)
        {
            DataTable TABLA = new DataTable();

            TABLA.Columns.Add("A", typeof(string));
            TABLA.Columns.Add("B", typeof(string));
            using (SqlConnection Conn2 = new SqlConnection(WebConfigurationManager.ConnectionStrings[stringConeccion].ConnectionString))
            {
                string Select5 = "SELECT VALOR AS 'A', LARGOOBLI AS 'B' FROM  PNOMDIM WITH (NOLOCK)  WHERE  CODIGO=@subfukopr";
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(Select5, Conn2);
                    adaptador2.SelectCommand.Parameters.AddWithValue("@subfukopr", subfukopr);

                    adaptador2.Fill(TABLA);
                    Conn2.Close();
                }
                catch (Exception ex)
                {
                    Conn2.Close();
                    TABLA.Rows.Add("0", "0");

                }


            }


            return TABLA;
        }


        public List<ListaTIPOOT> TIPOOT()
        {
            List<ListaTIPOOT> varItems = new List<ListaTIPOOT>();

            string consulta = "SELECT TIPOOT,NOTIPO, TIPOOT + ' -- ' + NOTIPO AS 'COMBINADO' FROM PTIPOOT  WITH ( NOLOCK ) ";

            DataTable TablaDetalle2 = new DataTable();

            using (SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[stringConeccion].ConnectionString))
            {
                SqlDataAdapter adaptador2 = new SqlDataAdapter(consulta, Conn);
               

                adaptador2.Fill(TablaDetalle2);

                
                if (TablaDetalle2.Rows.Count > 0)
                {
                    foreach (DataRow drRAN in TablaDetalle2.Rows)
                    {

                        ListaTIPOOT lista = new ListaTIPOOT {
                            _NOTIPO=drRAN["NOTIPO"].ToString().Trim(),
                            _TIPOOT=drRAN["TIPOOT"].ToString().Trim(),
                            _COMBINADO = drRAN["COMBINADO"].ToString().Trim(),
                        };

                        varItems.Add(lista);

                    }
                }
                else
                {

                }
            }

            return varItems;
        }



        public DataTable PrecioIFRS(string CodComp)
        {

            string StringSQL = "SELECT MAEPR.KOPR, MAEPREM.PMIFRS, MAEPR.NOKOPR, MAEPR.UD02PR, MAEPR.UD01PR, MAEPR.DIVISIBLE  " +
                "FROM MAEPREM, MAEPR WITH (NOLOCK) " +
                "WHERE MAEPREM.KOPR = MAEPR.KOPR AND MAEPREM.EMPRESA = '01' AND MAEPREM.KOPR =@codigo";
            DataTable TABLA = new DataTable();
            TABLA.Columns.Add("KOPR", typeof(string));
            TABLA.Columns.Add("PMIFRS", typeof(string));
            TABLA.Columns.Add("NOKOPR", typeof(string));
            TABLA.Columns.Add("UD02PR", typeof(string));
            TABLA.Columns.Add("UD01PR", typeof(string));
            TABLA.Columns.Add("DIVISIBLE", typeof(string));
            using (SqlConnection Conn2 = new SqlConnection(WebConfigurationManager.ConnectionStrings[stringConeccion].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador4 = new SqlDataAdapter(StringSQL, Conn2);
                    adaptador4.SelectCommand.Parameters.AddWithValue("@codigo", CodComp);

                    adaptador4.Fill(TABLA);
                    Conn2.Close();
                }
                catch (Exception EX)
                {
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " SELECT MAEPR.KOPR,:@Codigo" + CodComp, HttpContext.Current.Request.Url.ToString());
                }

            }



            return TABLA;
        }

        public double RLUD(string KOPR)
        {

            string StringSQL = "SELECT TOP 1 RLUD,TRATALOTE FROM MAEPR WITH ( NOLOCK )  WHERE KOPR=@KOPR";
            double VALOR = 0;
            ConnGlasser.Open();
            cmdGlasser1 = new SqlCommand(StringSQL, ConnGlasser);
            cmdGlasser1.Parameters.AddWithValue("@KOPR", KOPR);
            
            drGlasser1 = cmdGlasser1.ExecuteReader();
            drGlasser1.Read();

            if (drGlasser1.HasRows)
            {
                VALOR = Convert.ToDouble(drGlasser1[0].ToString());
            }

            drGlasser1.Close();
            ConnGlasser.Close();



            return VALOR;
        }


       

        private string BuscaCodNomen(string KOPR, string KOMODE)
        {
            string result = "";
            string Select = "SELECT PRELA.CODNOMEN FROM PRELA WITH(NOLOCK ) " +
                "INNER JOIN PNPE ON PNPE.CODIGO = PRELA.CODNOMEN AND PNPE.ESODD <> 'S'  " +
                "WHERE PRELA.CODIGO =@KOPR  OR PRELA.CODIGO =@KOMODE";
            ConnGlasser.Open();
            cmdGlasser1 = new SqlCommand(Select, ConnGlasser);
            cmdGlasser1.Parameters.AddWithValue("@KOPR", KOPR);
            cmdGlasser1.Parameters.AddWithValue("@KOMODE", KOMODE);
            drGlasser1 = cmdGlasser1.ExecuteReader();
            drGlasser1.Read();

            if (drGlasser1.HasRows)
            {
                result = drGlasser1[0].ToString();
            }

            drGlasser1.Close();
            ConnGlasser.Close();

            return result;
        }

        private string GETfunokopr(string ELEMENTO)
        {
            string result = "";
            string Select = "SELECT TOP 1 FUKOPR FROM TABMODE WITH ( NOLOCK ) WHERE KOMODE=@ELEMENTO ORDER BY KOMODE ";
            ConnGlasser.Open();
            cmdGlasser1 = new SqlCommand(Select, ConnGlasser);
            cmdGlasser1.Parameters.AddWithValue("@ELEMENTO", ELEMENTO);

            drGlasser1 = cmdGlasser1.ExecuteReader();
            drGlasser1.Read();

            if (drGlasser1.HasRows)
            {
                result = drGlasser1[0].ToString();
            }

            drGlasser1.Close();
            ConnGlasser.Close();


            return result;
        }





        //funciones de insert para la creacion de OT

        public string InsertPOTE(TablaPOTE POTE)
        {
            string VALOR = "";
            #region InsertString
            string Insert = "Insert INTO POTE VALUES (" +
                "@EMPRESA,@NUMOT,@ESTADO,@FIOT,@FTOT," +
                "@REFERENCIA,@PLANO,@EXTEN,@NUMPLAN,@SUOT," +
                "@KOFUCRE,@KOFUCIE,@HORAGRAB,@TIPOOT,@ESODD," +
                "@ENDO,@SUENDO,@FACTURAR,@KOFUFACTU,@MODO," +
                "@TIMODO,@TAMODO,@KOCT01,@POKOCT01,@KOCT02," +
                "@POKOCT02,@KOCT03,@POKOCT03,@DIASVIGCOT,@FORMAPAGO," +
                "@ENCARGADO,@CARGO,@KOPROYE,@KOETAPA,@OCDO,NULL," +
                "@FTESPROD,@PLANTAOT)";
            #endregion
            bool VALIDADOR = false;
            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", POTE._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@NUMOT", POTE._NUMOT);
                cmdGlasser1.Parameters.AddWithValue("@ESTADO", POTE._ESTADO);
                cmdGlasser1.Parameters.AddWithValue("@FIOT", POTE._FIOT);
                cmdGlasser1.Parameters.AddWithValue("@FTOT", POTE._FTOT);
                cmdGlasser1.Parameters.AddWithValue("@REFERENCIA", POTE._REFERENCIA);
                cmdGlasser1.Parameters.AddWithValue("@PLANO", POTE._PLANO);
                cmdGlasser1.Parameters.AddWithValue("@EXTEN", POTE._EXTEN);
                cmdGlasser1.Parameters.AddWithValue("@NUMPLAN", POTE._NUMPLAN);
                cmdGlasser1.Parameters.AddWithValue("@SUOT", POTE._SUOT);
                cmdGlasser1.Parameters.AddWithValue("@KOFUCRE", POTE._KOFUCRE);
                cmdGlasser1.Parameters.AddWithValue("@KOFUCIE", POTE._KOFUCIE);
                cmdGlasser1.Parameters.AddWithValue("@HORAGRAB", POTE._HORAGRAB);
                cmdGlasser1.Parameters.AddWithValue("@TIPOOT", POTE._TIPOOT);
                cmdGlasser1.Parameters.AddWithValue("@ESODD", POTE._ESODD);
                cmdGlasser1.Parameters.AddWithValue("@ENDO", POTE._ENDO);
                cmdGlasser1.Parameters.AddWithValue("@SUENDO", POTE._SUENDO);
                cmdGlasser1.Parameters.AddWithValue("@FACTURAR", POTE._FACTURAR);
                cmdGlasser1.Parameters.AddWithValue("@KOFUFACTU", POTE._KOFUFACTU);
                cmdGlasser1.Parameters.AddWithValue("@MODO", POTE._MODO);
                cmdGlasser1.Parameters.AddWithValue("@TIMODO", POTE._TIMODO);
                cmdGlasser1.Parameters.AddWithValue("@TAMODO", POTE._TAMODO);
                cmdGlasser1.Parameters.AddWithValue("@KOCT01", POTE._KOCT01);
                cmdGlasser1.Parameters.AddWithValue("@POKOCT01", POTE._POKOCT01);
                cmdGlasser1.Parameters.AddWithValue("@KOCT02", POTE._KOCT02);
                cmdGlasser1.Parameters.AddWithValue("@POKOCT02", POTE._POKOCT02);
                cmdGlasser1.Parameters.AddWithValue("@KOCT03", POTE._KOCT03);
                cmdGlasser1.Parameters.AddWithValue("@POKOCT03", POTE._POKOCT03);
                cmdGlasser1.Parameters.AddWithValue("@DIASVIGCOT", POTE._DIASVIGCOT);
                cmdGlasser1.Parameters.AddWithValue("@FORMAPAGO", POTE._FORMAPAGO);
                cmdGlasser1.Parameters.AddWithValue("@ENCARGADO ", POTE._ENCARGADO);
                cmdGlasser1.Parameters.AddWithValue("@CARGO", POTE._CARGO);
                cmdGlasser1.Parameters.AddWithValue("@KOPROYE", POTE._KOPROYE);
                cmdGlasser1.Parameters.AddWithValue("@KOETAPA", POTE._KOETAPA);
                cmdGlasser1.Parameters.AddWithValue("@OCDO", POTE._OCDO);
                cmdGlasser1.Parameters.AddWithValue("@FTESPROD", POTE._FTESPPROD);
                cmdGlasser1.Parameters.AddWithValue("@PLANTAOT", POTE._PLANTAOT);
                #endregion


                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
                VALIDADOR = true;
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
                VALOR = "ERROR1";
            }

            if (VALIDADOR)
            {
                string Select = "SELECT TOP 1 IDPOTE FROM POTE WITH (NOLOCK) WHERE NUMOT=@NUMOT AND KOFUCRE=@KOFUCRE AND TIPOOT=@TIPOOT AND REFERENCIA=@REFERENCIA ORDER BY IDPOTE ASC";
                try
                {
                    ConnGlasser.Open();
                    cmdGlasser1 = new SqlCommand(Select, ConnGlasser);
                    cmdGlasser1.Parameters.AddWithValue("@NUMOT", POTE._NUMOT);
                    cmdGlasser1.Parameters.AddWithValue("@REFERENCIA", POTE._REFERENCIA);
                    cmdGlasser1.Parameters.AddWithValue("@KOFUCRE", POTE._KOFUCRE);
                    cmdGlasser1.Parameters.AddWithValue("@TIPOOT", POTE._TIPOOT);
                    drGlasser1 = cmdGlasser1.ExecuteReader();
                    drGlasser1.Read();

                    if (drGlasser1.HasRows)
                    {
                        VALOR = drGlasser1[0].ToString();

                    }
                    else
                    {
                        VALOR = "ERROR2";
                    }

                    drGlasser1.Close();
                    ConnGlasser.Close();

                  
                }

                catch (Exception EX)
                {
                    VALOR = "ERROR2";
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " SELECT TOP 1 IDPOTE FROM POTE :NUMOT:" + POTE._NUMOT, HttpContext.Current.Request.Url.ToString());
                    ConnGlasser.Close();

                }
            }
            



            return VALOR;
        }

        public string InsertPOTL(TablaPOTL POTL)
        {
            string VALOR = "";
            bool VALIDADOR = false;
            #region InsertString
            string Insert = "Insert INTO POTL VALUES " +
            "(@IDPOTE,@EMPRESA,@NUMOT,@NREG,@ESTADO," +
            "@DESDE,@CODIGO,@UDAD,@CODNOMEN,@NUMECOTI," +
            "@NREGCOT,@FABRICAR,@REALIZADO,@DIFERENCIA,@MARCANOM," +
            "@NUMDEEST,@NIVEL,@GLOSA,@PLANO,@EXTEN," +
            "@NIVELSUP,@ESCADENA,@PORENTRAR,@NUMPLAN,@INFORABODE," +
            "@LILG,@NUMODC,@NREGODC,@SULIOTL,@BOLIOTL," +
            "@PORASIGNAR,@KOFUCRE,@KOFUCIE,GETDATE(),@C_FABRIC," +
            "@C_INSUMOS,@C_MAQUINAS,@C_M_OBRA,@ESODD,@VNSERVICIO," +
            "@P_FABRIC,@P_INSUMOS,@P_MAQUINAS,@P_M_OBRA,@CCFABRIC," +
            "@CCINSUMOS,@CCMAQUINAS,@CCM_OBRA,@PCFABRIC,@PCINSUMOS," +
            "@PCMAQUINAS,@PCM_OBRA,@PODESVNSER,@ECUVNSERVI,@OBSERVA," +
            "NULL,NULL,@PLANTA,@USOS,@PREFIJADO," +
            "@PODESINSU,@PODESMO,@PODESMAQ,@PODESSAL)";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);

                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@IDPOTE", POTL._IDPOTE);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", POTL._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@NUMOT", POTL._NUMOT);
                cmdGlasser1.Parameters.AddWithValue("@NREG", POTL._NREG);
                cmdGlasser1.Parameters.AddWithValue("@ESTADO", POTL._ESTADO);
                cmdGlasser1.Parameters.AddWithValue("@DESDE", POTL._DESDE);
                cmdGlasser1.Parameters.AddWithValue("@CODIGO", POTL._CODIGO);
                cmdGlasser1.Parameters.AddWithValue("@UDAD", POTL._UDAD);
                cmdGlasser1.Parameters.AddWithValue("@CODNOMEN", POTL._CODNOMEN);
                cmdGlasser1.Parameters.AddWithValue("@NUMECOTI", POTL._NUMECOTI);
                cmdGlasser1.Parameters.AddWithValue("@NREGCOT", POTL._NREGCOT);
                cmdGlasser1.Parameters.AddWithValue("@FABRICAR", POTL._FABRICAR);
                cmdGlasser1.Parameters.AddWithValue("@REALIZADO", POTL._REALIZADO);
                cmdGlasser1.Parameters.AddWithValue("@DIFERENCIA", POTL._DIFERENCIA);
                cmdGlasser1.Parameters.AddWithValue("@MARCANOM", POTL._MARCANOM);
                cmdGlasser1.Parameters.AddWithValue("@NUMDEEST", POTL._NUMDEEST);
                cmdGlasser1.Parameters.AddWithValue("@NIVEL", POTL._NIVEL);
                cmdGlasser1.Parameters.AddWithValue("@GLOSA", POTL._GLOSA);
                cmdGlasser1.Parameters.AddWithValue("@PLANO", POTL._PLANO);
                cmdGlasser1.Parameters.AddWithValue("@EXTEN", POTL._EXTEN);
                cmdGlasser1.Parameters.AddWithValue("@NIVELSUP", POTL._NIVELSUP);
                cmdGlasser1.Parameters.AddWithValue("@ESCADENA", POTL._ESCADENA);
                cmdGlasser1.Parameters.AddWithValue("@PORENTRAR", POTL._PORENTRAR);
                cmdGlasser1.Parameters.AddWithValue("@NUMPLAN", POTL._NUMPLAN);
                cmdGlasser1.Parameters.AddWithValue("@INFORABODE", POTL._INFORABODE);
                cmdGlasser1.Parameters.AddWithValue("@LILG", POTL._LILG);
                cmdGlasser1.Parameters.AddWithValue("@NUMODC", POTL._NUMODC);
                cmdGlasser1.Parameters.AddWithValue("@NREGODC", POTL._NREGODC);
                cmdGlasser1.Parameters.AddWithValue("@SULIOTL", POTL._SULIOTL);
                cmdGlasser1.Parameters.AddWithValue("@BOLIOTL", POTL._BOLIOTL);
                cmdGlasser1.Parameters.AddWithValue("@PORASIGNAR", POTL._PORASIGNAR);
                cmdGlasser1.Parameters.AddWithValue("@KOFUCRE", POTL._KOFUCRE);
                cmdGlasser1.Parameters.AddWithValue("@KOFUCIE", POTL._KOFUCIE);
                cmdGlasser1.Parameters.AddWithValue("@F_COSTEO", POTL._F_COSTEO);
                cmdGlasser1.Parameters.AddWithValue("@C_FABRIC", POTL._C_FABRIC);
                cmdGlasser1.Parameters.AddWithValue("@C_INSUMOS", POTL._C_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@C_MAQUINAS", POTL._C_MAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@C_M_OBRA", POTL._C_M_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@ESODD", POTL._ESODD);
                cmdGlasser1.Parameters.AddWithValue("@VNSERVICIO", POTL._VNSERVICIO);
                cmdGlasser1.Parameters.AddWithValue("@P_FABRIC", POTL._P_FABRIC);
                cmdGlasser1.Parameters.AddWithValue("@P_INSUMOS", POTL._P_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@P_MAQUINAS", POTL._P_MAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@P_M_OBRA", POTL._P_M_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@CCFABRIC", POTL._CCFABRIC);
                cmdGlasser1.Parameters.AddWithValue("@CCINSUMOS", POTL._CCINSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@CCMAQUINAS", POTL._CCMAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@CCM_OBRA", POTL._CCM_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@PCFABRIC", POTL._PCFABRIC);
                cmdGlasser1.Parameters.AddWithValue("@PCINSUMOS", POTL._PCINSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@PCMAQUINAS", POTL._PCMAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@PCM_OBRA", POTL._PCM_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@PODESVNSER", POTL._PODESVNSER);
                cmdGlasser1.Parameters.AddWithValue("@ECUVNSERVI", POTL._ECUVNSERVI);
                cmdGlasser1.Parameters.AddWithValue("@OBSERVA", POTL._OBSERVA);
                cmdGlasser1.Parameters.AddWithValue("@PLANTA", POTL._PLANTA);
                cmdGlasser1.Parameters.AddWithValue("@USOS", POTL._USOS);
                cmdGlasser1.Parameters.AddWithValue("@PREFIJADO", POTL._PREFIJADO);
                cmdGlasser1.Parameters.AddWithValue("@PODESINSU", POTL._PODESINSU);
                cmdGlasser1.Parameters.AddWithValue("@PODESMO", POTL._PODESMO);
                cmdGlasser1.Parameters.AddWithValue("@PODESMAQ", POTL._PODESMAQ);
                cmdGlasser1.Parameters.AddWithValue("@PODESSAL", POTL._PODESSAL);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
                VALIDADOR = true;
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
            }

            if(VALIDADOR)
            {
                string Select = "SELECT TOP 1 IDPOTL FROM POTL WITH (NOLOCK) WHERE IDPOTE=@IDPOTE AND CODIGO=@KOPR";
                try
                {
                    ConnGlasser.Open();
                    cmdGlasser1 = new SqlCommand(Select, ConnGlasser);
                    cmdGlasser1.Parameters.AddWithValue("@IDPOTE", POTL._IDPOTE);
                    cmdGlasser1.Parameters.AddWithValue("@KOPR", POTL._CODIGO);
                    
                    drGlasser1 = cmdGlasser1.ExecuteReader();
                    drGlasser1.Read();

                    if (drGlasser1.HasRows)
                    {
                        VALOR = drGlasser1[0].ToString();

                    }
                    else
                    {

                    }

                    drGlasser1.Close();
                    ConnGlasser.Close();


                }

                catch (Exception EX)
                {
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " SELECT IDPOTL FROM POTL:" + POTL._IDPOTE + " KOPR:" + POTL._CODIGO, HttpContext.Current.Request.Url.ToString());
                    ConnGlasser.Close();
                }
            }

            return VALOR;
        }

        public void InsertMAEST(TablaMAEST MAEST, string KOSU, string KOBO)
        {
            #region InsertString
            string Insert = "INSERT INTO MAEST " +
                "(EMPRESA,KOSU,KOBO,KOPR,STENFAB1,STENFAB2) VALUES" +
                "(@EMPRESA,@KOSU,@KOBO,@KOPR,@STENFAB1,@STENFAB2)";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", MAEST._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@KOSU", KOSU);
                cmdGlasser1.Parameters.AddWithValue("@KOBO", KOBO);
                cmdGlasser1.Parameters.AddWithValue("@KOPR", MAEST._KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB1", MAEST._STENFAB1);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB2", MAEST._STENFAB2);

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " INSERT INTO MAEST:EMPRESA:" + MAEST._EMPRESA + " KOSU:" + KOSU +
                    " KOBO:" + KOBO + " KOPR:" + MAEST._KOPR + " STENFAB1:" + MAEST._STENFAB1 + " STENFAB2:" + MAEST._STENFAB2, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
            }
        }

        public void InsertPDIMOT(TablaPDIMOT PDIMOT)
        {
            #region InsertString
            string Insert = "INSERT INTO PDIMOT VALUES (" +
                "@EMPRESA,@CODIGO,@NUMOT,@NREGOTL,@NOMBRE,@UDAD,@ANCHO," +
             "@CANT_M2,@CLAM180250,@CLAM200321,@CLAM220321,@CLAM223330," +
             "@CLAM225360,@CLAM240321,@CLAM240330,@CLAM244285,@CLAM244321," +
             "@CLAM244330,@CLAM250360,@LARGO,@M2_LAMINA,@PORC_AMAR," +
             "@PORC_AZUL,@PORC_BLCO,@PORC_ESME1,@PORC_ESME2,@PORC_NEGRO," +
             "@PORC_ORO,@PORC_ROJO,@PORC_VERDE,@RENDIM,@VELPULST," +
             "@PERFORA,@DESTAJE,@DLAM240321,@DLAM250360,@DLAM244330," +
             "@DLAM170220,@CLAM170220,@ESPESOR,@TONHRS,@CLAM190321," +
             "@V_BILAT_2,@M2AUN,@PRIMITIVO1,@PRIMITIVO2,@PORC_WDKGY," +
             "@PORC_OFWHT,@PORC_CRANB,@PORC_SNWHT,@PORC_VDFGR,@UN_JABA," +
             "@PERF_1,@PERF_2,@PERF_3,@ESQUINA_1,@ESQUINA_2," +
             "@FORMA_BOTT,@PRODUCTO,@DIAM_1,@DIAM_2,@DIAM_3," +
             "@COLOR_1,@COLOR_2,@COLOR_3,@COLOR_4,@COLOR_5," +
             "@COLOR_6,@COLOR_7,@MESCLA_1,@MESCLA_2,@ESQUINA_3," +
             "@ESQUINA_4,@PASADAS,@ESQUINA_5,@DESXPRO,@PERF_4," +
             "@DIAM_4,@CLAM220330,@VALOR_US,@MEZCLA_3,@MEZCLA_4," +
             "@MEZCLA_5,@MEZCLA_6,@PAPEL_1,@MUELA_1,@MUELA_2," +
             "@MUELA_3,@BROCA_1,@FRESA_1,@M2_C_SERG,@PITILLA," +
             "@MEZCLA_7,@MEZCLA_8,@BROCA_2,@CLAM152213,@CLAM160250," +
             "@CLAM183244,@COLOR_8,@COLOR_9,@COLOR_10,@COLOR_11," +
             "@COLOR_12,@CLAM360250,@AVELLANAD1,@AISLAPOL_1,@CINTA_EMBA," +
             "@SELLO_MET,@SEL_MET1,@ZUNCHO_MET,@ZUNCHO_PLA,@BOLSA_1," +
             "@CODCLIENTE,@SEPARADOR,@FILM_PLAS,@TIPOTEMPLA,@CLAM213330," +
             "@UNXH_CORT,@UNXH_PULI,@UNXH_PERF,@UNXH_,@UNXH_TEMP," +
             "@UNXH_PLAST,@LOTE_ENTRE,@LOTE_PINT,@LOTE_TEMP,@UNXH_VISOR," +
             "@HH_MAN_INT,@HH_MAN_EXT,@C_MO_IN,@C_MO_EXT,@APROVECHAM," +
             "@ESPESOR2,@LARGOG,@TP_DVH_COR,@ANCHOSEP,@SIPOLISUL," +
             "@SISILICON,@SIARGON,@SIVALVULA,@BYTETOTAL,@MERPROINS," +
             "@MARGTPNEL,@FACT_PORTE,@M_ANCHO,@CORREINDS,@CORRE_ARQ," +
             "@ANCHOSEP2,@TP_TVH_COR,@PED_ALFAK,@ITEM_ALFAK,@PDTO_ALFAK," +
             "@FPEDIDO_ALF,@CORRE_MAQ,@CORRE_REP,@UNXMP01,@UNXMP02," +
             "@UNXMP03,@UNAISLA01,@UNAISLA02,@UNAISLA03,@UNMADERA01," +
             "@UNMADERA02,@UNMADERA03,@CORRE_SEMI,@M2MP01,@M2MP02," +
             "@M2MP03,@M2CARTON,@ESPMP01,@ESPMP02,@ESPMP03," +
             "@CORRE_OBRA,@CORR_CTZA,@POMO_CTZA,@NPUE_CTZA,@NPAN_CTZA," +
             "@MLEN_CTZA,@M2LA_CTZA,@CORR_VPVC,@DRUR_CTZA,@FRUR_CTZA,@UNAML," +
             "0,0,0,0,0,0,0,0,0,0," +
             "0,0,0,0,0,0,0,0,0,0," +
             "0,0,0,0,0,0,0,0,0,0," +
             "0,0,0,0,0,0,0,0,0,0," +
             "0,0,0,0,0,0,0,0,0,0," +
             "0,0,0,0,0,0,0,0,0,0," +
             "0)";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", PDIMOT._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@CODIGO", PDIMOT._CODIGO);
                cmdGlasser1.Parameters.AddWithValue("@NUMOT", PDIMOT._NUMOT);
                cmdGlasser1.Parameters.AddWithValue("@NREGOTL", PDIMOT._NREGOTL);
                cmdGlasser1.Parameters.AddWithValue("@NOMBRE", PDIMOT._NOMBRE);
                cmdGlasser1.Parameters.AddWithValue("@UDAD", PDIMOT._UDAD);
                cmdGlasser1.Parameters.AddWithValue("@ANCHO", PDIMOT._ANCHO);
                cmdGlasser1.Parameters.AddWithValue("@CANT_M2", PDIMOT._CANT_M2);
                cmdGlasser1.Parameters.AddWithValue("@CLAM180250", PDIMOT._CLAM180250);
                cmdGlasser1.Parameters.AddWithValue("@CLAM200321", PDIMOT._CLAM200321);
                cmdGlasser1.Parameters.AddWithValue("@CLAM220321", PDIMOT._CLAM220321);
                cmdGlasser1.Parameters.AddWithValue("@CLAM223330", PDIMOT._CLAM223330);
                cmdGlasser1.Parameters.AddWithValue("@CLAM225360", PDIMOT._CLAM225360);
                cmdGlasser1.Parameters.AddWithValue("@CLAM240321", PDIMOT._CLAM240321);
                cmdGlasser1.Parameters.AddWithValue("@CLAM240330", PDIMOT._CLAM240330);
                cmdGlasser1.Parameters.AddWithValue("@CLAM244285", PDIMOT._CLAM244285);
                cmdGlasser1.Parameters.AddWithValue("@CLAM244321", PDIMOT._CLAM244321);
                cmdGlasser1.Parameters.AddWithValue("@CLAM244330", PDIMOT._CLAM244330);
                cmdGlasser1.Parameters.AddWithValue("@CLAM250360", PDIMOT._CLAM250360);
                cmdGlasser1.Parameters.AddWithValue("@LARGO", PDIMOT._LARGO);
                cmdGlasser1.Parameters.AddWithValue("@M2_LAMINA", PDIMOT._M2_LAMINA);
                cmdGlasser1.Parameters.AddWithValue("@PORC_AMAR", PDIMOT._PORC_AMAR);
                cmdGlasser1.Parameters.AddWithValue("@PORC_AZUL", PDIMOT._PORC_AZUL);
                cmdGlasser1.Parameters.AddWithValue("@PORC_BLCO", PDIMOT._PORC_BLCO);
                cmdGlasser1.Parameters.AddWithValue("@PORC_ESME1", PDIMOT._PORC_ESME1);
                cmdGlasser1.Parameters.AddWithValue("@PORC_ESME2", PDIMOT._PORC_ESME2);
                cmdGlasser1.Parameters.AddWithValue("@PORC_NEGRO", PDIMOT._PORC_NEGRO);
                cmdGlasser1.Parameters.AddWithValue("@PORC_ORO", PDIMOT._PORC_ORO);
                cmdGlasser1.Parameters.AddWithValue("@PORC_ROJO", PDIMOT._PORC_ROJO);
                cmdGlasser1.Parameters.AddWithValue("@PORC_VERDE", PDIMOT._PORC_VERDE);
                cmdGlasser1.Parameters.AddWithValue("@RENDIM", PDIMOT._RENDIM);
                cmdGlasser1.Parameters.AddWithValue("@VELPULST", PDIMOT._VELPULST);
                cmdGlasser1.Parameters.AddWithValue("@PERFORA", PDIMOT._PERFORA);
                cmdGlasser1.Parameters.AddWithValue("@DESTAJE", PDIMOT._DESTAJE);
                cmdGlasser1.Parameters.AddWithValue("@DLAM240321", PDIMOT._DLAM240321);
                cmdGlasser1.Parameters.AddWithValue("@DLAM250360", PDIMOT._DLAM250360);
                cmdGlasser1.Parameters.AddWithValue("@DLAM244330", PDIMOT._DLAM244330);
                cmdGlasser1.Parameters.AddWithValue("@DLAM170220", PDIMOT._DLAM170220);
                cmdGlasser1.Parameters.AddWithValue("@CLAM170220", PDIMOT._CLAM170220);
                cmdGlasser1.Parameters.AddWithValue("@ESPESOR", PDIMOT._ESPESOR);
                cmdGlasser1.Parameters.AddWithValue("@TONHRS", PDIMOT._TONHRS);
                cmdGlasser1.Parameters.AddWithValue("@CLAM190321", PDIMOT._CLAM190321);
                cmdGlasser1.Parameters.AddWithValue("@V_BILAT_2", PDIMOT._V_BILAT_2);
                cmdGlasser1.Parameters.AddWithValue("@M2AUN", PDIMOT._M2AUN);
                cmdGlasser1.Parameters.AddWithValue("@PRIMITIVO1", PDIMOT._PRIMITIVO1);
                cmdGlasser1.Parameters.AddWithValue("@PRIMITIVO2", PDIMOT._PRIMITIVO2);
                cmdGlasser1.Parameters.AddWithValue("@PORC_WDKGY", PDIMOT._PORC_WDKGY);
                cmdGlasser1.Parameters.AddWithValue("@PORC_OFWHT", PDIMOT._PORC_OFWHT);
                cmdGlasser1.Parameters.AddWithValue("@PORC_CRANB", PDIMOT._PORC_CRANB);
                cmdGlasser1.Parameters.AddWithValue("@PORC_SNWHT", PDIMOT._PORC_SNWHT);
                cmdGlasser1.Parameters.AddWithValue("@PORC_VDFGR", PDIMOT._PORC_VDFGR);
                cmdGlasser1.Parameters.AddWithValue("@UN_JABA", PDIMOT._UN_JABA);
                cmdGlasser1.Parameters.AddWithValue("@PERF_1", PDIMOT._PERF_1);
                cmdGlasser1.Parameters.AddWithValue("@PERF_2", PDIMOT._PERF_2);
                cmdGlasser1.Parameters.AddWithValue("@PERF_3", PDIMOT._PERF_3);
                cmdGlasser1.Parameters.AddWithValue("@ESQUINA_1", PDIMOT._ESQUINA_1);
                cmdGlasser1.Parameters.AddWithValue("@ESQUINA_2", PDIMOT._ESQUINA_2);
                cmdGlasser1.Parameters.AddWithValue("@FORMA_BOTT", PDIMOT._FORMA_BOTT);
                cmdGlasser1.Parameters.AddWithValue("@PRODUCTO", PDIMOT._PRODUCTO);
                cmdGlasser1.Parameters.AddWithValue("@DIAM_1", PDIMOT._DIAM_1);
                cmdGlasser1.Parameters.AddWithValue("@DIAM_2", PDIMOT._DIAM_2);
                cmdGlasser1.Parameters.AddWithValue("@DIAM_3", PDIMOT._DIAM_3);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_1", PDIMOT._COLOR_1);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_2", PDIMOT._COLOR_2);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_3", PDIMOT._COLOR_3);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_4", PDIMOT._COLOR_4);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_5", PDIMOT._COLOR_5);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_6", PDIMOT._COLOR_6);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_7", PDIMOT._COLOR_7);
                cmdGlasser1.Parameters.AddWithValue("@MESCLA_1", PDIMOT._MESCLA_1);
                cmdGlasser1.Parameters.AddWithValue("@MESCLA_2", PDIMOT._MESCLA_2);
                cmdGlasser1.Parameters.AddWithValue("@ESQUINA_3", PDIMOT._ESQUINA_3);
                cmdGlasser1.Parameters.AddWithValue("@ESQUINA_4", PDIMOT._ESQUINA_4);
                cmdGlasser1.Parameters.AddWithValue("@PASADAS", PDIMOT._PASADAS);
                cmdGlasser1.Parameters.AddWithValue("@ESQUINA_5", PDIMOT._ESQUINA_5);
                cmdGlasser1.Parameters.AddWithValue("@DESXPRO", PDIMOT._DESXPRO);
                cmdGlasser1.Parameters.AddWithValue("@PERF_4", PDIMOT._PERF_4);
                cmdGlasser1.Parameters.AddWithValue("@DIAM_4", PDIMOT._DIAM_4);
                cmdGlasser1.Parameters.AddWithValue("@CLAM220330", PDIMOT._CLAM220330);
                cmdGlasser1.Parameters.AddWithValue("@VALOR_US", PDIMOT._VALOR_US);
                cmdGlasser1.Parameters.AddWithValue("@MEZCLA_3", PDIMOT._MEZCLA_3);
                cmdGlasser1.Parameters.AddWithValue("@MEZCLA_4", PDIMOT._MEZCLA_4);
                cmdGlasser1.Parameters.AddWithValue("@MEZCLA_5", PDIMOT._MEZCLA_5);
                cmdGlasser1.Parameters.AddWithValue("@MEZCLA_6", PDIMOT._MEZCLA_6);
                cmdGlasser1.Parameters.AddWithValue("@PAPEL_1", PDIMOT._PAPEL_1);
                cmdGlasser1.Parameters.AddWithValue("@MUELA_1", PDIMOT._MUELA_1);
                cmdGlasser1.Parameters.AddWithValue("@MUELA_2", PDIMOT._MUELA_2);
                cmdGlasser1.Parameters.AddWithValue("@MUELA_3", PDIMOT._MUELA_3);
                cmdGlasser1.Parameters.AddWithValue("@BROCA_1", PDIMOT._BROCA_1);
                cmdGlasser1.Parameters.AddWithValue("@FRESA_1", PDIMOT._FRESA_1);
                cmdGlasser1.Parameters.AddWithValue("@M2_C_SERG", PDIMOT._M2_C_SERG);
                cmdGlasser1.Parameters.AddWithValue("@PITILLA", PDIMOT._PITILLA);
                cmdGlasser1.Parameters.AddWithValue("@MEZCLA_7", PDIMOT._MEZCLA_7);
                cmdGlasser1.Parameters.AddWithValue("@MEZCLA_8", PDIMOT._MEZCLA_8);
                cmdGlasser1.Parameters.AddWithValue("@BROCA_2", PDIMOT._BROCA_2);
                cmdGlasser1.Parameters.AddWithValue("@CLAM152213", PDIMOT._CLAM152213);
                cmdGlasser1.Parameters.AddWithValue("@CLAM160250", PDIMOT._CLAM160250);
                cmdGlasser1.Parameters.AddWithValue("@CLAM183244", PDIMOT._CLAM183244);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_8", PDIMOT._COLOR_8);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_9", PDIMOT._COLOR_9);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_10", PDIMOT._COLOR_10);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_11", PDIMOT._COLOR_11);
                cmdGlasser1.Parameters.AddWithValue("@COLOR_12", PDIMOT._COLOR_12);
                cmdGlasser1.Parameters.AddWithValue("@CLAM360250", PDIMOT._CLAM360250);
                cmdGlasser1.Parameters.AddWithValue("@AVELLANAD1", PDIMOT._AVELLANAD1);
                cmdGlasser1.Parameters.AddWithValue("@AISLAPOL_1", PDIMOT._AISLAPOL_1);
                cmdGlasser1.Parameters.AddWithValue("@CINTA_EMBA", PDIMOT._CINTA_EMBA);
                cmdGlasser1.Parameters.AddWithValue("@SELLO_MET", PDIMOT._SELLO_MET);
                cmdGlasser1.Parameters.AddWithValue("@SEL_MET1", PDIMOT._SEL_MET1);
                cmdGlasser1.Parameters.AddWithValue("@ZUNCHO_MET", PDIMOT._ZUNCHO_MET);
                cmdGlasser1.Parameters.AddWithValue("@ZUNCHO_PLA", PDIMOT._ZUNCHO_PLA);
                cmdGlasser1.Parameters.AddWithValue("@BOLSA_1", PDIMOT._BOLSA_1);
                cmdGlasser1.Parameters.AddWithValue("@CODCLIENTE", PDIMOT._CODCLIENTE);
                cmdGlasser1.Parameters.AddWithValue("@SEPARADOR", PDIMOT._SEPARADOR);
                cmdGlasser1.Parameters.AddWithValue("@FILM_PLAS", PDIMOT._FILM_PLAS);
                cmdGlasser1.Parameters.AddWithValue("@TIPOTEMPLA", PDIMOT._TIPOTEMPLA);
                cmdGlasser1.Parameters.AddWithValue("@CLAM213330", PDIMOT._CLAM213330);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_CORT", PDIMOT._UNXH_CORT);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_PULI", PDIMOT._UNXH_PULI);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_PERF", PDIMOT._UNXH_PERF);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_", PDIMOT._UNXH_PINT);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_TEMP", PDIMOT._UNXH_TEMP);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_PLAST", PDIMOT._UNXH_PLAST);
                cmdGlasser1.Parameters.AddWithValue("@LOTE_ENTRE", PDIMOT._LOTE_ENTRE);
                cmdGlasser1.Parameters.AddWithValue("@LOTE_PINT", PDIMOT._LOTE_PINT);
                cmdGlasser1.Parameters.AddWithValue("@LOTE_TEMP", PDIMOT._LOTE_TEMP);
                cmdGlasser1.Parameters.AddWithValue("@UNXH_VISOR", PDIMOT._UNXH_VISOR);
                cmdGlasser1.Parameters.AddWithValue("@HH_MAN_INT", PDIMOT._HH_MAN_INT);
                cmdGlasser1.Parameters.AddWithValue("@HH_MAN_EXT", PDIMOT._HH_MAN_EXT);
                cmdGlasser1.Parameters.AddWithValue("@C_MO_IN", PDIMOT._C_MO_INT);
                cmdGlasser1.Parameters.AddWithValue("@C_MO_EXT", PDIMOT._C_MO_EXT);
                cmdGlasser1.Parameters.AddWithValue("@APROVECHAM", PDIMOT._APROVECHAM);
                cmdGlasser1.Parameters.AddWithValue("@ESPESOR2", PDIMOT._ESPESOR2);
                cmdGlasser1.Parameters.AddWithValue("@LARGOG", PDIMOT._LARGOG);
                cmdGlasser1.Parameters.AddWithValue("@TP_DVH_COR", PDIMOT._TP_DVH_COR);
                cmdGlasser1.Parameters.AddWithValue("@ANCHOSEP", PDIMOT._ANCHOSEP);
                cmdGlasser1.Parameters.AddWithValue("@SIPOLISUL", PDIMOT._SIPOLISUL);
                cmdGlasser1.Parameters.AddWithValue("@SISILICON", PDIMOT._SISILICON);
                cmdGlasser1.Parameters.AddWithValue("@SIARGON", PDIMOT._SIARGON);
                cmdGlasser1.Parameters.AddWithValue("@SIVALVULA", PDIMOT._SIVALVULA);
                cmdGlasser1.Parameters.AddWithValue("@BYTETOTAL", PDIMOT._BYTETOTAL);
                cmdGlasser1.Parameters.AddWithValue("@MERPROINS", PDIMOT._MERPROINS);
                cmdGlasser1.Parameters.AddWithValue("@MARGTPNEL", PDIMOT._MARGTPNEL);
                cmdGlasser1.Parameters.AddWithValue("@FACT_PORTE", PDIMOT._FACT_PORTE);
                cmdGlasser1.Parameters.AddWithValue("@M_ANCHO", PDIMOT._M_ANCHO);
                cmdGlasser1.Parameters.AddWithValue("@CORREINDS", PDIMOT._CORREINDS);
                cmdGlasser1.Parameters.AddWithValue("@CORRE_ARQ", PDIMOT._CORRE_ARQ);
                cmdGlasser1.Parameters.AddWithValue("@ANCHOSEP2", PDIMOT._ANCHOSEP2);
                cmdGlasser1.Parameters.AddWithValue("@TP_TVH_COR", PDIMOT._TP_TVH_COR);
                cmdGlasser1.Parameters.AddWithValue("@PED_ALFAK", PDIMOT._PED_ALFAK);
                cmdGlasser1.Parameters.AddWithValue("@ITEM_ALFAK", PDIMOT._ITEM_ALFAK);
                cmdGlasser1.Parameters.AddWithValue("@PDTO_ALFAK", PDIMOT._PDTO_ALFAK);
                cmdGlasser1.Parameters.AddWithValue("@FPEDIDO_ALF", PDIMOT._FPEDIDO_ALF);
                cmdGlasser1.Parameters.AddWithValue("@CORRE_MAQ", PDIMOT._CORRE_MAQ);
                cmdGlasser1.Parameters.AddWithValue("@CORRE_REP", PDIMOT._CORRE_REP);
                cmdGlasser1.Parameters.AddWithValue("@UNXMP01", PDIMOT._UNXMP01);
                cmdGlasser1.Parameters.AddWithValue("@UNXMP02", PDIMOT._UNXMP02);
                cmdGlasser1.Parameters.AddWithValue("@UNXMP03", PDIMOT._UNXMP03);
                cmdGlasser1.Parameters.AddWithValue("@UNAISLA01", PDIMOT._UNAISLA01);
                cmdGlasser1.Parameters.AddWithValue("@UNAISLA02", PDIMOT._UNAISLA02);
                cmdGlasser1.Parameters.AddWithValue("@UNAISLA03", PDIMOT._UNAISLA03);
                cmdGlasser1.Parameters.AddWithValue("@UNMADERA01", PDIMOT._UNMADERA01);
                cmdGlasser1.Parameters.AddWithValue("@UNMADERA02", PDIMOT._UNMADERA02);
                cmdGlasser1.Parameters.AddWithValue("@UNMADERA03", PDIMOT._UNMADERA03);
                cmdGlasser1.Parameters.AddWithValue("@CORRE_SEMI", PDIMOT._CORRE_SEMI);
                cmdGlasser1.Parameters.AddWithValue("@M2MP01", PDIMOT._M2MP01);
                cmdGlasser1.Parameters.AddWithValue("@M2MP02", PDIMOT._M2MP02);
                cmdGlasser1.Parameters.AddWithValue("@M2MP03", PDIMOT._M2MP03);
                cmdGlasser1.Parameters.AddWithValue("@M2CARTON", PDIMOT._M2CARTON);
                cmdGlasser1.Parameters.AddWithValue("@ESPMP01", PDIMOT._ESPMP01);
                cmdGlasser1.Parameters.AddWithValue("@ESPMP02", PDIMOT._ESPMP02);
                cmdGlasser1.Parameters.AddWithValue("@ESPMP03", PDIMOT._ESPMP03);
                cmdGlasser1.Parameters.AddWithValue("@CORRE_OBRA", PDIMOT._CORRE_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@CORR_CTZA", PDIMOT._CORR_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@POMO_CTZA", PDIMOT._POMO_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@NPUE_CTZA", PDIMOT._NPUE_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@NPAN_CTZA", PDIMOT._NPAN_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@MLEN_CTZA", PDIMOT._MLEN_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@M2LA_CTZA", PDIMOT._M2LA_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@CORR_VPVC", PDIMOT._CORR_VPVC);
                cmdGlasser1.Parameters.AddWithValue("@DRUR_CTZA", PDIMOT._DRUR_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@FRUR_CTZA", PDIMOT._FRUR_CTZA);
                cmdGlasser1.Parameters.AddWithValue("@UNAML", PDIMOT._UNAML);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
            }
        }

        public void InsertPOTD(TablaPOTD POTD)
        {
            #region InsertString
            string Insert = "Insert INTO POTD VALUES (" +
                "@IDPOTL,@EMPRESA,@NUMOT,@NREG,@SUBNREG,@ESTADO," +
                "@PERTENECE,@NIVEL,@AUXI,@CODIGO,@MARCANOM," +
                "@CODNOMEN,@NUMDEEST,@GLOSA,@TIPOUNI,@DOBLEU," +
                "@CANTIDADF,@CANTIDACF,@CANTIDADR,@CANTANTI,@UNIDAD," +
                "@UNIDADC,@TIPO,@OPERACION,@CALIDAD,@NIVELSUP," +
                "@NUMPLAN,@LILG,@NUMODC,@NREGODC,@SULIOTD," +
                "@BOLIOTD,@PNPDNREG,@C_SALIDA,@C_INSUMOS,@P_INSUMOS," +
                "@CCINSUMOS,@PCINSUMOS)";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);

                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@IDPOTL", POTD._IDPOTL);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", POTD._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@NUMOT", POTD._NUMOT);
                cmdGlasser1.Parameters.AddWithValue("@NREG", POTD._NREG);
                cmdGlasser1.Parameters.AddWithValue("@SUBNREG", POTD._SUBNREG);
                cmdGlasser1.Parameters.AddWithValue("@ESTADO", POTD._ESTADO);
                cmdGlasser1.Parameters.AddWithValue("@PERTENECE", POTD._PERTENECE);
                cmdGlasser1.Parameters.AddWithValue("@NIVEL", POTD._NIVEL);
                cmdGlasser1.Parameters.AddWithValue("@AUXI", POTD._AUXI);
                cmdGlasser1.Parameters.AddWithValue("@CODIGO", POTD._CODIGO);
                cmdGlasser1.Parameters.AddWithValue("@MARCANOM", POTD._MARCANOM);
                cmdGlasser1.Parameters.AddWithValue("@CODNOMEN", POTD._CODNOMEN);
                cmdGlasser1.Parameters.AddWithValue("@NUMDEEST", POTD._NUMDEEST);
                cmdGlasser1.Parameters.AddWithValue("@GLOSA", POTD._GLOSA);
                cmdGlasser1.Parameters.AddWithValue("@TIPOUNI", POTD._TIPOUNI);
                cmdGlasser1.Parameters.AddWithValue("@DOBLEU", POTD._DOBLEU);
                cmdGlasser1.Parameters.AddWithValue("@CANTIDADF", POTD._CANTIDADF);
                cmdGlasser1.Parameters.AddWithValue("@CANTIDACF", POTD._CANTIDACF);
                cmdGlasser1.Parameters.AddWithValue("@CANTIDADR", POTD._CANTIDADR);
                cmdGlasser1.Parameters.AddWithValue("@CANTANTI", POTD._CANTANTI);
                cmdGlasser1.Parameters.AddWithValue("@UNIDAD", POTD._UNIDAD);
                cmdGlasser1.Parameters.AddWithValue("@UNIDADC", POTD._UNIDADC);
                cmdGlasser1.Parameters.AddWithValue("@TIPO", POTD._TIPO);
                cmdGlasser1.Parameters.AddWithValue("@OPERACION", POTD._OPERACION);
                cmdGlasser1.Parameters.AddWithValue("@CALIDAD", POTD._CALIDAD);
                cmdGlasser1.Parameters.AddWithValue("@NIVELSUP", POTD._NIVELSUP);
                cmdGlasser1.Parameters.AddWithValue("@NUMPLAN", POTD._NUMPLAN);
                cmdGlasser1.Parameters.AddWithValue("@LILG", POTD._LILG);
                cmdGlasser1.Parameters.AddWithValue("@NUMODC", POTD._NUMODC);
                cmdGlasser1.Parameters.AddWithValue("@NREGODC", POTD._NREGODC);
                cmdGlasser1.Parameters.AddWithValue("@SULIOTD", POTD._SULIOTD);
                cmdGlasser1.Parameters.AddWithValue("@BOLIOTD", POTD._BOLIOTD);
                cmdGlasser1.Parameters.AddWithValue("@PNPDNREG", POTD._PNPDNREG);
                cmdGlasser1.Parameters.AddWithValue("@C_SALIDA", POTD._C_SALIDA);
                cmdGlasser1.Parameters.AddWithValue("@C_INSUMOS", POTD._C_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@P_INSUMOS", POTD._P_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@CCINSUMOS", POTD._CCINSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@PCINSUMOS", POTD._PCINSUMOS);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " IDPOTL:" + POTD._IDPOTL, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
            }
        }

        public void InsertPOTLCOM(TablaPOTLCOM POTLCOM)
        {
            #region InsertString
            string Insert = "Insert INTO POTLCOM VALUES (" +
                "@IDPOTL,@ARCHIRST,@IDRST,@EMPRESA,@NUMOT," +
                "@NREGOTL,@DESDE,@NUMECOTI,@ENDO,@NREGCOT," +
                "@CODIGO,@UDAD,@FABRICAR,@REALIZADO,@DESISTIDO," +
                "@ASIGNADO,@ESODD,@ASIGNADO2)";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@IDPOTL", POTLCOM._IDPOTL);
                cmdGlasser1.Parameters.AddWithValue("@ARCHIRST", POTLCOM._ARCHIRST);
                cmdGlasser1.Parameters.AddWithValue("@IDRST", POTLCOM._IDRST);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", POTLCOM._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@NUMOT", POTLCOM._NUMOT);
                cmdGlasser1.Parameters.AddWithValue("@NREGOTL", POTLCOM._NREGOTL);
                cmdGlasser1.Parameters.AddWithValue("@DESDE", POTLCOM._DESDE);
                cmdGlasser1.Parameters.AddWithValue("@NUMECOTI", POTLCOM._NUMECOTI);
                cmdGlasser1.Parameters.AddWithValue("@ENDO", POTLCOM._ENDO);
                cmdGlasser1.Parameters.AddWithValue("@NREGCOT", POTLCOM._NREGCOT);
                cmdGlasser1.Parameters.AddWithValue("@CODIGO", POTLCOM._CODIGO);
                cmdGlasser1.Parameters.AddWithValue("@UDAD", POTLCOM._UDAD);
                cmdGlasser1.Parameters.AddWithValue("@FABRICAR", POTLCOM._FABRICAR);
                cmdGlasser1.Parameters.AddWithValue("@REALIZADO", POTLCOM._REALIZADO);
                cmdGlasser1.Parameters.AddWithValue("@DESISTIDO", POTLCOM._DESISTIDO);
                cmdGlasser1.Parameters.AddWithValue("@ASIGNADO", POTLCOM._ASIGNADO);
                cmdGlasser1.Parameters.AddWithValue("@ESODD", POTLCOM._ESODD);
                cmdGlasser1.Parameters.AddWithValue("@ASIGNADO2", POTLCOM._ASIGNADO2);

                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " IDPOTL:" + POTLCOM._IDPOTL, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
            }

        }

        public void InsertPOTPR(TablaPOTPR POTPR)
        {
            #region InsertString
            string Insert = "Insert INTO POTPR VALUES (" +
                "@IDPOTL,@EMPRESA,@NUMOT,@NREGOTL,@CODIGO," +
                "@OPERACION,@ORDEN,@POROPANT,@PORREQCOMP,@TIPOOP," +
                "@ESTADO,@SITPEDFAB,@FABRICAR,@REALIZADO,@SALPROXJOR," +
                "@PORMAQUILA,@LILG,@NUMODC,@NREGODC,@CODMAQOT," +
                "@C_FABRIC,@C_INSUMOS,@C_MAQUINAS,@C_M_OBRA,@P_FABRIC," +
                "@P_INSUMOS,@P_MAQUINAS,@P_M_OBRA,@CCFABRIC,@CCINSUMOS," +
                "@CCMAQUINAS,@CCM_OBRA,@PCFABRIC,@PCINSUMOS,@PCMAQUINAS," +
                "@PCM_OBRA,@NOTAS,@PERMAQALT,@PERMAQPAR)";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@IDPOTL", POTPR._IDPOTL);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", POTPR._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@NUMOT", POTPR._NUMOT);
                cmdGlasser1.Parameters.AddWithValue("@NREGOTL", POTPR._NREGOTL);
                cmdGlasser1.Parameters.AddWithValue("@CODIGO", POTPR._CODIGO);
                cmdGlasser1.Parameters.AddWithValue("@OPERACION", POTPR._OPERACION);
                cmdGlasser1.Parameters.AddWithValue("@ORDEN", POTPR._ORDEN);
                cmdGlasser1.Parameters.AddWithValue("@POROPANT", POTPR._POROPANT);
                cmdGlasser1.Parameters.AddWithValue("@PORREQCOMP", POTPR._PORREQCOMP);
                cmdGlasser1.Parameters.AddWithValue("@TIPOOP", POTPR._TIPOOP);
                cmdGlasser1.Parameters.AddWithValue("@ESTADO", POTPR._ESTADO);
                cmdGlasser1.Parameters.AddWithValue("@SITPEDFAB", POTPR._SITPEDFAB);
                cmdGlasser1.Parameters.AddWithValue("@FABRICAR", POTPR._FABRICAR);
                cmdGlasser1.Parameters.AddWithValue("@REALIZADO", POTPR._REALIZADO);
                cmdGlasser1.Parameters.AddWithValue("@SALPROXJOR", POTPR._SALPROXJOR);
                cmdGlasser1.Parameters.AddWithValue("@PORMAQUILA", POTPR._PORMAQUILA);
                cmdGlasser1.Parameters.AddWithValue("@LILG", POTPR._LILG);
                cmdGlasser1.Parameters.AddWithValue("@NUMODC", POTPR._NUMODC);
                cmdGlasser1.Parameters.AddWithValue("@NREGODC", POTPR._NREGODC);
                cmdGlasser1.Parameters.AddWithValue("@CODMAQOT", POTPR._CODMAQOT);
                cmdGlasser1.Parameters.AddWithValue("@C_FABRIC", POTPR._C_FABRIC);
                cmdGlasser1.Parameters.AddWithValue("@C_INSUMOS", POTPR._C_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@C_MAQUINAS", POTPR._C_MAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@C_M_OBRA", POTPR._C_M_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@P_FABRIC", POTPR._P_FABRIC);
                cmdGlasser1.Parameters.AddWithValue("@P_INSUMOS", POTPR._P_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@P_MAQUINAS", POTPR._P_MAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@P_M_OBRA", POTPR._P_M_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@CCFABRIC", POTPR._CCFABRIC);
                cmdGlasser1.Parameters.AddWithValue("@CCINSUMOS", POTPR._CCINSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@CCMAQUINAS", POTPR._CCMAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@CCM_OBRA", POTPR._CCM_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@PCFABRIC", POTPR._PCFABRIC);
                cmdGlasser1.Parameters.AddWithValue("@PCINSUMOS", POTPR._PCINSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@PCMAQUINAS", POTPR._PCMAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@PCM_OBRA", POTPR._PCM_OBRA);
                cmdGlasser1.Parameters.AddWithValue("@NOTAS", POTPR._NOTAS);
                cmdGlasser1.Parameters.AddWithValue("@PERMAQALT", POTPR._PERMAQALT);
                cmdGlasser1.Parameters.AddWithValue("@PERMAQPAR", POTPR._PERMAQPAR);


                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " IDPOTL:" + POTPR._IDPOTL, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }


        //funciones de update para la creacion de OT

        public void UpdateTABPRE(TablaTABPRE TABPRE)
        {
            #region InsertString
            string Insert = "UPDATE TABPRE " +
                "SET PP01UD =@PP01UD ," +
                "C_FABRIC =@C_FABRIC," +
                "C_INSUMOS=@C_INSUMOS," +
                "C_M_OBRA =@C_M_OBRA," +
                "C_MAQUINAS =@C_MAQUINAS " +
                "WHERE KOLT='01C' AND KOPR =@KOPR ";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@PP01UD", TABPRE._PP01UD);
                cmdGlasser1.Parameters.AddWithValue("@C_FABRIC", TABPRE._C_FABRIC);
                cmdGlasser1.Parameters.AddWithValue("@C_INSUMOS", TABPRE._C_INSUMOS);
                cmdGlasser1.Parameters.AddWithValue("@C_M_OBRA", TABPRE._C_MOBRA);
                cmdGlasser1.Parameters.AddWithValue("@C_MAQUINAS", TABPRE._C_MAQUINAS);
                cmdGlasser1.Parameters.AddWithValue("@KOPR", TABPRE._KOPR);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " UPDATE TABPRE:KOPR:" + TABPRE._KOPR, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEPRmaterial(string _KOPR, double _STREQFAB1, double _STREQFAB2)
        {
            #region InsertString
            string Insert = "UPDATE MAEPR SET  " +
                "STREQFAB1=STREQFAB1 + @STREQFAB1," +
                "STREQFAB2=STREQFAB2 + @STREQFAB2 " +
                "WHERE KOPR = @KOPR ";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@KOPR", _KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STREQFAB1",Math.Round(_STREQFAB1,3));
                cmdGlasser1.Parameters.AddWithValue("@STREQFAB2", Math.Round(_STREQFAB2,3));
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:UPDATE MAEPR SET STREQFAB1=STREQFAB1 +" + _STREQFAB1 +
                    ",STREQFAB2=STREQFAB2+" +_STREQFAB2 +" WHERE KOPR=" + _KOPR, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAESTmaterial(string _KOPR, double _STREQFAB1, double _STREQFAB2, string EMPRESA, string KOSU, string KOBO)
        {
            #region InsertString
            string Insert = "UPDATE MAEST SET  " +
                "STREQFAB1=STREQFAB1 + @STREQFAB1," +
                "STREQFAB2=STREQFAB2 + @STREQFAB2 " +
                "WHERE EMPRESA=@EMPRESA AND  KOSU=@KOSU AND  KOBO=@KOBO AND  KOPR=@KOPR";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@KOPR", _KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STREQFAB1", _STREQFAB1);
                cmdGlasser1.Parameters.AddWithValue("@STREQFAB2", _STREQFAB2);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@KOSU", KOSU);
                cmdGlasser1.Parameters.AddWithValue("@KOBO", KOBO);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert + "valores:" +_KOPR + "-" + _STREQFAB1 + "-" + _STREQFAB2 + "-" + EMPRESA + "-" + KOSU + "-" + KOBO , HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEPRproducto(string _KOPR, double _STENFAB1, double _STENFAB2)
        {
            #region InsertString
            string Insert = "UPDATE MAEPR SET  " +
                "STENFAB1=STENFAB1 + @STENFAB1," +
                "STENFAB2=STENFAB2 + @STENFAB2 " +
                "WHERE KOPR = @KOPR ";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@KOPR", _KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB1", _STENFAB1);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB2", _STENFAB2);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEPREMproducto(string _KOPR, double _STENFAB1, double _STENFAB2, string _EMPRESA)
        {
            #region InsertString
            string Insert = "UPDATE MAEPREM SET  " +
                "STENFAB1=STENFAB1 + @STENFAB1," +
                "STENFAB2=STENFAB2 + @STENFAB2 " +
                "WHERE EMPRESA=@EMPRESA AND KOPR = @KOPR ";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@KOPR", _KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB1", Math.Round(_STENFAB1,3));
                cmdGlasser1.Parameters.AddWithValue("@STENFAB2", Math.Round(_STENFAB2,3));
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", _EMPRESA);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:UPDATE MAEPREM SET STENFAB1=STENFAB1 +" + _STENFAB1 +
                    ",STENFAB2=STENFAB2+" + _STENFAB2 + " WHERE KOPR=" + _KOPR, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEPREMmaterial(string _KOPR, double _STREQFAB1, double _STREQFAB2, string _EMPRESA)
        {
            #region InsertString
            string Insert = "UPDATE MAEPREM SET  " +
                "STREQFAB1=STREQFAB1 + @STREQFAB1," +
                "STREQFAB2=STREQFAB2 + @STREQFAB2 " +
                "WHERE EMPRESA=@EMPRESA AND KOPR = @KOPR ";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@KOPR", _KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STREQFAB1", _STREQFAB1);
                cmdGlasser1.Parameters.AddWithValue("@STREQFAB2", _STREQFAB2);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", _EMPRESA);
                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:'" + _KOPR + "' " + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEEDO(string NUDO, string ENDO, string ESPRODDO, string ESFADO, string ESDO, double CAPRCO, double CAPRAD, string CAPREX)
        {
            #region InsertString
            string Insert = "UPDATE MAEEDO SET " +
                "CAPRCO=@CAPRCO," +
                "CAPRAD=@CAPRAD," +
                "CAPREX=@CAPREX," +
                "ESDO=@ESDO," +
                "ESFADO=@ESFADO, " +
                "ESPRODDO=@ESPRODDO  " +
                "WHERE TIDO='NVV' AND NUDO=@NUDO AND ENDO=@ENDO";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@CAPRCO", CAPRCO);
                cmdGlasser1.Parameters.AddWithValue("@CAPRAD", CAPRAD);
                cmdGlasser1.Parameters.AddWithValue("@CAPREX", CAPREX);
                cmdGlasser1.Parameters.AddWithValue("@ESDO", ESDO);
                cmdGlasser1.Parameters.AddWithValue("@ESFADO",ESFADO);
                cmdGlasser1.Parameters.AddWithValue("@ESPRODDO", ESPRODDO);
                cmdGlasser1.Parameters.AddWithValue("@NUDO", NUDO);
                cmdGlasser1.Parameters.AddWithValue("@ENDO", ENDO);

                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEDDO(string CAPRODEX, string IDMAEDDO)
        {
            #region InsertString
            string Insert = "UPDATE MAEDDO SET " +
                "CAPRODEX=CAPRODEX + @CAPRODEX," +
                "ESPRODLI=CASE  WHEN CAPRODEX-CAPRODAD-CAPRODRE+ @CAPRODEX=0 THEN 'F'  ELSE 'O'  END" +
                "  WHERE IDMAEDDO=@IDMAEDDO";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                #region Parameters
                cmdGlasser1.Parameters.AddWithValue("@CAPRODEX", CAPRODEX);
                cmdGlasser1.Parameters.AddWithValue("@IDMAEDDO", IDMAEDDO);
                

                #endregion

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();

            }
        }

        public void UpdateMAEST(TablaMAEST MAEST)
        {
            #region InsertString
            string Insert = "UPDATE MAEST SET " +
                " STENFAB1=STENFAB1 + @STENFAB1," +
                " STENFAB2=STENFAB2 + @STENFAB2 " +
                "WHERE EMPRESA=@EMPRESA AND KOSU=@KOSU AND KOBO=@KOBO AND KOPR=@KOPR";
            #endregion

            try
            {
                ConnGlasser.Open();
                cmdGlasser1 = new SqlCommand(Insert, ConnGlasser);
                cmdGlasser1.Parameters.AddWithValue("@EMPRESA", MAEST._EMPRESA);
                cmdGlasser1.Parameters.AddWithValue("@KOSU", MAEST._KOSU);
                cmdGlasser1.Parameters.AddWithValue("@KOBO", MAEST._KOBO);
                cmdGlasser1.Parameters.AddWithValue("@KOPR", MAEST._KOPR);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB1", MAEST._STENFAB1);
                cmdGlasser1.Parameters.AddWithValue("@STENFAB2", MAEST._STENFAB2);

                cmdGlasser1.ExecuteNonQuery();
                ConnGlasser.Close();
            }
            catch (Exception EX)
            {
                
                ErrorCatching errorCatching = new ErrorCatching();
                errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                ConnGlasser.Close();
            }
        }

    }

    public class VarEncNVV
    {
        public bool IsTrue { get; set; }
        public string NUMOP { get; set; }
        public DateTime FEEMLI { get; set; }
        public DateTime FEERLI { get; set; }
        public string IDMAEEDO { get; set; }
        public string KOEN { get; set; }
        public string NOKOEN { get; set; }
        public string EMPRESA { get; set; }
        public string TIDO { get; set; }
        public string NUDO { get; set; }
        public string LISACTIVA { get; set; }
        public string MsgIstrue { get; set; }
        public string ESPRODO { get; set; }
        public string NUMOT { get; set; }
        public double NAPROV { get; set; }
        public string SUDO { get; set; }
        public string MODO { get; set; }
        public string TIMODO { get; set; }
        public double TAMODO { get; set; }
        public string HORAGRAB { get; set; }
        public string KOMODE { get; set; }
        public string SULIDO { get; set; }
        public string _TIPOOT { get; set; }
    }
    public class VarItemNVV
    {
        public string NREG { get; set; }
        public string NOKOPR { get; set; }
        public string IDTINTERMO { get; set; }
        public string KOPR { get; set; }
        public string KOMODE { get; set; }
        public int CANTIDAD { get; set; }
        public string PRECIO { get; set; }
        public string PESOUBIC { get; set; }
        public string LTSUBIC { get; set; }
        public string ESPESOR { get; set; }
        public string ANCHO { get; set; }
        public string LARGO { get; set; }
        public string TIPOCOLOR { get; set; }
        public string ESPESO { get; set; }
        public string IDMAEDDO { get; set; }
        public string FORMA_BOTT { get; set; }
        public string PERFORA { get; set; }
        public string DESTAJE { get; set; }
        public string ALAMINA { get; set; }
        public string LLAMINA { get; set; }
        public string VIDRIO_EX { get; set; }
        public string TPSEPARA12 { get; set; }
        public string ANCHOSEP2 { get; set; }
        public string ANCHOSEP { get; set; }
        public string VIDRIO_IN { get; set; }
        public string FACT_PORTE { get; set; }
        public string SIPOLISUL { get; set; }
        public string SISILICON { get; set; }
        public string SIARGON { get; set; }
        public string SIVALVULA { get; set; }
        public string ITEM_ALFAK { get; set; }
        public string TP_TVH_COR { get; set; }
        public string TP_DVH_COR { get; set; }
        public string CORRE_ARQ { get; set; }
        
        
        public string CODMP01 { get; set; }
        public string CODMP02 { get; set; }
        public string CODMP03 { get; set; }
        public double M2MP01 { get; set; }
        public double M2MP02 { get; set; }
        public double M2MP03 { get; set; }
        
        public string CODCOLORES { get; set; }
        public string KODCOLORES { get; set; }
        public string CODNOMEN { get; set; }
        public string UD01PR { get; set; }
        public double CAPRCO2 { get; set; }
        public string APROVECHAM { get; set; }
        public string LARGOG { get; set; }
        public string BYTETOTAL { get; set; }
        public string M_ANCHO { get; set; }
        public string CORREINDS { get; set; }
        public string CORR_VPVC { get; set; }





    }

    public class VarMaq
    {
        public string CODIGO { get; set; }
        public string OPERACION { get; set; }
        public string ORDEN { get; set; }
        public string POROPANT { get; set; }
        public string PORREQCOMP { get; set; }
        public string TIPOOP { get; set; }
        public string FABRICAR { get; set; }
        public string REALIZADO { get; set; }
        public string SALPROXJOR { get; set; }
        public string PORMAQUILA { get; set; }
        public string CODMAQOT { get; set; }
        public double C_FABRIC { get; set; }
        public double C_INSUMOS { get; set; }
        public double C_MAQUINAS { get; set; }
        public double C_M_OBRA { get; set; }
        public string ECUTPU { get; set; }
        public string ECUCUP { get; set; }
        public string OBREROS { get; set; }
        public string ECUCHMOP { get; set; }
        public string UDAD { get; set; }



    }

    public class VarInsumos
    {
        //variables iniciales
        public string _SUBNREG { get; set; }
        public string _NREG { get; set; }
        public string _ELEMENTO { get; set; }
        public string _OPERACION { get; set; }
        public string _ECUCANT { get; set; }
        public string _TIPO { get; set; }
        public string _ESMODELO { get; set; }
        public string _GLOSA { get; set; }
        public string _UNIDAD { get; set; }
        public string _UNIDADC { get; set; }

        //calculados
        public double PMIFRS { get; set; }
        public double CANTIDADDF { get; set; }
        public decimal CANTUNIT { get; set; }
        public double STREQFAB2 { get; set; }


        public string ESTADO { get; set; }
        public string PERTENECE { get; set; }
        public string NIVEL { get; set; }
        public string MARCANOM { get; set; }
        public string CODNOMEN { get; set; }
        public string TIPOUNI { get; set; }
        public string DOBLEU { get; set; }
        
        
        public string C_INSUMOS { get; set; }
        
        



    }

    


    //Tablas que van a dar la DB
    public class TablaPOTE
    {
        public string IDPOTE { get; set; }
        public string _EMPRESA { get; set; }
        public string _NUMOT { get; set; }
        public string _ESTADO { get; set; }
        public DateTime _FIOT { get; set; }
        public DateTime _FTOT { get; set; }
        public string _REFERENCIA { get; set;}
        public string _PLANO { get; set; }
        public string _EXTEN { get; set; }
        public string _NUMPLAN { get; set; }
        public string _SUOT { get; set; }
        public string _KOFUCRE { get; set; }
        public string _KOFUCIE { get; set; }
        public string _HORAGRAB { get; set; }
        public string _TIPOOT { get; set; }
        public string _ESODD { get; set; }
        public string _ENDO { get; set; }
        public string _SUENDO { get; set; }
        public bool _FACTURAR { get; set; }
        public string _KOFUFACTU { get; set; }
        public string _MODO { get; set; }
        public string _TIMODO { get; set; }
        public double _TAMODO { get; set; }
        public string _KOCT01 { get; set; }
        public int _POKOCT01 { get; set; }
        public string _KOCT02 { get; set; }
        public int _POKOCT02 { get; set; }
        public string _KOCT03 { get; set; }
        public int _POKOCT03 { get; set; }
        public int _DIASVIGCOT { get; set; }
        public string _FORMAPAGO { get; set; }
        public string _ENCARGADO { get; set; }
        public string _CARGO { get; set; }
        public string _KOPROYE { get; set; }
        public string _KOETAPA { get; set; }
        public string _OCDO { get; set; }
        public DateTime _FTESPPROD { get; set; }
        public string _PLANTAOT { get; set; }

    }

    public class TablaPOTL
    {
        
        public string _IDPOTE { get; set; }
        public string _EMPRESA { get; set; }
        public string _NUMOT { get; set; }
        public string _NREG { get; set; }
        public string _ESTADO { get; set; }
        public string _DESDE { get; set; }
        public string _CODIGO { get; set; }
        public string _UDAD { get; set; }
        public string _CODNOMEN { get; set; }
        public string _NUMECOTI { get; set; }
        public string _NREGCOT { get; set; }
        public int _FABRICAR { get; set; }
        public int _REALIZADO { get; set; }
        public int _DIFERENCIA { get; set; }
        public string _MARCANOM { get; set; }
        public int _NUMDEEST { get; set; }
        public int _NIVEL { get; set; }
        public string _GLOSA { get; set; }
        public string _PLANO { get; set; }
        public string _EXTEN { get; set; }
        public string _NIVELSUP { get; set; }
        public string _ESCADENA { get; set; }
        public int _PORENTRAR { get; set; }
        public string _NUMPLAN { get; set; }
        public string _INFORABODE { get; set; }
        public string _LILG { get; set; }
        public string _NUMODC { get; set; }
        public string _NREGODC { get; set; }
        public string _SULIOTL { get; set; }
        public string _BOLIOTL { get; set; }
        public string _PORASIGNAR { get; set; }
        public string _KOFUCRE { get; set; }
        public string _KOFUCIE { get; set; }
        public DateTime _F_COSTEO { get; set; }
        public double _C_FABRIC { get; set; }
        public double _C_INSUMOS { get; set; }
        public double _C_MAQUINAS { get; set; }
        public double _C_M_OBRA { get; set; }
        public string _ESODD { get; set; }
        public int _VNSERVICIO { get; set; }
        public int _P_FABRIC { get; set; }
        public int _P_INSUMOS { get; set; }
        public int _P_MAQUINAS { get; set; }
        public int _P_M_OBRA { get; set; }
        public int _CCFABRIC { get; set; }
        public int _CCINSUMOS { get; set; }
        public int _CCMAQUINAS { get; set; }
        public int _CCM_OBRA { get; set; }
        public int _PCFABRIC { get; set; }
        public int _PCINSUMOS { get; set; }
        public int _PCMAQUINAS { get; set; }
        public int _PCM_OBRA { get; set; }
        public int _PODESVNSER { get; set; }
        public string _ECUVNSERVI { get; set; }
        public string _OBSERVA { get; set; }
        
        public int _PLANTA { get; set; }
        public int _USOS { get; set; }
        public string _PREFIJADO { get; set; }
        public int _PODESINSU { get; set; }
        public int _PODESMO { get; set; }
        public int _PODESMAQ { get; set; }
        public int _PODESSAL { get; set; }


    }

    public class TablaTABPRE
    {
        public string _PP01UD { get; set; }
        public double _C_FABRIC { get; set; }
        public double _C_INSUMOS { get; set; }
        public double _C_MOBRA { get; set; }
        public double _C_MAQUINAS { get; set; }
        public string _KOLT { get; set; }
        public string _KOPR { get; set; }
    }

       
    public class TablaMAEST
    {
        
        public string _EMPRESA { get; set; }
        public string _KOSU { get; set; }
        public string _KOBO { get; set; }
        public string _KOPR { get; set; }
        public double _STENFAB1 { get; set; }
        public double _STENFAB2 { get; set; }
    }

    public class TablaPDIMOT
    {
        
        public string _EMPRESA { get; set; }
        public string _CODIGO { get; set; }
        public string _NUMOT { get; set; }
        public string _NREGOTL { get; set; }
        public string _NOMBRE { get; set; }
        public string _UDAD { get; set; }
        public string _ANCHO { get; set; }
        public string _CANT_M2 { get; set; }
        public int _CLAM180250 { get; set; }
        public int _CLAM200321 { get; set; }
        public int _CLAM220321 { get; set; }
        public int _CLAM223330 { get; set; }
        public int _CLAM225360 { get; set; }
        public int _CLAM240321 { get; set; }
        public int _CLAM240330 { get; set; }
        public int _CLAM244285 { get; set; }
        public int _CLAM244321 { get; set; }
        public int _CLAM244330 { get; set; }
        public int _CLAM250360 { get; set; }
        public string _LARGO { get; set; }
        public string _M2_LAMINA { get; set; }
        public string _PORC_AMAR { get; set; }
        public string _PORC_AZUL { get; set; }
        public string _PORC_BLCO { get; set; }
        public string _PORC_ESME1 { get; set; }
        public string _PORC_ESME2 { get; set; }
        public string _PORC_NEGRO { get; set; }
        public string _PORC_ORO { get; set; }
        public string _PORC_ROJO { get; set; }
        public string _PORC_VERDE { get; set; }
        public string _RENDIM { get; set; }
        public string _VELPULST { get; set; }
        public string _PERFORA { get; set; }
        public string _DESTAJE { get; set; }
        public string _DLAM240321 { get; set; }
        public string _DLAM250360 { get; set; }
        public string _DLAM244330 { get; set; }
        public string _DLAM170220 { get; set; }
        public string _CLAM170220 { get; set; }
        public string _ESPESOR { get; set; }
        public string _TONHRS { get; set; }
        public string _CLAM190321 { get; set; }
        public string _V_BILAT_2 { get; set; }
        public string _M2AUN { get; set; }
        public string _PRIMITIVO1 { get; set; }
        public string _PRIMITIVO2 { get; set; }
        public string _PORC_WDKGY { get; set; }
        public string _PORC_OFWHT { get; set; }
        public string _PORC_CRANB { get; set; }
        public string _PORC_SNWHT { get; set; }
        public string _PORC_VDFGR { get; set; }
        public string _UN_JABA { get; set; }
        public string _PERF_1 { get; set; }
        public string _PERF_2 { get; set; }
        public string _PERF_3 { get; set; }
        public string _ESQUINA_1 { get; set; }
        public string _ESQUINA_2 { get; set; }
        public string _FORMA_BOTT { get; set; }
        public string _PRODUCTO { get; set; }
        public string _DIAM_1 { get; set; }
        public string _DIAM_2 { get; set; }
        public string _DIAM_3 { get; set; }
        public string _COLOR_1 { get; set; }
        public string _COLOR_2 { get; set; }
        public string _COLOR_3 { get; set; }
        public string _COLOR_4 { get; set; }
        public string _COLOR_5 { get; set; }
        public string _COLOR_6 { get; set; }
        public string _COLOR_7 { get; set; }
        public string _MESCLA_1 { get; set; }
        public string _MESCLA_2 { get; set; }
        public string _ESQUINA_3 { get; set; }
        public string _ESQUINA_4 { get; set; }
        public string _PASADAS { get; set; }
        public string _ESQUINA_5 { get; set; }
        public string _DESXPRO { get; set; }
        public string _PERF_4 { get; set; }
        public string _DIAM_4 { get; set; }
        public string _CLAM220330 { get; set; }
        public string _VALOR_US { get; set; }
        public string _MEZCLA_3 { get; set; }
        public string _MEZCLA_4 { get; set; }
        public string _MEZCLA_5 { get; set; }
        public string _MEZCLA_6 { get; set; }
        public string _PAPEL_1 { get; set; }
        public string _MUELA_1 { get; set; }
        public string _MUELA_2 { get; set; }
        public string _MUELA_3 { get; set; }
        public string _BROCA_1 { get; set; }
        public string _FRESA_1 { get; set; }
        public string _M2_C_SERG { get; set; }
        public string _PITILLA { get; set; }
        public string _MEZCLA_7 { get; set; }
        public string _MEZCLA_8 { get; set; }
        public string _BROCA_2 { get; set; }
        public string _CLAM152213 { get; set; }
        public string _CLAM160250 { get; set; }
        public string _CLAM183244 { get; set; }
        public string _COLOR_8 { get; set; }
        public string _COLOR_9 { get; set; }
        public string _COLOR_10 { get; set; }
        public string _COLOR_11 { get; set; }
        public string _COLOR_12 { get; set; }
        public string _CLAM360250 { get; set; }
        public string _AVELLANAD1 { get; set; }
        public string _AISLAPOL_1 { get; set; }
        public string _CINTA_EMBA { get; set; }
        public string _SELLO_MET { get; set; }
        public string _SEL_MET1 { get; set; }
        public string _ZUNCHO_MET { get; set; }
        public string _ZUNCHO_PLA { get; set; }
        public string _BOLSA_1 { get; set; }
        public string _CODCLIENTE { get; set; }
        public string _SEPARADOR { get; set; }
        public string _FILM_PLAS { get; set; }
        public string _TIPOTEMPLA { get; set; }
        public string _CLAM213330 { get; set; }
        public string _UNXH_CORT { get; set; }
        public string _UNXH_PULI { get; set; }
        public string _UNXH_PERF { get; set; }
        public string _UNXH_PINT { get; set; }
        public string _UNXH_TEMP { get; set; }
        public string _UNXH_PLAST { get; set; }
        public string _LOTE_ENTRE { get; set; }
        public string _LOTE_PINT { get; set; }
        public string _LOTE_TEMP { get; set; }
        public string _UNXH_VISOR { get; set; }
        public string _HH_MAN_INT { get; set; }
        public string _HH_MAN_EXT { get; set; }
        public string _C_MO_INT { get; set; }
        public string _C_MO_EXT { get; set; }
        public string _APROVECHAM { get; set; }
        public string _ESPESOR2 { get; set; }
        public string _LARGOG { get; set; }
        public string _TP_DVH_COR { get; set; }
        public string _ANCHOSEP { get; set; }
        public string _SIPOLISUL { get; set; }
        public string _SISILICON { get; set; }
        public string _SIARGON { get; set; }
        public string _SIVALVULA { get; set; }
        public string _BYTETOTAL { get; set; }
        public string _MERPROINS { get; set; }
        public string _MARGTPNEL { get; set; }
        public string _FACT_PORTE { get; set; }
        public string _M_ANCHO { get; set; }
        public string _CORREINDS { get; set; }
        public string _CORRE_ARQ { get; set; }
        public string _ANCHOSEP2 { get; set; }
        public string _TP_TVH_COR { get; set; }
        public string _PED_ALFAK { get; set; }
        public string _ITEM_ALFAK { get; set; }
        public string _PDTO_ALFAK { get; set; }
        public string _FPEDIDO_ALF { get; set; }
        public string _CORRE_MAQ { get; set; }
        public string _CORRE_REP { get; set; }
        public string _UNXMP01 { get; set; }
        public string _UNXMP02 { get; set; }
        public string _UNXMP03 { get; set; }
        public string _UNAISLA01 { get; set; }
        public string _UNAISLA02 { get; set; }
        public string _UNAISLA03 { get; set; }
        public string _UNMADERA01 { get; set; }
        public string _UNMADERA02 { get; set; }
        public string _UNMADERA03 { get; set; }
        public string _CORRE_SEMI { get; set; }
        public double _M2MP01 { get; set; }
        public double _M2MP02 { get; set; }
        public double _M2MP03 { get; set; }
        public string _M2CARTON { get; set; }
        public string _ESPMP01 { get; set; }
        public string _ESPMP02 { get; set; }
        public string _ESPMP03 { get; set; }
        public string _CORRE_OBRA { get; set; }
        public string _CORR_CTZA { get; set; }
        public string _POMO_CTZA { get; set; }
        public string _NPUE_CTZA { get; set; }
        public string _NPAN_CTZA { get; set; }
        public string _MLEN_CTZA { get; set; }
        public string _M2LA_CTZA { get; set; }
        public string _CORR_VPVC { get; set; }
        public string _DRUR_CTZA { get; set; }
        public string _FRUR_CTZA { get; set; }
        public string _UNAML { get; set; }





    }

    public class TablaPOTD
    {
        public string IDPOTD { get; set; }
        public string _IDPOTL { get; set; }
        public string _EMPRESA { get; set; }
        public string _NUMOT { get; set; }
        public string _NREG { get; set; }
        public string _SUBNREG { get; set; }
        public string _ESTADO { get; set; }
        public string _PERTENECE { get; set; }
        public string _NIVEL { get; set; }
        public string _AUXI { get; set; }
        public string _CODIGO { get; set; }
        public string _MARCANOM { get; set; }
        public string _CODNOMEN { get; set; }
        public string _NUMDEEST { get; set; }
        public string _GLOSA { get; set; }
        public string _TIPOUNI { get; set; }
        public string _DOBLEU { get; set; }
        public double _CANTIDADF { get; set; }
        public string _CANTIDACF { get; set; }
        public string _CANTIDADR { get; set; }
        public string _CANTANTI { get; set; }
        public string _UNIDAD { get; set; }
        public string _UNIDADC { get; set; }
        public string _TIPO { get; set; }
        public string _OPERACION { get; set; }
        public string _CALIDAD { get; set; }
        public string _NIVELSUP { get; set; }
        public string _NUMPLAN { get; set; }
        public string _LILG { get; set; }
        public string _NUMODC { get; set; }
        public string _NREGODC { get; set; }
        public string _SULIOTD { get; set; }
        public string _BOLIOTD { get; set; }
        public string _PNPDNREG { get; set; }
        public double _C_SALIDA { get; set; }
        public double _C_INSUMOS { get; set; }
        public string _P_INSUMOS { get; set; }
        public string _CCINSUMOS { get; set; }
        public string _PCINSUMOS { get; set; }

    }

    public class TablaPOTPR
    {
        public string _IDPOTL { get; set; }
        public string _EMPRESA { get; set; }
        public string _NUMOT { get; set; }
        public string _NREGOTL { get; set; }
        public string _CODIGO { get; set; }
        public string _OPERACION { get; set; }
        public string _ORDEN { get; set; }
        public string _POROPANT { get; set; }
        public string _PORREQCOMP { get; set; }
        public string _TIPOOP { get; set; }
        public string _ESTADO { get; set; }
        public string _SITPEDFAB { get; set; }
        public string _FABRICAR { get; set; }
        public string _REALIZADO { get; set; }
        public string _SALPROXJOR { get; set; }
        public string _PORMAQUILA { get; set; }
        public string _LILG { get; set; }
        public string _NUMODC { get; set; }
        public string _NREGODC { get; set; }
        public string _CODMAQOT { get; set; }
        public double _C_FABRIC { get; set; }
        public double _C_INSUMOS { get; set; }
        public double _C_MAQUINAS { get; set; }
        public double _C_M_OBRA { get; set; }
        public string _P_FABRIC { get; set; }
        public string _P_INSUMOS { get; set; }
        public string _P_MAQUINAS { get; set; }
        public string _P_M_OBRA { get; set; }
        public string _CCFABRIC { get; set; }
        public string _CCINSUMOS { get; set; }
        public string _CCMAQUINAS { get; set; }
        public string _CCM_OBRA { get; set; }
        public string _PCFABRIC { get; set; }
        public string _PCINSUMOS { get; set; }
        public string _PCMAQUINAS { get; set; }
        public string _PCM_OBRA { get; set; }
        public string _NOTAS { get; set; }
        public string _PERMAQALT { get; set; }
        public string _PERMAQPAR { get; set; }
    }

    public class TablaMAEDDO
    {
        public string _CAPRODEX { get; set; }
        public string _ESPRODLI { get; set; }
        public string _IDMAEDDO { get; set; }

    }

    public class TablaMAEEDO
    {
        public string _CAPRCO { get; set; }
        public string _CAPRAD { get; set; }
        public string _CAPREX { get; set; }
        public string _ESDO { get; set; }
        public string _ESFADO { get; set; }
        public string _ESPRODDO { get; set; }
        public string _NUDO { get; set; }

    }

    public class TablaPOTLCOM
    {
        public string _IDPOTL { get; set; }
        public string _ARCHIRST { get; set; }
        public string _IDRST { get; set; }
        public string _EMPRESA { get; set; }
        public string _NUMOT { get; set; }
        public string _NREGOTL { get; set; }
        public string _DESDE { get; set; }
        public string _NUMECOTI { get; set; }
        public string _ENDO { get; set; }
        public string _NREGCOT { get; set; }
        public string _CODIGO { get; set; }
        public string _UDAD { get; set; }
        public string _FABRICAR { get; set; }
        public string _REALIZADO { get; set; }
        public string _DESISTIDO { get; set; }
        public string _ASIGNADO { get; set; }
        public string _ESODD { get; set; }
        public string _ASIGNADO2 { get; set; }
    }

    public class ListaTIPOOT
    {
        public string _TIPOOT { get; set; }
        public string _NOTIPO { get; set; }
        public string _COMBINADO { get; set; }
    }

}
