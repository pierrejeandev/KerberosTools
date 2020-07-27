// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Basic.BasicAuthorizationMessage
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Text;
using System.Xml.Serialization;

namespace MB.Authorization.Basic
{
  [Serializable]
  public class BasicAuthorizationMessage : AuthorizationMessage
  {
    public string UserName;
    [XmlIgnore]
    public string Password;

    public override AuthorizationType AuthorizationType
    {
      get
      {
        return AuthorizationType.Basic;
      }
    }

    public BasicAuthorizationMessage()
    {
    }

    public BasicAuthorizationMessage(string base64EncodedMessage)
      : base(base64EncodedMessage)
    {
    }

    protected override void InitMessageFromData(byte[] data)
    {
      string str = Encoding.ASCII.GetString(data);
      int length = str.IndexOf(':');
      this.UserName = str.Substring(0, length);
      this.Password = str.Substring(length + 1);
    }
  }
}
