// Decompiled with JetBrains decompiler
// Type: MB.Utilities.Kerberos.TicketTreeNode
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

using System.Windows.Forms;

namespace MB.Utilities.Kerberos
{
  public class TicketTreeNode : TreeNode
  {
    public Ticket _ticket;

    public TicketTreeNode(Ticket ticket)
    {
      this._ticket = ticket;
      this.Text = this._ticket.ServerName + "@" + this._ticket.RealmName;
      this.Nodes.Add(string.Format("Server Name: {0}", (object) this._ticket.ServerName));
      this.Nodes.Add(string.Format("Realm Name: {0}", (object) this._ticket.RealmName));
      this.Nodes.Add(string.Format("Start Time: {0}", (object) this._ticket.StartTime));
      this.Nodes.Add(string.Format("End Time: {0}", (object) this._ticket.EndTime));
      this.Nodes.Add(string.Format("Renew Time: {0}", (object) this._ticket.RenewTime));
      this.Nodes.Add(string.Format("Encryption Type: {0}", (object) this._ticket.EncryptionType));
      this.Nodes.Add(string.Format("Ticket Flags: {0}", (object) this._ticket.TicketFlags));
    }

    public override string ToString()
    {
      return this._ticket.ServerName + "@" + this._ticket.RealmName;
    }
  }
}
