// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Asn1Tag
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace LipingShare.LCLib.Asn1Processor
{
  public class Asn1Tag
  {
    public const byte TAG_MASK = 31;
    public const byte BOOLEAN = 1;
    public const byte INTEGER = 2;
    public const byte BIT_STRING = 3;
    public const byte OCTET_STRING = 4;
    public const byte TAG_NULL = 5;
    public const byte OBJECT_IDENTIFIER = 6;
    public const byte OBJECT_DESCRIPTOR = 7;
    public const byte EXTERNAL = 8;
    public const byte REAL = 9;
    public const byte ENUMERATED = 10;
    public const byte UTF8_STRING = 12;
    public const byte RELATIVE_OID = 13;
    public const byte SEQUENCE = 16;
    public const byte SET = 17;
    public const byte NUMERIC_STRING = 18;
    public const byte PRINTABLE_STRING = 19;
    public const byte T61_STRING = 20;
    public const byte VIDEOTEXT_STRING = 21;
    public const byte IA5_STRING = 22;
    public const byte UTC_TIME = 23;
    public const byte GENERALIZED_TIME = 24;
    public const byte GRAPHIC_STRING = 25;
    public const byte VISIBLE_STRING = 26;
    public const byte GENERAL_STRING = 27;
    public const byte UNIVERSAL_STRING = 28;
    public const byte BMPSTRING = 30;
  }
}
