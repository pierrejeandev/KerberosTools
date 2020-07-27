// Decompiled with JetBrains decompiler
// Type: LCLib.Asn1Processor.Asn1Util
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.IO;

namespace LCLib.Asn1Processor
{
  public class Asn1Util
  {
    public static int BytePrecision(ulong value)
    {
      int num = 8;
      while (num > 0 && value >> (num - 1) * 8 == 0UL)
        --num;
      return num;
    }

    public static int DERLengthEncode(Stream xdata, ulong length)
    {
      int num1 = 0;
      int num2;
      if (length <= (ulong) sbyte.MaxValue)
      {
        xdata.WriteByte((byte) length);
        num2 = num1 + 1;
      }
      else
      {
        xdata.WriteByte((byte) (Asn1Util.BytePrecision(length) | 128));
        num2 = num1 + 1;
        for (int index = Asn1Util.BytePrecision(length); index > 0; --index)
        {
          xdata.WriteByte((byte) (length >> (index - 1) * 8));
          ++num2;
        }
      }
      return num2;
    }

    public static long DerLengthDecode(Stream bt)
    {
      byte num1 = (byte) bt.ReadByte();
      long num2;
      if (((int) num1 & 128) == 0)
      {
        num2 = (long) num1;
      }
      else
      {
        long num3 = (long) ((int) num1 & (int) sbyte.MaxValue);
        if (num3 == 0L)
          throw new Exception("Indefinite length.");
        num2 = 0L;
        while (num3-- > 0L)
        {
          if (num2 >> 56 > 0L)
            throw new Exception("Length overflow.");
          byte num4 = (byte) bt.ReadByte();
          num2 = num2 << 8 | (long) num4;
        }
      }
      return num2;
    }

    private Asn1Util()
    {
    }
  }
}
