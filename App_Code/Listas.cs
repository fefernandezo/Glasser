using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Listas
/// </summary>
public class TablaPedidos
{

    public string Nombre { get; set; }

    public string Numero { get; set; }

    public string FechaIng { get; set; }

    public string FechaEntr { get; set; }

    public string TipoDes { get; set; }

    public string Observa { get; set; }

    public string Usuario { get; set; }

    public string Cantidad { get; set; }

    public string Total { get; set; }

    public string Estado { get; set; }

    public int Pagina { get; set; }

}
public class TablaReclamos
{

}

public class EstadoOrden
{

    public int Total { get; set; }

    public int Fabricadas { get; set; }

    public int EnBodega { get; set; }

    public int Despachadas { get; set; }

}
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