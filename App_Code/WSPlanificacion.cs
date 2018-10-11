using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using PlanificacionOT;

/// <summary>
/// Descripción breve de WSPlanificacion
/// </summary>
[WebService(Namespace = "http://www.phglass.cl/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]

[System.Web.Script.Services.ScriptService]
public class WSPlanificacion : System.Web.Services.WebService
{
    SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
    SqlConnection ConnRANDOM = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlConnection ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlConnection ConnRANDOM2 = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
    SqlCommand cmdPlabal;
    SqlCommand cmdGlasser;
    static SqlDataReader drPlabal;
    static SqlDataReader drGlasser;

    public WSPlanificacion()
    {

        //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<PLA_OTINGList> StatusOT( string NUMOT)
    {
        List<PLA_OTINGList> resultado = new List<PLA_OTINGList>();

        string consulta = @"SELECT * FROM PLA_OTING WHERE NUMOT=@NUMOT";
        DataTable TablaDetalle = new DataTable();
        using (ConnPlabal)
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, ConnPlabal);
                adaptador.SelectCommand.Parameters.AddWithValue("@NUMOT", NUMOT);

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

                PLA_OTINGList pLA_OTING = new PLA_OTINGList {
                    _MsgStatus=drRAN["MsgStatus"].ToString(),
                    _Date=Convert.ToDateTime(drRAN["Date"].ToString()),
                    _NUMOT=NUMOT,
                    _Status=Convert.ToInt32(drRAN["Status"].ToString()),
                    
                };

                resultado.Add(pLA_OTING);

            }
        }

        return resultado;
    }

}
