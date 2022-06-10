// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Config
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace It.IDnova.Fxw
{
  public class Config
  {
    private string _configFileName = "appConfig.xml";
    private const string CFG_XML_FILE = "appConfig.xml";
    private const string CFG_SECTION_NAME = "ConfigParams";
    private XmlDocument _configXmlDoc;

    public Config(string configFileName)
    {
      if (configFileName != null)
        this._configFileName = configFileName;
      this._configXmlDoc = new XmlDocument();
      try
      {
        this._configXmlDoc.Load(this._configFileName);
      }
      catch (FileNotFoundException ex)
      {
        this.createConfigFile();
        this._configXmlDoc.Load(this._configFileName);
      }
    }

    public bool isEmpty()
    {
      XmlElement xmlElement = (XmlElement) this._configXmlDoc.SelectSingleNode("ConfigParams");
      return xmlElement == null || !xmlElement.HasChildNodes;
    }

    public int getIntParam(string parName, int defaultValue)
    {
      int num = defaultValue;
      string stringParam = this.getStringParam(parName, "x");
      if (stringParam != "x")
      {
        try
        {
          num = int.Parse(stringParam);
        }
        catch (Exception ex)
        {
          num = defaultValue;
        }
      }
      return num;
    }

    public byte setIntParam(string parName, int parValue)
    {
      return this.setStringParam(parName, parValue.ToString());
    }

    public string getStringParam(string parName, string defaultValue)
    {
      XmlElement xmlElement = (XmlElement) this._configXmlDoc.SelectSingleNode("ConfigParams//" + parName);
      return xmlElement == null ? defaultValue : xmlElement.InnerText;
    }

    public byte setStringParam(string parName, string parValue)
    {
      XmlElement xmlElement = (XmlElement) this._configXmlDoc.SelectSingleNode("ConfigParams//" + parName);
      if (xmlElement != null)
        xmlElement.InnerText = parValue;
      else
        this.createParam(parName, parValue);
      this._configXmlDoc.Save(this._configFileName);
      return 0;
    }

    private void createConfigFile()
    {
      XmlTextWriter xmlTextWriter = new XmlTextWriter(this._configFileName, (Encoding) null);
      xmlTextWriter.WriteStartDocument();
      xmlTextWriter.Formatting = Formatting.Indented;
      xmlTextWriter.WriteStartElement("ConfigParams");
      xmlTextWriter.WriteEndElement();
      xmlTextWriter.Close();
    }

    private void createParam(string parName, string parValue)
    {
      XmlElement xmlElement = (XmlElement) this._configXmlDoc.SelectSingleNode("ConfigParams");
      XmlElement element = this._configXmlDoc.CreateElement(parName);
      element.InnerText = parValue;
      xmlElement.AppendChild((XmlNode) element);
    }
  }
}
