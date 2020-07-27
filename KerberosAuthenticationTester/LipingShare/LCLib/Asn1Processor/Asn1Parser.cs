// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Asn1Parser
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.IO;

namespace LipingShare.LCLib.Asn1Processor
{
  public class Asn1Parser
  {
    private Asn1Node rootNode = new Asn1Node();
    private byte[] rawData;

    private bool ParseEncapsulatedData
    {
      get
      {
        return this.rootNode.ParseEncapsulatedData;
      }
      set
      {
        this.rootNode.ParseEncapsulatedData = value;
      }
    }

    public byte[] RawData
    {
      get
      {
        return this.rawData;
      }
    }

    public void LoadData(string fileName)
    {
      FileStream fileStream = new FileStream(fileName, FileMode.Open);
      this.rawData = new byte[fileStream.Length];
      fileStream.Read(this.rawData, 0, (int) fileStream.Length);
      fileStream.Close();
      this.LoadData((Stream) new MemoryStream(this.rawData));
    }

    public void LoadPemData(string fileName)
    {
      FileStream fileStream = new FileStream(fileName, FileMode.Open);
      byte[] numArray = new byte[fileStream.Length];
      fileStream.Read(numArray, 0, numArray.Length);
      fileStream.Close();
      string pemStr = Asn1Util.BytesToString(numArray);
      if (!Asn1Util.IsPemFormated(pemStr))
        throw new Exception("It is a invalid PEM file: " + fileName);
      Stream stream = Asn1Util.PemToStream(pemStr);
      stream.Position = 0L;
      this.LoadData(stream);
    }

    public void LoadData(Stream stream)
    {
      stream.Position = 0L;
      if (!this.rootNode.LoadData(stream))
        throw new Exception("Failed to load data.");
      this.rawData = new byte[stream.Length];
      stream.Position = 0L;
      stream.Read(this.rawData, 0, this.rawData.Length);
    }

    public void SaveData(string fileName)
    {
      FileStream fileStream = new FileStream(fileName, FileMode.Create);
      this.rootNode.SaveData((Stream) fileStream);
      fileStream.Close();
    }

    public Asn1Node RootNode
    {
      get
      {
        return this.rootNode;
      }
    }

    public Asn1Node GetNodeByPath(string nodePath)
    {
      return this.rootNode.GetDescendantNodeByPath(nodePath);
    }

    public Asn1Node GetNodeByOid(string oid)
    {
      return Asn1Node.GetDecendantNodeByOid(oid, this.rootNode);
    }

    public static string GetNodeTextHeader(int lineLen)
    {
      return string.Format("Offset| Len  |LenByte|\r\n") + "======+======+=======+" + Asn1Util.GenStr(lineLen + 10, '=') + "\r\n";
    }

    public override string ToString()
    {
      return Asn1Parser.GetNodeText(this.rootNode, 100);
    }

    public static string GetNodeText(Asn1Node node, int lineLen)
    {
      return Asn1Parser.GetNodeTextHeader(lineLen) + node.GetText(node, lineLen);
    }
  }
}
