// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.IAsn1Node
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System.IO;

namespace LipingShare.LCLib.Asn1Processor
{
  public interface IAsn1Node
  {
    bool LoadData(Stream xdata);

    bool SaveData(Stream xdata);

    Asn1Node ParentNode { get; }

    void AddChild(Asn1Node xdata);

    int InsertChild(Asn1Node xdata, int index);

    int InsertChild(Asn1Node xdata, Asn1Node indexNode);

    int InsertChildAfter(Asn1Node xdata, int index);

    int InsertChildAfter(Asn1Node xdata, Asn1Node indexNode);

    Asn1Node RemoveChild(int index);

    Asn1Node RemoveChild(Asn1Node node);

    long ChildNodeCount { get; }

    Asn1Node GetChildNode(int index);

    Asn1Node GetDescendantNodeByPath(string nodePath);

    byte Tag { get; set; }

    string TagName { get; }

    long DataLength { get; }

    long LengthFieldBytes { get; }

    long DataOffset { get; }

    byte UnusedBits { get; }

    byte[] Data { get; set; }

    bool ParseEncapsulatedData { get; set; }

    long Deepness { get; }

    string Path { get; }

    string GetText(Asn1Node startNode, int lineLen);

    string GetDataStr(bool pureHexMode);

    string GetLabel(uint mask);

    Asn1Node Clone();

    void ClearAll();
  }
}
