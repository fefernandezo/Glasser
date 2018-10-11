using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de LogClass
/// </summary>
/// 

namespace Logistica
{
    public class TokenClass
    {
       public string TokenId;

        public TokenClass()
        {
            string ID = "TduwW8sLP8tjesr0YfucYmd1yuLJHbqRsjcx1SksGlxXLrZa2WCUMgYH77iJ3Ci";
            this.TokenId = ID;
        }

        
    }

    public class LogClass
    {
        string StrGlasserConn = "GLASSERConnection";
        string StrPlabalConn = "PLABALConnection";
        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
        SqlConnection ConnRANDOM = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
        SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
        SqlConnection ConnRANDOM2 = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
        SqlCommand cmdPlabal;
        SqlCommand cmdGlasser;
        static SqlDataReader drPlabal;
        static SqlDataReader drGlasser;
        TokenClass _token;
        public LogClass()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }



        public Inventario GetVariables(string ID)
        {
            Inventario inventario = new Inventario();

            ConnPlabal.Open();
            string Select = @"Select * FROM LO_inventory WHERE Id=@ID";
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@ID", ID);
            
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();
            
            if (drPlabal.HasRows)
            {
                inventario.Name = drPlabal["Nombre"].ToString();
                inventario.Description= drPlabal["Descripcion"].ToString();
                inventario.DateCreate = Convert.ToDateTime(drPlabal["CreacionDate"].ToString());
                if (!string.IsNullOrEmpty(drPlabal["InicioDate"].ToString()))
                {
                    inventario.StartTime = Convert.ToDateTime(drPlabal["InicioDate"].ToString());
                }
                else
                {
                    
                }
                if (!string.IsNullOrEmpty(drPlabal["TerminoDate"].ToString()))
                {
                    inventario.EndTime = Convert.ToDateTime(drPlabal["TerminoDate"].ToString());
                }
                else
                {

                }

                inventario.KOSU = drPlabal["KOSU"].ToString();
                inventario.Status = drPlabal["Status"].ToString();

            }
            else
            {
          
            }
          
            drPlabal.Close();
            ConnPlabal.Close();
            return inventario;
        }

        public List<Bodega> GetListBodega(string KOSU)
        {
            List<Bodega> bodegas = new List<Bodega>();
            string consulta;
            if (KOSU=="ALL")
            {
                consulta = @"SELECT * FROM TABBO";
            }
            else
            {
                consulta = @"SELECT * FROM TABBO WHERE KOSU='" + KOSU + "'";
            }

             
            DataTable TablaDetalle = new DataTable();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[StrGlasserConn].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);
                    

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
                    Bodega bodega = new Bodega {
                        _Name = drRAN["NOKOBO"].ToString().Trim(),
                        _Address= drRAN["DIBO"].ToString().Trim(),
                        _EMPRESA=drRAN["EMPRESA"].ToString().Trim(),
                        _KOBO=drRAN["KOBO"].ToString().Trim(),
                        _KOSU=drRAN["KOSU"].ToString().Trim(),
                        COMBINADO = drRAN["KOBO"].ToString().Trim() + " - " + drRAN["NOKOBO"].ToString().Trim(),
                    };

                    bodegas.Add(bodega);

                }
            }


            return bodegas;
        }

        public List<Sucursal> GetListSucursal()
        {
            List<Sucursal> sucursals = new List<Sucursal>();



            string consulta = @"SELECT * FROM TABSU";
            DataTable TablaDetalle = new DataTable();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[StrGlasserConn].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);


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
                    Sucursal sucursal = new Sucursal {
                        _Name=drRAN["NOKOSU"].ToString(),
                        _Address=drRAN["DISU"].ToString(),
                        _CISU=drRAN["CISU"].ToString(),
                        _CMSU= drRAN["CMSU"].ToString(),
                        _EMPRESA= drRAN["EMPRESA"].ToString(),
                        _KOFUSU= drRAN["KOFUSU"].ToString(),
                        _KOSU= drRAN["KOSU"].ToString(),
                        _Phone = drRAN["FOSU"].ToString(),
                        COMBINADO = drRAN["KOSU"].ToString() + " - " + drRAN["NOKOSU"].ToString(),
                    };
                    sucursals.Add(sucursal);
                }
            }


            return sucursals;
        }

        public bool InsertRuta (Ruta ruta)
        {
            bool validate;
            #region InsertString
            string Insert = "Insert INTO LO_rutasinvent VALUES (@Nombre,@CodBodega,@Descripcion,@CodSUCU)";
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@Nombre", ruta._Nombre);
                cmdPlabal.Parameters.AddWithValue("@CodBodega", ruta._CodBodega);
                cmdPlabal.Parameters.AddWithValue("@Descripcion", ruta._Descripcion);
                cmdPlabal.Parameters.AddWithValue("@CodSUCU", ruta._CodSUCU);


                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();
                validate = true;
            }
            catch (Exception EX)
            {
                validate = false;
            }





            return validate;
        }

        public bool InsertAsignRuta(string ID_Iventory, string ID_Ruta)
        {
            bool validate;
            #region InsertString
            string Insert = "Insert INTO LO_Asignruta VALUES (@ID_Inventory,@ID_Ruta,GETDATE(),0)";
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@ID_Inventory", ID_Iventory);
                cmdPlabal.Parameters.AddWithValue("@ID_Ruta", ID_Ruta);
                
                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();
                validate = true;
            }
            catch (Exception EX)
            {
                validate = false;
            }





            return validate;
        }

        public bool InsertAsignoperario(string ID_Asignruta, string ID_Usuario)
        {
            bool validate;
            #region InsertString
            string Insert = "Insert INTO LO_invasignoperario VALUES (@ID_Asignruta,@ID_e_Usuario, GETDATE(),0)";
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@ID_Asignruta", ID_Asignruta);
                cmdPlabal.Parameters.AddWithValue("@ID_e_Usuario", ID_Usuario);

                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();
                validate = true;
            }
            catch (Exception EX)
            {
                validate = false;
            }





            return validate;
        }

        public List<Ruta> GetListRuta(string CodBodega, string ID_Imventory ,string KOSU)
        {
            List<Ruta> rutas = new List<Ruta>();
            string consulta;

            
            if (CodBodega == "ALL" && KOSU=="ALL")
            {
                //obtiene todas las rutas de la sucursal
                consulta = @"select * from LO_rutasinvent ru where not ru.Id in (select asi.ID_Ruta from LO_Asignruta asi where asi.ID_Inventory=@ID_Inventory and asi.ID_Ruta=ru.Id and asi.Status=0)";
            }
            else if (CodBodega=="ALL" && !string.IsNullOrEmpty(KOSU))
            {
                consulta = @"select * from LO_rutasinvent ru where not ru.Id in (select asi.ID_Ruta from LO_Asignruta asi where asi.ID_Inventory=@ID_Inventory and asi.ID_Ruta=ru.Id and asi.Status=0) and ru.CodSUCU='" + KOSU + "'";
            }
            else if(string.IsNullOrEmpty(KOSU))
            {
                consulta = @"select * from LO_rutasinvent ru where not ru.Id in (select asi.ID_Ruta from LO_Asignruta asi where asi.ID_Inventory=@ID_Inventory and asi.ID_Ruta=ru.Id and asi.Status=0) and ru.CodBodega='" + CodBodega + "'";
            }
            else
            {
                consulta = @"select * from LO_rutasinvent ru where not ru.Id in (select asi.ID_Ruta from LO_Asignruta asi where asi.ID_Inventory=@ID_Inventory and asi.ID_Ruta=ru.Id and asi.Status=0) and ru.CodSUCU='" + KOSU + "' and ru.CodBodega='" + CodBodega + "'";
            }


            DataTable TablaDetalle = new DataTable();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[StrPlabalConn].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_Inventory", ID_Imventory);


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
                    Ruta ruta = new Ruta
                    {
                        _Id=dr["Id"].ToString(),
                        _CodBodega=CodBodega,
                        _CodSUCU= dr["CodSUCU"].ToString() ,
                        _Descripcion=dr["Descripcion"].ToString(),
                        _Nombre=dr["Nombre"].ToString(),
                    };
                    rutas.Add(ruta);
                    

                }
            }


            return rutas;
        }

        public List<Ruta> GetListRutasxAsign(string IdUser,string ID_Imventory)
        {
            List<Ruta> rutas = new List<Ruta>();
            string consulta;


            if (IdUser == "ALL")
            {
                //obtiene todas las rutas de la sucursal
                consulta = @"select ru.Id,ru.CodBodega,ru.CodSUCU,ru.Descripcion,ru.Nombre,asig.Id as 'IdAsign' "
                + "FROM LO_rutasinvent ru, LO_Asignruta asig where asig.Status=0 AND ru.Id=asig.ID_Ruta and asig.ID_Inventory=@ID_Inventory";
            }
            else
            {
                consulta = @"select ru.Id,ru.CodBodega,ru.CodSUCU,ru.Descripcion,ru.Nombre,A1.Id as 'IdAsign' "
                +"FROM LO_rutasinvent ru, LO_Asignruta A1 "
                +"where not ru.Id in (select asig.ID_Ruta from LO_Asignruta asig, LO_invasignoperario oper where asig.ID_Inventory=@ID_Inventory and oper.Estado=0  and oper.ID_Asignruta=asig.Id and oper.Id_e_Usuario='" + IdUser + "') "
                +"and ru.Id=A1.ID_Ruta and A1.ID_Inventory=@ID_Inventory and A1.Status=0";
            }


            DataTable TablaDetalle = new DataTable();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[StrPlabalConn].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_Inventory", ID_Imventory);


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
                    Ruta ruta = new Ruta
                    {
                        _Id = dr["Id"].ToString(),
                        _CodBodega = dr["CodBodega"].ToString(),
                        _CodSUCU = dr["CodSUCU"].ToString(),
                        _Descripcion = dr["Descripcion"].ToString(),
                        _Nombre = dr["Nombre"].ToString(),
                        IdAsign= dr["IdAsign"].ToString(),
                    };
                    rutas.Add(ruta);


                }
            }


            return rutas;
        }

        

        public List<UsuariosInv> GetListUsuariosInv()
        {
            List<UsuariosInv> listaUsers = new List<UsuariosInv>();
            string consulta;


            
                consulta = @"SELECT * FROM e_Usuario WHERE Estado=3";
           


            DataTable TablaDetalle = new DataTable();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings[StrPlabalConn].ConnectionString))
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conn);
                    


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
                    UsuariosInv usuario = new UsuariosInv
                    {
                        _Id = dr["ID"].ToString(),
                        _Name= dr["Nombre"].ToString(),
                        _UserName = dr["Usuario"].ToString(),
                        
                    };
                    listaUsers.Add(usuario);


                }
            }


            return listaUsers;
        }

        public int InsertInventario (Inventario inventario)
        {
            int Valor;

            #region InsertString
            string Insert;
            if (inventario.EndTime.Year==1)
            {
                Insert = "Insert INTO LO_inventory VALUES (@Nombre,@Descripcion,@CreacionDate,@InicioDate,NULL,@KOSU,@Status)";
            }
            else
            {
                Insert = "Insert INTO LO_inventory VALUES (@Nombre,@Descripcion,@CreacionDate,@InicioDate,@TerminoDate,@KOSU,@Status)";
            }
             
            #endregion

            try
            {
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@Nombre", inventario.Name);
                cmdPlabal.Parameters.AddWithValue("@Descripcion", inventario.Description);
                cmdPlabal.Parameters.AddWithValue("@CreacionDate", inventario.DateCreate);
                cmdPlabal.Parameters.AddWithValue("@InicioDate", inventario.StartTime);
                if (inventario.EndTime.Year == 1)
                {
                   
                }
                else
                {
                    cmdPlabal.Parameters.AddWithValue("@TerminoDate", inventario.EndTime);
                }
                
                cmdPlabal.Parameters.AddWithValue("@KOSU", inventario.KOSU);
                cmdPlabal.Parameters.AddWithValue("@Status", inventario.Status);


                cmdPlabal.ExecuteNonQuery();
                ConnPlabal.Close();


                string Select = "SELECT Id FROM LO_inventory WHERE Nombre=@Nombre AND Status=@Status AND CreacionDate=@CreacionDate AND KOSU=@KOSU";
                ConnPlabal.Open();
                cmdPlabal = new SqlCommand(Select, ConnPlabal);
                cmdPlabal.Parameters.AddWithValue("@Nombre", inventario.Name);
                cmdPlabal.Parameters.AddWithValue("@CreacionDate", inventario.DateCreate);
                cmdPlabal.Parameters.AddWithValue("@KOSU", inventario.KOSU);
                cmdPlabal.Parameters.AddWithValue("@Status", inventario.Status);
                drPlabal = cmdPlabal.ExecuteReader();
                drPlabal.Read();

                if (drPlabal.HasRows)
                {
                    Valor = Convert.ToInt32(drPlabal["Id"].ToString());
                }
                else
                {
                    Valor = 0;
                }

                drPlabal.Close();
                ConnPlabal.Close();
            }
            catch (Exception EX)
            {
                Valor=0;
            }


            return Valor;
        }

        public void NullorCloseInventario (string ID, string token,string Action)
        {
            _token = new TokenClass();
            string To = _token.TokenId;

            if(To==token)
            {

                #region InsertString
                string Insert = "";
                if (Action == "Cerrar")
                {
                    Insert = "UPDATE LO_inventory SET Status=1 WHERE Id=@ID ";
                }
                else if (Action=="Anular")
                {
                    Insert = "UPDATE LO_inventory SET Status=3 WHERE Id=@ID ";
                }
                else if (Action == "Activar")
                {
                    Insert = "UPDATE LO_inventory SET Status=0 WHERE Id=@ID ";
                }

                #endregion

                try
                {
                    ConnPlabal.Open();
                    cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                    #region Parameters
                    cmdPlabal.Parameters.AddWithValue("@ID", ID);
                    #endregion

                    cmdPlabal.ExecuteNonQuery();
                    ConnPlabal.Close();
                }
                catch (Exception EX)
                {
                    
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                    ConnGlasser.Close();

                }
            }

            
        }

        public void AnularItemInv(string ID, string token)
        {
            _token = new TokenClass();
            string To = _token.TokenId;

            if (To == token)
            {
                #region InsertString
                string Insert = "UPDATE LO_Tomainventory SET Estado=3 WHERE Id=@ID ";
                #endregion

                try
                {
                    ConnPlabal.Open();
                    cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                    #region Parameters
                    cmdPlabal.Parameters.AddWithValue("@ID", ID);
                    #endregion

                    cmdPlabal.ExecuteNonQuery();
                    ConnPlabal.Close();
                }
                catch (Exception EX)
                {
                    throw EX;
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                    ConnGlasser.Close();

                }
            }


        }

        public void AnularRutaAsign(string ID, string token)
        {
            _token = new TokenClass();
            string To = _token.TokenId;

            if( To== token)
            {
                #region InsertString
                string Insert = "UPDATE LO_Asignruta SET Status=1 WHERE Id=@ID ";
                #endregion

                try
                {
                    ConnPlabal.Open();
                    cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                    #region Parameters
                    cmdPlabal.Parameters.AddWithValue("@ID", ID);
                    #endregion

                    cmdPlabal.ExecuteNonQuery();
                    ConnPlabal.Close();
                }
                catch (Exception EX)
                {
                    throw EX;
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                    ConnGlasser.Close();

                }
            }
            else
            {

            }
           
        }

        public bool AnularAsignUser(string ID, string token)
        {
            _token = new TokenClass();
            string To = _token.TokenId;
            bool valid = false;

            if (To == token)
            {
                #region InsertString
                string Insert = "UPDATE LO_invasignoperario SET Estado=1 WHERE Id=@ID ";
                #endregion

                try
                {
                    ConnPlabal.Open();
                    cmdPlabal = new SqlCommand(Insert, ConnPlabal);
                    #region Parameters
                    cmdPlabal.Parameters.AddWithValue("@ID", ID);
                    #endregion

                    cmdPlabal.ExecuteNonQuery();
                    ConnPlabal.Close();
                    valid = true;
                }
                catch (Exception EX)
                {
                    
                    ErrorCatching errorCatching = new ErrorCatching();
                    errorCatching.ErrorCatch(EX.Message + " TRAZO:" + EX.StackTrace + " STRING:" + Insert, HttpContext.Current.Request.Url.ToString());
                    ConnGlasser.Close();
                    valid = false;

                }
            }
            else
            {
                valid = false;
            }

            return valid;
        }

        public List<DetalleInv> GetDetailInventory(string IdInventario, string Token)
        {
            List<DetalleInv> tmp = new List<DetalleInv>();
            TokenClass _token = new TokenClass();
            if (Token == _token.TokenId)
            {

                string consulta = "Select A.KOPR,A.Cant,A.Unidad,A.Timestamp,B.Nombre AS 'Operario',D.Nombre AS 'Bodega', D.CodBodega, D.CodSUCU, A.Id " +
                    "FROM PLABAL.dbo.LO_Tomainventory A, PLABAL.dbo.e_Usuario B, PLABAL.dbo.LO_Asignruta C, PLABAL.dbo.LO_rutasinvent D WITH ( NOLOCK )  " +
                    " WHERE A.Cod_oper=B.ID AND A.Id_Asign=C.Id AND C.ID_Inventory=@ID AND C.ID_Ruta=D.Id AND A.Estado=0";



                DataTable TablaDetalle = new DataTable();
                using (ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnPlabal);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ID", IdInventario);

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
                            _KOPR = KOPR,
                            _Cant = Convert.ToDouble(drRAN["Cant"].ToString()),
                            _Unidad = drRAN["Unidad"].ToString(),
                            _Fecha = Convert.ToDateTime(drRAN["Timestamp"].ToString()).ToShortDateString(),
                            _Bodega = drRAN["Bodega"].ToString().Trim(),
                            _CodBodega = KOBO,
                            _Operario = drRAN["Operario"].ToString().Trim(),
                            _Id = drRAN["Id"].ToString(),

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
    }

   


   
}
