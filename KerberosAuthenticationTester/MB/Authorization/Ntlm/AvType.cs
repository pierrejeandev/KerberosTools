// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.AvType
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  public enum AvType
  {
    MsvAvEOL,
    MsvAvNbComputerName,
    MsvAvNbDomainName,
    MsvAvDnsComputerName,
    MsvAvDnsDomainName,
    MsvAvDnsTreeName,
    MsvAvFlags,
    MsvAvTimestamp,
    MsAvRestrictions,
    MsvAvTargetName,
    MsvChannelBindings,
  }
}
