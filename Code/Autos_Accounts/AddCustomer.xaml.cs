using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Autos_Accounts.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Xceed.Wpf.Toolkit;

namespace Autos_Accounts
{
	// Token: 0x0200000A RID: 10
	[DesignerGenerated]
	public partial class AddCustomer : Window
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000025C8 File Offset: 0x000009C8
		public AddCustomer()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004EE4 File Offset: 0x000032E4
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.txtCnicNo.Text.Contains("_") && Operators.CompareString(this.txtCnicNo.Text, "_____-_______-_", false) != 0;
			if (flag)
			{
				Interaction.MsgBox("Please Enter Complete CNIC OR REMOVE CNIC NO. to Continue", MsgBoxStyle.OkOnly, null);
				this.txtCnicNo.Focus();
			}
			else
			{
				bool flag2 = Operators.CompareString(this.txtCustName.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Enter Name to Continue", MsgBoxStyle.OkOnly, null);
					this.txtCustName.Focus();
				}
				else
				{
					bool flag3 = MainWindow.DSet.Tables["CustomersTable"].Select("Name = '" + this.txtCustName.Text + "'").Count<DataRow>() > 0;
					if (flag3)
					{
						Interaction.MsgBox("Customer Name already exists", MsgBoxStyle.OkOnly, null);
						this.txtCustName.Focus();
					}
					else
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004AC0 File Offset: 0x00002EC0
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004FF4 File Offset: 0x000033F4
		private void txtCnicNo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			int selectionStart = (sender as TextBox).SelectionStart;
			string text = (sender as TextBox).Text.Substring(0, selectionStart).Replace("_", "");
			try
			{
				(sender as TextBox).Select(text.Length, 0);
			}
			catch (Exception ex)
			{
				this.errorlog(ex.Message);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000251A File Offset: 0x0000091A
		private void txtCnicNo_PreviewKeyDown(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004B8C File Offset: 0x00002F8C
		public void errorlog(string StrError)
		{
			StreamWriter streamWriter = new StreamWriter(MyWpfExtension.Application.Info.DirectoryPath + "\\Log.txt", true);
			streamWriter.WriteLine("TimeStamp: " + DateTime.Now.ToString() + "\tMessage: " + StrError);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000251A File Offset: 0x0000091A
		private void txtCnicNo_GotFocus(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005074 File Offset: 0x00003474
		private void txtCnicNo_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			int selectionStart = this.txtCnicNo.SelectionStart;
			this.txtCnicNo.IncludeLiteralsInValue = false;
			string text = this.txtCnicNo.Text.Substring(0, selectionStart);
			try
			{
				bool flag = Versioned.IsNumeric(e.Text);
				if (flag)
				{
					bool flag2 = (sender as TextBox).SelectedText.Length > 0;
					if (flag2)
					{
						MyWpfExtension.Computer.Keyboard.SendKeys("Delete");
					}
					bool flag3 = text.Contains("_");
					if (flag3)
					{
						(sender as TextBox).Select(text.Split(new char[]
						{
							'_'
						})[0].Length, 0);
					}
					else
					{
						(sender as TextBox).Select(text.Length, 0);
					}
				}
				this.txtCnicNo.IncludeLiteralsInValue = true;
			}
			catch (Exception ex)
			{
				this.errorlog(ex.Message);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000025D6 File Offset: 0x000009D6
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000025E0 File Offset: 0x000009E0
		internal virtual DockPanel dockAddCustomer { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000025E9 File Offset: 0x000009E9
		// (set) Token: 0x06000084 RID: 132 RVA: 0x000025F3 File Offset: 0x000009F3
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000025FC File Offset: 0x000009FC
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00002606 File Offset: 0x00000A06
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000260F File Offset: 0x00000A0F
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00002619 File Offset: 0x00000A19
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002622 File Offset: 0x00000A22
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000262C File Offset: 0x00000A2C
		internal virtual DatePicker CustdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002635 File Offset: 0x00000A35
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000263F File Offset: 0x00000A3F
		internal virtual TextBox txtCustTitle { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002648 File Offset: 0x00000A48
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00002652 File Offset: 0x00000A52
		internal virtual TextBox txtCustName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000265B File Offset: 0x00000A5B
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00002665 File Offset: 0x00000A65
		internal virtual MaskedTextBox txtCnicNo { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000266E File Offset: 0x00000A6E
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002678 File Offset: 0x00000A78
		internal virtual TextBox txtCustPhoneNo { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002681 File Offset: 0x00000A81
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000268B File Offset: 0x00000A8B
		internal virtual TextBox txtCustAddress { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
