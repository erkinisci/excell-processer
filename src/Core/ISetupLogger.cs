namespace Excell.Processor.Core
{
  public interface ISetupLogger
  {
    void WriteInfoLog(string message);

    void WriteErrorLog(string message);

    string GetLogFilePath();
  }
}