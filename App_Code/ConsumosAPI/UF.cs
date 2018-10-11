using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using GlobalInfo;
using Newtonsoft;
using Newtonsoft.Json;

/// <summary>
/// Descripción breve de UF
/// </summary>
/// 
namespace ConsumosAPI
{
    public class UF
    {
        public string Valor { get; set; }
        public string Fecha { get; set; }
    }

    public class ListaUF
    {
        public List<UF> UFs { get; set; }
    }


    public class UnidadFomento
    {
       

        private readonly string APIkey = "f92001746db253ef25d3759ff972eb00613160a6";
        private readonly string APIbase = "https://api.sbif.cl/api-sbifv3/recursos_api/uf/";

        public UnidadFomento()
        {

        }
        public class GetUF
        {

            public readonly ListaUF Lista;
            

            public GetUF(int año,int mes)
            {
                UnidadFomento API = new UnidadFomento();
                
                string URL = API.APIbase + año.ToString() + "/" + mes.ToString() + "?apikey=" + API.APIkey + "&formato=json";
                GetSerializeFromUrl serializeFromUrl = new GetSerializeFromUrl(URL);
                Lista = JsonConvert.DeserializeObject<ListaUF>(serializeFromUrl.html);

            }

            public GetUF(int año)
            {
                UnidadFomento API = new UnidadFomento();
                string URL = API.APIbase + año.ToString() + "?apikey=" + API.APIkey + "&formato=json";
                GetSerializeFromUrl serializeFromUrl = new GetSerializeFromUrl(URL);
                Lista = JsonConvert.DeserializeObject<ListaUF>(serializeFromUrl.html);
            }

            public GetUF(DateTime Fecha)
            {
                UnidadFomento API = new UnidadFomento();
                string dia = Fecha.Day.ToString();
                string mes = Fecha.Month.ToString();
                string año = Fecha.Year.ToString();
                string URL = API.APIbase + año +"/" + mes + "/dias/" + dia + "?apikey=" + API.APIkey + "&formato=json";
                GetSerializeFromUrl serializeFromUrl = new GetSerializeFromUrl(URL);
                Lista = JsonConvert.DeserializeObject<ListaUF>(serializeFromUrl.html);
                
            }
        }
        
    }


    public class GetSerializeFromUrl
    {
        public readonly string html;

        public GetSerializeFromUrl(string URL)
        {
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                }
            }

        }
    }

    

}

