using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;

/// <summary>
/// Descripción breve de Coneccion
/// </summary>
/// 
namespace GlobalInfo
{
    public class Coneccion
    {
        public SqlConnection ConnPlabal { get; set; }
        public SqlConnection ConnGlasser { get; set; }
        public SqlConnection ConnAlfak { get; set; }
        public SqlCommand CmdPlabal { get; set; }
        public SqlCommand Cmd { get; set; }


        public Coneccion()
        {
            ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnection"].ConnectionString);
            ConnGlasser = new SqlConnection(WebConfigurationManager.ConnectionStrings["GLASSERConnection"].ConnectionString);
            ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALFAKConnection"].ConnectionString);
            
            CmdPlabal = new SqlCommand();
            Cmd = new SqlCommand();

        }
    }

}
