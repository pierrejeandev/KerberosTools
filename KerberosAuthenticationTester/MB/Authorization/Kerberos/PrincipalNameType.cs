// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.PrincipalNameType
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace MB.Authorization.Kerberos
{
  public enum PrincipalNameType
  {
    NT_UNKNOWN = 0,
    NT_PRINCIPAL = 1,
    NT_SRV_INST = 2,
    NT_SRV_HST = 3,
    NT_SRV_XHST = 4,
    NT_UID = 5,
    NT_X500_PRINCIPAL = 6,
    NT_SMTP_NAME = 7,
    NT_ENTERPRISE = 10, // 0x0000000A
  }
}
