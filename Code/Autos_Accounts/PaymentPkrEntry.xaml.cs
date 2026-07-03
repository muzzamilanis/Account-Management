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
	// Token: 0x0200000F RID: 15
	[DesignerGenerated]
	public partial class PaymentPkrEntry : Window
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00002A1D File Offset: 0x00000E1D
		public PaymentPkrEntry()
		{
			base.Loaded += this.PurchaseAuto_Loaded;
			this.InitializeComponent();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000064BC File Offset: 0x000048BC
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboPaymentPkrAccounts.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboPaymentPkrCust.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Select Customer to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag3 = Operators.CompareString(this.txtPaymentPkrAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtPaymentPkrDetail.Text, string.Empty, false) != 0;
					if (flag3)
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000546C File Offset: 0x0000386C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00002A3D File Offset: 0x00000E3D
		private void PurchaseAuto_Loaded(object sender, RoutedEventArgs e)
		{
			this.loadedbool = true;
			this.controlupdater();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000054B8 File Offset: 0x000038B8
		private void txtPaymentAmount_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Interaction.MsgBox(e.Key.ToString(), MsgBoxStyle.OkOnly, null);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006570 File Offset: 0x00004970
		private void controlupdater()
		{
			string commandText = "SELECT rowid, date(Date) as DATE, Title, Name, CNIC, Phone, Address, PaymentReceived, PaymentReceivable, PaymentPaid from CustomersTable;";
			MainWindow.DSet.Tables["CustomersTable"].Clear();
			MainWindow.DataAdapter.SelectCommand.CommandText = commandText;
			MainWindow.DataAdapter.Fill(MainWindow.DSet, "CustomersTable");
			this.comboPaymentPkrCust.ItemsSource = ((IListSource)MainWindow.DSet.Tables["CustomersTable"]).GetList();
			this.comboPaymentPkrCust.SelectedValuePath = "Name";
			this.comboPaymentPkrCust.DisplayMemberPath = MainWindow.DSet.Tables["CustomersTable"].Columns["Name"].ToString();
			bool flag = Operators.CompareString(this.selectedCustomer, string.Empty, false) == 0;
			if (flag)
			{
				this.comboPaymentPkrCust.SelectedIndex = checked(MainWindow.DSet.Tables["CustomersTable"].Rows.Count - 1);
			}
			else
			{
				this.comboPaymentPkrCust.Text = this.selectedCustomer;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000668C File Offset: 0x00004A8C
		private void BtnAddCust_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddCustomer addCustomer = new AddCustomer();
				addCustomer.ShowDialog();
				bool flag = addCustomer.DialogResult != null && addCustomer.DialogResult.Value;
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
								"INSERT INTO CustomersTable(Date, Title, Name, CNIC, Phone, Address) SELECT '",
								addCustomer.CustdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', '",
								addCustomer.txtCustTitle.Text,
								"', '",
								addCustomer.txtCustName.Text,
								"', '"
							}), addCustomer.txtCnicNo.Value), "', '"), addCustomer.txtCustPhoneNo.Text), "', '"), addCustomer.txtCustAddress.Text), "' WHERE NOT EXISTS(SELECT 1 FROM CustomersTable WHERE Name = '"), addCustomer.txtCustName.Text), "');"));
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						this.selectedCustomer = addCustomer.txtCustName.Text;
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				this.errorlog("When Adding Customer: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004B8C File Offset: 0x00002F8C
		public void errorlog(string StrError)
		{
			StreamWriter streamWriter = new StreamWriter(MyWpfExtension.Application.Info.DirectoryPath + "\\Log.txt", true);
			streamWriter.WriteLine("TimeStamp: " + DateTime.Now.ToString() + "\tMessage: " + StrError);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00002A4E File Offset: 0x00000E4E
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00002A58 File Offset: 0x00000E58
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00002A61 File Offset: 0x00000E61
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00002A6B File Offset: 0x00000E6B
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00002A74 File Offset: 0x00000E74
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00002A7E File Offset: 0x00000E7E
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00002A87 File Offset: 0x00000E87
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00002A91 File Offset: 0x00000E91
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00002A9A File Offset: 0x00000E9A
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00002AA4 File Offset: 0x00000EA4
		internal virtual StackPanel UniPaymentPkrDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00002AAD File Offset: 0x00000EAD
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00002AB7 File Offset: 0x00000EB7
		internal virtual ComboBox comboPaymentPkrCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00002AC0 File Offset: 0x00000EC0
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00002ACA File Offset: 0x00000ECA
		internal virtual Button BtnAddCustatPay { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00002AD3 File Offset: 0x00000ED3
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00002ADD File Offset: 0x00000EDD
		internal virtual DatePicker PaymentPkrdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00002AE6 File Offset: 0x00000EE6
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00002AF0 File Offset: 0x00000EF0
		internal virtual TextBox txtPaymentPkrAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00002AF9 File Offset: 0x00000EF9
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00002B03 File Offset: 0x00000F03
		internal virtual TextBox txtPaymentPkrDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00002B0C File Offset: 0x00000F0C
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00002B16 File Offset: 0x00000F16
		internal virtual ComboBox comboPaymentPkrAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x04000068 RID: 104
		private bool loadedbool;

		// Token: 0x04000069 RID: 105
		public string selectedCustomer;
	}
}
