// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.APOptions
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Kerberos
{
  [Flags]
  public enum APOptions : uint
  {
    USE_SESSION_KEY = 1073741824, // 0x40000000
    MUTUAL_REQUIRED = 536870912, // 0x20000000
  }
}
