// Decompiled with JetBrains decompiler
// Type: LipingShare.LCLib.Asn1Processor.Asn1TreeNode
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System.Windows.Forms;

namespace LipingShare.LCLib.Asn1Processor
{
  public class Asn1TreeNode : TreeNode
  {
    private Asn1Node asn1Node = new Asn1Node();

    public Asn1Node ANode
    {
      get
      {
        return this.asn1Node;
      }
    }

    private Asn1TreeNode()
    {
    }

    public Asn1TreeNode(Asn1Node node, uint mask)
    {
      this.asn1Node = node;
      this.Text = node.GetLabel(mask);
    }

    public static void AddSubNode(Asn1TreeNode node, uint mask, TreeView treeView)
    {
      for (int index = 0; (long) index < node.ANode.ChildNodeCount; ++index)
      {
        Asn1TreeNode node1 = new Asn1TreeNode();
        node1.asn1Node = node.ANode.GetChildNode(index);
        node1.Text = node1.ANode.GetLabel(mask);
        node.Nodes.Add((TreeNode) node1);
        node.Expand();
        if (treeView != null)
          treeView.SelectedNode = (TreeNode) node;
        Asn1TreeNode.AddSubNode(node1, mask, treeView);
      }
    }

    public static TreeNode SearchTreeNode(TreeNode treeNode, Asn1Node node)
    {
      TreeNode treeNode1 = (TreeNode) null;
      if (node == null)
        return treeNode1;
      if (((Asn1TreeNode) treeNode).ANode == node)
        return treeNode;
      for (int index = 0; index < treeNode.Nodes.Count; ++index)
      {
        if (((Asn1TreeNode) treeNode.Nodes[index]).ANode == node)
        {
          treeNode1 = treeNode.Nodes[index];
          break;
        }
        treeNode1 = Asn1TreeNode.SearchTreeNode(treeNode.Nodes[index], node);
        if (treeNode1 != null)
          break;
      }
      return treeNode1;
    }
  }
}
