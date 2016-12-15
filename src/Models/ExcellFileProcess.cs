using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

    public ExcellFileProcess()
    {
      SetupLogger = DependencyContainer.Resolver.GetService<ISetupLogger>("SetupLogger");
    }

    public DataTable GetConentAsTable(FileItem file)
    {
      var excelTable = new DataTable(file.FileName);





      return excelTable;
    }

    public List<ColumnItem> GetColumnCollection(FileItem file)
    {
      var columns = new List<ColumnItem>();
      try
      {


        var xlApp = new Excel.Application();
        var xlWorkbook = xlApp.Workbooks.Open(file.Path);
        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
        var xlRange = xlWorksheet.UsedRange;
        try
        {


          var colCount = xlRange.Columns.Count;

          for (var j = 1; j <= colCount; j++)
          {
            if (xlRange.Cells[1, j].value2 != null && xlRange.Cells[1, j].value2 != null)
              columns.Add(new ColumnItem() { ColumName = xlRange.Cells[1, j].value2, Index = j, IsSelected = false });
          }
        }
        catch (Exception ex)
        {


        }
        finally
        {
          GC.Collect();
          GC.WaitForPendingFinalizers();

          //rule of thumb for releasing com objects:
          //  never use two dots, all COM objects must be referenced and released individually
          //  ex: [somthing].[something].[something] is bad

          //release com objects to fully kill excel process from running in the background
          Marshal.ReleaseComObject(xlRange);
          Marshal.ReleaseComObject(xlWorksheet);

          //close and release
          xlWorkbook.Close();
          Marshal.ReleaseComObject(xlWorkbook);

          //quit and release
          xlApp.Quit();
          Marshal.ReleaseComObject(xlApp);
        }
      }
      catch (Exception)
      {

      }


      return columns;
    }
  }
}
