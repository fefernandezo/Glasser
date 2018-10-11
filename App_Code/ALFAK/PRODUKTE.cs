using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de PRODUKTE
/// </summary>
namespace Alfak
{
    public class FamiliasAlfak
    {

        public List<Familia> familias;

        Coneccion Conn;
        static SqlDataReader dr;


        public FamiliasAlfak()
        {
            Conn = new Coneccion();
            familias = new List<Familia>();
            
            string consulta = "select ID,BEZ from SYSADM.KA_WGR WHERE ID LIKE '%*' and ID like 'W%' and ID not like 'W**' order by ID asc";
            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnAlfak)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnAlfak);
                    
                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow row in TablaDetalle.Rows)
                {
                    Familia familia = new Familia {
                        ID = row[0].ToString(),
                        Name= row[1].ToString(),

                    };
                    familias.Add(familia);
                }
            }

            


        }

        public FamiliasAlfak(string Tipo)
        {
            Conn = new Coneccion();
            string consulta;
            if (Tipo=="Procesos")
            {
                consulta = "select ID,BEZ from PHGLASS.SYSADM.KA_WGR WHERE BEZ <>'' and ID like '%S**%' order by ID asc";
            }
            else if (Tipo=="Herrajes")
            {
                consulta = "select ID,BEZ from PHGLASS.SYSADM.KA_WGR WHERE BEZ <>'' and ID like '%T**%' order by ID asc";
            }
            else if (Tipo=="Termopanel")
            {
                consulta = "select ID,BEZ from PHGLASS.SYSADM.KA_WGR WHERE BEZ <>'' and ID like '%H**%' order by ID asc";
            }
            else if (Tipo == "Arquitectura")
            {
                consulta = "select ID,BEZ from PHGLASS.SYSADM.KA_WGR WHERE BEZ <>'' and ID like '%N**%' order by ID asc";
            }
            else
            {
                consulta = "select ID,BEZ from PHGLASS.SYSADM.KA_WGR WHERE BEZ <>'' and ID like '%**%' order by ID asc";
            }
            
            familias = new List<Familia>();

             
            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnAlfak)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnAlfak);

                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow row in TablaDetalle.Rows)
                {
                    Familia familia = new Familia
                    {
                        ID = row[0].ToString(),
                        Name = row[1].ToString(),

                    };
                    familias.Add(familia);
                }
            }

        }

        public List<Familia> GetLbl2Familia(string ID)
        {
            Conn = new Coneccion();
            string _ID = ID.Replace("*", "");
            List<Familia> Lbl2 = new List<Familia>();
            string _A = _ID + "%";
            string consulta = "select ID,BEZ from SYSADM.KA_WGR WHERE ID LIKE @ID and ID like '%*' and ID not like '%**%' order by ID asc";
            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnAlfak)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnAlfak);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID", _A);
                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow row in TablaDetalle.Rows)
                {
                    Familia familia = new Familia
                    {
                        ID = row[0].ToString(),
                        Name = row[1].ToString(),

                    };
                    Lbl2.Add(familia);
                }
            }


            return Lbl2;
        }

        public List<Familia> GetLbl3Familia(string ID)
        {
            Conn = new Coneccion();
            string _ID = ID.Replace("*", "");
            List<Familia> Lbl2 = new List<Familia>();
            string _A = "%" + _ID + "%";
            string consulta = "select ID,BEZ from PHGLASS.SYSADM.KA_WGR " +
                "WHERE BEZ <>'' and ID like @ID  and ID not like '%*%' order by ID asc";
            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnAlfak)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnAlfak);
                    adaptador.SelectCommand.Parameters.AddWithValue("@ID",_A);
                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    NewMethod(ex);
                }
            }
            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow row in TablaDetalle.Rows)
                {
                    Familia familia = new Familia
                    {
                        ID = row[0].ToString(),
                        Name = row[1].ToString(),

                    };
                    Lbl2.Add(familia);
                }
            }


            return Lbl2;
        }

        private static void NewMethod(Exception ex)
        {
            throw ex;
        }

        public class Familia
        {
            public string ID { get; set; }
            public string Name { get; set; }

        }


    }

    public class ProductoAlfak
    {
        public List<BA_PRODUKTE> Listaproductos;

        Coneccion Conn;
        static SqlDataReader dr;


        public ProductoAlfak()
        {

        }
        

        public ProductoAlfak(string CodFamilia)
        {
            Conn = new Coneccion();
            string consulta = "SELECT b.BA_BEZ1,C.BEZ AS 'FAMILIA',a.*,b.* FROM PHGLASS.SYSADM.BA_PRODUKTE a,PHGLASS.SYSADM.BA_PRODUKTE_BEZ b ," +
                " PHGLASS.SYSADM.KA_WGR C WHERE a.BA_WGR=C.ID and a.BA_PRODUKT=b.BA_PRODUKT and a.BA_WGR=@familia";

            Listaproductos = new List<BA_PRODUKTE>();

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnAlfak)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnAlfak);
                    adaptador.SelectCommand.Parameters.AddWithValue("@familia", CodFamilia);
                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow row in TablaDetalle.Rows)
                {
                    BA_PRODUKTE ITEM = new BA_PRODUKTE {
                        Descripcion = row["BA_BEZ1"].ToString(),
                        CodigoAlfak= row["BA_PRODUKT"].ToString(),
                        Abreviacion = row["BA_MCODE"].ToString(),
                        Id_familiaAlfak = row["BA_WGR"].ToString(),
                        Familia_Alfak = row["FAMILIA"].ToString(),
                        EAN = row["EAN"].ToString(),
                    };

                    Listaproductos.Add(ITEM);
                }
            }

        }

        public ProductoAlfak(string CodFamilia, string Filtro)
        {
            string Filter = "%" + Filtro + "%";
            Conn = new Coneccion();
            string consulta = "SELECT b.BA_BEZ1,C.BEZ AS 'FAMILIA',a.*,b.* FROM PHGLASS.SYSADM.BA_PRODUKTE a,PHGLASS.SYSADM.BA_PRODUKTE_BEZ b ," +
                " PHGLASS.SYSADM.KA_WGR C WHERE a.BA_WGR=C.ID and a.BA_PRODUKT=b.BA_PRODUKT and a.BA_WGR=@familia and (b.BA_BEZ1 LIKE @filter OR a.EAN LIKE @filter OR a.BA_PRODUKT LIKE @filter )";

            Listaproductos = new List<BA_PRODUKTE>();

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnAlfak)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(consulta, Conn.ConnAlfak);
                    adaptador.SelectCommand.Parameters.AddWithValue("@familia", CodFamilia);
                    adaptador.SelectCommand.Parameters.AddWithValue("@filter", Filter);
                    adaptador.Fill(TablaDetalle);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (TablaDetalle.Rows.Count > 0)
            {

                foreach (DataRow row in TablaDetalle.Rows)
                {
                    BA_PRODUKTE ITEM = new BA_PRODUKTE
                    {
                        Descripcion = row["BA_BEZ1"].ToString(),
                        CodigoAlfak = row["BA_PRODUKT"].ToString(),
                        Abreviacion = row["BA_MCODE"].ToString(),
                        Id_familiaAlfak = row["BA_WGR"].ToString(),
                        Familia_Alfak = row["FAMILIA"].ToString(),
                        EAN = row["EAN"].ToString(),
                    };

                    Listaproductos.Add(ITEM);
                }
            }

        }

        public BA_PRODUKTE GetByCod(string CodigoAlfak)
        {
            BA_PRODUKTE Item;

            Conn = new Coneccion();
            string Select = "SELECT  A.EXT_STAT AS 'STAT',A.EAN,A.BA_SN_MAKRO_NAME,A.BA_MASS_GEWICHT,A.BA_MASS_DICKE,ART.PRD_NR AS 'ARTPRDNR', GRP.PRDKTGRP_NR AS 'PRDKTGRPNR' , B.BA_MENGENEINH,B.BA_BEZ3, B.BA_BEZ1,C.BEZ AS 'FAMILIA',A.*,B.* " +
                "FROM PHGLASS.SYSADM.BA_PRODUKTE A, PHGLASS.SYSADM.BA_PRODUKTE_BEZ B, SYSADM.KA_WGR C, SYSADM.KA_PRODUKTART ART, SYSADM.KA_PRODUKTGRP GRP " +
                "WHERE A.BA_WGR = C.ID AND A.BA_PRODUKT = B.BA_PRODUKT and A.BA_PRODUKT=@CodAlfak AND A.BA_PRODUKTART = ART.BEZ AND ART.BEZ = GRP.PRDKTART_ID AND A.BA_PRODUKTGRP = GRP.PRDKTGRP_ID";
            try
            {
                Conn.ConnAlfak.Open();
                Conn.Cmd = new SqlCommand(Select,Conn.ConnAlfak);
                Conn.Cmd.Parameters.AddWithValue("@CodAlfak", CodigoAlfak);
                dr = Conn.Cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores VAL = new Validadores();
                    Item = new BA_PRODUKTE {
                        Descripcion = dr["BA_BEZ1"].ToString(),
                        CodigoAlfak = dr["BA_PRODUKT"].ToString(),
                        Abreviacion = dr["BA_MCODE"].ToString(),
                        Id_familiaAlfak = dr["BA_WGR"].ToString(),
                        Familia_Alfak = dr["FAMILIA"].ToString(),
                        BA_BEZ3 = dr["BA_BEZ3"].ToString(),
                        BA_MENGENEINH = dr["BA_MENGENEINH"].ToString(),
                        ARTPRDNR = dr["ARTPRDNR"].ToString(),
                        PRDKTGRPNR = dr["PRDKTGRPNR"].ToString(),
                        BA_MASS_DICKE= VAL.ParseoDouble(dr["BA_MASS_DICKE"].ToString()),
                        BA_MASS_GEWICHT=VAL.ParseoDouble(dr["BA_MASS_GEWICHT"].ToString()),
                        BA_SN_MAKRO_NAME = dr["BA_SN_MAKRO_NAME"].ToString(),
                        EAN=dr["EAN"].ToString(),
                        EXT_STAT=dr["STAT"].ToString(),

                    };
                }
                else
                {
                    Item = new BA_PRODUKTE { };
                }
                dr.Close();
                Conn.ConnAlfak.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }



            return Item;
        }


        public class Get
        {
            public readonly BA_PRODUKTE BA_PRODUKTE;
            public readonly List<Costo.Variables> Costos;

            public Get(string _BA_PRODUKT)
            {
                ProductoAlfak pr = new ProductoAlfak();
                BA_PRODUKTE = pr.GetByCod(_BA_PRODUKT);
                Costo Cst = new Costo(_BA_PRODUKT, "3", "5");
                Costos = Cst.Lista;
            }
        }

        public class Costo
        {
            #region Manejo de datos
            Coneccion Conn;
            private string Query;
            private SqlDataReader dr;
            #endregion


            public List<Variables> Lista;

            public readonly bool HasCosto;

            public Costo(string codigo, string LISTA, string SCHLS)
            {
                HasCosto = OBTENER(codigo, LISTA, SCHLS);
            }

            private bool OBTENER(string codigo, string LISTA, string SCHLS)
            {
                Conn = new Coneccion();

                Lista = new List<Variables>();

                Query = "SELECT * FROM SYSADM.PR_PREIS WHERE LISTE_ID=@LISTA AND SCHLS_ID=@SCHLS AND ID=@CODIGO";


                DataTable TablaDetalle = new DataTable();
                using (Conn.ConnAlfak)
                {
                    try
                    {
                        SqlDataAdapter adaptador = new SqlDataAdapter(Query, Conn.ConnAlfak);
                        adaptador.SelectCommand.Parameters.AddWithValue("@CODIGO", codigo);
                        adaptador.SelectCommand.Parameters.AddWithValue("@LISTA", LISTA);
                        adaptador.SelectCommand.Parameters.AddWithValue("@SCHLS", SCHLS);

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
                        Variables vAR = new Variables {
                            ID= codigo,
                            Liste_ID=LISTA,
                            SCHLS_ID=SCHLS,
                            PREIS_TYP=drR["PREIS_TYP"].ToString(),
                            GRENZE1=Convert.ToDouble(drR["GRENZE1"].ToString()),
                            GRENZE2 = Convert.ToDouble(drR["GRENZE2"].ToString()),
                            GRENZE3 = Convert.ToDouble(drR["GRENZE3"].ToString()),
                            PREIS = Convert.ToDouble(drR["PREIS"].ToString()),
                        };
                        
                        Lista.Add(vAR);

                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public class Variables
            {
                public string ID { get; set; }
                public string Liste_ID { get; set; }
                public string SCHLS_ID { get; set; }
                public string PREIS_TYP { get; set; }
                public double GRENZE1 { get; set; }
                public double GRENZE2 { get; set; }
                public double GRENZE3 { get; set; }
                public double PREIS { get; set; }
            }
        }

        public class BA_PRODUKTE
        {
            public string Descripcion { get; set; }
            public string CodigoAlfak { get; set; }
            public string Abreviacion { get; set; }
            public string Id_familiaAlfak { get; set; }
            public string Familia_Alfak { get; set; }
            public string EAN { get; set; }
            /*descripcion del proceso*/
            public string BA_BEZ3 { get; set; }
            /*unidad de medida m2,Pza,etc*/
            public string BA_MENGENEINH { get; set; }
            /*nro del tipo de producto*/
            public string ARTPRDNR { get; set; }
            /*nro del grupo del tipo de producto*/
            public string PRDKTGRPNR { get; set; }
            /*ESPESOR*/
            public double BA_MASS_DICKE { get; set; }
            /*PESO EN KG/M2*/
            public double BA_MASS_GEWICHT { get; set; }
            /*ARCHIVO SN si es que tiene makro*/
            public string BA_SN_MAKRO_NAME { get; set; }
            /*TIPO COLOR*/
            public string EXT_STAT { get; set; }



        }
    }



    
}