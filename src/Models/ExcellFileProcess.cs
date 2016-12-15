using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Excell.Processor.Core;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excell.Processor.Models
{
  public class ExcellFileProcess
  {
    private static ExcellFileProcess _instance;
    public static ExcellFileProcess Instance => _instance ?? (_instance = new ExcellFileProcess());
    public ISetupLogger SetupLogger { get; set; }

    private readonly List<string> _constColumns = new List<string>(new[] {"Ad", "Soyad", "Gsm"});

    public ExcellFileProcess()
    {
      SetupLogger = DependencyContainer.Resolver.GetService<ISetupLogger>("SetupLogger");
    }

    public DataTable GetConentAsTable(FileItem file, List<ColumnItem> columns)
    {
      var excelTable = new DataTable(file.FileName);

      try
      {

        SetupLogger.WriteInfoLog("Excell clone olusturuluyor.");
        var xlApp = new Excel.Application();
        var xlWorkbook = xlApp.Workbooks.Open(file.Path);
        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
        var xlRange = xlWorksheet.UsedRange;
        SetupLogger.WriteInfoLog("Excell used range alindi.");
        try
        {

          var colCount = xlRange.Columns.Count;
          var rowCount = xlRange.Rows.Count;
          SetupLogger.WriteInfoLog($"Kolon Sayisi.{colCount}");
          SetupLogger.WriteInfoLog($"Row Sayisi.{rowCount}");

          excelTable.Columns.Add(new DataColumn() { ColumnName = "Sıra No", DataType = typeof(int) });

          foreach (var columnItem in (from c in columns where c.IsSelected select c))
            excelTable.Columns.Add(new DataColumn() {ColumnName = columnItem.ColumName, DataType = typeof(string)});

          for (var i = 1; i <= rowCount; i++)
          {
            var row = excelTable.NewRow();
            excelTable.Rows.Add(row);

            row[0] = i;
            foreach (var columnItem in (from c in columns where c.IsSelected select c))
            {
              try
              {
                if (xlRange.Cells[i, columnItem.Index].value2 == null || xlRange.Cells[i, columnItem.Index].value2 == null) continue;
                var value = xlRange.Cells[i, columnItem.Index].value2.ToString();

                row[columnItem.ColumName] = value;
              }
              catch (Exception ex)
              {
                SetupLogger.WriteInfoLog($"Hata. Hucre degeri alinirken olustu. {ex.StackTrace}");
              }
            }
          }
        }
        catch (Exception ex)
        {
          SetupLogger.WriteInfoLog($"Hata ColumnsValue. {ex.StackTrace}");
        }
        finally
        {
          GC.Collect();
          GC.WaitForPendingFinalizers();

          Marshal.ReleaseComObject(xlRange);
          Marshal.ReleaseComObject(xlWorksheet);

          xlWorkbook.Close();
          Marshal.ReleaseComObject(xlWorkbook);

          xlApp.Quit();
          Marshal.ReleaseComObject(xlApp);
        }
      }
      catch (Exception ex)
      {
        SetupLogger.WriteInfoLog($"Hata GetColumnCollection: {ex.StackTrace}");
      }


      return excelTable;
    }

    public List<ColumnItem> GetColumnCollection(FileItem file)
    {
      var columns = new List<ColumnItem>();
      try
      {

        SetupLogger.WriteInfoLog("Excell clone olusturuluyor.");
        var xlApp = new Excel.Application();
        var xlWorkbook = xlApp.Workbooks.Open(file.Path);
        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
        var xlRange = xlWorksheet.UsedRange;
        SetupLogger.WriteInfoLog("Excell used range alindi.");
        try
        {
          var colCount = xlRange.Columns.Count;
          int rowCount = xlRange.Rows.Count;
          SetupLogger.WriteInfoLog($"Kolon Sayisi.{colCount}");
          SetupLogger.WriteInfoLog($"Row Sayisi.{rowCount}");

          for (var j = 1; j <= colCount; j++)
          {
            try
            {
              if (xlRange.Cells[1, j].value2 == null || xlRange.Cells[1, j].value2 == null) continue;
              var value = xlRange.Cells[1, j].value2.ToString();
              var selectedValue = _constColumns.Contains(value);
              columns.Add(new ColumnItem() { ColumName = value, Index = j, IsSelected = selectedValue });
            }
            catch (Exception ex)
            {
              SetupLogger.WriteInfoLog($"Hata. Hucre degeri alinirken olustu. {ex.StackTrace}");
            }
          }
        }
        catch (Exception ex)
        {
          SetupLogger.WriteInfoLog($"Hata ColumnsValue. {ex.StackTrace}");
        }
        finally
        {
          GC.Collect();
          GC.WaitForPendingFinalizers();

          Marshal.ReleaseComObject(xlRange);
          Marshal.ReleaseComObject(xlWorksheet);

          xlWorkbook.Close();
          Marshal.ReleaseComObject(xlWorkbook);

          xlApp.Quit();
          Marshal.ReleaseComObject(xlApp);
        }
      }
      catch (Exception ex)
      {
        SetupLogger.WriteInfoLog($"Hata GetColumnCollection: {ex.StackTrace}");
      }

      return columns;
    }
  }
}
