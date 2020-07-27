// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.RestrictionEncoding
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  public class RestrictionEncoding
  {
    private byte[] _buffer;
    public uint Size;
    public uint Z4;
    public uint IntegrityLevel;
    public uint SubjectIntegrityLevel;
    public byte[] MachineID;

    public RestrictionEncoding()
    {
    }

    public RestrictionEncoding(byte[] Buffer)
    {
      this._buffer = Buffer;
      this.SetValues();
    }

    private void SetValues()
    {
      this.Size = BitConverter.ToUInt32(this._buffer, 0);
      this.Z4 = BitConverter.ToUInt32(this._buffer, 4);
      this.IntegrityLevel = BitConverter.ToUInt32(this._buffer, 8);
      this.SubjectIntegrityLevel = BitConverter.ToUInt32(this._buffer, 12);
      this.SetMachineID();
    }

    private void SetMachineID()
    {
      this.MachineID = new byte[32];
      Buffer.BlockCopy((Array) this._buffer, 16, (Array) this.MachineID, 0, 32);
    }
  }
}
