using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

/// <summary>
/// Descripción breve de ErrorCatching
/// </summary>
public class ErrorCatching : Page
{
    Coneccion Conn;

    static SqlDataReader drPlabal;
    SqlCommand cmdPlabal;

    public void ErrorCatch(string ErrorString, string URL)
    {
        string UserName = Page.User.Identity.Name;
        try
        {
            Conn = new Coneccion();
            string Insert = "Insert INTO PLABAL.dbo.App_Error_Catch VALUES (@ErrorString,@URL,@UserName,GETDATE())";
            Conn.ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Insert, Conn.ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@ErrorString", ErrorString);
            cmdPlabal.Parameters.AddWithValue("@URL", URL);
            cmdPlabal.Parameters.AddWithValue("@UserName", UserName);
            cmdPlabal.ExecuteNonQuery();
            Conn.ConnPlabal.Close();
        }
        catch (Exception ex)
        {
            throw ex;


        }
        
    }

}