// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.NtlmV2Response
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  public class NtlmV2Response : NtlmResponse
  {
    public NtlmV2ClientChallenge NtlmV2ClientChallenge;

    public NtlmV2Response()
    {
    }

    public NtlmV2Response(byte[] ResponseBuffer)
      : base(ResponseBuffer)
    {
      this.SetValues();
    }

    private void SetValues()
    {
      this.Version = 2;
      this.SetResponse();
      this.NtlmV2ClientChallenge = new NtlmV2ClientChallenge(this._buffer, 16);
    }

    private void SetResponse()
    {
      this.Response = new byte[16];
      Buffer.BlockCopy((Array) this._buffer, 0, (Array) this.Response, 0, 16);
    }
  }
}
