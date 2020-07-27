// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Win32.LsaStringWrapper
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Runtime.InteropServices;

namespace MB.Utilities.Win32
{
  public class LsaStringWrapper : IDisposable
  {
    public LSA_STRING _string;

    public LsaStringWrapper(string value)
    {
      this._string = new LSA_STRING();
      this._string.Length = (ushort) value.Length;
      this._string.MaximumLength = (ushort) value.Length;
      this._string.Buffer = Marshal.StringToHGlobalAnsi(value);
    }

    ~LsaStringWrapper()
    {
      this.Dispose(false);
    }

    private void Dispose(bool disposing)
    {
      if (this._string.Buffer != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(this._string.Buffer);
        this._string.Buffer = IntPtr.Zero;
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
