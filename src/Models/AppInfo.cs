namespace Matriks.ClientAPI.Setup.Models
{
  public class AppInfo
  {
    public string ExeName { get; set; }
    public string ZipName { get; set; }
    public string FolderName { get; set; }
    public string[] Args { get; set; }
    public bool IsWindowsService { get; set; }
    public bool RunAuto { get; set; }
  }
}