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
	// Token: 0x0200000D RID: 13
	[DesignerGenerated]
	public partial class OfficeExpEntry : Window
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00002862 File Offset: 0x00000C62
		public OfficeExpEntry()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000466C File Offset: 0x00002A6C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[-+]?[.][0-9]+$|^[-+]?[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005C94 File Offset: 0x00004094
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboOfficeAccounts.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = this.convertInteger(this.txtOfficeExpAmount.Text) < 0;
				if (flag2)
				{
					Interaction.MsgBox("Negative Value Not Allowed", MsgBoxStyle.Exclamation, null);
				}
				else
				{
					bool flag3 = Operators.CompareString(this.txtOfficeExpAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtOfficeExpDetail.Text, string.Empty, false) != 0;
					if (flag3)
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000047C0 File Offset: 0x00002BC0
		private int convertInteger(object intInteger)
		{
			bool flag = intInteger == DBNull.Value;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = Conversions.ToInteger(intInteger);
			}
			return result;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00002870 File Offset: 0x00000C70
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000287A File Offset: 0x00000C7A
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00002883 File Offset: 0x00000C83
		// (set) Token: 0x060000DC RID: 220 RVA: 0x0000288D File Offset: 0x00000C8D
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00002896 File Offset: 0x00000C96
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000028A0 File Offset: 0x00000CA0
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000028A9 File Offset: 0x00000CA9
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000028B3 File Offset: 0x00000CB3
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000028BC File Offset: 0x00000CBC
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000028C6 File Offset: 0x00000CC6
		internal virtual StackPanel UniOfficeDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000028CF File Offset: 0x00000CCF
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000028D9 File Offset: 0x00000CD9
		internal virtual DatePicker OfficedatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000028E2 File Offset: 0x00000CE2
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000028EC File Offset: 0x00000CEC
		internal virtual TextBox txtOfficeExpAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000028F5 File Offset: 0x00000CF5
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000028FF File Offset: 0x00000CFF
		internal virtual TextBox txtOfficeExpDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00002908 File Offset: 0x00000D08
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00002912 File Offset: 0x00000D12
		internal virtual ComboBox comboOfficeAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
