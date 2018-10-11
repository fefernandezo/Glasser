using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using Comercial;
using GlobalInfo;
using Comercial;

/// <summary>
/// Descripción breve de WSPlanificacion
/// </summary>
[WebService(Namespace = "http://www.phglass.cl/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
// Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
[System.Web.Script.Services.ScriptService]
public class WSComercial : System.Web.Services.WebService
{
    Coneccion Conn;
   
    [WebMethod]
    public List<GenVarAsign> GetListVarAsignadas(string TypeOfasign, string IDfirst)
    {
        List<GenVarAsign> Lista = new List<GenVarAsign>();
        bool istru = true;
        Conn = new Coneccion();
        string Select = "";
        #region CONDICIONtabla
        if (TypeOfasign == "A")
        {
            Select = "SELECT A.ID,B.FAMNAME, C.CATNAME, CONVERT(VARCHAR,A.ASIGNDATE,105) FROM COT_MASIGFAMCAT A, COT_MTABFAM B, COT_MTABCAT C WHERE A.IDFAM=B.ID AND A.IDCAT=C.ID AND A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND C.ESTADO=@ESTADO  AND B.ID=@IDfirst";
        }
        else if (TypeOfasign == "B")
        {
            Select = "SELECT A.ID, C.CATNAME, B.COLORNAME, CONVERT(VARCHAR,A.ASIGNDATE,105) FROM COT_MASIGCATCOL A, COT_MTABCOL B, COT_MTABCAT C WHERE A.IDCOLOR=B.ID AND A.IDCAT=C.ID AND A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND C.ESTADO=@ESTADO AND A.IDCAT=@IDfirst";
        }
        else if (TypeOfasign == "PFAMCAT")
        {
            Select = "SELECT A.ID,B.FAMNAME, C.CATNAME, CONVERT(VARCHAR,A.ASIGNDATE,105) FROM COT_PASIGFAMCAT A, COT_PTABFAM B, COT_PTABCAT C WHERE A.IDFAM=B.ID AND A.IDCAT=C.ID AND A.ESTADO=@ESTADO AND B.ESTADO=@ESTADO AND C.ESTADO=@ESTADO  AND B.ID=@IDfirst";
        }
        else if (TypeOfasign == "D")
        {
            Select = "";
        }
        else if (TypeOfasign == "E")
        {
            Select = "";
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
                GenVarAsign item = new GenVarAsign
                {
                    _ID = drRAN[0].ToString(),
                    _NameVarA=drRAN[1].ToString(),
                    _NameVarB=drRAN[2].ToString(),
                    _AsignDate=drRAN[3].ToString(),
                };

                Lista.Add(item);
            }
        }

        return Lista;

    }


    [WebMethod]
    public List<Variables> GetListVariables (int TypeofVar)
    {
       List<Variables> Listavariables = new List<Variables>();
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
        else if (TypeofVar==7)
        {
            consulta = @"SELECT * FROM COT_PTABCANALES WHERE ESTADO=@Estado";
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

        return Listavariables;

    }

    [WebMethod]
    public List<_Componente> GetComponenteDetail (string ID)
    {
        List<_Componente> Componentes = new List<_Componente>();
        

        Conn = new Coneccion();
        bool Estado = true;

        

        string Select = "SELECT * FROM COT_MCOMPONENTES WHERE ESTADO=1 AND ID=@ID";

        DataTable TablaDetalle = new DataTable();
        using (Conn.ConnPlabal)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(Select, Conn.ConnPlabal);
                adaptador.SelectCommand.Parameters.AddWithValue("@ESTADO", Estado);
                adaptador.SelectCommand.Parameters.AddWithValue("@ID", ID);

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
                Componentes.Add(Item);

            }
        }




        return Componentes;
    }

   

}
