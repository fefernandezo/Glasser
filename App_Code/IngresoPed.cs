using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de FuncionesPedido
/// </summary>
// Funciones del detalle de pedido
public class FunDetPed
{
    //funcion info usuario
    FuncUser usuario = new FuncUser();
    //conecciones a las DB
    
    SqlConnection ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlCommand cmdPlabal;
    SqlCommand cmdPlabal2;
    SqlCommand cmdAlfak;
    static SqlDataReader drPlabal;
    static SqlDataReader drAlfak;
    static SqlDataReader drPlabal2;



    //Busca el último ID de pedidos en Plabal y asigna uno para ser utilizado en el actual
    public int AsegurarIdPlabal()
    {
        int ultimoid = 0;
        string Select = "SELECT TOP 1 ID  FROM PLABAL.dbo.NVenta ORDER BY ID DESC";
        string Insert = "INSERT INTO PLABAL.dbo.NVenta (ID) VALUES (@ID)";


        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);

        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            ultimoid = Convert.ToInt32(drPlabal[0].ToString()) + 1;

        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Insert, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@ID", ultimoid);
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();

        return ultimoid;
    }



    //Ingresa el pedido en Plabal
    public void IngresarOfertaPlabal(int idPlabal, int numOferta, string Cliente, int cantidad, DateTime horaIngreso, string estado, string usuario, decimal totalPedido)
    {
        string Update = "UPDATE PLABAL.dbo.NVenta SET OT=@OT,Cliente=@cliente, Cantidad=@cantidad,FIngreso= CONVERT(VARCHAR,GETDATE(),105),HIngreso=@hingreso,Estado=@estado,Usuario=@usuario,TotalPedido=@totalPedido WHERE ID=@ID";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Update, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@ID", idPlabal);
        cmdPlabal.Parameters.AddWithValue("@OT", numOferta);
        cmdPlabal.Parameters.AddWithValue("@cliente", Cliente);
        cmdPlabal.Parameters.AddWithValue("@cantidad", cantidad);
        cmdPlabal.Parameters.AddWithValue("@hingreso", horaIngreso.ToShortTimeString());
        cmdPlabal.Parameters.AddWithValue("@estado", estado);
        cmdPlabal.Parameters.AddWithValue("@usuario", usuario);
        cmdPlabal.Parameters.AddWithValue("@totalPedido", totalPedido);

        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();
    }


    //Ingresa el pedido en la tabla e_pedido del ecommerce
    public void Ingresar_e_pedido(string nom_pedido, DateTime fechaHoraPedido, DateTime fechaEntrega, String tipoDespacho, string observacion, int pedidoAlfak, int cantidad, decimal totalPedido)
    {
        Infousuario infousu = usuario.DatosUsuario();
        ConnPlabal.Open();
        string Insert = "INSERT INTO e_Pedidos VALUES (@id_user, @nom_pedido, @fechaHoraPedido, @fechaEntrega, @tipoDespacho, @observacion,@alfak,@estado,@cantidad,@totalpedido)";

        cmdPlabal = new SqlCommand(Insert, ConnPlabal);

        cmdPlabal.Parameters.AddWithValue("@id_user", infousu.Id);
        cmdPlabal.Parameters.AddWithValue("@nom_pedido", nom_pedido);
        cmdPlabal.Parameters.AddWithValue("@fechaHoraPedido", fechaHoraPedido);
        cmdPlabal.Parameters.AddWithValue("@fechaEntrega", fechaEntrega);
        cmdPlabal.Parameters.AddWithValue("@tipoDespacho", tipoDespacho);
        cmdPlabal.Parameters.AddWithValue("@observacion", observacion);
        cmdPlabal.Parameters.AddWithValue("@alfak", pedidoAlfak);
        cmdPlabal.Parameters.AddWithValue("@estado", "ING");
        cmdPlabal.Parameters.AddWithValue("@cantidad", cantidad);
        cmdPlabal.Parameters.AddWithValue("@totalpedido", totalPedido);


        int filas = cmdPlabal.ExecuteNonQuery();

        ConnPlabal.Close();
    }


    //Busca el último correlativo de un pedido en Alfak y le suma 1
    public int UltimoCorrelativoAlfak()
    {
        int correlativo = 0;
        ConnAlfak.Open();
        string SelectAlfak = "SELECT TOP 1 ID  FROM PHGLASS.SYSADM.BW_AUFTR_KOPF WHERE OR_AVBEREICH = 'SANTIAGO'  ORDER BY ID DESC ";
        cmdAlfak = new SqlCommand(SelectAlfak, ConnAlfak);
        drAlfak = cmdAlfak.ExecuteReader();
        drAlfak.Read();

        if (drAlfak.HasRows)
        {
            correlativo = Convert.ToInt32(drAlfak[0].ToString()) + 1;
        }
        else
        {

        }
        drAlfak.Close();
        ConnAlfak.Close();



        return correlativo;
    }


    //Ingresa un pedido en Alfak
    public void IngresarPedidoAlfak(int idPedido, int idCliente, string mcode, string nombreCliente, string direccion, string telefono, string rut, string mail, string tipoPago, string vendedor, DateTime fechaEntregaX)
    {
        string Insert = @"INSERT INTO PHGLASS.SYSADM.BW_AUFTR_KOPF (ID,AH_ARCHITECT, AH_IDENT, AH_LAND, AH_PROVINZ, AL_IDENT, AL_LAND, AL_PROVINZ, AR_IDENT,
            AR_LAND, AR_PROVINZ,CONTRACT, DOK_TYP, FER_RAHMENNR, OR_SPRACH_BASIS, FI_MWST1_ID, FI_MWST2_ID, FI_MWST3_ID, FI_MWST4_ID, FI_MWST5_ID, FI_WAEHRUNG,
            FI_ZAHLBED, FI_ZAHLWEG, KATEGORIE, KO_OBJEKT_KUNDE, KO_OBJEKT_LIEF, KZ_INDIV_KUNDE, OR_ADIENST, OR_ADIENST2, OR_AVBEREICH, OR_AWTOUR, 
            OR_BEARBEITER, OR_FACHBERATER, OR_GESCHART, OR_GRUPPE, OR_LIEFERBED, OR_MANDANT, OR_SPERRKZ, OR_SPRACH_ID, OR_TOUR, OR_VERPACKUNG, OR_ZOLLTOUR, PRIORITAET,
            STATUS, SZR_PRODART, SZR_PRODGRP, SZR_VARIANTE, TRANSPORT_ID , DATUM_ERF, AH_HAUPT_AUFTR,OR_FAHRER , KO_MASSEINH, FI_FB_KZ,OR_LKW, AH_LIEFER_KZ, KO_PREISDRUCK, TOUREN_RANGFOLGE,
            AH_NAME1,AH_MCODE,AH_MAIL,ETIK_LAYOUT,PRINTSEQ1,PRINTSEQ2,PRINTSEQ3,AH_STRASSE,AH_TELEFON,AH_FAX,KO_SAMMELRE,KO_TEILFAK,KO_TEILLIEF,FI_UST_ID,DATUM_LIEFER_PLAN,DATUM_LIEFER_TAT,MOD,DATUM_ANLIEFERUNG) 

            VALUES (@idOferta,0,@id_cli,'CL','SINCOMUNA',@id_cli,'','',@id_cli,'','',0,'<indf>',0,0,19,0,0,0,0,'$',@tipoPago,'<indf>',2,0,0,0,@ven,'<indf>','SANTIAGO','<indf>','ECOMMERCE','<indf>',
            '<indf>','VENTANERO GRANDE','2. Entrega en Santiago',1,'libre',0,'ZONA SANTIAGO','<indf>','<indf>','Normal',34,'<indf>','<indf>',0,0,@fechaHoy,0,'<indf>',0,7,'<indf>',0,1,0,@nom_cli,@mcode,@mail,1001,0,0,0,@dir,
            @tel,@tel,1,1,1,@rut,@fechaProg,@fechaProg,1,@fechaProg) ";

        ConnAlfak.Open();
        cmdAlfak = new SqlCommand(Insert, ConnAlfak);
        cmdAlfak.Parameters.AddWithValue("@idOferta", idPedido);
        cmdAlfak.Parameters.AddWithValue("@id_cli", idCliente);
        cmdAlfak.Parameters.AddWithValue("@mcode", mcode);
        cmdAlfak.Parameters.AddWithValue("@nom_cli", nombreCliente);
        cmdAlfak.Parameters.AddWithValue("@dir", direccion);
        cmdAlfak.Parameters.AddWithValue("@tel", telefono);
        cmdAlfak.Parameters.AddWithValue("@rut", rut);
        cmdAlfak.Parameters.AddWithValue("@mail", mail);
        cmdAlfak.Parameters.AddWithValue("@tipoPago", tipoPago);
        cmdAlfak.Parameters.AddWithValue("@ven", vendedor);
        cmdAlfak.Parameters.AddWithValue("@fechaHoy", DateTime.Now);
        cmdAlfak.Parameters.AddWithValue("@fechaProg", fechaEntregaX);

        int filas = cmdAlfak.ExecuteNonQuery();

        ConnAlfak.Close();
    }


    //Busca la composicion del pedido en Alfak
    public string ComposicionAlfak(string id)

    {
        string dato = "";
        ConnAlfak.Open();
        string SelectAlfak = @"SELECT TOP 1 BA_BEZ1 FROM PHGLASS.SYSADM.BA_PRODUKTE_BEZ WHERE BA_PRODUKT = @COD";
        cmdAlfak = new SqlCommand(SelectAlfak, ConnAlfak);
        cmdAlfak.Parameters.AddWithValue("@COD", id);
        drAlfak = cmdAlfak.ExecuteReader();
        drAlfak.Read();

        if (drAlfak.HasRows)
        {
            dato = drAlfak[0].ToString();
        }
        else
        {

        }
        drAlfak.Close();
        ConnAlfak.Close();


        return dato;
    }

    //ingresa en la tabla temp_pedidos y devuelve el id
    public string IngPedPaso1(string nompedido, string observa, string Rutadj, string TokenId)
    {
        string id_temp_pedido = "";

        Infousuario infousu = usuario.DatosUsuario();

        string Consulta = "INSERT INTO PLABAL.dbo.e_temp_pedidos (UserId,id_cliente,nombre,fecha,observacion,Estado,Rutadj,Token) VALUES (@id_user,@id_cliente,@nompedido,@fecha,@observa,@estado,@Rutadj,@TokenId)";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Consulta, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@id_user", infousu.Id);
        cmdPlabal.Parameters.AddWithValue("@id_cliente", infousu.IdEmpresa);
        cmdPlabal.Parameters.AddWithValue("@nompedido", nompedido);
        cmdPlabal.Parameters.AddWithValue("@fecha", DateTime.Now);
        cmdPlabal.Parameters.AddWithValue("@observa", observa);
        cmdPlabal.Parameters.AddWithValue("@estado", "0");
        cmdPlabal.Parameters.AddWithValue("@Rutadj", Rutadj);
        cmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);

        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();

        string Select = "SELECT TOP 1 id FROM PLABAL.dbo.e_temp_pedidos WHERE UserId = @id_usuario AND id_cliente=@id_cliente AND Estado=@Estado AND nombre=@nombre AND Rutadj=@Ruta ORDER BY id DESC";


        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@id_usuario", infousu.Id);
        cmdPlabal.Parameters.AddWithValue("@Estado", "0");
        cmdPlabal.Parameters.AddWithValue("@id_cliente", infousu.IdEmpresa);
        cmdPlabal.Parameters.AddWithValue("@nombre", nompedido);
        cmdPlabal.Parameters.AddWithValue("@Ruta", Rutadj);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            id_temp_pedido = drPlabal[0].ToString();

        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();

        return id_temp_pedido;
    }

    //Ingresa el item del pedido a Alfak
    public void IngresarItemAlfak(int pedido, string codigoCompo, int nv_pos, string referencia, decimal precioNeto, int cantidad, int ancho, int alto, decimal mt2, decimal sumaEspesor)
    {
        string nomElemento = ComposicionAlfak(codigoCompo);

        string Insert = @"INSERT INTO PHGLASS.SYSADM.BW_AUFTR_POS 
            (ID,POS_NR,AUFBAU_ID,EK_LIEFERANT,FER_BESCHAFFARTTYP,FER_BESCHAFFARTNR,FER_BESCHICH_SEITE,SPRACH_BASIS,FER_GEST_TYP,FER_STRUKTURSEITE,FER_STRUKTURVERL,
            ISOAUFBAUKEY,KOSTENART,KOSTENSTELLE,MISCH1,MISCH2,MISCH3,PACKREGEL,PMG_DEF,POS_LIEFERANT,POS_STATUS,POS_STTXT_NR,POS_VERPACKUNG,PR_EINHEIT,PR_EINHEIT_EK,PR_LISTE,
            PR_LISTE_EK,PR_SCHLUESSEL,PR_SCHLUESSEL_EK,PROD_PRODART,PROD_PRODGRP,PROD_FARB_ID,PROD_ID,PROD_KMB,PROD_KMB_STAT,PROD_LAGERORT,PROD_MEEINHEIT,PROD_WGR,PROD_WGR_STAT,REKLA_GRUND,REKLA_ORT,POS_KOMMISSION,
            PROD_BEZ1,FI_NETTO,FI_NETTO_FW,FI_NETTO_GES,FI_NETTO_GES_FW,EK_NETTO,EK_NETTO_GES, PP_MENGE,PP_BREITE,PP_HOEHE,PP_QM, LAGER_IDENT,HASH,PREISRELEVANTEK,DATUM,MITARB_ID,VPOS_NR,PP_ORIG_MENGE, FI_POS_STK, FI_POS_STK_FW, FI_POS_GES, FI_POS_GES_FW, FI_FESTPREIS,FI_VK_URSPRUNG, LIORDER_NR,
            RESTERROR,PP_DICKE) VALUES
             /*id oferta */
             (@idOFerta,
             /*numero de posicion*/ @nv_pos,
             /*aufbauID*/0,
             /*EK_LIEFERANT*/0,
             /*FER_BESCHAFFARTTYP*/2,
             /*FER_BESCHAFFARTNR*/0,
             /*FER_BESCHICH_SEITE*/0,
             /*SPRACH_BASIS*/0,
             /*FER_GEST_TYP*/0,
             /*FER_STRUKTURSEITE*/0,
             /*FER_STRUKTURVERL*/0,
             /*ISOAUFBAUKEY*/0,
             /*KOSTENART*/ '<indf>',
             /*KOSTENSTELLE*/ '21DVH4       VENTANA Cor-Arm',
             /*MISCH1*/ 0,
             /*MISCH2*/ 0,
             /*MISCH3*/ 0,
             /*PACKREGEL*/0,
             /*PMG_DEF*/ 0,
             /*POS_LIEFERANT*/0,
             /*POS_STATUS*/ 0,
             /*POS_STTXT_NR*/0,
             /*POS_VERPACKUNG*/'<indf>',
             /*PR_EINHEIT*/ 'Pza',
             /*PR_EINHEIT_EK*/'Pza',
             /*PR_LISTE*/ 1,
             /*PR_LISTE_EK*/3,
             /*PR_SCHLUESSEL*/ 1,
             /*PR_SCHLUESSEL_EK*/ 5,
             /*PROD_PRODART*/ 50,
             /*PROD_PRODGRP*/ 1,
             /*PROD_FARB_ID*/ 0,
             /*PROD_ID*/ @proID, 
             /*PROD_KMB*/ 'H11',
             /*PROD_KMB_STAT*/ 'H11',
             /*PROD_LAGERORT*/ 0,
             /*PROD_MEEINHEIT*/  'm²',
             /*PROD_WGR*/ 'H11',
             /*PROD_WGR_STAT*/ 'H11',
             /*REKLA_GRUND*/ '<indf>',
             /*REKLA_ORT*/ '<indf>',
             /*POS_KOMMISSION*/ @referencia,
             /*PROD_BEZ1*/ @nomComp,
             /*FI_NETTO*/ @precioNeto,
             /*FI_NETTO_FW*/  @precioNeto,
             /*FI_NETTO_GES*/  @precioNeto,
             /*FI_NETTO_GES_FW*/  @precioNeto,
             /*EK_NETTO*/  @precioNeto,
             /*EK_NETTO_GES*/  @precioNeto,
             /*PP_MENGE*/@cantidad,
             /*PP_BREITE*/@ancho,
             /*PP_HOEHE*/@alto,
             /*PP_QM*/@mt2,
             /*LAGER_IDENT*/0,
             /*HASH*/0000000000000000,
             /*PREISRELEVANTEK*/ 1 ,
             /*DATUM*/ @fecha,
             /*MITARB_ID*/ 'ECOMMERCE',
             /*VPOS_NR*/ @nv_pos,
             /*PP_ORIG_MENGE*/1 ,
             /*FI_POS_STK*/@precioPorUnidad ,
             /*FI_POS_STK_FW*/@precioPorUnidad,
             /*FI_POS_GES*/@precioNeto,
             /*FI_POS_GES_FW*/@precioNeto,
             /*FI_FESTPREIS*/@precioPorUnidad,
            /*FI_VK_URSPRUNG*/@precioNeto,
             /* LIORDER_NR*/ 0,
             /*RESTERROR*/ 0 ,
             /*PP_DICKE*/ @sumaEspesor )";

        ConnAlfak.Open();
        cmdAlfak = new SqlCommand(Insert, ConnAlfak);
        cmdAlfak.Parameters.AddWithValue("@idOferta", pedido);
        cmdAlfak.Parameters.AddWithValue("@proID", codigoCompo);
        cmdAlfak.Parameters.AddWithValue("@nv_pos", nv_pos);
        cmdAlfak.Parameters.AddWithValue("@nomcomp", nomElemento);
        cmdAlfak.Parameters.AddWithValue("@referencia", referencia);
        cmdAlfak.Parameters.AddWithValue("@precioNeto", precioNeto);
        cmdAlfak.Parameters.AddWithValue("@cantidad", cantidad);
        cmdAlfak.Parameters.AddWithValue("@alto", alto);
        cmdAlfak.Parameters.AddWithValue("@ancho", ancho);
        cmdAlfak.Parameters.AddWithValue("@mt2", mt2);
        cmdAlfak.Parameters.AddWithValue("@fecha", DateTime.Now.Date);
        cmdAlfak.Parameters.AddWithValue("@sumaEspesor", sumaEspesor);
        cmdAlfak.Parameters.AddWithValue("@PrecioPorUnidad", precioNeto / cantidad);


        int filas = cmdAlfak.ExecuteNonQuery();

        ConnAlfak.Close();
    }

    public DataTable FechasdeEntrega(string TipoProducto, DateTime StartDate)
    {
        DataTable Tabla = new DataTable();

        DataColumn col;
        DataRow drr = null; ;


        Tabla.TableName = "ExpressDates";

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "AddDays";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "Fecha";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "Tipo";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "diasem";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "Best";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        //cantidad de días express
        int diasexp = 0;
        string Select = "";
        if (TipoProducto == "Termo")
        {
            Select = @"Select Top 1 N1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        }
        else if (TipoProducto == "Lamina")
        {
            Select = @"Select Top 1 A1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        }
        else if (TipoProducto == "Arq")
        {
            Select = @"Select Top 1 H1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        }
        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();
        if (drPlabal.HasRows)
        {
            diasexp = Convert.ToInt32(drPlabal[0].ToString());
        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();
        if (Convert.ToInt32(DateTime.Now.Hour.ToString()) >= 15)
        {
            diasexp = diasexp + 1;
        }
        //iteració de fechas para llenar tabla
        int limite = diasexp +5;
        CultureInfo ci = new CultureInfo("en-US");
        int j = 1;
        int dhabil = 1;
        for (int i = 0; dhabil < limite; i++)
        {
            string diasem = StartDate.AddDays(j).DayOfWeek.ToString();

            if (Esferiado(StartDate.AddDays(j).ToString("M/d/yyyy", ci)) == 0)
            {
                if (diasem != "Saturday")
                {
                    if (diasem != "Sunday")
                    {
                        drr = Tabla.NewRow();
                        drr["AddDays"] = dhabil;
                        drr["Fecha"] = StartDate.AddDays(j).ToString("dddd, dd' de 'MMMM");
                        drr["diasem"] = j;
                        if (dhabil < diasexp)
                        {
                            drr["Tipo"] = "express";
                        }
                        else
                        {
                            if(dhabil == diasexp)
                            {
                                drr["Best"] = "Best";
                            }
                            drr["Tipo"] = "normal";
                        }

                        Tabla.Rows.Add(drr);
                        dhabil = dhabil + 1;

                    }
                }
            }
            j = j + 1;
        }




        return Tabla;
    }

    public double ExponencialInv(int Y0, double k, int t)
    {
        double valor = 0;
        double y = 0;
        valor = Y0 * Math.Exp(t * k);
        y = (valor / 100);

        return Math.Round(y, 2);
    }

    public PlazoEntrega PlazoEntrega(string TipoProducto)
    {
        PlazoEntrega variables = new PlazoEntrega();



        int dias = 0;
        string Select = "";

        if (TipoProducto == "Termo")
        {
            Select = @"Select Top 1 N1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        }
        else if (TipoProducto == "Lamina")
        {
            Select = @"Select Top 1 A1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        }
        else if (TipoProducto == "Arq")
        {
            Select = @"Select Top 1 H1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        }


        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            dias = Convert.ToInt32(drPlabal[0].ToString());
        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();
        variables.Dias = dias;

        //selecciona la fecha segun la matriz de entrega
        CultureInfo ci = new CultureInfo("en-US");
        int sumadias = dias;
        int j = 1;
        for (int i = 0; i < sumadias; i++)
        {
            string diasem = DateTime.Now.AddDays(j).DayOfWeek.ToString();


            if (diasem == "Saturday" || diasem == "Sábado" || diasem == "Sabado")
            {
                sumadias = sumadias + 2;
            }
            else if (Esferiado(DateTime.Now.AddDays(j).ToString("M/d/yyyy", ci)) == 1)
            {
                if (diasem != "Saturday" && diasem != "Sunday")
                {
                    if (diasem != "Domingo" && diasem != "Sábado")
                    {
                        sumadias = sumadias + 1;
                    }
                }

            }

            j = j + 1;
        }

        if (Convert.ToInt32(DateTime.Now.Hour.ToString()) >= 15)
        {
            sumadias = sumadias + 1;
        }
        variables.DiasCorridos = sumadias;
        variables.Fecha = DateTime.Now.AddDays(sumadias);
        return variables;
    }
    //Busca los dias de entrega
    public dEntrega DiasEntrega()
    {
        dEntrega dias = new dEntrega();
        SqlConnection conx;
        string consulta = @"";
        conx = new SqlConnection(ConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
        consulta = @"Select Top 1 N1,A1,H1 FROM PLABAL.dbo.DIASENTREGA ORDER By ID DESC";
        SqlCommand cmd = new SqlCommand(consulta, conx);
        try
        {
            cmd.Connection.Open();
            SqlDataReader leerCadena = cmd.ExecuteReader();
            while (leerCadena.HasRows)
            {
                while (leerCadena.Read())
                {
                    dias.diaTermo = Convert.ToInt32(leerCadena[0]);
                    dias.diaLamina = Convert.ToInt32(leerCadena[1]);
                    dias.diaArq = Convert.ToInt32(leerCadena[2]);
                }
                leerCadena.NextResult();
            }
            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }

        return dias;
    }

    //valida si la fecha ingresada es feriado
    public int Esferiado(string fecha)
    {
        int si = 0;
        SqlConnection conx;
        string Select = @"Select * FROM PLABAL.dbo.Feriados WHERE Fecha = @Fecha";
        conx = new SqlConnection(ConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);


        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@Fecha", fecha);

        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            si = 1;


        }
        else
        {
            si = 0;
        }
        drPlabal.Close();
        ConnPlabal.Close();

        return si;
    }

    public DataTable Feriados()
    {
        DataTable Lista = new DataTable();

        string Select = "Select CONVERT(VARCHAR,Fecha,103) FROM PLABAL.dbo.Feriados WHERE Fecha > GETDATE() AND Fecha < @dateend";

        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                adapter.SelectCommand.Parameters.AddWithValue("@dateend", DateTime.Now.AddDays(80));
                adapter.Fill(Lista);



            }
            catch (Exception Ex)
            {
                Console.Write(Ex);
            }

        }
        List<DateTime> list = new List<DateTime>();
        foreach (DataRow row in Lista.Rows)
        {
            int dia = Convert.ToDateTime(row[0].ToString()).Day;
            int mes = Convert.ToDateTime(row[0].ToString()).Month;
            int year = Convert.ToDateTime(row[0].ToString()).Year;
            list.Add(new DateTime(year, mes, dia));
        }
        return Lista;
    }

    public int Codexp(string id_temp, string Codexp, DateTime Datecodexp, string Status)
    {

        int validador = 0;

        string Consulta = "INSERT INTO PLABAL.dbo.e_codexp (id_temp,Codexp,Datecodexp,Status,Fecha) VALUES (@id_temp,@Codexp,@Datecodexp,@Estado,GETDATE())";

        try
        {
            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Consulta, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@id_temp", id_temp);
            cmdPlabal.Parameters.AddWithValue("@Codexp", Codexp);
            cmdPlabal.Parameters.AddWithValue("@Datecodexp", Datecodexp);
            cmdPlabal.Parameters.AddWithValue("@Estado", Status);
            cmdPlabal.ExecuteNonQuery();
            ConnPlabal.Close();


            validador = 1;

        }
        catch (Exception e)
        {
            validador = 0;
            Console.Write(e);

        }

        //update en e_temp_pedidos
        string Update = "UPDATE PLABAL.dbo.e_temp_pedidos SET Estado='1',fechaentrega=@fechaentrega WHERE id=@idtemp";


        ConnPlabal.Open();
        cmdPlabal2 = new SqlCommand(Update, ConnPlabal);
        cmdPlabal2.Parameters.AddWithValue("@idtemp", id_temp);
        cmdPlabal2.Parameters.AddWithValue("@fechaentrega", Datecodexp);
        cmdPlabal2.ExecuteNonQuery();
        ConnPlabal.Close();






        return validador;


    }

    public void Updatee_temp_pedidos(string id_temp, string tipo_despacho, string Direccion, DateTime fechaentrega)
    {

        //update en e_temp_pedidos
        string Update = "UPDATE PLABAL.dbo.e_temp_pedidos SET tipo_despacho=@tipo_despacho,DireccionEntrega=@direccion, fechaentrega=@fechaentrega,Estado='1' WHERE id=@idtemp";


        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Update, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@idtemp", id_temp);
        cmdPlabal.Parameters.AddWithValue("@fechaentrega", fechaentrega);
        cmdPlabal.Parameters.AddWithValue("@tipo_despacho", tipo_despacho);
        cmdPlabal.Parameters.AddWithValue("@direccion", Direccion);
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();




    }

    public SelectFromcodexp TablaCodExp(string id_temp)
    {
        SelectFromcodexp datos = new SelectFromcodexp();

        string Select = "SELECT Codexp, Datecodexp, Status, Autoriza, Fecha  FROM PLABAL.dbo.e_codexp WHERE Id_temp= @id_temp";
        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@id_temp", id_temp);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            datos.Hascodexp = 1;
            datos.Codigo = Convert.ToInt32(drPlabal[0].ToString());
            datos.Datecode = drPlabal.GetDateTime(1);
            datos.Status = Convert.ToInt32(drPlabal[2].ToString());
            datos.Autoriza = drPlabal[3].ToString();
            datos.Expiredtime = drPlabal.GetDateTime(4);
        }
        else
        {
            datos.Hascodexp = 0;
        }
        drPlabal.Close();
        ConnPlabal.Close();


        return datos;
    }

    //Busqueda en diccionarioclientes
    public ValDiccionario BuscaValDicc(string termino)
    {
        ValDiccionario valores = new ValDiccionario();
        ConnPlabal.Open();
        cmdPlabal = new SqlCommand("SELECT codigo,per_cri1,per_herraje,per_cri2 FROM PLABAL.dbo.e_diccionarioClientes WHERE comp_cliente='" + termino + "'", ConnPlabal);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            valores.Existe = true;
            valores.CodigoAlfakProd = drPlabal[0].ToString();
            valores.EspCRistal1 = Int32.Parse(drPlabal[1].ToString());
            valores.EspCRistal2 = Int32.Parse(drPlabal[3].ToString());
            valores.EspSeparador = Decimal.Parse(drPlabal[2].ToString());
            drPlabal.Close();
        }
        else
        {
            string termino2 = termino.Replace(" ", "");
            drPlabal.Close();
            cmdPlabal2 = new SqlCommand("SELECT codigo,per_cri1,per_herraje,per_cri2 FROM PLABAL.dbo.e_diccionarioClientes WHERE comp_cliente = '" + termino2.Trim() + "'", ConnPlabal);
            drPlabal2 = cmdPlabal2.ExecuteReader();
            drPlabal2.Read();
            if (drPlabal2.HasRows)
            {
                valores.Existe = true;
                valores.CodigoAlfakProd = drPlabal2[0].ToString();
                valores.EspCRistal1 = Int32.Parse(drPlabal2[1].ToString());
                valores.EspCRistal2 = Int32.Parse(drPlabal2[3].ToString());
                valores.EspSeparador = Decimal.Parse(drPlabal2[2].ToString());
                drPlabal2.Close();
            }
            else
            {
                valores.Existe = false;
                valores.CodigoAlfakProd = "";
                valores.EspCRistal1 = 0;
                valores.EspCRistal2 = 0;
                valores.EspSeparador = 0;
            }


        }
        ConnPlabal.Close();


        return valores;
    }
}

public class BOMdeProducto
{
    SqlConnection ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
    SqlCommand cmdAlfak;
    static SqlDataReader drAlfak;
    //Ingresar el BOM de cada producto en Alfak

    public void InsertBOM(int idOferta, int posElementoPadre, int proArt, int proGrp, string wgr, string stat, int perimetro, decimal mt2, decimal precioNetoGes, string unMedida,
         int cantidad, int ancho, int alto, int grosor, int posHijo, string nombreElemento, int bomPro, int ntr, decimal espesor)
    {
        string Insert = @"INSERT INTO PHGLASS.SYSADM.BW_AUFTR_STKL
               (ID --- id de oferta 
              ,POS_NR --- posicion del elemento padre
              ,STL_BEZ---- nombre de composicion
              ,PROD_FARB_BEZ-----vacio '' 
              ,STL_KURZ_BEZ ----- vacio '' 
              ,STL_STATUS ------ 0 
              ,STL_DRUCKPOS-------- 0 
              ,STL_MOD ------ 0 y despacho 1 
              ,STL_BIT ------- 44 
              ,STL_PRODART ----- elementos separador 60, vidrio 1 
              ,STL_PRODGRP ----- elementos separador 90, vidrios 11 despacho 0 
              ,STL_WGR --------- B11 VIDRIO INCOLORO , H31 SEPARADOR , Z21 DESPACHO 
              ,STL_WGR_STAT ----- B11 VIDRIO INCOLORO , H31 SEPARADOR , Z21 DESPACHO
              ,STL_KMB ---------- H11 TERMOPANEL
              ,STL_KMB_STAT ----- H11 TERMOPANEL 
              ,STL_LFM ---------- perimetro (alto + ancho ) * 2 
              ,STL_QM ----------- mt2 (alto * ancho)
              ,STL_LFM_FAKT ----- perimetro 
              ,STL_QM_FAKT ------ mt2 
              ,STL_STTXT_NR ----- 0 
              ,SPRACH_BASIS ----- 0 
              ,STL_PLANSTK------- NULL
              ,PR_RABATT -------- 0.00000000
              ,EK_NETTO_GES ----- PRECIO NETO PRODUCTO 
              ,EK_ZEIT ---------- 0.00000000
              ,STL_KZ_SKONTO ---- NULL 
              ,PR_PREIS_ME ------ ?????
              ,KZ_LAGER --------- 0 
              ,KZ_BESTELLUNG ---- 0 
              ,KZ_AUTOZUSCHLAG -- 0 
              ,ID_LIEFERANT ----- 0
              ,FER_STRUKTURVERL-- 0
              ,FER_STRUKTURSEITE--0  
              ,FER_REDUKTION -----0
              ,FER_LAUF --------- 0 
              ,PR_LISTE --------- 1
              ,PR_SCHLUESSEL ---- 1
              ,PR_EINHEIT -------'Pza' 
              ,PR_LISTE_EK ------ 3
              ,PR_SCHLUESSEL_EK-- 5
              ,PR_EINHEIT_EK ---- m² EN VIDRIO , SEPARADOR m lin. , DESPACHO Kg
              ,PR_MEEINHEIT ----- m² EN VIDRIO , SEPARADOR m lin. , DESPACHO Kg
              ,PR_PREIS_ME_FW --- ???
              ,PR_PREIS_OFFEN --- 2 
              ,PR_PREISDRUCK ---- 0 
              ,PR_ZUSCHLAGART---- 0
              ,PR_BETR_NETTO ----  PRECIO ELEMENTO 0 
              ,PR_BETR_NETTO_FW --- PRECIO ELEMENTO 0 
              ,PR_BETR_BRUTTO ----- PRECIO ELEMENTO 0 
              ,PR_BETR_BRUTTO_FW -- PRECIO ELEMENTO 0 
              ,PR_NETTO_GES ------- PRECIO ELEMENTO 0 
              ,PR_NETTO_GES_FW ---- PRECIO ELEMENTO 0 
              ,EK_QM -------------- METRO CUADRADO EN NUMERO 
              ,EK_QM_FAKT --------- METRO CUADRADO EN NUMERO 
              ,EK_LFM ------------- PERIMETRO 
              ,EK_LFM_FAKT -------- PERIMETRO 
              ,EK_PREIS_ME -------- PRECIO 0 ???
              ,EK_RABATT ---------- 0.00000000
              ,EK_BRUTTO ---------- PRECIO ELEMENTO 0
              ,EK_NETTO ----------- PRECIO ELEMENTO 0
              ,FI_VK_URSPRUNG ----- 0.00000000 
              ,MINMENGE ----------- 0.00000000
              ,VER_PREIS_ME ------- VALOR ELEMENTO
              ,VER_RABATT --------- 0.00000000
              ,PREISRELEVANT ------ 1 
              ,PROD_LAGERORT ------ 0 
              ,VER_BETR_NETTO ----- VALOR ELEMENTO 0
              ,BOM_PRODUKT -------- ID DE ELEMENTO ********************************************
              ,BOM_ID ------------- ELEMENTO ENUMERACION 1,2,3,4
              ,BOM_NODE ----------- 0
              ,BOM_POS ------------ ELEMENTO ENUMERACION 1,2,3,4 
              ,BOM_LEVEL----------- 1 
              ,KOSTENSTELLE ------- '<indf>'
              ,KOSTENART ---------- '<indf>'
              ,KZ_AUTOZUSCHN ------ 0 
              ,KZ_SN3 ------------- 0 
              ,PREISRELEVANTEK ---- 1 
              ,PROD_RELEVANT ------ 1 
              ,BOM_MASTER_ID ------ 0
              ,BEARB_INS ---------- 0 
              ,LAG_LAGER_ID ------- 0
              ,HASH --------------- 0000000000000000
              ,STL_BIT2 ----------- 0
              ,FI_KZ_STEUER ------- 1
              ,FI_KZ_SKONTO ------- 1
              ,VER_BETR_BRUTTO ---- valor elemento 
              ,FER_CUTTING_ONLY --- 0 
              ,VER_NETTO_GES ------ valor elemento 
              ,VER_EINHEIT -------- 'Pza'
              ,HK_MATGEMKOST------- 0.00000000
              ,HK_VERLUST --------- 0.00000000
              ,FER_BESCHAFFARTNR--- 1 para vidrio , 0 para separador, 0 para despacho
              ,FER_BESCHAFFARTTYP--- 1 
              ,PREIS_BIT ----------- 0 y 128 para despacho
              ,FER_BESCHICH_SEITE --- 0 
              ,STL_MENGE ------ cantidad 1 
              ,STL_BREITE ----- alto 
              ,STL_HOEHE ------ ancho 
              ,STL_DICKE ------ grosor ( inc 4.00000000 , 11.5 sep , 5.00000000 )
              ,FI_ZUBASIS ----- 0.00000000
              ,LIORDER_NR ----- 0 
              ,LIORDER_VSG----- vacio '' 
              ,STL_BIT3 ------- 0 
              ,PROD_ZUSCHLAG_BIT ----- bit 10000
              ,CE_CPIP --------------- vacio '' 
              ,CE_LEVEL -------------- 0 
              ,CE_INFLUENCING -------- 0 
              ,FI_ITEM_MWST1---------- 0.00000000
              ,FI_ITEM_MWST2---------- 0.00000000
              ,FI_ITEM_MWST1_FW---------- 0.00000000
              ,FI_ITEM_MWST2_FW---------- 0.00000000
              ,MINMENGE_EK ---------- 0.00000000
              ,MASSRUNDUNG ---- 0 
              ,MASSRUNDUNG_EK ---- 0
              ,MASS_BIT ---------- 0 
              ,PROD_FARB_ID ------ 0 
              ,GRENZTYP1---------- 0 y 1 para despacho 
              ,GRENZTYP2---------- 0 
              ,GRENZTYP3 --------- 0 
              ,GRENZTYP4 --------- 0 
              ,EK_AUTO ------------0.00000000
              ,FI_AUTO------------0.00000000
              ,FI_AUTO_FW------------0.00000000
              ,FI_MWST1------------0.00000000
              ,FI_MWST2------------0.00000000
              ,FI_MWST3------------0.00000000
              ,FI_MWST4------------0.00000000
              ,FI_MWST5------------0.00000000
              ,FI_ITEM_MWST3------------0.00000000
              ,FI_ITEM_MWST3_FW------------0.00000000
              ,FI_ITEM_MWST4------------0.00000000
              ,FI_ITEM_MWST4_FW------------0.00000000
              ,FI_ITEM_MWST5------------0.00000000
              ,FI_ITEM_MWST5_FW------------0.00000000
              ,EK_AUTO_STAT------------0.00000000
              ,FI_AUTO_STAT------------0.00000000
              ,FI_AUTO_STAT_FW -------- null 
              ,BOM_BASE_ID -----  (1,2,3, desapcho 0 )
              ,PRODUCTION_DATE ---- null 
              ,BOM_PUID ------- orden enumerado 1,2,3,4
              ,STL_BEZ_FOREIGN -----null
              ,PR_PREIS_ME_BASE -----null
              ,EK_PREIS_ME_BASE -----------0.00000000
              ,EK_AUTO_EK ------------0.00000000
              ,ROWID --- 0000 
              ,EK_AUTO_EK_STAT ---- null 
              ,BEARB_INS2 )  ---- null despacho 0 
              VALUES (@idOferta,@nv_pos,@nomElemento,'','',0,0,0,44,@proArt,@proGrp,@wgr,@stat,'H11','H11',@perimetro,@mt2,@perimetro,@mt2,0,0,NULL,0,@precioNetoGes,0,NULL,0,0,0,0,0,0,0,0,0,1,1,'Pza',3,5,@unMedida,@unMedida,0,2,0,0,0,0,0,0,0,0,
              2,2,6,6,0,0,0,0,0,0,0,0,1,0,0,@bomPro,@posHijo,0,@posHijo,1,'<indf>','<indf>',0,0,1,1,0,0,0,0,0,1,1,0,0,0,'Pza',0,0,@ntr,2,0,0,1,@ancho,@alto,@grosor,0,0,'',0,5321,'',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
              0,0,0,0,0,0,NULL,@posHijo,NULL,@posHijo,NULL,NULL,0,0,0000,NULL,NULL)";

        ConnAlfak.Open();
        cmdAlfak = new SqlCommand(Insert, ConnAlfak);
        cmdAlfak.Parameters.AddWithValue("@idOferta", idOferta);
        cmdAlfak.Parameters.AddWithValue("@nv_pos", posElementoPadre);
        cmdAlfak.Parameters.AddWithValue("@nomElemento", nombreElemento);
        cmdAlfak.Parameters.AddWithValue("@proArt", proArt);
        cmdAlfak.Parameters.AddWithValue("@proGrp", proGrp);
        cmdAlfak.Parameters.AddWithValue("@wgr", wgr);
        cmdAlfak.Parameters.AddWithValue("@stat", stat);
        cmdAlfak.Parameters.AddWithValue("@mt2", mt2);
        cmdAlfak.Parameters.AddWithValue("@precioNetoGes", precioNetoGes);
        cmdAlfak.Parameters.AddWithValue("@unMedida", unMedida);
        cmdAlfak.Parameters.AddWithValue("@cantidad", cantidad);
        cmdAlfak.Parameters.AddWithValue("@ancho", ancho);
        cmdAlfak.Parameters.AddWithValue("@alto", alto);
        cmdAlfak.Parameters.AddWithValue("@grosor", espesor);
        cmdAlfak.Parameters.AddWithValue("@posHijo", posHijo);
        cmdAlfak.Parameters.AddWithValue("@bomPro", bomPro);
        cmdAlfak.Parameters.AddWithValue("@Perimetro", 10);
        cmdAlfak.Parameters.AddWithValue("@ntr", ntr);


        int filas = cmdAlfak.ExecuteNonQuery();
        ConnAlfak.Close();



    }

    public void IngresarBOMAlfak(string codPadre, int idOferta, int v_pos, int perimetro, decimal mt2, decimal precio, int cantidad, int ancho, int alto, int grosor, ArrayList esp)
    {
        string consulta;
        SqlConnection conx;
        int codigoElementoHijo = 0;
        decimal espesorFinal = 0;
        conx = new SqlConnection(ConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
        consulta = @"SELECT BOM_PRODUKT  FROM SYSADM.BA_STUKL WHERE PRODUKT = @codPadre";
        SqlCommand cmd = new SqlCommand(consulta, conx);
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@codPadre", codPadre);
            SqlDataReader leerCadena = cmd.ExecuteReader();
            int contadorVpos = 0;



            while (leerCadena.HasRows)
            {
                while (leerCadena.Read())
                {
                    if (contadorVpos <= 2)
                    {
                        espesorFinal = Convert.ToDecimal(esp[contadorVpos]);
                    }

                    contadorVpos = contadorVpos + 1;

                    codigoElementoHijo = Convert.ToInt32(leerCadena[0]);

                    Componentes(codigoElementoHijo, idOferta, v_pos, perimetro, mt2, precio, cantidad, ancho, alto, grosor, contadorVpos, espesorFinal);
                }
                leerCadena.NextResult();
            }
            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }



    }


    public ElementoHijo Componentes(int cod, int idOferta, int v_pos, int perimetro, decimal mt2, decimal precio, int cantidad, int ancho, int alto, int grosor, int vpos_hijo, decimal espHijo)
    {
        ElementoHijo nuevoHijo = new ElementoHijo();
        string consulta;
        SqlConnection conx;
        int contador = 0;


        conx = new SqlConnection(ConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
        consulta = @"SELECT BA_PRODUKT, BA_BEZ1 FROM SYSADM.BA_PRODUKTE_BEZ WHERE BA_PRODUKT = @cod";
        SqlCommand cmd = new SqlCommand(consulta, conx);
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@cod", cod);
            SqlDataReader leerCadena = cmd.ExecuteReader();



            if (leerCadena.HasRows)
            {
                if (leerCadena.Read())
                {
                    contador = contador + 1;

                    nuevoHijo.codigoHijo = Convert.ToInt32(leerCadena[0]);
                    nuevoHijo.nombreHijo = Convert.ToString(leerCadena[1]);
                    int art = 0;
                    int grp = 0;
                    string stlwgr = "";
                    string unidadMedida = "";
                    int ntr = 0;

                    if (vpos_hijo == 1 || vpos_hijo == 3)
                    {
                        unidadMedida = "m²";
                        art = 1;
                        grp = 11;
                        stlwgr = "B11";
                        ntr = 1;
                    }
                    else if (vpos_hijo == 2)
                    {
                        art = 60;
                        grp = 80;
                        stlwgr = "H31";
                        unidadMedida = "m lin.";
                        ntr = 0;
                    }




                    InsertBOM(idOferta, v_pos, art, grp, stlwgr, stlwgr, perimetro, mt2, precio, unidadMedida, cantidad, ancho, alto, grosor, vpos_hijo, nuevoHijo.nombreHijo, nuevoHijo.codigoHijo, ntr, espHijo);


                }
                leerCadena.NextResult();
            }

            leerCadena.Close();
        }
        finally
        {
            cmd.Connection.Close();
        }



        return nuevoHijo;
    }
}


//funciones del pedido temporal
public class CSPedido
{
    //conecciones a las DB
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlCommand cmdPlabal;
    static SqlDataReader drPlabal;


    //
    public void Insert_e_temp_pedidos(string id_user, string nombre, DateTime fecha, string Tdespacho, string observa, DateTime fechaentrega)
    {
        string Consulta = "INSERT INTO PLABAL.dbo.e_temp_pedidos (id_usuario,nombre,fecha,tipo_despacho,observacion,Estado,fechaentrega) VALUES (@id_user,@nompedido,@fecha,@Tdespacho,@observa,@estado,@fechaentrega)";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Consulta, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@id_user", id_user);
        cmdPlabal.Parameters.AddWithValue("@nompedido", nombre);
        cmdPlabal.Parameters.AddWithValue("@fecha", fecha);
        cmdPlabal.Parameters.AddWithValue("@Tdespacho", Tdespacho);
        cmdPlabal.Parameters.AddWithValue("@observa", observa);
        cmdPlabal.Parameters.AddWithValue("@estado", "0");
        cmdPlabal.Parameters.AddWithValue("@fechaentrega", fechaentrega);
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();
    }

    public void Status_e_temp_pedidos(string ID, int Npedido)
    {
        string Consulta = "UPDATE PLABAL.dbo.e_temp_pedidos SET Estado=@Estado, PedidoAlfak=@Npedido WHERE id=@ID";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Consulta, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@ID", ID);
        cmdPlabal.Parameters.AddWithValue("Npedido", Npedido);
        cmdPlabal.Parameters.AddWithValue("@Estado", "1");
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();
    }


    public string IdTemp_pedido(string Id_user)
    {
        string IdTemp = "";

        string Select = "SELECT TOP 1 id FROM PLABAL.dbo.e_temp_pedidos WHERE id_usuario = @id_usuario AND Estado=@Estado AND PedidoAlfak IS NULL ORDER BY id DESC";


        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@id_usuario", Id_user);
        cmdPlabal.Parameters.AddWithValue("@Estado", "0");
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            IdTemp = drPlabal[0].ToString();

        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();


        return IdTemp;
    }

    public TempPedidos DatosPedidoTemp(string ID)
    {
        TempPedidos valores = new TempPedidos();

        string Select = "SELECT UserId,nombre,CONVERT(VARCHAR,fecha,105),tipo_despacho,observacion, Rutadj, Estado, Direccionentrega, fechaentrega FROM PLABAL.dbo.e_temp_pedidos WHERE id = @ID";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@ID", ID);
        cmdPlabal.Parameters.AddWithValue("@Estado", "0");
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            valores.id_usuario = drPlabal[0].ToString();
            valores.nombre = drPlabal[1].ToString();
            valores.fecha = drPlabal[2].ToString();
            valores.tipo_despacho = drPlabal[3].ToString().Trim();
            valores.observacion = drPlabal[4].ToString();
            valores.Rutadj = drPlabal[5].ToString();
            valores.Estado = drPlabal[6].ToString();
            valores.Direcciondesp = drPlabal[7].ToString();
            if (drPlabal[8] != DBNull.Value)
            {
                valores.Fechaentrega = drPlabal.GetDateTime(8);
            }
            else
            {

            }

        }
        else
        {

        }
        drPlabal.Close();
        ConnPlabal.Close();
        return valores;
    }


}

//Clase utilizada en CSPedido para los datos del pedido temporal
public class TempPedidos
{
    public string id_usuario { get; set; }
    public string nombre { get; set; }
    public string fecha { get; set; }
    public string tipo_despacho { get; set; }
    public string observacion { get; set; }
    public string Rutadj { get; set; }
    public string Estado { get; set; }
    public string Direcciondesp { get; set; }
    public DateTime Fechaentrega { get; set; }
}

public class ElementoHijo
{

    public string nombreHijo { get; set; }
    public int codigoHijo { get; set; }


}
public class PlazoEntrega
{
    public int Dias { get; set; }
    public DateTime Fecha { get; set; }
    public int DiasCorridos { get; set; }
    public DataTable ExpressDates { get; set; }

}

public class dEntrega
{
    public int diaTermo { get; set; }
    public int diaLamina { get; set; }
    public int diaArq { get; set; }
}
public class SelectFromcodexp
{
    public int Hascodexp { get; set; }
    public int Codigo { get; set; }
    public int Status { get; set; }
    public DateTime Datecode { get; set; }
    public DateTime Expiredtime { get; set; }
    public string Autoriza { get; set; }
}

public class Pedido
{

    public string Nombre { get; set; }

    public string TipoDespacho { get; set; }

    public string Observacion { get; set; }

    public string Archivo { get; set; }


}

public class ValDiccionario
{
    public string CodigoAlfakProd { get; set; }
    public int EspCRistal1 { get; set; }
    public int EspCRistal2 { get; set; }
    public decimal EspSeparador { get; set; }
    public bool Existe { get; set; }

}