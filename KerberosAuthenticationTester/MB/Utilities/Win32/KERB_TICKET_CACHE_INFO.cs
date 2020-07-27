// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Win32.KERB_TICKET_CACHE_INFO
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System.Runtime.InteropServices;

namespace MB.Utilities.Win32
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct KERB_TICKET_CACHE_INFO
  {
    public UNICODE_STRING ServerName;
    public UNICODE_STRING RealmName;
    public ulong StartTime;
    public ulong EndTime;
    public ulong RenewTime;
    public int EncryptionType;
    public uint TicketFlags;
  }
}
