using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de EcommList
/// </summary>
/// 
namespace Ecommerce
{
    public class T_temp_pedidos
    {
        public string _Id { get; set; }
        public string _UserId { get; set; }
        public string _Id_cliente { get; set; }
        public string _OrderName { get; set; }
        public DateTime _Fecha { get; set; }
        public string _TipoDespacho { get; set; }
        public string _Observacion { get; set; }
        public string _Estado { get; set; }
        public string _PedidoAlfak { get; set; }
        public string _Rutadj { get; set; }
        public string _DireccionDesp { get; set; }
        public DateTime? _FechaDesp { get; set; }
        public string _Token { get; set; }
    }

    public class T_temp_DetPedido
    {

        public string Id { get; set; }

        //Ingresados por el usuario

        public string _Referencia { get; set; }
        public string _Terminologia { get; set; }
        public int _Cantidad { get; set; }
        public double _Ancho { get; set; }
        public double _Alto { get; set; }
        public string _CodigoAlfak { get; set; }
        public bool _ExisteCode { get; set; }


        //calculados
        public double Neto { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }
        


        //calculados
        public double M2Item { get; set; }
        public double KilosItem { get; set; }
        public double PerimetroItem { get; set; }
        public double M2Unit { get; set; }
        public double KilosUnit { get; set; }
        public double PermietroUnit { get; set; }


    }

}
