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
using Xceed.Wpf.Toolkit;

namespace Autos_Accounts
{
	// Token: 0x02000012 RID: 18
	[DesignerGenerated]
	public partial class SaleAuto : Window
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00002D85 File Offset: 0x00001185
		public SaleAuto()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000466C File Offset: 0x00002A6C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[-+]?[.][0-9]+$|^[-+]?[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000077F0 File Offset: 0x00005BF0
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboChassis.Text, string.Empty, false) == 0 && Operators.CompareString(this.AccInfoText.Text, "Edit Sales Entry", false) != 0;
			if (flag)
			{
				Interaction.MsgBox("Please Select Chassis No. to continue...", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboSaleAccounts.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag3 = Operators.CompareString(this.comboCust.Text, string.Empty, false) == 0;
					if (flag3)
					{
						Interaction.MsgBox("Please Select Customer to continue...", MsgBoxStyle.OkOnly, null);
					}
					else
					{
						bool flag4 = Operators.CompareString(this.txtSalePrice.Text, string.Empty, false) != 0;
						if (flag4)
						{
							base.DialogResult = new bool?(true);
							base.Close();
						}
					}
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00002D93 File Offset: 0x00001193
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00002D9D File Offset: 0x0000119D
		internal virtual DockPanel dockSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00002DA6 File Offset: 0x000011A6
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00002DB0 File Offset: 0x000011B0
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00002DB9 File Offset: 0x000011B9
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00002DC3 File Offset: 0x000011C3
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00002DCC File Offset: 0x000011CC
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00002DD6 File Offset: 0x000011D6
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00002DDF File Offset: 0x000011DF
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00002DE9 File Offset: 0x000011E9
		internal virtual StackPanel UniDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00002DF2 File Offset: 0x000011F2
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00002DFC File Offset: 0x000011FC
		internal virtual Label lblchassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00002E05 File Offset: 0x00001205
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00002E0F File Offset: 0x0000120F
		internal virtual ComboBox comboChassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00002E18 File Offset: 0x00001218
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00002E22 File Offset: 0x00001222
		internal virtual ComboBox comboCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00002E2B File Offset: 0x0000122B
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00002E35 File Offset: 0x00001235
		internal virtual TextBox txtSalePrice { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00002E3E File Offset: 0x0000123E
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00002E48 File Offset: 0x00001248
		internal virtual IntegerUpDown txtAmountReceived { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00002E51 File Offset: 0x00001251
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00002E5B File Offset: 0x0000125B
		internal virtual ComboBox comboSaleAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00002E64 File Offset: 0x00001264
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00002E6E File Offset: 0x0000126E
		internal virtual DatePicker SaleDatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
