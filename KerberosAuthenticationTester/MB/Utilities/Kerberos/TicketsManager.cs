// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Kerberos.TicketsManager
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using MB.Utilities.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MB.Utilities.Kerberos
{
  public class TicketsManager : IDisposable
  {
    private IntPtr _lsaHandle = IntPtr.Zero;
    private IntPtr _kerberosPackageId;

    public TicketsManager()
    {
      WinStatusCodes winStatusCodes1 = Lsa.LsaConnectUntrusted(out this._lsaHandle);
      if (winStatusCodes1 != WinStatusCodes.STATUS_SUCCESS)
        throw new Exception("LsaConnectUntrusted failed with NTSTATUS code: " + (object) winStatusCodes1 + " (0x" + winStatusCodes1.ToString("x8") + ")");
      WinStatusCodes winStatusCodes2 = Lsa.LsaLookupAuthenticationPackage(this._lsaHandle, ref new LsaStringWrapper("Kerberos")._string, out this._kerberosPackageId);
      if (winStatusCodes2 != WinStatusCodes.STATUS_SUCCESS)
        throw new Exception("LsaLookupAuthenticationPackage failed with NTSTATUS code: " + (object) winStatusCodes2 + " (0x" + winStatusCodes2.ToString("x") + ")");
    }

    public List<Ticket> GetTickets()
    {
      List<Ticket> ticketList = new List<Ticket>();
      KERB_QUERY_TKT_CACHE_REQUEST ProtocolSubmitBuffer;
      ProtocolSubmitBuffer.MessageType = KERB_PROTOCOL_MESSAGE_TYPE.KerbQueryTicketCacheMessage;
      ProtocolSubmitBuffer.LogonId = 0L;
      IntPtr ProtocolReturnBuffer;
      WinStatusCodes tickets = Lsa.LsaGetTickets(this._lsaHandle, this._kerberosPackageId, ref ProtocolSubmitBuffer, 12U, out ProtocolReturnBuffer, out uint _, out uint _);
      if (tickets != WinStatusCodes.STATUS_SUCCESS)
        throw new Exception("LsaCallAuthenticationPackage (LsaGetTickets) failed with NTSTATUS code: " + (object) tickets + " (0x" + tickets.ToString("x8") + ")");
      KERB_QUERY_TKT_CACHE_RESPONSE structure1 = (KERB_QUERY_TKT_CACHE_RESPONSE) Marshal.PtrToStructure(ProtocolReturnBuffer, typeof (KERB_QUERY_TKT_CACHE_RESPONSE));
      KERB_TICKET_CACHE_INFO kerbTicketCacheInfo = new KERB_TICKET_CACHE_INFO();
      for (int index = 0; (long) index < (long) structure1.CountOfTickets; ++index)
      {
        KERB_TICKET_CACHE_INFO structure2 = (KERB_TICKET_CACHE_INFO) Marshal.PtrToStructure(new IntPtr(ProtocolReturnBuffer.ToInt64() + 8L + (long) (index * 48)), typeof (KERB_TICKET_CACHE_INFO));
        ticketList.Add(new Ticket()
        {
          ServerName = Helper.GetStringFromUNICODE_STRING(structure2.ServerName),
          RealmName = Helper.GetStringFromUNICODE_STRING(structure2.RealmName),
          StartTime = Helper.GetDateTimeFromFILETIME(structure2.StartTime),
          EndTime = Helper.GetDateTimeFromFILETIME(structure2.EndTime),
          RenewTime = Helper.GetDateTimeFromFILETIME(structure2.RenewTime),
          EncryptionType = structure2.EncryptionType,
          TicketFlags = (Ticket.KerbTicketFlags) structure2.TicketFlags
        });
      }
      if (ProtocolReturnBuffer != IntPtr.Zero)
      {
        int num = (int) Lsa.LsaFreeReturnBuffer(ProtocolReturnBuffer);
      }
      return ticketList;
    }

    public bool PurgeTicket(Ticket ticket)
    {
      byte[] bytes1 = Encoding.Unicode.GetBytes(ticket.ServerName + "\0");
      byte[] bytes2 = Encoding.Unicode.GetBytes(ticket.RealmName + "\0");
      int cb = 28 + bytes1.Length + bytes2.Length;
      IntPtr num = Marshal.AllocHGlobal(cb);
      IntPtr destination1 = new IntPtr(num.ToInt64() + 28L);
      IntPtr destination2 = new IntPtr(num.ToInt64() + 28L + (long) bytes1.Length);
      Marshal.Copy(bytes1, 0, destination1, bytes1.Length);
      Marshal.Copy(bytes2, 0, destination2, bytes2.Length);
      UNICODE_STRING unicodeString1;
      unicodeString1.Length = (ushort) (bytes1.Length - 2);
      unicodeString1.MaximumLength = (ushort) bytes1.Length;
      unicodeString1.Buffer = destination1;
      UNICODE_STRING unicodeString2;
      unicodeString2.Length = (ushort) (bytes2.Length - 2);
      unicodeString2.MaximumLength = (ushort) bytes2.Length;
      unicodeString2.Buffer = destination2;
      KERB_PURGE_TKT_CACHE_REQUEST purgeTktCacheRequest;
      purgeTktCacheRequest.MessageType = KERB_PROTOCOL_MESSAGE_TYPE.KerbPurgeTicketCacheMessage;
      purgeTktCacheRequest.ServerName = unicodeString1;
      purgeTktCacheRequest.RealmName = unicodeString2;
      purgeTktCacheRequest.LogonId = 0L;
      Marshal.StructureToPtr((object) purgeTktCacheRequest, num, false);
      uint ProtocolStatus;
      WinStatusCodes winStatusCodes = Lsa.LsaPurgeTickets(this._lsaHandle, this._kerberosPackageId, num, (uint) cb, out IntPtr _, out uint _, out ProtocolStatus);
      Marshal.FreeHGlobal(num);
      if (winStatusCodes != WinStatusCodes.STATUS_SUCCESS)
        throw new Exception("LsaCallAuthenticationPackage (LsaGetTickets) failed with NTSTATUS code: " + (object) winStatusCodes + " (0x" + winStatusCodes.ToString("x8") + ")");
      return ProtocolStatus == 0U;
    }

    ~TicketsManager()
    {
      this.Dispose(false);
    }

    private void Dispose(bool disposing)
    {
      if (this._lsaHandle != IntPtr.Zero)
      {
        int num = (int) Lsa.LsaDeregisterLogonProcess(this._lsaHandle);
        this._lsaHandle = IntPtr.Zero;
      }
      if (!disposing)
        return;
      GC.SuppressFinalize((object) this);
    }

    public void Dispose()
    {
      this.Dispose(true);
    }
  }
}
