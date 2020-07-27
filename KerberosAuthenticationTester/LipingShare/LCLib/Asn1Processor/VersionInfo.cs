// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.VersionInfo
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace LipingShare.LCLib.Asn1Processor
{
  public class VersionInfo
  {
    private static string versionStr = "V2008.09.29 - 1.0.20";
    private static string copyrightStr = "Copyright © 2003,2004,2005,2007,2008 Liping Dai. All rights reserved.";
    private static string contactInfo = "LipingShare@yahoo.com";
    private static string updateUrl = "http://www.lipingshare.com/Asn1Editor";
    private static string author = "Liping Dai";
    private static string releaseDate = "September 29, 2008";

    public static string VersionStr
    {
      get
      {
        return VersionInfo.versionStr;
      }
    }

    public static string CopyrightStr
    {
      get
      {
        return VersionInfo.copyrightStr;
      }
    }

    public static string ContactInfo
    {
      get
      {
        return VersionInfo.contactInfo;
      }
    }

    public static string UpdateUrl
    {
      get
      {
        return VersionInfo.updateUrl;
      }
    }

    public static string Author
    {
      get
      {
        return VersionInfo.author;
      }
    }

    public static string ReleaseDate
    {
      get
      {
        return VersionInfo.releaseDate;
      }
    }
  }
}
