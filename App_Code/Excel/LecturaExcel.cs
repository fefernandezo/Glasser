using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Descripción breve de LecturaExcel
/// </summary>
/// 
namespace GlasserExcel
{
    public class LecturaExcel : Page
    {
        private ExcelRange cell;
        private ExcelRow XlsRow;
        private readonly string Archivo;
        private readonly string Hoja;
        public Hashtable Htable;


        public LecturaExcel()
        {
            string fileName = "BDD _ MP ALFAK.xlsx";
            string path = Path.Combine(Server.MapPath("~/CargaMasiva/"));
            Archivo = path + fileName;
            Hoja = "Hoja1";

            ThruXlsx();
        }


        private void ThruXlsx()
        {
            Htable = new Hashtable();
            
            var fi = new FileInfo(Archivo);

            using (var paquete = new ExcelPackage(fi))
            {
                try
                {

                    var Libro = paquete.Workbook;
                    var Sheet = Libro.Worksheets.Where(it => it.Name == Hoja).First();

                    int i = 2;
                    bool DO = true;
                    while (DO)
                    {
                        string Codigo;
                        /*Codigoalfak*/
                        cell = Sheet.Cells[i, 1];
                        if (cell.Value != null)
                        {
                            Codigo = cell.Value.ToString();
                            DO = true;
                        }
                        else
                        {
                            Codigo = null;
                            DO = false;

                        }


                       


                        if (DO)
                        {
                            /*CodigoRandom*/
                            string CodigoRand;
                            cell = Sheet.Cells[i, 2];
                            CodigoRand = cell.Value.ToString();

                            /*Abrev*/
                            string Abreviacion;
                            cell = Sheet.Cells[i, 5];
                            Abreviacion = cell.Value.ToString();

                            /*Descr*/
                            string Descr;
                            cell = Sheet.Cells[i, 6];
                            Descr = cell.Value.ToString();

                            /*Precio*/
                            double Precio;
                            cell = Sheet.Cells[i, 13];
                            Precio = Convert.ToDouble(cell.Value.ToString());
                            object[] Datos = new object[] {
                                CodigoRand,Abreviacion,Descr,Precio

                            };
                            Htable.Add(Codigo, Datos);
                            i++;

                        }
                        else
                        {
                            break;
                        }

                    }
                    string salir = "";

                }
                catch (Exception ex)
                {

                    throw;
                }

            }

        }
    }
}
