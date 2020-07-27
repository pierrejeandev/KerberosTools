// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.PrincipalName
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Collections.Generic;

namespace MB.Authorization.Kerberos
{
  [Serializable]
  public class PrincipalName
  {
    public PrincipalNameType NameType;
    public List<string> NameString;

    public override string ToString()
    {
      if (this.NameString.Count == 2)
        return string.Format("{0}/{1}", (object) this.NameString[0], (object) this.NameString[1]);
      return this.NameString.Count == 3 ? string.Format("{0}/{1}:{2}", (object) this.NameString[0], (object) this.NameString[1], (object) this.NameString[2]) : "";
    }
  }
}
