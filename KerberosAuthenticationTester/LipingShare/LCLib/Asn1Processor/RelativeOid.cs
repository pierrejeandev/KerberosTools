// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.RelativeOid
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.IO;

namespace LipingShare.LCLib.Asn1Processor
{
  public class RelativeOid : Oid
  {
    public override void Encode(Stream bt, string oidStr)
    {
      string[] strArray = oidStr.Split('.');
      ulong[] numArray = new ulong[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        numArray[index] = Convert.ToUInt64(strArray[index]);
      for (int index = 0; index < numArray.Length; ++index)
        this.EncodeValue(bt, numArray[index]);
    }

    public override string Decode(Stream bt)
    {
      string str = "";
      ulong v = 0;
      bool flag = true;
      while (bt.Position < bt.Length)
      {
        try
        {
          this.DecodeValue(bt, ref v);
          if (flag)
          {
            str = v.ToString();
            flag = false;
          }
          else
            str = str + "." + v.ToString();
        }
        catch (Exception ex)
        {
          throw new Exception("Failed to decode OID value: " + ex.Message);
        }
      }
      return str;
    }
  }
}
