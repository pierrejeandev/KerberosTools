// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Win32.Lsa
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Runtime.InteropServices;

namespace MB.Utilities.Win32
{
  public static class Lsa
  {
    [DllImport("secur32.dll")]
    public static extern WinStatusCodes LsaConnectUntrusted(out IntPtr LsaHandle);

    [DllImport("secur32.dll")]
    public static extern WinStatusCodes LsaDeregisterLogonProcess([In] IntPtr LsaHandle);

    [DllImport("secur32.dll")]
    public static extern WinStatusCodes LsaLookupAuthenticationPackage(
      [In] IntPtr LsaHandle,
      [In] ref LSA_STRING PackageName,
      out IntPtr AuthenticationPackage);

    [DllImport("secur32.dll", EntryPoint = "LsaCallAuthenticationPackage")]
    public static extern WinStatusCodes LsaGetTickets(
      [In] IntPtr LsaHandle,
      [In] IntPtr AuthenticationPackage,
      [In] ref KERB_QUERY_TKT_CACHE_REQUEST ProtocolSubmitBuffer,
      [In] uint SubmitBufferLength,
      out IntPtr ProtocolReturnBuffer,
      out uint ReturnBufferLength,
      out uint ProtocolStatus);

    [DllImport("secur32.dll")]
    public static extern WinStatusCodes LsaFreeReturnBuffer([In] IntPtr buffer);

    [DllImport("secur32.dll", EntryPoint = "LsaCallAuthenticationPackage")]
    public static extern WinStatusCodes LsaPurgeTickets(
      [In] IntPtr LsaHandle,
      [In] IntPtr AuthenticationPackage,
      [In] IntPtr ProtocolSubmitBuffer,
      [In] uint SubmitBufferLength,
      out IntPtr ProtocolReturnBuffer,
      out uint ReturnBufferLength,
      out uint ProtocolStatus);
  }
}
