using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Configuration;

namespace ProRepo
{
    public class EncabezadoPedido
    {
        public string Nombre { get; set; }
        public string Fecha { get; set; }
        public string FechaEntrega { get; set; }
        public string TipoDespacho { get; set; }
        public string Observacion { get; set; }
        public string Npedido { get; set; }
        public string Estado { get; set; }
        public string NetoTotal { get; set; }
        public string Empresa { get; set; }
        public string Id { get; set; }
        public string Cantidad { get; set; }

    }

    public class ConsultaDetallePedido
    {
        public string codigo { get; set; }
        public string modelo { get; set; }
        public string codPro { get; set; }
        public string compos { get; set; }
        public string cantidad { get; set; }
        public string alto { get; set; }
        public string ancho { get; set; }
        public string mt2 { get; set; }
        public string precio { get; set; }
    }

    public class ListaFiltroEstado
    {
        public string nombre { get; set; }
        public string numero { get; set; }
        public string fecha { get; set; }
        public string fechaE { get; set; }
        public string TipoDesp { get; set; }
        public string observacion { get; set; }
    }

   

    public class Consultas
    {
        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnectionString"].ConnectionString);

        public DataTable Pedidos(DateTime Fdesde, DateTime Fhasta, string Estados)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
            string Select = "";

            Select = "SELECT TOP 1000 e_Pedidos.nom_pedido AS 'Pedido'," +
                " e_Pedidos.PedidoAlfak AS 'Numero', " +
                "e_Pedidos.fecha_hora_pedido AS 'Fecha'," +
               " e_Pedidos.tipo_despacho AS 'TipoDesp'," +
               " e_Pedidos.observacion AS 'Observa'," +
            " e_Usuario.Usuario AS 'Usuario'," +
            " e_Pedidos.Cantidad AS 'Cantidad', " +
            "'$' + replace(replace( convert( varchar(32), cast(cast( e_Pedidos.totalPedido AS varchar(32)) AS money), 1 ), '.00', '' ),',','.')  AS 'Neto'," +
            " CASE WHEN  e_Pedidos.Estado='ING' THEN 'Ingresado'" +
            " WHEN  e_Pedidos.Estado='PRG' THEN 'En fabricación'" +
            " WHEN  e_Pedidos.Estado='DIS' THEN 'Para despacho'" +
            " WHEN  e_Pedidos.Estado='DES' THEN 'Entregado' END AS 'Estado'" +
            " FROM e_Pedidos INNER JOIN e_Usuario ON e_Pedidos.id_usuario = e_Usuario.ID" +
            " INNER JOIN e_TipoUsu ON e_Usuario.Id_Tipo = e_TipoUsu.ID" +
            " WHERE (e_TipoUsu.Rut = @rut) AND (e_Pedidos.Estado IN (" + Estados + ")) AND (e_Pedidos.fecha_hora_pedido > @Fdesde) AND (e_Pedidos.fecha_hora_pedido < @Fhasta) ORDER BY e_Pedidos.Estado, e_Pedidos.fecha_hora_pedido ASC ";

            FuncUser Usu = new FuncUser();
            Infousuario Datosusuario = Usu.DatosUsuario();
            DataTable ListaTotal = new DataTable();
            using (ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                    adapter.SelectCommand.Parameters.AddWithValue("@rut", Datosusuario.Rutempresa);
                    adapter.SelectCommand.Parameters.AddWithValue("@Fdesde", Fdesde.ToString("MM-dd-yyyy"));
                    adapter.SelectCommand.Parameters.AddWithValue("@Fhasta", Fhasta.ToString("MM-dd-yyyy"));
                    adapter.Fill(ListaTotal);



                }
                catch (Exception ex)
                {

                }

            }





            return ListaTotal;
        }
    }


    //------------------------------------------WSlogistica------------------------------>
    public class WSL_Producto
    {
        public bool Existe { get; set; }
        public string Nombre { get; set; }
        public string UnidMed { get; set; }
        public string Codigo { get; set; }

    }

    public class WSL_Rutas
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CodBodega { get; set; }
        public string Descripcion { get; set; }
    }

    public class WSL_Validator
    {
        public bool Result { get; set; }
    }

    public class WSL_User
    {
        public bool Validate { get; set; }
        public string Id_tab { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }

    public class WSL_IngProdInventory
    {
        public bool Validate { get; set; }
        public string IdOperacion { get; set; }
    }

    public class WSL_TodosInventarios
    {
        public string _ID { get; set; }
        public string _Nombre { get; set; }
        public string _Descripcion { get; set; }
        public string _Creacion { get; set; }
        public string _Inicio { get; set; }
        public string _Termino { get; set; }
        public string _Status { get; set; }
        public string _Sucursal { get; set; }


    }

}