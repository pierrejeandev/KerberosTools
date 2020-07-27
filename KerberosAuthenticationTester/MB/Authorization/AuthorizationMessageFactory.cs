// Decompiled with JetBrains decompiler
// Type: MB.Authorization.AuthorizationMessageFactory
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using MB.Authorization.Basic;
using MB.Authorization.Kerberos;
using MB.Authorization.Ntlm;
using System;
using System.Text;

namespace MB.Authorization
{
  public static class AuthorizationMessageFactory
  {
    public static AuthorizationMessage Build(string AuthorizationHeader)
    {
      string[] strArray = AuthorizationHeader.Split(' ');
      if (strArray.Length != 2)
        throw new Exception("Unexpected Authorization Header");
      AuthorizationType authorizationType = AuthorizationType.Unknown;
      AuthorizationMessage authorizationMessage = (AuthorizationMessage) null;
      try
      {
        switch (strArray[0].ToLower())
        {
          case "ntlm":
            authorizationType = AuthorizationType.Ntlm;
            authorizationMessage = (AuthorizationMessage) new NtlmAuthorizeMessage(strArray[1]);
            break;
          case "basic":
            authorizationType = AuthorizationType.Basic;
            authorizationMessage = (AuthorizationMessage) new BasicAuthorizationMessage(strArray[1]);
            break;
          case "negotiate":
            if (Encoding.ASCII.GetString(Convert.FromBase64String(strArray[1]), 0, 4) == "NTLM")
            {
              authorizationType = AuthorizationType.Ntlm;
              authorizationMessage = (AuthorizationMessage) new NtlmAuthorizeMessage(strArray[1]);
              break;
            }
            authorizationType = AuthorizationType.Kerberos;
            authorizationMessage = (AuthorizationMessage) new KerberosAuthorizeMessage(strArray[1]);
            break;
        }
      }
      catch (Exception ex)
      {
        authorizationMessage = (AuthorizationMessage) new UnknownAuthorizationMessage(strArray[1], authorizationType);
      }
      return authorizationMessage;
    }
  }
}
