using Ecommerce;
using GlobalInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Descripción breve de IngresoPedido
/// </summary>
/// 
namespace Alfak
{
    public class PedidoAlfak
    {
        public PedidoAlfak()
        {
            
        }

        public class Generate
        {
            public readonly string[] NroPedido;
            public readonly ClienteAlfak clienteAlfak;
            public readonly bool IsSuccess;
            public readonly string Mensaje;


            private BW_AUFTR_KOPF AUFTR_KOPF;
            private readonly PedidoEcom Pedido;
            private readonly string Sucursal;
            private readonly double PERIMTOT;
            private readonly double M2TOT;
            private readonly double KGTOT;
            private readonly string S;
            private readonly string Ecom;
            private readonly string Ind;
            private readonly DateTime hoy;
            private readonly DateTime mañana;
            private InsertRow insert;
            private Hashtable Htable;
            private readonly GetNroPedido getNro;
            private PedSubDet.GetSubs subs;

            public Generate(PedidoEcom _PedEcom, string _Sucursal)
            {
                S = " ";
                Ecom = "ECOMMERCE";
                Ind = "<indf>";
                hoy = DateTime.Today;
                mañana = DateTime.Today.AddDays(1);
                /*VARIABLES PENDIENTES*/
                SelectRows selectRows = new SelectRows("PLABAL", "ECOM_PEDDET", "SUM(PERIMITM),SUM(M2ITM),SUM(KGITM)", "IDPEDIDO=@IDPEDIDO AND ESTADO=@ESTADO",
                    new object[] {_PedEcom.ID,true },new string[] {"IDPEDIDO","ESTADO" });

                Validadores Val = new Validadores();
                PERIMTOT = Val.ParseoDouble(selectRows.Datos.Rows[0][0].ToString());
                M2TOT = Val.ParseoDouble(selectRows.Datos.Rows[0][1].ToString());
                KGTOT = Val.ParseoDouble(selectRows.Datos.Rows[0][2].ToString());

                Sucursal = _Sucursal;
                Pedido = _PedEcom;
                getNro = new GetNroPedido();
                if (getNro.IsGotta)
                {
                    int PedIndex = 1;
                    NroPedido = new string[PedIndex];

                    NroPedido[0] = getNro.Numero;
                    
                    /*Generar encabezado en Alfak*/
                    ClienteAlfak.Get getCliAlf = new ClienteAlfak.Get(Pedido.RUT);
                    if (getCliAlf.IsSuccess)
                    {
                        clienteAlfak = getCliAlf.Datos;
                        /*Insertar el encabezado del pedido*/
                        if (Insert_ENCABEZADO())
                        {
                            /*INSERTAR ITEMS Y SUBITEMS*/
                            PedDet.Get Det = new PedDet.Get(Pedido.ID, true);
                            int Contador = 1;
                            int total = Det.Items.Count;
                            foreach (PedDet _item in Det.Items)
                            {
                                if (Insert_ItemySub(_item,Contador))
                                {
                                    if (!Insert_ItemTINTERMO(_item, Contador))
                                    {

                                    }

                                    if (Contador == 29 && total > 29)
                                    {
                                        total = total - 29;
                                        Contador = 0;
                                        getNro = new GetNroPedido();
                                        PedIndex++;
                                        Array.Resize(ref NroPedido, PedIndex);
                                        NroPedido[PedIndex-1] = getNro.Numero;
                                        

                                    }

                                    Contador++;
                                }
                                else
                                {
                                    Htable = new Hashtable {
                                        { "ErrorString","Error ingreso item en alfak IDPEDDET=" +_item.IDPEDDET},
                                        { "URL","Archivo:IngresoPedido.cs,Namespace:Alfak,Clase:PedidoAlfak.Generate,Linea:80"},
                                        { "UserName"," "},
                                        { "DateTime",DateTime.Now}
                                    };
                                    insert = new InsertRow("PLABAL", "App_Error_Catch", Htable);


                                }
                                
                                
                            }
                            UpdatetablaPedido();
                            IngresarEnPlabal();
                            IsSuccess = true;
                        }
                        else
                        {
                            IsSuccess = false;
                            Mensaje = "Error Alfak01:No se pudo obtener info del cliente";
                        }
                    }
                    else
                    {
                        IsSuccess = false;
                        Mensaje = "Error Alfak01:No se pudo obtener info del cliente";
                    }
                }
                else
                {
                    IsSuccess = false;
                    Mensaje = "Error al intentar obtener un número de Orden de Producción";
                }
            }

            private bool Insert_ENCABEZADO()
            {
                
                
                string Prioridad;
                if (Pedido.CEXPRESS > 0)
                {
                    Prioridad = "Urgente";
                }
                else
                {
                    Prioridad = "Normal";
                }
               
                int IDCli = Convert.ToInt32(clienteAlfak.ID);
                
                int FI_FB_KZ = Convert.ToInt32(clienteAlfak.ZAHL_FB_KZ);
                int STATUS = 40;

               
                 Htable= new Hashtable
                {
                    { "ID", NroPedido },
                    {"NR_LIEFERSCHEIN", 0 },
                    { "SU_LFM_REAL",PERIMTOT},
                    { "STATUS",STATUS},
                    { "DATUM_ERF",hoy},
                    { "DATUM_PROD1",mañana},
                    { "DATUM_PROD2",hoy},
                    { "DATUM_PROD3",mañana},
                    { "DATUM_LIEFER_PLAN",Pedido.F_Entrega},
                    { "DATUM_LIEFER_TAT",Pedido.F_Entrega},
                    { "DATUM_LIEFERWUNSCH", S },
                    { "PRIORITAET",Prioridad },
                    { "DATUM_BEST",hoy},
                    { "LAUF_PROD1",0},
                    { "LAUF_PROD2",0},
                    { "LAUF_PROD3",0},
                    { "BEST_TEXT1",S},
                    { "BEST_TEXT2",S},
                    { "AH_HAUPT_AUFTR",0},
                    { "AH_IDENT",clienteAlfak.ID},
                    { "AH_LIEFERANT",0},
                    { "AH_MCODE",clienteAlfak.MCODE},
                    { "AH_KOPF",clienteAlfak.ADRESS_KOPF},
                    { "AH_NAME1",clienteAlfak.NAME1},
                    { "AH_NAME2",S},
                    { "AH_NAME3",S},
                    { "AH_STRASSE", clienteAlfak.STRASSE},
                    { "AH_PLZ",S},
                    { "AH_ORT",S},
                    { "AH_LAND",clienteAlfak.LAND},
                    { "AH_TELEFON",S},
                    { "AH_FAX",S},
                    { "AH_ANREDE",S},
                    { "AH_PARTNER",S},
                    { "AH_BIT1",0},
                    { "AL_IDENT",clienteAlfak.ID},
                    { "AL_KOPF",S},
                    { "AL_NAME1",S},
                    { "AL_NAME2",S},
                    { "AL_NAME3",S},
                    { "AL_STRASSE",S},
                    { "AL_PLZ",S},
                    { "AL_ORT",S},
                    { "AL_LAND",S},
                    { "AL_TELEFON",S},
                    { "AL_ANREDE",S},
                    { "AL_PARTNER",S},
                    { "AR_IDENT",clienteAlfak.ID},
                    { "AR_KOPF",S},{ "AR_NAME1",S},{ "AR_NAME2",S},{ "AR_NAME3",S},{ "AR_STRASSE",S},{ "AR_PLZ",S},{ "AR_ORT",S},{ "AR_LAND",S},
                    { "AR_TELEFON",S},{ "AR_ANREDE",S},{ "AR_PARTNER",S},
                    { "OR_SPRACH_ID",0},
                    { "OR_SPRACH_BASIS",0},
                    { "OR_MANDANT",1},
                    { "OR_AVBEREICH",Sucursal},
                    { "OR_BEARBEITER",Ecom},
                    { "OR_FACHBERATER",Ecom},
                    { "OR_GESCHART",Ind},
                    { "OR_SPERRKZ","libre"},
                    { "OR_ADIENST",Ecom},
                    { "OR_VERPACKUNG",Ind},
                    { "OR_LIEFERBED","6. Otros"},
                    { "OR_TOUR","SIN ZONIFICACION"},
                    { "OR_AWTOUR",Ind},{ "OR_FAHRER",Ind},{ "OR_ZOLLTOUR",Ind},{ "OR_GRUPPE",Ind},
                    { "KO_MASSEINH",0},
                    { "SU_LFM_FAKT",PERIMTOT},
                    { "KO_OBJEKT_KUNDE",0},
                    { "KO_OBJEKT_LIEF",0},
                    { "KO_SAMMELRE",1},
                    { "KO_TEILFAK",1},
                    { "KO_TEILLIEF",1},
                    { "ENTFERNUNG",0},
                    { "KO_NETTOPREISE",0},
                    { "KO_FAXVERSAND",0},
                    { "OR_ADIENST2",Ind},
                    { "SU_QM_REAL",M2TOT},
                    { "SU_QM_FAKT",M2TOT},
                    { "SU_GEWICHT",KGTOT},
                    { "SU_GEWICHT_TARA",0},
                    { "SU_STUNDEN",0},{ "SU_KM",0},{ "SU_SPRLFM_FAKT",0},{ "SU_SPRLFM_REAL",0},
                    { "FI_VALUTAKURS",1},
                    { "FI_MWST1",Constantes.IVA19},
                    { "FI_MWST2",0},
                    { "FI_MWST1_BASIS",Pedido.Neto},
                    { "FI_ZAHLBED",clienteAlfak.ZAHLBED},
                    { "FI_ZAHLWEG",Ind},
                    { "FI_WAEHRUNG",clienteAlfak.WAEHRUNG},
                    { "FI_MWST2_BASIS",0},
                    { "FI_MWST1_BASIS_FW",Pedido.Neto},
                    { "FI_MWST2_BASIS_FW",0},
                    { "FI_MWST1_ID",Constantes.IVA19},
                    { "FI_MWST2_ID",0},
                    { "FI_RABATT1",0},
                    { "FI_BETR_NETTO",Pedido.Neto},
                    { "FI_BETR_BRUTTO",Pedido.Bruto},
                    { "FI_BETR_NETTO_FW",Pedido.Neto},
                    { "FI_UST_ID",Pedido.RUT},
                    { "FI_FB_KZ",clienteAlfak.ZAHL_FB_KZ},
                    { "FI_ZAHL_TAG1",0},
                    { "FI_BETR_BRUTTO_FW",Pedido.Bruto},
                    { "FI_BETR_FESTPREIS",0},
                    { "FI_BETR_MWST1",Pedido.Iva},
                    { "FI_BETR_MWST2",0},
                    { "FI_BETR_MWST1_FW",Pedido.Iva},
                    {"FI_BETR_MWST2_FW",0 },
                    {"FI_BETR_ZWSUMME2",0 },
                    {"FI_BETR_ZWSU1_FW",Pedido.Neto },
                    {"FI_BETR_ZWSU2_FW",0 },
                    { "FI_BETR_EK1",Pedido.CDIRECTO},
                    {"FI_BETR_EK2",0 },
                    { "FI_SKONTO_BASIS",Pedido.Neto},
                    { "FI_SKONTO_BASIS_FW",Pedido.Neto},
                    { "FI_BETR_SKONTO",0},
                    { "FI_BETR_SKONTO_FW",0},
                    { "FI_BETR_PROV",0},
                    { "FI_BETR_ZWSUMME1",Pedido.Neto},
                    { "EK_BETR_FESTPREIS",0},
                    { "FI_BETR_FESTPR_FW",0},{ "MINMENGE",0},{ "BIT",0},{ "ETIKETTEN_TYP",0},
                    { "DOK_TYP",Ind},
                    { "FI_ZAHL_TAG2",0},{ "FI_ZAHL_TAG3",0},{ "LADELISTE",0},{ "OR_LKW",Ind},
                    { "AH_PLZ_POSTFACH",S},{ "AH_POSTFACH",S},{ "AR_PLZ_POSTFACH",S},{ "AR_POSTFACH",S},
                    { "AH_PROVINZ",clienteAlfak.PROVINZ},
                    { "AL_PROVINZ",S},{ "AR_PROVINZ",S},
                    { "AH_LIEFER_KZ",0},{ "KO_PREISDRUCK",1},{ "HK_MATGEMKOST1",0 },
                    { "MOD",1},{ "KURS_FIX",0},{ "UMS_VERTR2",0},
                    { "TOUREN_RANGFOLGE",0},
                    { "HK_MATGEMKOST2",0},{ "HK_MATGEMKOST3",0},{ "HK_LOHNNEBKOST",0},
                    { "AH_MAIL",S},
                    { "DOC_CLIP_ID",0},
                    { "DATUM_ANLIEFERUNG",Pedido.F_Entrega},
                    { "FW_ART",0},{ "CONTRACT",0},{ "CLAIM",0},{ "CLAIM_ORDER",0},
                    { "FREMD_KEY",S},
                    { "HK_VOLLGEWINN",0},{ "HK_VERWGEMKOST",0},{ "HK_VERTRGEMKOST",0},{ "HK_GEWINN",0},
                    { "HK_SONZU2",0},{ "FI_BETR_EK3",0},{ "HK_VOLLEK1",0},{ "HK_VOLLEK2",0},
                    { "HK_VOLLEK3",0},{ "HK_VOLLSON",0},{ "HK_VOLLVERW",0},{ "HK_VOLLVERT",0},
                    { "HK_VOLLSONZU1",0},{ "HK_VOLLSONZU2",0},{ "KZ_INDIV_KUNDE",0},{ "HK_AKTIV",0},
                    { "AH_ARCHITECT",0},{ "NR_RECHNUNG",0},{ "KO_FALZ",0},
                    { "SU_STUECK",Pedido.CanTotal},{ "SU_STUECK_ISO",Pedido.CanTotal},
                    { "SZR_PRODART",Ind},{ "SZR_PRODGRP",Ind},
                    { "SZR_VARIANTE",0},
                    { "ETIK_LAYOUT",clienteAlfak.ETIK_LAYOUT},
                    { "FER_RAHMENNR",0},{ "ZUSCHLAG_BIT",0},{ "MINMENGE_EK",0},{ "MASSRUNDUNG",0},{ "MASSRUNDUNG_EK",0},
                    { "KATEGORIE",2},{ "VAR_DATA",S},
                    { "KO_PRIVAT",0},{ "FI_MWST3_ID",0},
                    { "FI_MWST3",0},{ "FI_MWST3_BASIS",0},{ "FI_MWST3_BASIS_FW",0},{ "FI_BETR_MWST3",0},
                    { "FI_BETR_MWST3_FW",0},{ "FI_MWST4_ID",0},{ "FI_MWST4",0},{ "FI_MWST4_BASIS",0},
                    { "FI_MWST4_BASIS_FW",0},{ "FI_BETR_MWST4",0},{ "FI_BETR_MWST4_FW",0},{ "FI_MWST5_ID",0},
                    { "FI_MWST5",0},{ "FI_MWST5_BASIS",0},{ "FI_MWST5_BASIS_FW",0},{ "FI_BETR_MWST5",0},
                    { "FI_BETR_MWST5_FW",0},{ "FI_TEILZAHL_BETR1",0},{ "FI_TEILZAHL_BETR2",0},{ "FI_TEILZAHL_BETR3",0},
                    { "FI_TEILZAHL_BETR4",0},{ "FI_TEILZAHL_BETR5",0},{ "FI_TEILZAHL_BETR6",0},{ "FI_TEILZAHL_BETR7",0},
                    { "PRINTGUID1",S},{ "PRINTGUID2",S},{ "PRINTGUID3",S},
                    { "PRINTSEQ1",0},{ "PRINTSEQ2",0},{ "PRINTSEQ3",0},{ "FI_KALK_FRACHTK",0},
                    { "TRANSPORT_ID",0},{ "TRANSPORT_RESPONSE",0},{ "INVOICE_CANCELED",0},{ "VALOR_UNITARIO_MODE",0},
                    { "HASH_CODE",S},{ "HASH_CODE_DELIVERY",S},
                    { "PROZ_ERFOLG",0}

                };
                /*insert en SYSADM.BW_AUFTR_KOPF*/
                 insert= new InsertRow("ALFAK", "SYSADM.BW_AUFTR_KOPF", Htable);

                if (insert.Insertado)
                {

                    Htable = new Hashtable
                    {
                        {"ID",NroPedido },
                        {"OTR_ROUTE",0 },
                        { "OTR_SEQUENCE",0},
                        { "OTR_STATUS",0},
                        { "USER_FIELD1",S},
                        { "USER_FIELD2",S},
                        { "USER_FIELD3",S},
                        { "DATUM_VORLAGE",hoy},
                        { "DISPATCHORDERNO",S},
                        { "ECOMMERCENO",S},
                        { "LIEFTERM_GRND",S}

                    };

                    /*insert en SYSADM.BW_AUFTR_KOPF*/
                    insert = new InsertRow("ALFAK", "SYSADM.BW_AUFTR_KOPF_EX", Htable);
                    if (insert.Insertado)
                    {
                        /*insert en insert en SYSADM.BW_AUFTR_HIST*/
                        
                        Htable = new Hashtable {
                            { "ID",NroPedido},
                            { "REF_ID",0},
                            { "STATUS_ID",1},
                            { "PUNKT_ID",1},
                            { "BEARBEITER",Ecom},
                            { "DATUM",DateTime.Now},
                            { "BEM", "Cliente: " + clienteAlfak.ID + ", Saldo: 0"}
                        };

                        
                        insert = new InsertRow("ALFAK", "SYSADM.BW_AUFTR_HIST", Htable);

                        Htable = new Hashtable {
                            { "ID",NroPedido},
                            { "REF_ID",0},
                            { "STATUS_ID",2},
                            { "PUNKT_ID",2},
                            { "BEARBEITER",Ecom},
                            { "DATUM",DateTime.Now},
                            { "BEM", " "}
                        };

                        
                        insert = new InsertRow("ALFAK", "SYSADM.BW_AUFTR_HIST", Htable);

                        Htable = new Hashtable {
                            { "ID",NroPedido},
                            { "REF_ID",0},
                            { "STATUS_ID",STATUS},
                            { "PUNKT_ID",STATUS},
                            { "BEARBEITER","SYSADM"},
                            { "DATUM",DateTime.Now},
                            { "BEM", "Num.: 14"}
                        };

                        
                        insert = new InsertRow("ALFAK", "SYSADM.BW_AUFTR_HIST", Htable);


                    }
                }

                return insert.Insertado; 
               
                
            }

            private bool Insert_ItemySub(PedDet Item,int C)
            {
                
                
                
                Double PRECIOXM2 = Math.Round(Item.NETOUN / Item.M2UN, 2);
                ProductoAlfak.Get PALF = new ProductoAlfak.Get(Item.ALFAKCODE);
                string Descr;
                if (Item.DESCRIPCION.Length>59)
                {
                    Descr = Item.DESCRIPCION.Substring(0,59);

                }
                else
                {
                    Descr = Item.DESCRIPCION;
                }
                
                Htable = new Hashtable {
                    { "ID",NroPedido},
                    { "POS_NR",C},
                    { "POS_TEXT",S},
                    { "POS_GRUPPE",C},
                    { "POS_KOMMISSION",Item.REFERENCIA},
                    { "POS_KUNDENPOS",S},
                    { "POS_BIT",0},{ "POS_STTXT_NR",0},{ "POS_STATUS",0},{ "POS_AUFTRINFO",0},{ "POS_LIEFERANT",0},
                    { "PROD_ID",Item.ALFAKCODE},
                    { "PROD_BEZ1",Descr},
                    { "PROD_BEZ2",S},{ "PROD_BEZ3",S},{ "PROD_KURZ_BEZ",S},{ "PROD_FARB_BEZ",S},
                    { "PROD_WGR",PALF.BA_PRODUKTE.Id_familiaAlfak},{ "PROD_WGR_STAT",PALF.BA_PRODUKTE.Id_familiaAlfak},
                    { "PROD_KMB",PALF.BA_PRODUKTE.Id_familiaAlfak},{ "PROD_KMB_STAT",PALF.BA_PRODUKTE.Id_familiaAlfak},
                    { "PROD_MEEINHEIT",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "SPRACH_BASIS",0},{ "PROD_KOMONR",0},
                    { "PROD_PRODART",PALF.BA_PRODUKTE.ARTPRDNR},{ "PROD_PRODGRP",PALF.BA_PRODUKTE.PRDKTGRPNR},
                    { "PR_LISTE",1},{ "PR_SCHLUESSEL",1},{ "PR_EINHEIT",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "PR_LISTE_EK",3},{ "PR_SCHLUESSEL_EK",5},
                    { "PR_EINHEIT_EK",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "PP_LFM", Item.PERIMUN},/*PERIMETRO UNIT*/
                    { "PP_GEWICHT",Item.KGUN},/*KG UNIT*/
                    { "PP_GEWICHT_TARA",0 },
                    { "PP_QM_FAKT",Item.M2UN},
                    { "PP_LFM_FAKT",Item.PERIMUN},
                    { "PP_DICKE",Item.ESPESORUN},/*ESPESOR*/
                    { "PP_FALZ",0},
                    { "PP_PLANSTK",0},
                    { "PP_ORIG_MENGE",Item.CANT},
                    { "PP_TEILGEL_MENGE",0},
                    { "FI_PREIS_ME",PRECIOXM2},
                    { "FI_PREIS_ME_FW",PRECIOXM2},
                    { "FI_RABATT1",0},
                    { "FI_BRUTTO",Item.NETOUN},
                    { "KZ_LAGER",0},{ "KZ_BESTELLUNG",0},
                    { "FI_KZ_SKONTO",1},
                    { "FI_BRUTTO_FW",Item.NETOUN},
                    { "FI_NETTO",Item.NETOUN},
                    { "FI_NETTO_FW",Item.NETOUN},
                    { "FI_NETTO_GES",Item.NETOITM},
                    { "FI_NETTO_GES_FW",Item.NETOITM},
                    { "FI_POS_STK",Item.NETOUN},
                    { "FI_POS_STK_FW",Item.NETOUN},
                    { "FI_POS_GES",Item.NETOITM},
                    { "FI_POS_GES_FW",Item.NETOITM},
                    { "FI_FESTPREIS",0},
                    { "EK_QM",Item.M2UN},
                    { "EK_QM_FAKT",Item.M2UN},
                    { "EK_LFM",Item.PERIMUN},
                    { "EK_LFM_FAKT",Item.PERIMUN},
                    { "FI_KZ_STEUER",1},
                    { "EK_PREIS_ME",0},
                    { "EK_RABATT",0},
                    { "EK_BRUTTO",0},
                    { "EK_NETTO",0},
                    { "EK_NETTO_GES",0},
                    { "EK_FESTPREIS",0},
                    { "EK_POS_STK",Item.CMPUN},
                    { "EK_POS_GES", Item.CMPUN*Item.CANT},
                    { "EK_ZEIT",0},
                    { "FI_VK_URSPRUNG",Item.NETOITM},
                    { "MISCHFAKTOR1",0},{ "MISCHFAKTOR2",0},{ "MISCHFAKTOR3",0},
                    { "FER_LAUF1",0},{ "FER_LINIE",0},{ "KZ_SERIE",0},{ "FER_UVRAND",0},
                    { "FER_KSCHUTZ",0},{ "FER_STRUKTURVERL",0},{ "FER_STRUKTURSEITE",0},{ "FER_RAHMENNR",0},
                    { "FER_RAHMENTEXT",S},
                    { "PREISRELEVANT",1},
                    { "PROD_LAGERORT",0},{ "LAGER_IDENT",0},{ "PROVISIONSSATZ",0},{ "FER_GEST_TYP",0},{ "FER_GEST_ANZ",0},
                    { "REKLA_ORT",Ind},{ "REKLA_GRUND",Ind},
                    { "MISCH1",0},{ "MISCH2",0},{ "MISCH3",0},
                    { "LAG_MIN_BEST",0},{ "LAG_BEST_MENGE",0},{ "EK_ZEIT_STK",0},
                    { "KOSTENSTELLE",Ind},
                    { "KOSTENART",Ind},
                    { "PROVISIONSSATZ2",0},{ "KZ_AUTOZUSCHN",0},
                    { "POS_TEXT1",S},{ "POS_TEXT2",S},{ "POS_TEXT3",S},{ "POS_TEXT4",S},{ "POS_TEXT5",S},
                    { "AUFTR_REF",0},/**/
                    { "POS_REF",0},
                    { "RUECKSCHNITT",10},
                    { "MINMENGE",0},{ "HASH",0},{ "PMGRP",0},
                    { "PROD_GESTELLNR",S},
                    { "HK_MATGEMKOST",0},{ "GRUPPE",0},{ "KZ_RANDENT",0},{ "LAG_LAGER_ID",0},
                    { "GRENZTYP1",0},{ "GRENZTYP2",0},{ "GRENZTYP3",0},{ "PACKREGEL",0},
                    { "POS_DOCK",S},
                    { "PMG_DEF",0},{ "ITM_REGEL_ID",0},{ "ALTERNATIV",0},{ "GRENZTYP4",0},{ "HK_VOLLGEWINN",0},
                    { "PREISRELEVANTEK",1},
                    { "POS_BIT2",0},{ "HK_VERWGEMKOST",0},{ "SKIZZEN_DRUCK",2},
                    { "POS_BLOCK",0},{ "FER_CUTTING_ONLY",0},{ "HK_VERTRGEMKOST",0},{ "VERS_BIT",0},
                    { "FER_GEST_NR",S},
                    { "BOHR_BEZUG",0},{ "EK_LIEFERANT",0},{ "PREIS_BIT",0},{ "HK_GEWINN",0},
                    { "HK_SONZU1",0},{ "HK_SONZU2",0},{ "HK_EK1",0},{ "HK_EK2",0},
                    { "HK_EK3",0},{ "HK_VOLLEK1",0},{ "HK_VOLLEK2",0},{ "HK_VOLLEK3",0},
                    { "HK_VOLLSON",0},{ "HK_VOLLVERW",0},{ "HK_VOLLVERT",0},
                    { "HK_VOLLSONZU1",0},{ "HK_VOLLSONZU2",0},{ "HK_VERLUST",0},{ "HK_VOLLSELBKOST",0},{ "FER_BESCHAFFARTNR",0},
                    { "FER_BESCHAFFARTTYP",2},{ "FER_BESCHICH_SEITE",0},
                    { "KZ_FIXIERT",0},
                    { "DATUM",hoy},
                    { "MITARB_ID",Ecom},
                    { "POS_VERPACKUNG",Ind},
                    { "MINPREIS_PWD",S},
                    { "PP_MENGE",Item.CANT},
                    { "PP_BREITE",Item.ANCHO},
                    { "PP_HOEHE",Item.ALTO},
                    { "PP_QM",Item.M2UN},
                    { "POS_BIT3",0},
                    { "AUFBAU_ID",260},
                    { "ISOAUFBAUKEY",0},{ "FI_ZUBASIS",0},{ "LIORDER_NR",0},
                    { "LIORDER_VSG",S},
                    { "PROD_ZUSCHLAG_BIT",36864},
                    { "RESTERROR",0},
                    { "FI_BETR_MWST1",0},{ "FI_BETR_MWST2",0},{ "FI_BETR_MWST1_FW",0},{ "FI_BETR_MWST2_FW",0},
                    { "CE_KZ",S},{ "PR_GRUPPE",S},
                    { "FI_POS_GES_MAN",0},{ "FI_POS_GES_BIT",0},
                    { "FI_POS_GES_BEM",S},{ "FI_POS_GES_RAB",0},{ "CE_FLAG",0},{ "CE_CPIP",S},
                    { "CE_LEVEL",0},{ "CE_BIT",0},{ "FI_ITEM_MWST1",0},{ "FI_ITEM_MWST2",0},
                    { "FI_ITEM_MWST1_FW",0},{ "FI_ITEM_MWST2_FW",0},{ "MINMENGE_EK",0},{ "MASSRUNDUNG",0},{ "MASSRUNDUNG_EK",0},
                    { "MAKRO_NAME",S},
                    { "PROD_FARB_ID",0},
                    { "VAR_DATA",S},
                    { "VPOS_NR",C},
                    { "EK_AUTO",0},{ "FI_AUTO",0},{ "FI_AUTO_FW",0},{ "EK_ZUBASIS",0},{ "FI_POS_GES_AB",0},
                    { "PREIS_AEN_BENUTZER",S},
                    { "REF_BEST_ID",S},
                    { "REF_BEST_POS",S},
                    { "REF_AUSLIEF_AUFTR_ID",NroPedido},
                    { "REF_AUSLIEF_AUFTR_POS",C},
                    { "REF_ENDKUNDE_BEST_ID",S},
                    { "REF_ENDKUNDE_BEST_POS",S},
                    { "SKIZZEN_DRUCK_SPR",0},{ "FI_MWST1",0},{ "FI_MWST2",0},{ "FI_MWST3",0},
                    { "FI_MWST4",0},{ "FI_MWST5",0},{ "FI_BETR_MWST3",0},{ "FI_BETR_MWST3_FW",0},
                    { "FI_ITEM_MWST3",0},{ "FI_ITEM_MWST3_FW",0},{ "FI_BETR_MWST4",0},{ "FI_BETR_MWST4_FW",0},
                    { "FI_ITEM_MWST4",0},{ "FI_ITEM_MWST4_FW",0},{ "FI_BETR_MWST5",0},{ "FI_BETR_MWST5_FW",0},
                    { "FI_ITEM_MWST5",0},{ "FI_ITEM_MWST5_FW",0},{ "EK_AUTO_STAT",0},{ "FI_AUTO_STAT",0},
                    { "PP_UEBERMENGE",0},{ "PP_UNTERMENGE",0},{ "PP_WUNSCHMENGE",0},
                    { "EK_PREIS_ME_BASE",0},{ "EK_AUTO_EK",0},
                };

                insert = new InsertRow("ALFAK", "SYSADM.BW_AUFTR_POS", Htable);
                if (insert.Insertado)
                {
                    subs = new PedSubDet.GetSubs(Item.IDPEDIDO, Item.IDPEDDET, Item.ESTADO,1);
                    if (subs.IsSuccess)
                    {
                        foreach (PedSubDet Sub in subs.Items)
                        {
                            if (!InsertSUB(Sub,Item.PERIMUN,Item.M2UN,Item.ANCHO,Item.ALTO,C))
                            {
                                Htable = new Hashtable {
                                        { "ErrorString","Error ingreso de Subitem en alfak IDPEDSUBDET=" +Sub.IDPEDSUBDET},
                                        { "URL","Archivo:IngresoPedido.cs,Namespace:Alfak,Clase:PedidoAlfak.Generate,Linea:80"},
                                        { "UserName"," "},
                                        { "DateTime",DateTime.Now}
                                };
                                insert = new InsertRow("PLABAL", "App_Error_Catch", Htable);
                            } 
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }

            }

            private bool Insert_ItemTINTERMO(PedDet Item,int C)
            {

                Htable = new Hashtable {
                    { "NRODOCU",NroPedido},
                    { "FEMDOCU",hoy},
                    { "KOEN",Pedido.RUT},
                    { "SUEN",S},
                    { "KOFU","CMP"},
                    { "KOPR",S},
                    { "KOMODE",Item.KOMODE},
                    { "CANTIDAD",Item.CANT},
                    { "PRECIO",Item.NETOUN},
                    { "MODO","$"},
                    { "FEERLI",Pedido.F_Entrega},
                    { "IDMAEEDO",0},
                    { "IDMAEDDO",0},
                    { "ESTADO",S},
                    { "PESOUBIC",Item.KGUN},
                    { "ANCHO",Item.ANCHO},
                    { "LARGO",Item.ALTO},
                    { "TIPOCOLOR",S}, /*-------------------------------------------------------VALIDAR PARA LOS TEMPLADOS*/
                    { "ESPESO",S}, /*-------------------------------------------------------VALIDAR PARA LOS TEMPLADOS*/
                    { "CODCLI",S},
                    { "PLANOPROD",S},
                    { "FORMA_BOTT",0},
                    { "PERFORA",0},
                    { "DESTAJE",0},
                    { "ALAMINA",S},
                    { "LLAMINA",S},
                    { "VIDRIO_CE","0000000"},
                    { "TPSEPARA22",S},
                    { "ANCHOSEP2",0},
                    { "FACT_PORTE",1},
                    { "SIPOLISUL",0},
                    { "PED_ALFAK",NroPedido},
                    { "ITEM_ALFAK",C},
                    { "PDTO_ALFAK",Item.ALFAKCODE},
                    { "TP_TVH_COR",0},
                    { "TP_DVH_COR",0},
                    { "CORRE_ARQ",0},
                    { "MON_ALFAK","$"},
                    { "FPDIDO_ALF",hoy.Day.ToString() + hoy.Month.ToString()+hoy.Year.ToString()},
                    { "SUC_ALFAK",Sucursal},
                    { "REFALFAK1",Item.REFERENCIA},
                    { "REFALFAK2",S},
                    { "CODMP02","0000000000000"},
                    { "SKUCLIE",S},
                    { "MOTIVOS",0},

                    
                };

                GenColtintermo Col = new GenColtintermo(subs.Items);
                double ESPESOR = (double)Col.Tabla["ESPESOR"];
                Htable.Add("LTSUBIC",(Item.ANCHO*Item.ALTO*ESPESOR)/1000);
                Htable.Add("ESPESOR", ESPESOR);
                Htable.Add("CODMP01", Col.Tabla["CODMP01"]);
                Htable.Add("M2MP01", Col.Tabla["M2MP01"]);
                Htable.Add("VIDRIO_EX", Col.Tabla["VIDRIO_EX"]);
                Htable.Add("SIARGON", Col.Tabla["SIARGON"]);
                Htable.Add("SIVALVULA", Col.Tabla["SIVALVULA"]);
                

                if (Col.Tabla.ContainsKey("TPSEPARA12"))
                {
                    Htable.Add("TPSEPARA12", Col.Tabla["TPSEPARA12"]);
                    Htable.Add("ANCHOSEP", Col.Tabla["ANCHOSEP"]);
                    Htable.Add("SISILICON", Col.Tabla["SISILICON"]);
                }
                else
                {
                    Htable.Add("TPSEPARA12", S);
                    Htable.Add("ANCHOSEP", 0);
                    Htable.Add("SISILICON", 0);
                }

                if (Col.Tabla.ContainsKey("CODMP03"))
                {
                    Htable.Add("CODMP03", Col.Tabla["CODMP03"]);
                    Htable.Add("VIDRIO_IN", Col.Tabla["VIDRIO_IN"]);
                    Htable.Add("M2MP03", Col.Tabla["M2MP03"]);
                }
                else
                {
                    Htable.Add("CODMP03", "0000000000000");
                    Htable.Add("VIDRIO_IN", "0000000");
                    
                }


                insert = new InsertRow("RANDOM", "TINTERMO", Htable);

                return insert.Insertado;

            }

            private void UpdatetablaPedido()
            {

                object[] Param = new object[] { string.Join(",", NroPedido),Pedido.ID };
                string[] ParamName = new string[] {"NROS","IDPEDIDO" };
                UpdateRow update = new UpdateRow("PLABAL", "UPDATE ECOM_PEDIDOS SET ESTADO='ING',NROPEDIDO=@NROS WHERE ID=@IDPEDIDO",Param,ParamName);
            }

            private void IngresarEnPlabal()
            {

            }

            private bool InsertSUB(PedSubDet Sub,double PERIMUN,double M2UN,double ANCHO, double ALTO, int C)
            {
                ProductoAlfak.Get PALF = new ProductoAlfak.Get(Sub.ALFAKCODE);
                double PRECIOXM2 = Sub.CNETOUN / M2UN;
                Htable = new Hashtable {
                    { "ID",NroPedido},
                    { "POS_NR",C},
                    { "STL_BEZ",Sub.DESCRIPCION},
                    { "PROD_FARB_BEZ",S},{ "STL_KURZ_BEZ",S},
                    { "STL_STATUS",0},{ "STL_DRUCKPOS",0},{ "STL_MOD",0},{ "STL_BIT",0},
                    { "STL_PRODART",Sub.STL_PRODART},
                    { "STL_PRODGRP",Sub.STL_PRODGRP},
                    { "STL_WGR",Sub.STL_WGR},{ "STL_WGR_STAT",Sub.STL_WGR},{ "STL_KMB",Sub.STL_WGR},{ "STL_KMB_STAT",Sub.STL_WGR},
                    { "STL_LFM",PERIMUN},
                    { "STL_QM",M2UN},
                    { "STL_LFM_FAKT",PERIMUN},
                    { "STL_QM_FAKT",M2UN},
                    { "STL_STTXT_NR",0},{ "SPRACH_BASIS",0},{ "PR_RABATT",0},
                    { "EK_NETTO_GES",PRECIOXM2},
                    { "EK_ZEIT",0},
                    { "PR_PREIS_ME",PRECIOXM2},
                    { "KZ_LAGER",0},{ "KZ_BESTELLUNG",0},{ "KZ_AUTOZUSCHLAG",0},{ "ID_LIEFERANT",0},
                    { "FER_STRUKTURVERL",0},{ "FER_STRUKTURSEITE",0},{ "FER_REDUKTION",0},{ "FER_LAUF",0},
                    { "PR_LISTE",1},{ "PR_SCHLUESSEL",1},
                    { "PR_EINHEIT",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "PR_LISTE_EK",1},{ "PR_SCHLUESSEL_EK",1},
                    { "PR_EINHEIT_EK",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "PR_MEEINHEIT",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "PR_PREIS_ME_FW",PRECIOXM2},
                    { "PR_PREIS_OFFEN",2},
                    { "PR_PREISDRUCK",0},
                    { "PR_ZUSCHLAGART",0 },
                    { "PR_BETR_NETTO",Sub.CNETOUN},
                    { "PR_BETR_NETTO_FW",Sub.CNETOUN},
                    { "PR_BETR_BRUTTO",Sub.CNETOUN},
                    { "PR_BETR_BRUTTO_FW",Sub.CNETOUN},
                    { "PR_NETTO_GES",Sub.CNETOUN},
                    { "PR_NETTO_GES_FW",Sub.CNETOUN},
                    { "EK_QM",M2UN},
                    { "EK_QM_FAKT",M2UN},
                    { "EK_PREIS_ME",PRECIOXM2},
                    { "EK_RABATT",0},
                    { "EK_BRUTTO",PRECIOXM2},
                    { "EK_NETTO",PRECIOXM2},
                    { "FI_VK_URSPRUNG",0},
                    { "MINMENGE",0},
                    { "VER_PREIS_ME",PRECIOXM2},
                    { "VER_RABATT",0},
                    { "PREISRELEVANT",1},
                    { "PROD_LAGERORT",0},
                    { "VER_BETR_NETTO",PRECIOXM2},
                    { "BOM_PRODUKT", Sub.ALFAKCODE},
                    { "BOM_ID",Sub.POS_NR},
                    { "BOM_NODE",0},
                    { "BOM_POS",Sub.POS_NR},
                    { "BOM_LEVEL",1},
                    { "KOSTENSTELLE",Ind},/*KOMODE*/
                    { "KOSTENART",Ind},
                    { "KZ_AUTOZUSCHN",0},
                    { "KZ_SN3",0},
                    { "PREISRELEVANTEK",1},{ "PROD_RELEVANT",1},
                    { "BOM_MASTER_ID",0},{ "BEARB_INS",0},{ "LAG_LAGER_ID",0},{ "HASH",0},{ "STL_BIT2",0},
                    { "FI_KZ_STEUER",1},{ "FI_KZ_SKONTO",1},
                    { "VER_BETR_BRUTTO",Sub.CNETOUN},
                    { "FER_CUTTING_ONLY",0},
                    { "VER_NETTO_GES",Sub.CNETOUN},
                    { "VER_EINHEIT",PALF.BA_PRODUKTE.BA_MENGENEINH},
                    { "HK_MATGEMKOST",0},
                    { "HK_VERLUST",0},
                    { "FER_BESCHAFFARTNR",1},
                    { "FER_BESCHAFFARTTYP",2},
                    { "PREIS_BIT",0},
                    { "FER_BESCHICH_SEITE",0},
                    { "STL_MENGE",1},
                    { "STL_BREITE",ANCHO},
                    { "STL_HOEHE",ALTO},
                    { "STL_DICKE",Sub.ESPESORUN},
                    { "FI_ZUBASIS",0},
                    { "LIORDER_NR",0},
                    { "LIORDER_VSG",S},
                    { "STL_BIT3",0},
                    { "PROD_ZUSCHLAG_BIT",36864},
                    { "CE_CPIP",S},
                    { "CE_LEVEL",0},{ "CE_INFLUENCING",0},{ "FI_ITEM_MWST1",0},{ "FI_ITEM_MWST2",0},{ "FI_ITEM_MWST1_FW",0},
                    { "FI_ITEM_MWST2_FW",0},{ "MINMENGE_EK",0},{ "MASSRUNDUNG",0},{ "MASSRUNDUNG_EK",0},{ "MASS_BIT",0},
                    { "PROD_FARB_ID",0},{ "GRENZTYP1",0},{ "GRENZTYP2",0},{ "GRENZTYP3",0},{ "GRENZTYP4",0},
                    { "EK_AUTO",0},{ "FI_AUTO",0},{ "FI_AUTO_FW",0},{ "FI_MWST1",0},{ "FI_MWST2",0},
                    { "FI_MWST3",0},{ "FI_MWST4",0},{ "FI_MWST5",0},
                    { "FI_ITEM_MWST3",0},
                    { "FI_ITEM_MWST3_FW",0},
                    { "FI_ITEM_MWST4",0},
                    { "FI_ITEM_MWST4_FW",0},
                    { "FI_ITEM_MWST5",0},
                    { "FI_ITEM_MWST5_FW",0},
                    { "EK_AUTO_STAT",0},
                    { "FI_AUTO_STAT",0},
                    { "BOM_BASE_ID",C},
                    { "BOM_PUID",C},
                    { "PR_PREIS_ME_BASE",0},
                    { "EK_PREIS_ME_BASE",0},{ "EK_AUTO_EK",0},{ "POLINK_GUID",S}

                };

                insert = new InsertRow("ALFAK", "SYSADM.BW_AUFTR_STKL", Htable);

                return insert.Insertado;

            }
        }
        
        public class GenColtintermo
        {
            private readonly List<PedSubDet> subs;
            public Hashtable Tabla;

            public GenColtintermo(List<PedSubDet> _subs)
            {
                subs = _subs;
                ParaTINTERMO();

            }

            private void ParaTINTERMO()
            {

                Tabla = new Hashtable();
                bool Vidrioex = true;
                int ARGON = 0;
                int VALVE = 0;
                double ANC; double LAR;
                double ESPESOR = 0;
         


                foreach (PedSubDet sub in subs)
                {

                    ProductoAlfak.Get PALF = new ProductoAlfak.Get(sub.ALFAKCODE);
                    if (sub.STL_WGR == "H31")
                    {
                        string COLORSEP = PALF.BA_PRODUKTE.Abreviacion.Substring(0, 2);
                        if (COLORSEP=="SM")
                        {
                            COLORSEP = "MA";
                        }
                        else if (COLORSEP=="SB")
                        {
                            COLORSEP = "BR";
                        }
                        else if (COLORSEP=="ST")
                        {
                            COLORSEP = "TI";
                        }
                        Tabla.Add("TPSEPARA12",COLORSEP );
                        int Largostr = PALF.BA_PRODUKTE.Abreviacion.Length-2;
                        Tabla.Add("ANCHOSEP", Convert.ToInt32(PALF.BA_PRODUKTE.Abreviacion.Substring(2, Largostr)));
                        Tabla.Add("SISILICON", 1);
                       
                    }
                    else if (sub.ALFAKCODE == "399001")
                    {
                        ARGON = 1;
                    }
                    else if (sub.ALFAKCODE=="398001")
                    {
                        VALVE = 1;
                    }
                    else
                    {
                        if (Vidrioex)
                        {
                            Tabla.Add("CODMP01", PALF.BA_PRODUKTE.EAN);
                            Tabla.Add("VIDRIO_EX", PALF.BA_PRODUKTE.EAN.Substring(0, 7));
                            ANC = Convert.ToDouble(PALF.BA_PRODUKTE.EAN.Substring(7, 3));
                            LAR = Convert.ToDouble(PALF.BA_PRODUKTE.EAN.Substring(10, 3));
                            Tabla.Add("M2MP01", Math.Round((ANC / 100) * (LAR / 100), 3));
                            ESPESOR = ESPESOR + PALF.BA_PRODUKTE.BA_MASS_DICKE;
                            Vidrioex = false;
                        }
                        else
                        {
                            Tabla.Add("CODMP03", PALF.BA_PRODUKTE.EAN);
                            Tabla.Add("VIDRIO_IN", PALF.BA_PRODUKTE.EAN.Substring(0, 7));
                            ANC = Convert.ToDouble(PALF.BA_PRODUKTE.EAN.Substring(7, 3));
                            LAR = Convert.ToDouble(PALF.BA_PRODUKTE.EAN.Substring(10, 3));
                            Tabla.Add("M2MP03", Math.Round((ANC / 100) * (LAR / 100), 3));
                            ESPESOR = ESPESOR + PALF.BA_PRODUKTE.BA_MASS_DICKE;
                            

                        }
                    }

                }
                Tabla.Add("SIARGON", ARGON);
                Tabla.Add("SIVALVULA", VALVE);
                Tabla.Add("ESPESOR", ESPESOR);
                

            }
        }

        public class GetNroPedido
        {

            public readonly string Numero;
            public readonly bool IsGotta;

            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion

            public GetNroPedido()
            {
                Numero = GetSetLast();
                if (!string.IsNullOrEmpty(Numero))
                {
                    IsGotta = true;
                }
                else
                {
                    IsGotta = false;
                }

            }

            private string GetSetLast()
            {
                Query = "select AUFTRAG FROM SYSADM.BA_NUMKREISE WHERE AV_BEREICH='SANTIAGO'";
                
                try
                {
                    Conn = new Coneccion();
                    Conn.ConnAlfak.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnAlfak);
                    

                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        int Nro = Convert.ToInt32(dr[0].ToString()) + 1;
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        if (!Existe(Nro))
                        {
                            if (SetLast(Nro))
                            {
                                return Nro.ToString();
                            }
                            else
                            {
                                return null;
                            }
                            
                            
                        }
                        else
                        {
                            Nro++;
                            bool Seguir = true;
                            while (Seguir)
                            {
                                if (!Existe(Nro))
                                {
                                    Seguir = false;
                                    
                                }
                                else
                                {
                                    Nro++;
                                }
                            }

                            if (SetLast(Nro))
                            {
                                return Nro.ToString();
                            }
                            else
                            {
                                return null;
                            }
                            
                        }
                        
                    }
                    else
                    {
                        Conn.ConnPlabal.Close();
                        dr.Close();
                        return  null;
                    }
                    

                }
                catch (Exception)
                {
                    return null;
                    

                }
            }

            private bool Existe(int Last)
            {
                Query = "SELECT TOP 1 *  FROM PHGLASS.SYSADM.BW_AUFTR_KOPF WHERE ID=@ID";

                try
                {
                    Conn = new Coneccion();
                    Conn.ConnAlfak.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnAlfak);
                    Conn.Cmd.Parameters.AddWithValue("@ID", Last);


                    dr = Conn.Cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        int Nro = Convert.ToInt32(dr[0].ToString());
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

            private bool SetLast(int Last)
            {
                Last++;
                Query = "UPDATE SYSADM.BA_NUMKREISE SET AUFTRAG=@AUFTRAG WHERE AV_BEREICH='SANTIAGO'";

                Conn = new Coneccion();
                try
                {
                    Conn.ConnAlfak.Open();
                    Conn.Cmd = new SqlCommand(Query, Conn.ConnAlfak);
                    Conn.Cmd.Parameters.AddWithValue("@AUFTRAG", Last);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();
                    return true;
                    

                }
                catch (Exception EX )
                {
                    throw EX;
                    return false;
                    
                }

            }
        }

        
    }

    public class ClienteAlfak
    {
        #region Variables
        public string ID { get; set; }
        public string MCODE { get; set; }
        public string NAME1 { get; set; }
        public string STRASSE { get; set; }
        public string LAND { get; set; }
        public string ZAHLBED { get; set; }
        public string PROVINZ { get; set; }
        public string ADRESS_KOPF { get; set; }
        public string WAEHRUNG { get; set; }
        public string ZAHL_FB_KZ { get; set; }
        public string ETIK_LAYOUT { get; set; }
        #endregion

        public class Get
        {
            public ClienteAlfak Datos;
            public bool IsSuccess;


            public Get(string _RUT)
            {
                IsSuccess = Obtener(_RUT);
            }

            private bool Obtener(string RUT)
            {
                object[] Param = new object[] { RUT };
                string[] ParamN = new string[] { "RUT" };
                SelectRows select = new SelectRows("ALFAK", "SYSADM.KU_KUNDEN", "TOP 1 *", "UST_ID=@RUT", Param, ParamN);
                if (select.IsGot)
                {
                    
                    Datos = new ClienteAlfak {
                        ID = select.Datos.Rows[0]["ID"].ToString(),
                        MCODE= select.Datos.Rows[0]["MCODE"].ToString(),
                        NAME1= select.Datos.Rows[0]["NAME1"].ToString(),
                        STRASSE= select.Datos.Rows[0]["STRASSE"].ToString(),
                        LAND= select.Datos.Rows[0]["LAND"].ToString(),
                        ZAHLBED= select.Datos.Rows[0]["ZAHLBED"].ToString(),
                        PROVINZ= select.Datos.Rows[0]["PROVINZ"].ToString(),
                        ADRESS_KOPF= select.Datos.Rows[0]["ADRESS_KOPF"].ToString(),
                        WAEHRUNG= select.Datos.Rows[0]["WAEHRUNG"].ToString(),
                        ZAHL_FB_KZ= select.Datos.Rows[0]["ZAHL_FB_KZ"].ToString(),
                        ETIK_LAYOUT= select.Datos.Rows[0]["ETIK_LAYOUT"].ToString(),
                    };
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }

    public class ActualizaTabla
    {
        public ActualizaTabla()
        {
            GlasserExcel.LecturaExcel lectura = new GlasserExcel.LecturaExcel();

            Hashtable Htable;
            UpdateRow UPDATE;
            foreach (var codigo in lectura.Htable.Keys)
            {
                object[] DATOS = (object[])lectura.Htable[codigo];
                object[] Param;
                string[] ParamName;
                Htable = new Hashtable() {
                { "BA_PRODUKT",codigo },
                { "EAN", DATOS[0] },
                { "BA_MCODE",DATOS[1]},
                };

                UPDATE = new UpdateRow("ALFAK", "UPDATE SYSADM.BA_PRODUKTE SET EAN=@EAN,BA_MCODE=@BA_MCODE WHERE BA_PRODUKT=@BA_PRODUKT", Htable);
                if (!UPDATE.Actualizado)
                {
                    bool dsdd = false;
                }

                Param = new object[] { codigo,DATOS[3] };
                ParamName = new string[] {"BA_PRODUKT","PREIS" };
                UPDATE = new UpdateRow("ALFAK", "SYSADM.PR_PREIS", "PREIS=@PREIS", "LISTE_ID=3 AND SCHLS_ID=5 AND ID=@BA_PRODUKT",
                    Param, ParamName);
                if (!UPDATE.Actualizado)
                {
                    bool dsdd = false;
                }

                Param = new object[] { codigo, DATOS[2] };
                ParamName = new string[] { "BA_PRODUKT", "BA_BEZ1" };
                UPDATE = new UpdateRow("ALFAK", "SYSADM.BA_PRODUKTE_BEZ", "BA_BEZ1=@BA_BEZ1" ," BA_PRODUKT=@BA_PRODUKT",
                    Param, ParamName);
                if (!UPDATE.Actualizado)
                {
                    bool dsdd = false;
                }


            }
        }
    }

    /*tabla encabezado del pedido*/
    public class BW_AUFTR_KOPF
    {
        public string ID { get; set; }
        public int NR_LIEFERSCHEIN { get; set; }
        public double SU_LFM_REAL { get; set; }
        public int STATUS { get; set; }
        public DateTime DATUM_ERF { get; set; }
        public DateTime DATUM_AB { get; set; }
        public DateTime DATUM_LIEFERSCHEIN { get; set; }
        public DateTime DATUM_RECHNUNG { get; set; }
        public DateTime DATUM_PROD1 { get; set; }
        public DateTime DATUM_PROD2 { get; set; }
        public DateTime DATUM_PROD3 { get; set; }
        public DateTime DATUM_LIEFER_PLAN { get; set; }
        public DateTime DATUM_LIEFER_TAT { get; set; }
        public string DATUM_LIEFERWUNSCH { get; set; }
        public string PRIORITAET { get; set; }
        public DateTime DATUM_FAELLIGK { get; set; }
        public DateTime DATUM_BEST { get; set; }
        public int LAUF_PROD1 { get; set; }
        public int LAUF_PROD2 { get; set; }
        public int LAUF_PROD3 { get; set; }
        public string BEST_TEXT1 { get; set; }
        public string BEST_TEXT2 { get; set; }
        public int AH_HAUPT_AUFTR { get; set; }
        public int AH_IDENT { get; set; }
        public int AH_LIEFERANT { get; set; }
        public string AH_MCODE { get; set; }
        public string AH_KOPF { get; set; }
        public string AH_NAME1 { get; set; }
        public string AH_NAME2 { get; set; }
        public string AH_NAME3 { get; set; }
        public string AH_STRASSE { get; set; }
        public string AH_PLZ { get; set; }
        public string AH_ORT { get; set; }
        public string AH_LAND { get; set; }
        public string AH_TELEFON { get; set; }
        public string AH_FAX { get; set; }
        public string AH_ANREDE { get; set; }
        public string AH_PARTNER { get; set; }
        public int AH_BIT1 { get; set; }
        public int AL_IDENT { get; set; }
        public string AL_KOPF { get; set; }
        public string AL_NAME1 { get; set; }
        public string AL_NAME2 { get; set; }
        public string AL_NAME3 { get; set; }
        public string AL_STRASSE { get; set; }
        public string AL_PLZ { get; set; }
        public string AL_ORT { get; set; }
        public string AL_LAND { get; set; }
        public string AL_TELEFON { get; set; }
        public string AL_FAX { get; set; }
        public string AL_ANREDE { get; set; }
        public string AL_PARTNER { get; set; }
        public int AR_IDENT { get; set; }
        public string AR_KOPF { get; set; }
        public string AR_NAME1 { get; set; }
        public string AR_NAME2 { get; set; }
        public string AR_NAME3 { get; set; }
        public string AR_STRASSE { get; set; }
        public string AR_PLZ { get; set; }
        public string AR_ORT { get; set; }
        public string AR_LAND { get; set; }
        public string AR_TELEFON { get; set; }
        public string AR_FAX { get; set; }
        public string AR_ANREDE { get; set; }
        public string AR_PARTNER { get; set; }
        public int OR_SPRACH_ID { get; set; }
        public int OR_SPRACH_BASIS { get; set; }
        public int OR_MANDANT { get; set; }
        public string OR_AVBEREICH { get; set; }
        public string OR_BEARBEITER { get; set; }
        public string OR_FACHBERATER { get; set; }
        public string OR_GESCHART { get; set; }
        public string OR_SPERRKZ { get; set; }
        public string OR_ADIENST { get; set; }
        public string OR_VERPACKUNG { get; set; }
        public string OR_LIEFERBED { get; set; }
        public string OR_TOUR { get; set; }
        public string OR_AWTOUR { get; set; }
        public string OR_FAHRER { get; set; }
        public string OR_ZOLLTOUR { get; set; }
        public string OR_GRUPPE { get; set; }
        public int KO_MASSEINH { get; set; }
        public double SU_LFM_FAKT { get; set; }
        public int KO_OBJEKT_KUNDE { get; set; }
        public int KO_OBJEKT_LIEF { get; set; }
        public int KO_SAMMELRE { get; set; }
        public int KO_TEILFAK { get; set; }
        public int KO_TEILLIEF { get; set; }
        public double ENTFERNUNG { get; set; }
        public int KO_NETTOPREISE { get; set; }
        public int KO_FAXVERSAND { get; set; }
        public string OR_ADIENST2 { get; set; }
        public double SU_QM_REAL { get; set; }
        public double SU_QM_FAKT { get; set; }
        public double SU_GEWICHT { get; set; }
        public double SU_GEWICHT_TARA { get; set; }
        public double SU_STUNDEN { get; set; }
        public double SU_KM { get; set; }
        public double SU_SPRLFM_FAKT { get; set; }
        public double SU_SPRLFM_REAL { get; set; }
        public double FI_VALUTAKURS { get; set; }
        public double FI_MWST1 { get; set; }
        public double FI_MWST2 { get; set; }
        public double FI_MWST1_BASIS { get; set; }
        public string FI_ZAHLBED { get; set; }
        public string FI_ZAHLWEG { get; set; }
        public string FI_WAEHRUNG { get; set; }
        public double FI_MWST2_BASIS { get; set; }
        public double FI_MWST1_BASIS_FW { get; set; }
        public double FI_MWST2_BASIS_FW { get; set; }
        public int FI_MWST1_ID { get; set; }
        public int FI_MWST2_ID { get; set; }
        public double FI_RABATT1 { get; set; }
        public double FI_BETR_NETTO { get; set; }
        public double FI_BETR_BRUTTO { get; set; }
        public double FI_BETR_NETTO_FW { get; set; }
        public string FI_UST_ID { get; set; }
        public int FI_FB_KZ { get; set; }
        public int FI_RLEG_TAG { get; set; }
        public int FI_ZAHL_TAG1 { get; set; }
        public double FI_BETR_BRUTTO_FW { get; set; }
        public double FI_BETR_FESTPREIS { get; set; }
        public double FI_BETR_MWST1 { get; set; }
        public double FI_BETR_MWST2 { get; set; }
        public double FI_BETR_MWST1_FW { get; set; }
        public double FI_BETR_MWST2_FW { get; set; }
        public double FI_BETR_ZWSUMME2 { get; set; }
        public double FI_BETR_ZWSU1_FW { get; set; }
        public double FI_BETR_ZWSU2_FW { get; set; }
        public double FI_BETR_EK1 { get; set; }
        public double FI_BETR_EK2 { get; set; }
        public double FI_SKONTO_BASIS { get; set; }
        public double FI_SKONTO_BASIS_FW { get; set; }
        public double FI_BETR_SKONTO { get; set; }
        public double FI_BETR_SKONTO_FW { get; set; }
        public double FI_BETR_PROV { get; set; }
        public double FI_BETR_ZWSUMME1 { get; set; }
        public double EK_BETR_FESTPREIS { get; set; }
        public double FI_BETR_FESTPR_FW { get; set; }
        public double MINMENGE { get; set; }
        public int BIT { get; set; }
        public int ETIKETTEN_TYP { get; set; }
        public string DOK_TYP { get; set; }
        public int FI_ZAHL_TAG2 { get; set; }
        public int FI_ZAHL_TAG3 { get; set; }
        public int LADELISTE { get; set; }
        public string OR_LKW { get; set; }
        public string AH_PLZ_POSTFACH { get; set; }
        public string AH_POSTFACH { get; set; }
        public string AR_PLZ_POSTFACH { get; set; }
        public string AR_POSTFACH { get; set; }
        public string AH_PROVINZ { get; set; }
        public string AL_PROVINZ { get; set; }
        public string AR_PROVINZ { get; set; }
        public int AH_LIEFER_KZ { get; set; }
        public int KO_PREISDRUCK { get; set; }
        public double HK_MATGEMKOST1 { get; set; }
        public int MOD { get; set; }
        public int KURS_FIX { get; set; }
        public int UMS_VERTR2 { get; set; }
        public int TOUREN_RANGFOLGE { get; set; }
        public double HK_MATGEMKOST2 { get; set; }
        public double HK_MATGEMKOST3 { get; set; }
        public double HK_LOHNNEBKOST { get; set; }
        public string AH_MAIL { get; set; }
        public int DOC_CLIP_ID { get; set; }
        public DateTime DATUM_ANLIEFERUNG { get; set; }
        public int FW_ART { get; set; }
        public int CONTRACT { get; set; }
        public int CLAIM { get; set; }
        public int CLAIM_ORDER { get; set; }
        public string FREMD_KEY { get; set; }
        public string STEUERNUMMER { get; set; }
        public double HK_VOLLGEWINN { get; set; }
        public double HK_VERWGEMKOST { get; set; }
        public double HK_VERTRGEMKOST { get; set; }
        public double HK_GEWINN { get; set; }
        public double HK_SONZU2 { get; set; }
        public double FI_BETR_EK3 { get; set; }
        public double HK_VOLLEK1 { get; set; }
        public double HK_VOLLEK2 { get; set; }
        public double HK_VOLLEK3 { get; set; }
        public double HK_VOLLSON { get; set; }
        public double HK_VOLLVERW { get; set; }
        public double HK_VOLLVERT { get; set; }
        public double HK_VOLLSONZU1 { get; set; }
        public double HK_VOLLSONZU2 { get; set; }
        public int KZ_INDIV_KUNDE { get; set; }
        public int HK_AKTIV { get; set; }
        public int AH_ARCHITECT { get; set; }
        public double NR_RECHNUNG { get; set; }
        public double KO_FALZ { get; set; }
        public double SU_STUECK { get; set; }
        public double SU_STUECK_ISO { get; set; }
        public string SZR_PRODART { get; set; }
        public string SZR_PRODGRP { get; set; }
        public int SZR_VARIANTE { get; set; }
        public int ETIK_LAYOUT { get; set; }
        public int FER_RAHMENNR { get; set; }
        public int ZUSCHLAG_BIT { get; set; }
        public double MINMENGE_EK { get; set; }
        public int MASSRUNDUNG { get; set; }
        public int MASSRUNDUNG_EK { get; set; }
        public int KATEGORIE { get; set; }
        public string VAR_DATA { get; set; }
        public int KO_PRIVAT { get; set; }
        public int KO_STEUERFLAG { get; set; }
        public int FI_MWST3_ID { get; set; }
        public double FI_MWST3 { get; set; }
        public double FI_MWST3_BASIS { get; set; }
        public double FI_MWST3_BASIS_FW { get; set; }
        public double FI_BETR_MWST3 { get; set; }
        public double FI_BETR_MWST3_FW { get; set; }
        public int FI_MWST4_ID { get; set; }
        public double FI_MWST4 { get; set; }
        public double FI_MWST4_BASIS { get; set; }
        public double FI_MWST4_BASIS_FW { get; set; }
        public double FI_BETR_MWST4 { get; set; }
        public double FI_BETR_MWST4_FW { get; set; }
        public int FI_MWST5_ID { get; set; }
        public double FI_MWST5 { get; set; }
        public double FI_MWST5_BASIS { get; set; }
        public double FI_MWST5_BASIS_FW { get; set; }
        public double FI_BETR_MWST5 { get; set; }
        public double FI_BETR_MWST5_FW { get; set; }
        public double FI_TEILZAHL_BETR1 { get; set; }
        public double FI_TEILZAHL_BETR2 { get; set; }
        public double FI_TEILZAHL_BETR3 { get; set; }
        public double FI_TEILZAHL_BETR4 { get; set; }
        public double FI_TEILZAHL_BETR5 { get; set; }
        public double FI_TEILZAHL_BETR6 { get; set; }
        public double FI_TEILZAHL_BETR7 { get; set; }
        public DateTime FI_TEILZAHL_DATUM1 { get; set; }
        public DateTime FI_TEILZAHL_DATUM2 { get; set; }
        public DateTime FI_TEILZAHL_DATUM3 { get; set; }
        public DateTime FI_TEILZAHL_DATUM4 { get; set; }
        public DateTime FI_TEILZAHL_DATUM5 { get; set; }
        public DateTime FI_TEILZAHL_DATUM6 { get; set; }
        public DateTime FI_TEILZAHL_DATUM7 { get; set; }
        public string PRINTGUID1 { get; set; }
        public string PRINTGUID2 { get; set; }
        public string PRINTGUID3 { get; set; }
        public int PRINTSEQ1 { get; set; }
        public int PRINTSEQ2 { get; set; }
        public int PRINTSEQ3 { get; set; }
        public double FI_KALK_FRACHTK { get; set; }
        public int TRANSPORT_ID { get; set; }
        public int TRANSPORT_RESPONSE { get; set; }
        public int INVOICE_CANCELED { get; set; }
        public int VALOR_UNITARIO_MODE { get; set; }
        public string HASH_CODE { get; set; }
        public string ROWID { get; set; }
        public string HASH_CODE_DELIVERY { get; set; }
        public int CHANGED { get; set; }
        public int PROZ_ERFOLG { get; set; }

    }

    /*variables/parametros de formas y procesos*/
    public class BW_AUFTR_BEARB
    {
        public int ID { get; set; }
        public int POS_NR { get; set; }
        public double BEA_PARAM4 { get; set; }
        public double BEA_PARAM5 { get; set; }
        public double BEA_PARAM6 { get; set; }
        public double BEA_PARAM7 { get; set; }
        public double BEA_PARAM8 { get; set; }
        public double BEA_PARAM9 { get; set; }
        public double BEA_PARAM10 { get; set; }
        public double BEA_PARAM11 { get; set; }
        public double BEA_PARAM12 { get; set; }
        public double BEA_PARAM13 { get; set; }
        public double BEA_PARAM14 { get; set; }
        public double BEA_PARAM15 { get; set; }
        public double BEA_PARAM16 { get; set; }
        public double BEA_PARAM17 { get; set; }
        public string BEA_TEXT { get; set; }
        public int BOM_ID { get; set; }
        public double BEA_PARAM18 { get; set; }
        public double BEA_PARAM19 { get; set; }
        public double BEA_PARAM20 { get; set; }
        public string BEA_TEXT_ORIG { get; set; }
        public double BEA_ANZAHL { get; set; }
        public double BEA_PARAM1 { get; set; }
        public double BEA_PARAM2 { get; set; }
        public double BEA_PARAM3 { get; set; }
        public int MASS_BIT { get; set; }
        public string SN_MAKRO_NAME { get; set; }
        public double BEA_PARAM21 { get; set; }
        public double BEA_PARAM22 { get; set; }
        public double BEA_PARAM23 { get; set; }
        public double BEA_PARAM24 { get; set; }
        public double BEA_PARAM25 { get; set; }
        public double BEA_PARAM26 { get; set; }
        public double BEA_PARAM27 { get; set; }
        public double BEA_PARAM28 { get; set; }
        public double BEA_PARAM29 { get; set; }
        public double BEA_PARAM30 { get; set; }
        public string BEA_TEXT_FOREIGN { get; set; }
        public string ROWID {get;set;}
        public double EDGE_ZUSCHL1 { get; set; }
        public double EDGE_ZUSCHL2 { get; set; }
        public double EDGE_ZUSCHL3 { get; set; }
        public double EDGE_ZUSCHL4 { get; set; }
        public double EDGE_ZUSCHL5 { get; set; }
        public double EDGE_ZUSCHL6 { get; set; }
        public double EDGE_ZUSCHL7 { get; set; }
        public double EDGE_ZUSCHL8 { get; set; }
        public int ANZ_ZUSCHL_WAAG { get; set; }
        public int ANZ_ZUSCHL_SENK { get; set; }
    }

    /*Items del pedido*/
    public class BW_AUFTR_POS
    {
        public int ID { get; set; }
        public int POS_NR { get; set; }
        public string POS_TEXT { get; set; }
        public int POS_GRUPPE { get; set; }
        public string POS_KOMMISSION { get; set; }
        public string POS_KUNDENPOS { get; set; }
        public int POS_BIT { get; set; }
        public int POS_STTXT_NR { get; set; }
        public int POS_STATUS { get; set; }
        public int POS_AUFTRINFO { get; set; }
        public int POS_LIEFERANT { get; set; }
        public int PROD_ID { get; set; }
        public string PROD_BEZ1 { get; set; }
        public string PROD_BEZ2 { get; set; }
        public string PROD_BEZ3 { get; set; }
        public string PROD_KURZ_BEZ { get; set; }
        public string PROD_FARB_BEZ { get; set; }
        public string PROD_WGR { get; set; }
        public string PROD_WGR_STAT { get; set; }
        public string PROD_KMB { get; set; }
        public string PROD_KMB_STAT { get; set; }
        public string PROD_MEEINHEIT { get; set; }
        public int SPRACH_BASIS { get; set; }
        public int PROD_KOMONR { get; set; }
        public int PROD_PRODART { get; set; }
        public int PROD_PRODGRP { get; set; }
        public int PR_LISTE { get; set; }
        public int PR_SCHLUESSEL { get; set; }
        public string PR_EINHEIT { get; set; }
        public int PR_LISTE_EK { get; set; }
        public int PR_SCHLUESSEL_EK { get; set; }
        public string PR_EINHEIT_EK { get; set; }
        public double PP_LFM { get; set; }
        public double PP_GEWICHT { get; set; }
        public double PP_GEWICHT_TARA { get; set; }
        public double PP_QM_FAKT { get; set; }
        public double PP_LFM_FAKT { get; set; }
        public double PP_DICKE { get; set; }
        public double PP_FALZ { get; set; }
        public double PP_PLANSTK { get; set; }
        public double PP_ORIG_MENGE { get; set; }
        public double PP_TEILGEL_MENGE { get; set; }
        public double FI_PREIS_ME { get; set; }
        public double FI_PREIS_ME_FW { get; set; }
        public double FI_RABATT1 { get; set; }
        public double FI_BRUTTO { get; set; }
        public int KZ_LAGER { get; set; }
        public int KZ_BESTELLUNG { get; set; }
        public int FI_KZ_SKONTO { get; set; }
        public double FI_BRUTTO_FW { get; set; }
        public double FI_NETTO { get; set; }
        public double FI_NETTO_FW { get; set; }
        public double FI_NETTO_GES { get; set; }
        public double FI_NETTO_GES_FW { get; set; }
        public double FI_POS_STK { get; set; }
        public double FI_POS_STK_FW { get; set; }
        public double FI_POS_GES { get; set; }
        public double FI_POS_GES_FW { get; set; }
        public double FI_FESTPREIS { get; set; }
        public double EK_QM { get; set; }
        public double EK_QM_FAKT { get; set; }
        public double EK_LFM { get; set; }
        public double EK_LFM_FAKT { get; set; }
        public int FI_KZ_STEUER { get; set; }
        public double EK_PREIS_ME { get; set; }
        public double EK_RABATT { get; set; }
        public double EK_BRUTTO { get; set; }
        public double EK_NETTO { get; set; }
        public double EK_NETTO_GES { get; set; }
        public double EK_FESTPREIS { get; set; }
        public double EK_POS_STK { get; set; }
        public double EK_POS_GES { get; set; }
        public double EK_ZEIT { get; set; }
        public double FI_VK_URSPRUNG { get; set; }
        public double MISCHFAKTOR1 { get; set; }
        public double MISCHFAKTOR2 { get; set; }
        public double MISCHFAKTOR3 { get; set; }
        public int FER_LAUF1 { get; set; }
        public int FER_LINIE { get; set; }
        public int KZ_SERIE { get; set; }
        public int FER_UVRAND { get; set; }
        public int FER_KSCHUTZ { get; set; }
        public int FER_STRUKTURVERL { get; set; }
        public int FER_STRUKTURSEITE { get; set; }
        public int FER_RAHMENNR { get; set; }
        public string FER_RAHMENTEXT { get; set; }
        public int PREISRELEVANT { get; set; }
        public int PROD_LAGERORT { get; set; }
        public string LAGER_IDENT { get; set; }
        public double PROVISIONSSATZ { get; set; }
        public int FER_GEST_TYP { get; set; }
        public int FER_GEST_ANZ { get; set; }
        public string REKLA_ORT { get; set; }
        public string REKLA_GRUND { get; set; }
        public int MISCH1 { get; set; }
        public int MISCH2 { get; set; }
        public int MISCH3 { get; set; }
        public double LAG_MIN_BEST { get; set; }
        public double LAG_BEST_MENGE { get; set; }
        public double EK_ZEIT_STK { get; set; }
        public string KOSTENSTELLE { get; set; }
        public string KOSTENART { get; set; }
        public double PROVISIONSSATZ2 { get; set; }
        public int KZ_AUTOZUSCHN { get; set; }
        public string POS_TEXT1 { get; set; }
        public string POS_TEXT2 { get; set; }
        public string POS_TEXT3 { get; set; }
        public string POS_TEXT4 { get; set; }
        public string POS_TEXT5 { get; set; }
        public int AUFTR_REF { get; set; }
        public int POS_REF { get; set; }
        public double RUECKSCHNITT { get; set; }
        public double MINMENGE { get; set; }
        public string HASH { get; set; }
        public int PMGRP { get; set; }
        public string PROD_GESTELLNR {get;set;}
        public double HK_MATGEMKOST { get; set; }
        public int GRUPPE { get; set; }
        public int KZ_RANDENT { get; set; }
        public int LAG_LAGER_ID { get; set; }
        public int GRENZTYP1 { get; set; }
        public int GRENZTYP2 { get; set; }
        public int GRENZTYP3 { get; set; }
        public int PACKREGEL { get; set; }
        public string POS_DOCK { get; set; }
        public int PMG_DEF { get; set; }
        public int ITM_REGEL_ID { get; set; }
        public int ALTERNATIV { get; set; }
        public int GRENZTYP4 { get; set; }
        public double HK_VOLLGEWINN { get; set; }
        public int PREISRELEVANTEK { get; set; }
        public int POS_BIT2 { get; set; }
        public double HK_VERWGEMKOST { get; set; }
        public int SKIZZEN_DRUCK { get; set; }
        public int POS_BLOCK { get; set; }
        public int FER_CUTTING_ONLY { get; set; }
        public double HK_VERTRGEMKOST { get; set; }
        public int VERS_BIT { get; set; }
        public string FER_GEST_NR { get; set; }
        public int BOHR_BEZUG { get; set; }
        public int EK_LIEFERANT { get; set; }
        public int PREIS_BIT { get; set; }
        public double HK_GEWINN { get; set; }
        public double HK_SONZU1 { get; set; }
        public double HK_SONZU2 { get; set; }
        public double HK_EK1 { get; set; }
        public double HK_EK2 { get; set; }
        public double HK_EK3 { get; set; }
        public double HK_VOLLEK1 { get; set; }
        public double HK_VOLLEK2 { get; set; }
        public double HK_VOLLEK3 { get; set; }
        public double HK_VOLLSON { get; set; }
        public double HK_VOLLVERW { get; set; }
        public double HK_VOLLVERT { get; set; }
        public double HK_VOLLSONZU1 { get; set; }
        public double HK_VOLLSONZU2 { get; set; }
        public double HK_VERLUST { get; set; }
        public double HK_VOLLSELBKOST { get; set; }
        public int FER_BESCHAFFARTNR { get; set; }
        public int FER_BESCHAFFARTTYP { get; set; }
        public int FER_BESCHICH_SEITE { get; set; }
        public int KZ_FIXIERT { get; set; }
        public DateTime DATUM { get; set; }
        public string MITARB_ID { get; set; }
        public string POS_VERPACKUNG { get; set; }
        public string MINPREIS_PWD { get; set; }
        public double PP_MENGE { get; set; }
        public double PP_BREITE { get; set; }
        public double PP_HOEHE { get; set; }
        public double PP_QM { get; set; }
        public int POS_BIT3 { get; set; }
        public int AUFBAU_ID { get; set; }
        public int ISOAUFBAUKEY { get; set; }
        public double FI_ZUBASIS { get; set; }
        public int LIORDER_NR { get; set; }
        public string LIORDER_VSG { get; set; }
        public string ETIK_STEUERUNG { get; set; }
        public int PROD_ZUSCHLAG_BIT { get; set; }
        public int FORMGRP { get; set; }
        public int RESTERROR { get; set; }
        public double FI_BETR_MWST1 { get; set; }
        public double FI_BETR_MWST2 { get; set; }
        public double FI_BETR_MWST1_FW { get; set; }
        public double FI_BETR_MWST2_FW { get; set; }
        public string CE_KZ { get; set; }
        public string PR_GRUPPE { get; set; }
        public double FI_POS_GES_MAN { get; set; }
        public int FI_POS_GES_BIT { get; set; }
        public string FI_POS_GES_BEM { get; set; }
        public double FI_POS_GES_RAB { get; set; }
        public int CE_FLAG { get; set; }
        public string CE_CPIP { get; set; }
        public int CE_LEVEL { get; set; }
        public int CE_BIT { get; set; }
        public double FI_ITEM_MWST1 { get; set; }
        public double FI_ITEM_MWST2 { get; set; }
        public double FI_ITEM_MWST1_FW { get; set; }
        public double FI_ITEM_MWST2_FW { get; set; }
        public double MINMENGE_EK { get; set; }
        public int MASSRUNDUNG { get; set; }
        public int MASSRUNDUNG_EK { get; set; }
        public string MAKRO_NAME { get; set; }
        public int PROD_FARB_ID { get; set; }
        public string VAR_DATA { get; set; }
        public string VPOS_NR { get; set; }
        public double EK_AUTO { get; set; }
        public double FI_AUTO { get; set; }
        public double FI_AUTO_FW { get; set; }
        public double EK_ZUBASIS { get; set; }
        public double FI_POS_GES_AB { get; set; }
        public string PREIS_AEN_BENUTZER { get; set; }
        public DateTime PREIS_AEN_DATUM { get; set; }
        public string REF_BEST_ID { get; set; }
        public string REF_BEST_POS { get; set; }
        public string REF_AUSLIEF_AUFTR_ID { get; set; }
        public string REF_AUSLIEF_AUFTR_POS { get; set; }
        public string REF_ENDKUNDE_BEST_ID { get; set; }
        public string REF_ENDKUNDE_BEST_POS { get; set; }
        public int SKIZZEN_DRUCK_SPR { get; set; }
        public double FI_MWST1 { get; set; }
        public double FI_MWST2 { get; set; }
        public double FI_MWST3 { get; set; }
        public double FI_MWST4 { get; set; }
        public double FI_MWST5 { get; set; }
        public double FI_BETR_MWST3 { get; set; }
        public double FI_BETR_MWST3_FW { get; set; }
        public double FI_ITEM_MWST3 { get; set; }
        public double FI_ITEM_MWST3_FW { get; set; }
        public double FI_BETR_MWST4 { get; set; }
        public double FI_BETR_MWST4_FW { get; set; }
        public double FI_ITEM_MWST4 { get; set; }
        public double FI_ITEM_MWST4_FW { get; set; }
        public double FI_BETR_MWST5 { get; set; }
        public double FI_BETR_MWST5_FW { get; set; }
        public double FI_ITEM_MWST5 { get; set; }
        public double FI_ITEM_MWST5_FW { get; set; }
        public double EK_AUTO_STAT { get; set; }
        public double FI_AUTO_STAT { get; set; }
        public double FI_AUTO_STAT_FW { get; set; }
        public double PP_UEBERMENGE { get; set; }
        public double PP_UNTERMENGE { get; set; }
        public double PP_WUNSCHMENGE { get; set; }
        public DateTime PRODUCTION_DATE { get; set; }
        public string PROD_BEZ1_FOREIGN { get; set; }
        public string PROD_BEZ2_FOREIGN { get; set; }
        public string PROD_BEZ3_FOREIGN { get; set; }
        public double FI_PREIS_ME_BASE { get; set; }
        public double EK_PREIS_ME_BASE { get; set; }
        public double EK_AUTO_EK { get; set; }
        public string ROWID {get;set;}

    }



    /*bom de los items*/
    public class BW_AUFTR_STKL
    {
        public int ID { get; set; }
        public int POS_NR { get; set; }
        public string STL_BEZ { get; set; }
        public string PROD_FARB_BEZ { get; set; }
        public string STL_KURZ_BEZ { get; set; }
        public int STL_STATUS { get; set; }
        public int STL_DRUCKPOS { get; set; }
        public int STL_MOD { get; set; }
        public int STL_BIT { get; set; }
        public int STL_PRODART { get; set; }
        public int STL_PRODGRP { get; set; }
        public string STL_WGR { get; set; }
        public string STL_WGR_STAT { get; set; }
        public string STL_KMB { get; set; }
        public string STL_KMB_STAT { get; set; }
        public double STL_LFM { get; set; }
        public double STL_QM { get; set; }
        public double STL_LFM_FAKT { get; set; }
        public double STL_QM_FAKT { get; set; }
        public int STL_STTXT_NR { get; set; }
        public int SPRACH_BASIS { get; set; }
        public double STL_PLANSTK { get; set; }
        public double PR_RABATT { get; set; }
        public double EK_NETTO_GES { get; set; }
        public double EK_ZEIT { get; set; }
        public int STL_KZ_SKONTO { get; set; }
        public double PR_PREIS_ME { get; set; }
        public int KZ_LAGER { get; set; }
        public int KZ_BESTELLUNG { get; set; }
        public int KZ_AUTOZUSCHLAG { get; set; }
        public int ID_LIEFERANT { get; set; }
        public int FER_STRUKTURVERL { get; set; }
        public int FER_STRUKTURSEITE { get; set; }
        public int FER_REDUKTION { get; set; }
        public int FER_LAUF { get; set; }
        public int PR_LISTE { get; set; }
        public int PR_SCHLUESSEL { get; set; }
        public string PR_EINHEIT { get; set; }
        public int PR_LISTE_EK { get; set; }
        public int PR_SCHLUESSEL_EK { get; set; }
        public string PR_EINHEIT_EK { get; set; }
        public string PR_MEEINHEIT { get; set; }
        public double PR_PREIS_ME_FW { get; set; }
        public int PR_PREIS_OFFEN { get; set; }
        public int PR_PREISDRUCK { get; set; }
        public int PR_ZUSCHLAGART { get; set; }
        public double PR_BETR_NETTO { get; set; }
        public double PR_BETR_NETTO_FW { get; set; }
        public double PR_BETR_BRUTTO { get; set; }
        public double PR_BETR_BRUTTO_FW { get; set; }
        public double PR_NETTO_GES { get; set; }
        public double PR_NETTO_GES_FW { get; set; }
        public double EK_QM { get; set; }
        public double EK_QM_FAKT { get; set; }
        public double EK_LFM { get; set; }
        public double EK_LFM_FAKT { get; set; }
        public double EK_PREIS_ME { get; set; }
        public double EK_RABATT { get; set; }
        public double EK_BRUTTO { get; set; }
        public double EK_NETTO { get; set; }
        public double FI_VK_URSPRUNG { get; set; }
        public double MINMENGE { get; set; }
        public double VER_PREIS_ME { get; set; }
        public double VER_RABATT { get; set; }
        public int PREISRELEVANT { get; set; }
        public int PROD_LAGERORT { get; set; }
        public double VER_BETR_NETTO { get; set; }
        public int BOM_PRODUKT { get; set; }
        public int BOM_ID { get; set; }
        public int BOM_NODE { get; set; }
        public int BOM_POS { get; set; }
        public int BOM_LEVEL { get; set; }
        public string KOSTENSTELLE { get; set; }
        public string KOSTENART { get; set; }
        public int KZ_AUTOZUSCHN { get; set; }
        public int KZ_SN3 { get; set; }
        public int PREISRELEVANTEK { get; set; }
        public int PROD_RELEVANT { get; set; }
        public int BOM_MASTER_ID { get; set; }
        public int BEARB_INS { get; set; }
        public int LAG_LAGER_ID { get; set; }
        public string HASH { get; set; }
        public int STL_BIT2 { get; set; }
        public int FI_KZ_STEUER { get; set; }
        public int FI_KZ_SKONTO { get; set; }
        public double VER_BETR_BRUTTO { get; set; }
        public int FER_CUTTING_ONLY { get; set; }
        public double VER_NETTO_GES { get; set; }
        public string VER_EINHEIT { get; set; }
        public double HK_MATGEMKOST { get; set; }
        public double HK_VERLUST { get; set; }
        public int FER_BESCHAFFARTNR { get; set; }
        public int FER_BESCHAFFARTTYP { get; set; }
        public int PREIS_BIT { get; set; }
        public int FER_BESCHICH_SEITE { get; set; }
        public double STL_MENGE { get; set; }
        public double STL_BREITE { get; set; }
        public double STL_HOEHE { get; set; }
        public double STL_DICKE { get; set; }
        public double FI_ZUBASIS { get; set; }
        public int LIORDER_NR { get; set; }
        public string LIORDER_VSG { get; set; }
        public int STL_BIT3 { get; set; }
        public int PROD_ZUSCHLAG_BIT { get; set; }
        public string CE_CPIP { get; set; }
        public int CE_LEVEL { get; set; }
        public int CE_INFLUENCING { get; set; }
        public double FI_ITEM_MWST1 { get; set; }
        public double FI_ITEM_MWST2 { get; set; }
        public double FI_ITEM_MWST1_FW { get; set; }
        public double FI_ITEM_MWST2_FW { get; set; }
        public double MINMENGE_EK { get; set; }
        public int MASSRUNDUNG { get; set; }
        public int MASSRUNDUNG_EK { get; set; }
        public int MASS_BIT { get; set; }
        public int PROD_FARB_ID { get; set; }
        public int GRENZTYP1 { get; set; }
        public int GRENZTYP2 { get; set; }
        public int GRENZTYP3 { get; set; }
        public int GRENZTYP4 { get; set; }
        public double EK_AUTO { get; set; }
        public double FI_AUTO { get; set; }
        public double FI_AUTO_FW { get; set; }
        public double FI_MWST1 { get; set; }
        public double FI_MWST2 { get; set; }
        public double FI_MWST3 { get; set; }
        public double FI_MWST4 { get; set; }
        public double FI_MWST5 { get; set; }
        public double FI_ITEM_MWST3 { get; set; }
        public double FI_ITEM_MWST3_FW { get; set; }
        public double FI_ITEM_MWST4 { get; set; }
        public double FI_ITEM_MWST4_FW { get; set; }
        public double FI_ITEM_MWST5 { get; set; }
        public double FI_ITEM_MWST5_FW { get; set; }
        public double EK_AUTO_STAT { get; set; }
        public double FI_AUTO_STAT { get; set; }
        public double FI_AUTO_STAT_FW { get; set; }
        public int BOM_BASE_ID { get; set; }
        public DateTime PRODUCTION_DATE { get; set; }
        public int BOM_PUID { get; set; }
        public string STL_BEZ_FOREIGN { get; set; }
        public double PR_PREIS_ME_BASE { get; set; }
        public double EK_PREIS_ME_BASE { get; set; }
        public double EK_AUTO_EK { get; set; }
        public string ROWID {get;set;}
        public double EK_AUTO_EK_STAT { get; set; }
        public int BEARB_INS2 { get; set; }
        public string AREA { get; set; }
        public int AREA_NUMBER { get; set; }
        public int DINLR { get; set; }
        public int AREA_BOM_PUID { get; set; }
        public int RANK { get; set; }
        public DateTime STL_ANLIEFERUNG { get; set; }
        public string POLINK_GUID { get; set; }
        public DateTime DELIVERY_CONFIRMED { get; set; }
    }


}


