using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de LogListas
/// </summary>
/// 

namespace Logistica
{
    public class DetalleInv
    {
        public string _Id { get; set; }
        public string _KOPR { get; set; }
        public string _RNDDescripcion { get; set; }
        public double _Cant { get; set; }
        public string _Operario { get; set; }
        public string _Fecha { get; set; }
        public string _Unidad { get; set; }
        public string _Bodega { get; set; }
        public string _CodBodega { get; set; }
    }

    public class Inventario
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string KOSU { get; set; }
    }

    public class Bodega
    {
        public string _Name { get; set; }
        public string _KOBO { get; set; }
        public string _EMPRESA { get; set; }
        public string _KOSU { get; set; }
        public string _Address { get; set; }
        public string COMBINADO { get; set; }

    }

    public class Sucursal
    {
        public string _EMPRESA { get; set; }
        public string _KOSU { get; set; }
        public string _KOFUSU { get; set; }
        public string _Name { get; set; }
        public string _CISU { get; set; }
        public string _CMSU { get; set; }
        public string _Address { get; set; }
        public string _Phone { get; set; }
        public string COMBINADO { get; set; }

    }

    public class Ruta
    {
        public string _Id { get; set; }
        public string _Nombre { get; set; }
        public string _CodBodega { get; set; }
        public string _Descripcion { get; set;}
        public string _CodSUCU { get; set; }
        public string IdAsign { get; set; }
    }

    public class DetalleInvItem
    {
        public int _Items { get; set; }
    }

    public class UsuariosInv
    {
        public string _Id { get; set; }
        public string _UserName { get; set; }
        public string _Name { get; set; }
        
    }

    public class RutasUserInv
    {
        public string _Id_Asignruta { get; set; }
        public string _Id_Asign_user { get; set; }
        public string _RutaName { get; set; }
        public string _Bodega { get; set; }
        public string _UserName { get; set; }
        public string _DescripRuta { get; set; }
        
    }
}

