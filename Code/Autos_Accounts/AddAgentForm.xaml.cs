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
	// Token: 0x02000009 RID: 9
	[DesignerGenerated]
	public partial class AddAgentForm : Window
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000250C File Offset: 0x0000090C
		public AddAgentForm()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002437 File Offset: 0x00000837
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			base.Close();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000049B0 File Offset: 0x00002DB0
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
				bool flag2 = Operators.CompareString(this.txtAgentName.Text, string.Empty, false) == 0;
				if (flag2)
				{
					Interaction.MsgBox("Please Enter Name to Continue", MsgBoxStyle.OkOnly, null);
					this.txtAgentName.Focus();
				}
				else
				{
					bool flag3 = MainWindow.DSet.Tables["AgentsTable"].Select("Name = '" + this.txtAgentName.Text + "'").Count<DataRow>() > 0;
					if (flag3)
					{
						Interaction.MsgBox("Agent Name already exists", MsgBoxStyle.OkOnly, null);
						this.txtAgentName.Focus();
					}
					else
					{
						base.DialogResult = new bool?(true);
						base.Close();
					}
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004AC0 File Offset: 0x00002EC0
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004B0C File Offset: 0x00002F0C
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

		// Token: 0x0600005F RID: 95 RVA: 0x0000251A File Offset: 0x0000091A
		private void txtCnicNo_PreviewKeyDown(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004B8C File Offset: 0x00002F8C
		public void errorlog(string StrError)
		{
			StreamWriter streamWriter = new StreamWriter(MyWpfExtension.Application.Info.DirectoryPath + "\\Log.txt", true);
			streamWriter.WriteLine("TimeStamp: " + DateTime.Now.ToString() + "\tMessage: " + StrError);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000251A File Offset: 0x0000091A
		private void txtCnicNo_GotFocus(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004BEC File Offset: 0x00002FEC
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000251D File Offset: 0x0000091D
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002527 File Offset: 0x00000927
		internal virtual DockPanel dockAddAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002530 File Offset: 0x00000930
		// (set) Token: 0x06000066 RID: 102 RVA: 0x0000253A File Offset: 0x0000093A
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002543 File Offset: 0x00000943
		// (set) Token: 0x06000068 RID: 104 RVA: 0x0000254D File Offset: 0x0000094D
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002556 File Offset: 0x00000956
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002560 File Offset: 0x00000960
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002569 File Offset: 0x00000969
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002573 File Offset: 0x00000973
		internal virtual DatePicker AgentdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000257C File Offset: 0x0000097C
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002586 File Offset: 0x00000986
		internal virtual TextBox txtAgentName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000258F File Offset: 0x0000098F
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002599 File Offset: 0x00000999
		internal virtual MaskedTextBox txtCnicNo { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000025A2 File Offset: 0x000009A2
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000025AC File Offset: 0x000009AC
		internal virtual TextBox txtAgentPhoneNo { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000025B5 File Offset: 0x000009B5
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000025BF File Offset: 0x000009BF
		internal virtual TextBox txtAgentAddress { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
