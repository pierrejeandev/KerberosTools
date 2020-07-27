// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Oid
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.IO;

namespace LipingShare.LCLib.Asn1Processor
{
  public class Oid
  {
    public string GetOidName(string inOidStr)
    {
      return "";
    }

    public byte[] Encode(string oidStr)
    {
      MemoryStream memoryStream = new MemoryStream();
      this.Encode((Stream) memoryStream, oidStr);
      memoryStream.Position = 0L;
      byte[] buffer = new byte[memoryStream.Length];
      memoryStream.Read(buffer, 0, buffer.Length);
      memoryStream.Close();
      return buffer;
    }

    public string Decode(byte[] data)
    {
      MemoryStream memoryStream = new MemoryStream(data);
      memoryStream.Position = 0L;
      string str = this.Decode((Stream) memoryStream);
      memoryStream.Close();
      return str;
    }

    public virtual void Encode(Stream bt, string oidStr)
    {
      string[] strArray = oidStr.Split('.');
      if (strArray.Length < 2)
        throw new Exception("Invalid OID string.");
      ulong[] numArray = new ulong[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        numArray[index] = Convert.ToUInt64(strArray[index]);
      bt.WriteByte((byte) (numArray[0] * 40UL + numArray[1]));
      for (int index = 2; index < numArray.Length; ++index)
        this.EncodeValue(bt, numArray[index]);
    }

    public virtual string Decode(Stream bt)
    {
      string str1 = "";
      ulong v = 0;
      byte num = (byte) bt.ReadByte();
      string str2 = str1 + Convert.ToString((int) num / 40) + "." + Convert.ToString((int) num % 40);
      while (bt.Position < bt.Length)
      {
        try
        {
          this.DecodeValue(bt, ref v);
          str2 = str2 + "." + v.ToString();
        }
        catch (Exception ex)
        {
          throw new Exception("Failed to decode OID value: " + ex.Message);
        }
      }
      return str2;
    }

    protected void EncodeValue(Stream bt, ulong v)
    {
      for (int index = (Asn1Util.BitPrecision(v) - 1) / 7; index > 0; --index)
        bt.WriteByte((byte) (128UL | v >> index * 7 & (ulong) sbyte.MaxValue));
      bt.WriteByte((byte) (v & (ulong) sbyte.MaxValue));
    }

    protected int DecodeValue(Stream bt, ref ulong v)
    {
      int num1 = 0;
      v = 0UL;
      byte num2;
      do
      {
        num2 = (byte) bt.ReadByte();
        ++num1;
        v <<= 7;
        v += (ulong) ((int) num2 & (int) sbyte.MaxValue);
      }
      while (((int) num2 & 128) != 0);
      return num1;
    }
  }
}
