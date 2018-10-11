using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alfak;
using nsCliente;

/// <summary>
/// Descripción breve de EnviarPedido
/// </summary>
/// 
namespace Ecommerce
{

    public class EnviarPedidoDVH
    {
        #region retorno de clase
        public readonly bool IsSuccess;
        public readonly string Mensaje;
        public readonly string[] NroPedido;
        #endregion


        private readonly PedidoEcom pedidoEcom;
        
        private readonly GetInfoClienteEcomm infoClienteEcomm;
        private readonly DatosCliente DatosCli;
        private readonly ClienteAlfak clienteAlfak;
        private readonly string Sucursal;

        public EnviarPedidoDVH(PedidoEcom _Pedido, GetInfoClienteEcomm InfoCliEcom)
        {
            
            pedidoEcom = _Pedido;
            infoClienteEcomm = InfoCliEcom;
            DatosCli = new DatosCliente(pedidoEcom.RUT);
            if (DatosCli.Region=="05")
            {
                Sucursal = "VIÑA DEL MAR";
            }
            else
            {
                Sucursal = "SANTIAGO";
            }
            if (!DatosCli.Bloqueado && DatosCli.EFinanciero.Disponible>_Pedido.Bruto)
            {
                PedidoAlfak.Generate generate = new PedidoAlfak.Generate(_Pedido,Sucursal);
                if (generate.IsSuccess)
                {
                    NroPedido = generate.NroPedido;
                    
                    /*actualizar el nro de pedido en PLABAL*/
                    /*Ingresar en PLABAL.Planificacion*/
                }
                else
                {
                    IsSuccess = false;
                    Mensaje = generate.Mensaje;
                }
            }
            else
            {
                IsSuccess = false;
                Mensaje = "El pedido no pudo ser ingresado debido a que el cliente está bloqueado o el cupo disponible no alcanza.";

            }
        }
    }


}

