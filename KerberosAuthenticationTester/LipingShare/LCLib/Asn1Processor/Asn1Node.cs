// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Asn1Node
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.Collections;
using System.IO;
using System.Text;

namespace LipingShare.LCLib.Asn1Processor
{
  public class Asn1Node : IAsn1Node
  {
    private string path = "";
    private bool requireRecalculatePar = true;
    private bool parseEncapsulatedData = true;
    private const int indentStep = 3;
    public const int defaultLineLen = 80;
    public const int minLineLen = 60;
    public const int TagLength = 1;
    public const int BitStringUnusedFiledLength = 1;
    private byte tag;
    private long dataOffset;
    private long dataLength;
    private long lengthFieldBytes;
    private byte[] data;
    private ArrayList childNodeList;
    private byte unusedBits;
    private long deepness;
    private Asn1Node parentNode;
    private bool isIndefiniteLength;

    private Asn1Node(Asn1Node parentNode, long dataOffset)
    {
      this.Init();
      this.deepness = parentNode.Deepness + 1L;
      this.parentNode = parentNode;
      this.dataOffset = dataOffset;
    }

    private void Init()
    {
      this.childNodeList = new ArrayList();
      this.data = (byte[]) null;
      this.dataLength = 0L;
      this.lengthFieldBytes = 0L;
      this.unusedBits = (byte) 0;
      this.tag = (byte) 48;
      this.childNodeList.Clear();
      this.deepness = 0L;
      this.parentNode = (Asn1Node) null;
    }

    private string GetHexPrintingStr(
      Asn1Node startNode,
      string baseLine,
      string lStr,
      int lineLen)
    {
      string str1 = "";
      string indentStr = this.GetIndentStr(startNode);
      string hexString = Asn1Util.ToHexString(this.data);
      string str2;
      if (hexString.Length > 0)
      {
        if (baseLine.Length + hexString.Length < lineLen)
          str2 = str1 + baseLine + "'" + hexString + "'";
        else
          str2 = str1 + baseLine + this.FormatLineHexString(lStr, indentStr.Length, lineLen, hexString);
      }
      else
        str2 = str1 + baseLine;
      return str2 + "\r\n";
    }

    private string FormatLineString(string lStr, int indent, int lineLen, string msg)
    {
      string str = "";
      indent += 3;
      int length = lineLen - indent;
      int len = indent;
      for (int startIndex = 0; startIndex < msg.Length; startIndex += length)
      {
        if (startIndex + length > msg.Length)
          str = str + "\r\n" + lStr + Asn1Util.GenStr(len, ' ') + "'" + msg.Substring(startIndex, msg.Length - startIndex) + "'";
        else
          str = str + "\r\n" + lStr + Asn1Util.GenStr(len, ' ') + "'" + msg.Substring(startIndex, length) + "'";
      }
      return str;
    }

    private string FormatLineHexString(string lStr, int indent, int lineLen, string msg)
    {
      string str = "";
      indent += 3;
      int length = lineLen - indent;
      int len = indent;
      for (int startIndex = 0; startIndex < msg.Length; startIndex += length)
      {
        if (startIndex + length > msg.Length)
          str = str + "\r\n" + lStr + Asn1Util.GenStr(len, ' ') + msg.Substring(startIndex, msg.Length - startIndex);
        else
          str = str + "\r\n" + lStr + Asn1Util.GenStr(len, ' ') + msg.Substring(startIndex, length);
      }
      return str;
    }

    public Asn1Node()
    {
      this.Init();
      this.dataOffset = 0L;
    }

    public bool IsIndefiniteLength
    {
      get
      {
        return this.isIndefiniteLength;
      }
      set
      {
        this.isIndefiniteLength = value;
      }
    }

    public Asn1Node Clone()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.SaveData((Stream) memoryStream);
      memoryStream.Position = 0L;
      Asn1Node asn1Node = new Asn1Node();
      asn1Node.LoadData((Stream) memoryStream);
      return asn1Node;
    }

    public byte Tag
    {
      get
      {
        return this.tag;
      }
      set
      {
        this.tag = value;
      }
    }

    public bool LoadData(byte[] byteData)
    {
      bool flag;
      try
      {
        MemoryStream memoryStream = new MemoryStream(byteData);
        memoryStream.Position = 0L;
        flag = this.LoadData((Stream) memoryStream);
        memoryStream.Close();
      }
      catch
      {
        flag = false;
      }
      return flag;
    }

    public static long GetDescendantNodeCount(Asn1Node node)
    {
      long num = 0L + node.ChildNodeCount;
      for (int index = 0; (long) index < node.ChildNodeCount; ++index)
        num += Asn1Node.GetDescendantNodeCount(node.GetChildNode(index));
      return num;
    }

    public bool LoadData(Stream xdata)
    {
      try
      {
        this.RequireRecalculatePar = false;
        return this.InternalLoadData(xdata);
      }
      finally
      {
        this.RequireRecalculatePar = true;
        this.RecalculateTreePar();
      }
    }

    public byte[] GetRawData()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.SaveData((Stream) memoryStream);
      byte[] buffer = new byte[memoryStream.Length];
      memoryStream.Position = 0L;
      memoryStream.Read(buffer, 0, (int) memoryStream.Length);
      memoryStream.Close();
      return buffer;
    }

    public bool IsEmptyData
    {
      get
      {
        return this.data == null || this.data.Length < 1;
      }
    }

    public bool SaveData(Stream xdata)
    {
      bool flag = true;
      long childNodeCount = this.ChildNodeCount;
      xdata.WriteByte(this.tag);
      Asn1Util.DERLengthEncode(xdata, (ulong) this.dataLength);
      if (this.tag == (byte) 3)
        xdata.WriteByte(this.unusedBits);
      if (childNodeCount == 0L)
      {
        if (this.data != null)
          xdata.Write(this.data, 0, this.data.Length);
      }
      else
      {
        for (int index = 0; (long) index < childNodeCount; ++index)
          flag = this.GetChildNode(index).SaveData(xdata);
      }
      return flag;
    }

    public void ClearAll()
    {
      this.data = (byte[]) null;
      for (int index = 0; index < this.childNodeList.Count; ++index)
        ((Asn1Node) this.childNodeList[index]).ClearAll();
      this.childNodeList.Clear();
      this.RecalculateTreePar();
    }

    public void AddChild(Asn1Node xdata)
    {
      this.childNodeList.Add((object) xdata);
      this.RecalculateTreePar();
    }

    public int InsertChild(Asn1Node xdata, int index)
    {
      this.childNodeList.Insert(index, (object) xdata);
      this.RecalculateTreePar();
      return index;
    }

    public int InsertChild(Asn1Node xdata, Asn1Node indexNode)
    {
      int index = this.childNodeList.IndexOf((object) indexNode);
      this.childNodeList.Insert(index, (object) xdata);
      this.RecalculateTreePar();
      return index;
    }

    public int InsertChildAfter(Asn1Node xdata, Asn1Node indexNode)
    {
      int index = this.childNodeList.IndexOf((object) indexNode) + 1;
      this.childNodeList.Insert(index, (object) xdata);
      this.RecalculateTreePar();
      return index;
    }

    public int InsertChildAfter(Asn1Node xdata, int index)
    {
      int index1 = index + 1;
      this.childNodeList.Insert(index1, (object) xdata);
      this.RecalculateTreePar();
      return index1;
    }

    public Asn1Node RemoveChild(int index)
    {
      Asn1Node asn1Node = (Asn1Node) null;
      if (index < this.childNodeList.Count - 1)
        asn1Node = (Asn1Node) this.childNodeList[index + 1];
      this.childNodeList.RemoveAt(index);
      if (asn1Node == null)
        asn1Node = this.childNodeList.Count <= 0 ? this : (Asn1Node) this.childNodeList[this.childNodeList.Count - 1];
      this.RecalculateTreePar();
      return asn1Node;
    }

    public Asn1Node RemoveChild(Asn1Node node)
    {
      return this.RemoveChild(this.childNodeList.IndexOf((object) node));
    }

    public long ChildNodeCount
    {
      get
      {
        return (long) this.childNodeList.Count;
      }
    }

    public Asn1Node GetChildNode(int index)
    {
      Asn1Node asn1Node = (Asn1Node) null;
      if ((long) index < this.ChildNodeCount)
        asn1Node = (Asn1Node) this.childNodeList[index];
      return asn1Node;
    }

    public string TagName
    {
      get
      {
        return Asn1Util.GetTagName(this.tag);
      }
    }

    public Asn1Node ParentNode
    {
      get
      {
        return this.parentNode;
      }
    }

    public string GetText(Asn1Node startNode, int lineLen)
    {
      string str1 = "";
      string str2;
      switch (this.tag)
      {
        case 2:
          if (this.data != null && this.dataLength < 8L)
          {
            str2 = str1 + string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : {5}\r\n", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName, (object) Asn1Util.BytesToLong(this.data).ToString());
            break;
          }
          string baseLine1 = string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : ", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName);
          str2 = str1 + this.GetHexPrintingStr(startNode, baseLine1, "      |      |       | ", lineLen);
          break;
        case 3:
          string str3 = string.Format("{0,6}|{1,6}|{2,7}|{3} {4} UnusedBits:{5} : ", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName, (object) this.unusedBits);
          string hexString = Asn1Util.ToHexString(this.data);
          if (str3.Length + hexString.Length < lineLen)
          {
            if (hexString.Length < 1)
            {
              str2 = str1 + str3 + "\r\n";
              break;
            }
            str2 = str1 + str3 + "'" + hexString + "'\r\n";
            break;
          }
          str2 = str1 + str3 + this.FormatLineHexString("      |      |       | ", this.GetIndentStr(startNode).Length, lineLen, hexString + "\r\n");
          break;
        case 6:
          Oid oid = new Oid();
          string inOidStr = oid.Decode((Stream) new MemoryStream(this.data));
          string oidName = oid.GetOidName(inOidStr);
          str2 = str1 + string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : {5} [{6}]\r\n", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName, (object) oidName, (object) inOidStr);
          break;
        case 12:
        case 18:
        case 19:
        case 22:
        case 23:
        case 24:
        case 26:
        case 27:
        case 28:
        case 30:
          string str4 = string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : ", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName);
          string msg1 = this.tag != (byte) 12 ? Asn1Util.BytesToString(this.data) : new UTF8Encoding().GetString(this.data);
          if (str4.Length + msg1.Length < lineLen)
          {
            str2 = str1 + str4 + "'" + msg1 + "'\r\n";
            break;
          }
          str2 = str1 + str4 + this.FormatLineString("      |      |       | ", this.GetIndentStr(startNode).Length, lineLen, msg1) + "\r\n";
          break;
        case 13:
          string str5 = new RelativeOid().Decode((Stream) new MemoryStream(this.data));
          string str6 = "";
          str2 = str1 + string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : {5} [{6}]\r\n", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName, (object) str6, (object) str5);
          break;
        default:
          if (((int) this.tag & 31) == 6 || Asn1Util.IsAsciiString(this.Data))
          {
            string str7 = string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : ", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName);
            string msg2 = Asn1Util.BytesToString(this.data);
            if (str7.Length + msg2.Length < lineLen)
            {
              str2 = str1 + str7 + "'" + msg2 + "'\r\n";
              break;
            }
            str2 = str1 + str7 + this.FormatLineString("      |      |       | ", this.GetIndentStr(startNode).Length, lineLen, msg2) + "\r\n";
            break;
          }
          string baseLine2 = string.Format("{0,6}|{1,6}|{2,7}|{3} {4} : ", (object) this.dataOffset, (object) this.dataLength, (object) this.lengthFieldBytes, (object) this.GetIndentStr(startNode), (object) this.TagName);
          str2 = str1 + this.GetHexPrintingStr(startNode, baseLine2, "      |      |       | ", lineLen);
          break;
      }
      if (this.childNodeList.Count >= 0)
        str2 += this.GetListStr(startNode, lineLen);
      return str2;
    }

    public string Path
    {
      get
      {
        return this.path;
      }
    }

    public string GetDataStr(bool pureHexMode)
    {
      string str;
      if (pureHexMode)
      {
        str = Asn1Util.FormatString(Asn1Util.ToHexString(this.data), 32, 2);
      }
      else
      {
        switch (this.tag)
        {
          case 2:
            str = Asn1Util.FormatString(Asn1Util.ToHexString(this.data), 32, 2);
            break;
          case 3:
            str = Asn1Util.FormatString(Asn1Util.ToHexString(this.data), 32, 2);
            break;
          case 6:
            str = new Oid().Decode((Stream) new MemoryStream(this.data));
            break;
          case 12:
            str = new UTF8Encoding().GetString(this.data);
            break;
          case 13:
            str = new RelativeOid().Decode((Stream) new MemoryStream(this.data));
            break;
          case 18:
          case 19:
          case 22:
          case 23:
          case 24:
          case 26:
          case 27:
          case 28:
          case 30:
            str = Asn1Util.BytesToString(this.data);
            break;
          default:
            str = ((int) this.tag & 31) == 6 || Asn1Util.IsAsciiString(this.Data) ? Asn1Util.BytesToString(this.data) : Asn1Util.FormatString(Asn1Util.ToHexString(this.data), 32, 2);
            break;
        }
      }
      return str;
    }

    public string GetLabel(uint mask)
    {
      string str1 = "";
      string str2 = ((int) mask & 4) == 0 ? (((int) mask & 8) == 0 ? string.Format("({0},{1})", (object) this.dataOffset, (object) this.dataLength) : string.Format("({0},{1},{2})", (object) this.tag, (object) this.dataOffset, (object) this.dataLength)) : (((int) mask & 8) == 0 ? string.Format("(0x{0:X6},0x{1:X4})", (object) this.dataOffset, (object) this.dataLength) : string.Format("(0x{0:X2},0x{1:X6},0x{2:X4})", (object) this.tag, (object) this.dataOffset, (object) this.dataLength));
      string str3;
      switch (this.tag)
      {
        case 2:
          if (((int) mask & 1) != 0)
            str1 += str2;
          str3 = str1 + " " + this.TagName;
          if (((int) mask & 2) != 0)
          {
            string str4 = this.data == null || this.dataLength >= 8L ? Asn1Util.ToHexString(this.data) : Asn1Util.BytesToLong(this.data).ToString();
            str3 += str4.Length > 0 ? " : '" + str4 + "'" : "";
            break;
          }
          break;
        case 3:
          if (((int) mask & 1) != 0)
            str1 += str2;
          str3 = str1 + " " + this.TagName + " UnusedBits: " + this.unusedBits.ToString();
          if (((int) mask & 2) != 0)
          {
            string hexString = Asn1Util.ToHexString(this.data);
            str3 += hexString.Length > 0 ? " : '" + hexString + "'" : "";
            break;
          }
          break;
        case 6:
          Oid oid = new Oid();
          string inOidStr = oid.Decode(this.data);
          string oidName = oid.GetOidName(inOidStr);
          if (((int) mask & 1) != 0)
            str1 += str2;
          str3 = str1 + " " + this.TagName + " : " + oidName;
          if (((int) mask & 2) != 0)
          {
            str3 += inOidStr.Length > 0 ? " : '" + inOidStr + "'" : "";
            break;
          }
          break;
        case 12:
        case 18:
        case 19:
        case 22:
        case 23:
        case 24:
        case 26:
        case 27:
        case 28:
        case 30:
          if (((int) mask & 1) != 0)
            str1 += str2;
          str3 = str1 + " " + this.TagName;
          if (((int) mask & 2) != 0)
          {
            string str4 = this.tag != (byte) 12 ? Asn1Util.BytesToString(this.data) : new UTF8Encoding().GetString(this.data);
            str3 += str4.Length > 0 ? " : '" + str4 + "'" : "";
            break;
          }
          break;
        case 13:
          string str5 = new RelativeOid().Decode(this.data);
          string str6 = "";
          if (((int) mask & 1) != 0)
            str1 += str2;
          str3 = str1 + " " + this.TagName + " : " + str6;
          if (((int) mask & 2) != 0)
          {
            str3 += str5.Length > 0 ? " : '" + str5 + "'" : "";
            break;
          }
          break;
        default:
          if (((int) mask & 1) != 0)
            str1 += str2;
          str3 = str1 + " " + this.TagName;
          if (((int) mask & 2) != 0)
          {
            string str4 = ((int) this.tag & 31) == 6 || Asn1Util.IsAsciiString(this.Data) ? Asn1Util.BytesToString(this.data) : Asn1Util.ToHexString(this.data);
            str3 += str4.Length > 0 ? " : '" + str4 + "'" : "";
            break;
          }
          break;
      }
      if (((int) mask & 16) != 0)
        str3 = "(" + this.path + ") " + str3;
      return str3;
    }

    public long DataLength
    {
      get
      {
        return this.dataLength;
      }
    }

    public long LengthFieldBytes
    {
      get
      {
        return this.lengthFieldBytes;
      }
    }

    public byte[] Data
    {
      get
      {
        MemoryStream memoryStream = new MemoryStream();
        long childNodeCount = this.ChildNodeCount;
        if (childNodeCount == 0L)
        {
          if (this.data != null)
            memoryStream.Write(this.data, 0, this.data.Length);
        }
        else
        {
          for (int index = 0; (long) index < childNodeCount; ++index)
            this.GetChildNode(index).SaveData((Stream) memoryStream);
        }
        byte[] buffer = new byte[memoryStream.Length];
        memoryStream.Position = 0L;
        memoryStream.Read(buffer, 0, (int) memoryStream.Length);
        memoryStream.Close();
        return buffer;
      }
      set
      {
        this.SetData(value);
      }
    }

    public long Deepness
    {
      get
      {
        return this.deepness;
      }
    }

    public long DataOffset
    {
      get
      {
        return this.dataOffset;
      }
    }

    public byte UnusedBits
    {
      get
      {
        return this.unusedBits;
      }
      set
      {
        this.unusedBits = value;
      }
    }

    public Asn1Node GetDescendantNodeByPath(string nodePath)
    {
      Asn1Node asn1Node = this;
      if (nodePath == null)
        return asn1Node;
      nodePath = nodePath.TrimEnd().TrimStart();
      if (nodePath.Length < 1)
        return asn1Node;
      string[] strArray = nodePath.Split('/');
      try
      {
        for (int index = 1; index < strArray.Length; ++index)
          asn1Node = asn1Node.GetChildNode(Convert.ToInt32(strArray[index]));
      }
      catch
      {
        asn1Node = (Asn1Node) null;
      }
      return asn1Node;
    }

    public static Asn1Node GetDecendantNodeByOid(string oid, Asn1Node startNode)
    {
      Asn1Node asn1Node = (Asn1Node) null;
      Oid oid1 = new Oid();
      for (int index = 0; (long) index < startNode.ChildNodeCount; ++index)
      {
        Asn1Node childNode = startNode.GetChildNode(index);
        if (((int) childNode.tag & 31) == 6 && oid == oid1.Decode(childNode.Data))
        {
          asn1Node = childNode;
          break;
        }
        asn1Node = Asn1Node.GetDecendantNodeByOid(oid, childNode);
        if (asn1Node != null)
          break;
      }
      return asn1Node;
    }

    protected bool RequireRecalculatePar
    {
      get
      {
        return this.requireRecalculatePar;
      }
      set
      {
        this.requireRecalculatePar = value;
      }
    }

    protected void RecalculateTreePar()
    {
      if (!this.requireRecalculatePar)
        return;
      Asn1Node asn1Node = this;
      while (asn1Node.ParentNode != null)
        asn1Node = asn1Node.ParentNode;
      Asn1Node.ResetBranchDataLength(asn1Node);
      asn1Node.dataOffset = 0L;
      asn1Node.deepness = 0L;
      long subOffset = asn1Node.dataOffset + 1L + asn1Node.lengthFieldBytes;
      this.ResetChildNodePar(asn1Node, subOffset);
    }

    protected static long ResetBranchDataLength(Asn1Node node)
    {
      long num = 0;
      if (node.ChildNodeCount < 1L)
      {
        if (node.data != null)
          num += (long) node.data.Length;
      }
      else
      {
        for (int index = 0; (long) index < node.ChildNodeCount; ++index)
          num += Asn1Node.ResetBranchDataLength(node.GetChildNode(index));
      }
      node.dataLength = num;
      if (node.tag == (byte) 3)
        ++node.dataLength;
      Asn1Node.ResetDataLengthFieldWidth(node);
      return node.dataLength + 1L + node.lengthFieldBytes;
    }

    protected static void ResetDataLengthFieldWidth(Asn1Node node)
    {
      MemoryStream memoryStream = new MemoryStream();
      Asn1Util.DERLengthEncode((Stream) memoryStream, (ulong) node.dataLength);
      node.lengthFieldBytes = memoryStream.Length;
      memoryStream.Close();
    }

    protected void ResetChildNodePar(Asn1Node xNode, long subOffset)
    {
      if (xNode.tag == (byte) 3)
        ++subOffset;
      for (int index = 0; (long) index < xNode.ChildNodeCount; ++index)
      {
        Asn1Node childNode = xNode.GetChildNode(index);
        childNode.parentNode = xNode;
        childNode.dataOffset = subOffset;
        childNode.deepness = xNode.deepness + 1L;
        childNode.path = xNode.path + (object) '/' + index.ToString();
        subOffset += 1L + childNode.lengthFieldBytes;
        this.ResetChildNodePar(childNode, subOffset);
        subOffset += childNode.dataLength;
      }
    }

    protected string GetListStr(Asn1Node startNode, int lineLen)
    {
      string str = "";
      for (int index = 0; index < this.childNodeList.Count; ++index)
      {
        Asn1Node childNode = (Asn1Node) this.childNodeList[index];
        str += childNode.GetText(startNode, lineLen);
      }
      return str;
    }

    protected string GetIndentStr(Asn1Node startNode)
    {
      string str = "";
      long num = 0;
      if (startNode != null)
        num = startNode.Deepness;
      for (long index = 0; index < this.deepness - num; ++index)
        str += "   ";
      return str;
    }

    protected bool GeneralDecode(Stream xdata)
    {
      bool flag = false;
      long num = xdata.Length - xdata.Position;
      this.tag = (byte) xdata.ReadByte();
      long position = xdata.Position;
      this.dataLength = Asn1Util.DerLengthDecode(xdata, ref this.isIndefiniteLength);
      if (this.dataLength < 0L)
        return flag;
      this.lengthFieldBytes = xdata.Position - position;
      if (num < this.dataLength + 1L + this.lengthFieldBytes || (this.ParentNode == null || ((int) this.ParentNode.tag & 32) == 0) && (((int) this.tag & 31) <= 0 || ((int) this.tag & 31) > 30))
        return flag;
      if (this.tag == (byte) 3)
      {
        if (this.dataLength < 1L)
          return flag;
        this.unusedBits = (byte) xdata.ReadByte();
        this.data = new byte[this.dataLength - 1L];
        xdata.Read(this.data, 0, (int) (this.dataLength - 1L));
      }
      else
      {
        this.data = new byte[this.dataLength];
        xdata.Read(this.data, 0, (int) this.dataLength);
      }
      return true;
    }

    protected bool ListDecode(Stream xdata)
    {
      bool flag = false;
      long position1 = xdata.Position;
      try
      {
        long num = xdata.Length - xdata.Position;
        this.tag = (byte) xdata.ReadByte();
        long position2 = xdata.Position;
        this.dataLength = Asn1Util.DerLengthDecode(xdata, ref this.isIndefiniteLength);
        if (this.dataLength < 0L || num < this.dataLength)
          return flag;
        this.lengthFieldBytes = xdata.Position - position2;
        long dataOffset = this.dataOffset + 1L + this.lengthFieldBytes;
        if (this.tag == (byte) 3)
        {
          this.unusedBits = (byte) xdata.ReadByte();
          --this.dataLength;
          ++dataOffset;
        }
        if (this.dataLength <= 0L)
          return flag;
        Stream xdata1 = (Stream) new MemoryStream((int) this.dataLength);
        byte[] buffer = new byte[this.dataLength];
        xdata.Read(buffer, 0, (int) this.dataLength);
        if (this.tag == (byte) 3)
          ++this.dataLength;
        xdata1.Write(buffer, 0, buffer.Length);
        xdata1.Position = 0L;
        while (xdata1.Position < xdata1.Length)
        {
          Asn1Node xdata2 = new Asn1Node(this, dataOffset);
          xdata2.parseEncapsulatedData = this.parseEncapsulatedData;
          long position3 = xdata1.Position;
          if (!xdata2.InternalLoadData(xdata1))
            return flag;
          this.AddChild(xdata2);
          long position4 = xdata1.Position;
          dataOffset += position4 - position3;
        }
        flag = true;
        return flag;
      }
      finally
      {
        if (!flag)
        {
          xdata.Position = position1;
          this.ClearAll();
        }
      }
    }

    protected void SetData(byte[] xdata)
    {
      if (this.childNodeList.Count > 0)
        throw new Exception("Constructed node can't hold simple data.");
      this.data = xdata;
      this.dataLength = this.data == null ? 0L : (long) this.data.Length;
      this.RecalculateTreePar();
    }

    protected bool InternalLoadData(Stream xdata)
    {
      bool flag = true;
      this.ClearAll();
      long position = xdata.Position;
      byte num1 = (byte) xdata.ReadByte();
      xdata.Position = position;
      int num2 = (int) num1 & 31;
      if (((int) num1 & 32) != 0 || this.parseEncapsulatedData && (num2 == 3 || num2 == 8 || (num2 == 27 || num2 == 24) || (num2 == 25 || num2 == 22 || (num2 == 4 || num2 == 19)) || (num2 == 16 || num2 == 17 || (num2 == 20 || num2 == 28) || (num2 == 12 || num2 == 21 || num2 == 26))))
      {
        if (!this.ListDecode(xdata) && !this.GeneralDecode(xdata))
          flag = false;
      }
      else if (!this.GeneralDecode(xdata))
        flag = false;
      return flag;
    }

    public bool ParseEncapsulatedData
    {
      get
      {
        return this.parseEncapsulatedData;
      }
      set
      {
        if (this.parseEncapsulatedData == value)
          return;
        byte[] data = this.Data;
        this.parseEncapsulatedData = value;
        this.ClearAll();
        if (((int) this.tag & 32) != 0 || this.parseEncapsulatedData)
        {
          MemoryStream memoryStream = new MemoryStream(data);
          memoryStream.Position = 0L;
          bool flag = true;
          while (memoryStream.Position < memoryStream.Length)
          {
            Asn1Node xdata = new Asn1Node();
            xdata.ParseEncapsulatedData = this.parseEncapsulatedData;
            if (!xdata.LoadData((Stream) memoryStream))
            {
              this.ClearAll();
              flag = false;
              break;
            }
            this.AddChild(xdata);
          }
          if (flag)
            return;
          this.Data = data;
        }
        else
          this.Data = data;
      }
    }

    public class TagTextMask
    {
      public const uint SHOW_OFFSET = 1;
      public const uint SHOW_DATA = 2;
      public const uint USE_HEX_OFFSET = 4;
      public const uint SHOW_TAG_NUMBER = 8;
      public const uint SHOW_PATH = 16;
    }
  }
}
