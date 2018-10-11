using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

/// <summary>
/// Descripción breve de administrador
/// </summary>
public class Administrador : Page
{
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlCommand cmdPlabal;
    static SqlDataReader drPlabal;
    
    public DataTable RolesList()
    {
        DataTable tabla = new DataTable();
        
        string Select = "Select RoleId, Description FROM PLABAL.dbo.Roles WHERE Plataforma = 2";

        ConnPlabal.Open();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);
                
                adapter.Fill(tabla);



            }
            catch (Exception Ex)
            {
            Console.WriteLine(Ex);
            }

        ConnPlabal.Close();
        return tabla;
    }

    public DataTable Clientes()
    {
        DataTable tabla = new DataTable();

        string Select = "Select ID, Empresa FROM PLABAL.dbo.e_TipoUsu ORDER BY Empresa ASC";

        ConnPlabal.Open();
        
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);

                adapter.Fill(tabla);



            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }

        
        ConnPlabal.Close();
        return tabla;
    }

    public DataTable ListaUsers()
    {
        DataTable tabla = new DataTable();

        string Select = "Select U.UserName AS 'UserName', U.Nombre + ' ' + U.Apellido AS 'Usuario' FROM PLABAL.dbo.Users U, PLABAL.dbo.Memberships M WHERE M.UserId=U.UserId AND M.IsApproved =1  ORDER BY Nombre ASC";

        ConnPlabal.Open();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(Select, ConnPlabal);

                adapter.Fill(tabla);



            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        ConnPlabal.Close();
       
        return tabla;
    }


    public DataTable ClientesRandom(string VariableSearch)
    {
        string variable= "%" + VariableSearch.ToUpper() + "%";
        
        Hashtable Ht = new Hashtable() { { "VAR",variable} };
        SelectRows select = new SelectRows("GLASSER", "MAEEN", "KOEN AS 'RUT',NOKOEN AS 'CLIENTE',DIEN AS 'DIR', CIEN AS 'REG'",
            "TIEN='C' AND (NOKOEN LIKE @VAR OR KOEN LIKE @VAR) ORDER BY NOKOEN ASC",Ht);

        if (select.IsGot)
        {
            return select.Datos;
        }
        else
        {
            return null;
        }
        
    }

    public bool CreacionCliente (string nombre, string Rut, double Margen,string Direccion, string CodRegion, string CodComuna, bool PrecioxM2, string tratoDiasEntrega)
    {
        int dias = Convert.ToInt32(tratoDiasEntrega);
        Hashtable Ht = new Hashtable() {
            {"MARGEN", Margen },
            { "RUT",Rut},
            { "TIPOXLSDVH",1},
            { "ECOMRPROPIO",0},
            { "ESPRECIOXM2",PrecioxM2},
            { "DIRECCION",Direccion},
            { "COD_COMUNA",CodComuna},
            { "COD_REGION",CodRegion},
            { "TRATODIASENTREGA",dias},
            { "ESTADO",true}

        };
        InsertRow insert = new InsertRow("PLABAL", "ECOM_MAEMP",Ht);
        return insert.Insertado;
    }

    public void InsertUserInRoles(string usuario, string RoleId)
    {
        string UserId = "";
        

        string Select = "Select UserId " +
            "FROM PLABAL.dbo.Users " +
            " WHERE UserName = @User ";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@User", usuario);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            UserId = drPlabal[0].ToString();
        }
        
        drPlabal.Close();
        ConnPlabal.Close();


        ConnPlabal.Open();
        string Insert = "INSERT INTO PLABAL.dbo.UsersInRoles (UserId,RoleId) VALUES (@UserId,@RoleId)";
        cmdPlabal = new SqlCommand(Insert,ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@UserId",UserId);
        cmdPlabal.Parameters.AddWithValue("@RoleId", RoleId);
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();

    }

    public void InsertUserInemp(string usuario, string IdEmpresa)
    {

        string UserId = "";


        string Select = "Select UserId " +
            "FROM PLABAL.dbo.Users " +
            " WHERE UserName = @User ";

        ConnPlabal.Open();
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@User", usuario);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();

        if (drPlabal.HasRows)
        {
            UserId = drPlabal[0].ToString();
        }

        drPlabal.Close();
        ConnPlabal.Close();

        ConnPlabal.Open();
        string Insert = "INSERT INTO PLABAL.dbo.e_UserInEmp (UserId,IDTipoUsu) VALUES (@UserId,@Idemp)";
        cmdPlabal = new SqlCommand(Insert, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@UserId", UserId);
        cmdPlabal.Parameters.AddWithValue("@Idemp", IdEmpresa);
        cmdPlabal.ExecuteNonQuery();
        ConnPlabal.Close();


    }

    public void ActualizarUser(string UserName, string nombre, string apellido)
    {
        ConnPlabal.Open();
        string Insert = "UPDATE PLABAL.dbo.Users SET Nombre=@Nombre, Apellido=@Apellido WHERE UserName=@UserName ";
        bool valid=false;
        try
        {
            cmdPlabal = new SqlCommand(Insert, ConnPlabal);

            cmdPlabal.Parameters.AddWithValue("@Nombre", nombre);
            cmdPlabal.Parameters.AddWithValue("@Apellido", apellido);
            cmdPlabal.Parameters.AddWithValue("@UserName", UserName);

            cmdPlabal.ExecuteNonQuery();
            valid = true;
        }
        catch
        {
            valid = false;
        }
        ConnPlabal.Close();
        if(valid)
        {
            string[] Usern = new string[1];
            Usern[0] = UserName;
            Mensaje mensaje = new Mensaje(Usern);
            string Msj = "Estimado " + nombre + ", le damos la bienvenida a nuestra plataforma. Para su seguridad debe realizar el cambio de clave haciendo click en su nombre de usuario, en el costado derecho de la pantalla";
            mensaje.CreateMsj(UserName, "Plataforma", Msj);
        }
        

    }

    public ConsultaUser SelectUser (string Username)
    {
        ConsultaUser datos = new ConsultaUser();

         string Select = "Select U.Nombre, U.Apellido, M.Email " +
            "FROM PLABAL.dbo.Users U, PLABAL.dbo.Memberships M" +
            " WHERE U.UserName = @User AND U.UserId = M.UserId";

            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@User", Username);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

        if (drPlabal.HasRows)
        {
            datos.Nombre = drPlabal[0].ToString();
            datos.Apellido = drPlabal[1].ToString();
            datos.Correo = drPlabal[2].ToString();


            }
            else
            {

            }
            drPlabal.Close();
            ConnPlabal.Close();

        return datos;
    }

}

public class ConsultaUser
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }

}

