using Ecommerce;
using nsCliente;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CrearExcel
/// </summary>
/// 
namespace GlasserExcel
{
    public class CrearExcel
    {
        
        public class PedidosEcomm
        {
            public readonly byte[] Excel;
            private int rowIndex = 1;
            private ExcelRange cell;
            private ExcelFill fill;
            private Border border;
            private List<PedidoEcom> Items;
            private readonly DatosCliente _Cli;

            public PedidosEcomm(List<PedidoEcom> _Items, DatosCliente _Cliente)
            {
                _Cli = _Cliente;
                Items = _Items;
                Excel = Generado();
            }

            public byte[] Generado()
            {
                using (var XlsPack= new ExcelPackage())
                {
                    XlsPack.Workbook.Properties.Author = "Glasser";
                    XlsPack.Workbook.Properties.Title = "Informe Pedidos " + DateTime.Today.ToShortDateString();
                    var hoja1 = XlsPack.Workbook.Worksheets.Add("Pedidos");
                    hoja1.Name = "Pedidos";
                    hoja1.PrinterSettings.PaperSize = ePaperSize.Letter;
                    

                    hoja1.Column(2).Width = 60;
                    hoja1.Column(3).Width = 20;
                    hoja1.Column(4).Width = 20;
                    hoja1.Column(5).Width = 10;
                    hoja1.Column(6).Width = 10;
                    hoja1.Column(7).Width = 25;
                    hoja1.Column(8).Width = 18;
                    hoja1.Column(9).Width = 12;
                    hoja1.Row(6).Height = 30;
                    hoja1.Row(6).Style.WrapText=true;
                    #region Header


                    int row = 2;
                    int col1 = 1;
                    
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Cliente: ";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    col1++;

                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = _Cli.Nombre;
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    col1++;

                    row++;
                    col1 = 1;
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Rut: ";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    col1++;

                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = _Cli.Rut;
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    col1++;


                    /*--Item--*/
                    row = 6;
                    col1 = 1;
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Item";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--nombre--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Pedido";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--Nro--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Números de Pedido";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--Tipo--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Tipo";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--Cantidad--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Cantidad";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--Total--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Neto";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--Ingresado por--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Ingresado por";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--fentre--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Fecha Entrega";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;

                    /*--Estado--*/
                    cell = hoja1.Cells[row, col1];
                    cell.Merge = true;
                    cell.Value = "Estado";
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    col1++;
                    #endregion

                    row++;
                    int cont = 1;
                    
                    foreach (PedidoEcom item in Items)
                    {
                        int col = 1;
                        /*item*/
                        cell = hoja1.Cells[row,col];
                        cell.Merge = true;
                        cell.Value = cont ;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Nombre*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = item.Nombre;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Nros*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = item.NroPedido;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Tipo*/
                        string Tipo = null;
                        if (item.OrderType=="TER")
                        {
                            Tipo = "Termopanel";
                        }
                        else if (item.OrderType=="TEM")
                        {
                            Tipo = "Templado";
                        }
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = Tipo;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Cant*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = item.CanTotal;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Neto*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = item.Neto;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Usuario*/
                        GlasserUser.GetInfo GGI = new GlasserUser.GetInfo(item.UserId);
                        string Usuario;
                        if (GGI.IsSuccess)
                        {
                            Usuario = GGI.Datos.Nombre + " " + GGI.Datos.Apellido;
                        }
                        else
                        {
                            Usuario = item.UserId;
                        }
                        /*Usuario*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = Usuario;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        /*Fentrega*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = item.F_Entrega;
                        cell.Style.Numberformat.Format = "dd-mm-yyyy";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        col++;

                        string Estado;
                       
                        if (item.Estado == "BOR")
                        {
                            Estado = " Borrador";
                            
                        }
                        else if (item.Estado == "DES")
                        {
                            Estado = " Entregado";
                            
                        }
                        else if (item.Estado == "ING")
                        {
                            Estado = " Ingresado";
                            
                        }
                        else if (item.Estado == "PRG")
                        {
                            Estado = " En Fabricación";
                           
                        }
                        else if (item.Estado == "DIS")
                        {
                            Estado = " Bodega";
                           
                        }
                        else
                        {
                            Estado = "";
                        }

                        /*Estado*/
                        cell = hoja1.Cells[row, col];
                        cell.Merge = true;
                        cell.Value = Estado;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        col++;

                        cont++;
                        row++;
                    }


                    return XlsPack.GetAsByteArray();
                }

                
            }
        }
    }
}
