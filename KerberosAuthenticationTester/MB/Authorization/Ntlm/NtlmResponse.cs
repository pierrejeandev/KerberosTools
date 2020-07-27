// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.NtlmResponse
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Xml.Serialization;

namespace MB.Authorization.Ntlm
{
  [XmlInclude(typeof (NtlmV2Response))]
  [XmlInclude(typeof (NtlmV1Response))]
  [Serializable]
  public abstract class NtlmResponse
  {
    protected byte[] _buffer;
    public int Version;
    public byte[] Response;

    public NtlmResponse()
    {
    }

    public NtlmResponse(byte[] ResponseBuffer)
    {
      this._buffer = ResponseBuffer;
    }
  }
}
