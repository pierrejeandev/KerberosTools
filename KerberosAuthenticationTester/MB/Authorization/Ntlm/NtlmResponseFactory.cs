// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.NtlmResponseFactory
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  internal static class NtlmResponseFactory
  {
    public static NtlmResponse CreateNtlmResponse(byte[] Buffer)
    {
      return Buffer.Length == 24 ? (NtlmResponse) new NtlmV1Response(Buffer) : (NtlmResponse) new NtlmV2Response(Buffer);
    }
  }
}
