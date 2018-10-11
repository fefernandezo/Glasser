using GlobalInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// Descripción breve de DatosCliente
/// </summary>
/// 

namespace nsCliente
{
    public class DatosCliente
    {
        public string Nombre { get; set; }
        public string[] Direccion { get; set; }
        public bool Bloqueado { get; set; }
        public string Region { get; set; }
        public string CondPago { get; set; }

        public EFinanciero EFinanciero { get; set; }

        Coneccion Conn;
        static SqlDataReader dr;
        private string Query { get; set; }
        public string Rut { get; set; }

        public DatosCliente(string _Rut)
        {
            Rut = _Rut;
            EFinanciero = new EFinanciero(Rut, "01");
            GetRandomData();
            
        }

        private void GetRandomData()
        {
            Conn = new Coneccion();
            Query = @"SELECT TOP 1 * FROM MAEEN WITH ( NOLOCK )  WHERE KOEN= @RUT";

            try
            {


                Conn.ConnGlasser.Open();
                Conn.CmdPlabal = new SqlCommand(Query, Conn.ConnGlasser);
                Conn.CmdPlabal.Parameters.AddWithValue("@RUT", Rut);




                dr = Conn.CmdPlabal.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Validadores Val = new Validadores();
                    Bloqueado = Val.ParseoBoolean(dr["BLOQUEADO"].ToString());
                    Nombre = dr["NOKOEN"].ToString().Trim();
                    CondPago = dr["CPEN"].ToString().Trim();
                    Region = dr["CIEN"].ToString().Trim().Substring(1);
                }
                dr.Close();
                Conn.ConnGlasser.Close();


            }
            catch (Exception EX)
            {
                string ERRORSTR = "Mensaje:" + EX.Message + " trace: " + EX.StackTrace + " Rut:" + Rut ;
                ErrorCatching gETerror = new ErrorCatching();
                gETerror.ErrorCatch(ERRORSTR, HttpContext.Current.Request.Url.ToString());
                dr.Close();
                Conn.ConnGlasser.Close();



            }

        }
    }

    public class Usuario
    {
        public readonly bool HasEmpresa;
        public List<DatosCliente> InfoEmpresas { get; set; }

        public Usuario(string User)
        {
            
            MembershipUser Miembro = Membership.GetUser(User);
            var UserId = (Guid)Miembro.ProviderUserKey;
            GetEmpresas empresas = new GetEmpresas(UserId.ToString());
            if (empresas.GotRut)
            {
                InfoEmpresas = new List<DatosCliente>();
                foreach (var item in empresas.Rut)
                {
                    DatosCliente datos = new DatosCliente(item);
                    InfoEmpresas.Add(datos);
                }

                HasEmpresa = true;
            }
            else
            {
                HasEmpresa = false;
            }
            

            
        }

       

    }

    public class GlasserUser
    {
        public string UserName { get; set; }
        public DateTime LastActivity { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }


        public class GetInfo
        {

            public readonly GlasserUser Datos;
            public readonly bool IsSuccess;
            private readonly object[] Param;
            private readonly string[] ParamName;
            private readonly SelectRows Select;

            public GetInfo(string Uid)
            {
                Param = new object[] { Uid };
                ParamName = new string[] {"Uid" };
                Select = new SelectRows("PLABAL", "Users", " TOP 1 *", "UserId=@Uid", Param, ParamName);
                IsSuccess = Select.IsGot;
                foreach (DataRow dr in Select.Datos.Rows)
                {
                    
                    Datos = new GlasserUser {
                        UserName = dr["UserName"].ToString(),
                        LastActivity = Convert.ToDateTime(dr["LastActivityDate"].ToString()),
                        Nombre= dr["Nombre"].ToString(),
                        Apellido= dr["Apellido"].ToString(),
                    };
                }
            }
        }
    }



    public class GetEmpresas
    {
        public string[] Rut { get; set; }

        public bool GotRut;

        Coneccion Conn;
        
        private string Query { get; set; }
        private readonly string UserId;


        public GetEmpresas(string _UserId)
        {
            UserId = _UserId;
            Rut= GetRut();
            
        }

        private string[] GetRut()
        {
            string[] Retorno;
            bool Istrue = true;
            string Consulta = "SELECT * FROM ECOM_EMPASIGN WHERE UserId=@UserId AND ESTADO=@Estado ORDER BY ID ASC";

            Conn = new Coneccion();

            DataTable TablaDetalle = new DataTable();
            using (Conn.ConnPlabal)
            {
                try
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(Consulta, Conn.ConnPlabal);
                    adaptador.SelectCommand.Parameters.AddWithValue("@UserId", UserId);
                    adaptador.SelectCommand.Parameters.AddWithValue("@Estado", Istrue);
                    adaptador.Fill(TablaDetalle);
                    Conn.ConnPlabal.Close();
                }
                catch (Exception ex)
                {

                }
            }

            if (TablaDetalle.Rows.Count > 0)
            {
                int i = 0;
                Retorno = new string[TablaDetalle.Rows.Count];

                foreach (DataRow drr in TablaDetalle.Rows)
                {
                    Retorno[i] = drr["RUTEMP"].ToString();
                    i++;

                }
                GotRut = true;
            }
            else
            {
                Retorno = null;
                GotRut = false;

            }

            return Retorno;

        }
    }

}

