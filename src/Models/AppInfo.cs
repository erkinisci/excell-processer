using System.Text;

namespace Excell.Processor.Models
{
  public class AppInfo
  {
    public string ExeName { get; set; }
    public string ZipName { get; set; }
    public string FolderName { get; set; }
    public string ShortCutName { get; set; }
    public string ServiceName { get; set; }
    public string[] Args { get; set; }
    public bool IsWindowsService { get; set; }
    public bool RunAuto { get; set; }

    public bool IsSetup { get; set; }

    public string Arguments
    {
      get
      {
        if (Args != null)
        {
          var sb = new StringBuilder();
          foreach (var arg in Args)
          {
            if (sb.Length > 0)
              sb.Append(" ");
            sb.Append(arg);
          }
          return sb.ToString();
        }

        return null;
      }
    }
  }
}