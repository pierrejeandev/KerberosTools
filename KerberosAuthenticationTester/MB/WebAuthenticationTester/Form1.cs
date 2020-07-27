// Decompiled with JetBrains decompiler
// Type: MB.WebAuthenticationTester.Form1
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using MB.Utilities.Kerberos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace MB.WebAuthenticationTester
{
  public class Form1 : Form
  {
    private TicketsManager _ticketManager = new TicketsManager();
    private WebTestRequest _request;
    private IContainer components;
    private TabPage tabSettings;
    private GroupBox groupBox2;
    private Panel panel1;
    private CredentialsControl proxyCredentials;
    private Label label2;
    private TextBox txtProxyUrl;
    private Label label1;
    private CheckBox cbUseProxy;
    private GroupBox groupBox1;
    private CredentialsControl webCredentials;
    private TabPage tabTest;
    private GroupBox groupBox3;
    private Button btnTest;
    private TextBox txtUrl;
    private Label label3;
    private TabControl tabControl1;
    private Label lblAuthType;
    private Label label6;
    private Label lblHttpResult;
    private Label label4;
    private Label lblUser;
    private Label label7;
    private Label lblDomain;
    private Label label8;
    private Label lblSpn;
    private Label label9;
    private TextBox txtHeaders;
    private Label label5;
    private LinkLabel btnDetails;
    private Label lblRequestDate;
    private Label label11;
    private TabPage tabTickets;
    private Button btnDelete;
    private Button btnRefresh;
    private Label label10;
    private TreeView tvTickets;
    private TabPage tabAbout;
    private Label label14;
    private Label label13;
    private Label label12;
    private PictureBox pictureBox1;
    private LinkLabel linkLabel1;
    private Label label15;
    private TabPage tabManual;
    private Button btnDecode;
    private Button btnClear;
    private TextBox txtDecode;
    private Label label16;
    private Label label17;

    public Form1()
    {
      this.InitializeComponent();
    }

    private void cbUseProxy_CheckedChanged(object sender, EventArgs e)
    {
      this.panel1.Enabled = this.cbUseProxy.Checked;
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        WebProxy webProxy = (WebProxy) null;
        if (this.cbUseProxy.Checked)
        {
          webProxy = new WebProxy(this.txtProxyUrl.Text);
          webProxy.Credentials = this.proxyCredentials.Credentials;
        }
        this._request = new WebTestRequest(this.txtUrl.Text, this.webCredentials.Credentials, (IWebProxy) webProxy);
        this._request.DoRequest();
        this.FillFromRequest();
      }
      catch (Exception ex)
      {
      }
      if (this._request != null)
        this.btnDetails.Visible = true;
      this.Cursor = Cursors.Default;
    }

    private void FillFromRequest()
    {
      this.lblUser.Text = this._request.UserName;
      this.lblDomain.Text = this._request.Domain;
      this.lblSpn.Text = this._request.SPN;
      this.lblHttpResult.Text = this._request.HttpResult;
      this.lblAuthType.Text = this._request.AuthorizationType.ToString();
      this.lblRequestDate.Text = this._request.RequestDate.ToString("dd-MM-yyyy hh:mm:ss");
      this.txtHeaders.Clear();
      this.txtHeaders.AppendText("Request headers:\r\n");
      foreach (string key in this._request.RequestHeadersDictionary.Keys)
        this.txtHeaders.AppendText(string.Format("{0}: {1}\r\n", (object) key, (object) this._request.RequestHeadersDictionary[key]));
      this.txtHeaders.AppendText("\r\nResponse headers:\r\n");
      foreach (string key in this._request.ResponseHeadersDictionary.Keys)
        this.txtHeaders.AppendText(string.Format("{0}: {1}\r\n", (object) key, (object) this._request.ResponseHeadersDictionary[key]));
      if (string.IsNullOrEmpty(this._request.ErrorMessage))
        return;
      int num = (int) MessageBox.Show(this._request.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void btnDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      int num = (int) new AuthorizationDetails(this._request).ShowDialog();
    }

    private void txtUrl_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnTest_Click(sender, (EventArgs) e);
    }

    private void Form1_Activated(object sender, EventArgs e)
    {
      this.txtUrl.Focus();
    }

    private void tabTickets_Enter(object sender, EventArgs e)
    {
      this.RefreshTicketList();
    }

    private void RefreshTicketList()
    {
      List<Ticket> tickets = this._ticketManager.GetTickets();
      this.tvTickets.Nodes.Clear();
      foreach (Ticket ticket in tickets)
        this.AddTicketNode(this.tvTickets, ticket);
    }

    private void AddTicketNode(TreeView tree, Ticket ticket)
    {
      tree.Nodes.Add((TreeNode) new TicketTreeNode(ticket));
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      try
      {
        this.RefreshTicketList();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Some error occured while fetching the tickets.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      try
      {
        TreeNode treeNode = this.tvTickets.SelectedNode;
        if (treeNode == null)
          return;
        while (treeNode.Parent != null)
          treeNode = treeNode.Parent;
        this._ticketManager.PurgeTicket((treeNode as TicketTreeNode)._ticket);
        this.RefreshTicketList();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Some error occured while deleting the ticket.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://mbar.nl/default.aspx?kat0.9.3");
    }

    private void btnDecode_Click(object sender, EventArgs e)
    {
      if (this.txtDecode.Text.Trim().Length <= 0)
        return;
      string text = this.txtDecode.Text;
      string str = "Authorization:";
      string authorizationHeader = text.Trim();
      if (authorizationHeader.StartsWith(str, StringComparison.CurrentCultureIgnoreCase))
        authorizationHeader = authorizationHeader.Remove(0, str.Length).Trim();
      WebTestRequest message = new WebTestRequest(authorizationHeader);
      if (!string.IsNullOrEmpty(message.ErrorMessage))
      {
        int num1 = (int) MessageBox.Show(message.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num2 = (int) new AuthorizationDetails(message).ShowDialog();
      }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.txtDecode.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.tabSettings = new TabPage();
      this.groupBox2 = new GroupBox();
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.txtProxyUrl = new TextBox();
      this.label1 = new Label();
      this.cbUseProxy = new CheckBox();
      this.groupBox1 = new GroupBox();
      this.tabTest = new TabPage();
      this.groupBox3 = new GroupBox();
      this.lblRequestDate = new Label();
      this.label11 = new Label();
      this.txtHeaders = new TextBox();
      this.label5 = new Label();
      this.btnDetails = new LinkLabel();
      this.lblSpn = new Label();
      this.label9 = new Label();
      this.lblDomain = new Label();
      this.label8 = new Label();
      this.lblUser = new Label();
      this.label7 = new Label();
      this.lblAuthType = new Label();
      this.label6 = new Label();
      this.lblHttpResult = new Label();
      this.label4 = new Label();
      this.btnTest = new Button();
      this.txtUrl = new TextBox();
      this.label3 = new Label();
      this.tabControl1 = new TabControl();
      this.tabTickets = new TabPage();
      this.btnDelete = new Button();
      this.btnRefresh = new Button();
      this.label10 = new Label();
      this.tvTickets = new TreeView();
      this.tabManual = new TabPage();
      this.label17 = new Label();
      this.label16 = new Label();
      this.btnDecode = new Button();
      this.btnClear = new Button();
      this.txtDecode = new TextBox();
      this.tabAbout = new TabPage();
      this.linkLabel1 = new LinkLabel();
      this.label15 = new Label();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label12 = new Label();
      this.pictureBox1 = new PictureBox();
      this.proxyCredentials = new CredentialsControl();
      this.webCredentials = new CredentialsControl();
      this.tabSettings.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabTest.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabTickets.SuspendLayout();
      this.tabManual.SuspendLayout();
      this.tabAbout.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.tabSettings.Controls.Add((Control) this.groupBox2);
      this.tabSettings.Controls.Add((Control) this.groupBox1);
      this.tabSettings.Location = new Point(4, 22);
      this.tabSettings.Name = "tabSettings";
      this.tabSettings.Padding = new Padding(3);
      this.tabSettings.Size = new Size(510, 331);
      this.tabSettings.TabIndex = 1;
      this.tabSettings.Text = "Settings";
      this.tabSettings.UseVisualStyleBackColor = true;
      this.groupBox2.Controls.Add((Control) this.panel1);
      this.groupBox2.Controls.Add((Control) this.cbUseProxy);
      this.groupBox2.Location = new Point(246, 6);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(256, 317);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Proxy Settings";
      this.panel1.Controls.Add((Control) this.proxyCredentials);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.txtProxyUrl);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Enabled = false;
      this.panel1.Location = new Point(7, 44);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(243, 266);
      this.panel1.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(91, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Proxy Credentials:";
      this.txtProxyUrl.Location = new Point(7, 21);
      this.txtProxyUrl.Name = "txtProxyUrl";
      this.txtProxyUrl.Size = new Size(233, 20);
      this.txtProxyUrl.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(52, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Proxy Url:";
      this.cbUseProxy.AutoSize = true;
      this.cbUseProxy.Location = new Point(7, 20);
      this.cbUseProxy.Name = "cbUseProxy";
      this.cbUseProxy.Size = new Size(106, 17);
      this.cbUseProxy.TabIndex = 0;
      this.cbUseProxy.Text = "Use Proxy server";
      this.cbUseProxy.UseVisualStyleBackColor = true;
      this.cbUseProxy.CheckedChanged += new EventHandler(this.cbUseProxy_CheckedChanged);
      this.groupBox1.Controls.Add((Control) this.webCredentials);
      this.groupBox1.Location = new Point(8, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(233, 228);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Web request credentials";
      this.tabTest.Controls.Add((Control) this.groupBox3);
      this.tabTest.Controls.Add((Control) this.btnTest);
      this.tabTest.Controls.Add((Control) this.txtUrl);
      this.tabTest.Controls.Add((Control) this.label3);
      this.tabTest.Location = new Point(4, 22);
      this.tabTest.Name = "tabTest";
      this.tabTest.Padding = new Padding(3);
      this.tabTest.Size = new Size(510, 331);
      this.tabTest.TabIndex = 0;
      this.tabTest.Text = "Test";
      this.tabTest.UseVisualStyleBackColor = true;
      this.groupBox3.Controls.Add((Control) this.lblRequestDate);
      this.groupBox3.Controls.Add((Control) this.label11);
      this.groupBox3.Controls.Add((Control) this.txtHeaders);
      this.groupBox3.Controls.Add((Control) this.label5);
      this.groupBox3.Controls.Add((Control) this.btnDetails);
      this.groupBox3.Controls.Add((Control) this.lblSpn);
      this.groupBox3.Controls.Add((Control) this.label9);
      this.groupBox3.Controls.Add((Control) this.lblDomain);
      this.groupBox3.Controls.Add((Control) this.label8);
      this.groupBox3.Controls.Add((Control) this.lblUser);
      this.groupBox3.Controls.Add((Control) this.label7);
      this.groupBox3.Controls.Add((Control) this.lblAuthType);
      this.groupBox3.Controls.Add((Control) this.label6);
      this.groupBox3.Controls.Add((Control) this.lblHttpResult);
      this.groupBox3.Controls.Add((Control) this.label4);
      this.groupBox3.Location = new Point(7, 36);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(492, 287);
      this.groupBox3.TabIndex = 3;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Result";
      this.lblRequestDate.AutoSize = true;
      this.lblRequestDate.Location = new Point(118, 16);
      this.lblRequestDate.Name = "lblRequestDate";
      this.lblRequestDate.Size = new Size(0, 13);
      this.lblRequestDate.TabIndex = 14;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(3, 16);
      this.label11.Name = "label11";
      this.label11.Size = new Size(74, 13);
      this.label11.TabIndex = 13;
      this.label11.Text = "Request date:";
      this.txtHeaders.BorderStyle = BorderStyle.FixedSingle;
      this.txtHeaders.Location = new Point(6, 128);
      this.txtHeaders.Multiline = true;
      this.txtHeaders.Name = "txtHeaders";
      this.txtHeaders.ReadOnly = true;
      this.txtHeaders.ScrollBars = ScrollBars.Both;
      this.txtHeaders.Size = new Size(480, 153);
      this.txtHeaders.TabIndex = 12;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 112);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "HTTP Headers:";
      this.btnDetails.AutoSize = true;
      this.btnDetails.Location = new Point(449, 16);
      this.btnDetails.Name = "btnDetails";
      this.btnDetails.Size = new Size(37, 13);
      this.btnDetails.TabIndex = 10;
      this.btnDetails.TabStop = true;
      this.btnDetails.Text = "details";
      this.btnDetails.Visible = false;
      this.btnDetails.LinkClicked += new LinkLabelLinkClickedEventHandler(this.btnDetails_LinkClicked);
      this.lblSpn.AutoSize = true;
      this.lblSpn.Location = new Point(118, 96);
      this.lblSpn.Name = "lblSpn";
      this.lblSpn.Size = new Size(0, 13);
      this.lblSpn.TabIndex = 9;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(3, 96);
      this.label9.Name = "label9";
      this.label9.Size = new Size(32, 13);
      this.label9.TabIndex = 8;
      this.label9.Text = "SPN:";
      this.lblDomain.AutoSize = true;
      this.lblDomain.Location = new Point(118, 80);
      this.lblDomain.Name = "lblDomain";
      this.lblDomain.Size = new Size(0, 13);
      this.lblDomain.TabIndex = 7;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 80);
      this.label8.Name = "label8";
      this.label8.Size = new Size(46, 13);
      this.label8.TabIndex = 6;
      this.label8.Text = "Domain:";
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new Point(118, 64);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(0, 13);
      this.lblUser.TabIndex = 5;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 64);
      this.label7.Name = "label7";
      this.label7.Size = new Size(32, 13);
      this.label7.TabIndex = 4;
      this.label7.Text = "User:";
      this.lblAuthType.AutoSize = true;
      this.lblAuthType.Location = new Point(118, 48);
      this.lblAuthType.Name = "lblAuthType";
      this.lblAuthType.Size = new Size(0, 13);
      this.lblAuthType.TabIndex = 3;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 48);
      this.label6.Name = "label6";
      this.label6.Size = new Size(101, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "Authentication type:";
      this.lblHttpResult.AutoSize = true;
      this.lblHttpResult.Location = new Point(118, 32);
      this.lblHttpResult.Name = "lblHttpResult";
      this.lblHttpResult.Size = new Size(0, 13);
      this.lblHttpResult.TabIndex = 1;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 32);
      this.label4.Name = "label4";
      this.label4.Size = new Size(58, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Http result:";
      this.btnTest.Location = new Point(424, 5);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new Size(75, 23);
      this.btnTest.TabIndex = 2;
      this.btnTest.Text = "Test";
      this.btnTest.UseVisualStyleBackColor = true;
      this.btnTest.Click += new EventHandler(this.btnTest_Click);
      this.txtUrl.Location = new Point(34, 7);
      this.txtUrl.Name = "txtUrl";
      this.txtUrl.Size = new Size(382, 20);
      this.txtUrl.TabIndex = 1;
      this.txtUrl.KeyUp += new KeyEventHandler(this.txtUrl_KeyUp);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(23, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Url:";
      this.tabControl1.Controls.Add((Control) this.tabTest);
      this.tabControl1.Controls.Add((Control) this.tabSettings);
      this.tabControl1.Controls.Add((Control) this.tabTickets);
      this.tabControl1.Controls.Add((Control) this.tabManual);
      this.tabControl1.Controls.Add((Control) this.tabAbout);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(518, 357);
      this.tabControl1.TabIndex = 3;
      this.tabTickets.Controls.Add((Control) this.btnDelete);
      this.tabTickets.Controls.Add((Control) this.btnRefresh);
      this.tabTickets.Controls.Add((Control) this.label10);
      this.tabTickets.Controls.Add((Control) this.tvTickets);
      this.tabTickets.Location = new Point(4, 22);
      this.tabTickets.Name = "tabTickets";
      this.tabTickets.Padding = new Padding(3);
      this.tabTickets.Size = new Size(510, 331);
      this.tabTickets.TabIndex = 3;
      this.tabTickets.Text = "Tickets";
      this.tabTickets.UseVisualStyleBackColor = true;
      this.tabTickets.Enter += new EventHandler(this.tabTickets_Enter);
      this.btnDelete.Location = new Point(427, 302);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 3;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnRefresh.Location = new Point(8, 302);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(75, 23);
      this.btnRefresh.TabIndex = 2;
      this.btnRefresh.Text = "Refresh";
      this.btnRefresh.UseVisualStyleBackColor = true;
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(9, 7);
      this.label10.Name = "label10";
      this.label10.Size = new Size(167, 13);
      this.label10.TabIndex = 1;
      this.label10.Text = "Kerberos Tickets (of current user):";
      this.tvTickets.Location = new Point(8, 23);
      this.tvTickets.Name = "tvTickets";
      this.tvTickets.Size = new Size(494, 273);
      this.tvTickets.TabIndex = 0;
      this.tabManual.Controls.Add((Control) this.label17);
      this.tabManual.Controls.Add((Control) this.label16);
      this.tabManual.Controls.Add((Control) this.btnDecode);
      this.tabManual.Controls.Add((Control) this.btnClear);
      this.tabManual.Controls.Add((Control) this.txtDecode);
      this.tabManual.Location = new Point(4, 22);
      this.tabManual.Name = "tabManual";
      this.tabManual.Size = new Size(510, 331);
      this.tabManual.TabIndex = 5;
      this.tabManual.Text = "Decode";
      this.tabManual.UseVisualStyleBackColor = true;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(11, 26);
      this.label17.Name = "label17";
      this.label17.Size = new Size(304, 13);
      this.label17.TabIndex = 4;
      this.label17.Text = "Just paste the Authorization header below and click \"Decode\".";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(8, 9);
      this.label16.Name = "label16";
      this.label16.Size = new Size(224, 13);
      this.label16.TabIndex = 3;
      this.label16.Text = "Manual decoding of the Authorization Header.";
      this.btnDecode.Location = new Point(347, 300);
      this.btnDecode.Name = "btnDecode";
      this.btnDecode.Size = new Size(75, 23);
      this.btnDecode.TabIndex = 2;
      this.btnDecode.Text = "Decode";
      this.btnDecode.UseVisualStyleBackColor = true;
      this.btnDecode.Click += new EventHandler(this.btnDecode_Click);
      this.btnClear.Location = new Point(427, 300);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 1;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.txtDecode.Location = new Point(4, 44);
      this.txtDecode.Multiline = true;
      this.txtDecode.Name = "txtDecode";
      this.txtDecode.ScrollBars = ScrollBars.Both;
      this.txtDecode.Size = new Size(503, 250);
      this.txtDecode.TabIndex = 0;
      this.tabAbout.Controls.Add((Control) this.linkLabel1);
      this.tabAbout.Controls.Add((Control) this.label15);
      this.tabAbout.Controls.Add((Control) this.label14);
      this.tabAbout.Controls.Add((Control) this.label13);
      this.tabAbout.Controls.Add((Control) this.label12);
      this.tabAbout.Controls.Add((Control) this.pictureBox1);
      this.tabAbout.Location = new Point(4, 22);
      this.tabAbout.Name = "tabAbout";
      this.tabAbout.Size = new Size(510, 331);
      this.tabAbout.TabIndex = 4;
      this.tabAbout.Text = "About";
      this.tabAbout.UseVisualStyleBackColor = true;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new Point(194, 100);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(149, 13);
      this.linkLabel1.TabIndex = 5;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "http://blog.michelbarneveld.nl";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(144, 101);
      this.label15.Name = "label15";
      this.label15.Size = new Size(52, 13);
      this.label15.TabIndex = 4;
      this.label15.Text = "Website: ";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(144, 78);
      this.label14.Name = "label14";
      this.label14.Size = new Size(146, 13);
      this.label14.TabIndex = 3;
      this.label14.Text = "Created by: Michel Barneveld";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(141, 42);
      this.label13.Name = "label13";
      this.label13.Size = new Size(93, 13);
      this.label13.TabIndex = 2;
      this.label13.Text = "Version 0.9.3 beta";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(139, 17);
      this.label12.Name = "label12";
      this.label12.Size = new Size(339, 25);
      this.label12.TabIndex = 1;
      this.label12.Text = "Kerberos Authentication Tester";
      this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(8, 14);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(112, 112);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.proxyCredentials.Location = new Point(10, 65);
      this.proxyCredentials.Name = "proxyCredentials";
      this.proxyCredentials.Size = new Size(216, 196);
      this.proxyCredentials.TabIndex = 3;
      this.webCredentials.Location = new Point(7, 20);
      this.webCredentials.Name = "webCredentials";
      this.webCredentials.Size = new Size(225, 206);
      this.webCredentials.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(518, 357);
      this.Controls.Add((Control) this.tabControl1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Form1);
      this.Text = "Kerberos Authentication Tester (v 0.9.3 beta)";
      this.Activated += new EventHandler(this.Form1_Activated);
      this.tabSettings.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.tabTest.ResumeLayout(false);
      this.tabTest.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabTickets.ResumeLayout(false);
      this.tabTickets.PerformLayout();
      this.tabManual.ResumeLayout(false);
      this.tabManual.PerformLayout();
      this.tabAbout.ResumeLayout(false);
      this.tabAbout.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
