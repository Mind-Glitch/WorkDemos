using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using TransStarter_Task.WPFApplication.Common.Models;
using TransStarter_Task.WPFApplication.Modules.MainApp.Model;

namespace TransStarter_Task.WPFApplication.API.Xlsx
{
    internal class XlsxReport
    {
        internal static bool CreateReportInFile (Stream stream, Dictionary<int, List<List<TableCellDTO>>> data, out Exception exception)
        {
            try
            {
                var wb = new XLWorkbook();
                var worksheet = wb.Worksheets.Add($"XlsxReport_{wb.Worksheets.Count}");

                var currentLine = 1;
                foreach ( var yearAndReport in data )
                {
                    worksheet.Cell(currentLine, 2).SetValue("Год :");
                    worksheet.Cell(currentLine, 3).SetValue(yearAndReport.Key);
                    currentLine++;

                    if ( !CreateAnnualReport((1, currentLine), yearAndReport.Value, worksheet, out currentLine, out exception) )
                        return false;

                    currentLine += 2;
                }

                wb.SaveAs(stream);
                exception = new Exception();
                return true;
            }
            catch ( Exception ex )
            {
                exception = ex;
                return false;
            }
        }

        private static bool CreateAnnualReport (
            (int x, int y) startPoint,
            List<List<TableCellDTO>> data,
            IXLWorksheet worksheet,
            out int nextLineIndex,
            out Exception exception)
        {
            nextLineIndex = startPoint.y;
            try
            {
                foreach ( var line in data )
                {
                    var columnIndex = startPoint.x;
                    foreach ( var item in line )
                    {
                        worksheet.Cell(nextLineIndex, columnIndex).SetValue(item.Value)
                            .Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        worksheet.Cell(nextLineIndex, columnIndex).WorksheetColumn().Width = item.CellWidth / 4;

                        columnIndex++;
                    }

                    nextLineIndex++;
                }

                exception = new Exception();
                return true;
            }
            catch ( Exception ex )
            {
                exception = ex;
                return false;
            }
        }
    }
}
