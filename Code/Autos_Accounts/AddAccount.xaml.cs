using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
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

namespace Autos_Accounts
{
	// Token: 0x02000015 RID: 21
	[DesignerGenerated]
	public partial class AddAccount : Window
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00002F3E File Offset: 0x0000133E
		public AddAccount()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008624 File Offset: 0x00006A24
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.txtAccBalance.Text, string.Empty, false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Please Enter Account Balance to Continue", MsgBoxStyle.OkOnly, null);
				this.txtAccBalance.Focus();
			}
			else
			{
				bool flag2 = Operators.CompareString(this.txtAccName.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Enter Account Name to Continue", MsgBoxStyle.OkOnly, null);
					this.txtAccName.Focus();
				}
				else
				{
					bool flag3 = this.stpBank.Visibility == Visibility.Visible;
					if (flag3)
					{
						bool flag4 = Operators.CompareString(this.txtAccNum.Text, string.Empty, false) == 0;
						if (flag4)
						{
							Interaction.MsgBox("Please Enter Account Number to Continue", MsgBoxStyle.OkOnly, null);
							this.txtAccNum.Focus();
						}
						else
						{
							bool flag5 = Operators.CompareString(this.txtAccTitle.Text, string.Empty, false) == 0;
							if (flag5)
							{
								Interaction.MsgBox("Please Enter Account Title to Continue", MsgBoxStyle.OkOnly, null);
								this.txtAccTitle.Focus();
							}
							else
							{
								bool flag6 = Operators.CompareString(this.BankName.Text, string.Empty, false) == 0;
								if (flag6)
								{
									Interaction.MsgBox("Please Enter Bank Name to Continue", MsgBoxStyle.OkOnly, null);
									this.BankName.Focus();
								}
								else
								{
									bool flag7 = MainWindow.DSet.Tables["AccountTable"].Select("AccountName = '" + this.txtAccName.Text + "'").Count<DataRow>() > 0 && Operators.CompareString(base.Title, "Edit Account", false) != 0;
									if (flag7)
									{
										Interaction.MsgBox("Account Name already exists", MsgBoxStyle.OkOnly, null);
										this.txtAccName.Focus();
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
					else
					{
						bool flag8 = MainWindow.DSet.Tables["AccountTable"].Select("AccountName = '" + this.txtAccName.Text + "'").Count<DataRow>() > 0;
						if (flag8)
						{
							Interaction.MsgBox("Account Name already exists", MsgBoxStyle.OkOnly, null);
							this.txtAccName.Focus();
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

		// Token: 0x060001BC RID: 444 RVA: 0x0000546C File Offset: 0x0000386C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000887C File Offset: 0x00006C7C
		private void comboAcctype_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.stpBank != null;
			if (flag)
			{
				bool flag2 = Operators.ConditionalCompareObjectEqual(this.comboAcctype.SelectedValue, "Cash", false);
				if (flag2)
				{
					this.stpBank.Visibility = Visibility.Hidden;
					base.Height = 225.0;
				}
				else
				{
					this.stpBank.Visibility = Visibility.Visible;
					base.Height = 325.0;
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00002F4C File Offset: 0x0000134C
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00002F56 File Offset: 0x00001356
		internal virtual DockPanel dockAddAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00002F5F File Offset: 0x0000135F
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00002F69 File Offset: 0x00001369
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00002F72 File Offset: 0x00001372
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00002F7C File Offset: 0x0000137C
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00002F85 File Offset: 0x00001385
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00002F8F File Offset: 0x0000138F
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00002F98 File Offset: 0x00001398
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00002FA2 File Offset: 0x000013A2
		internal virtual ComboBox comboAcctype { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00002FAB File Offset: 0x000013AB
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00002FB5 File Offset: 0x000013B5
		internal virtual TextBox txtAccName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00002FBE File Offset: 0x000013BE
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00002FC8 File Offset: 0x000013C8
		internal virtual TextBox txtAccBalance { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00002FD1 File Offset: 0x000013D1
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00002FDB File Offset: 0x000013DB
		internal virtual StackPanel stpBank { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00002FE4 File Offset: 0x000013E4
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00002FEE File Offset: 0x000013EE
		internal virtual TextBox BankName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00002FF7 File Offset: 0x000013F7
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00003001 File Offset: 0x00001401
		internal virtual TextBox BankBranch { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000300A File Offset: 0x0000140A
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00003014 File Offset: 0x00001414
		internal virtual TextBox txtAccNum { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000301D File Offset: 0x0000141D
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00003027 File Offset: 0x00001427
		internal virtual TextBox txtAccTitle { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
