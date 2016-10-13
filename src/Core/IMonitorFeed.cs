using Matriks.Oms.EnterpriseLibrary.Network.Monitoring;

namespace Matriks.ClientAPI.Setup.Core
{
  public interface IMonitorFeed
  {
    void OnData(IMonitorPacket packet);
  }
}