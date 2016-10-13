using System;

namespace Matriks.ClientAPI.UI.Models
{
  public class PacketModel
  {
    public int SeriesIndex { get; set; }
    public DateTime DateTime { get; set; }
    public double Value { get; set; }
  }
}