// Decompiled with JetBrains decompiler
// Type: MB.WebAuthenticationTester.CredentialsControl
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace MB.WebAuthenticationTester
{
  public class CredentialsControl : UserControl
  {
    private IContainer components;
    private RadioButton rbNone;
    private RadioButton rbDefault;
    private RadioButton rbCustom;
    private Panel panel1;
    private TextBox txtDomain;
    private Label label3;
    private TextBox txtPassword;
    private Label label2;
    private TextBox txtUsername;
    private Label label1;

    public CredentialsControl()
    {
      this.InitializeComponent();
    }

    private void rbCustom_CheckedChanged(object sender, EventArgs e)
    {
      this.panel1.Enabled = this.rbCustom.Checked;
    }

    public ICredentials Credentials
    {
      get
      {
        if (this.rbDefault.Checked)
          return (ICredentials) CredentialCache.DefaultNetworkCredentials;
        if (this.rbCustom.Checked && this.txtDomain.Text.Trim() == "")
          return (ICredentials) new NetworkCredential(this.txtUsername.Text, this.txtPassword.Text);
        return this.rbCustom.Checked ? (ICredentials) new NetworkCredential(this.txtUsername.Text, this.txtPassword.Text, this.txtDomain.Text) : (ICredentials) null;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rbNone = new RadioButton();
      this.rbDefault = new RadioButton();
      this.rbCustom = new RadioButton();
      this.panel1 = new Panel();
      this.txtDomain = new TextBox();
      this.label3 = new Label();
      this.txtPassword = new TextBox();
      this.label2 = new Label();
      this.txtUsername = new TextBox();
      this.label1 = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.rbNone.AutoSize = true;
      this.rbNone.Location = new Point(4, 4);
      this.rbNone.Name = "rbNone";
      this.rbNone.Size = new Size(94, 17);
      this.rbNone.TabIndex = 0;
      this.rbNone.TabStop = true;
      this.rbNone.Text = "No Credentials";
      this.rbNone.UseVisualStyleBackColor = true;
      this.rbDefault.AutoSize = true;
      this.rbDefault.Checked = true;
      this.rbDefault.Location = new Point(4, 23);
      this.rbDefault.Name = "rbDefault";
      this.rbDefault.Size = new Size(114, 17);
      this.rbDefault.TabIndex = 1;
      this.rbDefault.TabStop = true;
      this.rbDefault.Text = "Default Credentials";
      this.rbDefault.UseVisualStyleBackColor = true;
      this.rbCustom.AutoSize = true;
      this.rbCustom.Location = new Point(4, 42);
      this.rbCustom.Name = "rbCustom";
      this.rbCustom.Size = new Size(115, 17);
      this.rbCustom.TabIndex = 2;
      this.rbCustom.TabStop = true;
      this.rbCustom.Text = "Custom Credentials";
      this.rbCustom.UseVisualStyleBackColor = true;
      this.rbCustom.CheckedChanged += new EventHandler(this.rbCustom_CheckedChanged);
      this.panel1.Controls.Add((Control) this.txtDomain);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.txtPassword);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.txtUsername);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Enabled = false;
      this.panel1.Location = new Point(22, 67);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(188, 123);
      this.panel1.TabIndex = 3;
      this.txtDomain.Location = new Point(7, 99);
      this.txtDomain.Name = "txtDomain";
      this.txtDomain.Size = new Size(176, 20);
      this.txtDomain.TabIndex = 5;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 83);
      this.label3.Name = "label3";
      this.label3.Size = new Size(46, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Domain:";
      this.txtPassword.Location = new Point(7, 60);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(176, 20);
      this.txtPassword.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 44);
      this.label2.Name = "label2";
      this.label2.Size = new Size(56, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Password:";
      this.txtUsername.Location = new Point(7, 21);
      this.txtUsername.Name = "txtUsername";
      this.txtUsername.Size = new Size(176, 20);
      this.txtUsername.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Username:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.rbCustom);
      this.Controls.Add((Control) this.rbDefault);
      this.Controls.Add((Control) this.rbNone);
      this.Name = nameof (CredentialsControl);
      this.Size = new Size(216, 196);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
