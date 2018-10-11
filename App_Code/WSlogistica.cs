using Logistica;
using ProRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using Logistica;

/// <summary>
/// Descripción breve de WSlogistica
/// </summary>
[WebService(Namespace = "http://www.phglass.cl/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]

[System.Web.Script.Services.ScriptService]
public class WSlogistica : System.Web.Services.WebService
{
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnRANDOM = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlConnection ConnRANDOM2 = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    string StrPlabalConn = "PLABALConnection";
    SqlCommand cmdPlabal;
    SqlCommand cmdGlasser;
    static SqlDataReader drPlabal;
    static SqlDataReader drGlasser;

    [WebMethod]
    public List<WSL_Producto> Producto(string codigo)
    {
        List<WSL_Producto> tmp = new List<WSL_Producto>();
        WSL_Producto lista = new WSL_Producto();

        #region TablaMAEPR
        string consulta = @"SELECT NOKOPR,UD01PR,KOPR FROM GLASSER.dbo.MAEPR WHERE KOPR = @codigo ";
        DataTable TablaDetalle = new DataTable();
        using (ConnRANDOM)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnRANDOM);
                adaptador.SelectCommand.Parameters.AddWithValue("@codigo", codigo);

                adaptador.Fill(TablaDetalle);
            }
            catch (Exception ex)
            {

            }
        }
        if (TablaDetalle.Rows.Count > 0)
        {
            foreach (DataRow drRAN in TablaDetalle.Rows)
            {

                {
                    lista.Nombre = drRAN["NOKOPR"].ToString();
                    lista.UnidMed = drRAN["UD01PR"].ToString();
                    lista.Codigo = drRAN["KOPR"].ToString();
                    lista.Existe = true;
                };


            }
        }
        else
        {
            string consulta2 = @"SELECT MA.NOKOPR AS 'NOKOPR',MA.UD01PR AS 'UD01PR', MA.KOPR AS 'KOPR' FROM GLASSER.dbo.MAEPR MA, GLASSER.dbo.POTL PO WHERE (CONVERT(VARCHAR,PO.NUMOT) + CONVERT(VARCHAR,PO.NREG)) = @codigo AND PO.CODIGO=MA.KOPR ";
            DataTable TablaDetalle2 = new DataTable();
            using (ConnRANDOM2)
            {
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(consulta2, ConnRANDOM2);
                    adaptador2.SelectCommand.Parameters.AddWithValue("@codigo", codigo);

                    adaptador2.Fill(TablaDetalle2);
                }
                catch (Exception ex)
                {

                }
                if (TablaDetalle2.Rows.Count > 0)
                {
                    foreach (DataRow drRAN in TablaDetalle2.Rows)
                    {

                        {
                            lista.Nombre = drRAN["NOKOPR"].ToString();
                            lista.UnidMed = drRAN["UD01PR"].ToString();
                            lista.Codigo = drRAN["KOPR"].ToString();
                            lista.Existe = true;
                        };


                    }
                }
                else
                {
                    lista.Existe = false;
                }
            }


        }
        #endregion






        tmp.Add(lista);
        return tmp;
    }

    [WebMethod]
    public List<WSL_Rutas> Rutas(string operario)
    {
        List<WSL_Rutas> tmp = new List<WSL_Rutas>();

        string consulta = @"SELECT B.Id, D.Nombre,D.CodBodega,D.Descripcion
                                FROM PLABAL.dbo.LO_invasignoperario A, PLABAL.dbo.LO_inventory C, PLABAL.dbo.LO_Asignruta B, PLABAL.dbo.LO_rutasinvent D
                                WHERE A.Id_e_Usuario=@operario AND A.ID_Asignruta=B.Id AND B.ID_Inventory=C.Id AND B.ID_Ruta=D.Id AND C.Status=0 AND B.Status=0";
        DataTable TablaDetalle = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnPlabal);
                adaptador.SelectCommand.Parameters.AddWithValue("@operario", operario);

                adaptador.Fill(TablaDetalle);
            }
            catch (Exception ex)
            {

            }
        }

        if (TablaDetalle.Rows.Count > 0)
        {
            foreach (DataRow drRAN in TablaDetalle.Rows)
            {
                WSL_Rutas lista = new WSL_Rutas
                {
                    Id = drRAN["Id"].ToString(),
                    Nombre = drRAN["Nombre"].ToString(),
                    CodBodega = drRAN["CodBodega"].ToString(),
                    Descripcion = drRAN["Descripcion"].ToString()
                };

                tmp.Add(lista);

            }
        }

        return tmp;
    }

    [WebMethod]
    public List<Ruta> RutasXInventario(string IdInventario)
    {
        List<Ruta> tmp = new List<Ruta>();

        string consulta = @"SELECT D.Id, D.Nombre,D.CodBodega,D.Descripcion, D.CodSUCU,B.Id as 'IdAsign'
                                FROM LO_Asignruta B, LO_rutasinvent D
                                WHERE B.ID_Ruta=D.Id AND B.ID_Inventory=@IdInventario and B.Status=0";
        DataTable TablaDetalle = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnPlabal);
                adaptador.SelectCommand.Parameters.AddWithValue("@IdInventario", IdInventario);

                adaptador.Fill(TablaDetalle);
            }
            catch (Exception ex)
            {

            }
        }

        if (TablaDetalle.Rows.Count > 0)
        {
            LogClass logClass = new LogClass();
            List<Sucursal> sucursals = logClass.GetListSucursal();

            foreach (DataRow drRAN in TablaDetalle.Rows)
            {
                var fff = (from a in sucursals where a._KOSU == drRAN["CodSUCU"].ToString() select a).First();
                List<Bodega> bodegas = logClass.GetListBodega(fff._KOSU);
                var bbb = (from a in bodegas where a._KOBO == drRAN["CodBodega"].ToString() select a).First();
                Ruta lista = new Ruta
                {
                    _Id = drRAN["Id"].ToString(),
                    _Nombre = drRAN["Nombre"].ToString(),
                    _CodBodega = bbb._KOBO + " - " + bbb._Name,
                    _Descripcion = drRAN["Descripcion"].ToString(),
                    _CodSUCU = fff._Name,
                    IdAsign= drRAN["IdAsign"].ToString(),

                };

                tmp.Add(lista);

            }
        }

        return tmp;
    }

    [WebMethod]
    public List<WSL_IngProdInventory> IngresoInventario(string Idasign, string User, string Kopr, string Cant, string Unit, string Token)
    {
        List<WSL_IngProdInventory> tmp = new List<WSL_IngProdInventory>();
        if (Token == "BaPRiThWJLYvy3KOn04K43we4XtyMdJIiYzqFwvLH0")
        {



            string Insert = "Insert INTO PLABAL.dbo.LO_Tomainventory (Id_Asign, Cod_oper, KOPR, Cant, Unidad, Timestamp, Estado) VALUES (@Id_Asign,@Cod_oper,@KOPR,@Cant,@Unit,GETDATE(),0)";
            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Insert, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@Id_Asign", Idasign);
            cmdPlabal.Parameters.AddWithValue("@Cod_oper", User);
            cmdPlabal.Parameters.AddWithValue("@KOPR", Kopr);
            cmdPlabal.Parameters.AddWithValue("@Cant", Cant);
            cmdPlabal.Parameters.AddWithValue("@Unit", Unit);
            cmdPlabal.ExecuteNonQuery();
            ConnPlabal.Close();

            string Select = "Select TOP 1 Id FROM PLABAL.dbo.LO_Tomainventory WHERE Id_Asign=@Id_Asign AND Cod_oper=@Cod_oper AND KOPR=@KOPR ORDER BY Id DESC";

            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@Id_Asign", Idasign);
            cmdPlabal.Parameters.AddWithValue("@Cod_oper", User);
            cmdPlabal.Parameters.AddWithValue("@KOPR", Kopr);

            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();
            WSL_IngProdInventory datos = new WSL_IngProdInventory();
            if (drPlabal.HasRows)
            {
                datos.Validate = true;
                datos.IdOperacion = drPlabal[0].ToString();
            }
            else
            {
                datos.Validate = false;
            }

            tmp.Add(datos);
        }
        else
        {
            WSL_IngProdInventory datos = new WSL_IngProdInventory
            {
                Validate = false,
            };

            tmp.Add(datos);
        }



        return tmp;
    }



    [WebMethod]
    public List<WSL_User> LoginApp(string UserName, string Password)
    {
        List<WSL_User> Result = new List<WSL_User>();
        ConnPlabal.Open();
        string Select = @"Select ID, Nombre,Correo FROM PLABAL.dbo.e_Usuario WHERE Usuario=@UserName AND Clave=@Password AND Estado=3";
        cmdPlabal = new SqlCommand(Select, ConnPlabal);
        cmdPlabal.Parameters.AddWithValue("@UserName", UserName);
        cmdPlabal.Parameters.AddWithValue("@Password", Password);
        drPlabal = cmdPlabal.ExecuteReader();
        drPlabal.Read();
        WSL_User User = new WSL_User();
        if (drPlabal.HasRows)
        {

            User.Validate = true;
            User.Nombre = drPlabal[1].ToString().Trim();
            User.Correo = drPlabal[2].ToString().Trim();
            User.Id_tab = drPlabal[0].ToString();
        }
        else
        {
            User.Validate = false;
            User.Nombre = "";
            User.Correo = "";
        }
        Result.Add(User);
        drPlabal.Close();
        ConnPlabal.Close();


        return Result;
    }

    [WebMethod]
    public List<WSL_TodosInventarios> AllInventories(string status)
    {
        List<WSL_TodosInventarios> tmp = new List<WSL_TodosInventarios>();
        string consulta = "";
        if (status=="Todos")
        {
            consulta = @"SELECT Id,Nombre,Descripcion,CreacionDate,InicioDate,TerminoDate,Status,KOSU FROM PLABAL.dbo.LO_inventory WHERE Status IN ('0','1') ORDER BY Id";
        }
        else if (status=="0")
        {
            consulta = @"SELECT Id,Nombre,Descripcion,CreacionDate,InicioDate,TerminoDate,Status,KOSU FROM PLABAL.dbo.LO_inventory WHERE Status='0' ORDER BY Id";
        }
        else if (status == "1")
        {
            consulta = @"SELECT Id,Nombre,Descripcion,CreacionDate,InicioDate,TerminoDate,Status,KOSU FROM PLABAL.dbo.LO_inventory WHERE Status='1' ORDER BY Id";
        }


        DataTable TablaDetalle = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnPlabal);
                
                adaptador.Fill(TablaDetalle);
            }
            catch (Exception ex)
            {

            }
        }

        if (TablaDetalle.Rows.Count > 0)
        {
            LogClass logClass = new LogClass();
            List<Sucursal> sucursals = logClass.GetListSucursal();
            
            foreach (DataRow drRAN in TablaDetalle.Rows)
            {
                string _StrInicio;
                string _StrTermino;
                if (!string.IsNullOrEmpty(drRAN["InicioDate"].ToString()))
                {
                    DateTime Inicio = Convert.ToDateTime(drRAN["InicioDate"].ToString());
                    _StrInicio = Inicio.ToShortDateString();
                }
                else
                {
                    _StrInicio = "SIN FECHA";
                }
                if(!string.IsNullOrEmpty(drRAN["TerminoDate"].ToString()))
                {
                    DateTime Termino = Convert.ToDateTime(drRAN["TerminoDate"].ToString());
                    _StrTermino = Termino.ToShortDateString();
                }
                else
                {
                    _StrTermino = "SIN FECHA";
                }
                string Status;
                if (drRAN["Status"].ToString() == "0")
                {
                    Status = "Activo";
                }
                else if (drRAN["Status"].ToString() == "1")
                {
                    Status = "Cerrado";
                }
                else
                {
                    Status = "";
                }

                var fff = (from a in sucursals where a._KOSU == drRAN["KOSU"].ToString() select a).First();

                WSL_TodosInventarios lista = new WSL_TodosInventarios
                {
                    _ID= drRAN["Id"].ToString(),
                    _Nombre = drRAN["Nombre"].ToString(),
                    _Descripcion= drRAN["Descripcion"].ToString(),
                    _Creacion= drRAN["CreacionDate"].ToString(),
                    _Inicio = _StrInicio, 
                    _Termino= _StrTermino,
                    _Status=Status,
                    _Sucursal=fff._Name,
                    

                };
               
               
                

                tmp.Add(lista);
            }
        }

        return tmp;
    }

    [WebMethod]
    public List<DetalleInv> DetalleInventario(string IdInventario, string Token, int Page)
    {
        
        List<DetalleInv> tmp = new List<DetalleInv>();
        if (Token== "BaPRiThWJLYvy3KOn04K43we4XtyMdJIiYzqFwvLH0")
        {
            int desde;
            int hasta;
            if(Page==1)
            {
                desde = 1;
                hasta = 19;

            }
            else
            {
                desde = (Page - 1) * 20;
                hasta = desde + 19;
            }

            string consulta = "SELECT * FROM(SELECT    ROW_NUMBER() OVER(ORDER BY A.KOPR) AS RowNum, A.ID, A.KOPR, A.Cant, A.Unidad,A.Timestamp, B.Nombre AS 'Operario', D.Nombre AS 'Bodega', D.CodBodega, D.CodSUCU " +
                "FROM PLABAL.dbo.LO_Tomainventory A, PLABAL.dbo.e_Usuario B, PLABAL.dbo.LO_Asignruta C, PLABAL.dbo.LO_rutasinvent D WITH(NOLOCK) " +
                "WHERE A.Cod_oper = B.ID AND A.Estado=0 AND C.Status=0 AND A.Id_Asign = C.Id AND C.ID_Inventory =@IDInv AND C.ID_Ruta = D.Id) " +
                "AS RowConstrainedResult " +
                "WHERE RowNum >=@Desde " +
                "AND RowNum< @Hasta " +
                "ORDER BY RowNum";



            DataTable TablaDetalle = new DataTable();
            using (ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDInv", IdInventario);
                    adaptador.SelectCommand.Parameters.AddWithValue("@Desde", desde);
                    adaptador.SelectCommand.Parameters.AddWithValue("@Hasta", hasta);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                foreach (DataRow drRAN in TablaDetalle.Rows)
                {
                    string KOPR = drRAN["KOPR"].ToString().Trim();
                    string KOBO = drRAN["CodBodega"].ToString().Trim();
                    string KOSU = drRAN["CodSUCU"].ToString().Trim();

                    DetalleInv lista = new DetalleInv
                    {
                        _Id = drRAN["ID"].ToString(),
                        _KOPR = KOPR,
                        _Cant = Convert.ToDouble(drRAN["Cant"].ToString()),
                        _Unidad = drRAN["Unidad"].ToString(),
                        _Fecha = Convert.ToDateTime(drRAN["Timestamp"].ToString()).ToShortDateString(),
                        _Bodega = drRAN["Bodega"].ToString().Trim(),
                        _CodBodega = KOBO,
                        _Operario = drRAN["Operario"].ToString(),

                    };

                    string Select = "SELECT TOP 1 PR.NOKOPR FROM MAEPR PR WITH ( NOLOCK )  WHERE PR.KOPR=@KOPR ";
                    ConnGlasser.Open();
                    cmdGlasser = new SqlCommand(Select, ConnGlasser);
                    cmdGlasser.Parameters.AddWithValue("@KOPR", KOPR);
                    
                    drGlasser = cmdGlasser.ExecuteReader();
                    drGlasser.Read();

                    if (drGlasser.HasRows)
                    {
                        lista._RNDDescripcion = drGlasser["NOKOPR"].ToString().Trim();
                    }
                    else
                    {
                        
                    }

                    drGlasser.Close();
                    ConnGlasser.Close();


                    tmp.Add(lista);
                }
            }
        }
        else
        {
            
        }
        

        return tmp;
    }

    [WebMethod]
    public List<DetalleInvItem> DetalleinvItems(string IdInventario, string Token)
    {
        List<DetalleInvItem> detalleInvItems = new List<DetalleInvItem>();
        DetalleInvItem Item = new DetalleInvItem();
        
        if (Token == "BaPRiThWJLYvy3KOn04K43we4XtyMdJIiYzqFwvLH0")
        {
            
            string consulta = "Select COUNT(A.KOPR) " +
                "FROM PLABAL.dbo.LO_Tomainventory A, PLABAL.dbo.e_Usuario B, PLABAL.dbo.LO_Asignruta C, PLABAL.dbo.LO_rutasinvent D WITH(NOLOCK) " +
                " WHERE A.Estado=0 AND A.Cod_oper=B.ID AND A.Id_Asign=C.Id AND C.ID_Inventory=@IDInv AND C.ID_Ruta=D.Id";



            
            
            
                try
                {
                    ConnPlabal.Open();
                    cmdPlabal = new SqlCommand(consulta, ConnPlabal);
                    cmdPlabal.Parameters.AddWithValue("@IDInv", IdInventario);
                    drPlabal = cmdPlabal.ExecuteReader();
                    drPlabal.Read();

                    if (drPlabal.HasRows)
                    {
                        Item._Items = Convert.ToInt32(drPlabal[0].ToString());
                    
                    }
                    else
                    {
                        
                    }

                    drPlabal.Close();
                    ConnPlabal.Close();
                }
                catch (Exception ex)
                {
                Item._Items = 0;
            }
            

           
        }
        else
        {
            Item._Items = 0;
        }
        detalleInvItems.Add(Item);

        return detalleInvItems;
    }

    [WebMethod]
    public List<RutasUserInv> GetListRutasUserInv(string Usuario, string ID_Inventory)
    {
        List<RutasUserInv> rutas = new List<RutasUserInv>();
        string consulta;


        if (Usuario == "ALL")
        {
            //obtiene todas las rutas de la sucursal
            consulta = @"select asig.Id as 'IdAsignRuta',oper.Id as 'IdAsignOper',"
            + "ru.Nombre as 'RutaName',ru.CodBodega, usu.Nombre as 'UserName', ru.Descripcion " +
            "FROM LO_rutasinvent ru, LO_Asignruta asig, LO_invasignoperario oper, e_Usuario usu "
            + "where ru.Id=asig.ID_Ruta and asig.ID_Inventory=@ID_Inventory  and oper.ID_Asignruta=asig.Id and usu.ID=oper.Id_e_Usuario and oper.Estado=0 and asig.Status=0";
        }
        else
        {
            consulta = @"select asig.Id as 'IdAsignRuta',oper.Id as 'IdAsignOper',"
            + "ru.Nombre as 'RutaName',ru.CodBodega, usu.Nombre as 'UserName', ru.Descripcion " +
            "FROM LO_rutasinvent ru, LO_Asignruta asig, LO_invasignoperario oper, e_Usuario usu "
            + "where oper.Estado=0 ru.Id=asig.ID_Ruta and asig.ID_Inventory=@ID_Inventory  and oper.ID_Asignruta=asig.Id and usu.ID=oper.Id_e_Usuario and usu.ID='" + Usuario + "'";
        }


        DataTable TablaDetalle = new DataTable();
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[StrPlabalConn].ConnectionString))
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);
                adaptador.SelectCommand.Parameters.AddWithValue("@ID_Inventory", ID_Inventory);


                adaptador.Fill(TablaDetalle);
            }
            catch (Exception ex)
            {
                string exx = ex.Message.ToString();
            }
        }

        if (TablaDetalle.Rows.Count > 0)
        {
            foreach (DataRow dr in TablaDetalle.Rows)
            {
                RutasUserInv ruta = new RutasUserInv
                {
                    _Id_Asignruta = dr["IdAsignRuta"].ToString(),
                    _Id_Asign_user = dr["IdAsignOper"].ToString(),
                    _UserName = dr["UserName"].ToString(),
                    _Bodega = dr["CodBodega"].ToString(),
                    _RutaName = dr["RutaName"].ToString(),
                    _DescripRuta = dr["Descripcion"].ToString(),
                };
                rutas.Add(ruta);


            }
        }


        return rutas;
    }


}
