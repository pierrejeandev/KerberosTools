// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.KerberosAuthorizeMessageBuilder
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using LipingShare.LCLib.Asn1Processor;
using System;
using System.Collections.Generic;
using System.IO;

namespace MB.Authorization.Kerberos
{
  public class KerberosAuthorizeMessageBuilder
  {
    public static KerberosAuthorizeMessage BuildNew(byte[] data)
    {
      KerberosAuthorizeMessage kerberosAuthorizeMessage = new KerberosAuthorizeMessage();
      KerberosAuthorizeMessageBuilder.Build(kerberosAuthorizeMessage, data);
      return kerberosAuthorizeMessage;
    }

    public static void Build(KerberosAuthorizeMessage kerberosAuthorizeMessage, byte[] data)
    {
      Asn1Parser asn1Parser = new Asn1Parser();
      MemoryStream memoryStream = new MemoryStream(data);
      asn1Parser.LoadData((Stream) memoryStream);
      Asn1Node rootNode = asn1Parser.RootNode;
      KerberosAuthorizeMessageBuilder.BuildRoot(kerberosAuthorizeMessage, rootNode);
    }

    private static void BuildRoot(KerberosAuthorizeMessage msg, Asn1Node root)
    {
      if (root.ChildNodeCount == 2L)
      {
        Asn1Node childNode = root.GetChildNode(0);
        if (((int) childNode.Tag & 31) != 6)
          throw new Exception("OBJECT_IDENTIFIER expected. (path: " + childNode.Path + ")");
        msg.MechType = new MechType(childNode.GetDataStr(false));
        msg.NegotiationToken = (NegotiationToken) new NegTokenInit();
        KerberosAuthorizeMessageBuilder.BuildNegotiateTokenInit((NegTokenInit) msg.NegotiationToken, root.GetChildNode(1).GetChildNode(0));
      }
      else
      {
        if (root.ChildNodeCount != 1L)
          return;
        msg.NegotiationToken = (NegotiationToken) new NegTokenInit();
        KerberosAuthorizeMessageBuilder.BuildNegotiateTokenInit((NegTokenInit) msg.NegotiationToken, root.GetChildNode(0));
      }
    }

    private static void BuildNegotiateTokenInit(NegTokenInit negToken, Asn1Node sequence)
    {
      for (int index = 0; (long) index < sequence.ChildNodeCount; ++index)
      {
        switch (KerberosAuthorizeMessageBuilder.GetContextNumber(sequence.GetChildNode(index)))
        {
          case 0:
            KerberosAuthorizeMessageBuilder.BuildMechTypes(negToken, sequence.GetChildNode(index));
            break;
          case 2:
            negToken.MechToken = new InitialContextToken();
            KerberosAuthorizeMessageBuilder.BuildMechToken(negToken.MechToken, sequence.GetChildNode(index));
            break;
        }
      }
    }

    private static void BuildMechTypes(NegTokenInit negToken, Asn1Node asn1Node)
    {
      if (asn1Node.ChildNodeCount <= 0L)
        return;
      Asn1Node childNode = asn1Node.GetChildNode(0);
      negToken.MechTypes = new List<MechType>();
      for (int index = 0; (long) index < childNode.ChildNodeCount; ++index)
      {
        if (((int) childNode.GetChildNode(index).Tag & 31) == 6)
          negToken.MechTypes.Add(new MechType(childNode.GetChildNode(index).GetDataStr(false)));
      }
    }

    private static void BuildMechToken(InitialContextToken initialContextToken, Asn1Node asn1Node)
    {
      Asn1Node childNode = asn1Node.GetChildNode(0).GetChildNode(0);
      for (int index = 0; (long) index < childNode.ChildNodeCount; ++index)
      {
        if (((int) childNode.GetChildNode(index).Tag & 192) != 0)
        {
          if (((int) childNode.GetChildNode(index).Tag & 192) == 64)
          {
            initialContextToken.InnerContextToken = new KrbApReq();
            KerberosAuthorizeMessageBuilder.BuildKrbApReq(initialContextToken.InnerContextToken, childNode.GetChildNode(index));
          }
        }
        else if (((int) childNode.GetChildNode(index).Tag & 31) == 6)
          initialContextToken.ThisMech = new MechType(childNode.GetChildNode(index).GetDataStr(false));
      }
    }

    private static void BuildKrbApReq(KrbApReq krbApReq, Asn1Node asn1Node)
    {
      Asn1Node childNode1 = asn1Node.GetChildNode(0);
      for (int index = 0; (long) index < childNode1.ChildNodeCount; ++index)
      {
        Asn1Node childNode2 = childNode1.GetChildNode(index);
        switch (KerberosAuthorizeMessageBuilder.GetContextNumber(childNode2))
        {
          case 0:
            krbApReq.ProtocolVersionNumber = (int) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data);
            break;
          case 1:
            krbApReq.MessageType = (MessageType) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data);
            break;
          case 2:
            krbApReq.APOptions = (APOptions) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data);
            break;
          case 3:
            krbApReq.Ticket = new Ticket();
            KerberosAuthorizeMessageBuilder.BuildTicket(krbApReq.Ticket, childNode2);
            break;
          case 4:
            krbApReq.Authenticator = new EncryptedData();
            KerberosAuthorizeMessageBuilder.BuildEncryptedData(krbApReq.Authenticator, childNode2);
            break;
        }
      }
    }

    private static void BuildEncryptedData(EncryptedData encryptedData, Asn1Node asn1Node)
    {
      Asn1Node childNode1 = asn1Node.GetChildNode(0);
      for (int index = 0; (long) index < childNode1.ChildNodeCount; ++index)
      {
        Asn1Node childNode2 = childNode1.GetChildNode(index);
        switch (KerberosAuthorizeMessageBuilder.GetContextNumber(childNode2))
        {
          case 0:
            encryptedData.EncryptionType = (EncryptionType) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data);
            break;
          case 1:
            encryptedData.KeyVersionNumber = new uint?((uint) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data));
            break;
          case 2:
            encryptedData.Cipher = new byte[childNode2.GetChildNode(0).DataLength];
            Buffer.BlockCopy((Array) childNode2.GetChildNode(0).Data, 0, (Array) encryptedData.Cipher, 0, encryptedData.Cipher.Length);
            break;
        }
      }
    }

    private static void BuildTicket(Ticket ticket, Asn1Node asn1Node)
    {
      Asn1Node childNode1 = asn1Node.GetChildNode(0).GetChildNode(0);
      for (int index = 0; (long) index < childNode1.ChildNodeCount; ++index)
      {
        Asn1Node childNode2 = childNode1.GetChildNode(index);
        switch (KerberosAuthorizeMessageBuilder.GetContextNumber(childNode2))
        {
          case 0:
            ticket.TicketVersionNumber = (int) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data);
            break;
          case 1:
            ticket.Realm = childNode2.GetChildNode(0).GetDataStr(false);
            break;
          case 2:
            ticket.ServiceName = new PrincipalName();
            KerberosAuthorizeMessageBuilder.BuildPrincipleName(ticket.ServiceName, childNode2);
            break;
          case 3:
            ticket.EncPart = new EncryptedData();
            KerberosAuthorizeMessageBuilder.BuildEncryptedData(ticket.EncPart, childNode2);
            break;
        }
      }
    }

    private static void BuildPrincipleName(PrincipalName principalName, Asn1Node asn1Node)
    {
      Asn1Node childNode1 = asn1Node.GetChildNode(0);
      for (int index = 0; (long) index < childNode1.ChildNodeCount; ++index)
      {
        Asn1Node childNode2 = childNode1.GetChildNode(index);
        switch (KerberosAuthorizeMessageBuilder.GetContextNumber(childNode2))
        {
          case 0:
            principalName.NameType = (PrincipalNameType) Asn1Util.BytesToLong(childNode2.GetChildNode(0).Data);
            break;
          case 1:
            principalName.NameString = new List<string>();
            KerberosAuthorizeMessageBuilder.BuildPrincipleNameList(principalName.NameString, childNode2);
            break;
        }
      }
    }

    private static void BuildPrincipleNameList(List<string> list, Asn1Node asn1Node)
    {
      Asn1Node childNode = asn1Node.GetChildNode(0);
      for (int index = 0; (long) index < childNode.ChildNodeCount; ++index)
        list.Add(childNode.GetChildNode(index).GetDataStr(false));
    }

    private static int GetContextNumber(Asn1Node node)
    {
      if (((int) node.Tag & 192) != 128)
        throw new Exception("Node is not of class CONTEXT SPECIFIC. (Path: " + node.Path + ")");
      return (int) node.Tag & 31;
    }
  }
}
