using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PlanificacionOT;
using GlobalInfo;
using System.Web.UI.HtmlControls;

public partial class View_Planificacion_CROT : Page
{
    OTGeneric oTGeneric;
    VarEncNVV varEncNVV;
    VarEncNVV varEnc;
    List<VarItemNVV> varItemNVV;
    List<VarItemNVV> ItemNVV;
    List<VarMaq> varMaqs;
    List<VarInsumos> varIns;
    Fechas fechas;


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtnBuscarNvv_Click(object sender, EventArgs e)
    {
        BtnGenOT.Enabled = true;
        paneltipoot2.Visible = false;
        string NUDO = "%" + TxtBuscarNVV.Text + "%";
        oTGeneric = new OTGeneric();
        varEncNVV = new VarEncNVV();
        varItemNVV = new List<VarItemNVV>();
        fechas = new Fechas();
        
        varEncNVV = oTGeneric.VarEncNVV(NUDO);


        if(!varEncNVV.IsTrue)
        {
            PanelDetail.Visible = false;
            Mdl_LblMsg.Text = varEncNVV.MsgIstrue;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsg').modal('show');});</script>", false);
            
            
        }
        else
        {
            
            if(varEncNVV.ESPRODO=="O")
            {
                PanelDetail.Visible = false;
                Mdl_LblMsg.Text = varEncNVV.MsgIstrue;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsg').modal('show');});</script>", false);
                

            }
            else
            {
                VALIDADOR vALIDADOR = new VALIDADOR();
                string TIPOOT = vALIDADOR.TIPOOT(varEncNVV.KOMODE);
                if (TIPOOT== "NO")
                {
                    paneltipoot2.Visible = true;
                    VaoNO.Value = "NO";
                    List<ListaTIPOOT> TIPOOTlist= oTGeneric.TIPOOT();
                    DrpTIPOOT.DataSource = TIPOOTlist;
                    DrpTIPOOT.DataTextField = "_COMBINADO";
                    DrpTIPOOT.DataValueField = "_TIPOOT";
                    Komode.Value = varEncNVV.KOMODE;

                    DrpTIPOOT.DataBind();
                    DrpTIPOOT.Items.Insert(0, new ListItem("--Seleccionar Tipo OT--"));

                    Mdl_LblMsg.Text = "En la base de datos no existe un tipo de OT para el modelo " + varEncNVV.KOMODE + ", debes seleccionar uno antes de generar la OT";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsg').modal('show');});</script>", false);
                    BtnGenOT.Enabled = false;
                }
                else
                {
                    Tipoop.Value = TIPOOT;
                    Komode.Value = varEncNVV.KOMODE;
                    VaoNO.Value = "SI";
                }
                string nvv = varEncNVV.NUDO;
                varItemNVV = oTGeneric.VarItemNVVs(nvv);
                LblFEEMLI.Text = varEncNVV.FEEMLI.ToShortDateString();
                LblFEERLI.Text = varEncNVV.FEERLI.ToShortDateString();
                int _dias;
                if (varEncNVV.SUDO=="VMA")
                {
                    _dias = 2;
                }
                else if(varEncNVV.SUDO == "PHU")
                {
                    _dias = 1;
                }
                else
                {
                    _dias = 1;
                }
                TextENTPROD.Text = fechas.FechaHabilAnterior(varEncNVV.FEERLI, _dias).ToShortDateString();
                LblKOEN.Text = varEncNVV.KOEN;
                LblNOKOEN.Text = varEncNVV.NOKOEN;
                LblNVV.Text = varEncNVV.NUDO.Trim();
                LblOP.Text = varEncNVV.NUMOP;
                
                

                TablaDetail.DataSource = varItemNVV;

                TablaDetail.DataBind();
                //BtnGen();
                
                PanelDetail.Visible = true;
            }
            
        }

        

    }

    
   

    protected void BtnGenOT_Click(object sender, EventArgs e)
    {
        BtnGenOT.Enabled = false;
        //Demora el proceso
        //System.Threading.Thread.Sleep(300000);


        string nudoN= LblNVV.Text;
        string tipoot = Tipoop.Value;
        
        string noOT = "0000" + LblOP.Text;
        DateTime FechentrCli = new DateTime();
        DateTime FEntrPro = new DateTime();
        int Valid1 = 0;
        int Valid2 = 0;
        try
        {
             FechentrCli= Convert.ToDateTime(LblFEERLI.Text);
        }
        catch
        {
            Valid1 = 1;
        }
        try
        {
             FEntrPro= Convert.ToDateTime(TextENTPROD.Text);
        }
        catch
        {
            Valid2 = 1;
        }

        if(Valid1==1 && Valid2==0)
        {
            LblIngOT.Text = "La fecha de entrega al \"Cliente\" no tiene el formato correcto, debes corregirla";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalIngOT').modal('show');});</script>", false);
        }
        else if(Valid1==0 && Valid2==1)
        {
            LblIngOT.Text = "La fecha de entrega \"Producción\" no tiene el formato correcto, debes corregirla";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalIngOT').modal('show');});</script>", false);
        }
        else if(Valid1==1 && Valid2==1)
        {
            LblIngOT.Text = "La fecha de entrega al Cliente y entrega Producción no tienen el formato correcto, debes corregirlas";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalIngOT').modal('show');});</script>", false);
        }
        else
        {
            if(VaoNO.Value == "SI")
            {
               bool oTcomplete= OTAction(nudoN,FechentrCli,FEntrPro,tipoot);
                if (oTcomplete)
                {
                    LblIngOT.Text = "Se ha generado la OT N° " + noOT + "  para la Nota de Venta N° " + LblNVV.Text;
                    HtmlGenericControl Boton = new HtmlGenericControl("button");
                    Boton.Attributes.Add("class", "btn btn-success btn-lg botonIngOt");
                    Boton.Attributes.Add("onclick", "Redirecciona()");
                    Boton.InnerHtml = "OK";

                    PanelIngOT.Controls.Add(Boton);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(" +
                        "function () {$('#ModalIngOT').modal('show');" +
                        "setTimeout(function(){" +
                        "url = '../Planificacion/CROT.aspx'; $(location).attr('href', url); " +
                        "}, 6000);} " +
                        ");</script>", false);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('Hubo un error con el número de OT " + noOT + ", comuníquese con Francisco.');});</script>", false);
                }
                
                
                
                //Response.Redirect("~/View/Planificacion/CROT.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('No se ha seleccionado el tipo de OT');});</script>", false);
            }
            
           
        }

        

    }
    


    private bool OTAction(string nudoNVV, DateTime FechEntr, DateTime FEntrProd, string TIPOOT)
    {
        bool IsCOMPLETE = false;
        oTGeneric = new OTGeneric();
        //GENERA ENCABEZADO DEL PEDIDO
        varEnc = new VarEncNVV();
        varEnc = oTGeneric.VarEncNVV(nudoNVV);
        varEnc._TIPOOT = TIPOOT;


        //VALIDA SI ES SUCURSAL
        bool Sucursal;
        if (varEnc.SUDO == "VMA")
        {
            Sucursal = true;
        }
        else
        {
            Sucursal = false;
        }

        //CREACION TABLA POTE DEL PEDIDO
        TablaPOTE Tpote = new TablaPOTE();
        PLA_OTING oting = new PLA_OTING();

        Tpote = POTE(varEnc, FechEntr,FEntrProd);

        PLA_OTINGList Oting0 = new PLA_OTINGList {
            _NUMOT=Tpote._NUMOT,
            _MsgStatus="Ingresando OT",
            _Status= 5,
        };
        oting.InsertPLA_OTING(Oting0, User.Identity.Name);

        if(string.IsNullOrWhiteSpace(Tpote._TIPOOT))
        {
            Mdl_LblMsg.Text ="El tipo de OT no está ingresado en el sistema, comunicate con Francisco";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {$('#ModalMsg').modal('show');});</script>", false);
        }
        else
        {
      
        //GENERA VARIABLES DEL ITEM
        ItemNVV = new List<VarItemNVV>();
        ItemNVV = oTGeneric.VarItemNVVs(nudoNVV);


            PLA_OTINGList Oting1 = new PLA_OTINGList
            {
                _NUMOT = Tpote._NUMOT,
                _MsgStatus = "Generando Variables de item",
                _Status = 10,
            };
            oting.UpdatePLA_OTING(Oting1);


            //INSERTA TABLA POTE Y DEVUELVE EL ID DE POTE
            string IDPOTE = oTGeneric.InsertPOTE(Tpote);
            if (IDPOTE=="ERROR1")
            {
                IsCOMPLETE = false;
            }
            else if (IDPOTE=="ERROR2")
            {
                IsCOMPLETE = false;
            }
            else
            {
                double ItemsCount = ItemNVV.Count();
                double AddPorc = Math.Ceiling(83 / ItemsCount);

                int Avance = Convert.ToInt32(10 + AddPorc);
                //LOOP DE LOS ITEMS
                foreach (var item in ItemNVV)
                {


                    //SE OBTIENEN VARIABLES DE INSUMOS
                    varIns = new List<VarInsumos>();
                    varIns = oTGeneric.GetCostInsumos(item.CODNOMEN, item.IDTINTERMO, item.CANTIDAD);
                    PLA_OTINGList Oting2 = new PLA_OTINGList
                    {
                        _NUMOT = Tpote._NUMOT,
                        _MsgStatus = "Insertando Información del producto " + item.KOPR,
                        _Status = Avance,
                    };
                    Avance = Avance + Convert.ToInt32(AddPorc);

                    oting.UpdatePLA_OTING(Oting2);
                    //SE OBTIENEN VARIABLES DE MAQUINAS
                    varMaqs = new List<VarMaq>();
                    varMaqs = oTGeneric.GetCostMOyMAQ(item.CODNOMEN, item.IDTINTERMO);

                    //SE OBTIENEN LOS COSTOS DEL ITEM Y SE CREA LA TABLA TABPRE
                    double cinsumos = Math.Round(varIns.Sum(ite => ite.PMIFRS * Convert.ToDouble(ite.CANTUNIT)), 2);
                    double cmobra = Math.Round(varMaqs.Sum(ite => ite.C_M_OBRA), 2);
                    double cmaquinas = Math.Round(varMaqs.Sum(ite => ite.C_MAQUINAS), 2);
                    double cfabric = cinsumos + cmobra + cmaquinas;



                    #region CreacionTablas
                    TablaTABPRE Ttabpre = new TablaTABPRE
                    {
                        _C_FABRIC = cfabric,
                        _C_INSUMOS = cinsumos,
                        _C_MAQUINAS = cmaquinas,
                        _C_MOBRA = cmobra,
                        _KOPR = item.KOPR,
                        _KOLT = "01C",
                        _PP01UD = "0",

                    };

                    //CREACION DE TABLA POTL DEL ITEM
                    TablaPOTL Tpotl = new TablaPOTL
                    {
                        _IDPOTE = IDPOTE,
                        _EMPRESA = varEnc.EMPRESA,
                        _NUMOT = Tpote._NUMOT,
                        _NREG = item.NREG,
                        _ESTADO = "V",
                        _DESDE = "DET",
                        _CODIGO = item.KOPR,
                        _UDAD = item.UD01PR,
                        _CODNOMEN = item.CODNOMEN,
                        _NUMECOTI = "",
                        _NREGCOT = "",
                        _FABRICAR = item.CANTIDAD,
                        _REALIZADO = 0,
                        _DIFERENCIA = 0,
                        _MARCANOM = "S",
                        _NUMDEEST = 0,
                        _NIVEL = 0,
                        _GLOSA = item.NOKOPR,
                        _PLANO = "",
                        _EXTEN = "BMP",
                        _NIVELSUP = item.NREG,
                        _ESCADENA = "",
                        _PORENTRAR = 0,
                        _NUMPLAN = "",
                        _INFORABODE = "",
                        _LILG = "",
                        _NUMODC = "",
                        _NREGODC = "",
                        _SULIOTL = Tpote._SUOT,
                        _BOLIOTL = "PLA",
                        _PORASIGNAR = "",
                        _KOFUCRE = Tpote._KOFUCRE,
                        _KOFUCIE = Tpote._KOFUCIE,
                        _F_COSTEO = Tpote._FIOT,
                        _C_INSUMOS = cinsumos,
                        _C_MAQUINAS = cmaquinas,
                        _C_M_OBRA = cmobra,
                        _C_FABRIC = cinsumos + cmobra + cmaquinas,
                        _ESODD = "",
                        _VNSERVICIO = 0,
                        _P_FABRIC = 0,
                        _P_INSUMOS = 0,
                        _P_MAQUINAS = 0,
                        _P_M_OBRA = 0,
                        _CCFABRIC = 0,
                        _CCINSUMOS = 0,
                        _CCMAQUINAS = 0,
                        _CCM_OBRA = 0,
                        _PCFABRIC = 0,
                        _PCINSUMOS = 0,
                        _PCMAQUINAS = 0,
                        _PCM_OBRA = 0,
                        _PODESVNSER = 0,
                        _ECUVNSERVI = "",
                        _OBSERVA = "",
                        _PLANTA = 0,
                        _USOS = 0,
                        _PREFIJADO = "",
                        _PODESINSU = 0,
                        _PODESMO = 0,
                        _PODESMAQ = 0,
                        _PODESSAL = 0,








                    };

                    //CREACION DE TABLA MAEST
                    TablaMAEST Tmaest = new TablaMAEST
                    {
                        _EMPRESA = varEnc.EMPRESA,
                        _KOBO = "PLA",
                        _KOPR = item.KOPR,
                        _KOSU = Tpote._SUOT,
                        _STENFAB1 = item.CANTIDAD,
                        _STENFAB2 = item.CAPRCO2,

                    };

                    //CREACION DE TABLA PDIMOT
                    TablaPDIMOT Tpdimot = new TablaPDIMOT
                    {
                        _EMPRESA = varEnc.EMPRESA,
                        _CODIGO = item.KOPR,
                        _NUMOT = Tpote._NUMOT,
                        _NREGOTL = Tpotl._NREG,
                        _NOMBRE = Tpotl._GLOSA,
                        _UDAD = " ",
                        _ANCHO = item.ANCHO,
                        _CANT_M2 = "0",
                        _CLAM180250 = 0,
                        _CLAM200321 = 0,
                        _CLAM220321 = 0,
                        _CLAM223330 = 0,
                        _CLAM225360 = 0,
                        _CLAM240321 = 0,
                        _CLAM240330 = 0,
                        _CLAM244285 = 0,
                        _CLAM244321 = 0,
                        _CLAM244330 = 0,
                        _CLAM250360 = 0,
                        _LARGO = item.LARGO,
                        _M2_LAMINA = "0",
                        _PORC_AZUL = "0",
                        _PORC_AMAR = "0",
                        _PORC_BLCO = "0",
                        _PORC_ESME1 = "0",
                        _PORC_ESME2 = "0",
                        _PORC_NEGRO = "0",
                        _PORC_ORO = "0",
                        _PORC_ROJO = "0",
                        _PORC_VERDE = "0",
                        _RENDIM = "0",
                        _VELPULST = "0",
                        _PERFORA = item.PERFORA,
                        _DESTAJE = item.DESTAJE,
                        _DLAM240321 = "0",
                        _DLAM250360 = "0",
                        _DLAM244330 = "0",
                        _DLAM170220 = "0",
                        _CLAM170220 = "0",
                        _ESPESOR = item.ESPESOR,
                        _TONHRS = "0",
                        _CLAM190321 = "0",
                        _V_BILAT_2 = "0",
                        _M2AUN = "0",
                        _PRIMITIVO1 = "0",
                        _PRIMITIVO2 = "0",
                        _PORC_WDKGY = "0",
                        _PORC_OFWHT = "0",
                        _PORC_CRANB = "0",
                        _PORC_SNWHT = "0",
                        _PORC_VDFGR = "0",
                        _UN_JABA = "0",
                        _PERF_1 = "0",
                        _PERF_2 = "0",
                        _PERF_3 = "0",
                        _PERF_4 = "0",
                        _ESQUINA_1 = "0",
                        _ESQUINA_2 = "0",
                        _ESQUINA_3 = "0",
                        _ESQUINA_4 = "0",
                        _ESQUINA_5 = "0",
                        _FORMA_BOTT = item.FORMA_BOTT,
                        _PRODUCTO = "0",
                        _DIAM_1 = "0",
                        _DIAM_2 = "0",
                        _DIAM_3 = "0",
                        _DIAM_4 = "0",
                        _COLOR_1 = "0",
                        _COLOR_2 = "0",
                        _COLOR_3 = "0",
                        _COLOR_4 = "0",
                        _COLOR_5 = "0",
                        _COLOR_6 = "0",
                        _COLOR_7 = "0",
                        _COLOR_8 = "0",
                        _COLOR_9 = "0",
                        _COLOR_10 = "0",
                        _COLOR_11 = "0",
                        _COLOR_12 = "0",
                        _MESCLA_1 = "0",
                        _MESCLA_2 = "0",
                        _PASADAS = "0",
                        _DESXPRO = "0",
                        _CLAM220330 = "0",
                        _VALOR_US = "0",
                        _MEZCLA_3 = "0",
                        _MEZCLA_4 = "0",
                        _MEZCLA_5 = "0",
                        _MEZCLA_6 = "0",
                        _MEZCLA_7 = "0",
                        _MEZCLA_8 = "0",
                        _PAPEL_1 = "0",
                        _MUELA_1 = "0",
                        _MUELA_2 = "0",
                        _MUELA_3 = "0",
                        _BROCA_1 = "0",
                        _BROCA_2 = "0",
                        _FRESA_1 = "0",
                        _M2_C_SERG = "0",
                        _PITILLA = "0",
                        _CLAM152213 = "0",
                        _CLAM160250 = "0",
                        _CLAM183244 = "0",
                        _CLAM213330 = "0",
                        _CLAM360250 = "0",
                        _AVELLANAD1 = "0",
                        _AISLAPOL_1 = "0",
                        _CINTA_EMBA = "0",
                        _SELLO_MET = "0",
                        _SEL_MET1 = "0",
                        _ZUNCHO_MET = "0",
                        _ZUNCHO_PLA = "0",
                        _BOLSA_1 = "0",
                        _CODCLIENTE = "0",
                        _SEPARADOR = "0",
                        _FILM_PLAS = "0",
                        _TIPOTEMPLA = "0",
                        _UNXH_CORT = "0",
                        _UNXH_PERF = "0",
                        _UNXH_PINT = "0",
                        _UNXH_PULI = "0",
                        _UNXH_PLAST = "0",
                        _UNXH_TEMP = "0",
                        _UNXH_VISOR = "0",
                        _LOTE_ENTRE = "0",
                        _LOTE_PINT = "0",
                        _LOTE_TEMP = "0",
                        _HH_MAN_EXT = "0",
                        _HH_MAN_INT = "0",
                        _C_MO_EXT = "0",
                        _C_MO_INT = "0",
                        _APROVECHAM = item.APROVECHAM,
                        _ESPESOR2 = "0",
                        _LARGOG = item.LARGOG,
                        _TP_DVH_COR = item.TP_DVH_COR,
                        _ANCHOSEP = item.ANCHOSEP,
                        _SIPOLISUL = item.SIPOLISUL,
                        _SISILICON = item.SISILICON,
                        _SIARGON = item.SIARGON,
                        _SIVALVULA = item.SIVALVULA,
                        _BYTETOTAL = item.BYTETOTAL,
                        _MERPROINS = "0",
                        _MARGTPNEL = "0",
                        _FACT_PORTE = item.FACT_PORTE,
                        _M_ANCHO = item.M_ANCHO,
                        _CORREINDS = item.CORREINDS,
                        _CORRE_ARQ = item.CORRE_ARQ,
                        _TP_TVH_COR = item.TP_TVH_COR,
                        _M2MP01 = item.M2MP01,
                        _M2MP02 = item.M2MP02,
                        _M2MP03 = item.M2MP03,
                        _ANCHOSEP2 = item.ANCHOSEP2,
                        _PED_ALFAK = "0",
                        _ITEM_ALFAK = "0",
                        _PDTO_ALFAK = "0",
                        _FPEDIDO_ALF = "0",
                        _CORRE_MAQ = "0",
                        _CORRE_REP = "0",
                        _UNXMP01 = "0",
                        _UNXMP02 = "0",
                        _UNXMP03 = "0",
                        _UNAISLA01 = "0",
                        _UNAISLA02 = "0",
                        _UNAISLA03 = "0",
                        _UNMADERA01 = "0",
                        _UNMADERA02 = "0",
                        _UNMADERA03 = "0",
                        _CORRE_SEMI = "0",
                        _M2CARTON = "0",
                        _ESPMP01 = "0",
                        _ESPMP02 = "0",
                        _ESPMP03 = "0",
                        _CORRE_OBRA = "0",
                        _CORR_CTZA = "0",
                        _CORR_VPVC = item.CORR_VPVC,
                        _DRUR_CTZA = "0",
                        _FRUR_CTZA = "0",
                        _M2LA_CTZA = "0",
                        _MLEN_CTZA = "0",
                        _NPAN_CTZA = "0",
                        _NPUE_CTZA = "0",
                        _POMO_CTZA = "0",
                        _UNAML = "0",



                    };

                    //INSERTAR TABLA EN DB DEL ITEM Y DEVUELVE EL IDPOTL
                    string IDPOTL = oTGeneric.InsertPOTL(Tpotl);

                    //CREACION TABLA POTD DEL ITEM
                    TablaPOTD TpotdItem = new TablaPOTD
                    {
                        _IDPOTL = IDPOTL,
                        _EMPRESA = varEnc.EMPRESA,
                        _NUMOT = Tpote._NUMOT,
                        _NREG = Tpotl._NREG,
                        _SUBNREG = "",
                        _ESTADO = "V",
                        _PERTENECE = "",
                        _NIVEL = "0",
                        _AUXI = "",
                        _CODIGO = item.KOPR,
                        _MARCANOM = "S",
                        _CODNOMEN = item.CODNOMEN,
                        _NUMDEEST = "0",
                        _GLOSA = item.NOKOPR,
                        _TIPOUNI = "0",
                        _DOBLEU = "0",
                        _CANTIDADF = item.CANTIDAD,
                        _CANTIDACF = "0",
                        _CANTIDADR = "0",
                        _CANTANTI = "0",
                        _UNIDAD = Tpotl._UDAD,
                        _UNIDADC = "",
                        _TIPO = "",
                        _OPERACION = "",
                        _CALIDAD = "",
                        _NIVELSUP = "",
                        _NUMPLAN = "",
                        _LILG = "",
                        _NUMODC = "",
                        _SULIOTD = Tpote._SUOT,
                        _BOLIOTD = "PLA",
                        _PNPDNREG = "0",
                        _C_SALIDA = 0,
                        _C_INSUMOS = 0,
                        _P_INSUMOS = "0",
                        _CCINSUMOS = "0",
                        _PCINSUMOS = "0",
                        _NREGODC = "",



                    };
                    #endregion


                    //CREACION E INGRESO DE LOS COMPONENTES/INSUMOS
                    #region INSUMOS/COMPONENTES
                    foreach (VarInsumos itemI in varIns)
                    {
                        TablaPOTD TpotdIns = new TablaPOTD
                        {
                            _IDPOTL = IDPOTL,
                            _EMPRESA = varEnc.EMPRESA,
                            _NUMOT = Tpote._NUMOT,
                            _NREG = Tpotl._NREG,
                            _SUBNREG = itemI._SUBNREG,
                            _ESTADO = "V",
                            _PERTENECE = Tpotl._NREG,
                            _NIVEL = "1",
                            _AUXI = "",
                            _CODIGO = itemI._ELEMENTO,
                            _MARCANOM = "",
                            _CODNOMEN = "",
                            _NUMDEEST = "0",
                            _GLOSA = itemI._GLOSA,
                            _TIPOUNI = "1",
                            _DOBLEU = itemI.DOBLEU,
                            _CANTIDADF = itemI.CANTIDADDF,
                            _CANTIDACF = "0",
                            _CANTIDADR = "0",
                            _CANTANTI = "0",
                            _UNIDAD = itemI._UNIDAD,
                            _UNIDADC = itemI._UNIDADC,
                            _TIPO = itemI._TIPO,
                            _OPERACION = itemI._OPERACION,
                            _CALIDAD = "I",
                            _NIVELSUP = Tpotl._NREG,
                            _NUMPLAN = "",
                            _LILG = "",
                            _NUMODC = "",
                            _SULIOTD = Tpote._SUOT,
                            _BOLIOTD = "PPR",
                            _PNPDNREG = itemI._NREG,
                            _C_SALIDA = itemI.PMIFRS,
                            _C_INSUMOS = itemI.PMIFRS,
                            _P_INSUMOS = "0",
                            _CCINSUMOS = "0",
                            _PCINSUMOS = "0",
                            _NREGODC = "",
                        };
                        //INSERTA POTD DEL INSUMO
                        oTGeneric.InsertPOTD(TpotdIns);

                        //UPDATE MAEPR DEL INSUMO
                        oTGeneric.UpdateMAEPRmaterial(itemI._ELEMENTO, itemI.CANTIDADDF, itemI.STREQFAB2);

                        //UPDATE MAEPREM
                        oTGeneric.UpdateMAEPREMmaterial(itemI._ELEMENTO, itemI.CANTIDADDF, itemI.STREQFAB2, varEnc.EMPRESA);

                        //UPDATE MAEST
                        oTGeneric.UpdateMAESTmaterial(itemI._ELEMENTO, itemI.CANTIDADDF, itemI.STREQFAB2, varEnc.EMPRESA, TpotdIns._SULIOTD, TpotdIns._BOLIOTD);
                    }

                    #endregion

                    //TRANSMISION A DB
                    #region TransmisionDB
                    //INSERT POTD DEL ITEM
                    oTGeneric.InsertPOTD(TpotdItem);

                    //UPDATE MAEPR DEL ITEM
                    oTGeneric.UpdateMAEPRproducto(item.KOPR, item.CANTIDAD, item.CAPRCO2);

                    //UPDATE MAEPREM DEL ITEM
                    oTGeneric.UpdateMAEPREMproducto(item.KOPR, item.CANTIDAD, item.CAPRCO2, varEnc.EMPRESA);

                    //UPDATE TABPRE
                    oTGeneric.UpdateTABPRE(Ttabpre);

                    //INSERTA EN TABLA MAEST
                    if (Sucursal)
                    {
                        oTGeneric.InsertMAEST(Tmaest, "PHU", "PLA");

                    }
                    else
                    {
                        oTGeneric.UpdateMAEST(Tmaest);
                    }


                    //INSERTA EN TABLA PDIMOT
                    oTGeneric.InsertPDIMOT(Tpdimot);


                    //INSERTA EN LA TABLA POTLCOM
                    TablaPOTLCOM Tpotlcom = new TablaPOTLCOM
                    {
                        _IDPOTL = IDPOTL,
                        _ARCHIRST = "MAEDDO",
                        _IDRST = item.IDMAEDDO,
                        _EMPRESA = varEnc.EMPRESA,
                        _NUMOT = Tpote._NUMOT,
                        _NREGOTL = Tpotl._NREG,
                        _DESDE = "NVV",
                        _NUMECOTI = varEnc.NUDO,
                        _ENDO = varEnc.KOEN,
                        _NREGCOT = Tpotl._NREG,
                        _CODIGO = item.KOPR,
                        _UDAD = item.UD01PR,
                        _FABRICAR = item.CANTIDAD.ToString(),
                        _REALIZADO = "0",
                        _DESISTIDO = "0",
                        _ASIGNADO = "0",
                        _ESODD = "",
                        _ASIGNADO2 = "0",
                    };
                    oTGeneric.InsertPOTLCOM(Tpotlcom);

                    //UPDATE MAEDDO
                    oTGeneric.UpdateMAEDDO(item.CANTIDAD.ToString(), item.IDMAEDDO);

                    #endregion


                    //MAQUINAS
                    foreach (VarMaq ItemMaq in varMaqs)
                    {
                        TablaPOTPR Tpotpr = new TablaPOTPR
                        {
                            _IDPOTL = IDPOTL,
                            _EMPRESA = varEnc.EMPRESA,
                            _NUMOT = Tpote._NUMOT,
                            _NREGOTL = Tpotl._NREG,
                            _CODIGO = item.KOPR,
                            _OPERACION = ItemMaq.OPERACION,
                            _ORDEN = ItemMaq.ORDEN,
                            _POROPANT = ItemMaq.POROPANT,
                            _PORREQCOMP = ItemMaq.PORREQCOMP,
                            _TIPOOP = ItemMaq.TIPOOP,
                            _ESTADO = "",
                            _SITPEDFAB = "",
                            _FABRICAR = item.CANTIDAD.ToString(),
                            _REALIZADO = "0",
                            _SALPROXJOR = ItemMaq.SALPROXJOR,
                            _PORMAQUILA = "0",
                            _LILG = "",
                            _NUMODC = "",
                            _NREGODC = "",
                            _CODMAQOT = ItemMaq.CODMAQOT,
                            _P_FABRIC = "0",
                            _P_INSUMOS = "0",
                            _P_M_OBRA = "0",
                            _P_MAQUINAS = "0",
                            _CCFABRIC = "0",
                            _CCINSUMOS = "0",
                            _CCMAQUINAS = "0",
                            _CCM_OBRA = "0",
                            _PCFABRIC = "0",
                            _PCINSUMOS = "0",
                            _PCMAQUINAS = "0",
                            _PCM_OBRA = "0",
                            _NOTAS = "",
                            _PERMAQALT = "SI",
                            _PERMAQPAR = "SI",

                        };
                        double M_cmobra = ItemMaq.C_M_OBRA;
                        double M_cmaquINA = ItemMaq.C_MAQUINAS;
                        double M_cinsumo = varIns.Where(IT => IT._OPERACION == ItemMaq.OPERACION).Sum(IT => IT.PMIFRS * Convert.ToDouble(IT.CANTUNIT));
                        Tpotpr._C_INSUMOS = Math.Round(M_cinsumo, 2);
                        Tpotpr._C_FABRIC = Math.Round(M_cinsumo + M_cmaquINA + M_cmobra, 2);
                        Tpotpr._C_MAQUINAS = Math.Round(M_cmaquINA, 2);
                        Tpotpr._C_M_OBRA = Math.Round(M_cmobra, 2);

                        oTGeneric.InsertPOTPR(Tpotpr);


                    }

                }
                PLA_OTINGList Oting4 = new PLA_OTINGList
                {
                    _NUMOT = Tpote._NUMOT,
                    _MsgStatus = "Casi Casi...",
                    _Status = 98,
                };
                oting.UpdatePLA_OTING(Oting4);

                //UPDATE MAEEDO
                oTGeneric.UpdateMAEEDO(varEnc.NUDO, varEnc.KOEN, "O", "", "", ItemNVV.Sum(ITE => ITE.CANTIDAD), 0, "0");

                PLA_OTINGList Oting5 = new PLA_OTINGList
                {
                    _NUMOT = Tpote._NUMOT,
                    _MsgStatus = "Terminado",
                    _Status = 100,
                };
                oting.UpdatePLA_OTING(Oting5);

                IsCOMPLETE = true;
            }

            
        }

        return IsCOMPLETE;
    }

    

    private TablaPOTE POTE(VarEncNVV varEncNVV, DateTime FechEntr, DateTime FEntrProd)
    {

        string NrOT = varEncNVV.NUMOP.PadLeft(10, '0'); 
        
        TablaPOTE tablaPOTE = new TablaPOTE {
            _EMPRESA = varEncNVV.EMPRESA,
            _NUMOT = NrOT,
            _ESTADO = "V",
            _FIOT = DateTime.Today,
            _FTOT = FechEntr,
            _REFERENCIA = varEncNVV.NOKOEN,
            _PLANO="",
            _EXTEN="",
            _NUMPLAN="",
            _SUOT= "PHU",
            _KOFUCRE = "CMP",
            _KOFUCIE="",
            _HORAGRAB=varEncNVV.HORAGRAB,
            _ESODD="",
            _ENDO="",
            _SUENDO="",
            _FACTURAR=false,
            _KOFUFACTU="",
            _MODO=varEncNVV.MODO,
            _TIMODO=varEncNVV.TIMODO,
            _TAMODO=varEncNVV.TAMODO,
            _KOCT01="",
            _POKOCT01=0,
            _KOCT02="",
            _POKOCT02=0,
            _KOCT03="",
            _POKOCT03=0,
            _DIASVIGCOT=0,
            _FORMAPAGO="",
            _ENCARGADO="",
            _CARGO="",
            _KOPROYE="",
            _KOETAPA="",
            _OCDO="",
            _FTESPPROD=FEntrProd,
            _PLANTAOT="",
            _TIPOOT=varEnc._TIPOOT,
            
            

        };
        




        return tablaPOTE;
        
    }

   

    protected void MdlBtnOkMsg_Click(object sender, EventArgs e)
    {

    }

    private class VARIABLE
        {
        public string KOPR { get; set; }
        public string C_MAQUINAS { get; set; }
        public string CM_OBRA { get; set; }
        public string CODMAQOT { get; set; }
        public double C_INSUMOS { get; set; }
        public string OPERACION { get; set; }
    }
    private class VAriable2
    {
        public string _KOPR { get; set; }
        public string _CODIGO { get; set; }
        public string _GLOSA { get; set; }
        public string _CANT { get; set; }
        public decimal _CANTUNIT { get; set; }
        public string _UNID { get; set; }
        public double _PRECIO { get; set; }
        public string _OPERACION { get; set; }
        

    }

    private class COSTOS
    {
        public string KOPR { get; set; }
        public double C_MAQUINAS { get; set; }
        public double CM_OBRA { get; set; }
        public double C_INSUMOS { get; set; }
        public double C_FABRIC { get; set; }
        public string N_REG { get; set; }
    }


    protected void BtnCamFech_Click(object sender, EventArgs e)
    {
        LblFEERLI.Enabled = true;
    }

    protected void BtnCamFech2_Click(object sender, EventArgs e)
    {
        TextENTPROD.Enabled = true;
    }

    protected void BtnDrTIPOOT_Click(object sender, EventArgs e)
    {
        paneltipoot2.Visible = false;

        string tipoot = DrpTIPOOT.SelectedValue;
        VALIDADOR vALIDADOR = new VALIDADOR();
        vALIDADOR.InsertASIGTYPEOT(Komode.Value,tipoot);
        VaoNO.Value = "SI";
        Tipoop.Value = tipoot;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script> $(document).ready(function () {alert('Se ha ingresado a la base de datos el tipo " + tipoot + " con el modelo " + Komode.Value + "');});</script>", false);

        BtnGenOT.Enabled = true;

    }
}


