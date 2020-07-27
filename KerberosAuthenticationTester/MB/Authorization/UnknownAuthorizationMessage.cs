// Decompiled with JetBrains decompiler
// Type: MB.Authorization.UnknownAuthorizationMessage
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace MB.Authorization
{
  public class UnknownAuthorizationMessage : AuthorizationMessage
  {
    private AuthorizationType _authorizationType;

    public UnknownAuthorizationMessage()
    {
    }

    public UnknownAuthorizationMessage(
      string base64EncodedMessage,
      AuthorizationType authorizationType)
      : base(base64EncodedMessage)
    {
      this._authorizationType = authorizationType;
    }

    public override AuthorizationType AuthorizationType
    {
      get
      {
        return this._authorizationType;
      }
    }

    protected override void InitMessageFromData(byte[] data)
    {
    }
  }
}
