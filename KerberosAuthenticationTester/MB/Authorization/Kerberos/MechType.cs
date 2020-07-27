// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.MechType
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Kerberos
{
  [Serializable]
  public class MechType
  {
    public string OID;
    public string Mechanism;

    public MechType()
    {
    }

    public MechType(string OID)
    {
      this.OID = OID;
      this.Mechanism = MechType.LookupMechanism(OID);
    }

    private static string LookupMechanism(string OID)
    {
      switch (OID)
      {
        case "1.3.6.1.5.5.2":
          return "SPNEGO pseudo-mechanism (RFC2478)";
        case "1.2.840.48018.1.2.2":
          return "Kerberos v5 Legacy";
        case "1.2.840.113554.1.2.2":
          return "Kerberos v5";
        case "1.3.6.1.4.1.311.2.2.10":
          return "NTLMSSP - Microsoft NTLM Security Support Provider";
        default:
          return "";
      }
    }
  }
}
