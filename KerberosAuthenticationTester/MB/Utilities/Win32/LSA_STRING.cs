// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Win32.LSA_STRING
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;

namespace MB.Utilities.Win32
{
  public struct LSA_STRING
  {
    public ushort Length;
    public ushort MaximumLength;
    public IntPtr Buffer;
  }
}
