using GlobalInfo;
using nsCliente;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;


/// <summary>
/// Descripción breve de Formulario
/// </summary>
/// 
namespace Comercial
{
    namespace Cotizador
    {

        public class Cotizacion
        {
            #region Variables publicas
            public string ID { get; set; }
            public string IDCORR { get; set; }
            public string UserId { get; set; }
            public string RUTEMP { get; set; }
            public DateTime FEEMDO { get; set; }
            public string TOKENID { get; set; }
            public string CLIENTE { get; set; }
            public string OBSERVA { get; set; }
            public double COSTMO { get; set; }
            public double COSTDIR { get; set; }
            public double OTROSCOST { get; set; }
            public double GASTOSGEN { get; set; }
            public double MARGEN { get; set; }
            public double BENECONOM { get; set; }
            public double DSCTOPORC { get; set; }
            public double DSCTO { get; set; }
            public double NETO { get; set; }
            public double IVA { get; set; }
            public double BRUTO { get; set; }
            public bool ISORDERED { get; set; }
            public bool ENVIADA { get; set; }
            public bool ACEPTADA { get; set; }
            public DateTime VALIDEZ { get; set; }
            public bool ESTADO { get; set; }
            #endregion


            
            public Cotizacion()
            {

            }

            public class UpdateRow
            {
                Coneccion Conn;
                public readonly bool Actualizado;

                public UpdateRow(string _RowID, string _Campo, object _Valor)
                {
                    Actualizado = Update(_RowID, _Campo, _Valor);
                }

                private bool Update(string RowID, string Campo, object Valor)
                {
                    bool IsUpdated = false;
                    #region Query
                    string Query = "UPDATE COT_ENDO SET " + Campo + "=@Valor WHERE ID=@ID ";
                    #endregion
                    Conn = new Coneccion();
                    try
                    {
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                        Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                        Conn.Cmd.ExecuteNonQuery();
                        Conn.ConnPlabal.Close();

                        IsUpdated = true;

                    }
                    catch (Exception ex)
                    {

                        IsUpdated = false;
                    }

                    return IsUpdated;
                }

            }

            public class GetAll
            {
                public readonly List<GetRowInfo> Lista;

                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                #endregion


                private readonly bool estado;
                private readonly string Rut;


                public GetAll()
                {

                }

                public GetAll(bool _Estado)
                {
                    estado = _Estado;
                    Rut = null;
                    Lista = GetRows();

                }

                public GetAll(bool _Estado, string _Rut)
                {
                    estado = _Estado;
                    Rut = _Rut;
                    Lista = GetRows();

                }



                private List<GetRowInfo> GetRows()
                {
                    List<GetRowInfo> rowInfos = new List<GetRowInfo>();

                    Conn = new Coneccion();

                    if (!string.IsNullOrEmpty(Rut))
                    {
                        Query = "SELECT * FROM COT_ENDO WHERE ESTADO=@ESTADO AND RUTEMP=@RUT";
                    }
                    else
                    {
                        Query = "SELECT * FROM COT_ENDO WHERE ESTADO=@ESTADO";
                    }
                    
                    
                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            if (!string.IsNullOrEmpty(Rut))
                            {
                                adap.SelectCommand.Parameters.AddWithValue("@RUT", Rut);
                            }
                            
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", estado);
                            
                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in Dtable.Rows)
                            {
                                Validadores Val = new Validadores();
                                GetRowInfo rowInfo = new GetRowInfo {
                                    
                                ID = dr["ID"].ToString(),
                                Cliente = new DatosCliente(dr["RUTEMP"].ToString()),
                                UserId = dr["UserId"].ToString(),
                                FEEMDO = Val.ParseoDateTime(dr["FEEMDO"].ToString()),
                                TOKENID = dr["TOKENID"].ToString(),
                                CLIENTEDOC = dr["CLIENTE"].ToString(),
                                OBSERVA = dr["OBSERVA"].ToString(),
                                NETO = Val.ParseoDouble(dr["NETO"].ToString()),
                                IVA = Val.ParseoDouble(dr["IVA"].ToString()),
                                BRUTO = Val.ParseoDouble(dr["BRUTO"].ToString()),
                                ISORDERED = Val.ParseoBoolean(dr["ISORDERED"].ToString()),
                                ESTADO = Val.ParseoBoolean(dr["ESTADO"].ToString()),
                                Correlativo = Convert.ToInt32(dr["IDCORR"]),
                                ENVIADA = Val.ParseoBoolean(dr["ENVIADA"].ToString()),
                                ACEPTADA = Val.ParseoBoolean(dr["ACEPTADA"].ToString()),
                                VALIDEZ = Val.ParseoDateTime(dr["VALIDEZ"].ToString()),
                                COSTMO = Val.ParseoDouble(dr["COSTMO"].ToString()),
                                COSTDIR = Val.ParseoDouble(dr["COSTDIR"].ToString()),
                                OTROSCOST = Val.ParseoDouble(dr["OTROSCOST"].ToString()),
                                GASTOSGEN = Val.ParseoDouble(dr["GASTOSGEN"].ToString()),
                                MARGEN = Val.ParseoDouble(dr["MARGEN"].ToString()),
                                BENECONOM = Val.ParseoDouble(dr["BENECONOM"].ToString()),
                                DSCTOPORC = Val.ParseoDouble(dr["DSCTOPORC"].ToString()),
                                DSCTO = Val.ParseoDouble(dr["DSCTO"].ToString()),
                                


                            };
                                rowInfos.Add(rowInfo);
                            }

                            return rowInfos;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }

            public class GetRowInfo
            {
                public string ID { get; set; }
                public string UserId { get; set; }
                public DateTime FEEMDO { get; set; }
                public string TOKENID { get; set; }
                public string CLIENTEDOC { get; set; }
                public string OBSERVA { get; set; }
                public double NETO { get; set; }
                public double IVA { get; set; }
                public double BRUTO { get; set; }
                public double COSTMO { get; set; }
                public double COSTDIR { get; set; }
                public double OTROSCOST { get; set; }
                public double GASTOSGEN { get; set; }
                public double MARGEN { get; set; }
                public double BENECONOM { get; set; }
                public double DSCTOPORC { get; set; }
                public double DSCTO { get; set; }
                public bool ISORDERED { get; set; }
                public bool ENVIADA { get; set; }
                public bool ACEPTADA { get; set; }
                public DateTime VALIDEZ { get; set; }
                public bool ESTADO { get; set; }
                public int Correlativo { get; set; }
                public DatosCliente Cliente { get; set; }

                public readonly bool HasRow;

                Coneccion Conn;
                private SqlDataReader dr;
                private string Query;

                public GetRowInfo()
                {

                }

                public GetRowInfo(string ID, string TOKEN, bool ESTADO)
                {
                    Encriptacion Enc = new Encriptacion(TOKEN);
                    if (!string.IsNullOrEmpty(Enc.TokenDesencriptado))
                    {
                        HasRow = GetInfoFromCOT_ENDO(ID, Enc.TokenDesencriptado, ESTADO);
                    }
                    else
                    {
                        HasRow = false;
                    }
                    
                }

                private bool GetInfoFromCOT_ENDO(string _ID, string _tokenid, bool _ESTADO)
                {
                    
                    Query = "SELECT * FROM COT_ENDO WHERE ID=@id AND ESTADO=@estado AND " +
                        "TOKENID=@tokenid ";

                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@id", _ID);
                    Conn.Cmd.Parameters.AddWithValue("@tokenid", _tokenid);
                    Conn.Cmd.Parameters.AddWithValue("@estado", _ESTADO);
                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores Val = new Validadores();
                        ID = dr["ID"].ToString();
                        Cliente = new DatosCliente(dr["RUTEMP"].ToString());
                        UserId = dr["UserId"].ToString();
                        FEEMDO = Val.ParseoDateTime(dr["FEEMDO"].ToString());
                        TOKENID = dr["TOKENID"].ToString();
                        CLIENTEDOC = dr["CLIENTE"].ToString();
                        OBSERVA = dr["OBSERVA"].ToString();
                        NETO = Val.ParseoDouble(dr["NETO"].ToString());
                        IVA = Val.ParseoDouble(dr["IVA"].ToString());
                        BRUTO = Val.ParseoDouble(dr["BRUTO"].ToString());
                        ISORDERED = Val.ParseoBoolean(dr["ISORDERED"].ToString());
                        ESTADO = Val.ParseoBoolean(dr["ESTADO"].ToString());
                        Correlativo = Convert.ToInt32(dr["IDCORR"]);
                        ENVIADA = Val.ParseoBoolean(dr["ENVIADA"].ToString());
                        ACEPTADA = Val.ParseoBoolean(dr["ACEPTADA"].ToString());
                        VALIDEZ = Val.ParseoDateTime(dr["VALIDEZ"].ToString());
                        COSTMO = Val.ParseoDouble(dr["COSTMO"].ToString());
                        COSTDIR = Val.ParseoDouble(dr["COSTDIR"].ToString());
                        OTROSCOST= Val.ParseoDouble(dr["OTROSCOST"].ToString());
                        GASTOSGEN= Val.ParseoDouble(dr["GASTOSGEN"].ToString());
                        MARGEN= Val.ParseoDouble(dr["MARGEN"].ToString());
                        BENECONOM= Val.ParseoDouble(dr["BENECONOM"].ToString());
                        DSCTOPORC= Val.ParseoDouble(dr["DSCTOPORC"].ToString());
                        DSCTO= Val.ParseoDouble(dr["DSCTO"].ToString());

                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return true;
                    }
                    else
                    {
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return false;
                    }

                }


            }

            public class FirstEntry
            {
                public readonly string ID;
                public readonly string Token;

                public readonly bool IsSuccess;
                Coneccion Conn;
                private  SqlDataReader dr;
                private readonly string _Rut;
                private  string Query;

                public FirstEntry(string RutEmpresa, string Username)
                {
                    _Rut = RutEmpresa;
                    MembershipUser Miembro = Membership.GetUser(Username);
                    var UserId = (Guid)Miembro.ProviderUserKey;
                    string[] retorno = Generar(UserId.ToString());
                    if (!string.IsNullOrEmpty(retorno[0]) || !string.IsNullOrEmpty(retorno[1]))
                    {
                        ID = retorno[0];
                        Token = retorno[1];
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                    
                }

                private string[] Generar(string _UserId)
                {
                    Random random = new Random();
                    int TokenId = random.Next(10000, 9999999);
                    Encriptacion encriptacion = new Encriptacion(TokenId.ToString());
                    string[] RetStr = new string[2];
                    RetStr[1] = encriptacion.TokenEncriptado;

                    if (InsertIntoCOT_ENDO(_UserId,TokenId.ToString()))
                    {
                        RetStr[0] = GetIdFromCOT_ENDO(_UserId, TokenId.ToString());
                    }


                    return RetStr;
                }

                private string GetIdFromCOT_ENDO(string _userid, string token)
                {
                    bool Estado = true;
                    Query = "SELECT ID FROM COT_ENDO WHERE UserId=@userid AND RUTEMP=@rut AND ESTADO=@estado AND " +
                        "TOKENID=@tokenid ";

                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@userid", _userid);
                    Conn.Cmd.Parameters.AddWithValue("@rut", _Rut);
                    Conn.Cmd.Parameters.AddWithValue("@tokenid", token);
                    Conn.Cmd.Parameters.AddWithValue("@estado", Estado);
                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        string valor = dr[0].ToString();
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return valor;
                    }
                    else
                    {
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return null;
                    }

                }

                private bool InsertIntoCOT_ENDO(string _UserId, string _TokenId )
                {
                    bool IsInserted = false;
                    bool IsOrdered = false;
                    bool Estado = true;
                    int Corr = GetCorrelativo(_Rut);
                    DateTime validez = DateTime.Today.AddDays(5);
                    

                    Conn = new Coneccion();
                    Query = "INSERT INTO COT_ENDO (IDCORR,UserId,RUTEMP,FEEMDO,TOKENID,COSTMO,COSTDIR,OTROSCOST,GASTOSGEN,MARGEN," +
                        "BENECONOM,DSCTOPORC,DSCTO,NETO,IVA,BRUTO,ISORDERED,ENVIADA,ACEPTADA,VALIDEZ,ESTADO)" +
                        " VALUES (@idcorr,@userid,@rut,GETDATE(),@tokenid,0,0,0,0,0,0,0,0,0,0,0,@isordered,@isordered,@isordered,@VALIDEZ,@estado)";
                    try
                    {
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@userid", _UserId);
                        Conn.Cmd.Parameters.AddWithValue("@rut", _Rut);
                        Conn.Cmd.Parameters.AddWithValue("@tokenid", _TokenId);
                        Conn.Cmd.Parameters.AddWithValue("@isordered", IsOrdered);
                        Conn.Cmd.Parameters.AddWithValue("@estado", Estado);
                        Conn.Cmd.Parameters.AddWithValue("@idcorr", Corr);
                        Conn.Cmd.Parameters.AddWithValue("@VALIDEZ", validez);
                        Conn.Cmd.ExecuteNonQuery();
                        Conn.ConnPlabal.Close();
                        IsInserted = true;
                       
                    }
                    catch
                    {
                        IsInserted = false;
                    }

                    return IsInserted;
                }

                private int GetCorrelativo(string RutEmp)
                {
                    bool Estado = true;
                    Query = "SELECT TOP 1 IDCORR FROM COT_ENDO WHERE RUTEMP=@rut AND ESTADO=@estado ORDER BY IDCORR DESC ";

                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@rut", _Rut);
                    Conn.Cmd.Parameters.AddWithValue("@estado", Estado);
                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores Val = new Validadores();
                        double tr = Val.ParseoDouble(dr[0].ToString());
                        int valor = 1 + Convert.ToInt32(tr);
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return valor;
                    }
                    else
                    {
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return 1;
                    }
                }

            }


            public class MontosCotizacion
            {
                /*Clase para realizar los calculos entre tablas*/

                private readonly string _ID;

                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SqlDataReader dr;
                #endregion

                public MontosCotizacion()
                {

                }

                public MontosCotizacion(string IDCOTIZACION)
                {
                    _ID = IDCOTIZACION;

                    UpdateMontosCotizacion(_ID);

                }

                public void UpdateMontosCotizacion(string IDENDO)
                {
                    UpdatePLABALRow update;
                    DetalleCotizacion.Get get = new DetalleCotizacion.Get(IDENDO, true);
                    UpdateMontoItems(get.Detalle);

                    double[] COSTDIR = GetSUMCOSTITEMS(IDENDO);
                    /*COSTOS DIRECTOS*/
                    update = new UpdatePLABALRow("COT_ENDO","ID",IDENDO,"COSTDIR",COSTDIR[1]);

                    /*COSTOS MANO DE OBRA*/
                    update = new UpdatePLABALRow("COT_ENDO", "ID", IDENDO, "COSTMO", COSTDIR[0]);

                    
                    

                    update = new UpdatePLABALRow("COT_ENDO", "ID", IDENDO, "BENECONOM", COSTDIR[2]);

                    
                    double[] MontosCot = GetValores(IDENDO);
                    double DsctoPorc = MontosCot[6];
                    double CostosTotales = MontosCot[0] + MontosCot[1] + MontosCot[2] + MontosCot[3] + MontosCot[5];

                    GetDescuento descuento = new GetDescuento(DsctoPorc, CostosTotales);
                    update = new UpdatePLABALRow("COT_ENDO", "ID", IDENDO, "DSCTO", descuento.Descuento);
                    
                    double NetoTotal = descuento.Resultado;
                    update = new UpdatePLABALRow("COT_ENDO", "ID", IDENDO, "NETO", NetoTotal);
                    double IvaTotal = Math.Round(NetoTotal*0.19, 0);
                    update = new UpdatePLABALRow("COT_ENDO", "ID", IDENDO, "IVA", IvaTotal);
                    double brutoTotal = NetoTotal + IvaTotal;
                    update = new UpdatePLABALRow("COT_ENDO", "ID", IDENDO, "BRUTO", brutoTotal);


                }

                public double[] GetSUMCOSTITEMS(string IDENDO)
                {
                    bool eSTADO = true;
                    Query = "SELECT SUM(COSTMOUN*CANTIDAD), SUM(COSTDIRUN*CANTIDAD), SUM(BENECONOMUN*CANTIDAD) FROM COT_ENDODET WHERE IDENDO=@ID AND ESTADO=@ESTADO";

                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@ID", IDENDO);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", eSTADO);

                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores Val = new Validadores();
                        double[] VALORES = new double[3];
                        VALORES[0] = Val.ParseoDouble(dr[0].ToString());
                        VALORES[1] = Val.ParseoDouble(dr[1].ToString());
                        VALORES[2] = Val.ParseoDouble(dr[2].ToString());
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return VALORES;
                        
                        

                    }
                    else
                    {
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return null;

                    }
                    


                }
                
                

                public void UpdateMontoItems(List<DetalleCotizacion> Items)
                {
                    UpdatePLABALRow update;
                    double COSTDIR = 0;
                    foreach (DetalleCotizacion item in Items)
                    {
                        
                        double COSTDIRUN = NetoUnItem(item);
                        item.COSTDIRUN = COSTDIRUN;
                        DetalleCotizacion.ActualizarMontositem Upd = new DetalleCotizacion.ActualizarMontositem(item);

                        COSTDIR = COSTDIR + Upd.NetoTotalItem;

                    }

                    var UnItem = Items.First();
                    update = new UpdatePLABALRow("COT_ENDO", "ID", UnItem.IDENDO, "COSTDIR", COSTDIR);



                }

                public double NetoUnItem(DetalleCotizacion Item)
                {
                    SubDetalleCotizacion.Get get = new SubDetalleCotizacion.Get(1, Item.IDENDO, Item.IDENDODET, Item.ESTADO);
                    double _NetoUnItem = 0;
                    foreach (SubDetalleCotizacion item in get.SubDetalle)
                    {
                        _NetoUnItem = _NetoUnItem + Math.Round(NetoUnPieza(item),0);
                    }

                    UpdatePLABALRow update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", Item.IDENDODET, "COSTDIRUN", _NetoUnItem);


                    return _NetoUnItem;

                }

                public double NetoUnPieza(SubDetalleCotizacion Pieza)
                {
                    double Neto = GetNetoxLevel(Pieza.IDENDO, Pieza.IDENDODET, Pieza.NODE,"2");
                    UpdatePLABALRow update = new UpdatePLABALRow("COT_ENDOSUBDET","IDSUBDET",Pieza.IDSUBDET,"NETO",Neto);
                    return Neto;
                }

                
                public double[] GetValores(string IDENDO)
                {

                    double[] Valores = new double[11];
                    Query = "SELECT COSTMO,COSTDIR,OTROSCOST,GASTOSGEN,MARGEN,BENECONOM,DSCTOPORC,DSCTO,NETO,IVA,BRUTO FROM COT_ENDO WHERE ID=@ID";

                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@ID", IDENDO);
                    
                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores Val = new Validadores();
                        for (int i = 0; i < 11; i++)
                        {
                            Valores[i] = Val.ParseoDouble(dr[i].ToString());
                        }
                        
                        

                      
                    }
                    else
                    {
                        Valores = null;
                        
                    }
                    Conn.ConnPlabal.Close();
                    dr.Close();
                    return Valores;

                }

                public double GetNetoxLevel(string IDENDO, string IDENDODET, string NODE, string Level)
                {
                    bool ESTADO = true;
                    Query = "SELECT SUM(NETO) FROM COT_ENDOSUBDET WHERE LEVEL=@LEVEL AND NODE=@NODE AND IDENDODET=@IDENDODET" +
                        " AND IDENDO=@IDENDO AND ESTADO=@ESTADO";

                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@NODE", NODE);
                    Conn.Cmd.Parameters.AddWithValue("@IDENDODET", IDENDODET);
                    Conn.Cmd.Parameters.AddWithValue("@IDENDO", IDENDO);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                    Conn.Cmd.Parameters.AddWithValue("@LEVEL", Level);
                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores Val = new Validadores();

                        double Valor = Math.Round(Val.ParseoDouble(dr[0].ToString()), 0);
                        Conn.ConnPlabal.Close();
                        dr.Close();

                        return Valor;
                    }
                    else
                    {
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return 0;
                    }
                }

                
            }


        }

        public class SubDetalleCotizacion
        {
            #region Variables Publicas
            public string IDSUBDET { get; set; }
            public string IDENDODET { get; set; }
            public string IDENDO { get; set; }
            public string NOMBRE { get; set; }
            public string LEVEL { get; set; }
            public string POS_NR { get; set; }
            public string NODE { get; set; }
            public bool ESMODELO { get; set; }
            public string IDMODELBOM { get; set; }
            public double ANCHO { get; set; }
            public double ALTO { get; set; }
            public double NETO { get; set; }
            public double IVA { get; set; }
            public double BRUTO { get; set; }
            public DateTime F_ASIGNACION { get; set; }
            public bool ESTADO { get; set; }
            #endregion


            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion


            public SubDetalleCotizacion()
            {

            }

            public class FirstEntry
            {
                public readonly bool IsInserted;
                private readonly SubDetalleCotizacion Sub;

                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SqlDataReader dr;
                #endregion

                public FirstEntry(SubDetalleCotizacion subDetalle)
                {
                    Sub = subDetalle;
                    IsInserted = Insert();
                    
                }

                private bool Insert()
                {
                    Conn = new Coneccion();
                    Query = "INSERT INTO COT_ENDOSUBDET (IDENDODET,IDENDO,LEVEL,POS_NR,NODE,ESMODELO,NOMBRE,IDMODELBOM,ANCHO,ALTO,NETO,IVA,BRUTO,F_ASIGNACION,ESTADO)" +
                        " VALUES (@IDENDODET,@IDENDO,@LEVEL,@POS_NR,@NODE,@ESMODELO,@NOMBRE,@IDMODELBOM,@ANCHO,@ALTO,@NETO,@IVA,@BRUTO,@F_ASIGNACION,@ESTADO)";
                    try
                    {
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@IDENDODET", Sub.IDENDODET);
                        Conn.Cmd.Parameters.AddWithValue("@IDENDO", Sub.IDENDO);
                        Conn.Cmd.Parameters.AddWithValue("@LEVEL", Sub.LEVEL);
                        Conn.Cmd.Parameters.AddWithValue("@POS_NR", Sub.POS_NR);
                        Conn.Cmd.Parameters.AddWithValue("@NODE", Sub.NODE);
                        Conn.Cmd.Parameters.AddWithValue("@NOMBRE", Sub.NOMBRE);
                        Conn.Cmd.Parameters.AddWithValue("@ESMODELO", Sub.ESMODELO);
                        Conn.Cmd.Parameters.AddWithValue("@IDMODELBOM", Sub.IDMODELBOM);
                        Conn.Cmd.Parameters.AddWithValue("@ANCHO", Sub.ANCHO);
                        Conn.Cmd.Parameters.AddWithValue("@ALTO", Sub.ALTO);
                        Conn.Cmd.Parameters.AddWithValue("@NETO", Sub.NETO);
                        Conn.Cmd.Parameters.AddWithValue("@IVA", Sub.IVA);
                        Conn.Cmd.Parameters.AddWithValue("@BRUTO", Sub.BRUTO);
                        Conn.Cmd.Parameters.AddWithValue("@F_ASIGNACION", Sub.F_ASIGNACION);
                        Conn.Cmd.Parameters.AddWithValue("@ESTADO", Sub.ESTADO);
                        
                        Conn.Cmd.ExecuteNonQuery();
                        Conn.ConnPlabal.Close();
                        return true;

                    }
                    catch
                    {
                        Conn.ConnPlabal.Close();
                        
                        return false;
                    }

                }

            }

            public class Get
            {
                public readonly List<SubDetalleCotizacion> SubDetalle;
                public readonly bool HasDetail;

                private readonly string _IDENDO;
                private readonly string _IDENDODET;
                private readonly bool _ESTADO;
                private readonly int _Level;
                private readonly string _NODE;
                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SqlDataReader dr;
                private List<SubDetalleCotizacion> _Sub;
                #endregion


                public Get(string IDENDO, bool ESTADO)
                {
                    _IDENDO = IDENDO;
                    _ESTADO = ESTADO;
                    HasDetail = ObtainA();
                    if (HasDetail)
                    {
                        SubDetalle = new List<SubDetalleCotizacion>();
                        SubDetalle = _Sub;
                    }
                    
                }

                public Get(string IDENDO, string IDENDODET, bool ESTADO)
                {
                    _IDENDO = IDENDO;
                    _ESTADO = ESTADO;
                    _IDENDODET = IDENDODET;
                    HasDetail = ObtainB();
                    if (HasDetail)
                    {
                        SubDetalle = new List<SubDetalleCotizacion>();
                        SubDetalle = _Sub;
                    }

                }

                public Get(int Level,string IDENDO, string IDENDODET, bool ESTADO)
                {
                    _IDENDO = IDENDO;
                    _ESTADO = ESTADO;
                    _IDENDODET = IDENDODET;
                    _Level = Level;
                    HasDetail = ObtainC();
                    if (HasDetail)
                    {
                        SubDetalle = new List<SubDetalleCotizacion>();
                        SubDetalle = _Sub;
                    }

                }

                public Get(int Level, string IDENDO, bool ESTADO)
                {
                    _Level = Level;
                    _ESTADO = ESTADO;
                    _IDENDO = IDENDO;
                    HasDetail = ObtainE();
                    if (HasDetail)
                    {
                        SubDetalle = new List<SubDetalleCotizacion>();
                        SubDetalle = _Sub;
                    }
                }


                public Get(string NODE, string IDENDO, string IDENDODET, bool ESTADO)
                {
                    _IDENDO = IDENDO;
                    _ESTADO = ESTADO;
                    _IDENDODET = IDENDODET;
                    _NODE = NODE;
                    HasDetail = ObtainD();
                    if (HasDetail)
                    {
                        SubDetalle = new List<SubDetalleCotizacion>();
                        SubDetalle = _Sub;
                    }

                }

                public Get(string NODE,int Level, string IDENDO, string IDENDODET, bool ESTADO)
                {
                    _IDENDO = IDENDO;
                    _ESTADO = ESTADO;
                    _IDENDODET = IDENDODET;
                    _NODE = NODE;
                    _Level = Level;
                    HasDetail = ObtainF();
                    if (HasDetail)
                    {
                        SubDetalle = new List<SubDetalleCotizacion>();
                        SubDetalle = _Sub;
                    }

                }

                private bool ObtainA()
                {
                    Conn = new Coneccion();
                    _Sub = new List<SubDetalleCotizacion>();
                    Query = "SELECT * FROM COT_ENDOSUBDET WHERE IDENDO=@IDENDO AND ESTADO=@ESTADO";

                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                SubDetalleCotizacion sub = new SubDetalleCotizacion {
                                    IDSUBDET = row["IDSUBDET"].ToString(),
                                    IDENDODET = row["IDENDODET"].ToString(),
                                    IDENDO= row["IDENDO"].ToString(),
                                    LEVEL = row["LEVEL"].ToString(),
                                    POS_NR= row["POS_NR"].ToString(),
                                    NODE= row["NODE"].ToString(),
                                    ESMODELO= vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                    IDMODELBOM = row["IDMODELBOM"].ToString(),
                                    ANCHO= vAL.ParseoDouble(row["ANCHO"].ToString()),
                                    ALTO=vAL.ParseoDouble(row["ALTO"].ToString()),
                                    NETO=vAL.ParseoDouble(row["NETO"].ToString()),
                                    IVA=vAL.ParseoDouble(row["IVA"].ToString()),
                                    BRUTO=vAL.ParseoDouble(row["BRUTO"].ToString()),
                                    ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                    NOMBRE = row["NOMBRE"].ToString(),
                                    F_ASIGNACION = vAL.ParseoDateTime(row["F_ASIGNACION"].ToString()),
                                    

                                };
                                _Sub.Add(sub);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }

                private bool ObtainB()
                {
                    Conn = new Coneccion();

                    Query = "SELECT * FROM COT_ENDOSUBDET WHERE IDENDO=@IDENDO AND IDENDODET=@IDENDODET AND ESTADO=@ESTADO";
                    _Sub = new List<SubDetalleCotizacion>();
                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);
                            adap.SelectCommand.Parameters.AddWithValue("@IDENDODET", _IDENDODET);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                SubDetalleCotizacion sub = new SubDetalleCotizacion
                                {
                                    IDSUBDET = row["IDSUBDET"].ToString(),
                                    IDENDODET = row["IDENDODET"].ToString(),
                                    IDENDO = row["IDENDO"].ToString(),
                                    LEVEL = row["LEVEL"].ToString(),
                                    POS_NR = row["POS_NR"].ToString(),
                                    NODE = row["NODE"].ToString(),
                                    ESMODELO = vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                    IDMODELBOM = row["IDMODELBOM"].ToString(),
                                    ANCHO = vAL.ParseoDouble(row["ANCHO"].ToString()),
                                    ALTO = vAL.ParseoDouble(row["ALTO"].ToString()),
                                    NETO = vAL.ParseoDouble(row["NETO"].ToString()),
                                    IVA = vAL.ParseoDouble(row["IVA"].ToString()),
                                    BRUTO = vAL.ParseoDouble(row["BRUTO"].ToString()),
                                    ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                    NOMBRE = row["NOMBRE"].ToString(),
                                    F_ASIGNACION = vAL.ParseoDateTime(row["F_ASIGNACION"].ToString()),


                                };
                                _Sub.Add(sub);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }
                private bool ObtainC()
                {
                    Conn = new Coneccion();

                    Query = "SELECT * FROM COT_ENDOSUBDET WHERE IDENDO=@IDENDO AND IDENDODET=@IDENDODET AND ESTADO=@ESTADO AND LEVEL=@LEVEL";
                    _Sub = new List<SubDetalleCotizacion>();
                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);
                            adap.SelectCommand.Parameters.AddWithValue("@IDENDODET", _IDENDODET);
                            adap.SelectCommand.Parameters.AddWithValue("@LEVEL", _Level);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                SubDetalleCotizacion sub = new SubDetalleCotizacion
                                {
                                    IDSUBDET = row["IDSUBDET"].ToString(),
                                    IDENDODET = row["IDENDODET"].ToString(),
                                    IDENDO = row["IDENDO"].ToString(),
                                    LEVEL = row["LEVEL"].ToString(),
                                    POS_NR = row["POS_NR"].ToString(),
                                    NODE = row["NODE"].ToString(),
                                    ESMODELO = vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                    IDMODELBOM = row["IDMODELBOM"].ToString(),
                                    ANCHO = vAL.ParseoDouble(row["ANCHO"].ToString()),
                                    ALTO = vAL.ParseoDouble(row["ALTO"].ToString()),
                                    NETO = vAL.ParseoDouble(row["NETO"].ToString()),
                                    IVA = vAL.ParseoDouble(row["IVA"].ToString()),
                                    BRUTO = vAL.ParseoDouble(row["BRUTO"].ToString()),
                                    ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                    NOMBRE = row["NOMBRE"].ToString(),
                                    F_ASIGNACION = vAL.ParseoDateTime(row["F_ASIGNACION"].ToString()),


                                };
                                _Sub.Add(sub);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }

                private bool ObtainD()
                {
                    Conn = new Coneccion();

                    Query = "SELECT * FROM COT_ENDOSUBDET WHERE IDENDO=@IDENDO AND IDENDODET=@IDENDODET AND ESTADO=@ESTADO AND NODE=@NODE";
                    _Sub = new List<SubDetalleCotizacion>();
                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);
                            adap.SelectCommand.Parameters.AddWithValue("@IDENDODET", _IDENDODET);
                            adap.SelectCommand.Parameters.AddWithValue("@NODE", _NODE);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                SubDetalleCotizacion sub = new SubDetalleCotizacion
                                {
                                    IDSUBDET = row["IDSUBDET"].ToString(),
                                    IDENDODET = row["IDENDODET"].ToString(),
                                    IDENDO = row["IDENDO"].ToString(),
                                    LEVEL = row["LEVEL"].ToString(),
                                    POS_NR = row["POS_NR"].ToString(),
                                    NODE = row["NODE"].ToString(),
                                    ESMODELO = vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                    IDMODELBOM = row["IDMODELBOM"].ToString(),
                                    ANCHO = vAL.ParseoDouble(row["ANCHO"].ToString()),
                                    ALTO = vAL.ParseoDouble(row["ALTO"].ToString()),
                                    NETO = vAL.ParseoDouble(row["NETO"].ToString()),
                                    IVA = vAL.ParseoDouble(row["IVA"].ToString()),
                                    BRUTO = vAL.ParseoDouble(row["BRUTO"].ToString()),
                                    ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                    NOMBRE = row["NOMBRE"].ToString(),
                                    F_ASIGNACION = vAL.ParseoDateTime(row["F_ASIGNACION"].ToString()),


                                };
                                _Sub.Add(sub);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }

                private bool ObtainE()
                {
                    Conn = new Coneccion();

                    Query = "SELECT * FROM COT_ENDOSUBDET WHERE IDENDO=@IDENDO  AND ESTADO=@ESTADO AND LEVEL=@LEVEL";
                    _Sub = new List<SubDetalleCotizacion>();
                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);
                            adap.SelectCommand.Parameters.AddWithValue("@LEVEL", _Level);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                SubDetalleCotizacion sub = new SubDetalleCotizacion
                                {
                                    IDSUBDET = row["IDSUBDET"].ToString(),
                                    IDENDODET = row["IDENDODET"].ToString(),
                                    IDENDO = row["IDENDO"].ToString(),
                                    LEVEL = row["LEVEL"].ToString(),
                                    POS_NR = row["POS_NR"].ToString(),
                                    NODE = row["NODE"].ToString(),
                                    ESMODELO = vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                    IDMODELBOM = row["IDMODELBOM"].ToString(),
                                    ANCHO = vAL.ParseoDouble(row["ANCHO"].ToString()),
                                    ALTO = vAL.ParseoDouble(row["ALTO"].ToString()),
                                    NETO = vAL.ParseoDouble(row["NETO"].ToString()),
                                    IVA = vAL.ParseoDouble(row["IVA"].ToString()),
                                    BRUTO = vAL.ParseoDouble(row["BRUTO"].ToString()),
                                    ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                    NOMBRE = row["NOMBRE"].ToString(),
                                    F_ASIGNACION = vAL.ParseoDateTime(row["F_ASIGNACION"].ToString()),


                                };
                                _Sub.Add(sub);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }

                private bool ObtainF()
                {
                    Conn = new Coneccion();

                    Query = "SELECT * FROM COT_ENDOSUBDET WHERE IDENDO=@IDENDO AND IDENDODET=@IDENDODET AND ESTADO=@ESTADO AND NODE=@NODE AND LEVEL=@LEVEL";
                    _Sub = new List<SubDetalleCotizacion>();
                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);
                            adap.SelectCommand.Parameters.AddWithValue("@IDENDODET", _IDENDODET);
                            adap.SelectCommand.Parameters.AddWithValue("@NODE", _NODE);
                            adap.SelectCommand.Parameters.AddWithValue("@LEVEL", _Level);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                SubDetalleCotizacion sub = new SubDetalleCotizacion
                                {
                                    IDSUBDET = row["IDSUBDET"].ToString(),
                                    IDENDODET = row["IDENDODET"].ToString(),
                                    IDENDO = row["IDENDO"].ToString(),
                                    LEVEL = row["LEVEL"].ToString(),
                                    POS_NR = row["POS_NR"].ToString(),
                                    NODE = row["NODE"].ToString(),
                                    ESMODELO = vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                    IDMODELBOM = row["IDMODELBOM"].ToString(),
                                    ANCHO = vAL.ParseoDouble(row["ANCHO"].ToString()),
                                    ALTO = vAL.ParseoDouble(row["ALTO"].ToString()),
                                    NETO = vAL.ParseoDouble(row["NETO"].ToString()),
                                    IVA = vAL.ParseoDouble(row["IVA"].ToString()),
                                    BRUTO = vAL.ParseoDouble(row["BRUTO"].ToString()),
                                    ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                    NOMBRE = row["NOMBRE"].ToString(),
                                    F_ASIGNACION = vAL.ParseoDateTime(row["F_ASIGNACION"].ToString()),


                                };
                                _Sub.Add(sub);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }
            }

            public class GetItem
            {

                public readonly SubDetalleCotizacion Datos;
                public readonly bool IsGetting;
                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SubDetalleCotizacion Sub;
                private SqlDataReader dr;
                private readonly string ID;
                private readonly bool Estado;
                #endregion
                public GetItem(string _IDSUBDET, bool _ESTADO)
                {
                    ID = _IDSUBDET;
                    Estado = _ESTADO;
                    IsGetting = Obtener();
                    if (IsGetting)
                    {
                        Datos = Sub;
                    }
                }

                private bool Obtener()
                {
                    Query = "SELECT * FROM  COT_ENDOSUBDET WHERE ESTADO=@ESTADO AND IDSUBDET=@ID";

                    try
                    {
                        Conn = new Coneccion();
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@ID", ID);
                        Conn.Cmd.Parameters.AddWithValue("@ESTADO", Estado);

                        dr = Conn.Cmd.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            Validadores VAL = new Validadores();
                            Sub = new SubDetalleCotizacion {
                                IDSUBDET=dr["IDSUBDET"].ToString(),
                                IDENDODET = dr["IDENDODET"].ToString(),
                                IDENDO = dr["IDENDO"].ToString(),
                                LEVEL = dr["LEVEL"].ToString(),
                                POS_NR= dr["POS_NR"].ToString(),
                                NODE= dr["NODE"].ToString(),
                                ESMODELO = VAL.ParseoBoolean(dr["ESMODELO"].ToString()),
                                NOMBRE= dr["NOMBRE"].ToString(),
                                IDMODELBOM= dr["IDMODELBOM"].ToString(),
                                ANCHO = VAL.ParseoDouble(dr["ANCHO"].ToString()),
                                ALTO = VAL.ParseoDouble(dr["ALTO"].ToString()),
                                NETO = VAL.ParseoDouble(dr["NETO"].ToString()),
                                IVA=VAL.ParseoDouble(dr["IVA"].ToString()),
                                BRUTO=VAL.ParseoDouble(dr["BRUTO"].ToString()),
                                F_ASIGNACION=VAL.ParseoDateTime(dr["F_ASIGNACION"].ToString()),
                                ESTADO=VAL.ParseoBoolean(dr["ESTADO"].ToString()),
                            };
                            Conn.ConnPlabal.Close();
                            dr.Close();
                            return true;
                        }
                        else
                        {
                            Conn.ConnPlabal.Close();
                            dr.Close();
                            return false;
                        }

                    }
                    catch (Exception)
                    {
                        throw;

                    }
                }


            }

            public class CalcularPieza
            {
                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SqlDataReader dr;
                #endregion

                private readonly List<SubDetalleCotizacion> Sub;
                private readonly double ANCHO;
                private readonly double ALTO;
                private readonly string CANAL;

                public CalcularPieza(List<SubDetalleCotizacion> _Subdet, double Ancho, double Alto, string Canal)
                {
                    Sub = _Subdet;
                    ANCHO = Ancho;
                    ALTO = Alto;
                    CANAL = Canal;

                    Accion();

                }

                private void Accion()
                {
                    UpdatePLABALRow update;
                    foreach (var item in Sub)
                    {
                        if (item.LEVEL == "2")
                        {
                            if (item.ESMODELO)
                            {
                                int IDBOM = Convert.ToInt32(item.IDMODELBOM);

                                GetPrecioUnit getPrecio = new GetPrecioUnit(CANAL, IDBOM, ANCHO, ALTO);
                                update = new UpdatePLABALRow("COT_ENDOSUBDET", "IDSUBDET", item.IDSUBDET, "NETO", getPrecio.Neto);

                            }
                        }
                    }

                    SubDetalleCotizacion Level1 = Sub.Where(it => it.LEVEL == "1").First();
                    Cotizacion.MontosCotizacion Montos = new Cotizacion.MontosCotizacion();
                    double NetoPieza = Montos.GetNetoxLevel(Level1.IDENDO, Level1.IDENDODET, Level1.NODE, "2");
                    update = new UpdatePLABALRow("COT_ENDOSUBDET", "IDSUBDET", Level1.IDSUBDET, "NETO", NetoPieza);

                    Cotizacion.MontosCotizacion montos = new Cotizacion.MontosCotizacion();
                    double COSTDIRUN = montos.GetNetoxLevel(Level1.IDENDO, Level1.IDENDODET, Level1.NODE, "1");
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", Level1.IDENDODET, "COSTDIRUN",COSTDIRUN);

                    DetalleCotizacion.GetItem itemcot = new DetalleCotizacion.GetItem(Level1.IDENDODET, true);
                    DetalleCotizacion.ActualizarMontositem actualizaritem = new DetalleCotizacion.ActualizarMontositem(itemcot.Item);
                }
            }

            public class UpdateMontos
                {
                    public UpdateMontos(string IDENDO,string Canal)
                    {
                    //obtiene todas las piezas de la cotizacion
                        Get get = new Get(1,IDENDO,true);

                    foreach (SubDetalleCotizacion item in get.SubDetalle)
                    {
                        //por cada una de las piezas de la cotizacion se obtiene el detalle
                        
                        Get bom = new Get(item.NODE,2,IDENDO,item.IDENDODET,true);

                        
                        foreach (SubDetalleCotizacion itembom in bom.SubDetalle)
                        {
                            //por cada item se obtiene el precio del modelo y se hace el cálculo
                            int idmodelbom = Convert.ToInt32(itembom.IDMODELBOM);
                            GetPrecioUnit precioUnit = new GetPrecioUnit(Canal,idmodelbom,item.ANCHO,item.ALTO);
                            if (precioUnit.Neto!=itembom.NETO)
                            {
                                UpdatePLABALRow update = new UpdatePLABALRow("COT_ENDOSUBDET","IDSUBDET",itembom.IDSUBDET,"NETO", precioUnit.Neto);
                            }
                        }
                    }
                    }
                }
        }

        public class DetalleCotizacion
        {

            #region Variables publicas
            public string IDENDODET { get; set; }
            public string IDENDO { get; set; }
            public bool ESMODELO { get; set; }
            public string IDMODELO { get; set; }
            public string NOMBRE { get; set; }
            public double CANTIDAD { get; set; }
            public string OBSERVACION { get; set; }
            public double COSTMOUN { get; set; }
            public double COSTDIRUN { get; set; }
            public double BENECONOMUN { get; set; }
            public double NETOUN { get; set; }
            public double IVAUN { get; set; }
            public double BRUTOUN { get; set; }
            public double NETO { get; set; }
            public double IVA { get; set; }
            public double BRUTO { get; set; }
            public DateTime F_CREACION { get; set; }
            public string IMAGEN { get; set; }
            public bool ESTADO { get; set; }
            public string POS_NR { get; set; }
            #endregion


            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion


            public DetalleCotizacion()
            {


            }

            

            public void GetPOS_NR(string IDENDO, bool ESTADO)
            {
                Query = "SELECT TOP 1 POS_NR FROM COT_ENDODET WHERE IDENDO=@IDENDO AND ESTADO=@ESTADO ORDER BY POS_NR DESC";

                try
                {
                    Conn = new Coneccion();
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@IDENDO", IDENDO);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                    
                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        int Nro = Convert.ToInt32(dr[0].ToString());
                        POS_NR = (Nro +1).ToString();
                    } 
                    else
                    {
                        POS_NR= "1";
                    }
                    Conn.ConnPlabal.Close();
                    dr.Close();

                }
                catch (Exception)
                {
                    throw;

                }

            }

            public class Firstentry
            {
                public readonly bool IsInserted;
                public readonly string _IDENDODET;

                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SqlDataReader dr;
                #endregion

                private DetalleCotizacion Det;
                public Firstentry(DetalleCotizacion _detalle)
                {
                    Det = _detalle;
                    IsInserted = Insert();
                    if (IsInserted)
                    {
                        _IDENDODET = GETid();
                        if (Det.ESMODELO)
                        {
                            BomModel bom = new BomModel(Det.IDMODELO,true);
                            var Bommito = bom.Lista.Where(iterator=> iterator.OPCIONAL==false);

                            List<SubDetalleCotizacion> Subdet = new List<SubDetalleCotizacion>();
                            int NR = 1;
                            foreach (var item in Bommito)
                            {
                                SubDetalleCotizacion Subito = new SubDetalleCotizacion {
                                    IDENDO = Det.IDENDO,
                                    IDENDODET = _IDENDODET,
                                    IDMODELBOM = item.ID,
                                    F_ASIGNACION = Det.F_CREACION,
                                    IVA=0,
                                    BRUTO=0,
                                    NETO=0,
                                    ALTO=0,
                                    ANCHO=0,
                                    ESMODELO=Det.ESMODELO,
                                    ESTADO=Det.ESTADO,
                                    LEVEL=item.LEVEL,
                                    NODE=item.NODE,
                                    NOMBRE = item.NOMBRE,
                                    
                                };
                                if (item.LEVEL=="1")
                                {
                                    Subito.POS_NR = item.POS_NR;
                                }
                                else
                                {
                                    Subito.POS_NR = item.NODE + NR;
                                    NR++;
                                }
                                SubDetalleCotizacion.FirstEntry firstEntry = new SubDetalleCotizacion.FirstEntry(Subito);
                                

                            }

                        }
                    }
                }


                private bool Insert()
                {
                    Conn = new Coneccion();
                    Query = "INSERT INTO COT_ENDODET (IDENDO,ESMODELO,IDMODELO,NOMBRE,POS_NR,CANTIDAD,OBSERVACION," +
                        "COSTMOUN,COSTDIRUN,BENECONOMUN,NETOUN,IVAUN,BRUTOUN,NETO,IVA,BRUTO,F_CREACION,IMAGEN,ESTADO)" +
                        " VALUES (@IDENDO,@ESMODELO,@IDMODELO,@NOMBRE,@POS_NR,@CANTIDAD,@OBSERVACION," +
                        "@COSTMOUN,@COSTDIRUN,@BENECONOMUN,@NETOUN,@IVAUN,@BRUTOUN,@NETO,@IVA,@BRUTO,@F_CREACION,@IMAGEN,@ESTADO)";
                    try
                    {
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@IDENDO",Det.IDENDO );
                        Conn.Cmd.Parameters.AddWithValue("@ESMODELO", Det.ESMODELO);
                        Conn.Cmd.Parameters.AddWithValue("@IDMODELO", Det.IDMODELO);
                        Conn.Cmd.Parameters.AddWithValue("@NOMBRE", Det.NOMBRE);
                        Conn.Cmd.Parameters.AddWithValue("@POS_NR", Det.POS_NR);
                        Conn.Cmd.Parameters.AddWithValue("@CANTIDAD", Det.CANTIDAD);
                        Conn.Cmd.Parameters.AddWithValue("@OBSERVACION", Det.OBSERVACION);
                        Conn.Cmd.Parameters.AddWithValue("@NETO", Det.NETO);
                        Conn.Cmd.Parameters.AddWithValue("@IVA", Det.IVA);
                        Conn.Cmd.Parameters.AddWithValue("@BRUTO", Det.BRUTO);
                        Conn.Cmd.Parameters.AddWithValue("@NETOUN", Det.NETOUN);
                        Conn.Cmd.Parameters.AddWithValue("@COSTMOUN", Det.COSTMOUN);
                        Conn.Cmd.Parameters.AddWithValue("@COSTDIRUN", Det.COSTDIRUN);
                        Conn.Cmd.Parameters.AddWithValue("@BENECONOMUN", Det.BENECONOMUN);
                        Conn.Cmd.Parameters.AddWithValue("@IVAUN", Det.IVAUN);
                        Conn.Cmd.Parameters.AddWithValue("@BRUTOUN", Det.BRUTOUN);
                        Conn.Cmd.Parameters.AddWithValue("@F_CREACION", Det.F_CREACION);
                        Conn.Cmd.Parameters.AddWithValue("@IMAGEN", Det.IMAGEN);
                        Conn.Cmd.Parameters.AddWithValue("@ESTADO", Det.ESTADO);
                        Conn.Cmd.ExecuteNonQuery();
                        Conn.ConnPlabal.Close();
                        return true;

                    }
                    catch
                    {
                        return false;
                    }

                }

                private string GETid()
                {
                    Query = "SELECT IDENDODET FROM COT_ENDODET WHERE IDENDO=@IDENDO AND " +
                        "ESMODELO=@ESMODELO AND IDMODELO=@IDMODELO AND NOMBRE=@NOMBRE AND ESTADO=@ESTADO AND POS_NR=@POS_NR";

                    try
                    {
                        Conn = new Coneccion();
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@IDENDO", Det.IDENDO);
                        Conn.Cmd.Parameters.AddWithValue("@ESMODELO", Det.ESMODELO);
                        Conn.Cmd.Parameters.AddWithValue("@IDMODELO", Det.IDMODELO);
                        Conn.Cmd.Parameters.AddWithValue("@NOMBRE", Det.NOMBRE);
                        Conn.Cmd.Parameters.AddWithValue("@ESTADO", Det.ESTADO);
                        Conn.Cmd.Parameters.AddWithValue("@POS_NR", Det.POS_NR);
                        dr = Conn.Cmd.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            string Valor = dr[0].ToString();
                            Conn.ConnPlabal.Close();
                            dr.Close();
                            return Valor;
                        }
                        else
                        {
                            Conn.ConnPlabal.Close();
                            dr.Close();
                            return null;
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                        
                    }
                    

                }

                
               
            }

            public class Get
            {
                public List<DetalleCotizacion> Detalle { get; set; }
                public readonly bool HasDetail;

                private readonly string _IDENDO;
                private readonly bool _ESTADO;
                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private SqlDataReader dr;
                #endregion


                public Get(string IDENDO, bool ESTADO)
                {
                    _IDENDO = IDENDO;
                    _ESTADO = ESTADO;
                    HasDetail = Obtain();
                }

                private bool Obtain()
                {
                    Conn = new Coneccion();
                    Detalle = new List<DetalleCotizacion>();
                    Query = "SELECT * FROM COT_ENDODET WHERE IDENDO=@IDENDO AND ESTADO=@ESTADO";

                    DataTable Dtable = new DataTable();
                    using (Conn.ConnPlabal)
                    {
                        try
                        {
                            SqlDataAdapter adap = new SqlDataAdapter(Query, Conn.ConnPlabal);

                            adap.SelectCommand.Parameters.AddWithValue("@IDENDO", _IDENDO);
                            adap.SelectCommand.Parameters.AddWithValue("@ESTADO", _ESTADO);

                            adap.Fill(Dtable);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        if (Dtable.Rows.Count > 0)
                        {
                            foreach (DataRow row in Dtable.Rows)
                            {
                                Validadores vAL = new Validadores();
                                DetalleCotizacion Item = new DetalleCotizacion {
                                    
                                   ESMODELO= vAL.ParseoBoolean(row["ESMODELO"].ToString()),
                                   CANTIDAD = vAL.ParseoDouble(row["CANTIDAD"].ToString()),
                                   IDENDO = row["IDENDO"].ToString(),
                                   IDENDODET= row["IDENDODET"].ToString(),
                                   IDMODELO = row["IDMODELO"].ToString(),
                                   ESTADO = vAL.ParseoBoolean(row["ESTADO"].ToString()),
                                   F_CREACION = vAL.ParseoDateTime(row["F_CREACION"].ToString()),
                                   OBSERVACION= row["OBSERVACION"].ToString(),
                                   POS_NR= row["POS_NR"].ToString(),
                                   IMAGEN = row["IMAGEN"].ToString(),
                                   NOMBRE= row["NOMBRE"].ToString(),
                                   BENECONOMUN = vAL.ParseoDouble(row["BENECONOMUN"].ToString()),
                                   BRUTO= vAL.ParseoDouble(row["BRUTO"].ToString()),
                                   BRUTOUN= vAL.ParseoDouble(row["BRUTOUN"].ToString()),
                                   COSTDIRUN= vAL.ParseoDouble(row["COSTDIRUN"].ToString()),
                                   COSTMOUN= vAL.ParseoDouble(row["COSTMOUN"].ToString()),
                                   IVA= vAL.ParseoDouble(row["IVA"].ToString()),
                                   IVAUN= vAL.ParseoDouble(row["IVAUN"].ToString()),
                                   NETO= vAL.ParseoDouble(row["NETO"].ToString()),
                                   NETOUN= vAL.ParseoDouble(row["NETOUN"].ToString()),


                                };
                                Detalle.Add(Item);
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }
            }

            public class GetItem
            {

                public readonly DetalleCotizacion Item;
                public readonly bool IsGetting;
                #region Manejo de datos
                Coneccion Conn;
                private string Query;
                private DetalleCotizacion dET;
                private SqlDataReader dr;
                private readonly string ID;
                private readonly bool Estado;
                #endregion
                public GetItem(string _IDENDODET, bool _ESTADO)
                {
                    ID = _IDENDODET;
                    Estado = _ESTADO;
                    IsGetting = Obtener();
                    if (IsGetting)
                    {
                        Item = dET;
                    }
                }

                private bool Obtener()
                {
                    Query = "SELECT * FROM  COT_ENDODET WHERE ESTADO=@ESTADO AND IDENDODET=@ID";

                    try
                    {
                        Conn = new Coneccion();
                        Conn.ConnPlabal.Open();
                        Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                        Conn.Cmd.Parameters.AddWithValue("@ID", ID);
                        Conn.Cmd.Parameters.AddWithValue("@ESTADO", Estado);

                        dr = Conn.Cmd.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            Validadores VAL = new Validadores();
                            dET = new DetalleCotizacion {
                                IDENDODET = dr["IDENDODET"].ToString(),
                                IDENDO = dr["IDENDO"].ToString(),
                                ESMODELO = VAL.ParseoBoolean(dr["ESMODELO"].ToString()),
                                IDMODELO = dr["IDMODELO"].ToString(),
                                NOMBRE = dr["NOMBRE"].ToString(),
                                POS_NR=dr["POS_NR"].ToString(),
                                CANTIDAD = VAL.ParseoDouble(dr["CANTIDAD"].ToString()),
                                OBSERVACION = dr["OBSERVACION"].ToString(),
                                COSTMOUN=VAL.ParseoDouble(dr["COSTMOUN"].ToString()),
                                COSTDIRUN=VAL.ParseoDouble(dr["COSTDIRUN"].ToString()),
                                BENECONOMUN= VAL.ParseoDouble(dr["BENECONOMUN"].ToString()),
                                NETOUN= VAL.ParseoDouble(dr["NETOUN"].ToString()),
                                IVAUN= VAL.ParseoDouble(dr["IVAUN"].ToString()),
                                BRUTOUN= VAL.ParseoDouble(dr["BRUTOUN"].ToString()),
                                NETO= VAL.ParseoDouble(dr["NETO"].ToString()),
                                IVA= VAL.ParseoDouble(dr["IVA"].ToString()),
                                BRUTO= VAL.ParseoDouble(dr["BRUTO"].ToString()),
                                F_CREACION=VAL.ParseoDateTime(dr["F_CREACION"].ToString()),
                                ESTADO = VAL.ParseoBoolean(dr["ESTADO"].ToString()),
                                IMAGEN = dr["IMAGEN"].ToString(),
                            };
                            Conn.ConnPlabal.Close();
                            dr.Close();
                            return true;
                        }
                        else
                        {
                            Conn.ConnPlabal.Close();
                            dr.Close();
                            return false;
                        }

                    }
                    catch (Exception)
                    {
                        throw;

                    }
                }


            }


            public class ActualizarMontositem
            {
                private readonly UpdatePLABALRow update;

                public readonly double NetoUn;
                public readonly double IvaUn;
                public readonly double BrutoUn;
                public readonly double NetoTotalItem;
                public readonly double IvaTotalItem;
                public readonly double BrutoTotalItem;


                public ActualizarMontositem(DetalleCotizacion item)
                {
                    /*margen de la cotizacion*/
                    Cotizacion.MontosCotizacion Montos = new Cotizacion.MontosCotizacion();
                    double[] valores = Montos.GetValores(item.IDENDO);
                    double SumCostos = item.COSTDIRUN + item.COSTMOUN;
                    GetMargen margen = new GetMargen(valores[4], SumCostos);
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "BENECONOMUN", margen.BenEcon);
                    NetoUn = item.COSTDIRUN + item.COSTMOUN + margen.BenEcon;
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "NETOUN", NetoUn);
                    IvaUn = Math.Round(NetoUn * 0.19, 0);
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "IVAUN", IvaUn);
                    BrutoUn = IvaUn + NetoUn;
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "BRUTOUN", BrutoUn);
                    NetoTotalItem = item.CANTIDAD * NetoUn;
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "NETO", NetoTotalItem);
                    IvaTotalItem = Math.Round(NetoTotalItem * 0.19, 0);
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "IVA", IvaTotalItem);
                    BrutoTotalItem = NetoTotalItem + IvaTotalItem;
                    update = new UpdatePLABALRow("COT_ENDODET", "IDENDODET", item.IDENDODET, "BRUTO", BrutoTotalItem);

                }
            }
        }

        public class GetPrecioUnit
        {
            public double Neto { get; set; }
            public double Beneficio { get; set; }
            public double Iva { get; set; }
            public double Bruto { get; set; }
            Coneccion Conn;
            private readonly BomModel item;
            private string Query;
            private readonly string IDbom;
            
            private readonly double Ancho;
            private readonly double Alto;
            private readonly double Cantidad;
            private readonly double Precio;
            private readonly double Merma;
            private readonly double SubNeto;
            static SqlDataReader dr;


            public GetPrecioUnit(string Canal, int _IdBom, double _ANCHO, double _ALTO)
            {
                IDbom = _IdBom.ToString();
                Ancho = _ANCHO;
                Alto = _ALTO;

                item = new BomModel(_IdBom, true);
                Cantidad = CalcCantidad(item.ItemBom.FORMULA);
                
                
                if (item.ItemBom.FROM_TAB== "COT_MCOMPONENTES")
                {
                    ComponentesClass _ClassComp = new ComponentesClass(item.ItemBom.ID_FROM_TAB,true);
                    Precio = _ClassComp._Detalle.PrecioUn;
                    if (_ClassComp._Detalle.IsGlass)
                    {
                        MermaVidrio mermaVidrio = new MermaVidrio(Ancho, Alto);
                        Merma = Math.Round(mermaVidrio.Merma*Cantidad,2);
                    }
                    else
                    {
                        Merma = 0;
                    }
                }
                else if (item.ItemBom.FROM_TAB == "COT_MPROCESOS")
                {
                    ProcesosClass _ClassPro = new ProcesosClass();
                    ProcesosClass._Proceso proceso = _ClassPro.GetProceso(item.ItemBom.ID_FROM_TAB, true);
                    Precio = proceso.Costo_Unit;
                    Merma = proceso.Merma*Cantidad;

                }
                
                SubNeto = Math.Round(Precio * (Cantidad + Merma),0);
                GetMargen margen = new GetMargen(Canal,SubNeto);
                Beneficio = Math.Round(margen.BenEcon,0);

                Neto = SubNeto + Beneficio;

                Iva = Math.Round(Neto * 0.19,0);
                Bruto = Neto + Iva;

            }

            private double CalcCantidad(string Formula)
            {
                double valor;
                if (double.TryParse(Formula,out valor))
                {
                    return valor;
                }
                else
                {
                    CalcEcuacion ecuacion = new CalcEcuacion(Formula, Ancho, Alto);
                    return ecuacion.Resultado;
                }

                
            }

            
            
        }

        public class GetListModelos
        {

            Coneccion Conn;
            public List<Modelos._Modelo> Modelos { get; set; }

            public GetListModelos(string CAMPO, object VALOR, bool ESTADO, string Canal)
            {
                Conn = new Coneccion();
                Modelos = new List<Modelos._Modelo>();
                string Consulta = "SELECT * FROM COT_PTABMODEL WHERE " + CAMPO + "=@valor and ESTADO=@estado";

                DataTable Dtable = new DataTable();
                using (Conn.ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adap = new SqlDataAdapter(Consulta, Conn.ConnPlabal);

                        adap.SelectCommand.Parameters.AddWithValue("@valor", VALOR);
                        adap.SelectCommand.Parameters.AddWithValue("@estado", ESTADO);

                        adap.Fill(Dtable);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    if (Dtable.Rows.Count > 0)
                    {
                        foreach (DataRow row in Dtable.Rows)
                        {
                            Modelos._Modelo item = new Modelos. _Modelo
                            {
                                ID = row["ID"].ToString(),
                                NOMBRE = row["NOMBRE"].ToString(),
                                DESCRIPCION = row["DESCRIPCION"].ToString(),
                                IMAGE = row["IMG_PATH"].ToString(),
                                F_Update = Convert.ToDateTime(row["F_UPDATE"].ToString()),
                                F_Created = Convert.ToDateTime(row["F_CREATED"].ToString()),
                                HASPIECES = Convert.ToBoolean(row["HASPIECES"]),
                                ESTADO = Convert.ToBoolean(row["ESTADO"].ToString()),
                                TokenId = row["TokenId"].ToString(),


                            };
                            Modelos.GetFamilyOfModel getFamily = new Modelos.GetFamilyOfModel(row["ID_PASIGFAMCAT"].ToString());
                            if (getFamily.HasVarLine)
                            {
                                item.Familia = getFamily.Familia;
                                item.IdFamilia = getFamily.IdFamily;
                                item.ID_PASIGFAMCAT = Convert.ToInt32(getFamily.IdAsign);
                                item.Categoria = getFamily.Categoria;
                                item.IdCategoria = getFamily.IdCategoria;
                            }
                            Modelos.Add(item);
                        }
                    }
                }

            }
        }

        
    }

    public class CalcEcuacion
    {

        public double Resultado { get; set; }

        Coneccion Conn;
        private string Query;
        static SqlDataReader dr;
        private readonly double Ancho;
        private readonly double Alto;
        public CalcEcuacion(string _Formula, double _Ancho, double _Alto)
        {
            Ancho = _Ancho;
            Alto = _Alto;
            Calculo(_Formula);
        }

        private void Calculo(string Formula)
        {
            Query = "DECLARE @TEMP TABLE (ANCHO FLOAT, ALTO FLOAT)" +
                "INSERT INTO @TEMP VALUES(@ANCHO,@ALTO) SELECT " + Formula + " FROM @TEMP";
            if (!string.IsNullOrEmpty(Formula))
            {
                Conn = new Coneccion();
                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.CmdPlabal.Parameters.AddWithValue("@ANCHO", Ancho);
                    Conn.CmdPlabal.Parameters.AddWithValue("@ALTO", Alto);

                    dr = Conn.CmdPlabal.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Validadores VAL = new Validadores();
                        Resultado= VAL.ParseoDouble(dr[0].ToString());
                    }
                    else
                    {
                        Resultado = 0;
                    }
                    Conn.ConnPlabal.Close();
                    dr.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                Resultado= 0;
            }

        }
    }

    public class GetMargen
    {
        public readonly double Margen;
        public readonly double BenEcon;
        public readonly double BenEconSinRed;
        public readonly double Precio;
        public readonly double PrecioSinRed;

        Coneccion Conn;
        private string Query;
        static SqlDataReader dr;
        private readonly string Canal;
        private readonly double Costo;

        public GetMargen(string CANAL, double _Costo)
        {
            Canal = CANAL;
            Costo = _Costo;
            Margen=Get();
            BenEcon = Math.Round(CalcBenEcon(),0);
            Precio = _Costo + BenEcon;

        }

        private double CalcBenEcon() { return Costo * Margen / (1 - Margen); }

        public GetMargen(double _Margen, double _Costo)
        {
            Margen = _Margen;
            Costo = _Costo;
            BenEconSinRed = CalcBenEcon();
            BenEcon = Math.Round(BenEconSinRed,0);
            Precio = _Costo + BenEcon;
            PrecioSinRed = _Costo + BenEconSinRed;
        }

        
        private double Get()
        {
            Query = "SELECT * FROM COT_PTABCANALES WHERE ESTADO=@ESTADO AND CHANNAME=@CANAL";
            Conn = new Coneccion();
            bool ESTADO = true;

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@CANAL", Canal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);

            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Validadores val = new Validadores();
                double valor = val.ParseoDouble(dr["MARGEN"].ToString());
                Conn.ConnPlabal.Close();
                dr.Close();
                return valor;
            }
            else
            {
                Conn.ConnPlabal.Close();
                dr.Close();
                return 0;
            }
        }

    }

    public class GetDescuento
    {
        public readonly double Descuento;
        public readonly double Resultado;
        

        public GetDescuento(double Porcentaje, double Monto)
        {
            Descuento = Math.Round(Porcentaje * Monto,0);
            Resultado = Monto - Descuento;

        }
    }

    public class MermaVidrio
    {
        public double Merma { get; set; }

        private readonly double _Area;


        public MermaVidrio(double Area)
        {
            _Area = Area;
            Accion();
            
        }

        public MermaVidrio(double _Ancho, double _Alto)
        {
            _Area = (_Ancho / 1000) * (_Alto / 1000);
            Accion();
        }

        private void Accion()
        {
            if (0.09 <= _Area && _Area <= 1)
            {
                Merma = Funcion(_Area, 0.19, 1, 5.3);
            }
            else if (1 < _Area && _Area <= 7)
            {
                Merma = Funcion(_Area, 0.3, 4.9, 140);
            }
            else if (7 < _Area && _Area <= 9)
            {
                Merma = Funcion(_Area, 0.27, 7, 15);
            }
            else
            {
                Merma = 0.3;
            }
        }
       
        private double Funcion (double x, double H, double K, double p)
        {
            return Math.Round((H- (Math.Pow(x - K, 2)/p)),3);
        }

        
    }
}
