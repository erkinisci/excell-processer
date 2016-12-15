﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excell.Processor.Models
{
  public class FileItem
  {
    public string FileName { get; set; }
    public string Path { get; set; }
    public bool IsSelected { get; set; }
  }

  public class ColumnItem
  {
    public string ColumName { get; set; }
    public int Index { get; set; }
    public bool IsSelected { get; set; }
  }
}
