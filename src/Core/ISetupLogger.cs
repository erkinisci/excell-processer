namespace Matriks.ClientAPI.Setup.Core
{
  public interface ISetupLogger
  {
    void WriteInfoLog(string message);

    void WriteErrorLog(string message);
  }
}