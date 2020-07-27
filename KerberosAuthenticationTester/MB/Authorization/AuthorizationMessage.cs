// Decompiled with JetBrains decompiler
// Type: MB.Authorization.AuthorizationMessage
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using MB.Authorization.Basic;
using MB.Authorization.Kerberos;
using MB.Authorization.Ntlm;
using System;
using System.Xml.Serialization;

namespace MB.Authorization
{
  [XmlInclude(typeof (BasicAuthorizationMessage))]
  [XmlInclude(typeof (KerberosAuthorizeMessage))]
  [XmlInclude(typeof (NtlmAuthorizeMessage))]
  [XmlInclude(typeof (UnknownAuthorizationMessage))]
  [Serializable]
  public abstract class AuthorizationMessage
  {
    [XmlIgnore]
    public abstract AuthorizationType AuthorizationType { get; }

    public AuthorizationMessage()
    {
    }

    public AuthorizationMessage(string base64EncodedMessage)
    {
      this.InitMessageFromData(Convert.FromBase64String(base64EncodedMessage));
    }

    protected abstract void InitMessageFromData(byte[] data);
  }
}
