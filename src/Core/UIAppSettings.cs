using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Matriks.Oms.EnterpriseLibrary.Configuration;

namespace Excell.Processor.Core
{ 
  public class UIAppSettings : IAppSettings
  {
    private const string None = "_NONE";

    private IDictionary<string, string> settingsDictionary = new Dictionary<string, string>();

    public UIAppSettings()
    {
      var xDoc = XDocument.Load(File.Open(AppDomain.CurrentDomain.BaseDirectory + "config/settings.xml", FileMode.Open));

      var monitorPortElements = xDoc.XPathSelectElements("settings/settingGroup/add");

      foreach (XElement element in monitorPortElements)
      {
        settingsDictionary[element.Attribute("key").Value] = element.Attribute("value").Value;

      }
    }
    public bool GetBool(string key)
    {
      return GetBool(key, false);
    }
    public byte GetByte(string key)
    {
      return GetByte(key, 0);
    }

    public int GetInt(string key)
    {
      return GetInt(key, 0);
    }

    public long GetLong(string key)
    {
      return GetLong(key, 0);
    }

    public float GetFloat(string key)
    {
      return GetFloat(key, 0);
    }

    public decimal GetDecimal(string key)
    {
      return GetDecimal(key, 0);
    }

    public double GetDouble(string key)
    {
      return GetDouble(key, 0);
    }

    public string GetString(string key)
    {
      return GetString(key, null);
    }

    public bool GetBool(string key, bool defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      if (string.IsNullOrWhiteSpace(value))
        return false;

      if (value == "1" || value == bool.TrueString.ToLower())
        return true;

      return defaultValue;
    }

    public byte GetByte(string key, byte defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return Convert.ToByte(GetKeyValue(key));
    }

    public int GetInt(string key, int defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return Convert.ToInt32(value);
    }

    public long GetLong(string key, long defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return Convert.ToInt64(value);
    }

    public float GetFloat(string key, float defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return Convert.ToSingle(value);
    }

    public decimal GetDecimal(string key, decimal defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return Convert.ToDecimal(value);
    }

    public double GetDouble(string key, double defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return Convert.ToDouble(value);
    }

    public string GetString(string key, string defaultValue)
    {
      string value = GetKeyValue(key);

      if (value == None)
        return defaultValue;

      return value;
    }

    private string GetKeyValue(string key)
    {
      var element = settingsDictionary[key];

      return element ?? "_NONE";
    }
  }
}