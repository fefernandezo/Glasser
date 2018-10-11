using Ecommerce;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

/// <summary>
/// Descripción breve de DatosDeServicios
/// </summary>
[WebService(Namespace = "http://www.phglass.cl/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]

[System.Web.Script.Services.ScriptService]
public class DatosDeServicios : System.Web.Services.WebService
{
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
    SqlCommand cmdAlfak;
    static SqlDataReader drAlfak;

    [WebMethod]
    public List<TablaPedidos> TodosLosPedidos(string Tipo)
    {
        string Select = "";
        if (Tipo == "Todos")
        {
            Select = "SELECT e_Pedidos.nom_pedido AS 'Nombre', e_Pedidos.PedidoAlfak AS 'Numero', e_Pedidos.fecha_hora_pedido AS 'FechaIng', CONVERT(varchar,e_Pedidos.fecha_entrega,105) AS 'FechaEntr'," +
            " e_Pedidos.tipo_despacho AS 'TipoDesp', e_Pedidos.observacion AS 'Observa'," +
            " e_Usuario.Usuario AS 'Usuario', e_Pedidos.Cantidad AS 'Cantidad', CAST(convert(int,ROUND(e_Pedidos.totalPedido,0)) AS VARCHAR) AS 'Total', e_Pedidos.Estado AS 'Estado'" +
            " FROM e_Pedidos INNER JOIN e_Usuario ON e_Pedidos.id_usuario = e_Usuario.ID" +
            " INNER JOIN e_TipoUsu ON e_Usuario.Id_Tipo = e_TipoUsu.ID" +
            " WHERE (e_TipoUsu.Rut = @rut) AND (e_Pedidos.Estado = 'ING' OR e_Pedidos.Estado = 'PRG' OR e_Pedidos.Estado = 'DIS') ORDER BY e_Pedidos.fecha_hora_pedido DESC ";
        }
        else
        {
            Select = "SELECT TOP 100 e_Pedidos.nom_pedido AS 'Nombre', e_Pedidos.PedidoAlfak AS 'Numero', e_Pedidos.fecha_hora_pedido AS 'FechaIng', CONVERT(varchar,e_Pedidos.fecha_entrega,105) AS 'FechaEntr'," +
            " e_Pedidos.tipo_despacho AS 'TipoDesp', e_Pedidos.observacion AS 'Observa'," +
            " e_Usuario.Usuario AS 'Usuario', e_Pedidos.Cantidad AS 'Cantidad', CAST(convert(int,ROUND(e_Pedidos.totalPedido,0)) AS VARCHAR) AS 'Total', e_Pedidos.Estado AS 'Estado'" +
            " FROM e_Pedidos INNER JOIN e_Usuario ON e_Pedidos.id_usuario = e_Usuario.ID" +
            " INNER JOIN e_TipoUsu ON e_Usuario.Id_Tipo = e_TipoUsu.ID" +
            " WHERE (e_TipoUsu.Rut = @rut) AND (e_Pedidos.Estado IN ('" + Tipo + "')) ORDER BY e_Pedidos.fecha_hora_pedido DESC ";
        }
        FuncUser Usu = new FuncUser();
        Infousuario Datosusuario = Usu.DatosUsuario();
        DataTable ListaTotal = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                adapter.SelectCommand.Parameters.AddWithValue("@rut", Datosusuario.Rutempresa);

                adapter.Fill(ListaTotal);



            }
            catch (Exception ex)
            {

            }

        }
        List<TablaPedidos> TotalPedidos = new List<TablaPedidos>();
        int y = 10;
        int x = 0;
        int z = 1;
        if (ListaTotal.Rows.Count > 0)
        {
            foreach (DataRow dr in ListaTotal.Rows)
            {
                TablaPedidos Pedidos = new TablaPedidos
                {
                    Nombre = dr["Nombre"].ToString(),
                    Numero = dr["Numero"].ToString(),
                    FechaIng = dr["FechaIng"].ToString(),
                    FechaEntr = dr["FechaEntr"].ToString(),
                    TipoDes = dr["TipoDesp"].ToString(),
                    Observa = dr["Observa"].ToString(),
                    Usuario = dr["Usuario"].ToString(),
                    Cantidad = dr["Cantidad"].ToString(),
                    Total = dr["Total"].ToString(),
                    Estado = dr["Estado"].ToString(),
                    Pagina = z
                };
                TotalPedidos.Add(Pedidos);
                x++;
                if (x == y)
                {
                    y = y + 10;
                    z = z + 1;
                }
            }
        }



        return TotalPedidos;
    }

    [WebMethod]
    public List<TablaPedidos> Pedidos(string Fdesde, string Fhasta, string Estados)
    {
        string Select = "";

        Select = "SELECT TOP 100 e_Pedidos.nom_pedido AS 'Nombre', e_Pedidos.PedidoAlfak AS 'Numero', e_Pedidos.fecha_hora_pedido AS 'FechaIng', CONVERT(varchar,e_Pedidos.fecha_entrega,105) AS 'FechaEntr'," +
        " e_Pedidos.tipo_despacho AS 'TipoDesp', e_Pedidos.observacion AS 'Observa'," +
        " e_Usuario.Usuario AS 'Usuario', e_Pedidos.Cantidad AS 'Cantidad', CAST(convert(int,ROUND(e_Pedidos.totalPedido,0)) AS VARCHAR) AS 'Total', e_Pedidos.Estado AS 'Estado'" +
        " FROM e_Pedidos INNER JOIN e_Usuario ON e_Pedidos.id_usuario = e_Usuario.ID" +
        " INNER JOIN e_TipoUsu ON e_Usuario.Id_Tipo = e_TipoUsu.ID" +
        " WHERE (e_TipoUsu.Rut = @rut) AND (e_Pedidos.Estado IN ('" + Estados + "')) AND (e_Pedidos.fecha_hora_pedido > @Fdesde) AND (e_Pedidos.fecha_hora_pedido < @Fhasta) ORDER BY e_Pedidos.fecha_hora_pedido DESC ";

        FuncUser Usu = new FuncUser();
        Infousuario Datosusuario = Usu.DatosUsuario();
        DataTable ListaTotal = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                adapter.SelectCommand.Parameters.AddWithValue("@rut", Datosusuario.Rutempresa);
                adapter.SelectCommand.Parameters.AddWithValue("@Fdesde", Fdesde);
                adapter.SelectCommand.Parameters.AddWithValue("@Fhasta", Fhasta);
                adapter.Fill(ListaTotal);



            }
            catch (Exception ex)
            {

            }

        }
        List<TablaPedidos> TotalPedidos = new List<TablaPedidos>();
        int y = 10;
        int x = 0;
        int z = 1;
        if (ListaTotal.Rows.Count > 0)
        {
            foreach (DataRow dr in ListaTotal.Rows)
            {
                TablaPedidos Pedidos = new TablaPedidos();
                Pedidos.Nombre = dr["Nombre"].ToString();
                Pedidos.Numero = dr["Numero"].ToString();
                Pedidos.FechaIng = dr["FechaIng"].ToString();
                Pedidos.FechaEntr = dr["FechaEntr"].ToString();
                Pedidos.TipoDes = dr["TipoDesp"].ToString();
                Pedidos.Observa = dr["Observa"].ToString();
                Pedidos.Usuario = dr["Usuario"].ToString();
                Pedidos.Cantidad = dr["Cantidad"].ToString();
                Pedidos.Total = dr["Total"].ToString();
                Pedidos.Estado = dr["Estado"].ToString();
                Pedidos.Pagina = z;
                TotalPedidos.Add(Pedidos);
                x++;
                if (x == y)
                {
                    y = y + 10;
                    z = z + 1;
                }
            }
        }



        return TotalPedidos;
    }


    [WebMethod]
    public List<TablaPedidos> Reclamos(string Fdesde, string Fhasta, string Estados)
    {
        string Select = "";

        Select = "SELECT TOP 100 e_Pedidos.nom_pedido AS 'Nombre', e_Pedidos.PedidoAlfak AS 'Numero', e_Pedidos.fecha_hora_pedido AS 'FechaIng', CONVERT(varchar,e_Pedidos.fecha_entrega,105) AS 'FechaEntr'," +
        " e_Pedidos.tipo_despacho AS 'TipoDesp', e_Pedidos.observacion AS 'Observa'," +
        " e_Usuario.Usuario AS 'Usuario', e_Pedidos.Cantidad AS 'Cantidad', CAST(convert(int,ROUND(e_Pedidos.totalPedido,0)) AS VARCHAR) AS 'Total', e_Pedidos.Estado AS 'Estado'" +
        " FROM e_Pedidos INNER JOIN e_Usuario ON e_Pedidos.id_usuario = e_Usuario.ID" +
        " INNER JOIN e_TipoUsu ON e_Usuario.Id_Tipo = e_TipoUsu.ID" +
        " WHERE (e_TipoUsu.Rut = @rut) AND (e_Pedidos.Estado IN ('" + Estados + "')) AND (e_Pedidos.fecha_hora_pedido > @Fdesde) AND (e_Pedidos.fecha_hora_pedido < @Fhasta) ORDER BY e_Pedidos.fecha_hora_pedido DESC ";

        FuncUser Usu = new FuncUser();
        Infousuario Datosusuario = Usu.DatosUsuario();
        DataTable ListaTotal = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                adapter.SelectCommand.Parameters.AddWithValue("@rut", Datosusuario.Rutempresa);
                adapter.SelectCommand.Parameters.AddWithValue("@Fdesde", Fdesde);
                adapter.SelectCommand.Parameters.AddWithValue("@Fhasta", Fhasta);
                adapter.Fill(ListaTotal);



            }
            catch (Exception ex)
            {

            }

        }
        List<TablaPedidos> TotalPedidos = new List<TablaPedidos>();
        int y = 10;
        int x = 0;
        int z = 1;
        if (ListaTotal.Rows.Count > 0)
        {
            foreach (DataRow dr in ListaTotal.Rows)
            {
                TablaPedidos Pedidos = new TablaPedidos();
                Pedidos.Nombre = dr["Nombre"].ToString();
                Pedidos.Numero = dr["Numero"].ToString();
                Pedidos.FechaIng = dr["FechaIng"].ToString();
                Pedidos.FechaEntr = dr["FechaEntr"].ToString();
                Pedidos.TipoDes = dr["TipoDesp"].ToString();
                Pedidos.Observa = dr["Observa"].ToString();
                Pedidos.Usuario = dr["Usuario"].ToString();
                Pedidos.Cantidad = dr["Cantidad"].ToString();
                Pedidos.Total = dr["Total"].ToString();
                Pedidos.Estado = dr["Estado"].ToString();
                Pedidos.Pagina = z;
                TotalPedidos.Add(Pedidos);
                x++;
                if (x == y)
                {
                    y = y + 10;
                    z = z + 1;
                }
            }
        }



        return TotalPedidos;
    }


    [WebMethod]
    public List<ConsultaDetallePedido> DetalleDelPedido(string codigo)
    {

        string consulta = @"SELECT ID,POS_KOMMISSION,PROD_ID,PROD_BEZ1,CONVERT(INT,PP_MENGE) AS 'PP_MENGE',CONVERT(INT,PP_BREITE) AS 'PP_BREITE',CONVERT(INT,PP_HOEHE) AS 'PP_HOEHE',CONVERT(DECIMAL(4,2),PP_QM) AS 'PP_QM',FI_NETTO FROM PHGLASS.SYSADM.BW_AUFTR_POS WHERE ID = @id ";
        DataTable TablaDetalle = new DataTable();
        using (ConnAlfak)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnAlfak);
                adaptador.SelectCommand.Parameters.AddWithValue("@id", codigo);

                adaptador.Fill(TablaDetalle);
            }
            catch (Exception ex)
            {

            }
        }



        List<ConsultaDetallePedido> tmp = new List<ConsultaDetallePedido>();

        if (TablaDetalle.Rows.Count > 0)
        {
            foreach (DataRow drAlf in TablaDetalle.Rows)
            {
                ConsultaDetallePedido lista = new ConsultaDetallePedido();
                lista.codigo = drAlf["ID"].ToString();
                lista.modelo = drAlf["POS_KOMMISSION"].ToString();
                lista.codPro = drAlf["PROD_ID"].ToString();
                lista.compos = drAlf["PROD_BEZ1"].ToString();
                lista.cantidad = drAlf["PP_MENGE"].ToString();
                lista.alto = drAlf["PP_BREITE"].ToString();
                lista.ancho = drAlf["PP_HOEHE"].ToString();
                lista.mt2 = drAlf["PP_QM"].ToString();
                lista.precio = drAlf["FI_NETTO"].ToString();
                tmp.Add(lista);

            }
        }


        return tmp;

    }

    [WebMethod]
    public List<EncabezadoPedido> EncabezadoPedido(string Npedido)
    {
        EncabezadoPedido Epedido = new EncabezadoPedido();
        string Select = "Select P.id_pedidos AS 'id' , P.nom_pedido AS 'nombre', P.fecha_hora_pedido AS 'fecha', T.Empresa AS 'empresa'," +
            "CONVERT(varchar,P.fecha_entrega,105)  AS 'fechaentrega', P.tipo_despacho AS 'tipodespacho', P.observacion AS 'observa', P.Estado AS 'estado'," +
            "P.Cantidad AS 'cantidad', P.totalPedido AS 'netototal'" +
            "FROM PLABAL.dbo.e_pedidos P, PLABAL.dbo.e_Usuario U, PLABAL.dbo.e_TipoUsu T " +
            "WHERE T.ID= U.Id_Tipo AND U.ID= P.id_usuario and P.PedidoAlfak= @Numero";
        DataTable Lista = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                adapter.SelectCommand.Parameters.AddWithValue("@Numero", Npedido);

                adapter.Fill(Lista);



            }
            catch (Exception ex)
            {

            }
        }
        List<EncabezadoPedido> EncPedido = new List<EncabezadoPedido>();
        if (Lista.Rows.Count > 0)
        {

            foreach (DataRow dr in Lista.Rows)
            {
                Epedido.Id = dr["id"].ToString();
                Epedido.Nombre = dr["nombre"].ToString();
                Epedido.Fecha = dr["fecha"].ToString();
                Epedido.Empresa = dr["empresa"].ToString();
                Epedido.FechaEntrega = dr["fechaentrega"].ToString();
                Epedido.TipoDespacho = dr["tipodespacho"].ToString();
                Epedido.Observacion = dr["observa"].ToString();
                Epedido.Estado = dr["estado"].ToString();
                Epedido.Cantidad = dr["cantidad"].ToString();
                Epedido.NetoTotal = dr["netototal"].ToString();
                Epedido.Npedido = Npedido;
                EncPedido.Add(Epedido);
            }
        }



        return EncPedido;
    }

    [WebMethod]
    public List<T_temp_DetPedido> GetListTempDetail(string ID, string TokenId)
    {
         
        Encriptacion Token = new Encriptacion(TokenId);
        Pedidotemporal pedido = new Pedidotemporal(ID, Token.DesEncriptado);
        List<T_temp_DetPedido> detalle = new List<T_temp_DetPedido>();
        detalle = pedido.Detalle;
        return detalle;
    }
}
