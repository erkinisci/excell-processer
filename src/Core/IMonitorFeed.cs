using Matriks.Oms.EnterpriseLibrary.Network.Monitoring;

namespace Matriks.ClientAPI.UI.Core
{
  public interface IMonitorFeed
  {
    void OnData(IMonitorPacket packet);
  }
}