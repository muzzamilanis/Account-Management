using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts
{
	// Token: 0x0200000B RID: 11
	[DesignerGenerated]
	public partial class DutyExpEntry : Window
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00002694 File Offset: 0x00000A94
		public DutyExpEntry()
		{
			base.Loaded += this.PurchaseAuto_Loaded;
			this.InitializeComponent();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000538C File Offset: 0x0000378C
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboDutyChassis.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Chassis No. to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboDutyAgents.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Select Agent to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag3 = MainWindow.convertInteger(this.txtDutyExpAmount.Text) < 0;
					if (flag3)
					{
						Interaction.MsgBox("Negative Amount Not Allowed", MsgBoxStyle.Exclamation, null);
					}
					else
					{
						bool flag4 = Operators.CompareString(this.txtDutyExpAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtDutyExpDetail.Text, string.Empty, false) != 0;
						if (flag4)
						{
							base.DialogResult = new bool?(true);
							base.Close();
						}
					}
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000546C File Offset: 0x0000386C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000026B4 File Offset: 0x00000AB4
		private void PurchaseAuto_Loaded(object sender, RoutedEventArgs e)
		{
			this.loadedbool = true;
			this.controlupdater();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000054B8 File Offset: 0x000038B8
		private void txtPaymentAmount_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Interaction.MsgBox(e.Key.ToString(), MsgBoxStyle.OkOnly, null);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000054E4 File Offset: 0x000038E4
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

		// Token: 0x0600009F RID: 159 RVA: 0x000056D8 File Offset: 0x00003AD8
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000026C5 File Offset: 0x00000AC5
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000026CF File Offset: 0x00000ACF
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000026D8 File Offset: 0x00000AD8
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000026E2 File Offset: 0x00000AE2
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000026EB File Offset: 0x00000AEB
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x000026F5 File Offset: 0x00000AF5
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000026FE File Offset: 0x00000AFE
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00002708 File Offset: 0x00000B08
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00002711 File Offset: 0x00000B11
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x0000271B File Offset: 0x00000B1B
		internal virtual StackPanel UniDutyDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00002724 File Offset: 0x00000B24
		// (set) Token: 0x060000AB RID: 171 RVA: 0x0000272E File Offset: 0x00000B2E
		internal virtual ComboBox comboDutyChassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00002737 File Offset: 0x00000B37
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00002741 File Offset: 0x00000B41
		internal virtual DatePicker DutydatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000274A File Offset: 0x00000B4A
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00002754 File Offset: 0x00000B54
		internal virtual TextBox txtDutyExpAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000275D File Offset: 0x00000B5D
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00002767 File Offset: 0x00000B67
		internal virtual TextBox txtDutyExpDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00002770 File Offset: 0x00000B70
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x0000277A File Offset: 0x00000B7A
		internal virtual ComboBox comboDutyAgents { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00002783 File Offset: 0x00000B83
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000278D File Offset: 0x00000B8D
		internal virtual Button btnDutyAddAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x04000037 RID: 55
		private bool loadedbool;

		// Token: 0x04000038 RID: 56
		public string selectedAgent;
	}
}
