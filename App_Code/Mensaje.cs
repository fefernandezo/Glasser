using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de Mensaje
/// </summary>
public class Mensaje
{

    Coneccion Conn;

    public List<Mensajedetail> Mensajes { get; set; }

    public bool HasMessages { get; set; }


    public Mensaje()
    {

    }
    public Mensaje(string[] Users)
    {
        Mensajes = new List<Mensajedetail>();
        foreach (var item in Users)
        {
            GetMessagesforUser(item);
            string id = "";
        }
        
    }

    private void GetMessagesforUser(string Usuario)
    {
        string consulta;
        bool IsAvalaible = true;

        consulta = @"SELECT * FROM GLA_System_Msj WHERE UserName=@UserName AND IsAvailable=@IsAvailable";
        Conn = new Coneccion();
       

        DataTable TablaDetalle = new DataTable();
        using (Conn.ConnPlabal)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                adaptador.SelectCommand.Parameters.AddWithValue("@IsAvailable", IsAvalaible);
                adaptador.SelectCommand.Parameters.AddWithValue("@UserName", Usuario);
                adaptador.Fill(TablaDetalle);
                Conn.ConnPlabal.Close();
            }
            catch (Exception ex)
            {

            }
            if (TablaDetalle.Rows.Count>0)
            {
                HasMessages = true;
                foreach (DataRow drr in TablaDetalle.Rows)
                {
                    Mensajedetail mensaje = new Mensajedetail {
                        Cuerpo = drr["Message"].ToString(),
                        Id_Mensage = drr["Id"].ToString(),
                        From = drr["From"].ToString(),
                        HeadMsj = drr["HeadMsg"].ToString(),
                    };
                    Mensajes.Add(mensaje);
                    string istru = "";
                }
            }
        }

    }

    public void NullingMsj(string Id_Msj)
    {
        Conn = new Coneccion();
        #region InsertString
        string Insert = "UPDATE GLA_System_Msj SET IsAvailable=0 WHERE Id=@ID ";
        #endregion

        try
        {
            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Insert, Conn.ConnPlabal);
            #region Parameters
            Conn.CmdPlabal.Parameters.AddWithValue("@ID", Id_Msj);
            #endregion

            Conn.CmdPlabal.ExecuteNonQuery();
            Conn.ConnPlabal.Close();
        }
        catch (Exception EX)
        {
            
            ErrorCatching errorCatching = new ErrorCatching();
            errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
            Conn.ConnPlabal.Close();

        }
    }

    public void CreateMsj (string para, string from, string cuerpo)
    {
        Conn = new Coneccion();
        #region InsertString
        string Insert = "INSERT INTO GLA_System_Msj (UserName,From,Date,Message,IsAvailable) VALUES (@UserName,@From,GETDATE(),@Mensaje,1)";
        #endregion

        try
        {
            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Insert, Conn.ConnPlabal);
            #region Parameters
            Conn.CmdPlabal.Parameters.AddWithValue("@UserName", para);
            Conn.CmdPlabal.Parameters.AddWithValue("@From", from);
            Conn.CmdPlabal.Parameters.AddWithValue("@Mensaje", cuerpo);
            #endregion

            Conn.CmdPlabal.ExecuteNonQuery();
            Conn.ConnPlabal.Close();
        }
        catch (Exception EX)
        {

            ErrorCatching errorCatching = new ErrorCatching();
            errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
            Conn.ConnPlabal.Close();

        }
    }

    public class Mensajedetail
    {
        public string Cuerpo { get; set; }
        public string Id_Mensage { get; set; }
        public string From { get; set; }
        public string HeadMsj { get; set; }

    }
        

}