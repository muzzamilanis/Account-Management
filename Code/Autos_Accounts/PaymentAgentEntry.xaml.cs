using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Autos_Accounts.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts
{
	// Token: 0x0200000E RID: 14
	[DesignerGenerated]
	public partial class PaymentAgentEntry : Window
	{
		// Token: 0x060000ED RID: 237 RVA: 0x0000291B File Offset: 0x00000D1B
		public PaymentAgentEntry()
		{
			base.Loaded += this.PurchaseAuto_Loaded;
			this.InitializeComponent();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005EEC File Offset: 0x000042EC
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboPaymentAgentAccounts.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboPaymentAgent.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Select Agent to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag3 = Operators.CompareString(this.txtPaymentAgentAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtPaymentAgentDetail.Text, string.Empty, false) != 0;
					if (flag3)
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000546C File Offset: 0x0000386C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000293B File Offset: 0x00000D3B
		private void PurchaseAuto_Loaded(object sender, RoutedEventArgs e)
		{
			this.loadedbool = true;
			this.controlupdater();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000054B8 File Offset: 0x000038B8
		private void txtPaymentAmount_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Interaction.MsgBox(e.Key.ToString(), MsgBoxStyle.OkOnly, null);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005FA0 File Offset: 0x000043A0
		private void controlupdater()
		{
			string commandText = "SELECT rowid, date(Date) as DATE, Name, CNIC, Phone, Address, PaymentReceivable, PaymentPaid from AgentsTable;";
			MainWindow.DSet.Tables["AgentsTable"].Clear();
			MainWindow.DataAdapter.SelectCommand.CommandText = commandText;
			MainWindow.DataAdapter.Fill(MainWindow.DSet, "AgentsTable");
			this.comboPaymentAgent.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AgentsTable"]).GetList();
			this.comboPaymentAgent.SelectedValuePath = "Name";
			this.comboPaymentAgent.DisplayMemberPath = MainWindow.DSet.Tables["AgentsTable"].Columns["Name"].ToString();
			bool flag = Operators.CompareString(this.selectedAgent, string.Empty, false) == 0;
			if (flag)
			{
				this.comboPaymentAgent.SelectedIndex = checked(MainWindow.DSet.Tables["AgentsTable"].Rows.Count - 1);
			}
			else
			{
				this.comboPaymentAgent.Text = this.selectedAgent;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000060BC File Offset: 0x000044BC
		private void BtnAddAgent_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddAgentForm addAgentForm = new AddAgentForm();
				addAgentForm.ShowDialog();
				bool flag = addAgentForm.DialogResult != null && addAgentForm.DialogResult.Value;
				if (flag)
				{
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							sqliteCommand2.Connection = MainWindow.Connection;
							sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
							{
								"INSERT INTO AgentsTable(Date, Name, CNIC, Phone, Address) SELECT '",
								addAgentForm.AgentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', '",
								addAgentForm.txtAgentName.Text,
								"', '"
							}), addAgentForm.txtCnicNo.Value), "', '"), addAgentForm.txtAgentPhoneNo.Text), "', '"), addAgentForm.txtAgentAddress.Text), "' WHERE NOT EXISTS(SELECT 1 FROM AgentsTable WHERE Name = '"), addAgentForm.txtAgentName.Text), "');"));
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						this.selectedAgent = addAgentForm.txtAgentName.Text;
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				this.errorlog("When Adding Agent: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004B8C File Offset: 0x00002F8C
		public void errorlog(string StrError)
		{
			StreamWriter streamWriter = new StreamWriter(MyWpfExtension.Application.Info.DirectoryPath + "\\Log.txt", true);
			streamWriter.WriteLine("TimeStamp: " + DateTime.Now.ToString() + "\tMessage: " + StrError);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000294C File Offset: 0x00000D4C
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00002956 File Offset: 0x00000D56
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000295F File Offset: 0x00000D5F
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00002969 File Offset: 0x00000D69
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00002972 File Offset: 0x00000D72
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000297C File Offset: 0x00000D7C
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00002985 File Offset: 0x00000D85
		// (set) Token: 0x060000FE RID: 254 RVA: 0x0000298F File Offset: 0x00000D8F
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00002998 File Offset: 0x00000D98
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000029A2 File Offset: 0x00000DA2
		internal virtual StackPanel UniPaymentAgentDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000029AB File Offset: 0x00000DAB
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000029B5 File Offset: 0x00000DB5
		internal virtual ComboBox comboPaymentAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000029BE File Offset: 0x00000DBE
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000029C8 File Offset: 0x00000DC8
		internal virtual Button BtnAddAgentatPay { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000029D1 File Offset: 0x00000DD1
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000029DB File Offset: 0x00000DDB
		internal virtual DatePicker PaymentAgentdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000029E4 File Offset: 0x00000DE4
		// (set) Token: 0x06000108 RID: 264 RVA: 0x000029EE File Offset: 0x00000DEE
		internal virtual TextBox txtPaymentAgentAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000029F7 File Offset: 0x00000DF7
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00002A01 File Offset: 0x00000E01
		internal virtual TextBox txtPaymentAgentDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00002A0A File Offset: 0x00000E0A
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00002A14 File Offset: 0x00000E14
		internal virtual ComboBox comboPaymentAgentAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x0400005A RID: 90
		private bool loadedbool;

		// Token: 0x0400005B RID: 91
		public string selectedAgent;
	}
}
