// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.BinaryView
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System.Text;

namespace LipingShare.LCLib.Asn1Processor
{
  public class BinaryView
  {
    private int offsetWidth = 6;
    private int dataWidth = 16;
    private int totalWidth;
    private int hexWidth;

    public BinaryView()
    {
      this.CalculatePar();
    }

    public void SetPar(int offsetWidth, int dataWidth)
    {
      this.offsetWidth = offsetWidth;
      this.dataWidth = dataWidth;
      this.CalculatePar();
    }

    public BinaryView(int offsetWidth, int dataWidth)
    {
      this.SetPar(offsetWidth, dataWidth);
    }

    public int OffsetWidth
    {
      get
      {
        return this.offsetWidth;
      }
    }

    public int DataWidth
    {
      get
      {
        return this.dataWidth;
      }
    }

    public int TotalWidth
    {
      get
      {
        return this.totalWidth;
      }
    }

    public int HexWidth
    {
      get
      {
        return this.hexWidth;
      }
    }

    protected void CalculatePar()
    {
      this.totalWidth = this.offsetWidth + 2 + this.dataWidth * 3 + (this.dataWidth / 8 - 1) + 1 + this.dataWidth;
      this.hexWidth = this.totalWidth - this.dataWidth;
    }

    public string GenerateText(byte[] data)
    {
      return BinaryView.GetBinaryViewText(data, this.offsetWidth, this.dataWidth);
    }

    public void GetLocation(int byteOffset, ByteLocation loc)
    {
      int num1 = byteOffset - byteOffset / this.dataWidth * this.dataWidth;
      int num2 = byteOffset / this.dataWidth;
      int num3 = this.offsetWidth + 2 + num1 * 3;
      int num4 = 3;
      int num5 = num2 * this.totalWidth + num2 + num3;
      int num6 = this.hexWidth + num1;
      int num7 = num2 * this.totalWidth + num2 + num6;
      int num8 = 1;
      loc.hexOffset = num5;
      loc.hexColLen = num4;
      loc.line = num2;
      loc.chOffset = num7;
      loc.chColLen = num8;
    }

    public static string GetBinaryViewText(byte[] data, int offsetWidth, int dataWidth)
    {
      string format1 = "{0:X" + (object) offsetWidth + "}  ";
      int num1 = 0;
      int num2 = offsetWidth + 2 + dataWidth * 3 + (dataWidth / 8 - 1) + 1 + dataWidth;
      int totalWidth = num2 - dataWidth;
      StringBuilder stringBuilder = new StringBuilder();
      string format2 = "{0,-" + (object) num2 + "}\r\n";
      int num3 = 0;
      while (num3 < data.Length)
      {
        string str1 = string.Format(format1, (object) (num1++ * dataWidth));
        int num4 = num3;
        for (int index = 0; index < dataWidth; ++index)
        {
          str1 += string.Format("{0:X2} ", (object) data[num3++]);
          if (num3 < data.Length)
          {
            if ((index + 1) % 8 == 0 && index != 0 && index + 1 < dataWidth)
              str1 += " ";
          }
          else
            break;
        }
        string str2 = str1 + " ";
        int num5 = num3;
        string str3 = str2.PadRight(totalWidth, ' ');
        for (int index = num4; index < num5; ++index)
          str3 = data[index] < (byte) 32 || data[index] > (byte) 128 ? str3 + (object) '.' : str3 + (object) (char) data[index];
        stringBuilder.AppendFormat(format2, (object) str3);
      }
      return stringBuilder.ToString();
    }
  }
}
