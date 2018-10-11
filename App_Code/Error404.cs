using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web;

/// <summary>
/// Descripción breve de Error404
/// </summary>
public class Error404
{
    
    public static string Redireccion(string Master, string Mensaje)
    {
        string Redireccion= "~/404?Msj=" + Mensaje + "&Request=" + Master;

        return Redireccion;
    }
}