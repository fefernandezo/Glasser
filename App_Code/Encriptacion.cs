using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

/// <summary>
/// Descripción breve de Encriptacion
/// </summary>
public class Encriptacion
{
    public string Encriptado { get; set; }
    public string DesEncriptado { get; set; }
    public string TokenEncriptado { get; set; }
    public string TokenDesencriptado { get; set; }

    public Encriptacion(string Palabra)
    {
        Encriptado = Proteccion(Palabra);
        DesEncriptado = Desproteccion(Palabra);
        TokenEncriptado = ProtectToken(Palabra);
        TokenDesencriptado = DesprotecToken(Palabra);
        
    }
    
    private static string Desproteccion(string tokenID)
    {
        try
        {
            var ByteSinProteccion = MachineKey.Decode(tokenID, MachineKeyProtection.All);
            return Encoding.ASCII.GetString(ByteSinProteccion);
        }
        catch
        {
            return null;
        }
    }

    private static string DesprotecToken(string String)
    {
        try
        {
            var ByteSinProteccion = HttpServerUtility.UrlTokenDecode(String);
            return Encoding.ASCII.GetString(ByteSinProteccion);
        }
        catch 
        {

            return null;
        }
    }

    private static string ProtectToken(string String)
    {
        try
        {
            var ByteSinProteccion = Encoding.ASCII.GetBytes(String);
            var ByteConProteccion = HttpServerUtility.UrlTokenEncode(ByteSinProteccion);


            return ByteConProteccion;

        }
        catch 
        {

            return null;
        }
    }

    private static string Proteccion(string tokenid)
    {
        var ByteSinProteccion = Encoding.ASCII.GetBytes(tokenid);
        var ByteConProteccion = MachineKey.Encode(ByteSinProteccion, MachineKeyProtection.All);
        
        
        return ByteConProteccion;
    }
}
public class RandomNumber
{
    public int Numero { get; set; }

    public RandomNumber(int digitos)
    {
        string A = "";
        string B = "";
        for (int i = 1; i <= digitos; i++)
        {
            if (i == 1)
            {
                A = "1";
                B = "9";
            }
            else
            {
                A = A + "0";
                B = B + "9";
            }

        }
        int inf = Convert.ToInt32(A);
        int sup = Convert.ToInt32(B);

        Random random = new Random();

        Numero = random.Next(inf, sup);

    }

}