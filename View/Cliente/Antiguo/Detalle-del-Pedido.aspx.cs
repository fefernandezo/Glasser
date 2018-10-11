using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Membership.OpenAuth;

public partial class Cliente_DetallePedido : System.Web.UI.Page
{
    FuncUser DatosUsuario = new FuncUser();
    CSPedido csPEDIDO = new CSPedido();
    FunDetPed Funciones = new FunDetPed();
    INFOcliente iNFOcli = new INFOcliente();

    string FromFile;
    string IDTemp;
    string NombrePedido;
    string Observa;
    string RutaFile;
    double NetoPedido;
    double NetoPedidoCdesp;
    double NetoDespacho;
    int diascorridos;
    DateTime fechlimit;

    //variables del producto
    string CodigoAlfakProd;
    int EspCristal1;
    int EspCristal2;
    decimal EspSeparador;
    double m2;
    int iFgrhr;
    int correlativoPedido;
    int kilosPedido;
    double m2Pedido;
    int hasCodexp;
    int StatCodexp;

    //TABLA principal
    DataTable Tabla = new DataTable();
    //tabla fechas
    DataTable Tfechas = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        Infousuario Infousu = DatosUsuario.DatosUsuario();

        IDTemp = Request.QueryString["ID"];
        id_temp.Value = IDTemp;

        TempPedidos InfoPedido = csPEDIDO.DatosPedidoTemp(IDTemp);
        NombrePedido = InfoPedido.nombre;
        Observa = InfoPedido.observacion;
        RutaFile = InfoPedido.Rutadj;
        HiddenStatus.Value = InfoPedido.Estado;

        PlazoEntrega Plazoentrega = Funciones.PlazoEntrega("Termo");





        diascorridos = Plazoentrega.DiasCorridos;
        fechlimit = Plazoentrega.Fecha;
        if (RutaFile != "" && !IsPostBack)
        {
            //info de entrega
            string tipodes = InfoPedido.tipo_despacho;
            string direccion = "";
            direccion = InfoPedido.Direcciondesp;
            if (tipodes.Contains("Retiro"))
            {
                DirEntrega.Visible = false;

            }
            else
            {
                DirEntrega.Visible = true;
                DirEntrega.Text = "Dirección de entrega: " + direccion;
            }
            DateTime fechaentr;
            fechaentr = InfoPedido.Fechaentrega;

            Validacionfechacalendario(fechaentr);







            TipoEntrega.Text = InfoPedido.tipo_despacho;
           

                TextDireccion.Text = InfoPedido.Direcciondesp;
               
                TextDireccion.Text = direccion;


            SelectFromcodexp express = Funciones.TablaCodExp(IDTemp);

            if (express.Hascodexp == 1 && express.Status == 0)
            {
                AlertInfoEntrega.Text = "Código Express Solicitado. A la espera de liberación.";
                EnviarPedido.Enabled = false;
                AlertInfoEntrega.ForeColor = System.Drawing.Color.Red;
                AlertInfoEntrega.Font.Bold = true;
                Fechaentrega.Visible = true;

                Fechaentrega.Text = "Fecha de entrega: " + express.Datecode.ToString("dd-MM-yyyy");
            }
            else if (express.Hascodexp == 1 && express.Status == 1)
            {
                AlertInfoEntrega.Text = "Código Express liberado, el pedido debe ser enviado a fabricar antes de ";
                EnviarPedido.Enabled = true;
                CultureInfo ci = new CultureInfo("en-US");

                expiration.Value = express.Expiredtime.ToString("M/d/yyyy", ci) + " " + express.Expiredtime.ToString("h:mm tt", ci);
                AlertInfoEntrega.ForeColor = System.Drawing.Color.Blue;
                AlertInfoEntrega.Font.Bold = true;
                Fechaentrega.Visible = true;

                Fechaentrega.Text = "Fecha de entrega: " + express.Datecode.ToString("dd-MM-yyyy");
            }
            else if (express.Hascodexp == 0)
            {
                EnviarPedido.Enabled = true;
            }


            if (Infousu.Rutempresa == "99558220-1")
            {

            }
            else if (Infousu.Rutempresa == "78509610-K")
            {

            }
            else
            {
                //Funcion detalle pedido para otros clientes
                Tabla = DetalleOtrosCli(RutaFile, "Despacho normal");



            }

           
            MOPanelFechas.Controls.Add(TablaPlazosEntrega());
            
            



            NetodelPedido.Text = "Neto total " + NetoPedido.ToString("C0");
            LblKilos.Text = kilosPedido.ToString() + " kilos";
            //lblNombrePedido.Text = NombrePedido;
            lblObservacion.Text = Observa;
            LblM2.Text = m2Pedido.ToString("0.##") + " metros cuadrados";
            GridDetallePedido.DataSource = Tabla;
            GridDetallePedido.DataBind();


            //codigo express


        }


    }
    
    private HtmlGenericControl TablaPlazosEntrega ()
    {
        //tabla plazos de entrega
        DataTable tablaPlazos = Funciones.FechasdeEntrega("Termo", DateTime.Now);
        HtmlGenericControl tabla = new HtmlGenericControl("table");
        HtmlGenericControl thead = new HtmlGenericControl("thead");
        HtmlGenericControl tr = new HtmlGenericControl("tr");
        HtmlGenericControl th1 = new HtmlGenericControl("th");
        HtmlGenericControl th2 = new HtmlGenericControl("th");
        HtmlGenericControl th3 = new HtmlGenericControl("th");
        th1.InnerHtml = "Fecha";
        th2.InnerHtml = "Retiro";
        th3.InnerHtml = "Despacho";
        tr.Controls.Add(th1);
        tr.Controls.Add(th2);
        tr.Controls.Add(th3);
        tabla.ID = "TablaFechas";
        thead.Controls.Add(tr);
        thead.Attributes.Add("class", "cabecera");
        tabla.Controls.Add(thead);
        tabla.Attributes.Add("class", "table table-bordered tablafechas");
        int introw = 1;
        FunDetPed Funcion = new FunDetPed();
        PlazoEntrega Plazoentregatermo = Funcion.PlazoEntrega("Termo");
        int Dexp = Plazoentregatermo.Dias - 1;
        double valor = 180 / Dexp;
        int PorcColor = Convert.ToInt32(Math.Round(valor, 0));
        
        int contcolor = 90;

        HtmlGenericControl tbody = new HtmlGenericControl("tbody");
        foreach (DataRow row in tablaPlazos.Rows)
        {
            HtmlGenericControl tr2 = new HtmlGenericControl("tr");
            HtmlGenericControl td0 = new HtmlGenericControl("td");
            HtmlGenericControl td1 = new HtmlGenericControl("td");
            HtmlGenericControl td2 = new HtmlGenericControl("td");
            HtmlGenericControl td3 = new HtmlGenericControl("td");
            
            td1.Attributes.Add("class", "seleccionhover cuadros");
            td2.Attributes.Add("class", "seleccionhover cuadros");


            td0.Attributes.Add("id", introw.ToString() + "1");
            td0.InnerHtml = row[1].ToString();
            td3.Attributes.Add("style", "display:none");
            td3.InnerHtml = row[3].ToString();


            if (row[2].ToString() == "express")
            {
                
                double operacion1 = Math.Round(NetoPedido * (1 + Funciones.ExponencialInv(40, -0.25, Convert.ToInt32(row[0].ToString()))));

                
                td1.Attributes.Add("id", introw.ToString() + "2");
                td0.Attributes.Add("class", "express ");
                td1.Attributes.Add("class", "express seleccionhover cuadros");
                td1.InnerHtml = operacion1.ToString("C0");
                td1.Attributes.Add("onclick", "Seleccionfecha('" + row[3].ToString() + "', 'retiro','" + introw.ToString() + "2','" + row[1].ToString() + "')");

                double operacion2 = Math.Round(NetoDespacho + NetoPedido * (1 + Funciones.ExponencialInv(40, -0.25, Convert.ToInt32(row[0].ToString()))));


                td2.Attributes.Add("id", introw.ToString() + "3");
                td2.Attributes.Add("class", "express seleccionhover cuadros");
                td2.InnerHtml = operacion2.ToString("C0");
                td2.Attributes.Add("onclick", "Seleccionfecha('" + row[3].ToString() + "', 'despacho','" + introw.ToString() + "3','" + row[1].ToString() + "')");

                contcolor= contcolor + PorcColor;
            }
            else
            {

                td1.Attributes.Add("id", introw.ToString() + "2");
                td1.InnerHtml = NetoPedido.ToString("C0");
                td1.Attributes.Add("onclick", "Seleccionfecha('" + row[3].ToString() + "', 'retiro','" + introw.ToString() + "2','" + row[1].ToString() + "')");


                td2.Attributes.Add("id", introw.ToString() + "3");
                td2.InnerHtml = (NetoDespacho + NetoPedido).ToString("C0");
                td2.Attributes.Add("onclick", "Seleccionfecha('" + row[3].ToString() + "', 'despacho','" + introw.ToString() + "3','" + row[1].ToString() + "')");

            }
            if (row[4].ToString() == "Best")
            {
                tr2.Attributes.Add("class", "recomendado");
                tr2.Attributes.Add("data-content", "Recomendado");
                tr2.Attributes.Add("rel", "popover");
                tr2.Attributes.Add("data-placement", "top");
                tr2.Attributes.Add("data-trigger", "hover");
            }
            tr2.Controls.Add(td0);
            tr2.Controls.Add(td1);
            tr2.Controls.Add(td2);
            tr2.Controls.Add(td3);
            tbody.Controls.Add(tr2);





            introw++;
        }
        tabla.Controls.Add(tbody);

        return tabla;

    }
    private void Validacionfechacalendario(DateTime fecha)
    {
        PlazoEntrega Plazo = Funciones.PlazoEntrega("Termo");
        SelectFromcodexp Codexp = Funciones.TablaCodExp(id_temp.Value);

        if (Codexp.Hascodexp == 1 && Codexp.Status == 0)
        {
            panelExpress.Visible = true;
            LblExpress.Text = "ATENCIÓN: Hay una solicitud de código express pendiente, aún así usted puede solicitar un código para otra fecha o bien seleccionar una dentro de los plazos de entrega.";
            LblExpress.ForeColor = System.Drawing.Color.Red;
            PanelSinCod.Visible = true;
        }
        else if (Codexp.Hascodexp == 1 && Codexp.Status == 1)
        {
            //codigo express liberado

        }
        else
        {
            if (fecha < Plazo.Fecha.AddDays(-1) && fecha > DateTime.Now)
            {

                panelExpress.Visible = true;
                LblExpress.Text = "ATENCIÓN: Nuestro programa de producción puede ofrecer entrega a partir del día " + Plazo.Fecha.ToString("dd-MM-yyyy") + " en adelante. Si necesita que los productos sean entregados antes de la fecha " +
                    "indicada, debe solicitar un código express.";

                PanelSinCod.Visible = true;


            }
            else
            {
                panelExpress.Visible = false;


            }
        }


    }


    private DataTable DetalleOtrosCli(string rutafile, string Tipodesp)
    {

        
        Pedido Pedido = new Pedido();

        

        TablaEncabezado();
        DataTable retorno = (DataTable)ViewState["tabla"];
        DataRow drow = null;
        OleDbConnection cnn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" +
        "Data Source = " + rutafile + "; " + "Extended Properties='Excel 8.0;HDR=No';");
        Pedido.Archivo = rutafile;
        string sql = "select * from [Sheet1$A6:N1000]";
        OleDbDataAdapter da = new OleDbDataAdapter(sql, cnn);


        //Variables de item
        string detalleProducto;
        double Ancho;
        double Alto;
        double kg;
        double precioProd;
        double NetoItem;
        double NetoItemCDesp;
        double metroLineal;
        string modelo;
        kilosPedido = 0;
        NetoPedido = 0;
        double Margen = iNFOcli.FactorMargen();
        
        try
        {
            cnn.Open();
            OleDbDataReader leerCadena = da.SelectCommand.ExecuteReader();
            while (leerCadena.HasRows)
            {
                while (leerCadena.Read())
                {

                    
                    if (leerCadena[1] != DBNull.Value && leerCadena[2] != DBNull.Value)
                    {

                        
                        detalleProducto = Convert.ToString(leerCadena[1]);

                        ValDiccionario InfodelDicc = Funciones.BuscaValDicc(detalleProducto);

                        
                        if (InfodelDicc.Existe)
                        {
                            CodigoAlfakProd = InfodelDicc.CodigoAlfakProd;
                            EspCristal1 = InfodelDicc.EspCRistal1;
                            EspCristal2 = InfodelDicc.EspCRistal2;
                            EspSeparador = InfodelDicc.EspSeparador;

                        }
                        else
                        {
                            Response.Redirect("~/Diccionario/agregar-Modelo-DVH.aspx?Producto=" + detalleProducto + "&Archivo=" + Pedido.Archivo + "&ID=" + id_temp.Value);
                        }
                        

                        Ancho = 0;
                        Alto = 0;
                        kg = 0;
                        precioProd = 0;
                        NetoItem = 0;
                        NetoItemCDesp = 0;
                        metroLineal = 0;
                        
                        Ancho = Convert.ToDouble(leerCadena[2].ToString());
                        Alto = Convert.ToDouble(leerCadena[3].ToString());
                        
                        CalculosProd CalProd = new CalculosProd();
                        
                        UnidadesProd Datos = CalProd.Calculos(Ancho, Alto, EspCristal1, EspCristal2);
                        
                        kg = Datos.Kilos;
                        m2 = Datos.MetroCuad;
                        metroLineal = Datos.MetroLi;
                        
                        

                        modelo = Convert.ToString(leerCadena[0]).Trim();
                        int digitos = 22 - modelo.Length;



                        drow = retorno.NewRow();
                        drow["Modelo"] = Convert.ToString(leerCadena[0]) + " - " + NombrePedido.Substring(0, digitos);
                        drow["Detalle producto"] = Convert.ToString(leerCadena[1]);
                        drow["Cantidad"] = Convert.ToInt32(leerCadena[4]);
                        int cantidad = Convert.ToInt32(leerCadena[4]);
                        drow["Ancho"] = Convert.ToDecimal(leerCadena[2]);
                        drow["Alto"] = Convert.ToDecimal(leerCadena[3]);
                        drow["mt2"] = (m2 * cantidad).ToString("0.##");
                        drow["Kilos"] = (kg * cantidad); 
                        drow["Esp c1"] = EspCristal1;
                        drow["Esp sep"] = EspSeparador;
                        drow["Esp c2"] = EspCristal2;
                        drow["Codigo"] = CodigoAlfakProd;
                        kilosPedido = kilosPedido + (Convert.ToInt32(kg) * cantidad);
                        m2Pedido = m2Pedido + (m2 * cantidad);
                        CalculosProd Precio = new CalculosProd();
                        //traer precio unitado por producto
                        precioProd = Precio.PrecioProd(CodigoAlfakProd, Ancho, Alto);
                        //sumar proceso al precio uinitario
                        NetoItem = precioProd + Precio.PrecioProcesoDVH("Corte", m2, metroLineal, kg) + Precio.PrecioProcesoDVH("Armado", m2, metroLineal, kg);

                        //incluir Margen y cantidad
                        double factorC = iNFOcli.FactorMargen();
                        NetoItem = NetoItem * factorC * cantidad;
                        




                        //Castigo por dimension pequeña
                        if (Ancho <= 250 || Alto <= 250)
                        {
                            NetoItem = NetoItem + 20000;
                        }
                        else
                        {
                            if (Ancho <= 350 || Alto <= 350)
                            {
                                NetoItem = NetoItem + 5000;
                            }
                        }
                        
                        //Neto con despacho incluido
                       
                        NetoItemCDesp = NetoItem + Precio.CostoDespacho(Tipodesp, kg);
                        NetoDespacho = NetoDespacho + Precio.CostoDespacho(Tipodesp, kg);

                        drow["Neto"] = Math.Round(NetoItem);
                        drow["NetoCDesp"] = Math.Round(NetoItemCDesp);
                        NetoPedido = NetoPedido + Math.Round(NetoItem);
                        NetoPedidoCdesp = NetoPedidoCdesp + Math.Round(NetoItemCDesp);
                        retorno.Rows.Add(drow);

                    }
                    else
                    {

                    }
                }
                leerCadena.NextResult();
            }
            leerCadena.Close();
            cnn.Close();

        }
        catch (Exception e)
        {

        }



        ViewState["tabla"] = retorno;

        return retorno;
    }

    private void TablaEncabezado()
    {
        DataColumn col;
        DataRow drr;


        Tabla.TableName = "tabla";

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "Modelo";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "Cantidad";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "Ancho";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "Alto";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Decimal");
        col.ColumnName = "mt2";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "Detalle producto";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "Esp c1";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Decimal");
        col.ColumnName = "Esp sep";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "Esp c2";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Int32");
        col.ColumnName = "Codigo";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.String");
        col.ColumnName = "Kilos";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Double");
        col.ColumnName = "Neto";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);

        col = new DataColumn();
        col.DataType = System.Type.GetType("System.Double");
        col.ColumnName = "NetoCDesp";
        col.ReadOnly = true;
        Tabla.Columns.Add(col);



        ViewState["tabla"] = Tabla;






    }

   

  

    protected void BtnSolicitudCodExp_Click(object sender, EventArgs e)
    {

    }

    protected void BtnMisdirecciones_Click(object sender, EventArgs e)
    {

    }

    protected void DropDirecciones_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void BtnGuardarInfoEntrega_Click(object sender, EventArgs e)
    {

    }
}