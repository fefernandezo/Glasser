using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RandomERP;
using RandomERP.ConsumosPPR;
using PlanificacionOT;
using GlobalInfo;
using System.Web.UI.HtmlControls;

public partial class _Consumo : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        

    }

   

    protected void BtnListOts_Click(object sender, EventArgs e)
    {
        CreateGridOT();
        ScriptManager.RegisterStartupScript(this, typeof(Page), "CallMyFunction", "OpenListaOT();", true);



    }
    HtmlGenericControl thead;
    HtmlGenericControl tbody;
    HtmlGenericControl strong;
    HtmlGenericControl th;

    private void CreateGridOT()
    {
        GetTop40OtSinConsumo getTop20Ot = new GetTop40OtSinConsumo();
        List<GetTop40OtSinConsumo.OTsinConsumo> pote = getTop20Ot.Lista;
        GrdOT.DataSource = pote;
        GrdOT.DataBind();

    }
    

    

    
    List<DetalleConsumo> _DETALLE;

    protected void BtnGetDetailsOT_Click(object sender, EventArgs e)
    {

        
        if (GrdOT.Rows.Count>0)
        {
            
            
            _DETALLE = new List<DetalleConsumo>();
            int cont = 0;
            bool StockBreak = false;
            List<string> CodeBrk = new List<string>();
            List<string> OTCodeBrk = new List<string>();
            foreach (GridViewRow row in GrdOT.Rows)
            {
                var check = row.FindControl("ChkOT") as CheckBox;

                if (check.Checked)
                {
                    cont++;
                    GetDatosOtPconsumno GetOt = new GetDatosOtPconsumno(check.Text);
                    List<TablaPOTD> ListpOTD = new List<TablaPOTD>();
                    ListpOTD.AddRange(GetOt.POTD.Where(it => it._NIVEL == "1").Where(dot => dot._CANTIDADF > 0).ToList());
                    GetTotalStock totalStock = new GetTotalStock(GetOt.POTE._EMPRESA, "PHU", "PPR");
                    foreach (var item in ListpOTD)
                    {
                        bool GetAltern = false;
                        bool hastock = false;
                        if (totalStock.Stock.ContainsKey(item._CODIGO))
                        {
                            double ValueStock = (double)totalStock.Stock[item._CODIGO];
                            if (ValueStock <= 0 || ValueStock < item._CANTIDADF)
                            {
                                GetAltern = true;
                            }
                            else
                            {
                                totalStock.Stock[item._CODIGO] = ValueStock - item._CANTIDADF;
                                hastock = true;
                            }
                        }
                        else
                        {
                            GetAltern = true;
                        }
                        
                        
                        
                        string code;
                        
                        if (GetAltern)
                        {
                            VerStockMaterial verStock = new VerStockMaterial(GetOt.POTE._EMPRESA, "PHU", "PPR", item._CODIGO, item._CANTIDADF);
                            if (verStock.HasStock)
                            {
                                code = item._CODIGO;
                                hastock = true;
                            }
                            else if (verStock.IsAlternativa)
                            {
                                code = verStock.Datos.CODIGO;
                                hastock = true;
                            }
                            else
                            {
                                hastock = false;
                                code = "";
                            }
                        }
                        else
                        {
                            code = item._CODIGO;
                        }


                        
                        if (hastock)
                        {
                            GetPMSUCyCODMAQ PMSUCyCODMAQ = new GetPMSUCyCODMAQ(code,"PHU", item._IDPOTL, item._OPERACION, GetOt.POTE._EMPRESA);
                            DetalleConsumo detail = new DetalleConsumo
                            {
                                _IDPOTL=item._IDPOTL,
                                _CANTIDADF = item._CANTIDADF,
                                _CODIGO=code,
                                _IDPOTD = item.IDPOTD,
                                _KOFUCRE = GetOt.POTE._KOFUCRE,
                                _MODO = GetOt.POTE._MODO,
                                _NIVELSUP = item._NIVELSUP,
                                _NUMOT = GetOt.POTE._NUMOT,
                                _OPERACION = item._OPERACION,
                                _SUBNREG = item._SUBNREG,
                                _SUOT=GetOt.POTE._SUOT,
                                _TIMODO = GetOt.POTE._TIMODO,
                                _TIPO= item._TIPO,
                                _TIPOUNI= item._TIPOUNI,
                                _UNIDAD=item._UNIDAD,
                                _UNIDADC=item._UNIDADC,
                                _TAMODO = GetOt.POTE._TAMODO,
                                _CODMAQ= PMSUCyCODMAQ.CODMAQ,
                                _PPPRPMSUC=PMSUCyCODMAQ.PMSUC,
                                
                            };
                            _DETALLE.Add(detail);
                        }
                        else
                        {
                            StockBreak = true;
                            CodeBrk.Add(item._CODIGO.Trim());
                            OTCodeBrk.Add(GetOt.POTE._NUMOT);
                            code = "";

                        }
                        
                        
                        
                    }
                }
            }
            if (_DETALLE.Count>0)
            {
                BtnGenConsumo.Visible = true;
            }
            else
            {
                BtnGenConsumo.Visible = false;
            }
            panelDetalle.Visible = true;
            GrdDetalle.DataSource = _DETALLE;
            GrdDetalle.DataBind();

            if (StockBreak)
            {
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), 
                    "myalert", "alert('Los siguientes códigos no tienen stock ni producto alternativo: " + string.Join(", ", CodeBrk.Distinct())  +
                    ". La(s) OT(s) " + string.Join(", ", OTCodeBrk.Distinct()) + " seguirá apareciendo hasta que se haga el consumo de dicho(s) componentes.')", true);

            }

        }
        else
        {
            
        }

       
        

    }






    protected void BtnGenConsumo_Click(object sender, EventArgs e)
    {
        string Msj = "";
        string _TIDO = "GDI";
        string _Modalidad = "PHPRO";
        string _Empresa = "01";
        string _Endo = "76829725-8";
        string _Sudo = "PHU";
        string _Kofudo = "CMP";
        string _ListaPre = "TABPP02C";

        _DETALLE = new List<DetalleConsumo>();
        Validadores val = new Validadores();
        foreach (GridViewRow row in GrdDetalle.Rows)
        {
            
            DetalleConsumo detalle = new DetalleConsumo {
                _NUMOT = row.Cells[0].Text,
                _CODIGO= row.Cells[1].Text,
                _CANTIDADF= val.ParseoDouble(row.Cells[2].Text),
                _UNIDADC= row.Cells[3].Text,
                _TIPO= row.Cells[4].Text,
                _NIVELSUP= row.Cells[5].Text,
                _SUBNREG= row.Cells[6].Text,
                _OPERACION= row.Cells[7].Text,
                _SUOT= row.Cells[8].Text,
                _KOFUCRE= row.Cells[9].Text,
                _IDPOTD= row.Cells[10].Text,
                _IDPOTL= row.Cells[11].Text,
                _MODO= row.Cells[12].Text,
                _TIMODO= row.Cells[13].Text,
                _TIPOUNI= row.Cells[14].Text,
                _UNIDAD= row.Cells[15].Text,
                _TAMODO = val.ParseoDouble(row.Cells[16].Text),
                _PPPRPMSUC = val.ParseoDouble(row.Cells[17].Text),
                _CODMAQ = row.Cells[18].Text,

            };
            _DETALLE.Add(detalle);

        }
        int total = GrdDetalle.Rows.Count;
        bool CrearDoc = true;
        GenerarMAEDDOyMAEEDO generar = new GenerarMAEDDOyMAEEDO();
        int contador = 1;
        bool Doitem = false;
        double t= Convert.ToDouble(total)/28;
        int CantDoc = Convert.ToInt32(Math.Ceiling(t));
        string[] DocList = new string[CantDoc];
        
        foreach (var item in _DETALLE)
        {
            if (CrearDoc)
            {
                
                GenerarDocumento Doc = new GenerarDocumento(_Empresa,_Modalidad,_TIDO);
                if (Doc.IsSuccess)
                {
                    DateTime HOY = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    Tablas.MAEEDO MAEEDO = new Tablas.MAEEDO
                    {
                        EMPRESA = _Empresa,
                        TIDO = _TIDO,
                        IDMAEEDO = Doc.IDMAEEDO,
                        NUDO = Doc.NroDocumento,
                        ENDO = _Endo,
                        SUENDO = "",
                        ENDOFI = "",
                        TIGEDO = "I",
                        SUDO = _Sudo,
                        LUVTDO = "",
                        FEEMDO = HOY,
                        KOFUDO = _Kofudo,
                        ESDO = "C",
                        ESPGDO = "S",
                        CAPRAD = 0,
                        MEARDO = "N",
                        MODO = item._MODO,
                        TIMODO = item._TIMODO,
                        TAMODO = item._TAMODO,
                        NUCTAP = 0,
                        VACTDTNEDO = 0,
                        NUIVDO = 0,
                        POIVDO = 0,
                        NUIMDO = 0,
                        VAIMDO = 0,
                        POPIDO = 0,
                        VAPIDO = 0,
                        FE01VEDO = DateTime.Now,
                        FEULVEDO = DateTime.Now,
                        FEER = DateTime.Now,
                        NUVEDO = 0,
                        VAABDO = 0,
                        MARCA = "",
                        NUTRANSMI = "",
                        NUCOCO = "",
                        KOTU = "1",
                        LIBRO = "",
                        LCLV = val.ParseoDateTime("0"),
                        ESFADO = "",
                        KOTRPCVH = "",
                        NULICO = "",
                        PERIODO = "",
                        NUDONODEFI = false,
                        TRANSMASI = "",
                        POIVARET = 0,
                        VAIVARET = 0,
                        RESUMEN = "",
                        LAHORA = DateTime.Now,
                        KOFUAUDO = "",
                        KOOPDO = "",
                        ESPRODDO = "",
                        DESPACHO = 0,
                        HORAGRAB = (DateTime.Now.Hour*60 + DateTime.Now.Minute)*60,
                        RUTCONTACT = "",
                        SUBTIDO = "",
                        TIDOELEC = false,
                        ESDOIMP = "",
                        CUOGASDIF = "0",
                        BODESTI = "",
                        PROYECTO = "",
                        FECHATRIB = val.ParseoDateTime("0"),
                        NUMOPERVEN = "0",
                        BLOQUEAPAG = "",
                        VALORRET = 0,
                        FLIQUIFCV = val.ParseoDateTime("0"),
                        VADEIVDO = 0,
                        KOCANAL = "",
                        KOCRYPT = Doc.KOCRYPT,
                        LEYZONA = "",
                        KOSIFIC = "",
                        LISACTIVA = _ListaPre,
                        KOFUAUTO = "",
                        SUENDOFI = "",
                        VAIVDOZF = 0,
                        ENDOMANDA = "",
                        FLUVTCALZA = "",
                        ARCHIXML = "",
                        IDXML = "0",
                        SERIENUDO = "",
                        VALORAJU = "0",

                    };
                    generar.GetMAEEDO(MAEEDO);
                    CantDoc--;
                    DocList[CantDoc] = MAEEDO.NUDO;
                    Doitem = true;
                }
                else
                {
                    Doitem = false;
                    Msj = "Hubo un error al tratar de generar el documento";
                    break;
                }
                
                
            }

            if (Doitem)
            {
                string _NULIDO = contador.ToString();
                
                generar.AddItemMAEDDO(item);
            }
            

            if (contador==28)
            {
                if (total>28)
                {
                    total = total - 28;
                }
                
                CrearDoc = true;
                contador = 1;
                bool[] retorno =  generar.LuzCamaraAccion();
                if (!retorno[0] && !retorno[1])
                {
                    if (!retorno[0]) Msj = "Error al intentar generar el encabezado del documento";
                    if (!retorno[1]) Msj = "Error al intentar ingresar un item";
                    
                    break;
                }
            }
            else if (contador == total)
            {
                bool[] retorno = generar.LuzCamaraAccion();
                if (!retorno[0] && !retorno[1])
                {
                    if (!retorno[0]) Msj = "Error al intentar generar el encabezado del documento";
                    if (!retorno[1]) Msj = "Error al intentar ingresar un item";

                    break;
                }
                else
                {
                    Msj = "El consumo ha sido creado exitosamente con la(s) siguiente(s) " + _TIDO + ": " + string.Join(", ", DocList);
                }

            }
            
            else
            {
                CrearDoc = false;
                contador++;
            }

            
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + Msj +"'); window.location='" +
                Page.ResolveUrl("~/View/Planificacion/Consumo.aspx") + "';", true);

    }
}
