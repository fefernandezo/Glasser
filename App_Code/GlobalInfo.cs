using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de GenericInfo
/// </summary>
/// 
namespace GlobalInfo
{
    public class Fechas
    {
        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
        SqlConnection ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
        SqlCommand cmdPlabal;
        SqlCommand cmdPlabal2;
        SqlCommand cmdAlfak;
        static SqlDataReader drPlabal;
        static SqlDataReader drAlfak;
        static SqlDataReader drPlabal2;

        public DateTime FechaHabilAnterior( DateTime Fecha, int CantDiasHabiles)
        {
            DateTime Retorno;

            
            int cont = 0;
            for (int i = 1; i <= CantDiasHabiles; i++)
            {
                DateTime FECHA = Fecha.AddDays(-i);
               
                if(FECHA.DayOfWeek==DayOfWeek.Saturday)
                {
                    CantDiasHabiles++;
                }
                else if (FECHA.DayOfWeek==DayOfWeek.Sunday)
                {
                    CantDiasHabiles++;
                }
                else if(Esferiado(FECHA))
                {
                    CantDiasHabiles++;
                }

                cont = i;

            }
            Retorno = Fecha.AddDays(-cont);


            return Retorno;
        }

        //valida si la fecha ingresada es feriado
        public bool Esferiado(DateTime fecha)
        {
            bool si;
            SqlConnection conx;
            string _Fecha = fecha.ToShortDateString();
            string Select = @"Select * FROM PLABAL.dbo.Feriados WHERE Fecha = @Fecha";
            conx = new SqlConnection(ConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);


            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@Fecha", fecha);

            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                si = true;


            }
            else
            {
                si = false;
            }
            drPlabal.Close();
            ConnPlabal.Close();

            return si;
        }
    }
    
    
}
