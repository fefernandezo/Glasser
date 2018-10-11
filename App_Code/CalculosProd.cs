using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;




    public class CalculosProd
    {
        //conecciones a las DB
        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
        SqlCommand cmdPlabal;
        static SqlDataReader drPlabal;



        public double PrecioProd(string CodProd, double ancho, double largo)
        {
            double Preciototal = 0;
            double areadecimal = (ancho / 1000) * (largo / 1000);
            double perimetro = ((ancho / 1000)  + (largo / 1000)) * 2;
            double Merma = 0;

            double area = Convert.ToDouble(areadecimal);
            DataTable Detalle = new DataTable();
            string consulta = "Select  comp.unidad_medida_comp,comp.precio_componente FROM PLABAL.dbo.e_componente comp, PLABAL.dbo.e_asigncomp asign where asign.Id_componente=comp.id_componente and asign.Cod_prod='" + CodProd + "'";
            ConnPlabal.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(consulta, ConnPlabal);
            adapter.Fill(Detalle);
            ConnPlabal.Close();


            foreach (DataRow row in Detalle.Rows)
            {
                Merma = 0;
                string tipo = row[0].ToString().Trim();
                double preciocomp = 0;

                //calculo de la merma
                if (tipo == "mt2")
                {
                    preciocomp = (Convert.ToDouble(row[1].ToString()) * areadecimal);
                    Merma = (-6 * Math.Pow(10, -5) * Math.Pow(area, 6) + 0.0016 * Math.Pow(area, 5) - 0.0157 * Math.Pow(area, 4) + 0.0771 * Math.Pow(area, 3) - 0.2299 * Math.Pow(area, 2) + 0.4265 * area + 0.2317) * 0.3 * preciocomp;

                }
                else if (tipo == "ml")
                {
                    preciocomp = (Convert.ToDouble(row[1].ToString()) * perimetro);
                    Merma = preciocomp * 0.10;

                }
                Preciototal = Preciototal + preciocomp + Merma;
            }


            return Preciototal;
        }

        public double PrecioProcesoDVH(string proceso, double m2, double ml, double kg)
        {
            double PrecioProc = 0;
            double Merma;
            double precio = 0;


            ConnPlabal.Open();

            string consulta = "SELECT unidad_medida_proceso, CONVERT(INT,precio), merma FROM e_Proceso WHERE nombre_proceso = @nombreProceso";
            cmdPlabal = new SqlCommand(consulta, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@nombreProceso", proceso);
            drPlabal = cmdPlabal.ExecuteReader();

            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                string unidadmed = Convert.ToString(drPlabal[0]).Trim();
                Merma = Convert.ToDouble(drPlabal[2]) / 100;

                if (unidadmed.Equals("mt2"))
                {

                    double totalMt2 = Convert.ToDouble(drPlabal[1].ToString()) * m2;
                    precio = Convert.ToInt32(totalMt2);

                }
                if (unidadmed.Equals("ml"))
                {

                    double totalMl = Convert.ToDouble(drPlabal[1].ToString()) * ml;
                    precio = Convert.ToInt32(totalMl);
                }
                if (unidadmed.Equals("kg"))
                {

                    double totaKg = Convert.ToDouble(drPlabal[1].ToString()) * kg;
                    precio = Convert.ToInt32(totaKg);
                }


                PrecioProc = precio * (1 + Merma);

            }

            drPlabal.Close();
            ConnPlabal.Close();

            return PrecioProc;
        }

        public double CostoDespacho(string nomDespacho, double kg)
        {
            double costoDespacho = 0;
            double total = 0;

            string consulta;

            consulta = @"SELECT CONVERT(INT,precio_componente) FROM e_componente WHERE nombre_componente = @nom";
            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(consulta, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@nom", nomDespacho);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();
            if (drPlabal.HasRows)
            {
                costoDespacho = Convert.ToDouble(drPlabal[0].ToString());
            }

            drPlabal.Close();

            ConnPlabal.Close();

            total = costoDespacho * kg;

            return total;
        }

        public UnidadesProd Calculos(double ancho, double alto, int espesor1, int espesor2)
        {


        UnidadesProd Valores = new UnidadesProd
        {
            MetroCuad = Math.Round(((ancho / 1000) * (alto / 1000)), 2),
            MetroLi = ((ancho / 1000) + (alto / 1000)) * 2,
            Kilos = Math.Round((espesor1 + espesor2) * (Math.Round(((ancho / 1000) * (alto / 1000)), 2) * 5 / 2), 0),
    };
        
       
        
            return Valores;

        }


    }

    public class UnidadesProd
    {
        public double Kilos { get; set; }
        public double MetroCuad { get; set; }
        public double MetroLi { get; set; }
    }


