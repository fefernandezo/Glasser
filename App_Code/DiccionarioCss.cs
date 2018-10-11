using System.Data.SqlClient;
using System.Web.Configuration;

namespace ProRepo
{
    public class DiccionarioCss : System.Web.UI.Page
    {

        SqlConnection ConnPlabal = new SqlConnection(WebConfigurationManager.ConnectionStrings["PLABALConnectionString"].ConnectionString);
        SqlConnection ConnAlfak = new SqlConnection(WebConfigurationManager.ConnectionStrings["PHGLASSConnectionStringALFAK"].ConnectionString);
        SqlCommand cmd1;
        SqlCommand cmdAlfak;
        SqlCommand cmdPlabal;
        static SqlDataReader drAlfak;
        static SqlDataReader drPlabal;




        public void Inserte_asigncomp(string codigo, string id)
        {
            string Consulta = "INSERT INTO PLABAL.dbo.e_asigncomp (Cod_prod,Id_componente) VALUES (@codigo,@Id)";

            ConnPlabal.Open();
            cmd1 = new SqlCommand(Consulta, ConnPlabal);
            cmd1.Parameters.AddWithValue("@codigo", codigo);
            cmd1.Parameters.AddWithValue("@Id", id);
            cmd1.ExecuteNonQuery();
            ConnPlabal.Close();
        }

        public void Inserte_dicc(decimal espsep, int esp1, int esp3, string comph, string producli, string cod)
        {
            FuncUser Usuario = new FuncUser();
            Infousuario infousu = Usuario.DatosUsuario();

            ConnPlabal.Open();

            cmdPlabal = new SqlCommand("INSERT INTO PLABAL.dbo.e_diccionarioclientes (comp_ph,comp_cliente,per_cri1,per_herraje,per_cri2,codigo,Id_usuario,fecha) VALUES (@comph,@nombrepro,@esp1,@esp2,@esp3,@codigo,@Id_usuario, GETDATE())", ConnPlabal);

            cmdPlabal.Parameters.AddWithValue("@esp2", espsep);
            cmdPlabal.Parameters.AddWithValue("@esp1", esp1);
            cmdPlabal.Parameters.AddWithValue("@esp3", esp3);
            cmdPlabal.Parameters.AddWithValue("@comph", comph);
            cmdPlabal.Parameters.AddWithValue("@nombrepro", producli);
            cmdPlabal.Parameters.AddWithValue("@codigo", cod);
            cmdPlabal.Parameters.AddWithValue("@Id_usuario", infousu.Id);



            int filas = cmdPlabal.ExecuteNonQuery();

            ConnPlabal.Close();
        }

        public int Valid_Cod(string codigo)
        {
            int valor = 0;
            ConnAlfak.Open();
            string SelectAlfak = "SELECT * FROM PHGLASS.SYSADM.BA_PRODUKTE_BEZ WHERE BA_PRODUKT = @codAlfak";
            cmdAlfak = new SqlCommand(SelectAlfak, ConnAlfak);
            cmdAlfak.Parameters.AddWithValue("@codAlfak", codigo);
            drAlfak = cmdAlfak.ExecuteReader();
            drAlfak.Read();

            if (drAlfak.HasRows)
            {

            }
            else
            {
                valor = 4;
            }
            drAlfak.Close();
            ConnAlfak.Close();


            return valor;
        }


        public Valores Valid_comp(string Componente)
        {
            Valores valores = new Valores();


            string Select = "Select codigo_estruc_alfak, nombre_componente,convert(int,espesor),id_componente from PLABAL.dbo.e_componente where id_componente=@componente";

            ConnPlabal.Open();
            cmdPlabal = new SqlCommand(Select, ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@componente", Componente);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {
                valores.Cod1 = drPlabal[0].ToString();
                valores.Nom1 = drPlabal[1].ToString().Trim();
                valores.Esp1 = drPlabal[2].ToString();
                valores.Id1 = drPlabal[3].ToString();
            }
            else
            {
                valores.Validacion = "1";
            }


            ConnPlabal.Close();

            return valores;


        }

        public string AgregComp(string codigo)
        {

            string var = "";

            ConnPlabal.Open();
            cmdPlabal = new SqlCommand("Select * from PLABAL.dbo.e_asigncomp where Cod_prod=@codigo", ConnPlabal);
            cmdPlabal.Parameters.AddWithValue("@codigo", codigo);
            drPlabal = cmdPlabal.ExecuteReader();
            drPlabal.Read();

            if (drPlabal.HasRows)
            {

            }
            else
            {
                var = "1";

            }

            return var;
        }





    }
    public class Valores
    {
        public string Cod1 { get; set; }
        public string Nom1 { get; set; }
        public string Esp1 { get; set; }
        public string Id1 { get; set; }
        public string Validacion { get; set; }
    }

}