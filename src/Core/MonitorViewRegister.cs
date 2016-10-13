using System.Collections.Generic;
using System.Threading;
using Matriks.Oms.EnterpriseLibrary.Network.Monitoring;

namespace Matriks.ClientAPI.UI.Core
{
  public class MonitorViewRegister
  {
    private static readonly HashSet<IMonitorFeed> _monitorFeeds = new HashSet<IMonitorFeed>();

    public static void Register(IMonitorFeed monitorFeed)
    {
      _monitorFeeds.Add(monitorFeed);
    }

    public static void Push(IMonitorPacket packet)
    { 
      foreach (IMonitorFeed monitorFeed in _monitorFeeds)
      { 
        ThreadPool.QueueUserWorkItem(p => monitorFeed.OnData((IMonitorPacket)p), packet);
      }
    }
  }
}