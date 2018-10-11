using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Validadores
/// </summary>
/// 
namespace GlobalInfo
{
    public class Validadores
    {
        public Validadores()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public DateTime ParseoDateTime(string dateTime)
        {
            DateTime Fecha;
            DateTime FechaC = DateTime.TryParse(dateTime, out Fecha)
            ? Fecha
            :  new DateTime(1975,1,1);
            

            return FechaC;

        }
        public bool ParseoBoolean(string Bit)
        {
            bool Is;
            int Binario;
            if (int.TryParse(Bit,out Binario))
            {
                Is = Convert.ToBoolean(Binario);
            }
            else
            {
                if (bool.TryParse(Bit, out Is))
                {

                }
                else
                {
                    Is = false;
                }

            }
            

            return Is;
        }

        public double ParseoDouble(string Numero)
        {
            double retorno;
            if (double.TryParse(Numero,out retorno))
            {

            }
            else
            {
                retorno = 0;
            }

            return retorno;
        }

        public int ParseoInt(string Numero)
        {
            int retorno;
            if (int.TryParse(Numero, out retorno))
            {

            }
            else
            {
                retorno = 0;
            }
            return retorno;

        }
    }

}
