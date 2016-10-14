using System;
using NLog;
using NLog.Targets;

namespace Matriks.ClientAPI.Setup.Core
{
  public class SetupLogger : ISetupLogger
  {
    private static readonly Logger _setupLogger;

    static SetupLogger()
    {
      _setupLogger = LogManager.GetLogger("ClientApiSetupLog");
    }

    public void WriteErrorLog(string message)
    {
      _setupLogger.Log(LogLevel.Info, message);
    }

    public void WriteInfoLog(string message)
    {
      _setupLogger.Log(LogLevel.Error, message);
    }

    public string GetLogFilePath()
    {
      var fileTarget = (FileTarget)LogManager.Configuration.AllTargets[1];
      var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
      var fileName = fileTarget.FileName.Render(logEventInfo);
      return fileName;
    }
  }
}
