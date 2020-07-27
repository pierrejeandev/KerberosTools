// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.KerberosAuthorizeMessage
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Xml.Serialization;

namespace MB.Authorization.Kerberos
{
  [Serializable]
  public class KerberosAuthorizeMessage : AuthorizationMessage
  {
    public MechType MechType;
    public NegotiationToken NegotiationToken;

    public KerberosAuthorizeMessage()
    {
    }

    public KerberosAuthorizeMessage(string base64EncodedMessage)
      : base(base64EncodedMessage)
    {
    }

    [XmlIgnore]
    public override AuthorizationType AuthorizationType
    {
      get
      {
        return AuthorizationType.Kerberos;
      }
    }

    protected override void InitMessageFromData(byte[] data)
    {
      KerberosAuthorizeMessageBuilder.Build(this, data);
    }
  }
}
