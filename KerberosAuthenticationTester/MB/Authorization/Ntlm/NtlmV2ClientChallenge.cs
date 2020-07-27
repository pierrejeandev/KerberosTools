// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.NtlmV2ClientChallenge
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  public class NtlmV2ClientChallenge
  {
    private byte[] _buffer;
    private int _index;
    public byte ResponseType;
    public byte HiResponseType;
    public ushort Reserved1;
    public uint Reserved2;
    public DateTime TimeStamp;
    public byte[] ChallengeFromClient;
    public uint Reserved3;
    public AvPairs AvPairs;

    public NtlmV2ClientChallenge()
    {
    }

    public NtlmV2ClientChallenge(byte[] Buffer, int Index)
    {
      this._buffer = Buffer;
      this._index = Index;
      this.SetValues();
    }

    private void SetValues()
    {
      this.ResponseType = this._buffer[this._index];
      this.HiResponseType = this._buffer[this._index + 1];
      this.Reserved1 = BitConverter.ToUInt16(this._buffer, this._index + 2);
      this.Reserved2 = BitConverter.ToUInt32(this._buffer, this._index + 4);
      this.SetTimeStamp();
      this.SetChallengeFromClient();
      this.Reserved3 = BitConverter.ToUInt32(this._buffer, this._index + 24);
      this.AvPairs = new AvPairs(this._buffer, this._index + 28);
    }

    private void SetTimeStamp()
    {
      this.TimeStamp = DateTime.FromFileTime(BitConverter.ToInt64(this._buffer, this._index + 8));
    }

    private void SetChallengeFromClient()
    {
      this.ChallengeFromClient = new byte[8];
      Buffer.BlockCopy((Array) this._buffer, this._index + 16, (Array) this.ChallengeFromClient, 0, 8);
    }
  }
}
