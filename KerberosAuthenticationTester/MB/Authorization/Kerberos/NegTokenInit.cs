// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.NegTokenInit
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Collections.Generic;

namespace MB.Authorization.Kerberos
{
  [Serializable]
  public class NegTokenInit : NegotiationToken
  {
    public List<MechType> MechTypes;
    public InitialContextToken MechToken;
  }
}
