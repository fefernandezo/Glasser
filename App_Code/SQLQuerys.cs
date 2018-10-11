using GlobalInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Update
/// </summary>

public class UpdateRow
{
    Coneccion Conn;
    SqlConnection SqlConn;
    public readonly bool Actualizado;
    public readonly string ExMessage;
    private readonly string Query;
    private readonly string DataBase;
    private readonly object[] Parametros;
    private readonly string[] ParamName;
    public UpdateRow(string _DataBase, string _Tabla, string _SET, string _WHERE, object[] _Parametros, string[] _ParamName)
    {
        DataBase = _DataBase;
        Parametros = _Parametros;
        ParamName = _ParamName; 
        Query = "UPDATE " + _Tabla + " SET " + _SET + " WHERE " + _WHERE;

        object[] Actualiza = Accion();
        Actualizado = Convert.ToBoolean(Actualiza[0]);
        ExMessage = Actualiza[1].ToString();

    }

    public UpdateRow(string _DataBase, string _Query, object[] _Parametros, string[] _ParamName)
    {
        DataBase = _DataBase;
        Query = _Query;
        Parametros = _Parametros;
        ParamName = _ParamName;

        object[] Actualiza = Accion();
        Actualizado = Convert.ToBoolean(Actualiza[0]);
        ExMessage = Actualiza[1].ToString();

    }

    public UpdateRow(string _DataBase, string _Query, Hashtable Datos)
    {
        DataBase = _DataBase;
        Query = _Query;
        int Cant = Datos.Count;
        ParamName = new string[Cant];
        Parametros = new object[Cant];
        
        int cont = 0;
        foreach (string key in Datos.Keys)
        {
            ParamName[cont] = key;
            Parametros[cont] = Datos[key];
            cont++;
        }


        object[] Actualiza = Accion();
        Actualizado = Convert.ToBoolean(Actualiza[0]);
        ExMessage = Actualiza[1].ToString();

    }

    private object[] Accion()
    {
        Conn = new Coneccion();
        if (DataBase == "ALFAK")
        {
            SqlConn = Conn.ConnAlfak;
        }
        else if (DataBase == "PLABAL")
        {
            SqlConn = Conn.ConnPlabal;
        }
        else if (DataBase == "GLASSER")
        {
            SqlConn = Conn.ConnGlasser;
        }
        else
        {
            SqlConn = null;
        }

        object[] Retorno = new object[2];

        try
        {
            SqlConn.Open();
            Conn.Cmd = new SqlCommand(Query, SqlConn);
            for (int i = 0; i < ParamName.Count(); i++)
            {
                Conn.Cmd.Parameters.AddWithValue("@" + ParamName[i], Parametros[i]);
            }
            Conn.Cmd.ExecuteNonQuery();
            Conn.ConnPlabal.Close();
            Retorno[0] = true;
            Retorno[1] = "";
            SqlConn.Close();

        }
        catch (Exception ex)
        {
            Retorno[0] = false;
            Retorno[1] = ex.Message;

        }

        return Retorno;

    }
    

    
}
public class InsertRow
{
    Coneccion Conn;
    SqlConnection SqlConn;
    public bool Insertado;
    public string ExMessage;

    private string Query;
    private readonly string Tabla;
    private readonly string Columnas;
    private readonly string Valores;
    private readonly string DataBase;
    private readonly string[] ParamName;
    private readonly object[] Parametros;

    public InsertRow(string _DataBase, string _Tabla, string _Columnas, string _Valores, object[] _Parametros, string[] _ParamName)
    {
        Tabla = _Tabla;
        Columnas = _Columnas;
        Valores = _Valores;
        DataBase = _DataBase;
        ParamName = _ParamName;
        Parametros = _Parametros;
        Insertar();
    }

    private void Insertar()
    {
        Query = "INSERT INTO " + Tabla + " (" + Columnas + ") VALUES (" + Valores + ")";
        Conn = new Coneccion();
        if (DataBase == "ALFAK")
        {
            SqlConn = Conn.ConnAlfak;
        }
        else if (DataBase == "PLABAL")
        {
            SqlConn = Conn.ConnPlabal;
        }
        else if (DataBase == "RANDOM")
        {
            SqlConn = Conn.ConnGlasser;
        }
        else
        {
            SqlConn = null;
        }

        try
        {
            SqlConn.Open();
            Conn.Cmd = new SqlCommand(Query, SqlConn);
            for (int i = 0; i < ParamName.Count(); i++)
            {
                Conn.Cmd.Parameters.AddWithValue("@" + ParamName[i], Parametros[i]);
            }
            Conn.Cmd.ExecuteNonQuery();
            Conn.ConnPlabal.Close();
            Insertado = true;


        }
        catch (Exception ex)
        {
            ExMessage = ex.Message;
            Insertado = false;

        }

    }

    public InsertRow(string _DataBase, string _Tabla, Hashtable Datos)
    {
        DataBase = _DataBase;
        Tabla = _Tabla;
        int Cant = Datos.Count;
        ParamName = new string[Cant];
        Parametros = new object[Cant];
        string[] Values = new string[Cant];
        int cont = 0;
        foreach (string key in Datos.Keys)
        {
            ParamName[cont] = key;
            Parametros[cont] = Datos[key];
            Values[cont] = "@" + key;
            cont++;
        }
        Valores = string.Join(",", Values);
        Columnas = string.Join(",",ParamName);

        Insertar();

    }
}

public class SelectRows
{
    #region Manejo de datos
    Coneccion Conn;
    private string Query;
    public string ExMessage;
    public readonly bool IsGot;
    public readonly DataTable Datos;
    private SqlConnection SqlConn;
    private SqlCommand cmd;

    private readonly string DB;
    
    private readonly object[] PARAM;
    private readonly string[] PARAMNANE;
    
    private readonly Hashtable Htable;
    
    
    #endregion

    public SelectRows(string DataBase, string Tabla,string Columns,string Where, object[] Parametros, string[] ParamName, string Order_By)
    {
        DB = DataBase;
        cmd = new SqlCommand();
        Query = "SELECT " + Columns + " FROM " + Tabla + " WHERE " + Where + " ORDER BY " + Order_By;
        PARAM = Parametros;
        PARAMNANE = ParamName;
        Datos = Select();
        if (Datos.Rows.Count>0)
        {
            IsGot = true;
        }
        
        

    }
    public SelectRows(string DataBase, string Tabla, string Columns, string Where, object[] Parametros, string[] ParamName)
    {
        DB = DataBase;
        cmd = new SqlCommand();
        Query = "SELECT " + Columns + " FROM " + Tabla + " WHERE " + Where;
        PARAM = Parametros;
        PARAMNANE = ParamName;
        Datos = Select();
        if (Datos.Rows.Count > 0)
        {
            IsGot = true;
        }



    }

    public SelectRows(string DataBase, string Tabla, string Columns,string Order_By)
    {
        DB = DataBase;
        cmd = new SqlCommand();
        Query = "SELECT " + Columns + " FROM " + Tabla + " ORDER BY " + Order_By;
        Datos = Select();
        if (Datos.Rows.Count > 0)
        {
            IsGot = true;
        }

    }

    public SelectRows(string DataBase, string Tabla, string Columns)
    {
        DB = DataBase;
        cmd = new SqlCommand();
        Query = "SELECT " + Columns + " FROM " + Tabla;
        Datos = Select();
        if (Datos.Rows.Count > 0)
        {
            IsGot = true;
        }

    }

    public SelectRows(string DataBase,string Tabla, string Columns, string Where, Hashtable Parametros)
    {
        DB = DataBase;
        Query = "SELECT " + Columns + " FROM " + Tabla + " WHERE " + Where;
        Htable = Parametros;
        cmd = new SqlCommand();
        Datos = Select();
        if (Datos.Rows.Count > 0)
        {
            IsGot = true;
        }
    }

    public SelectRows(string DataBase,string ProcAlmacenado, Hashtable Parametros)
    {
        DB = DataBase;
        Query = ProcAlmacenado;
        Htable = Parametros;
        cmd = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure
        };

        Datos = Select();
        if (Datos.Rows.Count > 0)
        {
            IsGot = true;
        }
    }

    private DataTable Select()
    {

        #region Coneccion
        Conn = new Coneccion();
        if (DB == "ALFAK")
        {
            SqlConn = Conn.ConnAlfak;
        }
        else if (DB == "PLABAL")
        {
            SqlConn = Conn.ConnPlabal;
        }
        else if (DB == "GLASSER")
        {
            SqlConn = Conn.ConnGlasser;
        }
        else if (DB=="RANDOM")
        {
            SqlConn = Conn.ConnGlasser;
        }
        else
        {
            SqlConn = null;
        }
        #endregion



        using (SqlConn)
        {
            DataTable DT = new DataTable();
            SqlDataAdapter adap = new SqlDataAdapter();
            try
            {

                cmd = new SqlCommand(Query, SqlConn);
                
                
                if (PARAM != null)
                {
                    for (int i = 0; i < PARAM.Count(); i++)
                    {
                        cmd.Parameters.AddWithValue("@" + PARAMNANE[i], PARAM[i]);
                        
                    }
                }
                else if (Htable != null)
                {
                    foreach (string key in Htable.Keys)
                    {
                        cmd.Parameters.AddWithValue("@" + key, Htable[key]);
                    }
                }

                adap.SelectCommand = cmd;
                
                adap.Fill(DT);
                return DT;
            }
            catch (Exception ex)
            {
                ExMessage = ex.Message;
            }
            return DT;
        }


    }
}
public class UpdatePLABALRow
{
    Coneccion Conn;
    SqlConnection SqlConn;
    public readonly bool Actualizado;
    public readonly string ExMessage;
    private readonly string RowID;
    private readonly string Campo;
    private readonly object Valor;
    private readonly string Tabla;
    private readonly object ID;

    

    public UpdatePLABALRow(string _Tabla, string _RowIDName, object _ID, string _Campo, object _Valor)
    {
        RowID = _RowIDName;
        Campo = _Campo;
        Valor = _Valor;
        Tabla = _Tabla;
        ID = _ID;

        Actualizado = Update();
    }

    private bool Update()
    {
        bool IsUpdated = false;
        #region Query
        string Query = "UPDATE " + Tabla + " SET " + Campo + "=@Valor WHERE " + RowID + "=@ID ";
        #endregion
        Conn = new Coneccion();
        try
        {
            Conn.ConnPlabal.Open();
            Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
            Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
            Conn.Cmd.Parameters.AddWithValue("@ID", ID);

            Conn.Cmd.ExecuteNonQuery();
            Conn.ConnPlabal.Close();

            IsUpdated = true;

        }
        catch (Exception ex)
        {

            IsUpdated = false;
        }

        return IsUpdated;
    }

}