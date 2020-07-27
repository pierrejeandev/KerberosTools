// Decompiled with JetBrains decompiler
// Type: MB.WebAuthenticationTester.EventLogWriter
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System.Diagnostics;

namespace MB.WebAuthenticationTester
{
  public static class EventLogWriter
  {
    public static void Write(string message)
    {
      string str = "Web Authentication Tester";
      EventLog eventLog = new EventLog();
      if (!EventLog.SourceExists(str))
        EventLog.CreateEventSource(str, str);
      eventLog.Source = str;
      eventLog.EnableRaisingEvents = true;
      eventLog.WriteEntry(message, EventLogEntryType.Error);
    }
  }
}
