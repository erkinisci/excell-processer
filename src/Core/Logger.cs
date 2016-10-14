using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

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
  }
}
