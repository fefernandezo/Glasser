using GlobalInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Comercial
/// </summary>
/// 
namespace Comercial
{
   public class FormularioModelos
    {
        public class GetFamilias
        {
            Coneccion Conn;
            static SqlDataReader dr;

            public Hashtable Datos { get; set; }
            
            public GetFamilias()
            {
                bool Estado = true;
                Conn = new Coneccion();
                string Consulta = "select FAM.ID as 'ID',FAM.FAMNAME as 'FAMNAME' " +
                    "from COT_PTABMODEL MODEL, COT_PASIGFAMCAT ASIG, COT_PTABFAM FAM " +
                    "WHERE MODEL.ID_PASIGFAMCAT = ASIG.ID AND FAM.ID = ASIG.IDFAM " +
                    "AND MODEL.ESTADO =@estado AND ASIG.ESTADO =@estado AND FAM.ESTADO =@estado";
                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new SqlCommand(Consulta,Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@estado", Estado);

                    dr = Conn.Cmd.ExecuteReader();
                    Datos = new Hashtable();
                    while (dr.Read())
                    {
                        Datos.Add(dr[0].ToString(),dr[1].ToString());
                    }
                    dr.Close();
                    Conn.ConnPlabal.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

    public class CanalDis
    {
        public List<Canal> canales;

        Coneccion Conn;

        public CanalDis()
        {
            canales = new List<Canal>();
            Conn = new Coneccion();

            bool ESTADO = true;

            string consulta = "SELECT * FROM COT_PTABCANALES WHERE ESTADO=@ESTADO ORDER BY CHANNAME ASC";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drR in TablaDetalle.Rows)
                {
                    Canal ITEM = new Canal {
                        ID = drR["ID"].ToString(),
                        CHANNAME = drR["CHANNAME"].ToString(),
                        CHANDESCR= drR["CHANDESCR"].ToString(),
                        ESTADO = Convert.ToBoolean(drR["ESTADO"]),
                    };
                    canales.Add(ITEM);
                }
            }

        }

        

        public List<Canal> GetCanalesAsign(string ID_MODEL, bool ESTADO)
        {
            List<Canal> result = new List<Canal>();
            Conn = new Coneccion();

            

            string consulta = "SELECT b.ID AS 'Id_Asign', can.* FROM COT_PTABCANALES can, COT_PASIGMODCAN b WHERE can.ESTADO=@ESTADO and " +
                "b.ESTADO=@ESTADO AND b.IDMODEL=@ID_MODEL AND can.ID=b.IDCANAL ORDER BY CHANNAME ASC";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);


                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drR in TablaDetalle.Rows)
                {
                    Canal ITEM = new Canal
                    {
                        ID = drR["ID"].ToString(),
                        CHANNAME = drR["CHANNAME"].ToString(),
                        CHANDESCR = drR["CHANDESCR"].ToString(),
                        ESTADO = Convert.ToBoolean(drR["ESTADO"]),
                        ID_Asign = drR["Id_Asign"].ToString(),
                    };
                    result.Add(ITEM);
                }
            }

            return result;
        }

        public List<Canal> GetCanalesNOAsign(string ID_MODEL, bool ESTADO)
        {
            List<Canal> result = new List<Canal>();
            Conn = new Coneccion();



            string consulta = "SELECT can.* FROM COT_PTABCANALES can WHERE can.ESTADO=@ESTADO  AND can.ID NOT IN (SELECT b.IDCANAL FROM COT_PASIGMODCAN b WHERE b.IDMODEL=@ID_MODEL and b.ESTADO=@ESTADO) ORDER BY CHANNAME ASC";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);


                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drR in TablaDetalle.Rows)
                {
                    Canal ITEM = new Canal
                    {
                        ID = drR["ID"].ToString(),
                        CHANNAME = drR["CHANNAME"].ToString(),
                        CHANDESCR = drR["CHANDESCR"].ToString(),
                        ESTADO = Convert.ToBoolean(drR["ESTADO"]),
                    };
                    result.Add(ITEM);
                }
            }

            return result;
        }

        public bool AsigModeltoChann(string ID_CANAL, string ID_MODEL, bool ESTADO)
        {
            bool IsInserted = false;
            Conn = new Coneccion();

            #region InsertString
            string Query = "Insert INTO COT_PASIGMODCAN VALUES (@IDMODEL,@IDCANAL,GETDATE(),@ESTADO)";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@IDMODEL", ID_MODEL);
                Conn.Cmd.Parameters.AddWithValue("@IDCANAL", ID_CANAL);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsInserted = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsInserted = false;
            }



            return IsInserted;

        }

        public bool DeleteAsign(string ID_Asign)
        {
            bool ESTADO = false;
            bool IsDelete;
            Conn = new Coneccion();

            #region InsertString
            string Query = "UPDATE COT_PASIGMODCAN SET ESTADO=@ESTADO WHERE ID=@ID_ASIGN";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@ID_ASIGN", ID_Asign);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsDelete = true;

            }
            catch (Exception ex)
            {
                throw ex;
                
            }

            return IsDelete;

        }

        public class Canal
        {
            public string ID { get; set; }
            public string ID_Asign { get; set; }
            public string CHANNAME { get; set; }
            public string CHANDESCR { get; set; }
            public bool ESTADO { get; set; }
        }

    }

    public class Modelos
    {
        public string IdBeforeInsert { get; set; }

        public List<_Modelo> Lista { get; set; }

        public _Modelo _modelo { get; set; }

        Coneccion Conn;
        static SqlDataReader dr;

        public Modelos()
        {

        }

        public Modelos(string ID, bool ESTADO)
        {
            GetModelo(ID, ESTADO);

        }

        public Modelos(string CAMPO, object VALOR, bool ESTADO)
        {
            Conn = new Coneccion();
            Lista = new List<_Modelo>();
            string Consulta = "SELECT * FROM COT_PTABMODEL WHERE " + CAMPO + "=@valor and ESTADO=@estado";

            DataTable Dtable = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adap = new SqlDataAdapter(Consulta, Conn.ConnPlabal);
                    
                    adap.SelectCommand.Parameters.AddWithValue("@valor", VALOR);
                    adap.SelectCommand.Parameters.AddWithValue("@estado", ESTADO);

                    adap.Fill(Dtable);
                }
                catch (Exception)
                {

                    throw;
                }
                if (Dtable.Rows.Count>0)
                {
                    foreach (DataRow row in Dtable.Rows)
                    {
                        _Modelo item = new _Modelo
                        {
                            ID = row["ID"].ToString(),
                            NOMBRE = row["NOMBRE"].ToString(),
                            DESCRIPCION = row["DESCRIPCION"].ToString(),
                            IMAGE = row["IMG_PATH"].ToString(),
                            F_Update = Convert.ToDateTime(row["F_UPDATE"].ToString()),
                            F_Created = Convert.ToDateTime(row["F_CREATED"].ToString()),
                            HASPIECES = Convert.ToBoolean(row["HASPIECES"]),
                            ESTADO = Convert.ToBoolean(row["ESTADO"].ToString()),
                            TokenId = row["TokenId"].ToString(),


                        };
                        GetFamilyOfModel getFamily = new GetFamilyOfModel(row["ID_PASIGFAMCAT"].ToString());
                        if (getFamily.HasVarLine)
                        {
                            item.Familia = getFamily.Familia;
                            item.IdFamilia = getFamily.IdFamily;
                            item.ID_PASIGFAMCAT = Convert.ToInt32(getFamily.IdAsign);
                            item.Categoria = getFamily.Categoria;
                            item.IdCategoria = getFamily.IdCategoria;
                        }
                        Lista.Add(item);
                    }
                }
            }


        }

        public bool IsModel(string ID,bool ESTADO, string TOKEN)
        {
            bool _IsModel = false;
            Encriptacion encriptacion = new Encriptacion(TOKEN);
            int TokenId = Convert.ToInt32(encriptacion.TokenDesencriptado);

            GetModelo(ID, ESTADO, TokenId);

            if (_modelo.ESTADO)
            {
                _IsModel = true;
            }

            return _IsModel;
        }

        private void GetModelo(string ID, bool ESTADO)
        {

            string Select = "SELECT TOP 1 * FROM COT_PTABMODEL WHERE ID=@ID AND ESTADO=@ESTADO";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                _modelo = new _Modelo
                {
                    ID = dr["ID"].ToString(),
                    NOMBRE = dr["NOMBRE"].ToString(),
                    DESCRIPCION = dr["DESCRIPCION"].ToString(),
                    IMAGE = dr["IMG_PATH"].ToString(),
                    F_Update = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                    F_Created = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                    HASPIECES = Convert.ToBoolean(dr["HASPIECES"]),
                    
                    ESTADO = Convert.ToBoolean(dr["ESTADO"].ToString()),
                    TokenId = dr["TokenId"].ToString(),


                };
                GetFamilyOfModel getFamily = new GetFamilyOfModel(dr["ID_PASIGFAMCAT"].ToString());
                if (getFamily.HasVarLine)
                {
                    _modelo.Familia = getFamily.Familia;
                    _modelo.IdFamilia = getFamily.IdFamily;
                    _modelo.ID_PASIGFAMCAT = Convert.ToInt32(getFamily.IdAsign);
                    _modelo.Categoria = getFamily.Categoria;
                    _modelo.IdCategoria = getFamily.IdCategoria;
                }

            }
            else
            {

            }

            dr.Close();
            Conn.ConnPlabal.Close();

        }

        private void GetModelo(string ID, bool ESTADO, int TokenId)
        {

            string Select = "SELECT TOP 1 * FROM COT_PTABMODEL WHERE ID=@ID AND ESTADO=@ESTADO AND TokenId=@TokenId";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            Conn.CmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                _modelo = new _Modelo
                {
                    ID = dr["ID"].ToString(),
                    NOMBRE = dr["NOMBRE"].ToString(),
                    DESCRIPCION = dr["DESCRIPCION"].ToString(),
                    IMAGE = dr["IMG_PATH"].ToString(),
                    F_Update = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                    F_Created = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                    HASPIECES = Convert.ToBoolean(dr["HASPIECES"]),
                    ESTADO = Convert.ToBoolean(dr["ESTADO"].ToString()),
                    TokenId = dr["TokenId"].ToString(),


                };
                GetFamilyOfModel getFamily = new GetFamilyOfModel(dr["ID_PASIGFAMCAT"].ToString());
                if (getFamily.HasVarLine)
                {
                    _modelo.Familia = getFamily.Familia;
                    _modelo.IdFamilia = getFamily.IdFamily;
                    _modelo.ID_PASIGFAMCAT = Convert.ToInt32(getFamily.IdAsign);
                    _modelo.Categoria = getFamily.Categoria;
                    _modelo.IdCategoria = getFamily.IdCategoria;
                }

            }
            else
            {

            }

            dr.Close();
            Conn.ConnPlabal.Close();

        }

        public bool InsertModel(_Modelo modelo)
        {

            bool IsInserted = false;
            Conn = new Coneccion();

            #region InsertString
            string Query = "Insert INTO COT_PTABMODEL (NOMBRE,DESCRIPCION,IMG_PATH,HASPIECES," +
                "F_UPDATE,F_CREATED,ESTADO,TokenId) VALUES (@NOMBRE,@DESCRIPCION,@IMAGE,@HASPIECES," +
                "GETDATE(),GETDATE(),@ESTADO,@TokenId)";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@NOMBRE", modelo.NOMBRE);
                Conn.Cmd.Parameters.AddWithValue("@DESCRIPCION", modelo.DESCRIPCION);
                Conn.Cmd.Parameters.AddWithValue("@IMAGE", modelo.IMAGE );
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", modelo.ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@TokenId", modelo.TokenId);
                Conn.Cmd.Parameters.AddWithValue("@HASPIECES", modelo.HASPIECES);
                


                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsInserted = true;
                GetIdRowBeforeInsert(modelo.NOMBRE, modelo.TokenId);
            }
            catch(Exception EX)
            {
                throw EX;
                IsInserted = false;
            }



            return IsInserted;

        }

        private void GetIdRowBeforeInsert(string Nombre, string TokenId)
        {

            string Select = "SELECT TOP 1 ID FROM COT_PTABMODEL WHERE NOMBRE=@NOMBRE AND TokenId=@TokenId";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@NOMBRE", Nombre);
            Conn.CmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                IdBeforeInsert = dr[0].ToString();
            }

            dr.Close();
            Conn.ConnPlabal.Close();

        }

        public bool UpdateModel(string RowID, string Campo, string Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_PTABMODEL SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

        public bool UpdateModel(string RowID, string Campo, double Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_PTABMODEL SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

        public bool UpdateModel(string RowID, string Campo, bool Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_PTABMODEL SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

        public class GetFamilyOfModel
        {
            public string IdFamily { get; set; }
            public string Familia { get; set; }
            public string IdCategoria { get; set; }
            public string Categoria { get; set; }
            public bool HasVarLine { get; set; }
            public string IdAsign { get; set; }
            

            Coneccion Conn;

            public GetFamilyOfModel(string ID_PASIGFAMCAT)
            {
                bool ESTADO = true;
                string Select = "SELECT B.ID AS 'IDFAM', C.ID AS 'IDCAT', B.FAMNAME,B.FAMDESCR,C.CATNAME,C.CATDESCR  " +
                    "FROM COT_PASIGFAMCAT A, COT_PTABFAM B, COT_PTABCAT C WHERE A.ID=@ID and A.IDFAM=B.ID and A.IDCAT=C.ID and A.ESTADO=@ESTADO";
                Conn = new Coneccion();

                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID_PASIGFAMCAT);
                Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    HasVarLine = true;
                    IdFamily = dr["IDFAM"].ToString();
                    IdCategoria= dr["IDCAT"].ToString();
                    Familia= dr["FAMNAME"].ToString();
                    Categoria = dr["CATNAME"].ToString();
                    IdAsign = ID_PASIGFAMCAT;
                }
                else
                {
                    HasVarLine = false;
                }

                dr.Close();
                Conn.ConnPlabal.Close();


            }
        }

        public class GetTop20Models
        {
            public List<_Modelo> Lista { get; set; }
            Coneccion Conn;
            

            public GetTop20Models()
            {
                bool ESTADO = true;
                Lista = new List<_Modelo>();
                string Select = "SELECT TOP 20 * FROM COT_PTABMODEL WHERE ESTADO=@ESTADO ORDER BY ID DESC";
                Conn = new Coneccion();

                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                        

                        adaptador.Fill(TablaDetalle);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {

                    foreach (DataRow drR in TablaDetalle.Rows)
                    {
                        _Modelo modelo = new _Modelo
                        {
                            ID = drR["ID"].ToString(),
                            NOMBRE = drR["NOMBRE"].ToString(),
                            DESCRIPCION = drR["DESCRIPCION"].ToString(),
                            IMAGE = drR["IMG_PATH"].ToString(),
                            F_Update = Convert.ToDateTime(drR["F_UPDATE"].ToString()),
                            F_Created = Convert.ToDateTime(drR["F_CREATED"].ToString()),
                            HASPIECES = Convert.ToBoolean(drR["HASPIECES"]),
                            ESTADO = Convert.ToBoolean(drR["ESTADO"].ToString()),
                            TokenId = drR["TokenId"].ToString(),
                        };
                        GetFamilyOfModel getFamily = new GetFamilyOfModel(drR["ID_PASIGFAMCAT"].ToString());
                        if (getFamily.HasVarLine)
                        {
                            modelo.Familia = getFamily.Familia;
                            modelo.IdFamilia = getFamily.IdFamily;
                            modelo.ID_PASIGFAMCAT = Convert.ToInt32(drR["ID_PASIGFAMCAT"].ToString());
                            modelo.Categoria = getFamily.Categoria;
                            modelo.IdCategoria = getFamily.IdCategoria;
                        }
                        Lista.Add(modelo);

                    }
                }

            }

        }

        public class GetFilteredModels
        {
            public List<_Modelo> Lista { get; set; }
            Coneccion Conn;

            public GetFilteredModels(string SearchString, bool ESTADO)
            {
                string variable = "%" + SearchString + "%";
                Lista = new List<_Modelo>();
                string Select = "SELECT * FROM COT_PTABMODEL WHERE (NOMBRE LIKE @VARIABLE or DESCRIPCION LIKE @VARIABLE) AND ESTADO=@ESTADO ORDER BY F_UPDATE DESC";
                Conn = new Coneccion();

                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                        adaptador.SelectCommand.Parameters.AddWithValue("@VARIABLE", variable);

                        adaptador.Fill(TablaDetalle);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {

                    foreach (DataRow drR in TablaDetalle.Rows)
                    {
                        _Modelo modelo = new _Modelo
                        {
                            ID = drR["ID"].ToString(),
                            NOMBRE = drR["NOMBRE"].ToString(),
                            DESCRIPCION = drR["DESCRIPCION"].ToString(),
                            IMAGE = drR["IMG_PATH"].ToString(),
                            F_Update = Convert.ToDateTime(drR["F_UPDATE"].ToString()),
                            F_Created = Convert.ToDateTime(drR["F_CREATED"].ToString()),
                            HASPIECES = Convert.ToBoolean(drR["HASPIECES"]),
                            ESTADO = Convert.ToBoolean(drR["ESTADO"].ToString()),
                            TokenId = drR["TokenId"].ToString(),
                        };
                        GetFamilyOfModel getFamily = new GetFamilyOfModel(drR["ID_PASIGFAMCAT"].ToString());
                        if (getFamily.HasVarLine)
                        {
                            modelo.Familia = getFamily.Familia;
                            modelo.IdFamilia = getFamily.IdFamily;
                            modelo.ID_PASIGFAMCAT = Convert.ToInt32(drR["ID_PASIGFAMCAT"].ToString());
                            modelo.Categoria = getFamily.Categoria;
                            modelo.IdCategoria = getFamily.IdCategoria;
                        }
                        Lista.Add(modelo);

                    }
                }

            }

        }

       

        


        public class _Modelo
        {
            public string ID { get; set; }
            public string NOMBRE { get; set; }
            public string DESCRIPCION { get; set; }
            public string IMAGE { get; set; }
            public bool HASPIECES { get; set; }
            public int ID_PASIGFAMCAT { get; set; }
            public DateTime F_Update { get; set; }
            public DateTime F_Created { get; set; }
            public bool ESTADO { get; set; }
            public string TokenId { get; set; }
            public string IdFamilia { get; set; }
            public string Familia { get; set; }
            public string IdCategoria { get; set; }
            public string Categoria { get; set; }
        }

        public static class COT_PTABMODEL
        {
            public static string ID = "ID";
            public static string NOMBRE = "NOMBRE";
            public static string DESCRIPCION = "DESCRIPCION";
            public static string IMG_PATH = "IMG_PATH";
            public static string HASPIECES = "HASPIECES";
            public static string ID_PASIGFAMCAT = "ID_PASIGFAMCAT";
            public static string F_UPDATE = "F_UPDATE";
            public static string F_CREATED = "F_CREATED";
            public static string ESTADO = "ESTADO";
            public static string TokenId = "TokenId";


        }
    }
    public class SQLQueryVariables
    {
        public int _ID { get; set; }
        public string _CodeAsign { get; set; }
        Coneccion Conn;

        public SQLQueryVariables(string IDTipoVariable)
        {
            _ID = Convert.ToInt32(IDTipoVariable);
            if (_ID == 1)
            {
                _CodeAsign = "A";
            }
            else if (_ID == 2)
            {
                _CodeAsign = "B";
            }
            else if (_ID == 3)
            {
                _CodeAsign = "B";
            }
            else if (_ID == 5)
            {
                _CodeAsign = "PFAMCAT";
            }
            else if (_ID == 6)
            {
                _CodeAsign = "PFAMCAT";
            }
            else
            {
                _CodeAsign = "SIN";
            }
        }

        public bool InsertVar(Variables Variables)
        {
            
            bool Retorno = false;
            string Query = "";
            Conn = new Coneccion();
            if (_ID == 1)
            {
                #region InsertString
                Query = "Insert INTO COT_MTABFAM VALUES(@FAMNAME,@FAMDESCR,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@FAMNAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@FAMDESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }

            }
            else if (_ID == 2)
            {
                #region InsertString
                Query = "Insert INTO COT_MTABCAT VALUES(@FAMNAME,@FAMDESCR,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@FAMNAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@FAMDESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }

            }
            else if (_ID == 3)
            {
                #region InsertString
                Query = "Insert INTO COT_MTABCOL VALUES(@FAMNAME,@FAMDESCR,@CSSCLASS,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@FAMNAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@FAMDESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.Parameters.AddWithValue("@CSSCLASS", Variables._CssClass);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }

            }
            else if (_ID == 4)
            {
                #region InsertString
                Query = "Insert INTO COT_MTABMA VALUES(@FAMNAME,@FAMDESCR,@PROCEDENCIA,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@FAMNAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@FAMDESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.Parameters.AddWithValue("@PROCEDENCIA", Variables._Procedencia);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }

            }
            else if (_ID == 5)
            {
                #region InsertString
                Query = "Insert INTO COT_PTABFAM VALUES(@FAMNAME,@FAMDESCR,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@FAMNAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@FAMDESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }

            }
            else if (_ID == 6)
            {
                #region InsertString
                Query = "Insert INTO COT_PTABCAT VALUES(@FAMNAME,@FAMDESCR,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@FAMNAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@FAMDESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }
            }
            else if (_ID==7)
            {
                #region InsertString
                Query = "Insert INTO COT_PTABCANALES VALUES(@NAME,@DESCR,@ESTADO)";
                #endregion

                try
                {
                    Conn.ConnPlabal.Open();
                    Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                    Conn.Cmd.Parameters.AddWithValue("@NAME", Variables._Name);
                    Conn.Cmd.Parameters.AddWithValue("@DESCR", Variables._Descrition);
                    Conn.Cmd.Parameters.AddWithValue("@ESTADO", Variables._estado);
                    Conn.Cmd.ExecuteNonQuery();
                    Conn.ConnPlabal.Close();

                    Retorno = true;
                }
                catch
                {
                    Retorno = false;
                }
            }


            return Retorno;
        }

        public bool DeletetVar(string IDVariable)
        {

            bool Isvalid = false;
            bool Estado = false;
            string Query = "";
            Conn = new Coneccion();

            if (_ID == 1)
            {
                //FAMILIA DE COMPONENTES
                Query = "UPDATE COT_MTABFAM SET ESTADO=@Estado WHERE ID=@ID";
            }
            else if (_ID == 2)
            {
                //CATEGORIAS DE COMPONENTES
                Query = "UPDATE COT_MTABCAT SET ESTADO=@Estado WHERE ID=@ID";
            }
            else if (_ID == 3)
            {
                //COLORES DE COMPONENTES
                Query = "UPDATE COT_MTABCOL SET ESTADO=@Estado WHERE ID=@ID";
            }
            else if (_ID == 4)
            {
                //MARCAS DE COMPONENTES
                Query = "UPDATE COT_MTABMA SET ESTADO=@Estado WHERE ID=@ID";

            }
            else if (_ID == 5)
            {
                //FAMILIA PRODUCTOS
                Query = "UPDATE COT_PTABFAM SET ESTADO=@Estado WHERE ID=@ID";

            }
            else if (_ID == 6)
            {
                //CATEGORIAS PRODUCTOS
                Query = "UPDATE COT_PTABCAT SET ESTADO=@Estado WHERE ID=@ID";

            }
            else if (_ID==7)
            {
                Query = "UPDATE COT_PTABCANALES SET ESTADO=@Estado WHERE ID=@ID";
            }

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Estado", Estado);
                Conn.Cmd.Parameters.AddWithValue("@ID", IDVariable);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                Isvalid = true;
            }
            catch
            {
                Conn.ConnPlabal.Close();
                Isvalid = false;
            }


            return Isvalid;

        }

        public bool UpdateVar(Variables DatosVariable)
        {
            bool Isvalid = false;



            return Isvalid;
        }

        public bool InsertAsign(string IdVarA, string IdVarB)
        {
            bool Isvalid = false;
            string Query = "";
            bool Estado = true;
            Conn = new Coneccion();
            if (_CodeAsign == "A")
            {
                Query = "Insert INTO COT_MASIGFAMCAT VALUES(@VARA,@VARB,GETDATE(),@ESTADO)";
            }
            else if (_CodeAsign == "B")
            {
                Query = "Insert INTO COT_MASIGCATCOL VALUES(@VARA,@VARB,GETDATE(),@ESTADO)";
            }
            else if (_CodeAsign == "PFAMCAT")
            {
                Query = "Insert INTO COT_PASIGFAMCAT VALUES(@VARA,@VARB,GETDATE(),@ESTADO)";
            }

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@VARA", IdVarA);
                Conn.Cmd.Parameters.AddWithValue("@VARB", IdVarB);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", Estado);
                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                Isvalid = true;
            }
            catch (Exception ex)
            {
                throw ex;

                Isvalid = false;
            }


            return Isvalid;
        }

    }

    public class BomModel
    {
        public List<Bom> Lista;

        public Bom ItemBom;

        static SqlDataReader dr;
        Coneccion Conn;

        #region Constructores
        public BomModel()
        {

        }

        public BomModel(int ID, bool ESTADO)
        {
            
            Conn = new Coneccion();



            string consulta = "SELECT * FROM COT_PMODELBOM WHERE ID=@ID AND ESTADO=@ESTADO ORDER BY POS_NR,LEVEL";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(consulta, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Validadores Val = new Validadores();
                ItemBom = new Bom
                {
                    ID = dr["ID"].ToString(),
                    ID_MODEL = dr["ID_MODEL"].ToString(),
                    NODE = dr["NODE"].ToString(),
                    POS_NR = dr["POS_NR"].ToString(),
                    LEVEL = dr["LEVEL"].ToString(),
                    NOMBRE = dr["NOMBRE"].ToString(),
                    CANT_PRED = Convert.ToDouble(dr["CANT_PRED"].ToString()),
                    ALFAK_CODE = dr["ALFAK_CODE"].ToString(),
                    COSTO = Convert.ToDouble(dr["COSTO"].ToString()),
                    FROM_TAB = dr["FROM_TAB"].ToString(),
                    ID_FROM_TAB = dr["ID_FROM_TAB"].ToString(),
                    ESTADO = Convert.ToBoolean(dr["ESTADO"].ToString()),
                    FORMULA = dr["FORMULA"].ToString(),
                    OPCIONAL = Val.ParseoBoolean(dr["OPCIONAL"].ToString()),
                };
                double _Ancho;
                double _Largo;
                if (double.TryParse(dr["ALTO"].ToString(), out _Largo))
                {
                    ItemBom.LARGO = _Largo;
                }
                else
                {
                    ItemBom.LARGO = 0;
                }
                if (double.TryParse(dr["ANCHO"].ToString(), out _Ancho))
                {
                    ItemBom.ANCHO = _Ancho;
                }
                else
                {
                    ItemBom.ANCHO = 0;
                }

            }
            else
            {
                
            }

            

        }

        public BomModel(string ID_MODEL, bool ESTADO)
        {
            Lista = new List<Bom>();
            Conn = new Coneccion();



            string consulta = "SELECT * FROM COT_PMODELBOM WHERE ID_MODEL=@ID_MODEL AND ESTADO=@ESTADO ORDER BY NODE,LEVEL,POS_NR ASC";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                Validadores Val = new Validadores();
                foreach (DataRow drR in TablaDetalle.Rows)
                {
                    
                    Bom bom = new Bom
                    {
                        ID = drR["ID"].ToString(),
                        ID_MODEL = ID_MODEL,
                        NODE = drR["NODE"].ToString(),
                        POS_NR = drR["POS_NR"].ToString(),
                        LEVEL = drR["LEVEL"].ToString(),
                        NOMBRE = drR["NOMBRE"].ToString(),
                        CANT_PRED = Convert.ToDouble(drR["CANT_PRED"].ToString()),
                        ALFAK_CODE = drR["ALFAK_CODE"].ToString(),
                        COSTO = Convert.ToDouble(drR["COSTO"].ToString()),
                        FROM_TAB = drR["FROM_TAB"].ToString(),
                        ID_FROM_TAB = drR["ID_FROM_TAB"].ToString(),
                        ESTADO = Convert.ToBoolean(drR["ESTADO"].ToString()),
                        FORMULA = drR["FORMULA"].ToString(),
                        OPCIONAL = Val.ParseoBoolean(drR["OPCIONAL"].ToString()),
                    };
                    double _Ancho;
                    double _Largo;
                    if (double.TryParse(drR["ALTO"].ToString(), out _Largo))
                    {
                        bom.LARGO = _Largo;
                    }
                    else
                    {
                        bom.LARGO = 0;
                    }
                    if (double.TryParse(drR["ANCHO"].ToString(), out _Ancho))
                    {
                        bom.ANCHO = _Ancho;
                    }
                    else
                    {
                        bom.ANCHO = 0;
                    }

                    Lista.Add(bom);

                }
            }

        }

        public BomModel(string ID_MODEL, bool ESTADO, string LEVEL)
        {
            Lista = new List<Bom>();
            Conn = new Coneccion();



            string consulta = "SELECT * FROM COT_PMODELBOM WHERE ID_MODEL=@ID_MODEL AND ESTADO=@ESTADO AND LEVEL=@LEVEL ORDER BY POS_NR,LEVEL";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);
                    adaptador.SelectCommand.Parameters.AddWithValue("@LEVEL", LEVEL);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drR in TablaDetalle.Rows)
                {
                    Bom bom = new Bom
                    {
                        ID = drR["ID"].ToString(),
                        ID_MODEL = ID_MODEL,
                        NODE = drR["NODE"].ToString(),
                        POS_NR = drR["POS_NR"].ToString(),
                        LEVEL = drR["LEVEL"].ToString(),
                        NOMBRE = drR["NOMBRE"].ToString(),
                        CANT_PRED = Convert.ToDouble(drR["CANT_PRED"].ToString()),
                        ALFAK_CODE = drR["ALFAK_CODE"].ToString(),
                        COSTO = Convert.ToDouble(drR["COSTO"].ToString()),
                        FROM_TAB = drR["FROM_TAB"].ToString(),
                        ID_FROM_TAB = drR["ID_FROM_TAB"].ToString(),
                        ESTADO = Convert.ToBoolean(drR["ESTADO"].ToString()),
                        FORMULA = drR["FORMULA"].ToString(),
                    };
                    double _Ancho;
                    double _Largo;
                    if (double.TryParse(drR["ALTO"].ToString(), out _Largo))
                    {
                        bom.LARGO = _Largo;
                    }
                    else
                    {
                        bom.LARGO = 0;
                    }
                    if (double.TryParse(drR["ANCHO"].ToString(), out _Ancho))
                    {
                        bom.ANCHO = _Ancho;
                    }
                    else
                    {
                        bom.ANCHO = 0;
                    }

                    Lista.Add(bom);

                }
            }


        }

        public BomModel(string ID_MODEL, bool ESTADO, string LEVEL, string NODE)
        {
            Lista = new List<Bom>();
            Conn = new Coneccion();



            string consulta = "SELECT * FROM COT_PMODELBOM WHERE ID_MODEL=@ID_MODEL AND ESTADO=@ESTADO AND LEVEL=@LEVEL AND NODE=@NODE ORDER BY POS_NR,LEVEL";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);
                    adaptador.SelectCommand.Parameters.AddWithValue("@LEVEL", LEVEL);
                    adaptador.SelectCommand.Parameters.AddWithValue("@NODE", NODE);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drR in TablaDetalle.Rows)
                {
                    Bom bom = new Bom
                    {
                        ID = drR["ID"].ToString(),
                        ID_MODEL = ID_MODEL,
                        NODE = drR["NODE"].ToString(),
                        POS_NR = drR["POS_NR"].ToString(),
                        LEVEL = drR["LEVEL"].ToString(),
                        NOMBRE = drR["NOMBRE"].ToString(),
                        CANT_PRED = Convert.ToDouble(drR["CANT_PRED"].ToString()),
                        ALFAK_CODE = drR["ALFAK_CODE"].ToString(),
                        COSTO = Convert.ToDouble(drR["COSTO"].ToString()),
                        FROM_TAB = drR["FROM_TAB"].ToString(),
                        ID_FROM_TAB = drR["ID_FROM_TAB"].ToString(),
                        ESTADO = Convert.ToBoolean(drR["ESTADO"].ToString()),
                        FORMULA = drR["FORMULA"].ToString(),
                    };
                    double _Ancho;
                    double _Largo;
                    if (double.TryParse(drR["ALTO"].ToString(), out _Largo))
                    {
                        bom.LARGO = _Largo;
                    }
                    else
                    {
                        bom.LARGO = 0;
                    }
                    if (double.TryParse(drR["ANCHO"].ToString(), out _Ancho))
                    {
                        bom.ANCHO = _Ancho;
                    }
                    else
                    {
                        bom.ANCHO = 0;
                    }

                    Lista.Add(bom);

                }
            }

        }
        #endregion



        #region Funciones
        public List<_Componente> GetCompoByWGR(string KA_WGR, bool ESTADO)
        {
            
            _GetListComponentes _GetList = new _GetListComponentes(KA_WGR, ESTADO);

            return _GetList.ListComponentes;
        }

       public string[] GetInfoItem(string Tabla, string ID)
        {
            string[] datos = new string[3];

            if (Tabla=="COT_MCOMPONENTES")
            {
                ComponentesClass componente = new ComponentesClass(ID, true);
                datos[0] = componente._Detalle.UnMedSimbolo;
                datos[1] = componente._Detalle.UnidadMedida;
                datos[2] = componente._Detalle.PrecioUn.ToString();
            }
            else if (Tabla=="COT_MPROCESOS")
            {
                ProcesosClass ProClass = new ProcesosClass();
                ProcesosClass._Proceso _proceso = ProClass.GetProceso(ID, true);
                datos[0] = _proceso._MagSimbolo;
                datos[1] = _proceso._Unidad_Medida;
                datos[2] = _proceso.Costo_Unit.ToString();
            }



            return datos;
        }

        public bool InsertBom(Bom bom)
        {
            bool IsInserted = false;
            Conn = new Coneccion();

            #region InsertString
            string Query = "Insert INTO COT_PMODELBOM (ID_MODEL,NODE,POS_NR,LEVEL,NOMBRE,CANT_PRED,ALFAK_CODE,COSTO,FROM_TAB,ID_FROM_TAB,ANCHO,ALTO,FORMULA,ESTADO,OPCIONAL)" +
                "VALUES (@ID_MODEL,@NODE,@POS_NR,@LEVEL,@NOMBRE,@CANT_PRED,@ALFAK_CODE,@COSTO,@FROM_TAB,@ID_FROM_TAB,@ANCHO,@ALTO,@FORMULA,@ESTADO,@OPCIONAL)";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@ID_MODEL", bom.ID_MODEL);
                Conn.Cmd.Parameters.AddWithValue("@NODE", bom.NODE);
                Conn.Cmd.Parameters.AddWithValue("@POS_NR", bom.POS_NR);
                Conn.Cmd.Parameters.AddWithValue("@LEVEL", bom.LEVEL);
                Conn.Cmd.Parameters.AddWithValue("@NOMBRE", bom.NOMBRE);
                Conn.Cmd.Parameters.AddWithValue("@CANT_PRED", bom.CANT_PRED);
                Conn.Cmd.Parameters.AddWithValue("@ALFAK_CODE", bom.ALFAK_CODE);
                Conn.Cmd.Parameters.AddWithValue("@COSTO", bom.COSTO);
                Conn.Cmd.Parameters.AddWithValue("@FROM_TAB", bom.FROM_TAB);
                Conn.Cmd.Parameters.AddWithValue("@ID_FROM_TAB", bom.ID_FROM_TAB);
                Conn.Cmd.Parameters.AddWithValue("@ANCHO", bom.ANCHO);
                Conn.Cmd.Parameters.AddWithValue("@ALTO", bom.LARGO);
                Conn.Cmd.Parameters.AddWithValue("@FORMULA", bom.FORMULA);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", bom.ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@OPCIONAL", bom.OPCIONAL);



                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsInserted = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsInserted = false;
            }



            return IsInserted;

        }

        public bool DeleteItem(string ID_MODEL,string ID_Item, bool ESTADO)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_PMODELBOM SET ESTADO=@ESTADO WHERE ID=@ID AND ID_MODEL=@ID_MODEL";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@ID",ID_Item);
                Conn.Cmd.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);

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

        public bool DeleteNode(string ID_MODEL, string Node, bool ESTADO)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_PMODELBOM SET ESTADO=@ESTADO WHERE NODE=@NODE AND ID_MODEL=@ID_MODEL";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@NODE", Node);
                Conn.Cmd.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);

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

        public string GetPos_Nr(string ID_MODEL, bool ESTADO, string LEVEL)
        {
            int Nro;
            string Select = "SELECT COUNT(ID) FROM COT_PMODELBOM WHERE ID_MODEL=@ID_MODEL AND ESTADO=@ESTADO AND LEVEL=@LEVEL";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            Conn.CmdPlabal.Parameters.AddWithValue("@LEVEL", LEVEL);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Nro = Convert.ToInt32(dr[0].ToString()) + 1;
            }
            else
            {
                Nro = 1;
            }

            dr.Close();
            Conn.ConnPlabal.Close();



            return Nro.ToString();
        }

        public string GetPos_Nr(string ID_MODEL, bool ESTADO, string LEVEL,string NODE)
        {
            int Nro;
            string Select = "SELECT COUNT(ID) FROM COT_PMODELBOM WHERE ID_MODEL=@ID_MODEL AND ESTADO=@ESTADO AND LEVEL=@LEVEL AND NODE=@NODE";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID_MODEL", ID_MODEL);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            Conn.CmdPlabal.Parameters.AddWithValue("@LEVEL", LEVEL);
            Conn.CmdPlabal.Parameters.AddWithValue("@NODE", NODE);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Nro = Convert.ToInt32(dr[0].ToString()) + 1;
            }
            else
            {
                Nro = 1;
            }

            dr.Close();
            Conn.ConnPlabal.Close();



            return Nro.ToString();
        }

        #endregion




        public class Bom
        {
            public string ID { get; set; }
            public string ID_MODEL { get; set; }
            public string NODE { get; set; }
            public string POS_NR { get; set; }
            public string LEVEL { get; set; }
            public string NOMBRE { get; set; }
            public double CANT_PRED { get; set; }
            public string ALFAK_CODE { get; set; }
            public double COSTO { get; set; }
            public string FROM_TAB { get; set; }
            public string ID_FROM_TAB { get; set; }
            public double ANCHO { get; set; }
            public double LARGO { get; set; }
            public string FORMULA { get; set; }
            public bool ESTADO { get; set; }
            public bool OPCIONAL { get; set; }
        }
    }

    public class ProcesosClass
    {
        public string IdBeforeInsert { get; set; }
        public List<_Proceso> Procesos { get; set; }

        Coneccion Conn;
        static SqlDataReader dr;


        public ProcesosClass()
        {

        }

        public ProcesosClass(bool ESTADO)
        {
            
            Procesos = new List<_Proceso>();
            

            string Select = "select * FROM COT_MPROCESOS WHERE ESTADO=@ESTADO ORDER BY NOMBRE DESC";

            Conn = new Coneccion();

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                   

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drr in TablaDetalle.Rows)
                {
                    _Proceso Item = new _Proceso
                    {
                        _ID = drr["ID"].ToString(),
                        Nombre = drr["NOMBRE"].ToString(),
                        Descripcion = drr["DESCRIPCION"].ToString(),
                        F_Update = Convert.ToDateTime(drr["F_UPDATE"].ToString()),
                        F_Created = Convert.ToDateTime(drr["F_CREATED"].ToString()),
                        _KA_WGR = drr["KA_WGR"].ToString(),
                        _CodAlfak = drr["COD_ALFAK"].ToString(),
                        Costo_Unit = Convert.ToDouble(drr["COSTO_UNIT"].ToString()),
                        Merma= Convert.ToDouble(drr["MERMA"].ToString()),
                        Estado=Convert.ToBoolean(drr["ESTADO"].ToString()),
                        TokenId = drr["TokenId"].ToString(),
                       
                    };
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(drr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {

                        Item._Magnitud = Mag._Magnitud.MAGNITUD;
                        Item._MagSimbolo = Mag._Magnitud.SIMBOLO;
                        Item._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                    }

                    Procesos.Add(Item);

                }
            }

        }


        public _Proceso GetProceso (string ID, string TokenId,bool ESTADO)
        {
            _Proceso Item;
            
            string Select = "SELECT TOP 1 * FROM COT_MPROCESOS WHERE ID=@ID AND TokenId=@TokenId AND ESTADO=@ESTADO ";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
            Conn.CmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Item = new _Proceso {
                    _ID = dr["ID"].ToString(),
                    Nombre = dr["NOMBRE"].ToString(),
                    Descripcion = dr["DESCRIPCION"].ToString(),
                    ID_Magmed = dr["ID_MAGMED"].ToString(),
                    F_Update = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                    F_Created = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                    Costo_Unit = Convert.ToDouble(dr["COSTO_UNIT"].ToString()),
                    Merma = Convert.ToDouble(dr["MERMA"].ToString()),
                    Estado = Convert.ToBoolean(dr["ESTADO"].ToString()),
                    TokenId = dr["TokenId"].ToString(),
                    _CodAlfak = dr["COD_ALFAK"].ToString(),

                };
                GetMagnitudes Mag = new GetMagnitudes();
                bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                if (HasMag)
                {

                    Item._Magnitud = Mag._Magnitud.MAGNITUD;
                    Item._MagSimbolo = Mag._Magnitud.SIMBOLO;
                    Item._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                }
            }
            else
            {
                Item = new _Proceso
                {
                    Estado = false
                };
            }

            dr.Close();
            Conn.ConnPlabal.Close();



            return Item;
        }
        public _Proceso GetProceso(string ID,bool ESTADO)
        {
            _Proceso Item;
            
            string Select = "SELECT TOP 1 * FROM COT_MPROCESOS WHERE ID=@ID AND ESTADO=@ESTADO";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
            Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                Item = new _Proceso
                {
                    _ID = dr["ID"].ToString(),
                    Nombre = dr["NOMBRE"].ToString(),
                    Descripcion = dr["DESCRIPCION"].ToString(),
                    ID_Magmed = dr["ID_MAGMED"].ToString(),
                    F_Update = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                    F_Created = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                    Costo_Unit = Convert.ToDouble(dr["COSTO_UNIT"].ToString()),
                    Merma = Convert.ToDouble(dr["MERMA"].ToString()),
                    Estado = Convert.ToBoolean(dr["ESTADO"].ToString()),
                    TokenId = dr["TokenId"].ToString(),
                    _CodAlfak= dr["COD_ALFAK"].ToString(),

                };
                GetMagnitudes Mag = new GetMagnitudes();
                bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                if (HasMag)
                {

                    Item._Magnitud = Mag._Magnitud.MAGNITUD;
                    Item._MagSimbolo = Mag._Magnitud.SIMBOLO;
                    Item._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                }
            }
            else
            {
                Item = new _Proceso
                {
                    Estado = false
                };
            }

            dr.Close();
            Conn.ConnPlabal.Close();



            return Item;
        }

        public bool Insert(_Proceso DatosProceso)
        {
            bool IsInserted = false;
            Conn = new Coneccion();

            #region InsertString
            string Query = "Insert INTO COT_MPROCESOS (NOMBRE,DESCRIPCION,ID_MAGMED,F_UPDATE,F_CREATED,COSTO_UNIT,MERMA,ESTADO,TokenId) VALUES (@NOMBRE,@DESCRIPCION,@ID_MAGMED,GETDATE(),GETDATE(),@COSTO_UNIT,@MERMA,@ESTADO,@TokenId)";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@NOMBRE", DatosProceso.Nombre);
                Conn.Cmd.Parameters.AddWithValue("@DESCRIPCION", DatosProceso.Descripcion);
                Conn.Cmd.Parameters.AddWithValue("@COSTO_UNIT", DatosProceso.Costo_Unit);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", DatosProceso.Estado);
                Conn.Cmd.Parameters.AddWithValue("@TokenId", DatosProceso.TokenId);
                Conn.Cmd.Parameters.AddWithValue("@MERMA", DatosProceso.Merma);
                Conn.Cmd.Parameters.AddWithValue("@ID_MAGMED", DatosProceso.ID_Magmed);


                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsInserted = true;
                GetIdRowBeforeInsert(DatosProceso.Nombre,DatosProceso.TokenId);
            }
            catch
            {
                IsInserted = false;
            }



            return IsInserted;
        }

        private void GetIdRowBeforeInsert(string Nombre, string TokenId)
        {

            string Select = "SELECT TOP 1 ID FROM COT_MPROCESOS WHERE NOMBRE=@NOMBRE AND TokenId=@TokenId";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@NOMBRE", Nombre);
            Conn.CmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                IdBeforeInsert = dr[0].ToString();
            }

            dr.Close();
            Conn.ConnPlabal.Close();

        }

        public bool UpdateProceso (string RowID, string Campo, string Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MPROCESOS SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

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

        public bool UpdateProceso(string RowID, string Campo, bool Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MPROCESOS SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;

        }

        public bool UpdateProceso(string RowID, string Campo, double Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MPROCESOS SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;

        }

        public bool UpdateProceso(_Proceso proceso)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MPROCESOS SET NOMBRE=@NOMBRE,DESCRIPCION=@DESCRIPCION,ID_MAGMED=@ID_MAGMED,F_UPDATE=GETDATE(),COSTO_UNIT=@COSTO_UNIT,MERMA=@MERMA,ESTADO=@ESTADO WHERE ID=@ID AND TokenId=@TokenId";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@NOMBRE", proceso.Nombre );
                Conn.Cmd.Parameters.AddWithValue("@ID",proceso._ID );
                Conn.Cmd.Parameters.AddWithValue("@TokenId", proceso.TokenId );
                Conn.Cmd.Parameters.AddWithValue("@DESCRIPCION", proceso.Descripcion );
                Conn.Cmd.Parameters.AddWithValue("@ID_MAGMED", proceso.ID_Magmed );
                Conn.Cmd.Parameters.AddWithValue("@COSTO_UNIT", proceso.Costo_Unit );
                Conn.Cmd.Parameters.AddWithValue("@MERMA", proceso.Merma);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", proceso.Estado);


                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;

        }

        public bool UpdateAsignProc(string RowId,string Campo, bool Valor)
        {

            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MASIGPROC SET " + Campo + "=@Valor WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowId);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;

        }

        public bool InsertAsignProc(bool COMPBELONG,string ID_PROC, string ID_MOCOMP, bool OBLIGATORIO)
        {
            bool IsInserted = false;
            bool ESTADO = true;
            Conn = new Coneccion();

            #region InsertString
            string Query = "Insert INTO COT_MASIGPROC VALUES (@COMPBELONG,@ID_PROC,@ID_MOCOMP,GETDATE(),@OBLIGATORIO,@ESTADO)";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@COMPBELONG", COMPBELONG);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@ID_PROC", ID_PROC);
                Conn.Cmd.Parameters.AddWithValue("@ID_MOCOMP", ID_MOCOMP);
                Conn.Cmd.Parameters.AddWithValue("@OBLIGATORIO", OBLIGATORIO);


                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsInserted = true;
                SetHasProc(ID_MOCOMP, COMPBELONG);
                
            }
            catch(Exception ex)
            {
                throw ex;
                IsInserted = false;
            }



            return IsInserted;
        }

        private void SetHasProc(string ID_MOCOMP, bool COMPBELONG)
        {
            string Query;
            bool ESTADO = true;
            #region Query
            if (COMPBELONG)
            {
                Query = "UPDATE COT_MCOMPONENTES SET HASPROC=@ESTADO, F_UPDATE=GETDATE() WHERE ID=@ID_MOCOMP ";
            }
            else
            {
                Query = "UPDATE COT_MPROCESOS SET HASPROC=@ESTADO, F_UPDATE=GETDATE() WHERE ID=@ID_MOCOMP";
            }
            
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@ID_MOCOMP", ID_MOCOMP);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                

            }
            catch (Exception ex)
            {
                throw ex;
                
            }

        }

        public class GetProcNoAsign
        {
           public List<_Proceso> Procesos { get; set; }

            Coneccion Conn;

            public GetProcNoAsign(string ID_MOCOMP, bool COMPBELONG)
            {
                bool ESTADO = true;
                Procesos = new List<_Proceso>();
                _Proceso Item;

                string Select = "SELECT * FROM COT_MPROCESOS P WHERE NOT P.ID IN (SELECT ASI.ID_PROC FROM COT_MASIGPROC ASI WHERE ASI.ESTADO=@ESTADO AND ASI.COMPBELONG=@COMPBELONG AND ASI.ID_MOCOMP=@ID_MOCOMP) AND P.ESTADO =@ESTADO";

                Conn = new Coneccion();

                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ID_MOCOMP", ID_MOCOMP);
                        adaptador.SelectCommand.Parameters.AddWithValue("@COMPBELONG", COMPBELONG);

                        adaptador.Fill(TablaDetalle);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {

                    foreach (DataRow drr in TablaDetalle.Rows)
                    {
                        Item = new _Proceso
                        {
                            _ID = drr["ID"].ToString(),
                            Nombre = drr["NOMBRE"].ToString(),
                            Descripcion = drr["DESCRIPCION"].ToString(),
                            ID_Magmed = drr["ID_MAGMED"].ToString(),
                            F_Update = Convert.ToDateTime(drr["F_UPDATE"].ToString()),
                            F_Created = Convert.ToDateTime(drr["F_CREATED"].ToString()),
                            Costo_Unit = Convert.ToDouble(drr["COSTO_UNIT"].ToString()),
                            Merma = Convert.ToDouble(drr["MERMA"].ToString()),
                            Estado = Convert.ToBoolean(drr["ESTADO"].ToString()),
                            TokenId = drr["TokenId"].ToString(),
                            _CodAlfak= drr["COD_ALFAK"].ToString(),

                        };
                        GetMagnitudes Mag = new GetMagnitudes();
                        bool HasMag = Mag.IsMagnitud(drr["ID_MAGMED"].ToString());
                        if (HasMag)
                        {

                            Item._Magnitud = Mag._Magnitud.MAGNITUD;
                            Item._MagSimbolo = Mag._Magnitud.SIMBOLO;
                            Item._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                        }

                        Procesos.Add(Item);

                    }
                }

            }
                
            

            
        }

        public class GetProcAsign
        {

           public List<ProcAsign> Procesos { get; set; }

            Coneccion Conn;

            public GetProcAsign(string ID_MOCOMP, bool COMPBELONG)
            {
                bool ESTADO = true;
                Procesos = new List<ProcAsign>();
                ProcAsign Item;

                string Select = "select ASI.ID as 'ID_ASIGN', ASI.OBLIGATORIO, ASI.ASIGNDATE, P.* FROM COT_MPROCESOS P, COT_MASIGPROC ASI WHERE P.ID=ASI.ID_PROC AND P.ESTADO=@ESTADO AND ASI.ESTADO=@ESTADO AND ASI.COMPBELONG=@COMPBELONG AND ASI.ID_MOCOMP=@ID_MOCOMP";

                Conn = new Coneccion();

                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnPlabal)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                        adaptador.SelectCommand.Parameters.AddWithValue("@ID_MOCOMP", ID_MOCOMP);
                        adaptador.SelectCommand.Parameters.AddWithValue("@COMPBELONG", COMPBELONG);

                        adaptador.Fill(TablaDetalle);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (TablaDetalle.Rows.Count > 0)
                {

                    foreach (DataRow drr in TablaDetalle.Rows)
                    {
                        Item = new ProcAsign
                        {
                            ID_Proc = drr["ID"].ToString(),
                            Nombre = drr["NOMBRE"].ToString(),
                            Descripcion = drr["DESCRIPCION"].ToString(),
                            ID_Magmed = drr["ID_MAGMED"].ToString(),
                            F_Asign = Convert.ToDateTime(drr["ASIGNDATE"].ToString()),
                            Costo_Unit = Convert.ToDouble(drr["COSTO_UNIT"].ToString()),
                            Merma = Convert.ToDouble(drr["MERMA"].ToString()),
                            ID_Asign = drr["ID_ASIGN"].ToString(),
                            OBLIGATORIO = Convert.ToBoolean(drr["OBLIGATORIO"]),
                            CodAlfak= drr["COD_ALFAK"].ToString(),

                        };
                        GetMagnitudes Mag = new GetMagnitudes();
                        bool HasMag = Mag.IsMagnitud(drr["ID_MAGMED"].ToString());
                        if (HasMag)
                        {

                            Item._Magnitud = Mag._Magnitud.MAGNITUD;
                            Item._MagSimbolo = Mag._Magnitud.SIMBOLO;
                            Item._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                        }

                        Procesos.Add(Item);

                    }
                }

            }

            public class ProcAsign
            {
                public string ID_Asign { get; set; }
                public bool OBLIGATORIO { get; set; }
                public string ID_Proc { get; set; }
                public string Nombre { get; set; }
                public string Descripcion { get; set; }
                public DateTime F_Asign { get; set; }
                public string ID_Magmed { get; set; }
                public string _Magnitud { get; set; }
                public string _MagSimbolo { get; set; }
                public string _Unidad_Medida { get; set; }
                public double Costo_Unit { get; set; }
                public double Merma { get; set; }
                public string CodAlfak { get; set; }

            }

        }




        public class _Proceso
        {
            public string _ID { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string ID_Magmed { get; set; }
            public DateTime F_Update { get; set; }
            public DateTime F_Created { get; set; }
            public double Costo_Unit { get; set; }
            public double Merma { get; set; }
            public bool Estado { get; set; }
            public string TokenId { get; set; }
            public string _Magnitud { get; set; }
            public string _Unidad_Medida { get; set; }
            public string _MagSimbolo { get; set; }
            public string _CodAlfak { get; set; }
            public string _KA_WGR { get; set; }


        }


        public static class COT_MASIGPROC
        {
            public static string ID = "ID";
            public static string COMPBELONG = "COMPBELONG";
            public static string ID_PROC = "ID_PROC";
            public static string ID_MOCOMP = "ID_MOCOMP";
            public static string OBLIGATORIO = "OBLIGATORIO";
            public static string ESTADO = "ESTADO";
        }
        public static class COT_MPROCESOS
        {
            public static string ID = "ID";
            public static string NOMBRE = "NOMBRE";
            public static string DESCRIPCION = "DESCRIPCION";
            public static string ID_MAGMED = "ID_MAGMED";
            public static string F_UPDATE = "F_UPDATE";
            public static string F_CREATED = "F_CREATED";
            public static string COSTO_UNIT = "COSTO_UNIT";
            public static string MERMA = "MERMA";
            public static string ESTADO = "ESTADO";
            public static string TokenId = "TokenId";
            public static string COD_ALFAK = "COD_ALFAK";
            public static string KA_WGR = "KA_WGR";

        }
    }

    public class ComponentesClass
    {
        public string IdReturnedInsert { get; set; }

        public _Componente _Detalle { get; set; }

        Coneccion Conn;
        static SqlDataReader dr;

        public ComponentesClass()
        {

        }

        public ComponentesClass(string ID,bool ESTADO)
        {
            string Select = @"SELECT * FROM COT_MCOMPONENTES WHERE ID=@ID AND ESTADO=@ESTADO";
            Conn = new Coneccion();


            try
            {


                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
                Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores val = new Validadores();
                    _Detalle = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"]),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        COD_ALFAK = dr["COD_ALFAK"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        ID_Magnitud = dr["ID_MAGMED"].ToString(),
                        IsGlass = val.ParseoBoolean(dr["IsGlass"].ToString()),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasColor)
                    {
                        _Detalle.Color = Line.Color;
                        _Detalle.ID_Color = Line.ID_Color;

                    }
                    if (Line.HasMarca)
                    {
                        _Detalle.Marca = Line._Nombre_Marca;
                        _Detalle.ID_Marca = Line._ID_MARCA;
                    }
                    if (Line.HasVarline)
                    {
                        _Detalle.Categoria = Line.Categoria;
                        _Detalle.Familia = Line.Familia;
                        _Detalle.ID_Categoria = Line.ID_Categoria;
                        _Detalle.ID_Familia = Line.ID_Familia;
                        _Detalle.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();
                    }

                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        _Detalle.ID_Magnitud = Mag._Magnitud._ID;
                        _Detalle.Magnitud = Mag._Magnitud.MAGNITUD;
                        _Detalle.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                        _Detalle.UnidadMedida = Mag._Magnitud.UNIDAD_MEDIDA;
                    }

                   
                }
                else
                {
                    
                }


                dr.Close();
                Conn.ConnPlabal.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                
            }

        }

       


        public bool InsertinTabla(string Nombre, string Descripcion, string URLImagen, int TokenId)
        {
            bool IsInserted = false;
            Conn = new Coneccion();
            bool ESTADO = true;
            

            #region InsertString
            string Query = "Insert INTO COT_MCOMPONENTES (NOMBRE,DESCRIPCION,CANT_UNIT,PRECIO_UNIT,F_UPDATE,F_CREATED,HASPROC,IMG_PATH,ESTADO,TokenId) VALUES (@NOMBRE,@DESCRIPCION,0,0,GETDATE(),GETDATE(),@HASPROC,@IMG_PATH,@ESTADO,@TokenId)";
            #endregion

            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@NOMBRE", Nombre );
                Conn.Cmd.Parameters.AddWithValue("@DESCRIPCION", Descripcion);
                Conn.Cmd.Parameters.AddWithValue("@IMG_PATH", URLImagen);
                Conn.Cmd.Parameters.AddWithValue("@ESTADO", ESTADO);
                Conn.Cmd.Parameters.AddWithValue("@TokenId", TokenId);
                Conn.Cmd.Parameters.AddWithValue("@HASPROC", IsInserted);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsInserted = true;
                GetIdRowBeforeInsert(Nombre,TokenId);
            }
            catch
            {
                IsInserted = false;
            }


            return IsInserted;
        }

        private void GetIdRowBeforeInsert(string Nombre, int TokenId)
        {
            string Select = "SELECT TOP 1 ID FROM COT_MCOMPONENTES WHERE NOMBRE=@NOMBRE AND TokenId=@TokenId";
            Conn = new Coneccion();

            Conn.ConnPlabal.Open();
            Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
            Conn.CmdPlabal.Parameters.AddWithValue("@NOMBRE", Nombre);
            Conn.CmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);
            dr = Conn.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                IdReturnedInsert = dr[0].ToString();    
            }
            
            dr.Close();
            Conn.ConnPlabal.Close();

        }

        public bool IsComponente (string ID, string TOKEN)
        {
            Encriptacion encriptacion = new Encriptacion(TOKEN);
            int TokenId = Convert.ToInt32(encriptacion.TokenDesencriptado);
            bool retorno = false;
            bool ESTADO = true;
            string Select = @"SELECT * FROM COT_MCOMPONENTES WHERE ID=@ID AND TokenId=@TokenId AND ESTADO=@ESTADO";
            Conn = new Coneccion();
            

            try
            {
                

                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(Select, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID);
                Conn.CmdPlabal.Parameters.AddWithValue("@TokenId", TokenId);
                Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);
                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    _Detalle = new _Componente {
                        _ID=ID,
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"]),
                        Path_Photo= dr["IMG_PATH"].ToString(),
                        COD_ALFAK = dr["COD_ALFAK"].ToString(),

                        ID_Magnitud = dr["ID_MAGMED"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasColor)
                    {
                        _Detalle.Color = Line.Color;
                        _Detalle.ID_Color = Line.ID_Color;

                    }
                    if (Line.HasMarca)
                    {
                        _Detalle.Marca = Line._Nombre_Marca;
                        _Detalle.ID_Marca = Line._ID_MARCA;
                    }
                    if(Line.HasVarline)
                    {
                        _Detalle.Categoria = Line.Categoria;
                        _Detalle.Familia = Line.Familia;
                        _Detalle.ID_Categoria = Line.ID_Categoria;
                        _Detalle.ID_Familia = Line.ID_Familia;
                        _Detalle.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();
                    }

                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        _Detalle.ID_Magnitud = Mag._Magnitud._ID;
                        _Detalle.Magnitud = Mag._Magnitud.MAGNITUD;
                        _Detalle.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                        _Detalle.UnidadMedida = Mag._Magnitud.UNIDAD_MEDIDA;
                    }

                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
                

                dr.Close();
                Conn.ConnPlabal.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                retorno = false;
            }




            return retorno;

        }

        public bool UpdateComponente(string RowID, string Campo, string Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MCOMPONENTES SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);
                
                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;
                
            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

        public bool UpdateComponente(string RowID, string Campo, double Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MCOMPONENTES SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

        public bool UpdateComponente(string RowID, string Campo, bool Valor)
        {
            bool IsUpdated = false;

            #region Query
            string Query = "UPDATE COT_MCOMPONENTES SET " + Campo + "=@Valor, F_UPDATE=GETDATE() WHERE ID=@ID ";
            #endregion
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.Cmd = new System.Data.SqlClient.SqlCommand(Query, Conn.ConnPlabal);
                Conn.Cmd.Parameters.AddWithValue("@Valor", Valor);
                Conn.Cmd.Parameters.AddWithValue("@ID", RowID);

                Conn.Cmd.ExecuteNonQuery();
                Conn.ConnPlabal.Close();

                IsUpdated = true;

            }
            catch (Exception ex)
            {
                throw ex;
                IsUpdated = false;
            }


            return IsUpdated;
        }

       
    }

    public static class COT_MCOMPONENTES
    {
      


        public static class Campos
        {
            public static string NOMBRE()
            {
                return "NOMBRE";
            }
            public static string DESCRIPCION()
            {
                return "DESCRIPCION";
            }
            public static string ID_MTABMA()
            {
                return "ID_MTABMA";
            }
            public static string ID_MASIGFAMCAT()
            {
                return "ID_MASIGFAMCAT";
            }
            public static string ID_MTABCOL()
            {
                return "ID_MTABCOL";
            }
            public static string ID_MAGMED()
            {
                return "ID_MAGMED";
            }
            public static string CANT_UNIT()
            {
                return "CANT_UNIT";
            }
            public static string PRECIO_UNIT()
            {
                return "PRECIO_UNIT";
            }
            public static string F_UPDATE()
            {
                return "F_UPDATE";
                 
            }
            public static string F_CREATED()
            {
                return "F_CREATED";

            }
            public static string HASPROC()
            {
                return "HASPROC";

            }
            public static string IMG_PATH()
            {
                return "IMG_PATH";

            }
            public static string ESTADO()
            {
                return "ESTADO";

            }
            public static string COD_ALFAK()
            {
                return "COD_ALFAK";
            }
            public static string KA_WGR()
            {
                return "KA_WGR";
            }
        }

    }

    public class GetVarLine
    {
        public bool HasVarline { get; set; }
        public bool HasMarca { get; set; }
        public bool HasColor { get; set; }
        public string _Nombre_Marca { get; set; }
        public string _ID_MARCA { get; set; }
        public string _Descrip_Marca { get; set; }
        public string _proced_Marca { get; set; }
        public string Familia { get; set; }
        public string ID_Familia { get; set; }
        public string _Descr_Familia { get; set; }
        public string _Descr_Categoria { get; set; }
        public string Color { get; set; }
        public string ID_Color { get; set; }
        public string CSSColor { get; set; }
        public string Categoria { get; set; }
        public string ID_Categoria { get; set; }


        Coneccion Conn;
        static SqlDataReader dr0;
        static SqlDataReader dr2;
        static SqlDataReader dr3;

        public GetVarLine(string ID_MTABMA, string ID_MTABCOL, string ID_MASIGFAMCAT)
        {
            
            bool ESTADO = true;
            string SelectTABMA = "SELECT * FROM COT_MTABMA WHERE ID=@ID and ESTADO=@ESTADO ";
            string SelectTABCOL = "SELECT * FROM COT_MTABCOL WHERE ID=@ID and ESTADO=@ESTADO ";
            string SelectASIG = "SELECT B.ID AS 'IDFAM', C.ID AS 'IDCAT', B.FAMNAME,B.FAMDESCR,C.CATNAME,C.CATDESCR  FROM COT_MASIGFAMCAT A, COT_MTABFAM B, COT_MTABCAT C WHERE A.ID=@ID and A.IDFAM=B.ID and A.IDCAT=C.ID and A.ESTADO=@ESTADO ";
            Conn = new Coneccion();

            #region VerifyMtabma
            try
            {


                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(SelectTABMA, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID_MTABMA);
                Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);

                dr0 = Conn.CmdPlabal.ExecuteReader();
                dr0.Read();
                if (dr0.HasRows)
                {
                    HasMarca = true;
                    _Nombre_Marca = dr0["TMNAME"].ToString();
                    _Descrip_Marca = dr0["TMDESCR"].ToString();
                    _proced_Marca= dr0["PROCEDENCIA"].ToString();
                    _ID_MARCA = dr0["ID"].ToString();

                }
                else
                {
                    HasMarca = false;
                }

                dr0.Close();
                Conn.ConnPlabal.Close();

            }
            catch
            {
                HasMarca = false;
            }

            #endregion
            #region VerifyMtabCOL
            try
            {


                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(SelectTABCOL, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID_MTABCOL);
                Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);

                dr2 = Conn.CmdPlabal.ExecuteReader();
                dr2.Read();
                if (dr2.HasRows)
                {
                    HasColor = true;
                   Color= dr2["COLORNAME"].ToString();
                    CSSColor = dr2["CSSCLASS"].ToString();
                    ID_Color = dr2["ID"].ToString();
                }
                else
                {
                    HasColor = false;
                }

                dr2.Close();
                Conn.ConnPlabal.Close();

            }
            catch
            {
                HasColor = false;
            }

            #endregion
            #region VerifyCAT&FAM
            try
            {


                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(SelectASIG, Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID", ID_MASIGFAMCAT);
                Conn.CmdPlabal.Parameters.AddWithValue("@ESTADO", ESTADO);

                dr3 = Conn.CmdPlabal.ExecuteReader();
                dr3.Read();
                if (dr3.HasRows)
                {
                    HasVarline = true;
                    Familia = dr3["FAMNAME"].ToString();
                    _Descr_Familia = dr3["FAMDESCR"].ToString();
                    _Descr_Categoria= dr3["CATDESCR"].ToString();
                    Categoria = dr3["CATNAME"].ToString();
                    ID_Familia = dr3["IDFAM"].ToString();
                    ID_Categoria = dr3["IDCAT"].ToString();
                }
                else
                {
                    HasVarline = false;
                }

                dr3.Close();
                Conn.ConnPlabal.Close();

            }
            catch
            {
                HasVarline = false;
            }

            #endregion


        }

    }

    

    public class GetListVarAsign
    {
        public List<Variables> Listavariables { get; set; }

        public GetListVarAsign(string TypeOfAsign)
        {
            Listavariables = new List<Variables>();
            Coneccion Conn = new Coneccion();
            bool istru = true;
            string consulta = "";

            if (TypeOfAsign == "A")
            {
                consulta = @"SELECT * FROM COT_MTABFAM WHERE ESTADO=@Estado";
            }
            else if (TypeOfAsign == "B")
            {
                consulta = @"SELECT * FROM COT_MTABCAT WHERE ESTADO=@Estado";
            }
            if (TypeOfAsign == "PFAMCAT")
            {
                consulta = @"SELECT * FROM COT_PTABFAM WHERE ESTADO=@Estado";
            }



            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@Estado", istru);

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
                    Variables variable = new Variables
                    {
                        ID = drRAN[0].ToString(),
                        _Name = drRAN[1].ToString(),

                    };
                    Listavariables.Add(variable);



                }
            }

        }
    }

    public class GetListVariables
    {
        public List<Variables> Listavariables { get; set; }

        public List<Variables> ListCompCategory { get; set; }


        Coneccion Conn;
        public GetListVariables()
        {

        }

        public GetListVariables(int TypeofVar)
        {
            Listavariables = new List<Variables>();
             Conn = new Coneccion();
            bool istru = true;
            string consulta = "";

            if (TypeofVar == 1)
            {
                consulta = @"SELECT * FROM COT_MTABFAM WHERE ESTADO=@Estado";
               
            }
            else if (TypeofVar == 2)
            {
                consulta = @"SELECT * FROM COT_MTABCAT WHERE ESTADO=@Estado";
            }
            else if (TypeofVar == 3)
            {
                consulta = @"SELECT * FROM COT_MTABCOL WHERE ESTADO=@Estado";
            }
            else if (TypeofVar == 4)
            {
                consulta = @"SELECT * FROM COT_MTABMA WHERE ESTADO=@Estado";
            }
            else if (TypeofVar == 5)
            {
                consulta = @"SELECT * FROM COT_PTABFAM WHERE ESTADO=@Estado";
            }
            else if (TypeofVar == 6)
            {
                consulta = @"SELECT * FROM COT_PTABCAT WHERE ESTADO=@Estado";
            }



            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@Estado", istru);

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
                    Variables variable;
                    if (TypeofVar == 4)
                    {
                        variable = new Variables
                        {
                            ID = drRAN[0].ToString(),
                            _Name = drRAN[1].ToString(),
                            _Descrition = drRAN[2].ToString(),
                            _Procedencia = drRAN[3].ToString(),

                        };
                    }
                    else
                    {
                        variable = new Variables
                        {
                            ID = drRAN[0].ToString(),
                            _Name = drRAN[1].ToString(),
                            _Descrition = drRAN[2].ToString(),


                        };
                    }

                    Listavariables.Add(variable);



                }
            }

        }

        public void GetListCompCategory(string IDFAM)
        {


            ListCompCategory = new List<Variables>();
            Conn = new Coneccion();
            bool istru = true;

            string consulta = "SELECT B.ID,B.CATNAME,B.CATDESCR,A.ID FROM COT_MASIGFAMCAT A, COT_MTABCAT B " +
                "WHERE A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND A.IDFAM=@IDFAM AND A.IDCAT=B.ID";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", istru);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDFAM", IDFAM);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drRAN in TablaDetalle.Rows)
                {
                    Variables variable;
                    
                        variable = new Variables
                        {
                            ID = drRAN[0].ToString(),
                            _Name = drRAN[1].ToString(),
                            _Descrition = drRAN[2].ToString(),
                            _IDAsign = drRAN[3].ToString(),


                        };
                    
                    ListCompCategory.Add(variable);

                }
            }

        }

        public void GetListModelCategory(string IDFAM)
        {

            ListCompCategory = new List<Variables>();
            Conn = new Coneccion();
            bool istru = true;

            string consulta = "SELECT B.ID,B.CATNAME,B.CATDESCR,A.ID FROM COT_PASIGFAMCAT A, COT_PTABCAT B " +
                "WHERE A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND A.IDFAM=@IDFAM AND A.IDCAT=B.ID";


            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", istru);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDFAM", IDFAM);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow drRAN in TablaDetalle.Rows)
                {
                    Variables variable;

                    variable = new Variables
                    {
                        ID = drRAN[0].ToString(),
                        _Name = drRAN[1].ToString(),
                        _Descrition = drRAN[2].ToString(),
                        _IDAsign = drRAN[3].ToString(),


                    };

                    ListCompCategory.Add(variable);

                }
            }

        }
    }

    public class GetListProcesos
    {
        public List<ProcesosClass._Proceso> ListaProcesos { get; set; }

        Coneccion Conn;

        public GetListProcesos()
        {
            Conn = new Coneccion();
            bool Estado = true;

            ListaProcesos = new List<ProcesosClass._Proceso>();

            string Select = "SELECT TOP 30 * FROM COT_MPROCESOS WHERE ESTADO=@ESTADO ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    ProcesosClass._Proceso proceso = new ProcesosClass._Proceso {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        ID_Magmed = dr["ID_MAGMED"].ToString(),
                        F_Update  = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Created = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        Costo_Unit = Convert.ToDouble(dr["COSTO_UNIT"].ToString()),
                        Merma = Convert.ToDouble(dr["MERMA"].ToString()),
                        Estado = Convert.ToBoolean(dr["ESTADO"].ToString()),
                        TokenId = dr["TokenId"].ToString(),
                        _CodAlfak = dr["COD_ALFAK"].ToString(),

                    };

                    
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        
                        proceso._Magnitud = Mag._Magnitud.MAGNITUD;
                        proceso._MagSimbolo = Mag._Magnitud.SIMBOLO;
                        proceso._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                    }
                    ListaProcesos.Add(proceso);

                }
            }

        }

        public GetListProcesos(string StringSearch)
        {

            string variable = "%" + StringSearch + "%";

            Conn = new Coneccion();
            bool Estado = true;

            ListaProcesos = new List<ProcesosClass._Proceso>();

            string Select = "SELECT * FROM COT_MPROCESOS WHERE (NOMBRE LIKE @VARIABLE or DESCRIPCION LIKE @VARIABLE) AND  ESTADO=@ESTADO ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);
                    adaptador.SelectCommand.Parameters.AddWithValue("@VARIABLE", variable);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    ProcesosClass._Proceso proceso = new ProcesosClass._Proceso
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        ID_Magmed = dr["ID_MAGMED"].ToString(),
                        F_Update = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Created = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        Costo_Unit = Convert.ToDouble(dr["COSTO_UNIT"].ToString()),
                        Merma = Convert.ToDouble(dr["MERMA"].ToString()),
                        Estado = Convert.ToBoolean(dr["ESTADO"].ToString()),
                        TokenId = dr["TokenId"].ToString(),
                        _CodAlfak = dr["COD_ALFAK"].ToString(),

                    };


                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {

                        proceso._Magnitud = Mag._Magnitud.MAGNITUD;
                        proceso._MagSimbolo = Mag._Magnitud.SIMBOLO;
                        proceso._Unidad_Medida = Mag._Magnitud.UNIDAD_MEDIDA;
                    }
                    ListaProcesos.Add(proceso);

                }
            }


        }
    }

    public class _GetListComponentes
    {
        public List<_Componente> ListComponentes { get; set; }


        Coneccion Conn;


        public _GetListComponentes()
        {

            Conn = new Coneccion();
            bool Estado = true;

            ListComponentes = new List<_Componente>();

            string Select = "SELECT TOP 30 * FROM COT_MCOMPONENTES WHERE ESTADO=@ESTADO ORDER BY F_UPDATE DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    _Componente Item = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"].ToString()),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        COD_ALFAK = dr["COD_ALFAK"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasVarline)
                    {
                        Item.ID_Familia = Line.ID_Familia;
                        Item.Familia = Line.Familia;
                        Item.ID_Categoria = Line.ID_Categoria;
                        Item.Categoria = Line.Categoria;
                        Item.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();

                    }
                    if (Line.HasMarca)
                    {
                        Item.ID_Marca = Line._ID_MARCA;
                        Item.Marca = Line._Nombre_Marca;
                    }
                    if (Line.HasColor)
                    {
                        Item.ID_Color = Line.ID_Color;
                        Item.Color = Line.Color;

                    }
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        Item.ID_Magnitud = Mag._Magnitud._ID;
                        Item.Magnitud = Mag._Magnitud.MAGNITUD;
                        Item.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                        Item.UnidadMedida = Mag._Magnitud.UNIDAD_MEDIDA;
                    }
                    ListComponentes.Add(Item);

                }
            }

        }

        public _GetListComponentes(int IDFAM)
        {
            Conn = new Coneccion();
            bool Estado = true;
            ListComponentes = new List<_Componente>();

            string Select = "SELECT A.* FROM COT_MCOMPONENTES A, COT_MASIGFAMCAT B WHERE A.ID_MASIGFAMCAT=B.ID AND  A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND B.IDFAM=@IDFAM ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDFAM", IDFAM);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    _Componente Item = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"].ToString()),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        COD_ALFAK= dr["COD_ALFAK"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasVarline)
                    {
                        Item.ID_Familia = Line.ID_Familia;
                        Item.Familia = Line.Familia;
                        Item.ID_Categoria = Line.ID_Categoria;
                        Item.Categoria = Line.Categoria;
                        Item.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();

                    }
                    if (Line.HasMarca)
                    {
                        Item.ID_Marca = Line._ID_MARCA;
                        Item.Marca = Line._Nombre_Marca;
                    }
                    if (Line.HasColor)
                    {
                        Item.ID_Color = Line.ID_Color;
                        Item.Color = Line.Color;

                    }
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        Item.ID_Magnitud = Mag._Magnitud._ID;
                        Item.Magnitud = Mag._Magnitud.MAGNITUD;
                        Item.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                    }
                    ListComponentes.Add(Item);

                }
            }

        }

        public _GetListComponentes(int IDFAM, int IDCAT)
        {
            Conn = new Coneccion();
            bool Estado = true;
            ListComponentes = new List<_Componente>();

            string Select = "SELECT A.* FROM COT_MCOMPONENTES A, COT_MASIGFAMCAT B WHERE A.ID_MASIGFAMCAT=B.ID AND  A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND B.IDFAM=@IDFAM AND B.IDCAT=@IDCAT ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDFAM", IDFAM);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDCAT", IDCAT);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    _Componente Item = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"].ToString()),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        COD_ALFAK= dr["COD_ALFAK"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasVarline)
                    {
                        Item.ID_Familia = Line.ID_Familia;
                        Item.Familia = Line.Familia;
                        Item.ID_Categoria = Line.ID_Categoria;
                        Item.Categoria = Line.Categoria;
                        Item.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();

                    }
                    if (Line.HasMarca)
                    {
                        Item.ID_Marca = Line._ID_MARCA;
                        Item.Marca = Line._Nombre_Marca;
                    }
                    if (Line.HasColor)
                    {
                        Item.ID_Color = Line.ID_Color;
                        Item.Color = Line.Color;

                    }
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        Item.ID_Magnitud = Mag._Magnitud._ID;
                        Item.Magnitud = Mag._Magnitud.MAGNITUD;
                        Item.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                    }
                    ListComponentes.Add(Item);

                }
            }

        }

        public _GetListComponentes(string StringSearch)
        {
            string variable = "%" + StringSearch + "%";

            Conn = new Coneccion();
            bool Estado = true;

            ListComponentes = new List<_Componente>();

            string Select = "SELECT * FROM COT_MCOMPONENTES WHERE (NOMBRE LIKE @VARIABLE or DESCRIPCION LIKE @VARIABLE) AND  ESTADO=@ESTADO ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);
                    adaptador.SelectCommand.Parameters.AddWithValue("@VARIABLE", variable);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    _Componente Item = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"].ToString()),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        COD_ALFAK = dr["COD_ALFAK"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasVarline)
                    {
                        Item.ID_Familia = Line.ID_Familia;
                        Item.Familia = Line.Familia;
                        Item.ID_Categoria = Line.ID_Categoria;
                        Item.Categoria = Line.Categoria;
                        Item.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();

                    }
                    if (Line.HasMarca)
                    {
                        Item.ID_Marca = Line._ID_MARCA;
                        Item.Marca = Line._Nombre_Marca;
                    }
                    if (Line.HasColor)
                    {
                        Item.ID_Color = Line.ID_Color;
                        Item.Color = Line.Color;

                    }
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        Item.ID_Magnitud = Mag._Magnitud._ID;
                        Item.Magnitud = Mag._Magnitud.MAGNITUD;
                        Item.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                    }

                    ListComponentes.Add(Item);
                }
            }

        }

        public _GetListComponentes(string KA_WGR, bool ESTADO)
        {
            Conn = new Coneccion();
            
            ListComponentes = new List<_Componente>();

            string Select = "SELECT A.* FROM COT_MCOMPONENTES A WHERE  A.ESTADO=@ESTADO AND KA_WGR=@KA_WGR ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@KA_WGR", KA_WGR);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    _Componente Item = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"].ToString()),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        COD_ALFAK = dr["COD_ALFAK"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasVarline)
                    {
                        Item.ID_Familia = Line.ID_Familia;
                        Item.Familia = Line.Familia;
                        Item.ID_Categoria = Line.ID_Categoria;
                        Item.Categoria = Line.Categoria;
                        Item.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();

                    }
                    if (Line.HasMarca)
                    {
                        Item.ID_Marca = Line._ID_MARCA;
                        Item.Marca = Line._Nombre_Marca;
                    }
                    if (Line.HasColor)
                    {
                        Item.ID_Color = Line.ID_Color;
                        Item.Color = Line.Color;

                    }
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        Item.ID_Magnitud = Mag._Magnitud._ID;
                        Item.Magnitud = Mag._Magnitud.MAGNITUD;
                        Item.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                    }
                    ListComponentes.Add(Item);

                }
            }

        }

        public _GetListComponentes(string StringSearch,  string KA_WGR, bool ESTADO)
        {
            Conn = new Coneccion();
            string search = "%" + StringSearch + "%";
            ListComponentes = new List<_Componente>();

            string Select = "SELECT A.* FROM COT_MCOMPONENTES A WHERE  A.ESTADO=@ESTADO AND KA_WGR=@KA_WGR AND NOMBRE LIKE @SEARCH ORDER BY F_CREATED DESC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", ESTADO);
                    adaptador.SelectCommand.Parameters.AddWithValue("@KA_WGR", KA_WGR);
                    adaptador.SelectCommand.Parameters.AddWithValue("@SEARCH", search);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    _Componente Item = new _Componente
                    {
                        _ID = dr["ID"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        Descripcion = dr["DESCRIPCION"].ToString(),
                        PrecioUn = Convert.ToDouble(dr["PRECIO_UNIT"].ToString()),
                        CantEmb = Convert.ToDouble(dr["CANT_UNIT"].ToString()),
                        F_Actualizacion = Convert.ToDateTime(dr["F_UPDATE"].ToString()),
                        F_Creacion = Convert.ToDateTime(dr["F_CREATED"].ToString()),
                        HasProc = Convert.ToBoolean(dr["HASPROC"].ToString()),
                        Path_Photo = dr["IMG_PATH"].ToString(),
                        TokeId = dr["TokenId"].ToString(),
                        COD_ALFAK = dr["COD_ALFAK"].ToString(),
                    };
                    GetVarLine Line = new GetVarLine(dr["ID_MTABMA"].ToString(), dr["ID_MTABCOL"].ToString(), dr["ID_MASIGFAMCAT"].ToString());
                    if (Line.HasVarline)
                    {
                        Item.ID_Familia = Line.ID_Familia;
                        Item.Familia = Line.Familia;
                        Item.ID_Categoria = Line.ID_Categoria;
                        Item.Categoria = Line.Categoria;
                        Item.ID_MASIGFAMCAT = dr["ID_MASIGFAMCAT"].ToString();

                    }
                    if (Line.HasMarca)
                    {
                        Item.ID_Marca = Line._ID_MARCA;
                        Item.Marca = Line._Nombre_Marca;
                    }
                    if (Line.HasColor)
                    {
                        Item.ID_Color = Line.ID_Color;
                        Item.Color = Line.Color;

                    }
                    GetMagnitudes Mag = new GetMagnitudes();
                    bool HasMag = Mag.IsMagnitud(dr["ID_MAGMED"].ToString());
                    if (HasMag)
                    {
                        Item.ID_Magnitud = Mag._Magnitud._ID;
                        Item.Magnitud = Mag._Magnitud.MAGNITUD;
                        Item.UnMedSimbolo = Mag._Magnitud.SIMBOLO;
                    }
                    ListComponentes.Add(Item);

                }
            }

        }


    }

    public class GetMagnitudes
    {
        public List<Magnitud> AllMagnitudes { get; set; }

        public Magnitud _Magnitud { get; set; }

        Coneccion Conn;
        static SqlDataReader dr;

        public GetMagnitudes()
        {

            Conn = new Coneccion();
            
            AllMagnitudes = new List<Magnitud>();

            string Select = "SELECT * FROM MAGNITUDES ORDER BY MAGNITUD ASC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    Magnitud Item = new Magnitud {
                        _ID = dr["ID"].ToString(),
                        MAGNITUD=dr["MAGNITUD"].ToString(),
                        UNIDAD_MEDIDA=dr["UNIDAD_MEDIDA"].ToString(),
                        SIMBOLO=dr["SIMBOLO"].ToString(),
                    };

                    AllMagnitudes.Add(Item);
                }


            }


        }

        public GetMagnitudes(string MagName)
        {
            Conn = new Coneccion();

            AllMagnitudes = new List<Magnitud>();

            string Select = "SELECT * FROM MAGNITUDES WHERE MAGNITUD=@MAGNITUD ORDER BY UNIDAD_MEDIDA ASC";

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@MAGNITUD", MagName);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                foreach (DataRow dr in TablaDetalle.Rows)
                {
                    Magnitud Item = new Magnitud
                    {
                        _ID = dr["ID"].ToString(),
                        MAGNITUD = dr["MAGNITUD"].ToString(),
                        UNIDAD_MEDIDA = dr["UNIDAD_MEDIDA"].ToString(),
                        SIMBOLO = dr["SIMBOLO"].ToString(),
                    };

                    AllMagnitudes.Add(Item);
                }


            }


        }

        public bool IsMagnitud(string ID)
        {
            bool IsMag = false;
            string Select = "SELECT * FROM MAGNITUDES WHERE ID=@ID";
            Conn = new Coneccion();
            try
            {
                Conn.ConnPlabal.Open();
                Conn.CmdPlabal = new SqlCommand(Select,Conn.ConnPlabal);
                Conn.CmdPlabal.Parameters.AddWithValue("@ID",ID);
                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    _Magnitud = new Magnitud {
                        _ID = ID,
                        MAGNITUD= dr["MAGNITUD"].ToString(),
                        UNIDAD_MEDIDA=dr["UNIDAD_MEDIDA"].ToString(),
                        SIMBOLO=dr["SIMBOLO"].ToString(),
                    };
                    IsMag = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsMag;

        }

        public List<Magnitud> Magnitudes()
        {
            List<Magnitud> lista = new List<Magnitud>();
            DataTable tabla = new DataTable();
            Conn = new Coneccion();
            string Select = "SELECT DISTINCT MAGNITUD FROM MAGNITUDES ORDER BY MAGNITUD ASC";

            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    

                    adaptador.Fill(tabla);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (tabla.Rows.Count > 0)
            {

                foreach (DataRow dr in tabla.Rows)
                {
                    Magnitud Mag = new Magnitud { MAGNITUD = dr[0].ToString() };
                    lista.Add(Mag);
                }
            }





                    return lista;
        }


        public class Magnitud
        {
            
            public string _ID { get; set; }
            public string MAGNITUD { get; set; }
            public string UNIDAD_MEDIDA { get; set; }
            public string SIMBOLO { get; set; }
        }
    }



    public class GetListNoAsignadas
    {
        public List<Variables> ListaVar;

        Coneccion Conn;
        public GetListNoAsignadas(string TypeOfasign, string IDfirst)
        {
            bool istru = true;
            Conn = new Coneccion();
            ListaVar = new List<Variables>();
            string Select = "";
            #region CONDICIONtabla
            if (TypeOfasign == "A")
            {
                Select = "SELECT * FROM COT_MTABCAT A WHERE NOT A.ID IN (SELECT ASIG.IDCAT FROM COT_MASIGFAMCAT ASIG WHERE ASIG.IDFAM=@IDfirst AND ASIG.ESTADO=@ESTADO) AND A.ESTADO=@ESTADO";
            }
            else if (TypeOfasign == "B")
            {
                Select = "SELECT * FROM COT_MTABCOL A WHERE NOT A.ID IN (SELECT ASIG.IDCOLOR FROM COT_MASIGCATCOL ASIG WHERE ASIG.IDCAT=@IDfirst AND ASIG.ESTADO=@ESTADO) AND A.ESTADO=@ESTADO";
            }
            else if (TypeOfasign == "PFAMCAT")
            {
                Select = "SELECT * FROM COT_PTABCAT A WHERE NOT A.ID IN (SELECT ASIG.IDCAT FROM COT_PASIGFAMCAT ASIG WHERE ASIG.IDFAM=@IDfirst AND ASIG.ESTADO=@ESTADO) AND A.ESTADO=@ESTADO";
            }

            else if (TypeOfasign == "F")
            {
                Select = "";
            }
            #endregion

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", istru);
                    adaptador.SelectCommand.Parameters.AddWithValue("@IDfirst", IDfirst);

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
                    Variables item = new Variables
                    {
                        ID = drRAN[0].ToString(),
                        _Name = drRAN[1].ToString(),
                        _Descrition = drRAN[2].ToString(),
                    };

                    ListaVar.Add(item);
                }
            }

        }
    }


    public class GenVarAsign
    {
        public string _ID { get; set; }
        public string _NameVarA { get; set; }
        public string _NameVarB { get; set; }
        public string _AsignDate { get; set; }
    }

    public class Variables
    {
        public string _Name { get; set; }
        public string _Descrition { get; set; }
        public string _Procedencia { get; set; }
        public string _CssClass { get; set; }
        public bool _estado { get; set; }
        public string ID { get; set; }
        public string _IDAsign { get; set; }

    }

    public class VariablesAsignadas
    {
        public string _ID { get; set; }
        public string AvarName { get; set; }
        public string BVarName { get; set; }
        public string AsignDate { get; set; }
    }

    public class _Componente
    {
        public string _ID { get; set; }
        public string ID_MASIGFAMCAT { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string ID_Marca { get; set; }
        public string Familia { get; set; }
        public string ID_Familia { get; set; }
        public string Color { get; set; }
        public string ID_Color { get; set; }
        public string Categoria { get; set; }
        public string ID_Categoria { get; set; }
        public string Magnitud { get; set; }
        public string ID_Magnitud { get; set; }
        public string UnidadMedida { get; set; }
        public string UnMedSimbolo { get; set; }
        public double CantEmb { get; set; }
        public double PrecioUn { get; set; }
        public DateTime F_Actualizacion { get; set; }
        public DateTime F_Creacion { get; set; }
        public bool HasProc { get; set; }
        public string Path_Photo { get; set; }
        public string TokeId { get; set; }
        public string COD_ALFAK { get; set; }
        public bool IsGlass { get; set; }


    }

}
