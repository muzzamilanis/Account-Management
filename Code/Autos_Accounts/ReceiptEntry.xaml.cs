using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
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
	// Token: 0x02000011 RID: 17
	[DesignerGenerated]
	public partial class ReceiptEntry : Window
	{
		// Token: 0x06000164 RID: 356 RVA: 0x00002CB9 File Offset: 0x000010B9
		public ReceiptEntry()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000466C File Offset: 0x00002A6C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[-+]?[.][0-9]+$|^[-+]?[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007574 File Offset: 0x00005974
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboReceiptAccounts.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboReceiptCust.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Select Customer to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag3 = Operators.CompareString(this.txtReceiptAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtReceiptDetail.Text, string.Empty, false) != 0;
					if (flag3)
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00002CC7 File Offset: 0x000010C7
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00002CD1 File Offset: 0x000010D1
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00002CDA File Offset: 0x000010DA
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00002CE4 File Offset: 0x000010E4
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00002CED File Offset: 0x000010ED
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00002CF7 File Offset: 0x000010F7
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00002D00 File Offset: 0x00001100
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00002D0A File Offset: 0x0000110A
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00002D13 File Offset: 0x00001113
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00002D1D File Offset: 0x0000111D
		internal virtual StackPanel UniReceiptDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00002D26 File Offset: 0x00001126
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00002D30 File Offset: 0x00001130
		internal virtual ComboBox comboReceiptCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00002D39 File Offset: 0x00001139
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00002D43 File Offset: 0x00001143
		internal virtual DatePicker ReceiptdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00002D4C File Offset: 0x0000114C
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00002D56 File Offset: 0x00001156
		internal virtual TextBox txtReceiptAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00002D5F File Offset: 0x0000115F
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00002D69 File Offset: 0x00001169
		internal virtual TextBox txtReceiptDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00002D72 File Offset: 0x00001172
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00002D7C File Offset: 0x0000117C
		internal virtual ComboBox comboReceiptAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
