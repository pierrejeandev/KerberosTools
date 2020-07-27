// Decompiled with JetBrains decompiler
// Type: MB.WebAuthenticationTester.AuthorizationDetails
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace MB.WebAuthenticationTester
{
  public class AuthorizationDetails : Form
  {
    private WebTestRequest _msg;
    private string _msgXml;
    private IContainer components;
    private TreeView treeDetails;
    private Button btnSave;
    private Button btnClose;
    private SaveFileDialog saveFileDialog1;

    public AuthorizationDetails(WebTestRequest message)
    {
      this._msg = message;
      this.InitializeComponent();
      this.Cursor = Cursors.WaitCursor;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      try
      {
        this.saveFileDialog1.FileName = string.Format("{1}_{0}.xml", (object) this._msg.AuthorizationType.ToString(), (object) this._msg.RequestDate.ToString("yyyymmddhhmmss"));
        if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
          return;
        using (StreamWriter text = File.CreateText(this.saveFileDialog1.FileName))
        {
          new XmlSerializer(this._msg.GetType()).Serialize((TextWriter) text, (object) this._msg);
          text.Close();
          int num = (int) MessageBox.Show("File saved.");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Saving file failed.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        EventLogWriter.Write(ex.ToString());
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void AuthorizationDetails_Load(object sender, EventArgs e)
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(this._msg.GetType());
        StringWriter stringWriter = new StringWriter();
        xmlSerializer.Serialize((TextWriter) stringWriter, (object) this._msg);
        stringWriter.Close();
        this._msgXml = stringWriter.ToString();
        this.FillTree(this.treeDetails);
        this.treeDetails.SelectedNode = this.treeDetails.Nodes[0];
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Loading details failed. Details window will be closed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        EventLogWriter.Write(ex.ToString());
        this.Close();
      }
      this.Cursor = Cursors.Default;
    }

    private void FillTree(TreeView treeView)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this._msgXml);
      treeView.Nodes.Clear();
      treeView.Nodes.Add(new TreeNode(xmlDocument.DocumentElement.Name));
      TreeNode treeNode = new TreeNode();
      TreeNode node = treeView.Nodes[0];
      this.AddNode((XmlNode) xmlDocument.DocumentElement, node);
      treeView.ExpandAll();
    }

    private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
    {
      if (inXmlNode.HasChildNodes)
      {
        XmlNodeList childNodes = inXmlNode.ChildNodes;
        for (int index = 0; index <= childNodes.Count - 1; ++index)
        {
          XmlNode childNode = inXmlNode.ChildNodes[index];
          inTreeNode.Nodes.Add(new TreeNode(childNode.Name));
          TreeNode node = inTreeNode.Nodes[index];
          this.AddNode(childNode, node);
        }
      }
      else
        inTreeNode.Text = inXmlNode.OuterXml.Trim();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.treeDetails = new TreeView();
      this.btnSave = new Button();
      this.btnClose = new Button();
      this.saveFileDialog1 = new SaveFileDialog();
      this.SuspendLayout();
      this.treeDetails.Dock = DockStyle.Top;
      this.treeDetails.Location = new Point(0, 0);
      this.treeDetails.Name = "treeDetails";
      this.treeDetails.Size = new Size(754, 471);
      this.treeDetails.TabIndex = 0;
      this.btnSave.Location = new Point(586, 477);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnClose.Location = new Point(667, 477);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.saveFileDialog1.DefaultExt = "*.xml";
      this.saveFileDialog1.Filter = "Xml document|*.xml";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(754, 512);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.treeDetails);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AuthorizationDetails);
      this.Text = "Authorization Details";
      this.Load += new EventHandler(this.AuthorizationDetails_Load);
      this.ResumeLayout(false);
    }
  }
}
