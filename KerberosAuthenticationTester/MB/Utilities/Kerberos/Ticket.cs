// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Kerberos.Ticket
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Utilities.Kerberos
{
  public class Ticket
  {
    public string RealmName { get; set; }

    public string ServerName { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime RenewTime { get; set; }

    public int EncryptionType { get; set; }

    public Ticket.KerbTicketFlags TicketFlags { get; set; }

    [Flags]
    public enum KerbTicketFlags : uint
    {
      Forwardable = 1073741824, // 0x40000000
      Forwarded = 536870912, // 0x20000000
      HWAuthent = 1048576, // 0x00100000
      Initial = 4194304, // 0x00400000
      Invalid = 16777216, // 0x01000000
      MayPostdate = 67108864, // 0x04000000
      OkAsDelegate = 262144, // 0x00040000
      Postdated = 33554432, // 0x02000000
      PreAuthent = 2097152, // 0x00200000
      Proxiable = 268435456, // 0x10000000
      Proxy = 134217728, // 0x08000000
      Renewable = 8388608, // 0x00800000
      Reserved = 2147483648, // 0x80000000
      Reserved1 = 1,
    }
  }
}
