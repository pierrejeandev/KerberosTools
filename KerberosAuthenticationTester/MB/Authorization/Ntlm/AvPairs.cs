// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Ntlm.AvPairs
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Text;

namespace MB.Authorization.Ntlm
{
  [Serializable]
  public class AvPairs
  {
    private byte[] _buffer;
    private int _index;
    public string MsvAvNbComputerName;
    public string MsvAvNbDomainName;
    public string MsvAvDnsComputerName;
    public string MsvAvDnsDomainName;
    public string MsvAvDnsTreeName;
    public uint? MsvAvFlags;
    public DateTime? MsvAvTimestamp;
    public RestrictionEncoding MsAvRestrictions;
    public string MsvAvTargetName;
    public byte[] MsvChannelBindings;

    public AvPairs()
    {
    }

    public AvPairs(byte[] Buffer)
    {
      this._buffer = Buffer;
      this.SetAvFields();
    }

    public AvPairs(byte[] Buffer, int Index)
    {
      this._buffer = Buffer;
      this._index = Index;
      this.SetAvFields();
    }

    private void SetAvFields()
    {
      AvType int16;
      do
      {
        int16 = (AvType) BitConverter.ToInt16(this._buffer, this._index);
        ushort uint16 = BitConverter.ToUInt16(this._buffer, this._index + 2);
        this._index += 4;
        switch (int16 - 1)
        {
          case AvType.MsvAvEOL:
            this.MsvAvNbComputerName = Encoding.Unicode.GetString(this._buffer, this._index, (int) uint16);
            break;
          case AvType.MsvAvNbComputerName:
            this.MsvAvNbDomainName = Encoding.Unicode.GetString(this._buffer, this._index, (int) uint16);
            break;
          case AvType.MsvAvNbDomainName:
            this.MsvAvDnsComputerName = Encoding.Unicode.GetString(this._buffer, this._index, (int) uint16);
            break;
          case AvType.MsvAvDnsComputerName:
            this.MsvAvDnsDomainName = Encoding.Unicode.GetString(this._buffer, this._index, (int) uint16);
            break;
          case AvType.MsvAvDnsDomainName:
            this.MsvAvDnsTreeName = Encoding.Unicode.GetString(this._buffer, this._index, (int) uint16);
            break;
          case AvType.MsvAvDnsTreeName:
            this.MsvAvFlags = new uint?(BitConverter.ToUInt32(this._buffer, this._index));
            break;
          case AvType.MsvAvFlags:
            if (uint16 == (ushort) 8)
            {
              this.MsvAvTimestamp = new DateTime?(DateTime.FromFileTime(BitConverter.ToInt64(this._buffer, this._index)));
              break;
            }
            break;
          case AvType.MsvAvTimestamp:
            byte[] buffer = new byte[(int) uint16];
            Buffer.BlockCopy((Array) this._buffer, this._index, (Array) buffer, 0, (int) uint16);
            this.MsAvRestrictions = new RestrictionEncoding(buffer);
            break;
          case AvType.MsAvRestrictions:
            this.MsvAvTargetName = Encoding.Unicode.GetString(this._buffer, this._index, (int) uint16);
            break;
          case AvType.MsvAvTargetName:
            byte[] numArray = new byte[(int) uint16];
            Buffer.BlockCopy((Array) this._buffer, this._index, (Array) numArray, 0, (int) uint16);
            this.MsvChannelBindings = numArray;
            break;
        }
        this._index += (int) uint16;
      }
      while (int16 != AvType.MsvAvEOL);
    }
  }
}
