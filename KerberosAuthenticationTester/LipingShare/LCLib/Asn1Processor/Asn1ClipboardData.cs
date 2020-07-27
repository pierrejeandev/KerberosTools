// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Asn1ClipboardData
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.IO;
using System.Windows.Forms;

namespace LipingShare.LCLib.Asn1Processor
{
  [Serializable]
  public class Asn1ClipboardData
  {
    private static string asn1FormatName = "Asn1NodeDataFormat";

    public static void Copy(Asn1Node node)
    {
      DataFormats.GetFormat(Asn1ClipboardData.asn1FormatName);
      MemoryStream memoryStream = new MemoryStream();
      node.SaveData((Stream) memoryStream);
      memoryStream.Position = 0L;
      byte[] numArray = new byte[memoryStream.Length];
      memoryStream.Read(numArray, 0, (int) memoryStream.Length);
      memoryStream.Close();
      DataObject dataObject = new DataObject();
      dataObject.SetData(Asn1ClipboardData.asn1FormatName, (object) numArray);
      dataObject.SetData(DataFormats.Text, (object) Asn1Util.FormatString(Asn1Util.ToHexString(numArray), 32, 2));
      Clipboard.SetDataObject((object) dataObject, true);
    }

    public static Asn1Node Paste()
    {
      DataFormats.GetFormat(Asn1ClipboardData.asn1FormatName);
      Asn1Node asn1Node1 = new Asn1Node();
      IDataObject dataObject = Clipboard.GetDataObject();
      byte[] data1 = (byte[]) dataObject.GetData(Asn1ClipboardData.asn1FormatName);
      if (data1 != null)
      {
        MemoryStream memoryStream = new MemoryStream(data1);
        memoryStream.Position = 0L;
        asn1Node1.LoadData((Stream) memoryStream);
      }
      else
      {
        string data2 = (string) dataObject.GetData(DataFormats.Text);
        Asn1Node asn1Node2 = new Asn1Node();
        if (Asn1Util.IsAsn1EncodedHexStr(data2))
        {
          byte[] bytes = Asn1Util.HexStrToBytes(data2);
          asn1Node1.LoadData(bytes);
        }
      }
      return asn1Node1;
    }

    public static bool IsDataReady()
    {
      bool flag = false;
      try
      {
        IDataObject dataObject = Clipboard.GetDataObject();
        if ((byte[]) dataObject.GetData(Asn1ClipboardData.asn1FormatName) != null)
          flag = true;
        else if (Asn1Util.IsAsn1EncodedHexStr((string) dataObject.GetData(DataFormats.Text)))
          flag = true;
      }
      catch
      {
        flag = false;
      }
      return flag;
    }
  }
}
