// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Asn1Util
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using Microsoft.Win32;
using System;
using System.IO;

namespace LipingShare.LCLib.Asn1Processor
{
  public class Asn1Util
  {
    private static char[] hexDigits = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };
    private const string PemStartStr = "-----BEGIN";
    private const string PemEndStr = "-----END";

    public static bool IsAsn1EncodedHexStr(string dataStr)
    {
      bool flag = false;
      try
      {
        byte[] bytes = Asn1Util.HexStrToBytes(dataStr);
        if (bytes.Length > 0)
          flag = new Asn1Node().LoadData(bytes);
      }
      catch
      {
        flag = false;
      }
      return flag;
    }

    public static string FormatString(string inStr, int lineLen, int groupLen)
    {
      char[] chArray1 = new char[inStr.Length * 2];
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      for (int index1 = 0; index1 < inStr.Length; ++index1)
      {
        chArray1[num1++] = inStr[index1];
        ++num3;
        ++num2;
        if (num3 >= groupLen && groupLen > 0)
        {
          chArray1[num1++] = ' ';
          num3 = 0;
        }
        if (num2 >= lineLen)
        {
          char[] chArray2 = chArray1;
          int index2 = num1;
          int num4 = index2 + 1;
          chArray2[index2] = '\r';
          char[] chArray3 = chArray1;
          int index3 = num4;
          num1 = index3 + 1;
          chArray3[index3] = '\n';
          num2 = 0;
        }
      }
      return new string(chArray1).TrimEnd(new char[1]).TrimEnd('\n').TrimEnd('\r');
    }

    public static string GenStr(int len, char xch)
    {
      char[] chArray = new char[len];
      for (int index = 0; index < len; ++index)
        chArray[index] = xch;
      return new string(chArray);
    }

    public static long BytesToLong(byte[] bytes)
    {
      long num = 0;
      for (int index = 0; index < bytes.Length; ++index)
        num = num << 8 | (long) bytes[index];
      return num;
    }

    public static string BytesToString(byte[] bytes)
    {
      string str = "";
      if (bytes == null || bytes.Length < 1)
        return str;
      char[] chArray = new char[bytes.Length];
      int index = 0;
      int num = 0;
      for (; index < bytes.Length; ++index)
      {
        if (bytes[index] != (byte) 0)
          chArray[num++] = (char) bytes[index];
      }
      return new string(chArray).TrimEnd(new char[1]);
    }

    public static byte[] StringToBytes(string msg)
    {
      byte[] numArray = new byte[msg.Length];
      for (int index = 0; index < msg.Length; ++index)
        numArray[index] = (byte) msg[index];
      return numArray;
    }

    public static bool IsEqual(byte[] source, byte[] target)
    {
      if (source == null || target == null || source.Length != target.Length)
        return false;
      for (int index = 0; index < source.Length; ++index)
      {
        if ((int) source[index] != (int) target[index])
          return false;
      }
      return true;
    }

    public static string ToHexString(byte[] bytes)
    {
      if (bytes == null)
        return "";
      char[] chArray = new char[bytes.Length * 2];
      for (int index = 0; index < bytes.Length; ++index)
      {
        int num = (int) bytes[index];
        chArray[index * 2] = Asn1Util.hexDigits[num >> 4];
        chArray[index * 2 + 1] = Asn1Util.hexDigits[num & 15];
      }
      return new string(chArray);
    }

    public static bool IsValidHexDigits(char ch)
    {
      bool flag = false;
      for (int index = 0; index < Asn1Util.hexDigits.Length; ++index)
      {
        if ((int) Asn1Util.hexDigits[index] == (int) ch)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public static byte GetHexDigitsVal(char ch)
    {
      byte num = 0;
      for (int index = 0; index < Asn1Util.hexDigits.Length; ++index)
      {
        if ((int) Asn1Util.hexDigits[index] == (int) ch)
        {
          num = (byte) index;
          break;
        }
      }
      return num;
    }

    public static byte[] HexStrToBytes(string hexStr)
    {
      hexStr = hexStr.Replace(" ", "");
      hexStr = hexStr.Replace("\r", "");
      hexStr = hexStr.Replace("\n", "");
      hexStr = hexStr.ToUpper();
      if (hexStr.Length % 2 != 0)
        throw new Exception("Invalid Hex string: odd length.");
      for (int index = 0; index < hexStr.Length; ++index)
      {
        if (!Asn1Util.IsValidHexDigits(hexStr[index]))
          throw new Exception("Invalid Hex string: included invalid character [" + (object) hexStr[index] + "]");
      }
      int length = hexStr.Length / 2;
      byte[] numArray = new byte[length];
      for (int index = 0; index < length; ++index)
      {
        int num = (int) Asn1Util.GetHexDigitsVal(hexStr[index * 2]) << 4 | (int) Asn1Util.GetHexDigitsVal(hexStr[index * 2 + 1]);
        numArray[index] = (byte) num;
      }
      return numArray;
    }

    public static bool IsHexStr(string hexStr)
    {
      byte[] bytes;
      try
      {
        bytes = Asn1Util.HexStrToBytes(hexStr);
      }
      catch
      {
        return false;
      }
      return bytes != null && bytes.Length >= 0;
    }

    public static bool IsPemFormated(string pemStr)
    {
      byte[] bytes;
      try
      {
        bytes = Asn1Util.PemToBytes(pemStr);
      }
      catch
      {
        return false;
      }
      return bytes.Length > 0;
    }

    public static bool IsPemFormatedFile(string fileName)
    {
      try
      {
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        return Asn1Util.IsPemFormated(Asn1Util.BytesToString(numArray));
      }
      catch
      {
        return false;
      }
    }

    public static Stream PemToStream(string pemStr)
    {
      MemoryStream memoryStream = new MemoryStream(Asn1Util.PemToBytes(pemStr));
      memoryStream.Position = 0L;
      return (Stream) memoryStream;
    }

    public static byte[] PemToBytes(string pemStr)
    {
      string[] strArray = pemStr.Split('\n');
      string str = "";
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string upper = strArray[index].ToUpper();
        if (!(upper == ""))
        {
          if (upper.Length > "-----BEGIN".Length && !flag1 && upper.Substring(0, "-----BEGIN".Length) == "-----BEGIN")
          {
            flag1 = true;
          }
          else
          {
            if (upper.Length > "-----END".Length && upper.Substring(0, "-----END".Length) == "-----END")
            {
              flag2 = true;
              break;
            }
            if (flag1)
              str += strArray[index];
          }
        }
      }
      if (!flag1 || !flag2)
        throw new Exception("'BEGIN'/'END' line is missing.");
      return Convert.FromBase64String(str.Replace("\r", "").Replace("\n", "").Replace("\n", " "));
    }

    public static string BytesToPem(byte[] data)
    {
      return Asn1Util.BytesToPem(data, "");
    }

    public static string GetPemFileHeader(string fileName)
    {
      try
      {
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        return Asn1Util.GetPemHeader(Asn1Util.BytesToString(numArray));
      }
      catch
      {
        return "";
      }
    }

    public static string GetPemHeader(string pemStr)
    {
      string[] strArray = pemStr.Split('\n');
      bool flag = false;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = strArray[index].ToUpper().Replace("\r", "");
        if (!(str == "") && str.Length > "-----BEGIN".Length && (!flag && str.Substring(0, "-----BEGIN".Length) == "-----BEGIN"))
          return strArray[index].Substring("-----BEGIN".Length, strArray[index].Length - "-----BEGIN".Length).Replace("-----", "").Replace("\r", "");
      }
      return "";
    }

    public static string BytesToPem(byte[] data, string pemHeader)
    {
      if (pemHeader == null || pemHeader.Length < 1)
        pemHeader = "ASN.1 Editor Generated PEM File";
      if (pemHeader.Length > 0 && pemHeader[0] != ' ')
        pemHeader = " " + pemHeader;
      string str = Asn1Util.FormatString(Convert.ToBase64String(data), 64, 0);
      return "-----BEGIN" + pemHeader + "-----\r\n" + str + "\r\n-----END" + pemHeader + "-----\r\n";
    }

    public static int BitPrecision(ulong ivalue)
    {
      if (ivalue == 0UL)
        return 0;
      int num1 = 0;
      int num2 = 32;
      while (num2 - num1 > 1)
      {
        int num3 = (num1 + num2) / 2;
        if (ivalue >> num3 != 0UL)
          num1 = num3;
        else
          num2 = num3;
      }
      return num2;
    }

    public static int BytePrecision(ulong value)
    {
      int num = 4;
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

    public static long DerLengthDecode(Stream bt, ref bool isIndefiniteLength)
    {
      isIndefiniteLength = false;
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
        {
          isIndefiniteLength = true;
          long position = bt.Position;
          return -2;
        }
        num2 = 0L;
        while (num3-- > 0L)
        {
          if (num2 >> 24 > 0L)
            return -1;
          byte num4 = (byte) bt.ReadByte();
          num2 = num2 << 8 | (long) num4;
        }
      }
      return num2;
    }

    public static string GetTagName(byte tag)
    {
      string str = "";
      if (((int) tag & 192) != 0)
      {
        switch ((int) tag & 192)
        {
          case 0:
            str = str + "UNIVERSAL (" + ((int) tag & 31).ToString() + ")";
            break;
          case 32:
            str = str + "CONSTRUCTED (" + ((int) tag & 31).ToString() + ")";
            break;
          case 64:
            str = str + "APPLICATION (" + ((int) tag & 31).ToString() + ")";
            break;
          case 128:
            str = str + "CONTEXT SPECIFIC (" + ((int) tag & 31).ToString() + ")";
            break;
          case 192:
            str = str + "PRIVATE (" + ((int) tag & 31).ToString() + ")";
            break;
        }
      }
      else
      {
        switch ((int) tag & 31)
        {
          case 1:
            str += "BOOLEAN";
            break;
          case 2:
            str += "INTEGER";
            break;
          case 3:
            str += "BIT STRING";
            break;
          case 4:
            str += "OCTET STRING";
            break;
          case 5:
            str += "NULL";
            break;
          case 6:
            str += "OBJECT IDENTIFIER";
            break;
          case 7:
            str += "OBJECT DESCRIPTOR";
            break;
          case 8:
            str += "EXTERNAL";
            break;
          case 9:
            str += "REAL";
            break;
          case 10:
            str += "ENUMERATED";
            break;
          case 12:
            str += "UTF8 STRING";
            break;
          case 13:
            str += "RELATIVE-OID";
            break;
          case 16:
            str += "SEQUENCE";
            break;
          case 17:
            str += "SET";
            break;
          case 18:
            str += "NUMERIC STRING";
            break;
          case 19:
            str += "PRINTABLE STRING";
            break;
          case 20:
            str += "T61 STRING";
            break;
          case 21:
            str += "VIDEOTEXT STRING";
            break;
          case 22:
            str += "IA5 STRING";
            break;
          case 23:
            str += "UTC TIME";
            break;
          case 24:
            str += "GENERALIZED TIME";
            break;
          case 25:
            str += "GRAPHIC STRING";
            break;
          case 26:
            str += "VISIBLE STRING";
            break;
          case 27:
            str += "GENERAL STRING";
            break;
          case 28:
            str += "UNIVERSAL STRING";
            break;
          case 30:
            str += "BMP STRING";
            break;
          default:
            str += "UNKNOWN TAG";
            break;
        }
      }
      return str;
    }

    public static object ReadRegInfo(string path, string name)
    {
      object obj = (object) null;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(path, false);
      if (registryKey != null)
        obj = registryKey.GetValue(name);
      return obj;
    }

    public static void WriteRegInfo(string path, string name, object data)
    {
      (Registry.CurrentUser.OpenSubKey(path, true) ?? Registry.CurrentUser.CreateSubKey(path))?.SetValue(name, data);
    }

    public static bool IsAsciiString(byte[] data)
    {
      bool flag = true;
      for (int index = 0; index < data.Length; ++index)
      {
        if (data[index] < (byte) 32)
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    private Asn1Util()
    {
    }
  }
}
