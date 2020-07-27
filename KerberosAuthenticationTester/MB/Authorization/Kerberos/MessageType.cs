// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.MessageType
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace MB.Authorization.Kerberos
{
  public enum MessageType
  {
    KRB_AS_REQ = 10, // 0x0000000A
    KRB_AS_REP = 11, // 0x0000000B
    KRB_TGS_REQ = 12, // 0x0000000C
    KRB_TGS_REP = 13, // 0x0000000D
    KRB_AP_REQ = 14, // 0x0000000E
    KRB_AP_REP = 15, // 0x0000000F
    KRB_RESERVED16 = 16, // 0x00000010
    KRB_RESERVED17 = 17, // 0x00000011
    KRB_SAFE = 20, // 0x00000014
    KRB_PRIV = 21, // 0x00000015
    KRB_CRED = 22, // 0x00000016
    KRB_ERROR = 30, // 0x0000001E
  }
}
