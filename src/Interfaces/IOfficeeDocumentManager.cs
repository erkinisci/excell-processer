using System.Collections.Generic;
using System.Data;
using Excell.Processor.Models;

namespace Excell.Processor.Interfaces
{
  public interface IOfficeeDocumentManager
  {
    bool CreateExcelDocument(FileItem file, DataTable table);
    bool CreateExcelDocument<T>(string worksheetName, string xlsxFilePath, List<T> list);
    bool CreateExcellDocumentRaw(string worksheetName, string xlsxFilePath, List<List<string>> list);
  }
}