// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.NtlmAuthorizeMessage
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Text;
using System.Xml.Serialization;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  public class NtlmAuthorizeMessage : AuthorizationMessage
  {
    private byte[] _messageData;
    public string Signature;
    public int MessageType;
    public byte[] LmChallengeResponse;
    public NtlmResponse NtChallengeResponse;
    public string DomainName;
    public string UserName;
    public string Workstation;
    public byte[] EncryptedRandomSessionKey;
    public NegotiateFlags NegotiateFlags;
    public Version Version;
    public byte[] Mic;

    public NtlmAuthorizeMessage()
    {
    }

    public NtlmAuthorizeMessage(string base64EncodedMessage)
      : base(base64EncodedMessage)
    {
    }

    protected override void InitMessageFromData(byte[] data)
    {
      this._messageData = data;
      this.Signature = Encoding.ASCII.GetString(this._messageData, 0, 7);
      this.MessageType = BitConverter.ToInt32(this._messageData, 8);
      this.SetLmChallengeResponse();
      this.SetNtChallengeResponse();
      this.DomainName = this.ConvertNtlmMessageByteArrayToString(28);
      this.UserName = this.ConvertNtlmMessageByteArrayToString(36);
      this.Workstation = this.ConvertNtlmMessageByteArrayToString(44);
      this.SetEncryptedRandomSessionKey();
      this.NegotiateFlags = (NegotiateFlags) BitConverter.ToUInt32(this._messageData, 60);
      this.SetVersion();
      this.SetMic();
    }

    [XmlIgnore]
    public override AuthorizationType AuthorizationType
    {
      get
      {
        return AuthorizationType.Ntlm;
      }
    }

    private void SetLmChallengeResponse()
    {
      short int16 = BitConverter.ToInt16(this._messageData, 12);
      int int32 = BitConverter.ToInt32(this._messageData, 16);
      if (int16 > (short) 0 && int32 > 0)
      {
        this.LmChallengeResponse = new byte[(int) int16];
        Buffer.BlockCopy((Array) this._messageData, int32, (Array) this.LmChallengeResponse, 0, (int) int16);
      }
      else
        this.LmChallengeResponse = new byte[0];
    }

    private void SetNtChallengeResponse()
    {
      short int16 = BitConverter.ToInt16(this._messageData, 20);
      int int32 = BitConverter.ToInt32(this._messageData, 24);
      if (int16 > (short) 0)
      {
        byte[] Buffer = new byte[(int) int16];
        Buffer.BlockCopy((Array) this._messageData, int32, (Array) Buffer, 0, (int) int16);
        this.NtChallengeResponse = NtlmResponseFactory.CreateNtlmResponse(Buffer);
      }
      else
        this.NtChallengeResponse = (NtlmResponse) null;
    }

    private string ConvertNtlmMessageByteArrayToString(int Index)
    {
      short int16 = BitConverter.ToInt16(this._messageData, Index);
      int int32 = BitConverter.ToInt32(this._messageData, Index + 4);
      return int16 > (short) 0 ? Encoding.Unicode.GetString(this._messageData, int32, (int) int16) : "";
    }

    private void SetEncryptedRandomSessionKey()
    {
      short int16 = BitConverter.ToInt16(this._messageData, 52);
      int int32 = BitConverter.ToInt32(this._messageData, 56);
      if (int16 > (short) 0)
      {
        this.EncryptedRandomSessionKey = new byte[(int) int16];
        Buffer.BlockCopy((Array) this._messageData, int32, (Array) this.EncryptedRandomSessionKey, 0, (int) int16);
      }
      else
        this.EncryptedRandomSessionKey = new byte[0];
    }

    private void SetVersion()
    {
      this.Version = new Version();
      this.Version.ProductMajorVersion = this._messageData[64];
      this.Version.ProductMinorVersion = this._messageData[65];
      this.Version.ProductBuild = BitConverter.ToUInt16(this._messageData, 66);
      this.Version.NTLMRevisionCurrent = this._messageData[71];
    }

    private void SetMic()
    {
      this.Mic = new byte[16];
      Buffer.BlockCopy((Array) this._messageData, 72, (Array) this.Mic, 0, 16);
    }
  }
}
