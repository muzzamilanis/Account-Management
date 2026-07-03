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
	// Token: 0x0200000C RID: 12
	[DesignerGenerated]
	public partial class MiscExpEntry : Window
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00002796 File Offset: 0x00000B96
		public MiscExpEntry()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000466C File Offset: 0x00002A6C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[-+]?[.][0-9]+$|^[-+]?[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000059EC File Offset: 0x00003DEC
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboMiscChassis.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Chassis No. to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboMiscAccounts.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag3 = MainWindow.convertInteger(this.txtMiscExpAmount.Text) < 0;
					if (flag3)
					{
						Interaction.MsgBox("Negative Amount Not Allowed", MsgBoxStyle.Exclamation, null);
					}
					else
					{
						bool flag4 = Operators.CompareString(this.txtMiscExpAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtMiscExpDetail.Text, string.Empty, false) != 0;
						if (flag4)
						{
							base.DialogResult = new bool?(true);
							base.Close();
						}
					}
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000027A4 File Offset: 0x00000BA4
		// (set) Token: 0x060000BE RID: 190 RVA: 0x000027AE File Offset: 0x00000BAE
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000027B7 File Offset: 0x00000BB7
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x000027C1 File Offset: 0x00000BC1
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000027CA File Offset: 0x00000BCA
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000027D4 File Offset: 0x00000BD4
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000027DD File Offset: 0x00000BDD
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000027E7 File Offset: 0x00000BE7
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000027F0 File Offset: 0x00000BF0
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000027FA File Offset: 0x00000BFA
		internal virtual StackPanel UniReceiptDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00002803 File Offset: 0x00000C03
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000280D File Offset: 0x00000C0D
		internal virtual ComboBox comboMiscChassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00002816 File Offset: 0x00000C16
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00002820 File Offset: 0x00000C20
		internal virtual DatePicker MiscdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00002829 File Offset: 0x00000C29
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00002833 File Offset: 0x00000C33
		internal virtual TextBox txtMiscExpAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000283C File Offset: 0x00000C3C
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00002846 File Offset: 0x00000C46
		internal virtual TextBox txtMiscExpDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000284F File Offset: 0x00000C4F
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00002859 File Offset: 0x00000C59
		internal virtual ComboBox comboMiscAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
