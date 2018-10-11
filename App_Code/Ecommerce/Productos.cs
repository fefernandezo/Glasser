using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


/// <summary>
/// Descripción breve de Productos
/// </summary>
/// 
namespace Ecommerce
{
    public class ListasComponentesDVH
    {
        public List<Cristales> Cristales;
        public List<Separadores> Separadores;


        Coneccion conect;
        static SqlDataReader dr;


        public ListasComponentesDVH()
        {

            #region SQLString
            string Select = "SELECT * FROM e_componente WHERE FAMILIA='Cristal' ORDER BY espesor,DESCRIPCION DESC";
            string Selectsep = "SELECT * FROM e_componente WHERE FAMILIA='Separador' ORDER BY espesor,DESCRIPCION DESC";
            #endregion

            DataTable TablaDetalle2 = new DataTable();
            conect = new Coneccion();
            using (conect.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(Select, conect.ConnPlabal);
                    

                    adaptador2.Fill(TablaDetalle2);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message, ex);
                }

                if (TablaDetalle2.Rows.Count > 0)
                {
                    Cristales = new List<Cristales>();
                    
                    Cristales item;
                    foreach (DataRow dr in TablaDetalle2.Rows)
                    {

                        item = new Cristales {
                            _ID=dr["id_componente"].ToString(),
                            _Abreviacion= dr["nombre_componente"].ToString(),
                            _UnidadMedida=dr["unidad_medida_comp"].ToString(),
                            _Precio=Convert.ToDouble(dr["precio_componente"].ToString()),
                            _CodEstructuraAlfak=dr["codigo_estruc_alfak"].ToString(),
                            _Descripcion=dr["DESCRIPCION"].ToString(),
                            _Espesor=Convert.ToDouble(dr["espesor"].ToString()),
                            _CssClass = dr["CssClass"].ToString(),
                            _CodigoAlfak = dr["codigo_compo"].ToString(),
                        };
                        Cristales.Add(item);

                    }


                }
                else
                {
                    
                }
            }


            DataTable Tabla = new DataTable();
            conect = new Coneccion();
            using (conect.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador2 = new SqlDataAdapter(Selectsep, conect.ConnPlabal);


                    adaptador2.Fill(Tabla);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message, ex);
                }

                if (Tabla.Rows.Count > 0)
                {
                    Separadores = new List<Separadores>();
                    Separadores items;
                    foreach (DataRow dr in Tabla.Rows)
                    {

                        items = new Separadores
                        {
                            _ID = dr["id_componente"].ToString(),
                            _Abreviacion = dr["nombre_componente"].ToString(),
                            _UnidadMedida = dr["unidad_medida_comp"].ToString(),
                            _Precio = Convert.ToDouble(dr["precio_componente"].ToString()),
                            _CodEstructuraAlfak = dr["codigo_estruc_alfak"].ToString(),
                            _Descripcion = dr["DESCRIPCION"].ToString(),
                            _Espesor = Convert.ToDouble(dr["espesor"].ToString()),
                            _CssClass= dr["CssClass"].ToString(),
                            _CodigoAlfak = dr["codigo_compo"].ToString(),

                        };
                        Separadores.Add(items);

                    }


                }
                else
                {

                }
            }

        }

        
    }

    public class GetCssClassComponente
    {
        public string CssClass { get; set; }

        Coneccion conect;
        static SqlDataReader dr;

        public GetCssClassComponente(string IDComponete)
        {

            conect = new Coneccion();
            string Select = "SELECT TOP 1 * FROM e_componente WHERE id_componente=@IdComponente";

            conect.ConnPlabal.Open();
            conect.CmdPlabal = new SqlCommand(Select,conect.ConnPlabal);
            conect.CmdPlabal.Parameters.AddWithValue("@IdComponente", IDComponete);
            dr = conect.CmdPlabal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                CssClass = dr["CssClass"].ToString();
            }
            else
            {
                CssClass = "";
            }
            dr.Close();
            conect.ConnPlabal.Close();
                

        }
    }


    public class Cristales
    {
        public string _ID { get; set; }
        public string _Descripcion { get; set; }
        public double _Espesor { get; set; }
        public double _Precio { get; set; }
        public string _UnidadMedida { get; set; }
        public string _Abreviacion { get; set; }
        public string _CodEstructuraAlfak { get; set; }
        public string _CssClass { get; set; }
        public string _CodigoAlfak { get; set; }

    }
    public class Separadores
    {
        public string _ID { get; set; }
        public string _Descripcion { get; set; }
        public double _Espesor { get; set; }
        public double _Precio { get; set; }
        public string _UnidadMedida { get; set; }
        public string _Abreviacion { get; set; }
        public string _CodEstructuraAlfak { get; set; }
        public string _CssClass { get; set; }
        public string _CodigoAlfak { get; set; }

    }

}
