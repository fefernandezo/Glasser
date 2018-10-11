using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Windows.Media.Imaging;

/// <summary>
/// Descripción breve de ExcelCotizacion
/// </summary>
/// 
namespace Comercial
{
    namespace Cotizador
    {

        public class ExcelCotizacion
        {
            public readonly byte[] Excel;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            Border border;
            private readonly string IDENDO;
            private readonly string TOKEN;
            private readonly bool ESTADO;
            public ExcelCotizacion(string _IDENDO, string _TOKEN, bool _ESTADO)
            {
                IDENDO = _IDENDO;
                ESTADO = _ESTADO;
                TOKEN = _TOKEN;
                Excel = ExcelGenerado();
            }

            public byte[] ExcelGenerado()
            {
                Cotizacion.GetRowInfo Cot = new Cotizacion.GetRowInfo(IDENDO, TOKEN, ESTADO);
                using (var XlsPack=new ExcelPackage())
                {
                    XlsPack.Workbook.Properties.Author = "Glasser";
                    XlsPack.Workbook.Properties.Title = "Presupuesto N°" + Cot.Correlativo + " " + Cot.CLIENTEDOC;
                    var hoja1 = XlsPack.Workbook.Worksheets.Add("Formulario");
                    hoja1.Name = "Presupuesto";
                    hoja1.PrinterSettings.PaperSize = ePaperSize.Letter;
                    hoja1.Column(1).Width = 4.5;
                    border = hoja1.Cells.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    #region Report Header
                    hoja1.Row(3).Height = 32;
                    hoja1.Column(2).Width = 2;
                    hoja1.Column(8).Width = 2.5;
                    hoja1.Column(9).Width = 2.5;
                    cell = hoja1.Cells[3,1,3,12];
                    cell.Merge = true;
                    cell.Value = "Presupuesto N°" + Cot.Correlativo + " " + Cot.CLIENTEDOC;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 18;
                    
                    
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    #endregion

                    DetalleCotizacion.Get Detalle = new DetalleCotizacion.Get(IDENDO, ESTADO);

                    int row = 6;
                    #region Report Detail
                    foreach (DetalleCotizacion item in Detalle.Detalle)
                    {
                        /*Nro de item*/
                        cell = hoja1.Cells[row,1];
                        cell.Value =Convert.ToInt32(item.POS_NR);

                        /*nombre*/
                        cell = hoja1.Cells[row, 3,row,10];
                        cell.Merge = true;
                        cell.Value = item.NOMBRE;
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        /*descripcion*/
                        cell = hoja1.Cells[row + 1, 3, row + 2, 7];
                        cell.Merge = true;
                        cell.Style.WrapText = true;
                        cell.Value = item.OBSERVACION;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        /*imagen*/
                        AddImage(hoja1, row + 1, 9, item.IMAGEN, item.NOMBRE);

                        row = row + 3;
                        /*lista de piezas*/
                        SubDetalleCotizacion.Get get = new SubDetalleCotizacion.Get(1, IDENDO, item.IDENDODET, true);

                        if (get.HasDetail)
                        {
                            /*encabezado piezas*/
                            /*--pieza--*/
                            cell = hoja1.Cells[row, 4, row, 5];
                            cell.Merge = true;
                            cell.Value = "Pieza";
                            cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            /*--Ancho--*/
                            cell = hoja1.Cells[row, 6];
                            cell.Value = "Ancho";
                            cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            /*--Alto--*/
                            cell = hoja1.Cells[row, 7];
                            cell.Value = "Alto";
                            cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            row++;

                            foreach (SubDetalleCotizacion Sub in get.SubDetalle)
                            {
                                /*pieza*/
                                cell = hoja1.Cells[row, 4, row, 5];
                                cell.Merge = true;
                                cell.Value = Sub.NOMBRE;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                /*ancho*/
                                cell = hoja1.Cells[row, 6];
                                cell.Value = Sub.ANCHO;
                                
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                /*alto*/
                                cell = hoja1.Cells[row, 7];
                                cell.Value = Sub.ALTO;
                                
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                row++;

                            }
                        }

                        
                        row++;

                        /*Precio Unitario*/
                        
                        cell = hoja1.Cells[row, 4, row, 5];
                        cell.Merge = true;
                        cell.Value = "Precio Un. Neto:";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell = hoja1.Cells[row, 6, row, 7];
                        cell.Merge = true;
                        cell.Value = item.NETOUN;
                        cell.Style.Numberformat.Format = "$#,##0";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        row++;
                        /*Cantidad*/
                        cell = hoja1.Cells[row, 4, row, 5];
                        cell.Merge = true;
                        cell.Value = "Cantidad:";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell = hoja1.Cells[row, 6, row, 7];
                        cell.Merge = true;
                        cell.Value = item.CANTIDAD;
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        row++;
                        row++;
                        /*Total*/
                        cell = hoja1.Cells[row, 4, row, 5];
                        cell.Merge = true;
                        cell.Value = "Neto Total:";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell = hoja1.Cells[row, 6, row, 7];
                        cell.Merge = true;
                        cell.Value = item.NETO;
                        cell.Style.Numberformat.Format = "$#,##0";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        

                        row = row + 6;


                    }



                    #endregion

                    #region Report footer

                    #endregion


                    return XlsPack.GetAsByteArray();
                }
            }

            private void AddImage(ExcelWorksheet oSheet, int rowIndex, int ColIndex, string ImgPath, string nombre)
            {
                WebClient wc = new WebClient();
                byte[] ORG = wc.DownloadData(ImgPath);
                MemoryStream Memstm = new MemoryStream(ORG);
                
                Bitmap image = new Bitmap(Memstm);
                ExcelPicture excelImg = null;
                if (image !=null)
                {
                    excelImg = oSheet.Drawings.AddPicture(nombre, image);
                    excelImg.From.Column = ColIndex;
                    excelImg.From.Row = rowIndex;
                    excelImg.SetSize(200,200);
                    excelImg.From.ColumnOff = Pixewl2MTU(2);
                    excelImg.From.RowOff = Pixewl2MTU(2);
                }
            }

            private int Pixewl2MTU(int pixels)
            {
                int mtus = pixels * 9525;
                return mtus;
            }
        }
    }
}
