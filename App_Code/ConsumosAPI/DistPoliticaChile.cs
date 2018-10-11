using ConsumosAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Descripción breve de DistPoliticaChile
/// </summary>
public class DistPoliticaChile
{
    public DistPoliticaChile()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //

    }

    public class Variable
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }
        [JsonProperty("tipo")]
        public string Tipo { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lng")]
        public double Lng { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("codigo_padre")]
        public string Codigo_padre { get; set; }
        
    }


    public class Error
    {
        public bool error { get; set; }
        public string message { get; set; }
    }




    public class GetRegiones
    {
        public readonly List<Variable> Regiones;
        public GetRegiones()
        {
            GetFromAPI get = new GetFromAPI("regiones");
            Regiones = get.Lista;
        }

        public GetRegiones(string Codigo)
        {
            string[] tipos = new string[] { "regiones" };
            string[] codigos = new string[] { Codigo };
            GetFromAPI get = new GetFromAPI(tipos,codigos);
            Regiones = get.Lista;

        }
    }

    public class GetProvincias
    {

        public GetProvincias(string Codregion)
        {

        }

        public GetProvincias(string Codregion, string CodProvincias)
        {

        }

        public GetProvincias()
        {

        }

    }

    public class GetComunas
    {
        public readonly List<Variable> Comunas;
        public GetComunas(string CodRegion)
        {
            string[] tipos = new string[] {"regiones","comunas" };
            string[] codigos = new string[] {CodRegion };
            GetFromAPI get = new GetFromAPI(tipos,codigos);
            Comunas = get.Lista;

        }

        public GetComunas(string CodRegion, string CodComuna)
        {
            string[] tipos = new string[] { "regiones", "comunas" };
            string[] codigos = new string[] { CodRegion, CodComuna };
            GetFromAPI get = new GetFromAPI(tipos, codigos);
            Comunas = get.Lista;

        }



    }

    public class GetFromAPI
    {
        public List<Variable> Lista;

        public Error error;
        public bool IsSuccess;

        private Variable variable;
        private readonly string URL;
        private readonly string Compo;
        private readonly string APIbase = "http://apis.modernizacion.cl/dpa";

        public GetFromAPI(string[] tipos, string[] codigos)
        {
            int cont1 = tipos.Count();
            int cont2 = codigos.Count();
            for (int i = 0; i < cont1; i++)
            {
                Compo += "/" + tipos[i];
                if (i<cont2)
                {
                    Compo += "/" + codigos[i];
                }
                
            }
            URL = APIbase + Compo;
            Accion();
        }

        public GetFromAPI(string tipo)
        {

            URL = APIbase + "/" + tipo;
            Accion();

        }

        private void Accion()
        {
            string json = new WebClient().DownloadString(URL);
            Lista = new List<Variable>();
            try
            {
                var datos = JsonConvert.DeserializeObject<List<Variable>>(json);
                
                foreach (var item in datos)
                {
                    variable = new Variable
                    {
                        Codigo = item.Codigo,
                        Codigo_padre = item.Codigo_padre,
                        Lat = item.Lat,
                        Lng = item.Lng,
                        Nombre = item.Nombre,
                        Tipo = item.Tipo,
                        Url = item.Url,
                    };
                    Lista.Add(variable);

                }
                if (Lista.Count > 0)
                {
                    IsSuccess = true;
                }
            }
            catch
            {
               var datos = JsonConvert.DeserializeObject(json);

                Variable var = JsonConvert.DeserializeObject<Variable>(json);

                Lista.Add(var);



            }

            
            

        }
    }
}