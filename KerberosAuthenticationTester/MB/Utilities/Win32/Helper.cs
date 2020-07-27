// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Win32.Helper
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Runtime.InteropServices;

namespace MB.Utilities.Win32
{
  public static class Helper
  {
    public static DateTime GetDateTimeFromFILETIME(ulong fileTime)
    {
      return DateTime.FromFileTime((long) fileTime);
    }

    public static string GetStringFromUNICODE_STRING(UNICODE_STRING unicodeString)
    {
      return Marshal.PtrToStringUni(unicodeString.Buffer, (int) unicodeString.Length / 2);
    }
  }
}
