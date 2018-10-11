using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GlobalInfo;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Web.Security;
using nsCliente;
using Alfak;
using Comercial;
using System.Web.UI.HtmlControls;
using OfficeOpenXml;
using System.Collections;

/// <summary>
/// Descripción breve de IngresoPedidos
/// </summary>
/// 

namespace Ecommerce
{

    /*para actualizar fechas de productos en Alfak*/
    public class DDDF
    {
        public DDDF()
        {
            object[] Param = new object[] {"1","1"};
            string[] PRMN = new string[] {"LISTA","SCHL" };
            SelectRows select = new SelectRows("ALFAK", "SYSADM.PR_PREIS PR, SYSADM.BA_PRODUKTE BA, SYSADM.BA_PRODUKTE_BEZ BEZ, SYSADM.PR_PREISKPF KPF", "BA.BA_PRODUKT",
                "PR.ID=BA.BA_PRODUKT AND BA.BA_PRODUKT = BEZ.BA_PRODUKT AND BA.BA_PRODUKT = PR.ID AND PR.ID = KPF.ID AND KPF.LISTE_ID =@LISTA AND PR.LISTE_ID =@LISTA AND KPF.SCHLS_ID =@SCHL AND PR.SCHLS_ID =@SCHL AND BA.BA_WGR IN('B11', 'B21', 'B31', 'B71', 'C11', 'H31', 'H51', 'H61', 'H71')",Param,PRMN);

            if (select.IsGot)
            {
                foreach (DataRow item in select.Datos.Rows)
                {
                    object[] Pr = new object[] { DateTime.Today, item["BA_PRODUKT"].ToString() };
                    string[] PrM = new string[] {"DATUM","ID" };
                    UpdateRow update = new UpdateRow("ALFAK", "SYSADM.PR_PREISKPF", "DATUM=@DATUM", "ID=@ID AND LISTE_ID='1' AND SCHLS_ID='1'", Pr, PrM);
                    if (update.Actualizado)
                    {
                        bool upd = true;
                    }
                }

            }
        }
    }
    /*--------------------------------------*/


    public static class Constantes
    {
        public const int HoraCorte = 14;

        public const double FactorTExpress = -0.235;

        public const double IVA19 = 19;

    }

    public class GetDiaHabil
    {
        
        public static DateTime Fecha(int Dias)
        {
            GetFeriados F = new GetFeriados(DateTime.Today);

            for (int i = 0; i < Dias; i++)
            {
                int Ndia = i + 1;
                DateTime dia = DateTime.Today.AddDays(Ndia);
                if (F.Feriados.Exists(it => it == dia) || dia.DayOfWeek.ToString() == "Saturday" || dia.DayOfWeek.ToString() == "Sunday")
                {
                    Dias++;
                }


            }
            return DateTime.Today.AddDays(Dias);

        }
    }


    public static class FactorExpress
    {
        
        
        public static double Normal(int MatrizEntrega,int NroDelDia)
        {
            if (MatrizEntrega>=NroDelDia)
            {
                int K = MatrizEntrega;
                int Y = NroDelDia;
                double Retorno = ((K + 1 - Y) * Math.Exp(Y * Constantes.FactorTExpress)) / 10;
                return Math.Round(Retorno, 2);
            }
            else
            {
                return 0;
            }
            
        }

        
    }

    public class FechasExpress
    {
        public int Index { get; set; }
        public DateTime Fecha { get; set; }
        public double FactorExp { get; set; }

        public class Get
        {
            public List<FechasExpress> Items;
            public DateTime FechaCorte;
            public int DiaCorte;
            public int DiasEntrega;


            private  GetFeriados F;
            private int Y0;

            
            private readonly string Tipo;
            
            
            private readonly int Dia;
            private readonly int TratoDiasentrega;


            public Get(string _Tipo, int _TratoDiasentrega, int Dias)
            {
                Tipo = _Tipo;
                Dia = Dias;
                TratoDiasentrega = _TratoDiasentrega;
                FPrimero();

            }

            public Get(string _Tipo,int Dias)
            {
                Tipo = _Tipo;
                Dia = Dias;
                TratoDiasentrega = 0;
                FPrimero();
            }

            private void FPrimero()
            {

                if (TratoDiasentrega > 0)
                {
                    Y0 = TratoDiasentrega;
                    if (DateTime.Now.Hour > Constantes.HoraCorte)
                    {
                        Y0++;
                    }
                    Calcular();
                }
                else
                {
                    GetDiaCorte diaCorte = new GetDiaCorte(Tipo);
                    if (diaCorte.IsSuccess)
                    {
                        Y0 = diaCorte.DiaCorte;
                        if (DateTime.Now.Hour > Constantes.HoraCorte)
                        {
                            Y0++;
                        }
                        Calcular();
                    }
                }

            }

            private void Calcular()
            {
                F = new GetFeriados(DateTime.Today);
                Items = new List<FechasExpress>();
                int corte = Dia;
                int cont = 1;
                DiasEntrega = Y0;
                
                Hashtable HT = new Hashtable() { {"ID",834 } };
                SelectRows select = new SelectRows("ALFAK", "SYSADM.PR_PREIS", "GRENZE1,PREIS", "LISTE_ID=1 AND SCHLS_ID=1 AND ID=@ID",HT);
                HT = new Hashtable();
                foreach (DataRow grnxe in select.Datos.Rows)
                {
                    double grenze = Convert.ToDouble(grnxe["GRENZE1"].ToString());
                    HT.Add(Convert.ToInt32(grenze), Convert.ToDouble(grnxe["PREIS"].ToString()));
                }

                double Fact;
                for (int i = 0; i < corte; i++)
                {
                    int Ndia = i + 1;
                    DateTime dia = DateTime.Today.AddDays(Ndia);
                    if (F.Feriados.Exists(it => it == dia) || dia.DayOfWeek.ToString() == "Saturday" || dia.DayOfWeek.ToString() == "Sunday")
                    {
                        corte++;
                    }
                    else
                    {
                         
                        if (cont == Y0)
                        {
                            DiaCorte = Ndia;
                            FechaCorte = dia;
                        }
                        
                        if (HT.ContainsKey(cont) && cont<Y0)
                        {
                            Fact = (double)HT[cont];
                        }
                        else
                        {
                            Fact = 0;
                        }
                        
                        
                        FechasExpress factor = new FechasExpress
                        {
                            Index = cont,
                            FactorExp = Fact,
                            Fecha = dia,
                        };
                        Items.Add(factor);
                        cont++;
                        
                    }

                }
                

            }


        }

        

        public class GetFechaCorte
        {
            public DateTime FechaCorte;
            public int DiasEntrega;

            private readonly GetInfoClienteEcomm ClienteEcom;

            public GetFechaCorte(string _Tipo,string Rut)
            {
                ClienteEcom = new GetInfoClienteEcomm(Rut);
                if (ClienteEcom.DiasDeTrato > 0)
                {
                    DiasEntrega = ClienteEcom.DiasDeTrato;

                    
                }
                else
                {
                    GetDiaCorte diaCorte = new GetDiaCorte(_Tipo);
                    if (diaCorte.IsSuccess)
                    {
                        DiasEntrega = diaCorte.DiaCorte;
                    }
                }

                if (DateTime.Now.Hour > Constantes.HoraCorte)
                {
                    DiasEntrega++;
                }
                GetDate();


            }

            private void GetDate()
            {
                GetFeriados F = new GetFeriados(DateTime.Today);
                int end = DiasEntrega;
                for (int i = 0; i < end; i++)
                {
                    int Ndia = i + 1;
                    DateTime dia = DateTime.Today.AddDays(Ndia);
                    if (F.Feriados.Exists(it => it == dia) || dia.DayOfWeek.ToString() == "Saturday" || dia.DayOfWeek.ToString() == "Sunday")
                    {
                        end++;
                    }
                    

                }
                FechaCorte = DateTime.Today.AddDays(end);
            }
        }

    }


    public class CostodelDia
    {
        public DateTime Fecha { get; set; }
        public double PrEntrega { get; set; }
        public double PrDespacho { get; set; }
        public bool IsCorte { get; set; }

        public class Get
        {
            public List<CostodelDia> Lista;

            private  int start;
            private int end;
            private FechasExpress.Get FExpress;
            private GetFeriados Feriados;

            private readonly PedidoEcom _Pedido;
            private readonly GetInfoClienteEcomm Cliente;
            private readonly int Pagina;
            private readonly bool ESTADO;
            private readonly string Region;
            private readonly string Comuna;



            public Get(PedidoEcom pedido, int _Pagina, bool _ESTADO, string _Region, string _Comuna)
            {
                _Pedido = pedido;
                Pagina = _Pagina;
                Cliente = new GetInfoClienteEcomm(pedido.RUT);
                ESTADO = _ESTADO;
                Comuna = _Comuna;
                Region = _Region;
                HacerLista();
            }


            public Get(PedidoEcom pedido, int _Pagina, bool _ESTADO)
            {
                _Pedido = pedido;
                Pagina = _Pagina;
                ESTADO = _ESTADO;
                Cliente = new GetInfoClienteEcomm(pedido.RUT);
                if (string.IsNullOrWhiteSpace(pedido.COD_REGION))
                {
                    DatosCliente datoscli = new DatosCliente(pedido.RUT);
                    Region = datoscli.Region;
                    DistPoliticaChile.GetComunas getComunas = new DistPoliticaChile.GetComunas(Region);
                    Comuna = getComunas.Comunas.Select(it=>it.Codigo).First();
                }
                else
                {
                    Region = pedido.COD_REGION;
                    Comuna = pedido.COD_COMUNA;
                }
                
                
                HacerLista();
            }

            private void HacerLista()
            {
                
                start = (5 * Pagina) - 4;
                end = start + 7;

                PedDet.Get P = new PedDet.Get(_Pedido.ID, ESTADO);
                GetMargen Margen;
                FechasExpress DatosExp;
                Lista = new List<CostodelDia>();
                FExpress = new FechasExpress.Get(_Pedido.OrderType,Cliente.DiasDeTrato,end);
                double NetoNormalEntr = 0;
                
                GetFlete Flete = new GetFlete(P.Items.Sum(it=>it.KGITM), Region, Comuna, true);
                foreach (PedDet it in P.Items)
                {
                    Margen = new GetMargen(Cliente.Margen, (it.CMPUN + it.CMERMAUN + it.CPROCUN));
                    NetoNormalEntr = NetoNormalEntr + Margen.Precio*it.CANT;

                }

                
                for (int i = start; i < end; i++)
                {
                    DatosExp = FExpress.Items.Where(iterator => iterator.Index == i).First();
                    CostodelDia Cdia;
                    if (DatosExp.FactorExp>0)
                    {
                        double NCFexp = 0;
                        foreach (PedDet it in P.Items)
                        {
                            CostExpressUn CExpressUn = new CostExpressUn(FExpress.Items, it, DatosExp.Fecha);
                            Margen = new GetMargen(Cliente.Margen, (it.CMPUN + it.CMERMAUN + it.CPROCUN + CExpressUn.Costo));
                            NCFexp = NCFexp + Margen.Precio*it.CANT;

                        }


                        Cdia = new CostodelDia {
                            Fecha = DatosExp.Fecha,
                            PrDespacho = NCFexp + Flete.Costo,
                            PrEntrega = NCFexp,
                        };

                    }
                    else
                    {
                        Cdia = new CostodelDia {
                            Fecha = DatosExp.Fecha,
                            PrDespacho = NetoNormalEntr + Flete.Costo,
                            PrEntrega = NetoNormalEntr,
                            
                        };
                        if (i==FExpress.DiasEntrega)
                        {
                            Cdia.IsCorte = true;
                        }

                    }
                    Lista.Add(Cdia);





                }

                

            }




        }
    }

    public class CostExpressUn
    {
        public readonly double Costo;


        private readonly List<FechasExpress> FExpress;
        private readonly DateTime Fecha;
        private readonly double _CnetoUn;

        public CostExpressUn(string TipoPedido, PedDet _Item, DateTime _Fecha, GetInfoClienteEcomm Cliente)
        {
            Fecha = _Fecha;
            TimeSpan Tspan = Fecha.Subtract(DateTime.Today);
            FechasExpress.Get GetExp = new FechasExpress.Get(TipoPedido,Cliente.DiasDeTrato,Tspan.Days);
            FExpress = GetExp.Items;
            _CnetoUn = (_Item.CMPUN + _Item.CMERMAUN + _Item.CPROCUN);
            Costo = ObtenerC();
        }

        public CostExpressUn(List<FechasExpress> _FExpress, PedDet _Item, DateTime _Fecha)
        {
            Fecha = _Fecha;
            FExpress = _FExpress;
            _CnetoUn = (_Item.CMPUN + _Item.CMERMAUN + _Item.CPROCUN);
            Costo = ObtenerC();


        }

        private double ObtenerC()
        {
            double C = 0;
            if (FExpress.Exists(it => it.Fecha == Fecha))
            {
                double Fact = FExpress.Where(it => it.Fecha == Fecha).Select(it => it.FactorExp).Sum();
                C = _CnetoUn *Fact;
            }

            return Math.Round(C, 0);

        }
    }

    public class PedidoEcom
    {
        #region vARIABLES PUBLICAS
        public string ID { get; set; }
        public string UserId { get; set; }
        public string RUT { get; set; }
        public string Nombre { get; set; }
        public DateTime F_Ingreso { get; set; }
        public string Observa { get; set; }
        public string Estado { get; set; }
        public string NroPedido { get; set; }
        public double Neto { get; set; }
        public double Iva { get; set; }
        public double Bruto { get; set; }
        public int CanTotal { get; set; }
        public bool HasFile { get; set; }
        public string FileXLS { get; set; }
        public bool EsDespacho { get; set; }
        public string DirEntrega { get; set; }
        public DateTime F_Entrega { get; set; }
        public string OrderType { get; set; }
        public double KGTOTAL { get; set; }
        public string TokenId { get; set; }
        public double CDIRECTO { get; set; }
        public double CEXPRESS { get; set; }
        public double CDESPACHO { get; set; }
        public double BENECONOM { get; set; }
        public string COD_COMUNA { get; set; }
        public  string COD_REGION { get; set; }
        #endregion


        public PedidoEcom()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public class FirstEntry
        {
            public readonly bool IsSuccess;
            public readonly string ID;
            public readonly string Token;

            private readonly string OrderType;
            private readonly string Nombre;
            private readonly string Observa;
            private readonly string rut;
            private readonly string Region;
            private readonly GetInfoClienteEcomm ClienteEcom;


            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion


            public FirstEntry(string ordertype, string nombre, string observa, string Username, DatosCliente Cli)
            {
                OrderType = ordertype;
                Nombre = nombre;
                Observa = observa;
                rut = Cli.Rut;
                ClienteEcom = new GetInfoClienteEcomm(rut);
                Region = Cli.Region;
                MembershipUser Miembro = Membership.GetUser(Username);
                var UserId = (Guid)Miembro.ProviderUserKey;
                string[] valores = Generar(UserId.ToString());
                if (!string.IsNullOrEmpty(valores[0]) && !string.IsNullOrEmpty(valores[1]))
                {
                    ID = valores[0];
                    Token = valores[1];
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
                int TokenId = random.Next(10000, 99999999);
                Encriptacion encriptacion = new Encriptacion(TokenId.ToString());
                string[] RetStr = new string[2];
                RetStr[1] = encriptacion.TokenEncriptado;

                if (Insert(TokenId,_UserId))
                {
                    RetStr[0] = GetId(TokenId,_UserId);
                }


                return RetStr;
            }

            public string GetId(int TOKENID, string USERID)
            {
                Conn = new Coneccion();
                Query = "SELECT TOP 1 ID FROM ECOM_PEDIDOS WHERE UserId=@USERID AND RUTCLI=@RUT AND NOMBRE=@NOMBRE AND TOKENID=@TOKENID";
                Conn.ConnPlabal.Open();
                Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@USERID", USERID);
                Conn.Cmd.Parameters.AddWithValue("@RUT", rut );
                Conn.Cmd.Parameters.AddWithValue("@NOMBRE", Nombre);
                Conn.Cmd.Parameters.AddWithValue("@TOKENID", TOKENID);
                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    return dr[0].ToString();

                }
                else
                {
                    return null;
                }

            }

            private bool Insert(int TOKENID,string USERID)
            {

                
                FechasExpress.GetFechaCorte fechaCorte = new FechasExpress.GetFechaCorte(OrderType, rut);
                

                Conn = new Coneccion();
                Query = "INSERT INTO ECOM_PEDIDOS (UserId,RUTCLI,NOMBRE,F_INGRESO,OBSERVA,ESTADO,NETO,IVA,BRUTO," +
                    "CANTOTAL,ORDERTYPE,TOKENID,KGTOTAL,F_ENTREGA, COD_REGION )" +
                    " VALUES (@UserId,@RUTCLI,@NOMBRE,GETDATE(),@OBSERVA,'BOR',0,0,0," +
                    "0,@ORDERTYPE,@TOKENID,0,@F_ENTREGA,@COD_REGION)";
                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@UserId", USERID);
                    Conn.Cmd.Parameters.AddWithValue("@RUTCLI", rut);
                    Conn.Cmd.Parameters.AddWithValue("@NOMBRE", Nombre);
                    Conn.Cmd.Parameters.AddWithValue("@OBSERVA", Observa);
                    Conn.Cmd.Parameters.AddWithValue("@ORDERTYPE", OrderType);
                    Conn.Cmd.Parameters.AddWithValue("@TOKENID", TOKENID);
                    Conn.Cmd.Parameters.AddWithValue("@F_ENTREGA", fechaCorte.FechaCorte);
                    Conn.Cmd.Parameters.AddWithValue("@COD_REGION", Region);


                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();
                    return true;

                }
                catch
                {
                    return false;
                }

            }
        }

        public class Get
        {
            public PedidoEcom Pedido;
            public DatosCliente Cliente;
            public GetInfoClienteEcomm InfoCliente;
            public readonly bool IsSuccess;

            private readonly string ID;
            private readonly string TokenId;
            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            public Get(string _ID)
            {
                ID = _ID;
                IsSuccess= Action(1);
            }

            public Get(string _ID, string _Token)
            {
                ID = _ID;
                Encriptacion Enc = new Encriptacion(_Token);
                TokenId = Enc.TokenDesencriptado;
               IsSuccess= Action(2);
            }

            private void GetDatosCliente(string Rut)
            {
                Cliente = new DatosCliente(Rut);
            }

            private void GetDCli(string Rut)
            {
                InfoCliente = new GetInfoClienteEcomm(Rut);
            }

            private bool Action(int ctor)
            {
                Conn = new Coneccion();
                if (ctor==1)
                {
                    Query = "SELECT * FROM ECOM_PEDIDOS WHERE ID=@ID";
                }
                else if (ctor==2)
                {
                    Query = "SELECT * FROM ECOM_PEDIDOS WHERE ID=@ID AND TOKENID=@TOKENID";
                }
                
                Conn.ConnPlabal.Open();
                Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@ID", ID);
                if (ctor==2)
                {
                    Conn.Cmd.Parameters.AddWithValue("@TOKENID", TokenId);
                }
                
                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores Val = new Validadores();
                    Pedido = new PedidoEcom {
                        ID = ID,
                        UserId = dr["UserId"].ToString(),
                        RUT = dr["RUTCLI"].ToString(),
                        Nombre= dr["NOMBRE"].ToString(),
                        F_Ingreso = Val.ParseoDateTime(dr["F_INGRESO"].ToString()),
                        Observa= dr["OBSERVA"].ToString(),
                        Estado= dr["ESTADO"].ToString(),
                        NroPedido= dr["NROPEDIDO"].ToString(),
                        Neto=Val.ParseoDouble(dr["NETO"].ToString()),
                        Iva= Val.ParseoDouble(dr["IVA"].ToString()),
                        Bruto=Val.ParseoDouble(dr["BRUTO"].ToString()),
                        CanTotal=Convert.ToInt32(dr["CANTOTAL"].ToString()),
                        HasFile=Val.ParseoBoolean(dr["HASFILE"].ToString()),
                        FileXLS= dr["FILEXLS"].ToString(),
                        EsDespacho = Val.ParseoBoolean(dr["ESDESPACHO"].ToString()),
                        DirEntrega= dr["DIRENTREGA"].ToString(),
                        F_Entrega=Val.ParseoDateTime(dr["F_ENTREGA"].ToString()),
                        OrderType= dr["ORDERTYPE"].ToString(),
                        TokenId= dr["TOKENID"].ToString(),
                        BENECONOM= Val.ParseoDouble(dr["BENECONOM"].ToString()),
                        CDESPACHO= Val.ParseoDouble(dr["CDESPACHO"].ToString()),
                        CDIRECTO= Val.ParseoDouble(dr["CDIRECTO"].ToString()),
                        KGTOTAL = Val.ParseoDouble(dr["KGTOTAL"].ToString()),
                        COD_COMUNA=dr["COD_COMUNA"].ToString(),
                        COD_REGION=dr["COD_REGION"].ToString(),
                        CEXPRESS = Val.ParseoDouble(dr["CEXPRESS"].ToString()),

                    };
                    GetDCli(Pedido.RUT);
                    GetDatosCliente(Pedido.RUT);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class GetItems
        {
            private Hashtable Parametros;
            private SelectRows select;
            private Detalle Item;
            private int Index;
            private readonly Validadores Val;
            public List<Detalle> Lista;
            public bool TieneItems;

            public GetItems(string NroPedido)
            {
                Lista = new List<Detalle>();
                Index = 1;
                Val = new Validadores();
                Obtener(NroPedido);
                if (Lista.Count>0)
                {
                    TieneItems = true;
                }
            }

            public GetItems(string[] NroPedidos)
            {
                Lista = new List<Detalle>();
                Index = 1;
                Val = new Validadores();
                foreach (var item in NroPedidos)
                {
                    Obtener(item);
                }

                if (Lista.Count > 0)
                {
                    TieneItems = true;
                }
            }


            private void Obtener(string _NroPedido)
            {
                Parametros = new Hashtable { { RandomERP.ProcedimientosAlm.Detalle_Pedido.Variables.NRO, _NroPedido } };
                select = new SelectRows("RANDOM", RandomERP.ProcedimientosAlm.Detalle_Pedido.Name,Parametros);
                if (select.IsGot)
                {
                    foreach (DataRow dr in select.Datos.Rows)
                    {
                        Item = new Detalle {
                            CodRandom=dr["KOPR"].ToString(),
                            NroPedido=_NroPedido,
                            Cantidad=Val.ParseoInt(dr["CANTIDAD"].ToString()),
                            Ancho= Val.ParseoDouble(dr["ANCHO"].ToString()),
                            Alto= Val.ParseoDouble(dr["ALTO"].ToString()),
                            Descripcion=dr["DESCR"].ToString(),
                            Referencia = dr["REFERENCIA"].ToString(),
                            NetoUn=Val.ParseoDouble(dr["NETOUN"].ToString()),
                            Fabricado = Val.ParseoBoolean(dr["FABRICADO"].ToString()),
                            En_Bodega = Val.ParseoBoolean(dr["REALIZADO"].ToString()),
                            DocEntrega = dr["DOCUMENTO"].ToString(),
                        };
                        if (!string.IsNullOrWhiteSpace(Item.DocEntrega))
                        {
                            Item.Entregado = true;
                        }
                        if (Item.NetoUn>0)
                        {
                            Item.Neto = Math.Round(Item.NetoUn * Item.Cantidad,0);
                            Item.Iva = Math.Round(Item.Neto*0.19, 0);
                            Item.Bruto = Item.Neto + Item.Iva;
                        }
                        Item.Index = Index;
                        Lista.Add(Item);
                        Index++;
                    }
                }
                else
                {
                    Parametros = new Hashtable { { "ID", _NroPedido } };
                    select = new SelectRows("ALFAK", "SYSADM.BW_AUFTR_POS", "*", "ID=@ID", Parametros);
                    if (select.IsGot)
                    {
                        foreach (DataRow dr in select.Datos.Rows)
                        {
                            Item = new Detalle
                            {

                                NroPedido = _NroPedido,
                                Cantidad = Convert.ToInt32(Val.ParseoDouble(dr["PP_MENGE"].ToString())),
                                Ancho = Val.ParseoDouble(dr["PP_BREITE"].ToString()),
                                Alto = Val.ParseoDouble(dr["PP_HOEHE"].ToString()),
                                Descripcion = dr["PROD_BEZ1"].ToString(),
                                Referencia = dr["POS_KOMMISSION"].ToString(),
                                NetoUn = Val.ParseoDouble(dr["FI_POS_STK"].ToString()),
                                

                            };

                            if (Item.NetoUn > 0)
                            {
                                Item.Neto = Math.Round(Item.NetoUn * Item.Cantidad, 0);
                                Item.Iva = Math.Round(Item.Neto * 0.19, 0);
                                Item.Bruto = Item.Neto + Item.Iva;
                            }
                            Item.Index = Index;
                            Lista.Add(Item);
                            Index++;

                        }
                    }

                }

                
            }
            
        }

        public class GetOld
        {
            public PedidoEcom Pedido;
            public DatosCliente Cliente;
            public GetInfoClienteEcomm InfoCliente;
            public readonly bool IsSuccess;

            private readonly string ID;
            private readonly string Rut;


            public GetOld(string _ID, string _Rut)
            {
                ID = _ID;
                Rut = _Rut;
                GetPedido();
                GetCliente();
            }

            private void GetCliente()
            {
                Cliente = new DatosCliente(Rut);
                InfoCliente = new GetInfoClienteEcomm(Rut);
            }
            private void GetPedido()
            {
                
                Hashtable Htable = new Hashtable() { { "ID",ID} };
                SelectRows select = new SelectRows("PLABAL", "e_Pedidos", " * ", "id_pedidos=@ID",Htable);
                if ( select.IsGot)
                {
                    Validadores Val = new Validadores();
                    Pedido = new PedidoEcom {
                        ID = select.Datos.Rows[0]["id_pedidos"].ToString(),
                        Nombre = select.Datos.Rows[0]["nom_pedido"].ToString(),
                        Observa = select.Datos.Rows[0]["observacion"].ToString(),
                        F_Ingreso = Val.ParseoDateTime(select.Datos.Rows[0]["fecha_hora_pedido"].ToString()),
                        F_Entrega = Val.ParseoDateTime(select.Datos.Rows[0]["fecha_entrega"].ToString()),
                        Estado = select.Datos.Rows[0]["Estado"].ToString(),
                        NroPedido = select.Datos.Rows[0]["PedidoAlfak"].ToString(),
                        CanTotal = Val.ParseoInt(select.Datos.Rows[0]["Cantidad"].ToString()),
                        Neto = Val.ParseoDouble(select.Datos.Rows[0]["totalPedido"].ToString()),
                        
                    };
                    Pedido.Iva = Math.Round(Pedido.Neto * 0.19, 0);
                    Pedido.Bruto = Pedido.Neto + Pedido.Iva;

                }
            }
        }

        public class GetbyCli
        {
            public readonly List<PedidoEcom> Items;
            private readonly object[] Param;
            private readonly string[] ParamName;
            private readonly SelectRows Select;
            
            public GetbyCli(string RUT)
            {
                
                Items = new List<PedidoEcom>();
                Param = new object[] { RUT };
                ParamName = new string[] {"RUTCLI" };
                Select = new SelectRows("PLABAL", "ECOM_PEDIDOS", "*", "RUTCLI=@RUTCLI AND ESTADO<>'NUL'", Param, ParamName,"F_INGRESO DESC");
                GetValues();
            }

            public GetbyCli(SelectRows DatosDelSelect)
            {
                Items = new List<PedidoEcom>();
                Select = DatosDelSelect;
                GetValues();
            }

            private void GetValues()
            {
                foreach (DataRow dr in Select.Datos.Rows)
                {
                    Validadores Val = new Validadores();
                    PedidoEcom Pedido = new PedidoEcom
                    {
                        ID = dr["ID"].ToString(),
                        UserId = dr["UserId"].ToString(),
                        RUT = dr["RUTCLI"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        F_Ingreso = Val.ParseoDateTime(dr["F_INGRESO"].ToString()),
                        Observa = dr["OBSERVA"].ToString(),
                        Estado = dr["ESTADO"].ToString(),
                        NroPedido = dr["NROPEDIDO"].ToString(),
                        Neto = Val.ParseoDouble(dr["NETO"].ToString()),
                        Iva = Val.ParseoDouble(dr["IVA"].ToString()),
                        Bruto = Val.ParseoDouble(dr["BRUTO"].ToString()),
                        CanTotal = Convert.ToInt32(dr["CANTOTAL"].ToString()),
                        HasFile = Val.ParseoBoolean(dr["HASFILE"].ToString()),
                        FileXLS = dr["FILEXLS"].ToString(),
                        EsDespacho = Val.ParseoBoolean(dr["ESDESPACHO"].ToString()),
                        DirEntrega = dr["DIRENTREGA"].ToString(),
                        F_Entrega = Val.ParseoDateTime(dr["F_ENTREGA"].ToString()),
                        OrderType = dr["ORDERTYPE"].ToString(),
                        TokenId = dr["TOKENID"].ToString(),
                        BENECONOM = Val.ParseoDouble(dr["BENECONOM"].ToString()),
                        CDESPACHO = Val.ParseoDouble(dr["CDESPACHO"].ToString()),
                        CDIRECTO = Val.ParseoDouble(dr["CDIRECTO"].ToString()),
                        KGTOTAL = Val.ParseoDouble(dr["KGTOTAL"].ToString()),
                        COD_COMUNA = dr["COD_COMUNA"].ToString(),
                        COD_REGION = dr["COD_REGION"].ToString(),
                    };
                    Items.Add(Pedido);
                }


            }
        }

        public class OrderFiltering
        {
            public List<PedidoEcom> Items;
            private string Where;
            private string WhereOld;
            private Hashtable HTable;
            private Hashtable HTableOld;
            private SelectRows Select;
            private SelectRows SelectOld;
            private readonly string[] OType;
            private readonly string[] Status;
            private bool DoOld;
            private readonly string Rut;
            private GetbyCli getbyCli;
            private GetListOld getListOld;

            public OrderFiltering(string rut,DateTime DFrom, DateTime DTo, double MontoFrom, double MontoTo, string[] Ordertype, string[] Estados)
            {
                OType = Ordertype;
                Status = Estados;
                
                Where = "RUTCLI=@RUT AND ESTADO<>'NUL' AND BRUTO>=@MFrom AND BRUTO<=@MTo AND F_INGRESO>=@DFrom AND F_INGRESO<=@DTo";
                WhereOld = "TU.ID=US.Id_Tipo AND US.ID=pe.id_usuario and TU.Rut=@RUT and PE.Estado<>'NUL' AND totalPedido>=@MFrom AND totalPedido<=@MTo AND " +
                    "fecha_hora_pedido>=@DFrom AND fecha_hora_pedido<=@DTo";
                HTable = new Hashtable {
                    { "RUT",rut},{ "MFrom",MontoFrom},{ "MTo",MontoTo},{ "DFrom",DFrom},{ "DTo",DTo}
                };
                HTableOld = new Hashtable {
                    { "RUT",rut},{ "MFrom",MontoFrom/1.19},{ "MTo",MontoTo/1.19},{ "DFrom",DFrom},{ "DTo",DTo}
                };

                ArmarHtables();

                
            }

            private void Accion()
            {
                Where += " ORDER BY F_INGRESO DESC";
                WhereOld += " ORDER BY pe.fecha_hora_pedido DESC";
                Items = new List<PedidoEcom>();
                Select = new SelectRows("PLABAL", "ECOM_PEDIDOS", "TOP 10000 *", Where, HTable);
                if (Select.IsGot)
                {
                    getbyCli = new GetbyCli(Select);

                    Items.AddRange(getbyCli.Items);
                }
                

                if (DoOld)
                {

                    SelectOld = new SelectRows("PLABAL",
                        "e_Pedidos PE, e_TipoUsu TU, e_Usuario US",
                        "TOP 10000 PE.*,US.Nombre AS 'Usuario'",
                        WhereOld, HTableOld);
                    if (SelectOld.IsGot)
                    {
                        getListOld = new GetListOld(SelectOld);
                        Items.AddRange(getListOld.Items);
                    }
                    
                }

            }

            private void ArmarHtables()
            {
                if (OType != null)
                {
                    Where += " AND ORDERTYPE IN (" + string.Join(",", OType) + ")";
                    /*HTable.Add("ORTYPE", OType);*/
                    if (OType.Contains("'TER'"))
                    {
                        DoOld = true;
                    }
                }
                else
                {
                    DoOld = true;
                }

                if (Status != null)
                {
                    Where += " AND ESTADO IN (" + string.Join(",", Status) + ")";
                    //HTable.Add("ESTADOS", Status);
                    WhereOld += " AND PE.Estado IN (" + string.Join(",", Status) + ")";
                    //HTableOld.Add("ESTADOS", Status);
                }

                Accion();

            }

            public OrderFiltering(string rut, DateTime DTo, double MontoFrom, double MontoTo, string[] Ordertype, string[] Estados)
            {
                OType = Ordertype;
                Status = Estados;

                Where = "RUTCLI=@RUT AND ESTADO<>'NUL' AND BRUTO>=@MFrom AND BRUTO<=@MTo AND F_INGRESO<=@DTo";
                WhereOld = "TU.ID=US.Id_Tipo AND US.ID=pe.id_usuario and TU.Rut=@RUT and PE.Estado<>'NUL' AND totalPedido>=@MFrom AND totalPedido<=@MTo AND " +
                    " fecha_hora_pedido<=@DTo";
                HTable = new Hashtable {
                    { "RUT",rut},{ "MFrom",MontoFrom},{ "MTo",MontoTo},{ "DTo",DTo}
                };
                HTableOld = new Hashtable {
                    { "RUT",rut},{ "MFrom",MontoFrom},{ "MTo",MontoTo},{ "DTo",DTo}
                };

                ArmarHtables();


            }

            public OrderFiltering(string rut, double MontoFrom, double MontoTo, string[] Ordertype, string[] Estados)
            {
                OType = Ordertype;
                Status = Estados;

                Where = "RUTCLI=@RUT AND ESTADO<>'NUL' AND BRUTO>=@MFrom AND BRUTO<=@MTo ";
                WhereOld = "TU.ID=US.Id_Tipo AND US.ID=pe.id_usuario and TU.Rut=@RUT and PE.Estado<>'NUL' AND totalPedido>=@MFrom AND totalPedido<=@MTo ";
                HTable = new Hashtable {
                    { "RUT",rut},{ "MFrom",MontoFrom},{ "MTo",MontoTo}
                };
                HTableOld = new Hashtable {
                    { "RUT",rut},{ "MFrom",MontoFrom},{ "MTo",MontoTo}
                };

                ArmarHtables();


            }

            public OrderFiltering(string rut, string SearchString)
            {
                SearchString = "%" + SearchString + "%";
                Where = "RUTCLI=@RUT AND ESTADO<>'NUL' AND (NOMBRE LIKE @StrSearch OR OBSERVA LIKE @StrSearch OR NROPEDIDO LIKE @StrSearch)";
                WhereOld = "TU.ID=US.Id_Tipo AND US.ID=pe.id_usuario and TU.Rut=@RUT and PE.Estado<>'NUL' AND (nom_pedido LIKE @StrSearch OR observacion LIKE @StrSearch OR PedidoAlfak LIKE @StrSearch )";
                HTable = new Hashtable {
                    { "RUT",rut},{"StrSearch",SearchString }
                };
                HTableOld = new Hashtable {
                    { "RUT",rut},{"StrSearch",SearchString }
                };

                ArmarHtables();

            }
        }

        public class GetListOld
        {
            public readonly List<PedidoEcom> Items;
            private readonly object[] Param;
            private readonly string[] ParamName;
            private readonly SelectRows Select;
            public GetListOld(string Rut)
            {
                Items = new List<PedidoEcom>();
                Param = new object[] { Rut };
                ParamName = new string[] { "rut" };
                Select = new SelectRows("PLABAL", "e_Pedidos PE, e_TipoUsu TU, e_Usuario US", "TOP 20 PE.*,US.Nombre AS 'Usuario'", "TU.ID=US.Id_Tipo AND US.ID=pe.id_usuario and TU.Rut=@rut and PE.Estado<>'NUL'",Param,ParamName, "PE.fecha_hora_pedido DESC");
                GetValues();

            }

            public GetListOld(SelectRows DatosDelSelect)
            {
                Items = new List<PedidoEcom>();
                Select = DatosDelSelect;
                GetValues();
            }

            private void GetValues()
            {
                foreach (DataRow dr in Select.Datos.Rows)
                {
                    Validadores Val = new Validadores();
                    double NETO = Val.ParseoDouble(dr["totalPedido"].ToString());
                    double IVA = Math.Round(NETO*0.19,0);
                    double BRUTO = NETO+IVA;
                    PedidoEcom Pedido = new PedidoEcom
                    {
                        ID = dr["id_pedidos"].ToString(),
                        Nombre = dr["nom_pedido"].ToString(),
                        OrderType = "TER",
                        CanTotal = Convert.ToInt32(dr["Cantidad"].ToString()),
                        Neto = NETO,
                        Iva = IVA,
                        Bruto = BRUTO,
                        F_Entrega = Val.ParseoDateTime(dr["fecha_entrega"].ToString()),
                        F_Ingreso = Val.ParseoDateTime(dr["fecha_hora_pedido"].ToString()),
                        Estado = dr["Estado"].ToString(),
                        Observa = dr["observacion"].ToString(),
                        NroPedido = dr["PedidoAlfak"].ToString(),
                        UserId = dr["Usuario"].ToString(),
                        FileXLS = "OLD",
                        
                    };
                    Items.Add(Pedido);
                }


            }

        }

        public class UpdateMontos
        {
            public bool IsSuccess;
            public bool TieneMensaje;
            public string Mensaje;
            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            private UpdateRow update;
            private object[] Param;
            private string[] ParamName;
            private string tipoentrega;
            public PedidoEcom Pedido;
            private readonly GetInfoClienteEcomm ClienteEcom;
            private readonly bool ESTADO;
            public readonly nsCliente.DatosCliente datosCliente;
            

            public UpdateMontos(string ID,string TOKEN)
            {
                Get GetPedido = new Get(ID,TOKEN);
                if (GetPedido.IsSuccess)
                {
                    datosCliente = GetPedido.Cliente;
                    Pedido = GetPedido.Pedido;
                    ClienteEcom = GetPedido.InfoCliente;

                    ESTADO = true;
                    Doit();
                    Get getP = new Get(Pedido.ID);
                    Pedido = getP.Pedido;
                }
                else
                {
                    IsSuccess = false;
                    Mensaje = "No se pudo obtener el pedido, Token invalido.";
                }

            }

            public UpdateMontos(PedidoEcom _Pedido,bool _ESTADO, GetInfoClienteEcomm _ClienteEcom)
            {
                Pedido = _Pedido;
                ESTADO = _ESTADO;
                ClienteEcom = _ClienteEcom;

                Doit();

            }

            private void Doit()
            {
                FechasExpress.GetFechaCorte Fexpress = new FechasExpress.GetFechaCorte(Pedido.OrderType, Pedido.RUT);
                if (Pedido.F_Entrega <= DateTime.Today)
                {


                    UpdatePLABALRow pLABALRow = new UpdatePLABALRow("ECOM_PEDIDOS", "ID", Pedido.ID, "F_ENTREGA", Fexpress.FechaCorte);

                    /*Hacer cálculos*/
                    if (Pedido.EsDespacho)
                    {
                        PedDet.CalcDespUn calcDespUn = new PedDet.CalcDespUn(Pedido, ESTADO);

                        Param = new object[] { Pedido.ID, ESTADO };
                        ParamName = new string[] { "IDPEDIDO", "ESTADO" };
                        update = new UpdateRow("PLABAL", "ECOM_PEDDET", "CEXPRESSUN=0", "IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO", Param, ParamName);
                        tipoentrega = "el despacho";
                    }
                    else
                    {
                        Param = new object[] { Pedido.ID, ESTADO };
                        ParamName = new string[] { "IDPEDIDO", "ESTADO" };
                        update = new UpdateRow("PLABAL", "ECOM_PEDDET", "CEXPRESSUN=0,CDESPACHOUN=0", "IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO", Param, ParamName);
                        tipoentrega = "el retiro";

                    }

                    TieneMensaje = true;
                    Mensaje = "La fecha Ingresada anteriormente en el pedido expiró, por lo tanto se reprogramó " + tipoentrega + " para el " + Fexpress.FechaCorte.ToLongDateString();

                }
                else
                {

                    /*Hacer cálculos*/
                    if (Pedido.EsDespacho)
                    {
                        PedDet.CalcDespUn calcDespUn = new PedDet.CalcDespUn(Pedido, ESTADO);
                        tipoentrega = " el despacho";
                    }
                    else
                    {
                        Param = new object[] { Pedido.ID, ESTADO };
                        ParamName = new string[] { "IDPEDIDO", "ESTADO" };
                        update = new UpdateRow("PLABAL", "ECOM_PEDDET", "CDESPACHOUN=0", "IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO", Param, ParamName);
                        tipoentrega = "el retiro";

                    }

                    if (DateTime.Now.Hour > Constantes.HoraCorte && DateTime.Today.AddDays(1) == Pedido.F_Entrega)
                    {
                        UpdatePLABALRow pLABALRow = new UpdatePLABALRow("ECOM_PEDIDOS", "ID", Pedido.ID, "F_ENTREGA", Fexpress.FechaCorte);
                        TieneMensaje = true;
                        Mensaje = "La fecha Ingresada anteriormente en el pedido expiró, por lo tanto se reprogramó " + tipoentrega + " para el " + Fexpress.FechaCorte.ToLongDateString();


                    }
                    
                    PedDet.CalcExpressUn calcExpressUn = new PedDet.CalcExpressUn(Pedido.OrderType, Pedido.ID, ESTADO, Pedido.F_Entrega, ClienteEcom);



                }

                double[] Val = GetSum(Pedido.ID, ESTADO);
                double NETO = Val[3] + Val[4] + Val[5] + Val[6];
                double IVA = Math.Round(NETO * 0.19, 0);
                double BRUTO = Math.Round(NETO + IVA, 0);
                Param = new object[] { NETO, IVA, BRUTO, Val[1], Pedido.ID, Val[2], Val[3], Val[4], Val[5], Val[6] };
                ParamName = new string[] { "NETO", "IVA", "BRUTO", "CANTOTAL", "IDPEDIDO", "KGTOTAL", "CDIRECTO", "CDESPACHO", "BENECONOM", "CEXPRESS" };
                update = new UpdateRow("PLABAL", "ECOM_PEDIDOS", "NETO=@NETO,IVA=@IVA,BRUTO=@BRUTO, CANTOTAL=@CANTOTAL,KGTOTAL=@KGTOTAL,CDIRECTO=@CDIRECTO,CDESPACHO=@CDESPACHO,BENECONOM=@BENECONOM,CEXPRESS=@CEXPRESS",
                    "ID=@IDPEDIDO",
                    Param, ParamName);

                IsSuccess = update.Actualizado;
                

            }

            private double[] GetSum( string IDPEDIDO, bool ESTADO)
            {
                int TOT = 7;
                double[] Valores = new double[TOT];
                Conn = new Coneccion();
                Query = "SELECT SUM(NETOITM), SUM(CANT), SUM(KGITM), SUM((CMPUN+CMERMAUN+CPROCUN)*CANT), SUM(CDESPACHOUN*CANT), SUM(BENECONOMUN*CANT),SUM(CEXPRESSUN*CANT) FROM ECOM_PEDDET WHERE IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO";
                Conn.ConnPlabal.Open();
                Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IDPEDIDO);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);

                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    for (int i = 0; i < TOT; i++)
                    {
                        double a;
                        
                        if (double.TryParse(dr[i].ToString(),out a))
                        {
                            Valores[i] = Math.Round(a,0);
                        }
                           
                        
                        
                    }

                    return Valores;

                }
                else
                {
                    for (int i = 0; i < TOT; i++)
                    {
                        Valores[i] = 0;
                    }

                    return Valores;
                }

            }
        }

        public class AddItemDVH
        {
            public readonly bool IsSuccess;

            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            private string[] AlfakCode;
            private readonly int Cant;
            private readonly double Ancho;
            private readonly double Alto;
            private readonly PedidoEcom Pedido;
            private readonly string Terminologia;
            private readonly string Referencia;
            private readonly bool IsFromFile;
            private readonly GetInfoClienteEcomm Cliente;
            private PedDet item;
            private double _M2UN;
            private double _M2ITM;
            private double _PERIMUN;
            private double _PERIMITM;
            private PedDet.Firstentry DetInsert;


            #region Constructores
            /*Constructor ingreso normal*/
            public AddItemDVH(string _Referencia, string[] _AlfakCode, int _Cant, double _Ancho, double _Alto, PedidoEcom _Pedido)
            {
                Pedido = _Pedido;
                Cant = _Cant;
                Ancho = _Ancho;
                Alto = _Alto;
                AlfakCode = _AlfakCode;
                Referencia = _Referencia;
                /*Obtener datos del cliente*/
                Cliente = new GetInfoClienteEcomm(Pedido.RUT);
                GetM2nPerim();
                if (GetItemNormal())
                {
                   IsSuccess= UltimoVoid();

                }
                


            }

            


            /*Constructor desde archivo xls*/
            public AddItemDVH(bool _IsFromFile, string _Terminologia, string _Referencia, int _Cant, double _Ancho, double _Alto, PedidoEcom _Pedido)
            {
                Pedido = _Pedido;
                Cant = _Cant;
                Ancho = _Ancho;
                Alto = _Alto;
                Referencia = _Referencia;
                IsFromFile = _IsFromFile;
                Terminologia = _Terminologia;
                /*Obtener datos del cliente*/
                Cliente = new GetInfoClienteEcomm(Pedido.RUT);
                GetM2nPerim();
                if (GetItemFromFile())
                {
                    IsSuccess = UltimoVoid();
                }
                



            }

            #endregion

            private bool UltimoVoid()
            {
                item.IDPEDDET = DetInsert.IDPEDDET;
                item.NODO = DetInsert.NODO;
                PedSubDet.InsertSub insertSub = new PedSubDet.InsertSub(item, AlfakCode, Pedido, Cliente.Margen);
                return insertSub.Insertados;

            }

            private bool GetItemFromFile()
            {
                /*si viene de un archivo excel*/
                item = new PedDet
                {
                    IDPEDIDO = Pedido.ID,
                    REFERENCIA = Referencia,
                    ISFROMFILE = IsFromFile,
                    TERMINOLOGIA = Terminologia,
                    GOTFDICC = GetAlfakCode(Terminologia),
                    CANT = Cant,
                    ANCHO = Ancho,
                    ALTO = Alto,
                    CMPUN = 0,
                    CMERMAUN = 0,
                    CDESPACHOUN = 0,
                    CPROCUN = 0,
                    BENECONOMUN = 0,
                    NETOITM = 0,
                    NETOUN = 0,
                    M2UN = _M2UN,
                    M2ITM = _M2ITM,
                    PERIMUN = _PERIMUN,
                    PERIMITM = _PERIMITM,
                    ESTADO = true,
                    KGITM = 0,
                    KGUN = 0,
                    CEXPRESSUN=0,
                    ALFAKCODE="1000",
                    KOMODE="21DVH4",
                };

                DetInsert = new PedDet.Firstentry(item);

                if (!item.GOTFDICC)
                {
                    return false;
                }
                else
                {
                    
                    return DetInsert.IsSuccess;
                }

            }

            private bool GetItemNormal()
            {
                
                /*agregar item manual*/
                item = new PedDet
                {
                    IDPEDIDO = Pedido.ID,
                    REFERENCIA = Referencia,
                    TERMINOLOGIA="",
                    ISFROMFILE = false,
                    GOTFDICC = false,
                    CANT = Cant,
                    ANCHO = Ancho,
                    ALTO = Alto,
                    CMPUN = 0,
                    CMERMAUN = 0,
                    CDESPACHOUN = 0,
                    CPROCUN = 0,
                    BENECONOMUN = 0,
                    NETOITM = 0,
                    NETOUN = 0,
                    M2UN = _M2UN,
                    M2ITM = _M2ITM,
                    PERIMUN = _PERIMUN,
                    PERIMITM = _PERIMITM,
                    ESTADO = true,
                    KGITM = 0,
                    KGUN = 0,
                    CEXPRESSUN=0,
                    ALFAKCODE="1000",
                    KOMODE = "21DVH4",
                };
                DetInsert = new PedDet.Firstentry(item);

                return DetInsert.IsSuccess;

            }

            private void GetM2nPerim()
            {
                Getm2yPerim GM2Pe = new Getm2yPerim(Ancho, Alto);
                _M2UN = GM2Pe.M2;
                _M2ITM = GM2Pe.M2 * Cant;
                _PERIMUN = GM2Pe.Perimetro;
                _PERIMITM = GM2Pe.Perimetro * Cant;
            }

           



            

            private bool GetAlfakCode(string Termino)
            {
                PedSubDet.GetSubDiccionario subDiccionario = new PedSubDet.GetSubDiccionario(Termino);
                if (subDiccionario.Obtenido)
                {
                    AlfakCode = subDiccionario.AlfakCodes;
                    return true;
                }
                else
                {
                    return false;
                }

                
                
                

            }

          

            
        }

        public class Detalle
        {
            public int Index { get; set; }
            public string CodRandom { get; set; }
            public string Descripcion { get; set; }
            public string Referencia { get; set; }
            public int Cantidad { get; set; }
            public double Ancho { get; set; }
            public double Alto { get; set; }
            public double Neto { get; set; }
            public double NetoUn { get; set; }
            public double Iva { get; set; }
            public double Bruto { get; set; }
            public string NroPedido { get; set; }
            public bool Fabricado { get; set; }
            public bool En_Bodega { get; set; }
            public bool Entregado { get; set; }
            public string DocEntrega { get; set; }
            
        }
    }


    public class GetFeriados
    {
        public readonly List<DateTime> Feriados;

        public GetFeriados(DateTime FechaInicio)
        {
            Feriados = new List<DateTime>();
            object[] Param = new object[] { FechaInicio };
            string[] ParamName = new string[] {"Fecha" };
            SelectRows select = new SelectRows("PLABAL", "Feriados", "Fecha", "Fecha>@Fecha", Param, ParamName);
            if (select.IsGot)
            {
                foreach (DataRow dr in select.Datos.Rows)
                {
                    DateTime date = Convert.ToDateTime(dr["Fecha"].ToString());
                    Feriados.Add(date);
                }
            }
        }
    }


    public class GetDiaCorte
    {
        public int DiaCorte;
        public bool IsSuccess;

        public GetDiaCorte(string Tipo)
        {
            if (Tipo == "TER")
            {
                Obtener("N1");
            }
            else if (Tipo=="ARQ")
            {
                Obtener("F1");
            }
            else if (Tipo=="LAMINA")
            {
                Obtener("H1");
            }

        }

        private void Obtener(string columnas)
        {
            SelectRows select = new SelectRows("PLABAL", "DiasEntrega", "TOP 1 " + columnas, " ID DESC");
            if (select.IsGot)
            {

                DiaCorte = Convert.ToInt32(select.Datos.Rows[0][0].ToString());
                
                IsSuccess = true;
            }
            else
            {
                IsSuccess = false;
            }


        }
    }

    

    public class Misdirecciones
    {
        #region Variables Publicas
        public string ID { get; set; }
        public string UserId { get; set; }
        public string RUTCLI { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Cod_com { get; set; }
        public string Region { get; set; }
        public string Cod_Reg { get; set; }
        public bool Estado { get; set; }
        #endregion


        public class Get
        {
            public readonly List<Misdirecciones> Lista;
            public readonly bool IsGot;
            private readonly SelectRows Seleccionar;
            

            
            public Get()
            {
                Seleccionar = new SelectRows("PLABAL", "ECOM_MADIRCLI", "*");
                Lista = GetLista(Seleccionar.Datos);
                if (Lista.Count>0)
                {
                    IsGot = true;
                }
            }

            public Get(string RUTCLI, bool ESTADO)
            {
                object[] Param = new object[] { RUTCLI, ESTADO };
                string[] ParamName = new string[] { "RUTCLI","ESTADO" };
                Seleccionar = new SelectRows("PLABAL", "ECOM_MADIRCLI", "*","RUTCLI=@RUTCLI AND ESTADO=@ESTADO",Param,ParamName," REGION,COMUNA,DIRECCION ASC ");
                Lista= GetLista(Seleccionar.Datos);
                if (Lista.Count > 0)
                {
                    IsGot = true;
                }
            }

            private List<Misdirecciones> GetLista(DataTable dt)
            {
                List<Misdirecciones> misdirecciones = new List<Misdirecciones>();

                foreach (DataRow dr in dt.Rows)
                {
                    Misdirecciones dir = new Misdirecciones {
                        Comuna = dr["COMUNA"].ToString(),
                        ID= dr["ID"].ToString(),
                        UserId= dr["UserId"].ToString(),
                        RUTCLI = dr["RUTCLI"].ToString(),
                        Direccion= dr["DIRECCION"].ToString(),
                        Region = dr["REGION"].ToString(),
                        Estado = Convert.ToBoolean(dr["ESTADO"].ToString()),
                        Cod_com= dr["COD_COM"].ToString(),
                        Cod_Reg= dr["COD_REG"].ToString(),
                    };
                    misdirecciones.Add(dir);
                    
                }

                return misdirecciones;
            }
        }

    }


    public class Getm2yPerim
    {
        public readonly double M2;
        public readonly double Perimetro;
        public Getm2yPerim(double Ancho, double Alto)
        {
            M2 = Math.Round((Ancho / 1000) * (Alto / 1000),2);
            Perimetro = Math.Round(((Ancho / 1000) + (Alto / 1000))*2,2);
        }
    }

    public class GetFlete
    {
        public readonly double Costo;


        private int reg;
        private string AlfakCode;
        private readonly int Com;
        private double Vector;
        private readonly bool EsDesp;

        public GetFlete(double Kg, string Cod_Region, string Cod_Comuna, bool _EsDespacho)
        {
            EsDesp = _EsDespacho;
            GetAlfakCode(Cod_Region, Cod_Comuna);
            if (!string.IsNullOrEmpty(AlfakCode))
            {
                Costo = Math.Round(Flete() * Kg, 0);
            }

                
            
            
        }

        public GetFlete(string Cod_Region, string Cod_Comuna, bool _EsDespacho)
        {
            EsDesp = _EsDespacho;
            GetAlfakCode(Cod_Region, Cod_Comuna);
            if (!string.IsNullOrEmpty(AlfakCode))
            {
                Costo = Flete();
            }

        }

        private void GetAlfakCode( string CodReg, string CodCom)
        {
            reg = Convert.ToInt32(CodReg);
            if (reg < 5)
            {
                AlfakCode = "830";
                Vector = reg;
            }
            else if (reg == 13)
            {
                if (!EsDesp)
                {
                    /*Retirno en la region metropolitana (Default)*/
                    AlfakCode = "832";
                    Vector = 0;
                }
                else
                {
                    AlfakCode = "832";
                    DistPoliticaChile.GetComunas getProv = new DistPoliticaChile.GetComunas(CodReg, CodCom);
                    Vector = Convert.ToInt32(getProv.Comunas.Select(it => it.Codigo_padre).First());
                }


            }
            else if (reg > 4 && reg != 13)
            {
                if (reg == 5 && !EsDesp)
                {
                    /*Retiro en a 5ta region*/
                    AlfakCode = "833";
                    Vector = 0;

                }
                else if (reg == 5 && EsDesp)
                {
                    AlfakCode = "833";
                    DistPoliticaChile.GetComunas getProv = new DistPoliticaChile.GetComunas(CodReg, CodCom);
                    Vector = Convert.ToInt32(getProv.Comunas.Select(it => it.Codigo_padre).First());
                    if (Vector == 57)
                    {
                        Vector = 58;
                    }

                }
                else
                {
                    AlfakCode = "831";
                    Vector = reg;
                }

            }

        }

        private double Flete()
        {

            /*obtener flete por codigo de despacho ej: 200=>stgo, etc.... aplicar margen*/
            ProductoAlfak.Costo cflete = new ProductoAlfak.Costo(AlfakCode, "1", "1");
            var listaC = cflete.Lista.Where(it => it.GRENZE1 ==Vector).OrderBy(it => it.GRENZE1);
            return listaC.Select(it => it.PREIS).First();
        }
    }

    public class PedSubDet
    {
        #region VARIABLES PUBLICAS
        public string IDPEDSUBDET { get; set; }
        public string IDPEDDET { get; set; }
        public string DESCRIPCION { get; set; }
        public string IDPEDIDO { get; set; }
        public double CNETOUN { get; set; }
        public double KGUN { get; set; }
        public double CMERMAUN { get; set; }
        public double CPROCUN { get; set; }
        public double ESPESORUN { get; set; }
        public string ALFAKCODE { get; set; }
        public int POS_NR { get; set; }
        public int NODO { get; set; }
        public int NIVEL { get; set; }
        public string STL_PRODART { get; set; }
        public string STL_PRODGRP { get; set; }
        public string STL_WGR { get; set; }
        public bool ESTADO { get; set; }
        

        #endregion
        public PedSubDet()
        {

        }

        public class CalcCostos
        {
            public readonly List<PedSubDet> Subdets;
            private readonly PedDet item;

            public CalcCostos(string[] _AlfakCode, PedDet _item)
            {
                
                item = _item;
                Subdets = new List<PedSubDet>();
                foreach (var code in _AlfakCode)
                {
                    PedSubDet Sub = Calculo(code);
                    Subdets.Add(Sub);

                }

            }

            public CalcCostos(string _AlfakCode, PedDet _item)
            {
                item = _item;
                Subdets = new List<PedSubDet>();
                PedSubDet Sub = Calculo(_AlfakCode);
                Subdets.Add(Sub);

            }

            private PedSubDet Calculo(string AlfakCode)
            {
                
                    /*get variables de alfak*/
                    ProductoAlfak.Get Alfak = new ProductoAlfak.Get(AlfakCode);
                    double cnetoun = 0;
                    double cmermaun = 0;
                    double cprocun = 0;
                    double KG = 0;
                double Multiplo = 0;
                int nivel = 1;
                if (Alfak.BA_PRODUKTE.BA_MENGENEINH== "m²")
                {
                    Multiplo = item.M2UN;
                }
                else if (Alfak.BA_PRODUKTE.BA_MENGENEINH == "m lin.")
                {
                    Multiplo = item.PERIMUN;
                }
                    
                    if (Alfak.BA_PRODUKTE.ARTPRDNR == "1" || Alfak.BA_PRODUKTE.ARTPRDNR == "2" || Alfak.BA_PRODUKTE.ARTPRDNR == "3")
                    {
                        MermaVidrio getmerma = new MermaVidrio(Multiplo);
                    cnetoun = Alfak.Costos.Select(it => it.PREIS).First() * Multiplo;
                    cmermaun = cnetoun * getmerma.Merma;
                        
                    nivel = 1;
                    }

                    /*separador*/
                    if (Alfak.BA_PRODUKTE.ARTPRDNR == "60")
                    {

                        cnetoun = Alfak.Costos.Select(it => it.PREIS).First() * Multiplo;
                    nivel = 1;
                    }

                /*Procesos*/
                if (Alfak.BA_PRODUKTE.ARTPRDNR =="20" && Alfak.BA_PRODUKTE.PRDKTGRPNR=="6")
                {
                    
                    cprocun = Alfak.Costos.Select(it => it.PREIS).First() * Multiplo;
                    nivel = 2;

                }
                    KG = Alfak.BA_PRODUKTE.BA_MASS_GEWICHT * item.M2UN;
                    PedSubDet Sub = new PedSubDet
                    {
                        IDPEDDET = item.IDPEDDET,
                        IDPEDIDO = item.IDPEDIDO,
                        CNETOUN = Math.Round(cnetoun, 0),
                        CMERMAUN = Math.Round(cmermaun, 0),
                        KGUN = Math.Round(KG, 1),
                        CPROCUN = Math.Round(cprocun, 0),
                        ALFAKCODE = AlfakCode,
                        NODO = item.NODO,
                        NIVEL = nivel,
                        STL_PRODART = Alfak.BA_PRODUKTE.ARTPRDNR,
                        STL_PRODGRP = Alfak.BA_PRODUKTE.PRDKTGRPNR,
                        STL_WGR = Alfak.BA_PRODUKTE.Id_familiaAlfak,
                        ESTADO = true,
                        DESCRIPCION = Alfak.BA_PRODUKTE.Descripcion,
                        ESPESORUN=Alfak.BA_PRODUKTE.BA_MASS_DICKE,
                        

                    };

                 

                return Sub;
            }
        }

        public class InsertSub
        {

            public readonly bool Insertados;
            private readonly PedDet item;
            private readonly string[] AlfakCode;
            private readonly bool EsDespacho;
            private readonly double Margen;
            private readonly PedidoEcom Pedido;
            public InsertSub(PedDet _item, string[] _AlfakCode, PedidoEcom _Pedido, double _Margen)
            {
                Pedido = _Pedido;
                item = _item;
                AlfakCode = _AlfakCode;
                Insertados = Insertando();
                Margen = _Margen;
                Calcular();

            }

            private void Calcular()
            {
                if (Insertados)
                {
                    /*hacer calculos en ecom_peddet*/
                    PedDet.Calculos Calculos = new PedDet.Calculos(item,Pedido , Margen);
                    
                }
                
            }

            private bool Insertando()
            {
                bool IsSuccess = true;
                CalcCostos calcCostos = new CalcCostos(AlfakCode, item);

                foreach (PedSubDet Sub in calcCostos.Subdets)
                {
                    Firstentry SubEntry = new Firstentry(Sub);
                    if (!SubEntry.IsSuccess)
                    {
                        IsSuccess = false;
                    }

                }


                string[] Descrip = calcCostos.Subdets.Select(it => it.DESCRIPCION).ToArray();

                UpdatePLABALRow update = new UpdatePLABALRow("ECOM_PEDDET", "IDPEDDET", item.IDPEDDET, "DESCRIPCION", string.Join(" || ", Descrip));
                if (!update.Actualizado)
                {
                    IsSuccess = false;
                }

                return IsSuccess;

            }
        }

        public class GetSubDiccionario
        {
            public readonly bool Obtenido;
            public string[] AlfakCodes;
            public readonly string Termino;
            public GetSubDiccionario(string _Termino)
            {
                Termino = _Termino;
                Obtenido = Get();
            }

            private bool Get()
            {
                object[] Param = new object[] { Termino.Replace(" ", "") };
                string[] ParamN = new string[] { "TERMINOLOGIA" };
                SelectRows selectItem = new SelectRows("PLABAL", "ECOM_DICCIONARIO DI, ECOM_DICCSUB SU", "SU.ALFAKCODE", "DI.TERMINOLOGIA=@TERMINOLOGIA AND DI.IDDICC=SU.IDDICC", Param, ParamN);

                AlfakCodes = new string[selectItem.Datos.Rows.Count];
                for (int i = 0; i < selectItem.Datos.Rows.Count; i++)
                {
                    AlfakCodes[i] = selectItem.Datos.Rows[i][0].ToString();
                }

                if (AlfakCodes.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

       public class GetSubs
       {
            public List<PedSubDet> Items;
            public bool IsSuccess;

            private readonly string[] ParamName;
            private readonly object[] Param;
            private readonly string WHERE;

            public GetSubs(string IDPEDIDO, string IDPEDDET,bool ESTADO)
            {
                Param = new object[] {IDPEDIDO,IDPEDDET,ESTADO };
                ParamName = new string[] {"IDPEDIDO","IDPEDDET","ESTADO" };
                WHERE = "IDPEDIDO=@IDPEDIDO AND IDPEDDET=@IDPEDDET AND ESTADO=@ESTADO";
               IsSuccess= Select();
            }

            public GetSubs(string IDPEDIDO, string IDPEDDET, bool ESTADO,int NIVEL)
            {
                Param = new object[] { IDPEDIDO, IDPEDDET, ESTADO ,NIVEL};
                ParamName = new string[] { "IDPEDIDO", "IDPEDDET", "ESTADO","NIVEL" };
                WHERE = "IDPEDIDO=@IDPEDIDO AND IDPEDDET=@IDPEDDET AND ESTADO=@ESTADO AND NIVEL=@NIVEL";
                IsSuccess= Select();
            }
            private bool Select()
            {
                SelectRows select = new SelectRows("PLABAL","ECOM_PEDSUBDET"," * ",WHERE,Param,ParamName);
                if (select.IsGot)
                {
                    Items = new List<PedSubDet>();
                    foreach (DataRow dr in select.Datos.Rows)
                    {
                        Validadores Val = new Validadores();
                        PedSubDet subDet = new PedSubDet {
                            IDPEDSUBDET = dr["IDPEDSUBDET"].ToString(),
                            IDPEDIDO = dr["IDPEDIDO"].ToString(),
                            IDPEDDET = dr["IDPEDDET"].ToString(),
                            DESCRIPCION = dr["DESCRIPCION"].ToString(),
                            CNETOUN = Val.ParseoDouble(dr["CNETOUN"].ToString()),
                            CMERMAUN = Val.ParseoDouble(dr["CMERMAUN"].ToString()),
                            CPROCUN = Val.ParseoDouble(dr["CPROCUN"].ToString()),
                            KGUN = Val.ParseoDouble(dr["KGUN"].ToString()),
                            ESPESORUN = Val.ParseoDouble(dr["ESPESORUN"].ToString()),
                            ALFAKCODE = dr["ALFAKCODE"].ToString(),
                            POS_NR = Val.ParseoInt(dr["POS_NR"].ToString()),
                            NODO = Val.ParseoInt(dr["NODO"].ToString()),
                            NIVEL = Val.ParseoInt(dr["NIVEL"].ToString()),
                            STL_PRODART= dr["STL_PRODART"].ToString(),
                            STL_PRODGRP=dr["STL_PRODGRP"].ToString(),
                            STL_WGR=dr["STL_WGR"].ToString(),
                            ESTADO=Val.ParseoBoolean(dr["ESTADO"].ToString()),

                        };
                        Items.Add(subDet);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

       }

        public class Firstentry
        {
            public readonly bool IsSuccess;
            public readonly string IDPEDSUBDET;
            private int POS_NR;

            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            public Firstentry(PedSubDet item)
            {
                IDPEDSUBDET = ACCION(item);

                if (!string.IsNullOrEmpty(IDPEDSUBDET))
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }

            }

            private string ACCION(PedSubDet IT)
            {
                if (Insert(IT))
                {
                    /*aqui hay q ver*/
                    Conn = new Coneccion();
                    Query = "SELECT TOP 1 IDPEDSUBDET FROM ECOM_PEDSUBDET WHERE IDPEDDET=@IDPEDDET AND IDPEDIDO=@IDPEDIDO " +
                        "AND POS_NR=@POS_NR AND NODO=@NODO AND NIVEL=@NIVEL AND ALFAKCODE=@ALFAKCODE AND ESTADO=@ESTADO";
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@IDPEDDET", IT.IDPEDDET);
                    Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IT.IDPEDIDO);
                    Conn.Cmd.Parameters.AddWithValue("@POS_NR", POS_NR);
                    Conn.Cmd.Parameters.AddWithValue("@NODO", IT.NODO);
                    Conn.Cmd.Parameters.AddWithValue("@NIVEL", IT.NIVEL);
                    Conn.Cmd.Parameters.AddWithValue("@ALFAKCODE", IT.ALFAKCODE);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", IT.ESTADO);

                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        return dr[0].ToString();

                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }

            private bool Insert(PedSubDet IT)
            {
                POS_NR = GetPOS_NR(IT.IDPEDIDO, IT.IDPEDDET);
                Conn = new Coneccion();
                Query = "INSERT INTO ECOM_PEDSUBDET (IDPEDDET,IDPEDIDO,CNETOUN,CMERMAUN,CPROCUN,KGUN,ALFAKCODE,POS_NR,NODO,NIVEL,STL_PRODART,STL_PRODGRP,STL_WGR,ESTADO,DESCRIPCION,ESPESORUN)" +
                    " VALUES (@IDPEDDET,@IDPEDIDO,@CNETOUN,@CMERMAUN,@CPROCUN,@KGUN,@ALFAKCODE,@POS_NR,@NODO,@NIVEL,@STL_PRODART,@STL_PRODGRP,@STL_WGR,@ESTADO,@DESCRIPCION,@ESPESORUN)";
                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@IDPEDDET", IT.IDPEDDET);
                    Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IT.IDPEDIDO);
                    Conn.Cmd.Parameters.AddWithValue("@CNETOUN", IT.CNETOUN);
                    Conn.Cmd.Parameters.AddWithValue("@CMERMAUN", IT.CMERMAUN);
                    Conn.Cmd.Parameters.AddWithValue("@CPROCUN", IT.CPROCUN);
                    Conn.Cmd.Parameters.AddWithValue("@KGUN", IT.KGUN);
                    Conn.Cmd.Parameters.AddWithValue("@ALFAKCODE", IT.ALFAKCODE);
                    Conn.Cmd.Parameters.AddWithValue("@POS_NR", POS_NR);
                    Conn.Cmd.Parameters.AddWithValue("@NODO", IT.NODO);
                    Conn.Cmd.Parameters.AddWithValue("@NIVEL", IT.NIVEL);
                    Conn.Cmd.Parameters.AddWithValue("@STL_PRODART", IT.STL_PRODART);
                    Conn.Cmd.Parameters.AddWithValue("@STL_PRODGRP", IT.STL_PRODGRP);
                    Conn.Cmd.Parameters.AddWithValue("@STL_WGR", IT.STL_WGR);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", IT.ESTADO);
                    Conn.Cmd.Parameters.AddWithValue("@DESCRIPCION", IT.DESCRIPCION);
                    Conn.Cmd.Parameters.AddWithValue("@ESPESORUN", IT.ESPESORUN);

                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    return true;

                }
                catch
                {
                    return false;
                }

            }

            private int GetPOS_NR(string IDPEDIDO, string IDPEDDET)
            {
                Conn = new Coneccion();
                Query = "SELECT TOP 1 POS_NR FROM ECOM_PEDSUBDET WHERE  ESTADO=@ESTADO AND IDPEDIDO=@IDPEDIDO AND IDPEDDET=@IDPEDDET ORDER BY POS_NR DESC";
                Conn.ConnPlabal.Open();
                Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IDPEDIDO);
                Conn.Cmd.Parameters.AddWithValue("@IDPEDDET", IDPEDDET);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", true);



                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    return Convert.ToInt32(dr[0].ToString()) + 1;

                }
                else
                {
                    return 1;
                }

            }
        }
    }

    public class PedDet
    {
        #region VARIABLES PUBLICAS
        
        public string IDPEDDET { get; set; }
        public string IDPEDIDO { get; set; }
        public string REFERENCIA { get; set; }
        public bool ISFROMFILE { get; set; }
        public string TERMINOLOGIA { get; set; }
        public bool GOTFDICC { get; set; }
        public int NODO { get; set; }
        public int CANT { get; set; }
        public double ANCHO { get; set; }
        public double ALTO { get; set; }
        public double CMPUN { get; set; }
        public double CMERMAUN { get; set; }
        public double CPROCUN { get; set; }
        public double CDESPACHOUN { get; set; }
        public double BENECONOMUN { get; set; }
        public double NETOUN { get; set; }
        public double NETOITM { get; set; }
        public double M2UN { get; set; }
        public double M2ITM { get; set; }
        public double KGUN { get; set; }
        public double KGITM { get; set; }
        public double PERIMUN { get; set; }
        public double ESPESORUN { get; set; }
        public double PERIMITM { get; set; }
        public bool ESTADO { get; set; }
        public string TOKEN { get; set; }
        public string KOMODE { get; set; }
        public string DESCRIPCION { get; set; }
        public string ALFAKCODE { get; set; }
        public double CEXPRESSUN { get; set; }


        #endregion

        public PedDet()
        {

        }

        public class Firstentry
        {
            public readonly bool IsSuccess;
            public readonly string IDPEDDET;
            public int NODO;

            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            public Firstentry(PedDet item)
            {
                Random random = new Random();
                int TokenId = random.Next(10000, 99999999);
                Encriptacion encr = new Encriptacion(TokenId.ToString());
                IDPEDDET = ACCION(item,encr.TokenEncriptado);

                if (!string.IsNullOrEmpty(IDPEDDET))
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }

            }

            private string ACCION(PedDet IT, string TOKEN)
            {

                if (Insert(IT,TOKEN))
                {
                    Conn = new Coneccion();
                    Query = "SELECT TOP 1 IDPEDDET FROM ECOM_PEDDET WHERE IDPEDIDO=@IDPEDIDO AND REFERENCIA=@REFERENCIA AND" +
                        " CANT=@CANT AND ANCHO=@ANCHO AND ALTO=@ALTO AND ESTADO=@ESTADO AND TOKEN=@TOKEN";
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IT.IDPEDIDO);
                    Conn.Cmd.Parameters.AddWithValue("@REFERENCIA", IT.REFERENCIA);
                    Conn.Cmd.Parameters.AddWithValue("@CANT", IT.CANT);
                    Conn.Cmd.Parameters.AddWithValue("@ANCHO", IT.ANCHO);
                    Conn.Cmd.Parameters.AddWithValue("@ALTO", IT.ALTO);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", IT.ESTADO);
                    Conn.Cmd.Parameters.AddWithValue("@TOKEN", TOKEN);


                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        return dr[0].ToString();

                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }


            private int GetNODO(string IDPEDIDO)
            {
                Conn = new Coneccion();
                Query = "SELECT TOP 1 NODO FROM ECOM_PEDDET WHERE  ESTADO=@ESTADO AND IDPEDIDO=@IDPEDIDO ORDER BY NODO DESC";
                Conn.ConnPlabal.Open();
                Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IDPEDIDO);
                
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", true);
                


                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    return Convert.ToInt32(dr[0].ToString()) + 1;

                }
                else
                {
                    return 1;
                }

            }
            private bool Insert(PedDet IT, string TOKEN)
            {
                NODO = GetNODO(IT.IDPEDIDO );
                Conn = new Coneccion();
                object[] Param = new object[] { IT.IDPEDIDO, IT.REFERENCIA, IT.ISFROMFILE, IT.GOTFDICC, IT.CANT, NODO, IT.ANCHO,
                    IT.ALTO, IT.CMPUN, IT.CMERMAUN, IT.CPROCUN, IT.CDESPACHOUN,IT.BENECONOMUN, IT.NETOUN, IT.NETOITM, IT.M2UN, IT.M2ITM, IT.KGUN,
                    IT.KGITM, IT.PERIMUN, IT.PERIMITM, IT.ESTADO, TOKEN, IT.TERMINOLOGIA,IT.CEXPRESSUN, IT.ALFAKCODE,IT.KOMODE };
                string[] ParamName = new string[] { "IDPEDIDO", "REFERENCIA", "ISFROMFILE", "GOTFDICC", "CANT", "NODO", "ANCHO",
                    "ALTO", "CMPUN", "CMERMAUN", "CPROCUN", "CDESPACHOUN", "BENECONOMUN", "NETOUN", "NETOITM", "M2UN", "M2ITM", "KGUN",
                    "KGITM", "PERIMUN", "PERIMITM", "ESTADO", "TOKEN", "TERMINOLOGIA","CEXPRESSUN","ALFAKCODE","KOMODE" };
                InsertRow insertRow = new InsertRow("PLABAL","ECOM_PEDDET", "IDPEDIDO,REFERENCIA,ISFROMFILE,GOTFDICC,CANT,NODO,ANCHO,ALTO,CMPUN,CMERMAUN,CPROCUN,CDESPACHOUN,BENECONOMUN,NETOUN, NETOITM,M2UN,M2ITM,KGUN,KGITM,PERIMUN,PERIMITM,ESTADO,TOKEN,TERMINOLOGIA,CEXPRESSUN,ALFAKCODE,KOMODE",
                    "@IDPEDIDO,@REFERENCIA,@ISFROMFILE,@GOTFDICC,@CANT,@NODO,@ANCHO,@ALTO,@CMPUN,@CMERMAUN,@CPROCUN,@CDESPACHOUN,@BENECONOMUN,@NETOUN, @NETOITM,@M2UN,@M2ITM,@KGUN,@KGITM,@PERIMUN,@PERIMITM,@ESTADO,@TOKEN,@TERMINOLOGIA,@CEXPRESSUN,@ALFAKCODE,@KOMODE",
                    Param,ParamName);

                return insertRow.Insertado;
               

            }
        }

        public class Calculos
        {
            public readonly bool IsSuccess;
            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion
            public Calculos(PedDet Det, PedidoEcom Pedido, double Margen)
            {
                double[] Valores = GetSum(Det.IDPEDDET, Det.IDPEDIDO,Det.ESTADO);
                if (Valores.Sum() > 0)
                {

                    UpdateRow Update;
                    string[] Paramname;
                    object[] Param;
                    string Set;
                    GetFlete FleteUn = new GetFlete(Valores[3],Pedido.COD_REGION,Pedido.COD_COMUNA, Pedido.EsDespacho);
                    
                    double Costos = Valores[0] + Valores[1] + Valores[2] + Det.CEXPRESSUN;

                    if (Pedido.EsDespacho)
                    {
                        
                        Paramname = new string[] { "CMPUN", "CMERMAUN", "CPROCUN", "KGUN", "IDPEDDET","CDESPACHOUN","KGITM","ESPESORUN" };
                        Param = new object[] { Valores[0], Valores[1], Valores[2], Valores[3], Det.IDPEDDET, FleteUn.Costo,Valores[3]*Det.CANT,Valores[4] };
                        Set = "CMPUN=@CMPUN,CMERMAUN=@CMERMAUN,CPROCUN=@CPROCUN,KGUN=@KGUN,CDESPACHOUN=@CDESPACHOUN,KGITM=@KGITM, ESPESORUN=@ESPESORUN";
                        
                    }
                    else
                    {
                        Paramname = new string[] { "CMPUN", "CMERMAUN", "CPROCUN", "KGUN", "IDPEDDET","KGITM","ESPESORUN" };
                        Param = new object[] { Valores[0], Valores[1], Valores[2], Valores[3], Det.IDPEDDET, Valores[3]*Det.CANT,Valores[4] };
                        Set = "CMPUN=@CMPUN,CMERMAUN=@CMERMAUN,CPROCUN=@CPROCUN,KGUN=@KGUN,KGITM=@KGITM,ESPESORUN=@ESPESORUN";
                    }

                    Update = new UpdateRow("PLABAL", "ECOM_PEDDET", Set, "IDPEDDET=@IDPEDDET", Param, Paramname);
                    
                    if (Margen > 0)
                    {
                       
                        GetMargen margen = new GetMargen(Margen, Costos);

                        double NetoUn = margen.Precio + FleteUn.Costo;
                        double NETOitem = NetoUn * Det.CANT;
                        Paramname = new string[] {"IDPEDDET","BENECONOMUN","NETOUN","NETOITM" };
                        Param = new object[] { Det.IDPEDDET,margen.BenEcon,NetoUn, NETOitem};
                        Set = "BENECONOMUN=@BENECONOMUN, NETOUN=@NETOUN, NETOITM=@NETOITM";
                        Update = new UpdateRow("PLABAL", "ECOM_PEDDET", Set, "IDPEDDET=@IDPEDDET", Param, Paramname);
                        if (Update.Actualizado)
                        {
                            
                        }

                        
                    }
                    else
                    {
                        Paramname = new string[] { "IDPEDDET", "NETOUN", "NETOITM" };
                        Param = new object[] { Det.IDPEDDET, Costos, Costos * Det.CANT };
                        Set = " NETOUN=@NETOUN, NETOITM=@NETOITM";
                        Update = new UpdateRow("PLABAL", "ECOM_PEDDET", Set, "IDPEDDET=@IDPEDDET", Param, Paramname);
                        if (Update.Actualizado)
                        {
                            
                        }

                    }


                    IsSuccess = Update.Actualizado;



                }

            }

            

            private double[] GetSum(string IDPEDDET, string IDPEDIDO,bool ESTADO)
            {
                double[] Valores = new double[5];
                Conn = new Coneccion();
                Query = "SELECT SUM(CNETOUN),SUM(CMERMAUN),SUM(CPROCUN),SUM(KGUN),SUM(ESPESORUN) FROM ECOM_PEDSUBDET WHERE IDPEDDET=@IDPEDDET AND IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO";
                Conn.ConnPlabal.Open();
                Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@IDPEDDET", IDPEDDET);
                Conn.Cmd.Parameters.AddWithValue("@IDPEDIDO", IDPEDIDO);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);

                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i==0 || i==1 || i==2)
                        {
                            Valores[i] = Math.Round(Convert.ToDouble(dr[i].ToString()),0);
                        }
                        else
                        {
                            Valores[i] = Convert.ToDouble(dr[i].ToString());
                        }
                        
                    }
                    
                    return Valores;

                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Valores[i] = 0;
                    }

                    return Valores;
                }

            }

        }

        public class Get
        {
            public readonly List<PedDet> Items;
            public readonly bool GotItems;
            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            private readonly bool ESTADO;
            private readonly string IDPEDIDO;

            public Get(string _IDPEDIDO, bool _ESTADO)
            {
                ESTADO = _ESTADO;
                IDPEDIDO = _IDPEDIDO;
                Items = Getlista();
                if (Items.Count>0)
                {
                    GotItems = true;
                }

            }

            private List<PedDet> Getlista()
            {
                List<PedDet> Lista = new List<PedDet>();
                Conn = new Coneccion();



                string consulta = "SELECT * FROM ECOM_PEDDET WHERE IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO ORDER BY NODO ASC";


                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                        adaptador.SelectCommand.Parameters.AddWithValue("@IDPEDIDO", IDPEDIDO);
                        

                        adaptador.Fill(TablaDetalle);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {

                    foreach (DataRow drR in TablaDetalle.Rows)
                    {
                        Validadores val = new Validadores();
                        PedDet det = new PedDet {
                            ALTO= val.ParseoDouble(drR["ALTO"].ToString()),
                            ANCHO = val.ParseoDouble(drR["ANCHO"].ToString()),
                            CANT = Convert.ToInt32(drR["CANT"].ToString()),
                            NODO = Convert.ToInt32(drR["NODO"].ToString()),
                            CMPUN = val.ParseoDouble(drR["CMPUN"].ToString()),
                            CMERMAUN = val.ParseoDouble(drR["CMERMAUN"].ToString()),
                            CPROCUN= val.ParseoDouble(drR["CPROCUN"].ToString()),
                            CDESPACHOUN= val.ParseoDouble(drR["CDESPACHOUN"].ToString()),
                            BENECONOMUN = val.ParseoDouble(drR["BENECONOMUN"].ToString()),
                            NETOUN= val.ParseoDouble(drR["NETOUN"].ToString()),
                            NETOITM  = val.ParseoDouble(drR["NETOITM"].ToString()),
                            M2UN = val.ParseoDouble(drR["M2UN"].ToString()),
                            M2ITM = val.ParseoDouble(drR["M2ITM"].ToString()),
                            KGUN = val.ParseoDouble(drR["KGUN"].ToString()),
                            KGITM = val.ParseoDouble(drR["KGITM"].ToString()),
                            PERIMUN= val.ParseoDouble(drR["PERIMUN"].ToString()),
                            PERIMITM= val.ParseoDouble(drR["PERIMITM"].ToString()),
                            ESTADO = val.ParseoBoolean(drR["ESTADO"].ToString()),
                            TOKEN = drR["TOKEN"].ToString(),
                            GOTFDICC = val.ParseoBoolean(drR["GOTFDICC"].ToString()),
                            IDPEDDET= drR["IDPEDDET"].ToString(),
                            IDPEDIDO = drR["IDPEDIDO"].ToString(),
                            ISFROMFILE= val.ParseoBoolean(drR["ISFROMFILE"].ToString()),
                            REFERENCIA = drR["REFERENCIA"].ToString(),
                            DESCRIPCION= drR["DESCRIPCION"].ToString(),
                            TERMINOLOGIA = drR["TERMINOLOGIA"].ToString(),
                            CEXPRESSUN= val.ParseoDouble(drR["CEXPRESSUN"].ToString()),
                            ALFAKCODE=drR["ALFAKCODE"].ToString(),
                            ESPESORUN= val.ParseoDouble(drR["ESPESORUN"].ToString()),
                            KOMODE=drR["KOMODE"].ToString(),

                        };

                        Lista.Add(det);

                    }
                }

                return Lista;
            }
        }

        public class GetItem
        {
            public readonly PedDet Item;
            public readonly bool IsGot;
            
            public GetItem(string IDPEDIDO, string IDPEDDET, bool ESTADO)
            {
                object[] param = new object[] { IDPEDIDO,IDPEDDET,ESTADO };
                string[] paramN = new string[] { "IDPEDIDO", "IDPEDDET", "ESTADO" };

                SelectRows select = new SelectRows("PLABAL", "ECOM_PEDDET", "*", "IDPEDIDO=@IDPEDIDO AND IDPEDDET=@IDPEDDET AND ESTADO=@ESTADO", param, paramN);
                if (select.IsGot)
                {
                    IsGot = true;
                    foreach (DataRow drR in select.Datos.Rows)
                    {
                        Validadores val = new Validadores();
                        Item = new PedDet {
                            ALTO = val.ParseoDouble(drR["ALTO"].ToString()),
                            ANCHO = val.ParseoDouble(drR["ANCHO"].ToString()),
                            CANT = Convert.ToInt32(drR["CANT"].ToString()),
                            NODO = Convert.ToInt32(drR["NODO"].ToString()),
                            CMPUN = val.ParseoDouble(drR["CMPUN"].ToString()),
                            CMERMAUN = val.ParseoDouble(drR["CMERMAUN"].ToString()),
                            CPROCUN = val.ParseoDouble(drR["CPROCUN"].ToString()),
                            CDESPACHOUN = val.ParseoDouble(drR["CDESPACHOUN"].ToString()),
                            BENECONOMUN = val.ParseoDouble(drR["BENECONOMUN"].ToString()),
                            NETOUN = val.ParseoDouble(drR["NETOUN"].ToString()),
                            NETOITM = val.ParseoDouble(drR["NETOITM"].ToString()),
                            M2UN = val.ParseoDouble(drR["M2UN"].ToString()),
                            M2ITM = val.ParseoDouble(drR["M2ITM"].ToString()),
                            KGUN = val.ParseoDouble(drR["KGUN"].ToString()),
                            KGITM = val.ParseoDouble(drR["KGITM"].ToString()),
                            PERIMUN = val.ParseoDouble(drR["PERIMUN"].ToString()),
                            PERIMITM = val.ParseoDouble(drR["PERIMITM"].ToString()),
                            ESTADO = val.ParseoBoolean(drR["ESTADO"].ToString()),
                            TOKEN = drR["TOKEN"].ToString(),
                            GOTFDICC = val.ParseoBoolean(drR["GOTFDICC"].ToString()),
                            IDPEDDET = drR["IDPEDDET"].ToString(),
                            IDPEDIDO = drR["IDPEDIDO"].ToString(),
                            ISFROMFILE = val.ParseoBoolean(drR["ISFROMFILE"].ToString()),
                            REFERENCIA = drR["REFERENCIA"].ToString(),
                            DESCRIPCION = drR["DESCRIPCION"].ToString(),
                            TERMINOLOGIA=drR["TERMINOLOGIA"].ToString(),
                            CEXPRESSUN= val.ParseoDouble(drR["CEXPRESSUN"].ToString()),
                            ALFAKCODE=drR["ALFAKCODE"].ToString(),
                            ESPESORUN= val.ParseoDouble(drR["ESPESORUN"].ToString()),
                            KOMODE=drR["KOMODE"].ToString(),

                        };
                    }

                }
            }

            
        }

        public class Itemizar
        {
            public Itemizar(string IDPEDIDO)
            {
                Get items = new Get(IDPEDIDO, true);
                int NODO = 1;
                foreach (PedDet item in items.Items)
                {
                    UpdatePLABALRow update = new UpdatePLABALRow("ECOM_PEDDET","IDPEDDET",item.IDPEDDET,"NODO",NODO);
                    if (update.Actualizado)
                    {
                        NODO++;
                    }
                    
                }
            }
        }

        public class DeleteItem
        {
            public readonly bool IsDeleted;

            public DeleteItem(string IDPEDIDO, string IDPEDDET)
            {
                UpdatePLABALRow update;
                update= new UpdatePLABALRow("ECOM_PEDDET", "IDPEDDET", IDPEDDET, "ESTADO", false);
                if (update.Actualizado)
                {
                    object[] Obj = new object[] { IDPEDDET,IDPEDIDO,false };
                    string[] ParamN = new string[] {"IDPEDDET","IDPEDIDO","ESTADO" };
                    UpdateRow updateSub = new UpdateRow("PLABAL", "ECOM_PEDSUBDET", "ESTADO=@ESTADO", "IDPEDIDO=IDPEDIDO AND IDPEDDET=@IDPEDDET",Obj,ParamN);

                    if (updateSub.Actualizado)
                    {
                        Itemizar itemizar = new Itemizar(IDPEDIDO);
                        IsDeleted = true;
                    }
                }


            }
        }

        public class AddDiccionario
        {
            public readonly bool Agregado;
            private readonly string Termino;
            private readonly string UserName;
            private readonly string RutCli;
            private readonly string[] Codigos;
            private readonly string IDDICC;
                
            public AddDiccionario(string _Terminologia, string _UserName, string _RutCli,string[] _AlfakCodes)
            {
                Termino = _Terminologia;
                UserName = _UserName;
                RutCli = _RutCli;
                Codigos = _AlfakCodes;
                IDDICC = Addtermino();
                if (!string.IsNullOrEmpty(IDDICC))
                {
                    int x = AddDiccSub().Where(it => !it).Count();
                    if (x>0)
                    {
                        Agregado = false;
                    }
                    else
                    {
                        Agregado = true;
                    }
                }
                
            }

            private string Addtermino()
            {
                /*GetUserId*/
                MembershipUser Miembro = Membership.GetUser(UserName);
                var UserId = (Guid)Miembro.ProviderUserKey;
                object[] Param = new object[] { Termino.Replace(" ",""),UserId,RutCli,true };
                string[] ParamName = new string[] {"TERMINOLOGIA","UserId","RUTCLI","ESTADO" };
                InsertRow insert = new InsertRow("PLABAL", "ECOM_DICCIONARIO", "TERMINOLOGIA,UserId,RUTCLI,F_INGRESO,ESTADO", "@TERMINOLOGIA,@UserId,@RUTCLI,GETDATE(),@ESTADO",Param,ParamName);

                if (insert.Insertado)
                {
                    SelectRows select = new SelectRows("PLABAL","ECOM_DICCIONARIO", "TOP 1 IDDICC", "TERMINOLOGIA=@TERMINOLOGIA AND UserId=UserId AND RUTCLI=@RUTCLI AND ESTADO=@ESTADO", Param, ParamName);
                    if (select.IsGot)
                    {
                        return select.Datos.Rows[0][0].ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            private bool[] AddDiccSub()
            {
                bool[] insertado = new bool[Codigos.Count()];
                int y = 0;
                foreach (var codigo in Codigos)
                {
                    object[] Param = new object[] { IDDICC,codigo };
                    string[] ParamName = new string[] { "IDDICC", "ALFAKCODE" };
                    InsertRow insert = new InsertRow("PLABAL","ECOM_DICCSUB","IDDICC,ALFAKCODE","@IDDICC,@ALFAKCODE",Param,ParamName);
                    insertado[y] = insert.Insertado;
                    y++;

                }

                return insertado;
            }
        }

        public class CheckDiccionario
        {
            public readonly bool HayCambios;
            public CheckDiccionario(List<PedDet> items, PedidoEcom pedido, double _Margen)
            {

                foreach (PedDet item in items)
                {
                    if (item.ISFROMFILE && !item.GOTFDICC)
                    {
                        PedSubDet.GetSubDiccionario subDiccionario = new PedSubDet.GetSubDiccionario(item.TERMINOLOGIA);
                        if (subDiccionario.Obtenido)
                        {
                            PedSubDet.InsertSub insertSub = new PedSubDet.InsertSub(item, subDiccionario.AlfakCodes, pedido, _Margen);
                            if (insertSub.Insertados)
                            {
                                
                                    UpdatePLABALRow updatePLABALRow = new UpdatePLABALRow("ECOM_PEDDET", "IDPEDDET", item.IDPEDDET, "GOTFDICC", true);
                                
                                HayCambios = true;

                            }
                        }
                    }
                }
            }
        }

        public class CalcExpressUn
        {
            private readonly CostExpressUn CExpressUn;
            private readonly FechasExpress.Get FExpress;
            
            public CalcExpressUn(string Tipo, string IDPEDIDO,bool ESTADO, DateTime Fecha, GetInfoClienteEcomm ClienteEcom)
            {
                TimeSpan Tspan = Fecha.Subtract(DateTime.Today);
                Get get = new Get(IDPEDIDO, ESTADO);
                FExpress = new FechasExpress.Get(Tipo, ClienteEcom.DiasDeTrato,Tspan.Days);
                foreach (PedDet item in get.Items)
                {
                    CExpressUn = new CostExpressUn(FExpress.Items,item,Fecha);
                    double costos = item.CMERMAUN + item.CMPUN + item.CPROCUN ;
                    GetMargen Margen = new GetMargen(ClienteEcom.Margen, costos + CExpressUn.Costo);

                    double Netoun = Margen.Precio + item.CDESPACHOUN;

                    object[] Param = new object[] { CExpressUn.Costo, Netoun, Netoun*item.CANT,item.IDPEDDET, Margen.BenEcon };
                    string[] ParamName = new string[] {"CEXPRESSUN","NETOUN","NETOITM","IDPEDDET","BENECONOMUN" };
                    UpdateRow updateRow = new UpdateRow("PLABAL", "ECOM_PEDDET",
                        "CEXPRESSUN=@CEXPRESSUN,NETOUN=@NETOUN,NETOITM=@NETOITM, BENECONOMUN=@BENECONOMUN",
                        "IDPEDDET=@IDPEDDET",Param,ParamName);
                    bool upd = updateRow.Actualizado;
                }

            }

            
        }

        public class CalcDespUn
        {
            private readonly PedidoEcom _Pedido;
            private readonly GetInfoClienteEcomm Cliente;
            private readonly double CFlete;
            public CalcDespUn(PedidoEcom Pedido, bool ESTADO)
            {
                _Pedido = Pedido;
                Cliente = new GetInfoClienteEcomm(Pedido.RUT);
                Get get = new Get(Pedido.ID, ESTADO);
                GetFlete getFlete = new GetFlete(_Pedido.COD_REGION, _Pedido.COD_COMUNA, _Pedido.EsDespacho);
                CFlete = getFlete.Costo;
                foreach (PedDet _item in get.Items)
                {
                    Accion(_item);
                }
            }

            public CalcDespUn(PedDet _item)
            {
                PedidoEcom.Get getP = new PedidoEcom.Get(_item.IDPEDIDO);
                _Pedido = getP.Pedido;
                Cliente = getP.InfoCliente;
                
                GetFlete getFlete = new GetFlete(_Pedido.COD_REGION, _Pedido.COD_COMUNA, _Pedido.EsDespacho);
                CFlete = getFlete.Costo;
                Accion(_item);
            }

            private void Accion(PedDet item)
            {
                
                double CDespUn = Math.Round(CFlete*item.KGUN,0);
                double costos = item.CMERMAUN + item.CMPUN + item.CPROCUN + item.CEXPRESSUN;
                GetMargen Margen = new GetMargen(Cliente.Margen,costos);
                double Netoun = Margen.Precio + CDespUn;
                object[] Param = new object[] { CDespUn, Netoun, Netoun * item.CANT, item.IDPEDDET,Margen.BenEcon };
                string[] ParamName = new string[] { "CDESPACHOUN", "NETOUN", "NETOITM", "IDPEDDET","BENECONOMUN" };
                UpdateRow updateRow = new UpdateRow("PLABAL", "ECOM_PEDDET",
                    "CDESPACHOUN=@CDESPACHOUN,NETOUN=@NETOUN,NETOITM=@NETOITM, BENECONOMUN=@BENECONOMUN",
                    "IDPEDDET=@IDPEDDET", Param, ParamName);
                bool upd = updateRow.Actualizado;

            }


        }




    }

    public class GetInfoClienteEcomm
    {
        public readonly double Margen;
        public readonly int TipoXlsDvh;
        public readonly bool EsPrecioXm2;
        public readonly bool Estado;
        public readonly bool TratoXDias;
        public readonly int DiasDeTrato;


        #region Manejo de datos
        Coneccion Conn;
        private readonly string Query;
        private SqlDataReader dr;
        #endregion

        public GetInfoClienteEcomm(string RUT)
        {
            Conn = new Coneccion();

            Query = "SELECT * FROM ECOM_MAEMP WHERE RUT=@RUT";

            Conn.ConnPlabal.Open();
            Conn.Cmd = new SqlCommand(Query, Conn.ConnPlabal);
            Conn.Cmd.Parameters.AddWithValue("@RUT", RUT);

            dr = Conn.Cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Validadores Val = new Validadores();
                Margen = Val.ParseoDouble(dr["MARGEN"].ToString());
                TipoXlsDvh = Convert.ToInt32(dr["TIPOXLSDVH"].ToString());
                EsPrecioXm2 = Val.ParseoBoolean(dr["ESPRECIOXM2"].ToString());
                Estado = Val.ParseoBoolean(dr["ESTADO"].ToString());
                int R = Val.ParseoInt(dr["TRATODIASENTREGA"].ToString());

                if (R>0)
                {
                    TratoXDias = true;
                }
            }
            

        }


    }

    public class Pedidotemporal
    {
        public bool IsTrue { get; set; }
        public T_temp_pedidos Datos { get; set; }
        public bool HasDetail { get; set; }
        public List<T_temp_DetPedido> Detalle;

        Coneccion Coneccion;
        static SqlDataReader dr;

        public Pedidotemporal(string Id, string Token)
        {
            T_temp_pedidos t_Temp;
            string Select = @"Select * FROM e_temp_pedidos WHERE id=@Id AND Token=@Token";

            Coneccion = new Coneccion();

            Coneccion.ConnPlabal.Open();
            Coneccion.CmdPlabal = new SqlCommand(Select, Coneccion.ConnPlabal);
            Coneccion.CmdPlabal.Parameters.AddWithValue("@Id", Id);
            Coneccion.CmdPlabal.Parameters.AddWithValue("@Token", Token);
            dr = Coneccion.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                IsTrue = true;

                t_Temp = new T_temp_pedidos
                {
                    _Id = dr["Id"].ToString(),
                    _UserId = dr["UserId"].ToString(),
                    _Id_cliente = dr["id_cliente"].ToString(),
                    _OrderName = dr["nombre"].ToString(),
                    _Fecha = Convert.ToDateTime(dr["fecha"].ToString()),
                    _TipoDespacho = dr["tipo_despacho"].ToString(),
                    _Observacion = dr["observacion"].ToString(),
                    _Estado = dr["Estado"].ToString(),
                    _PedidoAlfak = dr["PedidoAlfak"].ToString(),
                    _Rutadj = dr["Rutadj"].ToString(),
                    _DireccionDesp = dr["DireccionEntrega"].ToString(),
                    _Token = dr["Token"].ToString(),

                };
                if (!string.IsNullOrEmpty(dr["fechaentrega"].ToString()))
                {
                    t_Temp._FechaDesp = Convert.ToDateTime(dr["fechaentrega"].ToString());
                }
                else
                {
                    t_Temp._FechaDesp = null;
                }

            }
            else
            {
                IsTrue = false;
                t_Temp = null;
            }

            HasDetail = GetDetail(Id);
            Datos = t_Temp;
            dr.Close();
            Coneccion.ConnPlabal.Close();



        }


        private bool GetDetail(string Id)
        {
            bool Isornot = false;

            Detalle = new List<T_temp_DetPedido>();

            #region SQLString
            string Select = "SELECT * FROM e_temp_det_pedido WHERE id_temp_pedido=@Id";
            #endregion

            DataTable TablaDetalle2 = new DataTable();
            Coneccion conect = new Coneccion();
            using (conect.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(Select, conect.ConnPlabal);
                    adaptador2.SelectCommand.Parameters.AddWithValue("@Id", Id);

                    adaptador2.Fill(TablaDetalle2);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message, ex);
                }

                if (TablaDetalle2.Rows.Count > 0)
                {
                    Isornot = true;
                    T_temp_DetPedido item;
                    foreach (DataRow dr in TablaDetalle2.Rows)
                    {

                        item = new T_temp_DetPedido
                        {
                            Id = dr["id"].ToString(),
                            _Referencia = dr["_Referencia"].ToString(),
                            _Terminologia = dr["_Terminologia"].ToString(),
                            _Cantidad = Convert.ToInt32(dr["_Cantidad"].ToString()),
                            _Ancho = Convert.ToDouble(dr["_Ancho"].ToString()),
                            _Alto = Convert.ToDouble(dr["_Alto"].ToString()),
                            _CodigoAlfak = dr["_CodigoAlfak"].ToString(),
                            _ExisteCode = Convert.ToBoolean(dr["_ExisteCode"].ToString()),
                            Neto = Convert.ToDouble(dr["Neto"].ToString()),
                            Iva = Convert.ToDouble(dr["Iva"].ToString()),
                            Total = Convert.ToDouble(dr["Total"].ToString()),
                            M2Item = Convert.ToDouble(dr["M2Item"].ToString()),
                            KilosItem = Convert.ToDouble(dr["KilosItem"].ToString()),
                            PerimetroItem = Convert.ToDouble(dr["PerimetroItem"].ToString()),
                            M2Unit = Convert.ToDouble(dr["M2Unit"].ToString()),
                            KilosUnit = Convert.ToDouble(dr["KilosUnit"].ToString()),
                            PermietroUnit = Convert.ToDouble(dr["PerimetroUnit"].ToString()),


                        };
                        Detalle.Add(item);

                    }


                }
                else
                {
                    Isornot = false;
                }
            }



            return Isornot;
        }


        public bool Insert_temp_det_pedido(List<T_temp_DetPedido> Detalle, string Id_temp_pedido)
        {
            bool valid = true;

            if (IsTrue)
            {
                foreach (var item in Detalle)
                {
                    #region InsertString
                    string Insert = "Insert INTO e_temp_det_pedido  VALUES (@id_temp_pedido,@referencia,@termino,@cant,@ancho,@alto,@codigoalfak,@existecod,@neto,@iva,@total,@m2item,@kgitem,@perimetroitem" +
                        ",@m2unit,@kgunit,@perimetrounit)";
                    #endregion
                    Coneccion conec = new Coneccion();
                    try
                    {


                        conec.ConnPlabal.Open();
                        conec.CmdPlabal = new SqlCommand(Insert, conec.ConnPlabal);

                        conec.CmdPlabal.Parameters.AddWithValue("@id_temp_pedido", Id_temp_pedido);
                        conec.CmdPlabal.Parameters.AddWithValue("@referencia", item._Referencia);
                        conec.CmdPlabal.Parameters.AddWithValue("@termino", item._Terminologia);
                        conec.CmdPlabal.Parameters.AddWithValue("@cant", item._Cantidad);
                        conec.CmdPlabal.Parameters.AddWithValue("@ancho", item._Ancho);
                        conec.CmdPlabal.Parameters.AddWithValue("@alto", item._Alto);
                        conec.CmdPlabal.Parameters.AddWithValue("@codigoalfak", item._CodigoAlfak);
                        conec.CmdPlabal.Parameters.AddWithValue("@existecod", item._ExisteCode);
                        conec.CmdPlabal.Parameters.AddWithValue("@neto", item.Neto);
                        conec.CmdPlabal.Parameters.AddWithValue("@iva", item.Iva);
                        conec.CmdPlabal.Parameters.AddWithValue("@total", item.Total);
                        conec.CmdPlabal.Parameters.AddWithValue("@m2item", item.M2Item);
                        conec.CmdPlabal.Parameters.AddWithValue("@kgitem", item.KilosItem);
                        conec.CmdPlabal.Parameters.AddWithValue("@perimetroitem", item.PerimetroItem);
                        conec.CmdPlabal.Parameters.AddWithValue("@m2unit", item.M2Unit);
                        conec.CmdPlabal.Parameters.AddWithValue("@kgunit", item.KilosUnit);
                        conec.CmdPlabal.Parameters.AddWithValue("@perimetrounit", item.PermietroUnit);


                        conec.CmdPlabal.ExecuteNonQuery();
                        conec.ConnPlabal.Close();

                    }
                    catch (Exception EX)
                    {
                        throw EX;

                        valid = false;
                    }

                }


            }




            return valid;
        }

    }

    public class LecturaxlsTP : Page
    {
        public List<Valores> Lista;

        public bool EsValido { get; set; }
        public string[] ErrorString { get; set; }

        #region Prop
        
        private readonly string Query;
        private readonly string Hoja;
        private readonly int Col_Ref;
        private readonly int Col_Term;
        private readonly int Col_Anc;
        private readonly int Col_Alt;
        private readonly int Col_Cant;
        private readonly int RowStart;
        private readonly OleDbConnection cnn;
        private OleDbDataAdapter da;
        private OleDbDataReader dr;
        private readonly SelectRows Select;
        private readonly object[] Param;
        private readonly string[] ParamName;
        private readonly string Connstr;
        private readonly string Archivo;
        private ExcelRange cell;
        private ExcelRow XlsRow;

        private string Ref;
        private string Term;
        private int Ancho;
        private int Alto;
        private int Cant;

        #endregion

        public LecturaxlsTP(string fileName, int IDXLSASIG)
        {
            Lista = new List<Valores>();
            string path = Path.Combine(Server.MapPath("~/CargaMasiva/"));
            Archivo = path + fileName;
            var listaError = new List<string>();
            Connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
        "Data Source = " + Archivo + "; " + "Extended Properties='Excel 8.0;HDR=YES;'";

            Param = new object[] { IDXLSASIG,true };
            ParamName = new string[] { "ID","ESTADO"};
            Select = new SelectRows("PLABAL","ECOM_XLSASIG","TOP 1 *","ID=@ID AND ESTADO=@ESTADO",Param,ParamName);

            if (Select.IsGot)
            {
                foreach (DataRow Drow in Select.Datos.Rows)
                {
                    RowStart= Convert.ToInt32(Drow["ROWSTART"].ToString());
                    Hoja = Drow["HOJA"].ToString();
                    Col_Ref = Convert.ToInt32(Drow["COLREFERENCIA"].ToString());
                    Col_Term = Convert.ToInt32(Drow["COLTERMINOLOGIA"].ToString());
                    Col_Anc= Convert.ToInt32(Drow["COLANCHO"].ToString());
                    Col_Alt= Convert.ToInt32(Drow["COLALTO"].ToString());
                    Col_Cant= Convert.ToInt32(Drow["COLCANT"].ToString());
                }
                cnn = new OleDbConnection(Connstr);
                if (fileName.Contains(".xlsx"))
                {
                    ThruXlsx();
                }
                else
                {
                    Col_Alt--;
                    Col_Anc--;
                    Col_Cant--;
                    Col_Ref--;
                    Col_Term--;
                    RowStart--;

                    Query = "select * from [" + Hoja  + "$A" + RowStart.ToString() + ":Z100]";
                    ThruXls();
                }




            }

            
        }

        private void ThruXlsx()
        {
            var fi = new FileInfo(Archivo);

            using (var paquete= new ExcelPackage(fi))
            {
                try
                {

                    var Libro = paquete.Workbook;
                    var Sheet = Libro.Worksheets.Where(it => it.Name == Hoja).First();

                    int i = RowStart;
                    bool DO = true;
                    while (DO)
                    {
                        /*Referencia*/
                        cell = Sheet.Cells[i, Col_Ref];
                        if (cell.Value != null)
                        {
                            Ref = cell.Value.ToString();
                            DO = true;
                        }
                        else
                        {
                            DO = false;

                        }

                        /*Termino*/
                        cell = Sheet.Cells[i, Col_Term];
                        if (cell.Value != null)
                        {
                            Term = cell.Value.ToString();
                        }
                        else
                        {
                            Term = "";
                        }

                        /*Cant*/
                        cell = Sheet.Cells[i, Col_Cant];
                        if (cell.Value != null)
                        {
                            Cant = Convert.ToInt32(cell.Value);
                        }
                        else
                        {
                            Cant = 0;
                        }

                        /*Ancho*/
                        cell = Sheet.Cells[i, Col_Anc];
                        if (cell.Value != null)
                        {
                            Ancho = Convert.ToInt32(cell.Value);
                        }
                        else
                        {
                            Ancho = 0;
                        }

                        /*aLTO*/
                        cell = Sheet.Cells[i, Col_Alt];
                        if (cell.Value != null)
                        {
                            Alto = Convert.ToInt32(cell.Value);
                        }
                        else
                        {
                            Alto = 0;
                        }


                        if (DO)
                        {
                            Valores VAL = new Valores
                            {
                                Terminologia = Term,
                                Alto = Alto,
                                Ancho = Ancho,
                                Cantidad = Cant,
                                Referencia = Ref,
                                Item = i,
                            };
                            Lista.Add(VAL);
                            i++;

                        }

                    }
                    
                }
                catch (Exception ex)
                {

                    throw;
                }
                
            }

        }

        private void ThruXls() {
            
            try
            {
                
                cnn.Open();

                da = new OleDbDataAdapter(Query, cnn);

                dr = da.SelectCommand.ExecuteReader();
                int y = 1;
                while (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string var1 = dr.GetValue(Col_Ref).ToString();
                        string var2 = dr.GetValue(Col_Term).ToString();
                        if (!string.IsNullOrWhiteSpace(var1))
                        {
                            Valores Val;
                            if (!string.IsNullOrWhiteSpace(var2))
                            {

                                if (!string.IsNullOrWhiteSpace(dr.GetValue(Col_Anc).ToString()))
                                {

                                    if (!string.IsNullOrWhiteSpace(dr.GetValue(Col_Alt).ToString()))
                                    {

                                        if (!string.IsNullOrWhiteSpace(dr.GetValue(Col_Cant).ToString()))
                                        {
                                            Val = new Valores
                                            {
                                                Referencia = var1,
                                                Terminologia = var2,
                                                Alto = Convert.ToDouble(dr.GetValue(Col_Alt).ToString()),
                                                Ancho = Convert.ToDouble(dr.GetValue(Col_Anc).ToString()),
                                                Cantidad = Convert.ToInt32(dr.GetValue(Col_Cant).ToString()),
                                                Item = y,
                                            };

                                            Lista.Add(Val);
                                            y++;

                                        }
                                        else
                                        {

                                        }

                                    }
                                    else
                                    {


                                    }

                                }
                                else
                                {

                                }

                            }
                            else
                            {

                            }

                            
                        }



                    }
                    dr.NextResult();
                }

                dr.Close();
                cnn.Close();

            }
            catch (Exception EX)
            {
                EsValido = false;
                throw EX;
            }

        }


        public class Valores
        {
            public int Item { get; set; }
            public string Referencia { get; set; }
            public string Terminologia { get; set; }
            public double Ancho { get; set; }
            public double Alto { get; set; }
            public int Cantidad { get; set; }

        }



    }




}
