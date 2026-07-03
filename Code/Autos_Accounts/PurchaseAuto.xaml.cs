using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Xceed.Wpf.Toolkit;

namespace Autos_Accounts
{
	// Token: 0x02000010 RID: 16
	[DesignerGenerated]
	public partial class PurchaseAuto : Window
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00002B1F File Offset: 0x00000F1F
		public PurchaseAuto()
		{
			base.Loaded += this.PurchaseAuto_Loaded;
			this.InitializeComponent();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006AA4 File Offset: 0x00004EA4
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.txtPaymentAmount.Text, string.Empty, false) == 0;
			if (flag)
			{
				this.txtPaymentAmount.Text = Conversions.ToString(0);
			}
			bool flag2 = Operators.CompareString(this.txtPurChassisNo.Text, string.Empty, false) == 0;
			if (flag2)
			{
				Interaction.MsgBox("Please Enter Chassis No to Continue", MsgBoxStyle.OkOnly, null);
				this.txtPurChassisNo.Focus();
			}
			else
			{
				bool flag3 = Operators.CompareString(this.txtPaymentPrice.Text, string.Empty, false) == 0;
				if (flag3)
				{
					Interaction.MsgBox("Please Enter Payment Price to Continue", MsgBoxStyle.OkOnly, null);
					this.txtPaymentPrice.Focus();
				}
				else
				{
					bool flag4 = Operators.CompareString(this.comboDutyAgents.Text, string.Empty, false) == 0;
					if (flag4)
					{
						Interaction.MsgBox("Please Select Duty Agent to Continue", MsgBoxStyle.OkOnly, null);
						this.comboDutyAgents.Focus();
					}
					else
					{
						bool flag5 = Conversions.ToDouble(this.txtPaymentAmount.Text) > 0.0;
						if (flag5)
						{
							bool flag6 = Operators.CompareString(this.txtPaymentRate.Text, string.Empty, false) == 0;
							if (flag6)
							{
								Interaction.MsgBox("Please Enter Payment Rate to Continue", MsgBoxStyle.OkOnly, null);
								this.txtPaymentRate.Focus();
							}
							else
							{
								bool flag7 = Operators.CompareString(this.AccInfoText.Text, "Add Auto Purchase Entry", false) == 0 && MainWindow.DSet.Tables["StocksTable"].Select("Chassis = '" + this.txtPurChassisNo.Text + "'").Count<DataRow>() > 0;
								if (flag7)
								{
									Interaction.MsgBox("Chassis no already exists", MsgBoxStyle.OkOnly, null);
									this.txtPurChassisNo.Focus();
								}
								else
								{
									base.DialogResult = new bool?(true);
									base.Close();
								}
							}
						}
						else
						{
							bool flag8 = Operators.CompareString(this.AccInfoText.Text, "Add Auto Purchase Entry", false) == 0 && MainWindow.DSet.Tables["StocksTable"].Select("Chassis = '" + this.txtPurChassisNo.Text + "'").Count<DataRow>() > 0;
							if (flag8)
							{
								Interaction.MsgBox("Chassis no already exists", MsgBoxStyle.OkOnly, null);
								this.txtPurChassisNo.Focus();
							}
							else
							{
								base.DialogResult = new bool?(true);
								base.Close();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000546C File Offset: 0x0000386C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006D20 File Offset: 0x00005120
		private void txtPaymentAmount_TextChanged(object sender, TextChangedEventArgs e)
		{
			bool flag = this.loadedbool && this.txtPaymentAmount != null;
			if (flag)
			{
				bool flag2 = this.txtPaymentAmount.Text.Length != 0 && this.txtPaymentRate.Text.Length != 0;
				if (flag2)
				{
					double num;
					double num2;
					bool flag3 = double.TryParse(this.txtPaymentAmount.Text, out num) && double.TryParse(this.txtPaymentRate.Text, out num2);
					if (flag3)
					{
						this.txtPaymentAmountPkr.Text = Conversions.ToString(num2 * num);
					}
				}
				else
				{
					this.txtPaymentAmountPkr.Text = "";
				}
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00002B3F File Offset: 0x00000F3F
		private void PurchaseAuto_Loaded(object sender, RoutedEventArgs e)
		{
			this.loadedbool = true;
			this.controlupdater();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000054B8 File Offset: 0x000038B8
		private void txtPaymentAmount_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Interaction.MsgBox(e.Key.ToString(), MsgBoxStyle.OkOnly, null);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006DD4 File Offset: 0x000051D4
		private void txtPaymentPrice_TextChanged(object sender, TextChangedEventArgs e)
		{
			bool flag = Operators.CompareString(this.txtPaymentPrice.Text, string.Empty, false) != 0;
			if (flag)
			{
				int? value = this.intPaymentAmount.Value;
				double? num = (value != null) ? new double?((double)value.GetValueOrDefault()) : null;
				double num2 = Conversions.ToDouble(this.txtPaymentPrice.Text);
				bool valueOrDefault = ((num != null) ? new bool?(num.GetValueOrDefault() > num2) : null).GetValueOrDefault();
				if (valueOrDefault)
				{
					this.intPaymentAmount.Value = new int?(Conversions.ToInteger(this.txtPaymentPrice.Text));
				}
			}
			else
			{
				this.intPaymentAmount.Value = new int?(0);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006EB0 File Offset: 0x000052B0
		private void btnDutyAddAgent_Click(object sender, RoutedEventArgs e)
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
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Agent: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000070A4 File Offset: 0x000054A4
		private void controlupdater()
		{
			string commandText = "SELECT rowid, date(Date) as DATE, Name, CNIC, Phone, Address, PaymentReceivable, PaymentPaid from AgentsTable;";
			MainWindow.DSet.Tables["AgentsTable"].Clear();
			MainWindow.DataAdapter.SelectCommand.CommandText = commandText;
			MainWindow.DataAdapter.Fill(MainWindow.DSet, "AgentsTable");
			this.comboDutyAgents.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AgentsTable"]).GetList();
			this.comboDutyAgents.DisplayMemberPath = MainWindow.DSet.Tables["AgentsTable"].Columns["Name"].ToString();
			this.comboDutyAgents.SelectedValuePath = "Name";
			bool flag = Operators.CompareString(this.selectedAgent, string.Empty, false) == 0;
			if (flag)
			{
				this.comboDutyAgents.SelectedIndex = checked(MainWindow.DSet.Tables["AgentsTable"].Rows.Count - 1);
			}
			else
			{
				this.comboDutyAgents.Text = this.selectedAgent;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00002B50 File Offset: 0x00000F50
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00002B5A File Offset: 0x00000F5A
		internal virtual DockPanel dockAddAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00002B63 File Offset: 0x00000F63
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00002B6D File Offset: 0x00000F6D
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00002B76 File Offset: 0x00000F76
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00002B80 File Offset: 0x00000F80
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00002B89 File Offset: 0x00000F89
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00002B93 File Offset: 0x00000F93
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00002B9C File Offset: 0x00000F9C
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00002BA6 File Offset: 0x00000FA6
		internal virtual DatePicker PaymentdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00002BAF File Offset: 0x00000FAF
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00002BB9 File Offset: 0x00000FB9
		internal virtual TextBox txtPurChassisNo { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00002BC2 File Offset: 0x00000FC2
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00002BCC File Offset: 0x00000FCC
		internal virtual TextBox txtPurModel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00002BD5 File Offset: 0x00000FD5
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00002BDF File Offset: 0x00000FDF
		internal virtual TextBox txtPurColor { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00002BE8 File Offset: 0x00000FE8
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00002BF2 File Offset: 0x00000FF2
		internal virtual TextBox txtPaymentPrice { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00002BFB File Offset: 0x00000FFB
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00002C05 File Offset: 0x00001005
		internal virtual IntegerUpDown intPaymentAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00002C0E File Offset: 0x0000100E
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00002C18 File Offset: 0x00001018
		internal virtual TextBox txtPaymentAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00002C21 File Offset: 0x00001021
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00002C2B File Offset: 0x0000102B
		internal virtual TextBox txtPaymentRate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00002C34 File Offset: 0x00001034
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00002C3E File Offset: 0x0000103E
		internal virtual TextBox txtPaymentAmountPkr { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00002C47 File Offset: 0x00001047
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00002C51 File Offset: 0x00001051
		internal virtual TextBox txtPurDuty { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00002C5A File Offset: 0x0000105A
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00002C64 File Offset: 0x00001064
		internal virtual ComboBox comboDutyAgents { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00002C6D File Offset: 0x0000106D
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00002C77 File Offset: 0x00001077
		internal virtual Button btnDutyAddAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00002C80 File Offset: 0x00001080
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00002C8A File Offset: 0x0000108A
		internal virtual TextBox txtPurMiscExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00002C93 File Offset: 0x00001093
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00002C9D File Offset: 0x0000109D
		internal virtual ComboBox comboPaymentAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00002CA6 File Offset: 0x000010A6
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00002CB0 File Offset: 0x000010B0
		internal virtual TextBox txtPurComments { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x04000076 RID: 118
		private bool loadedbool;

		// Token: 0x04000077 RID: 119
		public string selectedAgent;
	}
}
