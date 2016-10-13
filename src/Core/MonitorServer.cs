using System.Collections.Concurrent;
using System.Collections.Generic;
using Matriks.Oms.EnterpriseLibrary.Network.Monitoring;

namespace Matriks.ClientAPI.UI.Core
{
  public class MonitorServer
  {
    public event MonitorServerReceiveHandler Received;

    private static readonly IDictionary<string, MonitorReceiver> MonitorDictionary = new ConcurrentDictionary<string, MonitorReceiver>();
    private static readonly IDictionary<int, string> MonitorNameDictionary = new ConcurrentDictionary<int, string>();
    public static MonitorReceiver CreateReceiver(int port)
    {
      return new MonitorReceiver(port, null);
    }

    public void Register(string name, MonitorReceiver monitorReceiver)
    {
      MonitorDictionary[name] = monitorReceiver;
      MonitorNameDictionary[monitorReceiver.Port] = name;
    }

    public void Start()
    {
      foreach (var kvPair in MonitorDictionary)
      {
        kvPair.Value.Start();
        kvPair.Value.Received += Value_Received;
      }
    }

    public void Stop()
    {
      foreach (var kvPair in MonitorDictionary)
      {
        kvPair.Value.Received -= Value_Received;
      }
    }

    private void Value_Received(object sender, MonitorPacketArgs e)
    {
      MonitorReceiver senderReceiver = (MonitorReceiver)sender;
      string name;
      bool result = MonitorNameDictionary.TryGetValue(senderReceiver.Port, out name);
      if (result)
      {
        Received?.Invoke(name, e);
      }
    }
  }
}