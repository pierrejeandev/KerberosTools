// Decompiled with JetBrains decompiler
// Type: LCLib.Asn1Processor.BinaryDump
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace LCLib.Asn1Processor
{
  public class BinaryDump
  {
    private int offsetWidth = 3;
    private int dataWidth = 16;
    private byte[] data;

    public byte[] Data
    {
      get
      {
        return this.data;
      }
      set
      {
        this.data = value;
      }
    }

    public int OffsetWidth
    {
      get
      {
        return this.offsetWidth;
      }
      set
      {
        this.offsetWidth = value;
      }
    }

    public int DataWidth
    {
      get
      {
        return this.dataWidth;
      }
      set
      {
        this.dataWidth = value;
      }
    }

    public static string Dump(byte[] data, int offsetWidth, int dataWidth)
    {
      string str = "";
      int num = 0;
      while (num < data.Length)
        ++num;
      return str;
    }
  }
}
