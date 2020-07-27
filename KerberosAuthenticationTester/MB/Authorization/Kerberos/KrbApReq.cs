// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.KrbApReq
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Kerberos
{
  [Serializable]
  public class KrbApReq
  {
    public int ProtocolVersionNumber;
    public MessageType MessageType;
    public APOptions APOptions;
    public Ticket Ticket;
    public EncryptedData Authenticator;
  }
}
