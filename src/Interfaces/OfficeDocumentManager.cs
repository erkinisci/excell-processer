using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excell.Processor.Core;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using OfficeOpenXml;

namespace Excell.Processor.Interfaces
{
  public class OfficeDocumentManager : IOfficeeDocumentManager
  {
    private readonly ISetupLogger _logger;

    public OfficeDocumentManager()
    {
      _logger = DependencyContainer.Resolver.GetService<ISetupLogger>("SetupLogger");
    }

    public bool CreateExcelDocument(FileItem file, DataTable table)
    {
      try
      {
        var excellProcessorSetup = DependencyContainer.Resolver.GetService<ExcellProcessorSetupModel>("ExcellProcessorSetupModel");
        var filePath = new FileInfo(Path.Combine(excellProcessorSetup.OutherFolderPath, file.FileName));
        var xlPackage = new ExcelPackage(filePath);

        var worksheet = xlPackage.Workbook.Worksheets.Add(file.FileName.Replace(Path.GetExtension(file.FileName),""));

        var index = 1;
        foreach (DataColumn tableColumn in table.Columns)
        {
          worksheet.Cells[1, index].Value = " ";
          worksheet.Cells[2, index++].Value = tableColumn.ColumnName;
        }


        for (var i = 1; i < table.Rows.Count; i++)
        {
          for (var j = 0; j < table.Columns.Count; j++)
          {
            var value= table.Rows[i][j].ToString();
            worksheet.Cells[i + 2, j + 1].Value = value;
          }
        }

        xlPackage.SaveAs(filePath);
      }
      catch (Exception e)
      {
        _logger.WriteErrorLog(e.StackTrace);
        return false;
      }

      return true;
    }

    public bool CreateExcelDocument<T>(string worksheetName, string xlsxFilePath, List<T> list)
    {
      try
      {
        var filePath = new FileInfo(xlsxFilePath);
        var xlPackage = new ExcelPackage(filePath);

        var worksheet = xlPackage.Workbook.Worksheets.Add(worksheetName);

        var propertyInfoList = typeof(T).GetProperties();

        for (var i = 0; i < propertyInfoList.Length; i++)
        {
          worksheet.Cells[1, i + 1].Value = propertyInfoList[i].Name;
        }

        for (var i = 2; i < list.Count; i++)
        {
          for (var j = 1; j < propertyInfoList.Length; j++)
          {
            worksheet.Cells[i, j].Value = propertyInfoList[j - 1].GetValue(list[i], null);
          }
        }

        xlPackage.SaveAs(filePath);
      }
      catch (Exception e)
      {
        _logger.WriteErrorLog(e.StackTrace);
        return false;
      }

      return true;
    }

    public bool CreateExcellDocumentRaw(string worksheetName, string xlsxFilePath, List<List<string>> list)
    {
      try
      {
        var filePath = new FileInfo(xlsxFilePath);
        var xlPackage = new ExcelPackage(filePath);

        var worksheet = xlPackage.Workbook.Worksheets.Add(worksheetName);

        for (var i = 0; i < list.Count; i++)
        {
          var row = list[i];

          for (var j = 0; j < row.Count; j++)
            worksheet.Cells[i + 1, j + 1].Value = row[j];
        }

        xlPackage.SaveAs(filePath);
      }
      catch (Exception e)
      {
        _logger.WriteErrorLog(e.StackTrace);
        return false;
      }

      return true;
    }
  }
}