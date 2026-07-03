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
	// Token: 0x02000008 RID: 8
	[DesignerGenerated]
	public partial class AccounttoAccount : Window
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002429 File Offset: 0x00000829
		public AccounttoAccount()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000466C File Offset: 0x00002A6C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[-+]?[.][0-9]+$|^[-+]?[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000046F4 File Offset: 0x00002AF4
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboCAccounts.Text, string.Empty, false) == 0 || Operators.CompareString(this.comboDAccounts.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = this.convertInteger(this.txtOfficeAccountAmount.Text) < 0;
				if (flag2)
				{
					Interaction.MsgBox("Negative Value Not Allowed", MsgBoxStyle.Exclamation, null);
				}
				else
				{
					bool flag3 = Operators.CompareString(this.txtOfficeAccountAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtOfficeAccountDetail.Text, string.Empty, false) != 0;
					if (flag3)
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000047C0 File Offset: 0x00002BC0
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000244E File Offset: 0x0000084E
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002458 File Offset: 0x00000858
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002461 File Offset: 0x00000861
		// (set) Token: 0x06000046 RID: 70 RVA: 0x0000246B File Offset: 0x0000086B
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002474 File Offset: 0x00000874
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000247E File Offset: 0x0000087E
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002487 File Offset: 0x00000887
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002491 File Offset: 0x00000891
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000249A File Offset: 0x0000089A
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000024A4 File Offset: 0x000008A4
		internal virtual StackPanel UniOfficeAccountDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000024AD File Offset: 0x000008AD
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000024B7 File Offset: 0x000008B7
		internal virtual DatePicker OfficeAccountdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000024C0 File Offset: 0x000008C0
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000024CA File Offset: 0x000008CA
		internal virtual TextBox txtOfficeAccountAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000024D3 File Offset: 0x000008D3
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000024DD File Offset: 0x000008DD
		internal virtual TextBox txtOfficeAccountDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000024E6 File Offset: 0x000008E6
		// (set) Token: 0x06000054 RID: 84 RVA: 0x000024F0 File Offset: 0x000008F0
		internal virtual ComboBox comboCAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000024F9 File Offset: 0x000008F9
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002503 File Offset: 0x00000903
		internal virtual ComboBox comboDAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
