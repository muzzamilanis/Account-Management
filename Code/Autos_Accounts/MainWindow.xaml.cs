using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps.Packaging;
using Autos_Accounts.My;
using CodeReason.Reports;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace Autos_Accounts
{
	// Token: 0x02000018 RID: 24
	[DesignerGenerated]
	public partial class MainWindow : Window
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00008C78 File Offset: 0x00007078
		public MainWindow()
		{
			base.Loaded += this.MainWindow_Loaded;
			base.Closing += this.MainWindow_Closing;
			base.ManipulationInertiaStarting += this.MainWindow_ManipulationInertiaStarting;
			this.InitializeComponent();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008CC8 File Offset: 0x000070C8
		private void BtnCreateData_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
			Microsoft.Win32.SaveFileDialog saveFileDialog2 = saveFileDialog;
			saveFileDialog2.Filter = "BusinessNetworkDatabase files|; *.bndb";
			saveFileDialog2.Title = "Select a BNDB file";
			saveFileDialog2.FileName = "";
			try
			{
				Microsoft.Win32.SaveFileDialog saveFileDialog3 = saveFileDialog;
				bool? flag = saveFileDialog3.ShowDialog();
				bool valueOrDefault = ((flag != null) ? new bool?(flag.GetValueOrDefault()) : null).GetValueOrDefault();
				if (valueOrDefault)
				{
					MainWindow.Connection.Close();
					this.FileName = saveFileDialog3.FileName;
					MainWindow.StrDataFile = this.FileName;
					MySettingsProperty.Settings.StrDataFile = MainWindow.StrDataFile;
					MySettingsProperty.Settings.Save();
					SQLiteConnection.CreateFile(MainWindow.StrDataFile);
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						MainWindow.Connection.ConnectionString = "Data Source=" + MainWindow.StrDataFile + ";Version=3;New=False;Compress=True;";
						MainWindow.Connection.SetPassword("karachi123");
						MainWindow.Connection.Open();
						SQLiteCommand sqliteCommand2 = sqliteCommand;
						sqliteCommand2.Connection = MainWindow.Connection;
						sqliteCommand2.CommandText = "CREATE TABLE IF NOT EXISTS AccountTable(AccountDate DATE, AccountType VARCHAR, AccountName VARCHAR, AccountNumber VARCHAR, AccountTitle VARCHAR, BankName VARCHAR, BankBranch VARCHAR, OpeningBalance REAL DEFAULT 0, CurrentBalance REAL DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS MiscExpTable(chassis VARCHAR, MiscExpDate DATE, MiscExpAmount REAL DEFAULT 0, MiscExpDetail VARCHAR, MiscExpPaidBy VARCHAR);\r\nCREATE TABLE IF NOT EXISTS OfficeExpTable(OfficeExpDate DATE, OfficeExpAmount REAL DEFAULT 0, OfficeExpDetail VARCHAR, OfficeExpPaidBy VARCHAR);\r\nCREATE TABLE IF NOT EXISTS OfficeAccountTable(Date DATE, Amount REAL DEFAULT 0, Detail VARCHAR, CreditFrom VARCHAR, DebitTo VARCHAR, LedgerRowId INT);\r\nCREATE TABLE IF NOT EXISTS LedgerTable(Date DATE, Amount REAL DEFAULT 0, Detail VARCHAR, Account VARCHAR);\r\nCREATE TABLE IF NOT EXISTS ReceiptsTable(receiptDate DATE, receiptAmount REAL DEFAULT 0, receiptDetail VARCHAR, receivedIn VARCHAR, receivedFrom VARCHAR);\r\nCREATE TABLE IF NOT EXISTS PaymentsTable(PaymentDate DATE, PaymentAmountYen REAL DEFAULT 0, PaymentExcRate REAL DEFAULT 0, PaymentAmountPkr REAL DEFAULT 0, PaymentDetail VARCHAR, PaidFrom VARCHAR);\r\nCREATE TABLE IF NOT EXISTS PaymentsPkrTable(PaymentDate DATE, PaymentAmount REAL DEFAULT 0, PaymentDetail VARCHAR, PaidFrom VARCHAR, PaidTo VARCHAR);\r\nCREATE TABLE IF NOT EXISTS PaymentsAgentTable(PaymentDate DATE, PaymentAmount REAL DEFAULT 0, PaymentDetail VARCHAR, PaidFrom VARCHAR, PaidTo VARCHAR);\r\nCREATE TABLE IF NOT EXISTS StocksTable(Date DATE, Chassis VARCHAR, Model VARCHAR, Color VARCHAR, PriceYen REAL DEFAULT 0, Rate REAL DEFAULT 0, PricePkr REAL DEFAULT 0, Duty REAL DEFAULT 0, MiscExpense REAL DEFAULT 0, Cost REAL DEFAULT 0, Status VARCHAR, PaidYen REAL DEFAULT 0, PaidAmount REAL DEFAULT 0, Comments VARCHAR);\r\nCREATE TABLE IF NOT EXISTS CustomersTable(Date DATE, Title VARCHAR, Name VARCHAR, CNIC VARCHAR, Phone VARCHAR, Address VARCHAR, PaymentReceived REAL DEFAULT 0, PaymentReceivable REAL DEFAULT 0, PaymentPaid REAL DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS SalesTable(SaleDate DATE, SaleChassis VARCHAR, SaleCustomer VARCHAR, SalePrice REAL DEFAULT 0, SaleAmountReceived REAL DEFAULT 0, PaymentReceivedIn VARCHAR);\r\nCREATE TABLE IF NOT EXISTS DutyExpTable(chassis VARCHAR, DutyExpDate DATE, DutyExpAmount REAL DEFAULT 0, DutyExpDetail VARCHAR, DutyExpPaidBy VARCHAR, DutyExpAgent VARCHAR);\r\nCREATE TABLE If NOT EXISTS ProfitTable(Amount REAL DEFAULT 0);\r\nCREATE TABLE If NOT EXISTS InfoTable(MiscExpLast INT DEFAULT 0, DutyExpLast INT DEFAULT 0, OfficeExpLast INT DEFAULT 0, SaleLast INT DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS AgentsTable(Date DATE, Name VARCHAR, CNIC VARCHAR, Phone VARCHAR, Address VARCHAR, PaymentReceivable REAL DEFAULT 0, PaymentPaid REAL DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS PayableYenTable(Amount INT DEFAULT 0, Rate REAL DEFAULT 0, Status VARCHAR);";
						sqliteCommand.ExecuteNonQuery();
					}
					this.openData();
				}
			}
			catch (SQLiteException ex)
			{
				Interaction.MsgBox(ex.Message + "\r\n" + ex.ToString(), MsgBoxStyle.OkOnly, null);
				MainWindow.errorlog("During Database Creation: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008EA0 File Offset: 0x000072A0
		private void LstCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.LstCust.SelectedIndex != -1;
			if (flag)
			{
				this.BtnDeleteCust.IsEnabled = true;
				this.BtnEditCust.IsEnabled = true;
				bool flag2 = Operators.ConditionalCompareObjectGreater(NewLateBinding.LateIndexGet(this.LstCust.SelectedItem, new object[]
				{
					"PaymentReceivable"
				}, null), 0, false);
				if (flag2)
				{
					this.runCustRecPay.Text = "Payment Receivable: ";
				}
				else
				{
					this.runCustRecPay.Text = "Payment Payable: ";
				}
			}
			else
			{
				this.BtnDeleteCust.IsEnabled = false;
				this.BtnEditCust.IsEnabled = false;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008F54 File Offset: 0x00007354
		private void LstStocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.LstStocks.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditStock.IsEnabled = true;
				this.BtnDeleteStocks.IsEnabled = true;
			}
			else
			{
				this.BtnEditStock.IsEnabled = false;
				this.BtnDeleteStocks.IsEnabled = false;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008FB0 File Offset: 0x000073B0
		private void LstAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.LstAccounts.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditAccount.IsEnabled = true;
				this.BtnDeleteAccount.IsEnabled = true;
			}
			else
			{
				this.BtnEditAccount.IsEnabled = false;
				this.BtnDeleteAccount.IsEnabled = false;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000466C File Offset: 0x00002A6C
		public void txtSalePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[-+]?[.][0-9]+$|^[-+]?[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as System.Windows.Controls.TextBox).Text.Insert((sender as System.Windows.Controls.TextBox).SelectionStart, e.Text));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000046B8 File Offset: 0x00002AB8
		public void txtSalePrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool flag = Operators.CompareString(e.Command.ToString(), ApplicationCommands.Paste.ToString(), false) == 0;
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000900C File Offset: 0x0000740C
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

		// Token: 0x060001E7 RID: 487 RVA: 0x00003053 File Offset: 0x00001453
		private void hyplnk_Click(object sender, RoutedEventArgs e)
		{
			this.WrapExcRate.Visibility = Visibility.Hidden;
			this.WrapExcEdit.Visibility = Visibility.Visible;
			this.txtEditExcRate.Focus();
			this.txtEditExcRate.SelectAll();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00003088 File Offset: 0x00001488
		private void btnExcEdit_Click(object sender, RoutedEventArgs e)
		{
			this.hyplnk.Focus();
			MySettingsProperty.Settings.Save();
			this.WrapExcEdit.Visibility = Visibility.Hidden;
			this.WrapExcRate.Visibility = Visibility.Visible;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000090C0 File Offset: 0x000074C0
		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(this.SelectAllText));
			try
			{
				UserLogin userLogin = new UserLogin();
				userLogin.ShowDialog();
				bool flag = userLogin.DialogResult != null && userLogin.DialogResult.Value;
				if (!flag)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Account: " + ex.Message + "\r\n" + ex.ToString());
			}
			this.loadedbool = true;
			bool flag2 = Operators.CompareString(MySettingsProperty.Settings.StrDataFile, string.Empty, false) != 0;
			if (flag2)
			{
				this.openData();
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000919C File Offset: 0x0000759C
		private void SelectAllText(object sender, RoutedEventArgs e)
		{
			System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
			bool flag = textBox != null;
			if (flag)
			{
				bool flag2 = !textBox.IsReadOnly;
				if (flag2)
				{
					textBox.SelectAll();
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000091D4 File Offset: 0x000075D4
		private void openData()
		{
			try
			{
				this.boolCopied = false;
				this.payableYens = 0;
				bool flag = Operators.CompareString(MainWindow.StrDataFile, string.Empty, false) != 0;
				if (flag)
				{
					MainWindow.Connection.Close();
					MainWindow.Connection.ConnectionString = "Data Source=" + MainWindow.StrDataFile + ";Version=3;New=False;Compress=True;Password=karachi123;";
					MainWindow.Connection.Open();
					DataTable dataTable = new DataTable();
					dataTable = MainWindow.Connection.GetSchema("Columns");
					DataRow[] array = dataTable.Select("TABLE_NAME LIKE '*CustomersTable*'");
					string text = string.Empty;
					foreach (DataRow dataRow in array)
					{
						text = Conversions.ToString(Operators.AddObject(text, Operators.AddObject(dataRow["COLUMN_NAME"], "\r\n")));
					}
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						SQLiteCommand sqliteCommand2 = sqliteCommand;
						sqliteCommand2.Connection = MainWindow.Connection;
						sqliteCommand2.CommandText = "CREATE TABLE IF NOT EXISTS AccountTable(AccountDate DATE, AccountType VARCHAR, AccountName VARCHAR, AccountNumber VARCHAR, AccountTitle VARCHAR, BankName VARCHAR, BankBranch VARCHAR, OpeningBalance REAL DEFAULT 0, CurrentBalance REAL DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS MiscExpTable(chassis VARCHAR, MiscExpDate DATE, MiscExpAmount REAL DEFAULT 0, MiscExpDetail VARCHAR, MiscExpPaidBy VARCHAR);\r\nCREATE TABLE IF NOT EXISTS OfficeExpTable(OfficeExpDate DATE, OfficeExpAmount REAL DEFAULT 0, OfficeExpDetail VARCHAR, OfficeExpPaidBy VARCHAR);\r\nCREATE TABLE IF NOT EXISTS OfficeAccountTable(Date DATE, Amount REAL DEFAULT 0, Detail VARCHAR, CreditFrom VARCHAR, DebitTo VARCHAR, LedgerRowId INT);\r\nCREATE TABLE IF NOT EXISTS LedgerTable(Date DATE, Amount REAL DEFAULT 0, Detail VARCHAR, Account VARCHAR);\r\nCREATE TABLE IF NOT EXISTS ReceiptsTable(receiptDate DATE, receiptAmount REAL DEFAULT 0, receiptDetail VARCHAR, receivedIn VARCHAR, receivedFrom VARCHAR);\r\nCREATE TABLE IF NOT EXISTS PaymentsTable(PaymentDate DATE, PaymentAmountYen REAL DEFAULT 0, PaymentExcRate REAL DEFAULT 0, PaymentAmountPkr REAL DEFAULT 0, PaymentDetail VARCHAR, PaidFrom VARCHAR);\r\nCREATE TABLE IF NOT EXISTS PaymentsPkrTable(PaymentDate DATE, PaymentAmount REAL DEFAULT 0, PaymentDetail VARCHAR, PaidFrom VARCHAR, PaidTo VARCHAR);\r\nCREATE TABLE IF NOT EXISTS PaymentsAgentTable(PaymentDate DATE, PaymentAmount REAL DEFAULT 0, PaymentDetail VARCHAR, PaidFrom VARCHAR, PaidTo VARCHAR);\r\nCREATE TABLE IF NOT EXISTS StocksTable(Date DATE, Chassis VARCHAR, Model VARCHAR, Color VARCHAR, PriceYen REAL DEFAULT 0, Rate REAL DEFAULT 0, PricePkr REAL DEFAULT 0, Duty REAL DEFAULT 0, MiscExpense REAL DEFAULT 0, Cost REAL DEFAULT 0, Status VARCHAR, PaidYen REAL DEFAULT 0, PaidAmount REAL DEFAULT 0, Comments VARCHAR);\r\nCREATE TABLE IF NOT EXISTS CustomersTable(Date DATE, Title VARCHAR, Name VARCHAR, CNIC VARCHAR, Phone VARCHAR, Address VARCHAR, PaymentReceived REAL DEFAULT 0, PaymentReceivable REAL DEFAULT 0, PaymentPaid REAL DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS SalesTable(SaleDate DATE, SaleChassis VARCHAR, SaleCustomer VARCHAR, SalePrice REAL DEFAULT 0, SaleAmountReceived REAL DEFAULT 0, PaymentReceivedIn VARCHAR);\r\nCREATE TABLE IF NOT EXISTS DutyExpTable(chassis VARCHAR, DutyExpDate DATE, DutyExpAmount REAL DEFAULT 0, DutyExpDetail VARCHAR, DutyExpPaidBy VARCHAR, DutyExpAgent VARCHAR);\r\nCREATE TABLE If NOT EXISTS ProfitTable(Amount REAL DEFAULT 0);\r\nCREATE TABLE If NOT EXISTS InfoTable(MiscExpLast INT DEFAULT 0, DutyExpLast INT DEFAULT 0, OfficeExpLast INT DEFAULT 0, SaleLast INT DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS AgentsTable(Date DATE, Name VARCHAR, CNIC VARCHAR, Phone VARCHAR, Address VARCHAR, PaymentReceivable REAL DEFAULT 0, PaymentPaid REAL DEFAULT 0);\r\nCREATE TABLE IF NOT EXISTS PayableYenTable(Amount INT DEFAULT 0, Rate REAL DEFAULT 0, Status VARCHAR);";
						sqliteCommand.ExecuteNonQuery();
						bool flag2 = !text.Contains("PaymentPaid");
						if (flag2)
						{
							sqliteCommand.CommandText = "ALTER TABLE CustomersTable ADD COLUMN PaymentPaid REAL Default 0";
							sqliteCommand.ExecuteNonQuery();
						}
					}
					this.controlupdater();
					this.btnenabler(true);
					this.MainGrid.SelectedIndex = 1;
				}
			}
			catch (SQLiteException ex)
			{
				Interaction.MsgBox(ex.Message + "\r\n" + ex.ToString(), MsgBoxStyle.OkOnly, null);
				MainWindow.errorlog("During Opening Database: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000093C4 File Offset: 0x000077C4
		private void controlupdater()
		{
			checked
			{
				try
				{
					this.btnClosing.Visibility = Visibility.Collapsed;
					string commandText = "SELECT rowid, date(AccountDate) as AccountDate, AccountType, AccountName, AccountNumber, AccountTitle, BankName, BankBranch, OpeningBalance, CurrentBalance from  AccountTable;";
					string commandText2 = "SELECT rowid, chassis, date(MiscExpDate) as DATE, MiscExpAmount, MiscExpDetail, MiscExpPaidBy from MiscExpTable;";
					string commandText3 = "SELECT rowid, chassis, date(DutyExpDate) as DATE, DutyExpAmount, DutyExpDetail, DutyExpPaidBy, DutyExpAgent from DutyExpTable;";
					string commandText4 = "SELECT rowid, date(OfficeExpDate) as Date, OfficeExpAmount, OfficeExpDetail, OfficeExpPaidBy from OfficeExpTable;";
					string commandText5 = "SELECT rowid, date(Date) as Date, Amount, Detail, CreditFrom, DebitTo, LedgerRowId from OfficeAccountTable;";
					string commandText6 = "SELECT rowid, date(Date) as DATE, Name, CNIC, Phone, Address, PaymentReceivable, PaymentPaid from AgentsTable;";
					string commandText7 = "SELECT rowid, Amount, Rate, Status from PayableYenTable;";
					string commandText8 = "SELECT rowid, MiscExpLast, DutyExpLast, OfficeExpLast, SaleLast from InfoTable;";
					string commandText9 = "SELECT rowid, Amount from ProfitTable;";
					string commandText10 = "SELECT rowid, date(Date) as Date, Amount, Detail, Account from LedgerTable;";
					string commandText11 = "SELECT rowid, date(receiptDate) as DATE, receiptAmount, receiptDetail, receivedIn, receivedFrom from ReceiptsTable;";
					string commandText12 = "SELECT rowid, date(PaymentDate) as DATE, PaymentAmountYen, PaymentExcRate, PaymentAmountPkr, PaymentDetail, PaidFrom from PaymentsTable;";
					string commandText13 = "SELECT rowid, date(PaymentDate) as DATE, PaymentAmount, PaymentDetail, PaidFrom, PaidTo from PaymentsPkrTable;";
					string commandText14 = "SELECT rowid, date(PaymentDate) as DATE, PaymentAmount, PaymentDetail, PaidFrom, PaidTo from PaymentsAgentTable;";
					double value = (double)MySettingsProperty.Settings.exchangeRate;
					string commandText15 = "SELECT rowid, date(Date) as DATE, Chassis, Model, Color, PriceYen, Rate, PricePkr, Duty, MiscExpense, ROUND((PriceYen - PaidYen) * " + Conversions.ToString(value) + ", 2) + (PaidYen * Rate) + Duty + MiscExpense as Cost, Status, PaidYen, PaidAmount, Comments from StocksTable;";
					string commandText16 = "SELECT rowid, date(Date) as DATE, Title, Name, CNIC, Phone, Address, PaymentReceived, PaymentReceivable, PaymentPaid from CustomersTable;";
					string commandText17 = "SELECT rowid, date(SaleDate) as SaleDate, SaleChassis, SaleCustomer, SalePrice, SaleAmountReceived, PaymentReceivedIn from SalesTable;";
					SQLiteCommand cmd = new SQLiteCommand(commandText, MainWindow.Connection);
					MainWindow.DataAdapter = new SQLiteDataAdapter(cmd);
					MainWindow.CmdBuilder = new SQLiteCommandBuilder(MainWindow.DataAdapter);
					bool flag = MainWindow.DataAdapter != null;
					if (flag)
					{
						this.boolCopied = false;
						this.documentViewer2.Visibility = Visibility.Collapsed;
						this.flowviewertrial.Visibility = Visibility.Collapsed;
						MainWindow.DSet = new DataSet();
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "AccountTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText2;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "MiscExpTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText3;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "DutyExpTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText6;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "AgentsTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText7;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "PayableYenTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText9;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "ProfitTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText8;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "InfoTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText4;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "OfficeExpTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText5;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "OfficeAccountTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText10;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "LedgerTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText11;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "ReceiptsTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText12;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "PaymentsTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText13;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "PaymentsPkrTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText14;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "PaymentsAgentTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText15;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "StocksTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText16;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "CustomersTable");
						MainWindow.DataAdapter.SelectCommand.CommandText = commandText17;
						MainWindow.DataAdapter.Fill(MainWindow.DSet, "SalesTable");
					}
					bool flag2 = MainWindow.DSet.Tables["InfoTable"].Rows.Count > 0;
					if (flag2)
					{
						MySettings settings = MySettingsProperty.Settings;
						settings.strclosingDutyExp = Conversions.ToString(MainWindow.DSet.Tables["InfoTable"].Rows[MainWindow.DSet.Tables["InfoTable"].Rows.Count - 1]["DutyExpLast"]);
						settings.strclosingmiscexp = Conversions.ToString(MainWindow.DSet.Tables["InfoTable"].Rows[MainWindow.DSet.Tables["InfoTable"].Rows.Count - 1]["MiscExpLast"]);
						settings.strclosingOfficeRow = Conversions.ToString(MainWindow.DSet.Tables["InfoTable"].Rows[MainWindow.DSet.Tables["InfoTable"].Rows.Count - 1]["OfficeExpLast"]);
						settings.strclosingSalesRow = Conversions.ToString(MainWindow.DSet.Tables["InfoTable"].Rows[MainWindow.DSet.Tables["InfoTable"].Rows.Count - 1]["SaleLast"]);
						settings.Save();
					}
					else
					{
						MySettings settings2 = MySettingsProperty.Settings;
						settings2.strclosingDutyExp = Conversions.ToString(0);
						settings2.strclosingmiscexp = Conversions.ToString(0);
						settings2.strclosingOfficeRow = Conversions.ToString(0);
						settings2.strclosingSalesRow = Conversions.ToString(0);
						settings2.Save();
					}
					this.LstAccounts.ItemsSource = MainWindow.DSet.Tables["AccountTable"].DefaultView;
					this.LstCust.ItemsSource = MainWindow.DSet.Tables["CustomersTable"].DefaultView;
					this.LstStocks.ItemsSource = MainWindow.DSet.Tables["StocksTable"].DefaultView;
					this.LstAgent.ItemsSource = MainWindow.DSet.Tables["AgentsTable"].DefaultView;
					bool flag3 = MainWindow.DSet.Tables["StocksTable"].Rows.Count > 0 | MainWindow.DSet.Tables["PaymentsTable"].Rows.Count > 0;
					if (flag3)
					{
						int num = 0;
						bool flag4 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountYen)", "")));
						if (flag4)
						{
							num = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountYen)", "")));
						}
						this.payableYens = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PriceYen)", ""))) - num;
						this.TotalAccountsPayable.Text = Conversions.ToString(this.payableYens);
						bool flag5 = Conversions.ToDouble(this.TotalAccountsPayable.Text) < (double)MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PriceYen) - Sum(PaidYen)", "PriceYen <> PaidYen")));
						if (flag5)
						{
							bool flag6 = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PriceYen) - Sum(PaidYen)", "PriceYen <> PaidYen"))) > 0;
							if (flag6)
							{
								this.AdjustLink.Visibility = Visibility.Visible;
							}
							else
							{
								this.AdjustLink.Visibility = Visibility.Collapsed;
							}
						}
						else
						{
							this.AdjustLink.Visibility = Visibility.Collapsed;
						}
					}
					DataTable dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["StocksTable"].Clone();
					DataRow[] array = MainWindow.DSet.Tables["StocksTable"].Select("Status IS NULL");
					foreach (DataRow row in array)
					{
						dataTable.ImportRow(row);
					}
					this.comboChassis.ItemsSource = ((IListSource)dataTable).GetList();
					this.comboChassis.DisplayMemberPath = MainWindow.DSet.Tables["StocksTable"].Columns["Chassis"].ToString();
					this.comboChassis.SelectedValuePath = "Chassis";
					this.comboChassis.SelectedIndex = 0;
					this.comboCust.ItemsSource = ((IListSource)MainWindow.DSet.Tables["CustomersTable"]).GetList();
					this.comboCust.DisplayMemberPath = MainWindow.DSet.Tables["CustomersTable"].Columns["Name"].ToString();
					this.comboCust.SelectedValuePath = "Name";
					this.comboCust.SelectedIndex = 0;
					this.comboSaleAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboSaleAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboSaleAccounts.SelectedValuePath = "AccountName";
					this.comboSaleAccounts.SelectedIndex = 0;
					this.comboMiscChassis.ItemsSource = ((IListSource)MainWindow.DSet.Tables["StocksTable"]).GetList();
					this.comboMiscChassis.DisplayMemberPath = MainWindow.DSet.Tables["StocksTable"].Columns["Chassis"].ToString();
					this.comboMiscChassis.SelectedValuePath = "Chassis";
					this.comboMiscChassis.SelectedIndex = 0;
					this.comboMiscAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboMiscAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboMiscAccounts.SelectedValuePath = "AccountName";
					this.comboMiscAccounts.SelectedIndex = 0;
					this.comboDutyChassis.ItemsSource = ((IListSource)MainWindow.DSet.Tables["StocksTable"]).GetList();
					this.comboDutyChassis.DisplayMemberPath = MainWindow.DSet.Tables["StocksTable"].Columns["Chassis"].ToString();
					this.comboDutyChassis.SelectedValuePath = "Chassis";
					this.comboDutyChassis.SelectedIndex = 0;
					this.comboDutyAgents.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AgentsTable"]).GetList();
					this.comboDutyAgents.DisplayMemberPath = MainWindow.DSet.Tables["AgentsTable"].Columns["Name"].ToString();
					this.comboDutyAgents.SelectedValuePath = "Name";
					this.comboDutyAgents.SelectedIndex = MainWindow.DSet.Tables["AgentsTable"].Rows.Count - 1;
					DataTable dataTable2 = new DataTable();
					dataTable2 = MainWindow.DSet.Tables["AccountTable"].Copy();
					DataRow dataRow = dataTable2.NewRow();
					dataRow["AccountName"] = "All";
					dataTable2.Rows.InsertAt(dataRow, 0);
					this.comboReportAccounts.ItemsSource = ((IListSource)dataTable2).GetList();
					this.comboReportAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboReportAccounts.SelectedValuePath = "AccountName";
					this.comboReportAccounts.SelectedIndex = 0;
					dataTable2 = new DataTable();
					dataTable2 = MainWindow.DSet.Tables["AgentsTable"].Copy();
					dataRow = dataTable2.NewRow();
					dataRow["Name"] = "All";
					dataTable2.Rows.InsertAt(dataRow, 0);
					this.comboReportDutyAgents.ItemsSource = ((IListSource)dataTable2).GetList();
					this.comboReportDutyAgents.DisplayMemberPath = MainWindow.DSet.Tables["AgentsTable"].Columns["Name"].ToString();
					this.comboReportDutyAgents.SelectedValuePath = "Name";
					this.comboReportDutyAgents.SelectedIndex = MainWindow.DSet.Tables["AgentsTable"].Rows.Count - 1;
					dataTable2 = new DataTable();
					dataTable2 = MainWindow.DSet.Tables["CustomersTable"].Copy();
					dataRow = dataTable2.NewRow();
					dataRow["Name"] = "All";
					dataTable2.Rows.InsertAt(dataRow, 0);
					this.comboReportCustomers.ItemsSource = ((IListSource)dataTable2).GetList();
					this.comboReportCustomers.DisplayMemberPath = MainWindow.DSet.Tables["CustomersTable"].Columns["Name"].ToString();
					this.comboReportCustomers.SelectedValuePath = "Name";
					this.comboReportCustomers.SelectedIndex = 0;
					this.comboOfficeAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboOfficeAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboOfficeAccounts.SelectedValuePath = "AccountName";
					this.comboOfficeAccounts.SelectedIndex = 0;
					this.comboDAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboDAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboDAccounts.SelectedValuePath = "AccountName";
					this.comboDAccounts.SelectedIndex = 0;
					this.comboCAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboCAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboCAccounts.SelectedValuePath = "AccountName";
					this.comboCAccounts.SelectedIndex = 0;
					this.comboProfitWithdrawalAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboProfitWithdrawalAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboProfitWithdrawalAccounts.SelectedValuePath = "AccountName";
					this.comboProfitWithdrawalAccounts.SelectedIndex = 0;
					this.comboReceiptAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboReceiptAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboReceiptAccounts.SelectedValuePath = "AccountName";
					this.comboReceiptAccounts.SelectedIndex = 0;
					this.comboReceiptCust.ItemsSource = ((IListSource)MainWindow.DSet.Tables["CustomersTable"]).GetList();
					this.comboReceiptCust.DisplayMemberPath = MainWindow.DSet.Tables["CustomersTable"].Columns["Name"].ToString();
					this.comboReceiptCust.SelectedValuePath = "Name";
					this.comboReceiptCust.SelectedIndex = 0;
					this.comboPaymentAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboPaymentAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboPaymentAccounts.SelectedValuePath = "AccountName";
					this.comboPaymentAccounts.SelectedIndex = 0;
					this.comboPaymentPkrAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					this.comboPaymentPkrAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					this.comboPaymentPkrAccounts.SelectedValuePath = "AccountName";
					this.comboPaymentPkrAccounts.SelectedIndex = 0;
					this.comboPaymentPkrCust.ItemsSource = ((IListSource)MainWindow.DSet.Tables["CustomersTable"]).GetList();
					this.comboPaymentPkrCust.DisplayMemberPath = MainWindow.DSet.Tables["CustomersTable"].Columns["Name"].ToString();
					this.comboPaymentPkrCust.SelectedValuePath = "Name";
					this.comboPaymentPkrCust.SelectedIndex = MainWindow.DSet.Tables["CustomersTable"].Rows.Count - 1;
					dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["SalesTable"].Clone();
					DataRow[] array3 = MainWindow.DSet.Tables["SalesTable"].Select("rowid > " + MySettingsProperty.Settings.strclosingSalesRow);
					foreach (DataRow row2 in array3)
					{
						dataTable.ImportRow(row2);
					}
					this.DataGridSales.DataContext = dataTable.DefaultView;
					dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["ReceiptsTable"].Clone();
					array3 = MainWindow.DSet.Tables["ReceiptsTable"].Select("receiptDetail NOT LIKE '*Sold Chasssis*'");
					foreach (DataRow row3 in array3)
					{
						dataTable.ImportRow(row3);
					}
					this.DataGridReceipts.DataContext = dataTable.DefaultView;
					this.TotalProfit.Text = Conversions.ToString(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["ProfitTable"].Compute("Sum(Amount)", ""))));
					dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["MiscExpTable"].Clone();
					bool flag7 = Operators.CompareString(MySettingsProperty.Settings.strclosingmiscexp, string.Empty, false) != 0;
					if (flag7)
					{
						array3 = MainWindow.DSet.Tables["MiscExpTable"].Select("MiscExpDetail NOT LIKE '*Expense at purchase*' AND rowid >" + MySettingsProperty.Settings.strclosingmiscexp);
					}
					else
					{
						array3 = MainWindow.DSet.Tables["MiscExpTable"].Select("MiscExpDetail NOT LIKE '*Expense at purchase*'");
					}
					foreach (DataRow row4 in array3)
					{
						dataTable.ImportRow(row4);
					}
					this.DataGridMiscExp.DataContext = dataTable.DefaultView;
					dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["DutyExpTable"].Clone();
					bool flag8 = Operators.CompareString(MySettingsProperty.Settings.strclosingDutyExp, string.Empty, false) != 0;
					if (flag8)
					{
						array3 = MainWindow.DSet.Tables["DutyExpTable"].Select("DutyExpDetail NOT LIKE '*Expense at purchase*' AND rowid >" + MySettingsProperty.Settings.strclosingDutyExp);
					}
					else
					{
						array3 = MainWindow.DSet.Tables["DutyExpTable"].Select("DutyExpDetail NOT LIKE '*Expense at purchase*'");
					}
					foreach (DataRow row5 in array3)
					{
						dataTable.ImportRow(row5);
					}
					this.DataGridDutyExp.DataContext = dataTable.DefaultView;
					dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["OfficeExpTable"].Clone();
					bool flag9 = Operators.CompareString(MySettingsProperty.Settings.strclosingDutyExp, string.Empty, false) != 0;
					if (flag9)
					{
						array3 = MainWindow.DSet.Tables["OfficeExpTable"].Select("rowid >" + MySettingsProperty.Settings.strclosingOfficeRow);
						foreach (DataRow row6 in array3)
						{
							dataTable.ImportRow(row6);
						}
					}
					else
					{
						dataTable = MainWindow.DSet.Tables["OfficeExpTable"];
					}
					this.DataGridOfficeExp.DataContext = dataTable.DefaultView;
					this.DataGridPayAgent.DataContext = MainWindow.DSet.Tables["PaymentsAgentTable"].DefaultView;
					this.DataGridPayPkr.DataContext = MainWindow.DSet.Tables["PaymentsPkrTable"].DefaultView;
					this.DataGridOfficeAccount.DataContext = MainWindow.DSet.Tables["OfficeAccountTable"].DefaultView;
				}
				catch (Exception ex)
				{
					MainWindow.errorlog("During Control Updating: " + ex.Message + "\r\n" + ex.ToString());
				}
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000AA60 File Offset: 0x00008E60
		private void Sales_Report()
		{
			checked
			{
				try
				{
					bool flag = MainWindow.DSet.Tables["SalesTable"].Rows.Count > 0;
					if (flag)
					{
						bool flag2 = !this.boolCopied;
						if (flag2)
						{
							int num = MainWindow.DSet.Tables["StocksTable"].Columns.Count - 1;
							for (int i = 2; i <= num; i++)
							{
								MainWindow.DSet.Tables["SalesTable"].Columns.Add(MainWindow.DSet.Tables["StocksTable"].Columns[i].ColumnName, MainWindow.DSet.Tables["StocksTable"].Columns[i].DataType);
							}
							this.boolCopied = true;
						}
						bool flag3 = !MainWindow.DSet.Tables["SalesTable"].Columns.Contains("Profit");
						if (flag3)
						{
							MainWindow.DSet.Tables["SalesTable"].Columns.Add("Profit");
						}
						string str = string.Empty;
						try
						{
							foreach (object obj in MainWindow.DSet.Tables["SalesTable"].Columns)
							{
								DataColumn dataColumn = (DataColumn)obj;
								str = str + ", " + dataColumn.ColumnName;
							}
						}
						finally
						{
							IEnumerator enumerator;
							if (enumerator is IDisposable)
							{
								(enumerator as IDisposable).Dispose();
							}
						}
						int num2 = MainWindow.DSet.Tables["SalesTable"].Rows.Count - 1;
						for (int j = 0; j <= num2; j++)
						{
							DataRow[] array = MainWindow.DSet.Tables["StocksTable"].Select("Chassis = '" + MainWindow.DSet.Tables["SalesTable"].Rows[j]["SaleChassis"].ToString() + "'");
							int num3 = MainWindow.DSet.Tables["StocksTable"].Columns.Count - 1;
							for (int k = 2; k <= num3; k++)
							{
								MainWindow.DSet.Tables["SalesTable"].Rows[j][MainWindow.DSet.Tables["StocksTable"].Columns[k].ColumnName] = RuntimeHelpers.GetObjectValue(array[0][MainWindow.DSet.Tables["StocksTable"].Columns[k]]);
							}
							MainWindow.DSet.Tables["SalesTable"].Rows[j]["Profit"] = Operators.SubtractObject(MainWindow.DSet.Tables["SalesTable"].Rows[j]["SalePrice"], MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["SalesTable"].Rows[j]["Cost"])));
						}
						this.documentViewer2.Visibility = Visibility.Visible;
						this.flowviewertrial.Visibility = Visibility.Collapsed;
					}
					ReportDocument reportDocument = new ReportDocument();
					StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReport.xaml", FileMode.Open, FileAccess.Read));
					reportDocument.XamlData = streamReader.ReadToEnd();
					reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
					streamReader.Close();
					string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
					string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
					DataTable dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["SalesTable"].Clone();
					DataRow[] array2 = MainWindow.DSet.Tables["SalesTable"].Select(string.Concat(new string[]
					{
						"SaleDate >='",
						text,
						"' AND SaleDate <='",
						text2,
						"'"
					}));
					foreach (DataRow row in array2)
					{
						dataTable.ImportRow(row);
					}
					XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
					{
						ReportDocumentValues = 
						{
							{
								"PrintDate",
								DateTime.Now
							}
						},
						DataTables = 
						{
							dataTable
						}
					});
					this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				}
				catch (Exception ex)
				{
					MainWindow.errorlog("During Sales Report: " + ex.Message + "\r\n" + ex.ToString());
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000AFF4 File Offset: 0x000093F4
		private void Stocks_Report()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportStocks.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				DataTable dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["StocksTable"].Clone();
				DataRow[] array = MainWindow.DSet.Tables["StocksTable"].Select("Status IS NULL");
				foreach (DataRow dataRow in array)
				{
					bool flag = Operators.CompareString(this.comboReportStockDuty.Text, "All", false) == 0;
					if (flag)
					{
						dataTable.ImportRow(dataRow);
					}
					else
					{
						bool flag2 = Operators.CompareString(this.comboReportStockDuty.Text, "Duty Paid", false) == 0;
						if (flag2)
						{
							bool flag3 = Operators.ConditionalCompareObjectNotEqual(dataRow["Duty"], 0, false);
							if (flag3)
							{
								dataTable.ImportRow(dataRow);
							}
						}
						else
						{
							bool flag4 = Operators.CompareString(this.comboReportStockDuty.Text, "Duty Not Paid", false) == 0;
							if (flag4)
							{
								bool flag5 = Operators.ConditionalCompareObjectEqual(dataRow["Duty"], 0, false);
								if (flag5)
								{
									dataTable.ImportRow(dataRow);
								}
							}
						}
					}
				}
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Stocks Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000B240 File Offset: 0x00009640
		private void OfficeExpense_Report()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportExpenses.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
				string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
				DataTable dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["OfficeExpTable"].Clone();
				DataRow[] array = MainWindow.DSet.Tables["OfficeExpTable"].Select(string.Concat(new string[]
				{
					"Date >='",
					text,
					"' AND Date <='",
					text2,
					"'"
				}));
				foreach (DataRow row in array)
				{
					dataTable.ImportRow(row);
				}
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Office Expense Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B440 File Offset: 0x00009840
		private void Accounts_Report()
		{
			checked
			{
				try
				{
					string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
					string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
					ReportDocument reportDocument = new ReportDocument();
					StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportAccounts.xaml", FileMode.Open, FileAccess.Read));
					reportDocument.XamlData = streamReader.ReadToEnd();
					reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
					streamReader.Close();
					DataTable dataTable = new DataTable();
					bool flag = Operators.CompareString(this.comboReportAccounts.Text, "All", false) == 0;
					if (flag)
					{
						dataTable = MainWindow.DSet.Tables["LedgerTable"].Clone();
						DataRow[] array = MainWindow.DSet.Tables["LedgerTable"].Select(string.Concat(new string[]
						{
							"DATE >='",
							text,
							"' AND DATE <='",
							text2,
							"' AND Amount <> 0"
						}));
						foreach (DataRow row in array)
						{
							dataTable.ImportRow(row);
						}
						DataView defaultView = dataTable.DefaultView;
						defaultView.Sort = "DATE ASC, rowid ASC";
						dataTable = defaultView.ToTable();
						dataTable.Columns.Add("Balance");
						dataTable.Columns.Add("Debit");
						dataTable.Columns.Add("Credit");
						DataRow dataRow = dataTable.NewRow();
						dataRow["Amount"] = 0;
						dataRow["Detail"] = "Sum of All Opening Balances on " + text;
						dataRow["Balance"] = Operators.AddObject(MainWindow.DSet.Tables["AccountTable"].Compute("SUM(OpeningBalance)", ""), MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["LedgerTable"].Compute("Sum(Amount)", "DATE <'" + text + "'"))));
						dataTable.Rows.InsertAt(dataRow, 0);
						int num = dataTable.Rows.Count - 1;
						for (int j = 1; j <= num; j++)
						{
							dataTable.Rows[j]["Balance"] = Operators.AddObject(dataTable.Rows[j - 1]["Balance"], dataTable.Rows[j]["Amount"]);
							bool flag2 = Operators.ConditionalCompareObjectLess(dataTable.Rows[j]["Amount"], 0, false);
							if (flag2)
							{
								dataTable.Rows[j]["Credit"] = Operators.MultiplyObject(dataTable.Rows[j]["Amount"], -1);
							}
							else
							{
								dataTable.Rows[j]["Debit"] = RuntimeHelpers.GetObjectValue(dataTable.Rows[j]["Amount"]);
							}
						}
						reportDocument.ReportTitle = "All Accounts\r\nFrom: " + text + " To: " + text2;
					}
					else
					{
						dataTable = MainWindow.DSet.Tables["LedgerTable"].Clone();
						DataRow[] array3 = MainWindow.DSet.Tables["LedgerTable"].Select(string.Concat(new string[]
						{
							"Account = '",
							this.comboReportAccounts.Text,
							"' AND DATE >='",
							text,
							"' AND DATE <='",
							text2,
							"' AND Amount <> 0"
						}));
						foreach (DataRow row2 in array3)
						{
							dataTable.ImportRow(row2);
						}
						DataView defaultView2 = dataTable.DefaultView;
						defaultView2.Sort = "DATE ASC, rowid ASC";
						dataTable = defaultView2.ToTable();
						dataTable.Columns.Add("Balance");
						dataTable.Columns.Add("Debit");
						dataTable.Columns.Add("Credit");
						DataRow dataRow2 = dataTable.NewRow();
						dataRow2["Detail"] = "Opening Balance of " + this.comboReportAccounts.Text + " on " + text;
						dataRow2["Amount"] = 0;
						dataRow2["Balance"] = Operators.AddObject(MainWindow.DSet.Tables["AccountTable"].Select("AccountName = '" + this.comboReportAccounts.Text + "'")[0]["OpeningBalance"], MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["LedgerTable"].Compute("Sum(Amount)", string.Concat(new string[]
						{
							"Account = '",
							this.comboReportAccounts.Text,
							"' AND DATE <'",
							text,
							"'"
						})))));
						dataTable.Rows.InsertAt(dataRow2, 0);
						int num2 = dataTable.Rows.Count - 1;
						for (int l = 1; l <= num2; l++)
						{
							dataTable.Rows[l]["Balance"] = Operators.AddObject(dataTable.Rows[l - 1]["Balance"], dataTable.Rows[l]["Amount"]);
							bool flag3 = Operators.ConditionalCompareObjectLess(dataTable.Rows[l]["Amount"], 0, false);
							if (flag3)
							{
								dataTable.Rows[l]["Credit"] = Operators.MultiplyObject(dataTable.Rows[l]["Amount"], -1);
							}
							else
							{
								dataTable.Rows[l]["Debit"] = RuntimeHelpers.GetObjectValue(dataTable.Rows[l]["Amount"]);
							}
						}
						reportDocument.ReportTitle = string.Concat(new string[]
						{
							"Accounts: ",
							this.comboReportAccounts.Text,
							"\r\nFrom: ",
							text,
							" To: ",
							text2
						});
					}
					XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
					{
						ReportDocumentValues = 
						{
							{
								"PrintDate",
								DateTime.Now
							}
						},
						DataTables = 
						{
							dataTable
						}
					});
					this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
					this.documentViewer2.Visibility = Visibility.Visible;
					this.flowviewertrial.Visibility = Visibility.Collapsed;
				}
				catch (Exception ex)
				{
					MainWindow.errorlog("During Accounts Report: " + ex.Message + "\r\n" + ex.ToString());
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000BBFC File Offset: 0x00009FFC
		private void Receipts_Report()
		{
			try
			{
				string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
				string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportReceipts.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				reportDocument.ReportTitle = this.comboReportCustomers.Text;
				DataTable dataTable = new DataTable();
				bool flag = Operators.CompareString(this.comboReportCustomers.Text, "All", false) == 0;
				if (flag)
				{
					dataTable = MainWindow.DSet.Tables["ReceiptsTable"].Clone();
					DataRow[] array = MainWindow.DSet.Tables["ReceiptsTable"].Select(string.Concat(new string[]
					{
						"DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					foreach (DataRow row in array)
					{
						dataTable.ImportRow(row);
					}
				}
				else
				{
					dataTable = MainWindow.DSet.Tables["ReceiptsTable"].Clone();
					DataRow[] array3 = MainWindow.DSet.Tables["ReceiptsTable"].Select(string.Concat(new string[]
					{
						"receivedFrom ='",
						this.comboReportCustomers.Text,
						"' AND DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					foreach (DataRow row2 in array3)
					{
						dataTable.ImportRow(row2);
					}
				}
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Receipts Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000BEDC File Offset: 0x0000A2DC
		private void MiscAutoExpense_Report()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportMiscExpenses.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
				string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
				DataTable dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["MiscExpTable"].Clone();
				DataRow[] array = MainWindow.DSet.Tables["MiscExpTable"].Select(string.Concat(new string[]
				{
					"DATE >='",
					text,
					"' AND DATE <='",
					text2,
					"'"
				}));
				foreach (DataRow row in array)
				{
					dataTable.ImportRow(row);
				}
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Misc Exp Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000C0DC File Offset: 0x0000A4DC
		private void Payments_Report()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportPayments.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
				string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
				DataTable dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["PaymentsTable"].Clone();
				DataRow[] array = MainWindow.DSet.Tables["PaymentsTable"].Select(string.Concat(new string[]
				{
					"PaymentAmountPkr <> 0 AND DATE >='",
					text,
					"' AND DATE <='",
					text2,
					"'"
				}));
				foreach (DataRow row in array)
				{
					dataTable.ImportRow(row);
				}
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Payments Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C2DC File Offset: 0x0000A6DC
		private void PartyPayments_Report()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportPaymentsPkr.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				reportDocument.ReportTitle = this.comboReportCustomers.Text;
				DataTable dataTable = new DataTable();
				DataTable dataTable2 = new DataTable();
				DataTable dataTable3 = new DataTable();
				DataTable dataTable4 = new DataTable();
				dataTable4.TableName = "SummaryTable";
				string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
				string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
				dataTable = MainWindow.DSet.Tables["PaymentsPkrTable"].Clone();
				dataTable2 = MainWindow.DSet.Tables["ReceiptsTable"].Clone();
				dataTable3 = MainWindow.DSet.Tables["SalesTable"].Clone();
				dataTable4.Columns.Add("Total Paid");
				dataTable4.Columns.Add("Total Received");
				dataTable4.Columns.Add("Total Sold");
				dataTable4.Columns.Add("Balance");
				DataRow dataRow = dataTable4.NewRow();
				bool flag = Operators.CompareString(this.comboReportCustomers.Text, "All", false) == 0;
				DataRow[] array;
				DataRow[] array2;
				DataRow[] array3;
				if (flag)
				{
					array = MainWindow.DSet.Tables["PaymentsPkrTable"].Select(string.Concat(new string[]
					{
						"DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					array2 = MainWindow.DSet.Tables["ReceiptsTable"].Select(string.Concat(new string[]
					{
						"DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					array3 = MainWindow.DSet.Tables["SalesTable"].Select(string.Concat(new string[]
					{
						"SaleDate >='",
						text,
						"' AND SaleDate <='",
						text2,
						"'"
					}));
				}
				else
				{
					array = MainWindow.DSet.Tables["PaymentsPkrTable"].Select(string.Concat(new string[]
					{
						"PaidTo ='",
						this.comboReportCustomers.Text,
						"' AND DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					array2 = MainWindow.DSet.Tables["ReceiptsTable"].Select(string.Concat(new string[]
					{
						"receivedFrom ='",
						this.comboReportCustomers.Text,
						"' AND DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					array3 = MainWindow.DSet.Tables["SalesTable"].Select(string.Concat(new string[]
					{
						"SaleCustomer ='",
						this.comboReportCustomers.Text,
						"' AND SaleDate >='",
						text,
						"' AND SaleDate <='",
						text2,
						"'"
					}));
				}
				foreach (DataRow row in array)
				{
					dataTable.ImportRow(row);
				}
				foreach (DataRow row2 in array2)
				{
					dataTable2.ImportRow(row2);
				}
				foreach (DataRow row3 in array3)
				{
					dataTable3.ImportRow(row3);
				}
				bool flag2 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(dataTable.Compute("Sum(PaymentAmount)", "")));
				if (flag2)
				{
					dataRow["Total Paid"] = RuntimeHelpers.GetObjectValue(dataTable.Compute("Sum(PaymentAmount)", ""));
				}
				else
				{
					dataRow["Total Paid"] = 0;
				}
				bool flag3 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(dataTable2.Compute("Sum(receiptAmount)", "")));
				if (flag3)
				{
					dataRow["Total Received"] = RuntimeHelpers.GetObjectValue(dataTable2.Compute("Sum(receiptAmount)", ""));
				}
				else
				{
					dataRow["Total Received"] = 0;
				}
				bool flag4 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(dataTable3.Compute("Sum(SalePrice)", "")));
				if (flag4)
				{
					dataRow["Total Sold"] = RuntimeHelpers.GetObjectValue(dataTable3.Compute("Sum(SalePrice)", ""));
				}
				else
				{
					dataRow["Total Sold"] = 0;
				}
				dataRow["Balance"] = checked(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(dataRow["Total Paid"])) + MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(dataRow["Total Sold"])) - MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(dataRow["Total Received"])));
				bool flag5 = Operators.ConditionalCompareObjectLess(dataRow["Balance"], 0, false);
				if (flag5)
				{
					dataRow["Balance"] = Operators.ConcatenateObject("Total Payable: ", Operators.MultiplyObject(dataRow["Balance"], -1));
				}
				else
				{
					dataRow["Balance"] = Operators.ConcatenateObject("Total Receivable: ", dataRow["Balance"]);
				}
				dataTable4.Rows.InsertAt(dataRow, 0);
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable,
						dataTable2,
						dataTable3,
						dataTable4
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Payments Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000C9E4 File Offset: 0x0000ADE4
		private void btnenabler(bool @bool)
		{
			this.BtnAddAccount.IsEnabled = @bool;
			this.BtnAddCust.IsEnabled = @bool;
			this.BtnAddStocks.IsEnabled = @bool;
			this.btnPaymentEntry.IsEnabled = @bool;
			this.btnPaymentPkrEntry.IsEnabled = @bool;
			this.btnOfficeExpEntry.IsEnabled = @bool;
			this.btnMiscExpEntry.IsEnabled = @bool;
			this.btnDutyExpEntry.IsEnabled = @bool;
			this.btnReceiptEntry.IsEnabled = @bool;
			this.btnSaleEntry.IsEnabled = @bool;
			this.btnProfitWithdrawalEntry.IsEnabled = @bool;
			this.btnOfficeAccountEntry.IsEnabled = @bool;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000CA90 File Offset: 0x0000AE90
		private void BtnAddAccount_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddAccount addAccount = new AddAccount();
				addAccount.ShowDialog();
				bool flag = addAccount.DialogResult != null && addAccount.DialogResult.Value;
				if (flag)
				{
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							sqliteCommand2.Connection = MainWindow.Connection;
							bool flag2 = addAccount.stpBank.Visibility == Visibility.Visible;
							if (flag2)
							{
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO AccountTable(AccountDate, AccountType, AccountName, AccountNumber, AccountTitle, BankName, BankBranch, OpeningBalance, CurrentBalance) SELECT '",
									DateTime.Now.ToString("yyyy-MM-dd"),
									"', '",
									addAccount.comboAcctype.Text,
									"', '",
									addAccount.txtAccName.Text,
									"', '",
									addAccount.txtAccNum.Text,
									"', '",
									addAccount.txtAccTitle.Text,
									"', '",
									addAccount.BankName.Text,
									"', '",
									addAccount.BankBranch.Text,
									"', '",
									addAccount.txtAccBalance.Text,
									"', '",
									addAccount.txtAccBalance.Text,
									"' WHERE NOT EXISTS(SELECT 1 FROM AccountTable WHERE AccountNumber = '",
									addAccount.txtAccName.Text,
									"');"
								});
							}
							else
							{
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO AccountTable(AccountDate, AccountType, AccountName, OpeningBalance, CurrentBalance) SELECT '",
									DateTime.Now.ToString("yyyy-MM-dd"),
									"', '",
									addAccount.comboAcctype.Text,
									"', '",
									addAccount.txtAccName.Text,
									"', '",
									addAccount.txtAccBalance.Text,
									"', '",
									addAccount.txtAccBalance.Text,
									"' WHERE NOT EXISTS(SELECT 1 FROM AccountTable WHERE AccountNumber = '",
									addAccount.txtAccName.Text,
									"');"
								});
							}
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Account: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000CD90 File Offset: 0x0000B190
		public static void errorlog(string StrError)
		{
			StreamWriter streamWriter = new StreamWriter(MyWpfExtension.Application.Info.DirectoryPath + "\\Log.txt", true);
			streamWriter.WriteLine("TimeStamp: " + DateTime.Now.ToString() + "\tMessage: " + StrError);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000CDF0 File Offset: 0x0000B1F0
		private void BtnEditAccount_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddAccount addAccount = new AddAccount();
				addAccount.Title = "Edit Account";
				addAccount.AccInfoText.Text = "Edit Bank Account";
				addAccount.comboAcctype.SelectedValue = this.runAccountType.Text;
				addAccount.txtAccBalance.Text = this.runOpeningBalance.Text;
				addAccount.txtAccName.Text = this.runAccountName.Text;
				addAccount.txtAccNum.Text = this.runAccountNumber.Text;
				addAccount.txtAccTitle.Text = this.runAccountTitle.Text;
				addAccount.BankBranch.Text = this.runBankBranch.Text;
				addAccount.BankName.Text = this.runBankName.Text;
				bool flag = Operators.CompareString(this.runAccountType.Text, "Bank", false) == 0;
				if (flag)
				{
					addAccount.stpBank.Visibility = Visibility.Visible;
				}
				else
				{
					addAccount.stpBank.Visibility = Visibility.Hidden;
				}
				addAccount.ShowDialog();
				bool flag2 = addAccount.DialogResult != null && addAccount.DialogResult.Value;
				if (flag2)
				{
					double num = Conversions.ToDouble(addAccount.txtAccBalance.Text);
					double num2 = Conversions.ToDouble(this.runCurrentBalance.Text);
					bool flag3 = num != Convert.ToDouble(this.runOpeningBalance.Text);
					if (flag3)
					{
						num2 += num - Convert.ToDouble(this.runOpeningBalance.Text);
						Interaction.MsgBox(DateTime.Now.ToString("yyyy-MM-dd"), MsgBoxStyle.OkOnly, null);
					}
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							sqliteCommand2.Connection = MainWindow.Connection;
							bool flag4 = addAccount.stpBank.Visibility == Visibility.Visible;
							if (flag4)
							{
								sqliteCommand2.CommandText = (string.Concat(new string[]
								{
									"UPDATE AccountTable SET AccountDate='",
									DateTime.Now.ToString("yyyy-MM-dd"),
									"', AccountType ='",
									addAccount.comboAcctype.Text,
									"', AccountName ='",
									addAccount.txtAccName.Text,
									"', AccountNumber ='",
									addAccount.txtAccNum.Text,
									"', AccountTitle ='",
									addAccount.txtAccTitle.Text,
									"', BankName ='",
									addAccount.BankName.Text,
									"', BankBranch ='",
									addAccount.BankBranch.Text,
									"', OpeningBalance ='",
									Conversions.ToString(num),
									"', CurrentBalance ='",
									Conversions.ToString(num2),
									"' WHERE rowid = ",
									this.runRowId.Text
								}) ?? "");
							}
							else
							{
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable SET AccountDate='",
									DateTime.Now.ToString("yyyy-MM-dd"),
									"', AccountType ='",
									addAccount.comboAcctype.Text,
									"', AccountName ='",
									addAccount.txtAccName.Text,
									"', OpeningBalance ='",
									Conversions.ToString(num),
									"', CurrentBalance ='",
									Conversions.ToString(num2),
									"' WHERE rowid = '",
									this.runRowId.Text,
									"'"
								});
							}
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						int selectedIndex = this.LstAccounts.SelectedIndex;
						this.controlupdater();
						this.LstAccounts.SelectedIndex = selectedIndex;
						this.LstAccounts.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Account: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D278 File Offset: 0x0000B678
		private void BtnAddStocks_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PurchaseAuto purchaseAuto = new PurchaseAuto();
				purchaseAuto.comboPaymentAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
				purchaseAuto.comboPaymentAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
				purchaseAuto.comboPaymentAccounts.SelectedValuePath = "AccountName";
				purchaseAuto.comboPaymentAccounts.SelectedIndex = 0;
				purchaseAuto.ShowDialog();
				bool flag = purchaseAuto.DialogResult != null && purchaseAuto.DialogResult.Value;
				if (flag)
				{
					PurchaseAuto purchaseAuto2 = purchaseAuto;
					string text = purchaseAuto2.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd") + "', '";
					text = text + purchaseAuto2.txtPurChassisNo.Text + "', '";
					text = text + purchaseAuto2.txtPurModel.Text + "', '";
					text = text + purchaseAuto2.txtPurColor.Text + "', '";
					text = text + purchaseAuto2.txtPaymentPrice.Text + "', '";
					text = text + purchaseAuto2.txtPaymentRate.Text + "', '";
					text = text + purchaseAuto2.txtPaymentAmountPkr.Text + "', '";
					text = text + purchaseAuto2.txtPurDuty.Text + "', '";
					text = text + purchaseAuto2.txtPurMiscExp.Text + "', '";
					text = text + purchaseAuto2.intPaymentAmount.Text + "', '";
					text = text + purchaseAuto2.txtPaymentAmountPkr.Text + "', '";
					text = text + purchaseAuto2.txtPurComments.Text + "', '";
					double[] source = new double[]
					{
						Conversions.ToDouble(purchaseAuto2.txtPurDuty.Text),
						Conversions.ToDouble(purchaseAuto2.txtPurMiscExp.Text),
						Conversions.ToDouble(purchaseAuto2.txtPaymentAmountPkr.Text)
					};
					bool flag2 = Operators.CompareString(purchaseAuto2.txtPaymentPrice.Text, purchaseAuto2.intPaymentAmount.Text, false) == 0;
					double value;
					if (flag2)
					{
						value = source.Sum();
					}
					else
					{
						value = 0.0;
					}
					text += Conversions.ToString(value);
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							double[] array = new double[]
							{
								Conversions.ToDouble(purchaseAuto.txtPaymentAmountPkr.Text),
								Conversions.ToDouble(purchaseAuto.txtPurDuty.Text),
								Conversions.ToDouble(purchaseAuto.txtPurMiscExp.Text)
							};
							sqliteCommand2.Connection = MainWindow.Connection;
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO StocksTable(Date, Chassis, Model, Color, PriceYen, Rate, PricePkr, Duty, MiscExpense, PaidYen, PaidAmount, Comments, Cost) SELECT '",
								text,
								"' WHERE NOT EXISTS(SELECT 1 FROM StocksTable WHERE Chassis = '",
								purchaseAuto.txtPurChassisNo.Text,
								"');"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO MiscExpTable(chassis, MiscExpDate, MiscExpAmount, MiscExpDetail, MiscExpPaidBy) VALUES ('",
								purchaseAuto.txtPurChassisNo.Text,
								"', '",
								purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', ",
								Conversions.ToString(array[2]),
								", 'Expense at purchase: ",
								purchaseAuto.txtPurChassisNo.Text,
								" Comments: ",
								purchaseAuto.txtPurComments.Text,
								"', '",
								purchaseAuto.comboPaymentAccounts.Text,
								"');"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO DutyExpTable(chassis, DutyExpDate, DutyExpAmount, DutyExpDetail, DutyExpPaidBy, DutyExpAgent) VALUES ('",
								purchaseAuto.txtPurChassisNo.Text,
								"', '",
								purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', ",
								Conversions.ToString(array[1]),
								", 'Expense at purchase: ",
								purchaseAuto.txtPurChassisNo.Text,
								" Comments: ",
								purchaseAuto.txtPurComments.Text,
								"', '",
								purchaseAuto.comboPaymentAccounts.Text,
								"', '",
								purchaseAuto.comboDutyAgents.Text,
								"');"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"UPDATE AccountTable SET CurrentBalance = CurrentBalance - ",
								Conversions.ToString(array[0] + array[2]),
								" WHERE AccountName ='",
								purchaseAuto.comboPaymentAccounts.Text,
								"'"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
								purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', ",
								Conversions.ToString(array[0] * -1.0),
								", 'Amount Paid at Stock purchase (chassis): ",
								purchaseAuto.txtPurChassisNo.Text,
								"', '",
								purchaseAuto.comboPaymentAccounts.Text,
								"');"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
								purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', ",
								Conversions.ToString(array[2] * -1.0),
								", 'Misc Expenses at purchase (chassis): ",
								purchaseAuto.txtPurChassisNo.Text,
								"', '",
								purchaseAuto.comboPaymentAccounts.Text,
								"');"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable - ",
								Conversions.ToString(array[1]),
								" WHERE Name = '",
								purchaseAuto.comboDutyAgents.Text,
								"'"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO PaymentsTable(PaymentDate, PaymentAmountYen, PaymentExcRate, PaymentAmountPkr, PaymentDetail, PaidFrom) VALUES('",
								purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', '",
								purchaseAuto.txtPaymentAmount.Text,
								"', '",
								purchaseAuto.txtPaymentRate.Text,
								"', '",
								purchaseAuto.txtPaymentAmountPkr.Text,
								"', '",
								purchaseAuto.txtPurComments.Text,
								" REF-",
								purchaseAuto.txtPurChassisNo.Text,
								"', '",
								purchaseAuto.comboPaymentAccounts.Text,
								"');"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Stock: " + ex.Message + "\r\n" + ex.ToString());
				Interaction.MsgBox("When Adding Stock: " + ex.ToString(), MsgBoxStyle.OkOnly, null);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000251A File Offset: 0x0000091A
		private void BtnDeleteStocks_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000DAD4 File Offset: 0x0000BED4
		private void BtnEditStock_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.runStockStatus.Text, "Sold", false) == 0;
			if (flag)
			{
				Interaction.MsgBox("Sold Entry cannot Edited", MsgBoxStyle.OkOnly, null);
			}
			else
			{
				double num = Conversions.ToDouble(this.runStockDuty.Text);
				double num2 = Conversions.ToDouble(this.runStockMiscExpense.Text);
				try
				{
					DataRow dataRow = MainWindow.DSet.Tables["PaymentsTable"].Select("PaymentDetail LIKE '%" + this.runStockchassis.Text + "%'")[0];
					DataRow dataRow2 = MainWindow.DSet.Tables["MiscExpTable"].Select("Chassis ='" + this.runStockchassis.Text + "'")[0];
					DataRow dataRow3 = MainWindow.DSet.Tables["DutyExpTable"].Select("Chassis ='" + this.runStockchassis.Text + "'")[0];
					double num3 = Conversions.ToDouble(dataRow2["MiscExpAmount"]);
					double num4 = Conversions.ToDouble(dataRow3["DutyExpAmount"]);
					PurchaseAuto purchaseAuto = new PurchaseAuto();
					purchaseAuto.Title = "Edit Auto Purchased Entry";
					purchaseAuto.AccInfoText.Text = "Edit Auto Parchased";
					purchaseAuto.comboPaymentAccounts.ItemsSource = ((IListSource)MainWindow.DSet.Tables["AccountTable"]).GetList();
					purchaseAuto.comboPaymentAccounts.DisplayMemberPath = MainWindow.DSet.Tables["AccountTable"].Columns["AccountName"].ToString();
					purchaseAuto.comboPaymentAccounts.SelectedValuePath = "AccountName";
					purchaseAuto.comboPaymentAccounts.SelectedIndex = 0;
					PurchaseAuto purchaseAuto2 = purchaseAuto;
					string[] array = this.runStockDAte.Text.Split(new char[]
					{
						'-'
					});
					purchaseAuto2.PaymentdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
					purchaseAuto2.selectedAgent = Conversions.ToString(dataRow3["DutyExpAgent"]);
					purchaseAuto2.intPaymentAmount.Value = new int?(checked((int)Math.Round(Conversions.ToDouble(this.runStockAmountPaid.Text) / Conversions.ToDouble(this.runStockRate.Text))));
					purchaseAuto2.txtPurChassisNo.Text = this.runStockchassis.Text;
					purchaseAuto2.txtPurModel.Text = this.runStockModel.Text;
					purchaseAuto2.txtPurColor.Text = this.runStockColor.Text;
					purchaseAuto2.txtPaymentPrice.Text = this.runStockPriceYen.Text;
					purchaseAuto2.txtPaymentRate.Text = Conversions.ToString(dataRow["PaymentExcRate"]);
					purchaseAuto2.intPaymentAmount.Text = Conversions.ToString(dataRow["PaymentAmountYen"]);
					purchaseAuto2.txtPaymentAmount.Text = Conversions.ToString(dataRow["PaymentAmountYen"]);
					purchaseAuto2.txtPaymentAmountPkr.Text = Conversions.ToString(dataRow["PaymentAmountPkr"]);
					purchaseAuto2.txtPurDuty.Text = Conversions.ToString(dataRow3["DutyExpAmount"]);
					purchaseAuto2.txtPurMiscExp.Text = Conversions.ToString(dataRow2["MiscExpAmount"]);
					purchaseAuto2.txtPurComments.Text = this.runStockComments.Text;
					purchaseAuto2.comboPaymentAccounts.Text = Conversions.ToString(dataRow["PaidFrom"]);
					purchaseAuto.ShowDialog();
					bool flag2 = purchaseAuto.DialogResult != null && purchaseAuto.DialogResult.Value;
					if (flag2)
					{
						PurchaseAuto purchaseAuto3 = purchaseAuto;
						double[] array2 = new double[]
						{
							Conversions.ToDouble(purchaseAuto3.txtPurDuty.Text),
							Conversions.ToDouble(purchaseAuto3.txtPurMiscExp.Text),
							Conversions.ToDouble(purchaseAuto3.txtPaymentAmountPkr.Text)
						};
						double num5 = Conversions.ToDouble(NewLateBinding.LateGet(null, typeof(Math), "Round", new object[]
						{
							Operators.DivideObject(Operators.AddObject(Operators.SubtractObject(MainWindow.convertInteger(this.runStockAmountPaid.Text), dataRow["PaymentAmountPkr"]), purchaseAuto3.txtPaymentAmountPkr.Text), Operators.AddObject(Operators.SubtractObject(MainWindow.convertInteger(this.runStockAmountPaidYen.Text), dataRow["PaymentAmountYen"]), purchaseAuto3.intPaymentAmount.Text)),
							4
						}, null, null, null));
						bool flag3 = double.IsNaN(num5);
						if (flag3)
						{
							num5 = 0.0;
							Interaction.MsgBox("ok", MsgBoxStyle.OkOnly, null);
						}
						double num6;
						string text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
						{
							"UPDATE StocksTable Set Date ='",
							purchaseAuto3.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
							"', Chassis ='",
							purchaseAuto3.txtPurChassisNo.Text,
							"', Model ='",
							purchaseAuto3.txtPurModel.Text,
							" ', Color ='",
							purchaseAuto3.txtPurColor.Text,
							"', PriceYen =",
							purchaseAuto3.txtPaymentPrice.Text,
							", Rate =",
							Conversions.ToString(num5),
							", PricePkr ="
						}), Operators.AddObject(Operators.SubtractObject(MainWindow.convertInteger(Conversions.ToDouble(this.runStockPriceYen.Text) * Conversions.ToDouble(this.runStockRate.Text)), dataRow["PaymentAmountPkr"]), purchaseAuto3.txtPaymentAmountPkr.Text)), ", "), "Duty ="), Operators.AddObject(Operators.SubtractObject(num, dataRow3["DutyExpAmount"]), MainWindow.convertInteger(purchaseAuto3.txtPurDuty.Text))), ", "), "MiscExpense ="), Operators.AddObject(Operators.SubtractObject(num2, dataRow2["MiscExpAmount"]), MainWindow.convertInteger(purchaseAuto3.txtPurMiscExp.Text))), ", "), "PaidYen ="), Operators.AddObject(Operators.SubtractObject(MainWindow.convertInteger(this.runStockAmountPaidYen.Text), dataRow["PaymentAmountYen"]), purchaseAuto3.intPaymentAmount.Text)), ", "), "PaidAmount ="), Operators.AddObject(Operators.SubtractObject(MainWindow.convertInteger(this.runStockAmountPaid.Text), dataRow["PaymentAmountPkr"]), purchaseAuto3.txtPaymentAmountPkr.Text)), ", "), "Cost ="), num6), ", "), "Comments ='"), purchaseAuto3.txtPurComments.Text), "' WHERE rowid ="), this.runStockId.Text));
						Interaction.MsgBox(text, MsgBoxStyle.OkOnly, null);
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = text;
								sqliteCommand2.ExecuteNonQuery();
								double[] array3 = new double[]
								{
									Conversions.ToDouble(purchaseAuto.txtPaymentAmountPkr.Text),
									Conversions.ToDouble(purchaseAuto.txtPurDuty.Text),
									Conversions.ToDouble(purchaseAuto.txtPurMiscExp.Text)
								};
								double[] array4 = new double[]
								{
									Conversions.ToDouble(dataRow["PaymentAmountPkr"]),
									num4,
									num3
								};
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable SET CurrentBalance = CurrentBalance + " + Conversions.ToString(array4.Sum() - array4[1]) + " WHERE AccountName ='", dataRow["PaidFrom"]), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable SET CurrentBalance = CurrentBalance - ",
									Conversions.ToString(array3.Sum() - array3[1]),
									" WHERE AccountName ='",
									purchaseAuto.comboPaymentAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE MiscExpTable SET chassis = '",
									purchaseAuto.txtPurChassisNo.Text,
									"', MiscExpAmount = ",
									Conversions.ToString(array3[2]),
									", MiscExpDetail = 'Expense at purchase: ",
									purchaseAuto.txtPurComments.Text,
									"', MiscExpPaidBy = '",
									purchaseAuto.comboPaymentAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM MiscExpTable WHERE chassis LIKE '%",
									purchaseAuto.txtPurChassisNo.Text,
									"%' LIMIT 1)"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE DutyExpTable SET chassis = '",
									purchaseAuto.txtPurChassisNo.Text,
									"', DutyExpAmount = ",
									Conversions.ToString(array3[1]),
									", DutyExpDetail = 'Duty at purchase: ",
									purchaseAuto.txtPurComments.Text,
									"', DutyExpAgent = '",
									purchaseAuto.comboDutyAgents.Text,
									"', DutyExpPaidBy = '",
									purchaseAuto.comboPaymentAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM DutyExpTable WHERE chassis LIKE '%",
									purchaseAuto.txtPurChassisNo.Text,
									"%' LIMIT 1)"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount = ",
									Conversions.ToString(array3[0] * -1.0),
									", Detail = 'Amount Paid at Stock purchase (chassis): ",
									purchaseAuto.txtPurChassisNo.Text,
									"', Account = '",
									purchaseAuto.comboPaymentAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Detail LIKE '%Amount Paid at Stock purchase (chassis): ",
									purchaseAuto.txtPurChassisNo.Text,
									"%' LIMIT 1)"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount = ",
									Conversions.ToString(array3[2] * -1.0),
									", Detail = 'Misc Expenses at purchase (chassis): ",
									purchaseAuto.txtPurChassisNo.Text,
									"', Account = '",
									purchaseAuto.comboPaymentAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Detail LIKE '%Misc Expenses at purchase (chassis): ",
									purchaseAuto.txtPurChassisNo.Text,
									"%' LIMIT 1)"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable - ",
									Conversions.ToString(array3[1]),
									" WHERE Name = '",
									purchaseAuto.comboDutyAgents.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable + " + Conversions.ToString(array4[1]) + " WHERE Name = '", dataRow3["DutyExpAgent"]), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE PaymentsTable SET PaymentDate ='",
									purchaseAuto.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', PaymentAmountYen =",
									purchaseAuto.txtPaymentAmount.Text,
									", PaymentExcRate =",
									purchaseAuto.txtPaymentRate.Text,
									", PaymentAmountPkr =",
									purchaseAuto.txtPaymentAmountPkr.Text,
									", PaymentDetail ='",
									purchaseAuto.txtPurComments.Text,
									" REF-",
									purchaseAuto.txtPurChassisNo.Text,
									"', PaidFrom ='",
									purchaseAuto.comboPaymentAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM PaymentsTable WHERE PaymentDetail LIKE '%",
									purchaseAuto.txtPurChassisNo.Text,
									"%' LIMIT 1)"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							int selectedIndex = this.LstStocks.SelectedIndex;
							this.controlupdater();
							this.LstStocks.SelectedIndex = selectedIndex;
							this.LstStocks.Focus();
						}
					}
				}
				catch (Exception ex)
				{
					MainWindow.errorlog("When Editing Stock: " + ex.Message + "\r\n" + ex.ToString());
				}
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000E90C File Offset: 0x0000CD0C
		private void BtnAddCust_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddCustomer addCustomer = new AddCustomer();
				addCustomer.ShowDialog();
				bool flag = addCustomer.DialogResult != null && addCustomer.DialogResult.Value;
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
								"INSERT INTO CustomersTable(Date, Title, Name, CNIC, Phone, Address) SELECT '",
								addCustomer.CustdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', '",
								addCustomer.txtCustTitle.Text,
								"', '",
								addCustomer.txtCustName.Text,
								"', '"
							}), addCustomer.txtCnicNo.Value), "', '"), addCustomer.txtCustPhoneNo.Text), "', '"), addCustomer.txtCustAddress.Text), "' WHERE NOT EXISTS(SELECT 1 FROM CustomersTable WHERE Name = '"), addCustomer.txtCustName.Text), "');"));
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Addting Customer: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000EB14 File Offset: 0x0000CF14
		private void BtnEditCust_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddCustomer addCustomer = new AddCustomer();
				addCustomer.Title = "Edit Customer Entry";
				addCustomer.AccInfoText.Text = "Edit Customer";
				AddCustomer addCustomer2 = addCustomer;
				string[] array = this.RunCustDate.Text.Split(new char[]
				{
					'-'
				});
				addCustomer2.CustdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				addCustomer2.txtCustTitle.Text = this.RunCustTitle.Text;
				addCustomer2.txtCustName.Text = this.RunCustName.Text;
				addCustomer2.txtCnicNo.Value = this.RunCustCNIC.Text;
				addCustomer2.txtCustPhoneNo.Text = this.RunCustPhone.Text;
				addCustomer2.txtCustAddress.Text = this.RunCustAddress.Text;
				addCustomer.ShowDialog();
				bool flag = addCustomer.DialogResult != null && addCustomer.DialogResult.Value;
				if (flag)
				{
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							sqliteCommand2.Connection = MainWindow.Connection;
							sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
							{
								"UPDATE CustomersTable SET Date ='",
								addCustomer.CustdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', Title ='",
								addCustomer.txtCustTitle.Text,
								"', Name ='",
								addCustomer.txtCustName.Text,
								"', CNIC ='"
							}), addCustomer.txtCnicNo.Value), "', Phone ='"), addCustomer.txtCustPhoneNo.Text), "', Address ='"), addCustomer.txtCustAddress.Text), "' WHERE rowid = "), this.RunCustId.Text));
							sqliteCommand2.ExecuteNonQuery();
							bool flag2 = Operators.CompareString(addCustomer.txtCustName.Text, this.RunCustName.Text, false) != 0;
							if (flag2)
							{
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE SalesTable SET SaleCustomer = '",
									addCustomer.txtCustName.Text,
									"' WHERE SaleCustomer = '",
									this.RunCustName.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Detail = replace(Detail, '",
									this.RunCustName.Text,
									"', '",
									addCustomer.txtCustName.Text,
									"') WHERE Detail LIKE 'Payment PKR%",
									this.RunCustName.Text,
									"%'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Detail = replace(Detail, '",
									this.RunCustName.Text,
									"', '",
									addCustomer.txtCustName.Text,
									"') WHERE Detail LIKE 'Sale Payment%",
									this.RunCustName.Text,
									"%'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE PaymentsPkrTable SET PaidTo = '",
									addCustomer.txtCustName.Text,
									"' WHERE Paidto = '",
									this.RunCustName.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
							}
							sqliteTransaction.Commit();
						}
						int selectedIndex = this.LstCust.SelectedIndex;
						this.controlupdater();
						this.LstCust.SelectedIndex = selectedIndex;
						this.LstCust.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Customer: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000251A File Offset: 0x0000091A
		private void BtnDeleteCust_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000EFB0 File Offset: 0x0000D3B0
		private void BtnOpenData_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			Microsoft.Win32.OpenFileDialog openFileDialog2 = openFileDialog;
			openFileDialog2.Filter = "BusinessNetworkDatabase files|; *.bndb";
			openFileDialog2.Title = "Open a BNDB file";
			openFileDialog2.FileName = "";
			try
			{
				Microsoft.Win32.OpenFileDialog openFileDialog3 = openFileDialog;
				bool? flag = openFileDialog3.ShowDialog();
				bool valueOrDefault = ((flag != null) ? new bool?(flag.GetValueOrDefault()) : null).GetValueOrDefault();
				if (valueOrDefault)
				{
					this.FileName = openFileDialog3.FileName;
					MainWindow.StrDataFile = this.FileName;
					MySettingsProperty.Settings.StrDataFile = MainWindow.StrDataFile;
					MySettingsProperty.Settings.Save();
					this.openData();
				}
			}
			catch (SQLiteException ex)
			{
				Interaction.MsgBox(ex.Message + "\r\n" + ex.ToString(), MsgBoxStyle.OkOnly, null);
				MainWindow.errorlog("During Opening Database: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000030BC File Offset: 0x000014BC
		private void txtAccountFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			MainWindow.DSet.Tables["AccountTable"].DefaultView.RowFilter = "AccountName LIKE '*" + this.txtAccountFilter.Text + "*'";
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000F0CC File Offset: 0x0000D4CC
		private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.LstAccounts.SelectedIndex != -1;
			if (flag)
			{
				DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Delete Selected Account From Database?", "Alert", MessageBoxButton.YesNo);
				bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
				if (flag2)
				{
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							try
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.AddObject(Operators.AddObject("DELETE FROM AccountTable WHERE AccountName ='", NewLateBinding.LateIndexGet(this.LstAccounts.SelectedItem, new object[]
								{
									"AccountName"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
							}
							catch (SQLiteException ex)
							{
								MainWindow.errorlog("Deleting Account Table Row: " + ex.Message + "\r\n" + ex.ToString());
								Interaction.MsgBox(ex.Message + "\r\n" + ex.ToString(), MsgBoxStyle.OkOnly, null);
							}
							sqliteTransaction.Commit();
						}
						this.controlupdater();
					}
				}
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000F230 File Offset: 0x0000D630
		private void LstAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.LstAccounts.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditAccount_Click(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000F264 File Offset: 0x0000D664
		private void LstStocks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.LstStocks.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditStock_Click(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000F298 File Offset: 0x0000D698
		private void btnSaleEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool flag = Operators.CompareString(this.comboChassis.Text, string.Empty, false) == 0;
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
								DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Sale Entry?", "Alert", MessageBoxButton.YesNo);
								bool flag5 = dialogResult == System.Windows.Forms.DialogResult.Yes;
								if (flag5)
								{
									using (SQLiteCommand sqliteCommand = new SQLiteCommand())
									{
										using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
										{
											SQLiteCommand sqliteCommand2 = sqliteCommand;
											sqliteCommand2.Connection = MainWindow.Connection;
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"INSERT INTO SalesTable(SaleDate, SaleChassis, SaleCustomer, SalePrice, SaleAmountReceived, PaymentReceivedIn) VALUES ('",
												this.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
												"', '",
												this.comboChassis.Text,
												"', '",
												this.comboCust.Text,
												"', '",
												this.txtSalePrice.Text,
												"', '",
												this.txtAmountReceived.Text,
												"', '",
												this.comboSaleAccounts.Text,
												"');"
											});
											sqliteCommand2.ExecuteNonQuery();
											sqliteCommand2.CommandText = "UPDATE StocksTable SET Status = 'Sold' WHERE Chassis='" + this.comboChassis.Text + "'";
											sqliteCommand2.ExecuteNonQuery();
											bool flag6 = Operators.CompareString(this.txtAmountReceived.Text, string.Empty, false) != 0;
											if (flag6)
											{
												double value = Conversions.ToDouble(this.txtAmountReceived.Text);
												sqliteCommand2.CommandText = string.Concat(new string[]
												{
													"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
													this.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
													"', ",
													Conversions.ToString(value),
													", 'Sale Payment Received Chassis: ",
													this.comboChassis.Text,
													" from: ",
													this.comboCust.Text,
													"', '",
													this.comboSaleAccounts.Text,
													"');"
												});
												sqliteCommand2.ExecuteNonQuery();
												sqliteCommand2.CommandText = string.Concat(new string[]
												{
													"UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ",
													Conversions.ToString(value),
													", PaymentReceivable = PaymentReceivable + ",
													this.txtSalePrice.Text,
													" - ",
													Conversions.ToString(value),
													" WHERE Name = '",
													this.comboCust.Text,
													"'"
												});
												sqliteCommand2.ExecuteNonQuery();
												sqliteCommand2.CommandText = string.Concat(new string[]
												{
													"INSERT INTO ReceiptsTable(receiptDate, receiptAmount, receiptDetail, receivedIn, receivedFrom) VALUES ('",
													this.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
													"', ",
													Conversions.ToString(value),
													", 'Auto Sold Chasssis No.: ",
													this.comboChassis.Text,
													"', '",
													this.comboSaleAccounts.Text,
													"', '",
													this.comboCust.Text,
													"');"
												});
												sqliteCommand2.ExecuteNonQuery();
												sqliteCommand2.CommandText = string.Concat(new string[]
												{
													"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
													Conversions.ToString(value),
													" WHERE AccountName ='",
													this.comboSaleAccounts.Text,
													"'"
												});
												sqliteCommand2.ExecuteNonQuery();
											}
											else
											{
												sqliteCommand2.CommandText = string.Concat(new string[]
												{
													"UPDATE CustomersTable SET PaymentReceivable = PaymentReceivable + ",
													this.txtSalePrice.Text,
													" WHERE Name = '",
													this.comboCust.Text,
													"'"
												});
												sqliteCommand2.ExecuteNonQuery();
											}
											sqliteTransaction.Commit();
										}
										this.controlupdater();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Customer: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000F7D8 File Offset: 0x0000DBD8
		private void btnMiscExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
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
								DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Misc Exxpense Entry?", "Alert", MessageBoxButton.YesNo);
								bool flag5 = dialogResult == System.Windows.Forms.DialogResult.Yes;
								if (flag5)
								{
									using (SQLiteCommand sqliteCommand = new SQLiteCommand())
									{
										using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
										{
											SQLiteCommand sqliteCommand2 = sqliteCommand;
											sqliteCommand2.Connection = MainWindow.Connection;
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"INSERT INTO MiscExpTable(chassis, MiscExpDate, MiscExpAmount, MiscExpDetail, MiscExpPaidBy) VALUES ('",
												this.comboMiscChassis.Text,
												"', '",
												this.MiscdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
												"', ",
												this.txtMiscExpAmount.Text,
												", '",
												this.txtMiscExpDetail.Text,
												"', '",
												this.comboMiscAccounts.Text,
												"');"
											});
											sqliteCommand2.ExecuteNonQuery();
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
												this.MiscdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
												"', ",
												Conversions.ToString(checked(MainWindow.convertInteger(this.txtMiscExpAmount.Text) * -1)),
												", 'MiscExp: ",
												this.txtMiscExpDetail.Text,
												" (chassis): ",
												this.comboMiscChassis.Text,
												"', '",
												this.comboMiscAccounts.Text,
												"');"
											});
											sqliteCommand2.ExecuteNonQuery();
											double value = Conversions.ToDouble(this.txtMiscExpAmount.Text);
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"UPDATE StocksTable SET MiscExpense = MiscExpense + ",
												Conversions.ToString(value),
												" WHERE Chassis='",
												this.comboMiscChassis.Text,
												"'"
											});
											sqliteCommand2.ExecuteNonQuery();
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
												Conversions.ToString(value),
												" WHERE AccountName ='",
												this.comboMiscAccounts.Text,
												"'"
											});
											sqliteCommand2.ExecuteNonQuery();
											sqliteTransaction.Commit();
										}
										this.controlupdater();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Misc Expense: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000FBB4 File Offset: 0x0000DFB4
		private void btnOfficeExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool flag = Operators.CompareString(this.comboOfficeAccounts.Text, string.Empty, false) == 0;
				if (flag)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag2 = MainWindow.convertInteger(this.txtOfficeExpAmount.Text) < 0;
					if (flag2)
					{
						Interaction.MsgBox("Negative Value Not Allowed", MsgBoxStyle.Exclamation, null);
					}
					else
					{
						bool flag3 = Operators.CompareString(this.txtOfficeExpAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtOfficeExpDetail.Text, string.Empty, false) != 0;
						if (flag3)
						{
							DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Office Exxpense Entry?", "Alert", MessageBoxButton.YesNo);
							bool flag4 = dialogResult == System.Windows.Forms.DialogResult.Yes;
							if (flag4)
							{
								using (SQLiteCommand sqliteCommand = new SQLiteCommand())
								{
									using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
									{
										SQLiteCommand sqliteCommand2 = sqliteCommand;
										sqliteCommand2.Connection = MainWindow.Connection;
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO OfficeExpTable(OfficeExpDate, OfficeExpAmount, OfficeExpDetail, OfficeExpPaidBy) VALUES ('",
											this.OfficedatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											this.txtOfficeExpAmount.Text,
											", '",
											this.txtOfficeExpDetail.Text,
											"', '",
											this.comboOfficeAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
											this.OfficedatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											Conversions.ToString(Conversions.ToDouble(this.txtOfficeExpAmount.Text) * -1.0),
											", 'Office Expense: ",
											this.txtOfficeExpDetail.Text,
											"', '",
											this.comboOfficeAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										double value = Conversions.ToDouble(this.txtOfficeExpAmount.Text);
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
											Conversions.ToString(value),
											" WHERE AccountName ='",
											this.comboOfficeAccounts.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteTransaction.Commit();
									}
									this.controlupdater();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Office Expense: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000FEEC File Offset: 0x0000E2EC
		private void btnReceiptEntry_Click(object sender, RoutedEventArgs e)
		{
			try
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
							DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Receipt Entry?", "Alert", MessageBoxButton.YesNo);
							bool flag4 = dialogResult == System.Windows.Forms.DialogResult.Yes;
							if (flag4)
							{
								using (SQLiteCommand sqliteCommand = new SQLiteCommand())
								{
									using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
									{
										SQLiteCommand sqliteCommand2 = sqliteCommand;
										sqliteCommand2.Connection = MainWindow.Connection;
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO ReceiptsTable(receiptDate, receiptAmount, receiptDetail, receivedIn, receivedFrom) VALUES ('",
											this.ReceiptdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											this.txtReceiptAmount.Text,
											", '",
											this.txtReceiptDetail.Text,
											"', '",
											this.comboReceiptAccounts.Text,
											"', '",
											this.comboReceiptCust.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
											this.ReceiptdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											this.txtReceiptAmount.Text,
											", 'Payment Received: ",
											this.txtReceiptDetail.Text,
											" from: ",
											this.comboReceiptCust.Text,
											"', '",
											this.comboReceiptAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										double value = Conversions.ToDouble(this.txtReceiptAmount.Text);
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
											Conversions.ToString(value),
											" WHERE AccountName ='",
											this.comboReceiptAccounts.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ",
											Conversions.ToString(value),
											", PaymentReceivable = PaymentReceivable - ",
											Conversions.ToString(value),
											" WHERE Name = '",
											this.comboReceiptCust.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteTransaction.Commit();
									}
									this.controlupdater();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Receipt: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000102A4 File Offset: 0x0000E6A4
		private void btnPaymentEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool flag = Operators.CompareString(this.comboPaymentAccounts.Text, string.Empty, false) == 0;
				if (flag)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag2 = Operators.CompareString(this.txtPaymentAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtPaymentDetail.Text, string.Empty, false) != 0;
					if (flag2)
					{
						DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Payment Entry?", "Alert", MessageBoxButton.YesNo);
						bool flag3 = dialogResult == System.Windows.Forms.DialogResult.Yes;
						if (flag3)
						{
							using (SQLiteCommand sqliteCommand = new SQLiteCommand())
							{
								using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
								{
									SQLiteCommand sqliteCommand2 = sqliteCommand;
									sqliteCommand2.Connection = MainWindow.Connection;
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"INSERT INTO PaymentsTable(PaymentDate, PaymentAmountYen, PaymentExcRate, PaymentAmountPkr, PaymentDetail, PaidFrom) VALUES('",
										this.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
										"', '",
										this.txtPaymentAmount.Text,
										"', '",
										this.txtPaymentRate.Text,
										"', '",
										this.txtPaymentAmountPkr.Text,
										"', '",
										this.txtPaymentDetail.Text,
										" REF-Multiple', '",
										this.comboPaymentAccounts.Text,
										"');"
									});
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
										this.PaymentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
										"', ",
										Conversions.ToString(Conversions.ToDouble(this.txtPaymentAmountPkr.Text) * -1.0),
										", 'Payment: ",
										this.txtPaymentDetail.Text,
										"', '",
										this.comboPaymentAccounts.Text,
										"');"
									});
									sqliteCommand2.ExecuteNonQuery();
									bool flag4 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["PayableYenTable"].Compute("Sum(Amount)", "Status IS NULL")));
									if (flag4)
									{
										DataRow[] array = MainWindow.DSet.Tables["PayableYenTable"].Select("Status IS NULL");
										int num = Conversions.ToInteger(this.txtPaymentAmount.Text);
										foreach (DataRow dataRow in array)
										{
											bool flag5 = num != 0;
											if (!flag5)
											{
												break;
											}
											bool flag6 = Operators.ConditionalCompareObjectGreater(dataRow["Amount"], num, false);
											if (flag6)
											{
												int value = Conversions.ToInteger(Operators.SubtractObject(Operators.MultiplyObject(num, dataRow["Rate"]), (double)num * Conversions.ToDouble(this.txtPaymentRate.Text)));
												sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject("UPDATE PayableYenTable SET Amount = Amount - " + Conversions.ToString(num) + " WHERE rowid = ", dataRow["rowid"]));
												sqliteCommand2.ExecuteNonQuery();
												sqliteCommand2.CommandText = "INSERT INTO ProfitTable(Amount) VALUES (" + Conversions.ToString(value) + ");";
												sqliteCommand2.ExecuteNonQuery();
												break;
											}
											int value2 = Conversions.ToInteger(Operators.SubtractObject(Operators.MultiplyObject(dataRow["Amount"], dataRow["Rate"]), Operators.MultiplyObject(dataRow["Amount"], this.txtPaymentRate.Text)));
											sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject("UPDATE PayableYenTable SET Status = 'Close' WHERE rowid = ", dataRow["rowid"]));
											sqliteCommand2.ExecuteNonQuery();
											sqliteCommand2.CommandText = "INSERT INTO ProfitTable(Amount) VALUES (" + Conversions.ToString(value2) + ");";
											sqliteCommand2.ExecuteNonQuery();
											num = Conversions.ToInteger(Operators.SubtractObject(num, dataRow["Amount"]));
										}
									}
									double value3 = Conversions.ToDouble(this.txtPaymentAmountPkr.Text);
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
										Conversions.ToString(value3),
										" WHERE AccountName ='",
										this.comboPaymentAccounts.Text,
										"'"
									});
									sqliteCommand2.ExecuteNonQuery();
									DataRow[] array3 = MainWindow.DSet.Tables["StocksTable"].Select("PriceYen > PaidYen");
									double num2 = Conversions.ToDouble(this.txtPaymentAmount.Text);
									foreach (DataRow dataRow2 in array3)
									{
										bool flag7 = num2 == 0.0;
										if (flag7)
										{
											break;
										}
										bool flag8 = true;
										double num3 = Conversions.ToDouble(dataRow2["PaidYen"]);
										double num4 = Conversions.ToDouble(Operators.SubtractObject(dataRow2["PriceYen"], num3));
										bool flag9 = num4 > num2;
										if (flag9)
										{
											num4 = num2;
											flag8 = false;
										}
										double num5 = num4 * Conversions.ToDouble(this.txtPaymentRate.Text);
										double num6 = Convert.ToDouble(decimal.Round(Conversions.ToDecimal(Operators.DivideObject(Operators.AddObject(dataRow2["PaidAmount"], num5), num3 + num4)), 4));
										bool flag10 = flag8;
										if (flag10)
										{
											sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE StocksTable Set Rate = " + Conversions.ToString(num6) + ", PricePkr = ", Operators.MultiplyObject(dataRow2["PriceYen"], num6)), ", PaidYen = PaidYen + "), num4), ", PaidAmount = PaidAmount + "), num5), ", Cost = Duty + MiscExpense + PaidAmount + "), num5), " WHERE rowid ='"), dataRow2["rowid"]), "'"));
										}
										else
										{
											sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE StocksTable Set Rate = " + Conversions.ToString(num6) + ", PricePkr = ", Operators.MultiplyObject(dataRow2["PriceYen"], num6)), ", PaidYen = PaidYen + "), num4), ", PaidAmount = PaidAmount + "), num5), " WHERE rowid ='"), dataRow2["rowid"]), "'"));
										}
										sqliteCommand2.ExecuteNonQuery();
										num2 -= num4;
									}
									sqliteTransaction.Commit();
								}
								this.controlupdater();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Payment Yen: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000030F8 File Offset: 0x000014F8
		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			MainWindow.Connection.Close();
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00003106 File Offset: 0x00001506
		private void txtFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			MainWindow.DSet.Tables["StocksTable"].DefaultView.RowFilter = "Chassis LIKE '*" + this.txtFilter.Text + "*'";
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000251A File Offset: 0x0000091A
		private void btnPrntSoldCars_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00010A74 File Offset: 0x0000EE74
		private void TabAccountsReceivable_GotFocus()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Templates\\SimpleReport 1.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Templates\\");
				reportDocument.ReportTitle = "Account Receivable";
				streamReader.Close();
				DataTable dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["CustomersTable"].Clone();
				DataRow[] array = MainWindow.DSet.Tables["CustomersTable"].Select("PaymentReceivable > 0");
				foreach (DataRow row in array)
				{
					dataTable.ImportRow(row);
				}
				ReportData reportData = new ReportData();
				reportData.ReportDocumentValues.Add("PrintDate", DateTime.Now);
				reportData.DataTables.Add(dataTable);
				dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["AgentsTable"].Clone();
				array = MainWindow.DSet.Tables["AgentsTable"].Select("PaymentReceivable > 0");
				foreach (DataRow row2 in array)
				{
					dataTable.ImportRow(row2);
				}
				reportData.DataTables.Add(dataTable);
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(reportData);
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("Account Receivable: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00010C88 File Offset: 0x0000F088
		private void TabAccountsPayable_GotFocus()
		{
			checked
			{
				try
				{
					ReportDocument reportDocument = new ReportDocument();
					StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Templates\\SimpleReport 1.xaml", FileMode.Open, FileAccess.Read));
					reportDocument.XamlData = streamReader.ReadToEnd();
					reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Templates\\");
					reportDocument.ReportTitle = "Account Payable";
					streamReader.Close();
					DataTable dataTable = new DataTable();
					dataTable = MainWindow.DSet.Tables["CustomersTable"].Clone();
					DataRow[] array = MainWindow.DSet.Tables["CustomersTable"].Select("PaymentReceivable < 0");
					foreach (DataRow row in array)
					{
						dataTable.ImportRow(row);
					}
					int num = dataTable.Rows.Count - 1;
					for (int j = 0; j <= num; j++)
					{
						DataRow dataRow;
						(dataRow = dataTable.Rows[j])["PaymentReceivable"] = Operators.MultiplyObject(dataRow["PaymentReceivable"], -1);
					}
					ReportData reportData = new ReportData();
					reportData.ReportDocumentValues.Add("PrintDate", DateTime.Now);
					reportData.DataTables.Add(dataTable);
					dataTable = MainWindow.DSet.Tables["AgentsTable"].Clone();
					array = MainWindow.DSet.Tables["AgentsTable"].Select("PaymentReceivable < 0");
					foreach (DataRow row2 in array)
					{
						dataTable.ImportRow(row2);
					}
					int num2 = dataTable.Rows.Count - 1;
					for (int l = 0; l <= num2; l++)
					{
						DataRow dataRow;
						(dataRow = dataTable.Rows[l])["PaymentReceivable"] = Operators.MultiplyObject(dataRow["PaymentReceivable"], -1);
					}
					reportData.DataTables.Add(dataTable);
					XpsDocument xpsDocument = reportDocument.CreateXpsDocument(reportData);
					this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
					this.documentViewer2.Visibility = Visibility.Visible;
					this.flowviewertrial.Visibility = Visibility.Collapsed;
				}
				catch (Exception ex)
				{
					MainWindow.errorlog("Account Payable: " + ex.Message + "\r\n" + ex.ToString());
				}
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00010F38 File Offset: 0x0000F338
		private void TabTrialBalance_GotFocus()
		{
			try
			{
				this.closingProfit = 0.0;
				string left = MySettingsProperty.Settings.strClosing;
				bool flag = Operators.CompareString(left, string.Empty, false) == 0;
				if (flag)
				{
					left = Conversions.ToString(DateTime.Today);
				}
				this.runT.Text = "Trial Balance";
				this.runD.Text = "Date: " + DateTime.Today.ToString("yyyy-MM-dd");
				this.TableTrialBalance.RowGroups.Clear();
				double num = 0.0;
				double num2 = 0.0;
				TableRow tableRow = new TableRow();
				Paragraph paragraph = new Paragraph();
				Paragraph paragraph2 = new Paragraph();
				Paragraph paragraph3 = new Paragraph();
				TableCell item = new TableCell();
				TableCell item2 = new TableCell();
				TableCell item3 = new TableCell();
				double num3 = 0.0;
				this.TableTrialBalance.RowGroups.Insert(0, new TableRowGroup());
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph3 = new Paragraph();
				paragraph.Inlines.Add("");
				paragraph3.Inlines.Add("Credit");
				paragraph2.Inlines.Add("Debit");
				this.TableTrialBalance.RowGroups.Insert(0, new TableRowGroup());
				this.TableTrialBalance.RowGroups[0].FontSize = Conversions.ToDouble("14");
				this.TableTrialBalance.RowGroups[0].FontWeight = FontWeights.DemiBold;
				this.TableTrialBalance.RowGroups[0].Background = Brushes.LightGray;
				this.TableTrialBalance.RowGroups[0].Rows.Add(tableRow);
				item = new TableCell(paragraph);
				item2 = new TableCell(paragraph2);
				item3 = new TableCell(paragraph3);
				tableRow.Cells.Add(item);
				tableRow.Cells.Add(item2);
				tableRow.Cells.Add(item3);
				this.TableTrialBalance.RowGroups.Insert(1, new TableRowGroup());
				try
				{
					foreach (object obj in MainWindow.DSet.Tables["AccountTable"].Rows)
					{
						DataRow dataRow = (DataRow)obj;
						tableRow = new TableRow();
						paragraph = new Paragraph();
						paragraph2 = new Paragraph();
						paragraph3 = new Paragraph();
						paragraph2.TextAlignment = TextAlignment.Right;
						paragraph3.TextAlignment = TextAlignment.Right;
						NewLateBinding.LateCall(paragraph.Inlines, null, "Add", new object[]
						{
							Operators.ConcatenateObject(Operators.ConcatenateObject(dataRow["AccountType"], ": "), dataRow["AccountName"])
						}, null, null, null, true);
						paragraph2.Inlines.Add(Convert.ToDouble(RuntimeHelpers.GetObjectValue(dataRow["CurrentBalance"])).ToString("N", CultureInfo.CurrentCulture));
						num = Conversions.ToDouble(Operators.AddObject(num, dataRow["CurrentBalance"]));
						paragraph3.Inlines.Add("");
						this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
						item = new TableCell(paragraph);
						item2 = new TableCell(paragraph2);
						item3 = new TableCell(paragraph3);
						tableRow.Cells.Add(item);
						tableRow.Cells.Add(item2);
						tableRow.Cells.Add(item3);
					}
				}
				finally
				{
					IEnumerator enumerator;
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
				bool flag2 = MainWindow.DSet.Tables["AccountTable"].Rows.Count > 0;
				if (flag2)
				{
					tableRow = new TableRow();
					paragraph = new Paragraph();
					paragraph2 = new Paragraph();
					paragraph3 = new Paragraph();
					paragraph2.TextAlignment = TextAlignment.Right;
					paragraph3.TextAlignment = TextAlignment.Right;
					paragraph.Inlines.Add("Capital");
					double num4 = Conversions.ToDouble(MainWindow.DSet.Tables["AccountTable"].Compute("Sum(OpeningBalance)", ""));
					paragraph3.Inlines.Add(num4.ToString("N", CultureInfo.CurrentCulture));
					num2 += num4;
					paragraph2.Inlines.Add("");
					this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
					item = new TableCell(paragraph);
					item2 = new TableCell(paragraph2);
					item3 = new TableCell(paragraph3);
					tableRow.Cells.Add(item);
					tableRow.Cells.Add(item2);
					tableRow.Cells.Add(item3);
				}
				bool flag3 = MainWindow.DSet.Tables["StocksTable"].Rows.Count > 0 | MainWindow.DSet.Tables["PaymentsTable"].Rows.Count > 0;
				if (flag3)
				{
					tableRow = new TableRow();
					paragraph = new Paragraph();
					paragraph2 = new Paragraph();
					paragraph3 = new Paragraph();
					paragraph2.TextAlignment = TextAlignment.Right;
					paragraph3.TextAlignment = TextAlignment.Right;
					paragraph.Inlines.Add("Stock IN HAND");
					int num5 = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(Cost) - Sum(Duty) - Sum(MiscExpense)", "Status IS NULL")));
					paragraph2.Inlines.Add(num5.ToString("N", CultureInfo.CurrentCulture));
					num += (double)num5;
					paragraph3.Inlines.Add("");
					this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
					item = new TableCell(paragraph);
					item2 = new TableCell(paragraph2);
					item3 = new TableCell(paragraph3);
					tableRow.Cells.Add(item);
					tableRow.Cells.Add(item2);
					tableRow.Cells.Add(item3);
				}
				bool flag4 = MainWindow.DSet.Tables["OfficeExpTable"].Rows.Count > 0;
				double ptr;
				if (flag4)
				{
					string str = string.Empty;
					bool flag5 = Operators.CompareString(MySettingsProperty.Settings.strclosingOfficeRow, string.Empty, false) != 0;
					if (flag5)
					{
						str = " And rowid > " + MySettingsProperty.Settings.strclosingOfficeRow;
					}
					double num6 = (double)MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["OfficeExpTable"].Compute("Sum(OfficeExpAmount)", "OfficeExpAmount > 0" + str)));
					ptr = ref this.closingProfit;
					this.closingProfit = ptr - num6;
					DataRow[] array = MainWindow.DSet.Tables["OfficeExpTable"].Select("OfficeExpAmount > 0" + str);
					foreach (DataRow dataRow2 in array)
					{
						tableRow = new TableRow();
						paragraph = new Paragraph();
						paragraph2 = new Paragraph();
						paragraph3 = new Paragraph();
						paragraph2.TextAlignment = TextAlignment.Right;
						paragraph3.TextAlignment = TextAlignment.Right;
						DataRow dataRow3;
						object[] array3;
						bool[] array4;
						NewLateBinding.LateCall(paragraph.Inlines, null, "Add", array3 = new object[]
						{
							(dataRow3 = dataRow2)["OfficeExpDetail"]
						}, null, null, array4 = new bool[]
						{
							true
						}, true);
						if (array4[0])
						{
							dataRow3["OfficeExpDetail"] = RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(array3[0]));
						}
						double num7 = Conversions.ToDouble(dataRow2["OfficeExpAmount"]);
						paragraph2.Inlines.Add(num7.ToString("N", CultureInfo.CurrentCulture));
						num += num7;
						paragraph3.Inlines.Add("");
						this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
						item = new TableCell(paragraph);
						item2 = new TableCell(paragraph2);
						item3 = new TableCell(paragraph3);
						tableRow.Cells.Add(item);
						tableRow.Cells.Add(item2);
						tableRow.Cells.Add(item3);
					}
				}
				string filter = string.Empty;
				bool flag6 = Operators.CompareString(MySettingsProperty.Settings.strclosingmiscexp, string.Empty, false) != 0;
				if (flag6)
				{
					filter = "rowid > " + MySettingsProperty.Settings.strclosingmiscexp;
				}
				double num8 = (double)MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["MiscExpTable"].Compute("Sum(MiscExpAmount)", filter)));
				num3 -= num8;
				ptr = ref this.closingProfit;
				this.closingProfit = ptr - num8;
				string filter2 = string.Empty;
				bool flag7 = Operators.CompareString(MySettingsProperty.Settings.strclosingDutyExp, string.Empty, false) != 0;
				if (flag7)
				{
					filter2 = "rowid > " + MySettingsProperty.Settings.strclosingDutyExp;
				}
				double num9 = (double)MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["DutyExpTable"].Compute("Sum(DutyExpAmount)", filter2)));
				num3 -= num9;
				ptr = ref this.closingProfit;
				this.closingProfit = ptr - num9;
				bool flag8 = MainWindow.DSet.Tables["CustomersTable"].Rows.Count > 0;
				double num11;
				double num13;
				if (flag8)
				{
					bool flag9 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["CustomersTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable > 0")));
					if (flag9)
					{
						tableRow = new TableRow();
						paragraph = new Paragraph();
						paragraph2 = new Paragraph();
						paragraph3 = new Paragraph();
						paragraph2.TextAlignment = TextAlignment.Right;
						paragraph3.TextAlignment = TextAlignment.Right;
						double num10 = Conversions.ToDouble(MainWindow.DSet.Tables["CustomersTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable > 0"));
						num11 += num10;
						paragraph.Inlines.Add("Accounts Receivable");
						paragraph2.Inlines.Add(num10.ToString("N", CultureInfo.CurrentCulture));
						num += num10;
						paragraph3.Inlines.Add("");
						DataRow[] array5 = MainWindow.DSet.Tables["CustomersTable"].Select("PaymentReceivable > 0");
						foreach (DataRow dataRow4 in array5)
						{
							NewLateBinding.LateCall(paragraph.Inlines, null, "Add", new object[]
							{
								Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("\r\n\t", dataRow4["Name"]), ": "), "\t"), dataRow4["PaymentReceivable"])
							}, null, null, null, true);
						}
						this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
						item = new TableCell(paragraph);
						item2 = new TableCell(paragraph2);
						item3 = new TableCell(paragraph3);
						tableRow.Cells.Add(item);
						tableRow.Cells.Add(item2);
						tableRow.Cells.Add(item3);
					}
					bool flag10 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["CustomersTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable < 0")));
					if (flag10)
					{
						double num12 = Conversions.ToDouble(MainWindow.DSet.Tables["CustomersTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable < 0"));
						num13 += num12;
						bool flag11 = num12 != 0.0;
						if (flag11)
						{
							tableRow = new TableRow();
							paragraph = new Paragraph();
							paragraph2 = new Paragraph();
							paragraph3 = new Paragraph();
							paragraph2.TextAlignment = TextAlignment.Right;
							paragraph3.TextAlignment = TextAlignment.Right;
							paragraph.Inlines.Add("Accounts Payable (Parties)");
							double num14 = num12 * -1.0;
							paragraph3.Inlines.Add(num14.ToString("N", CultureInfo.CurrentCulture));
							num2 += num12 * -1.0;
							paragraph2.Inlines.Add("");
							DataRow[] array7 = MainWindow.DSet.Tables["CustomersTable"].Select("PaymentReceivable < 0");
							foreach (DataRow dataRow5 in array7)
							{
								NewLateBinding.LateCall(paragraph.Inlines, null, "Add", new object[]
								{
									Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("\r\n\t", dataRow5["Name"]), ": "), "\t"), Operators.MultiplyObject(dataRow5["PaymentReceivable"], -1))
								}, null, null, null, true);
							}
							this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
							item = new TableCell(paragraph);
							item2 = new TableCell(paragraph2);
							item3 = new TableCell(paragraph3);
							tableRow.Cells.Add(item);
							tableRow.Cells.Add(item2);
							tableRow.Cells.Add(item3);
						}
					}
				}
				bool flag12 = MainWindow.DSet.Tables["AgentsTable"].Rows.Count > 0;
				if (flag12)
				{
					bool flag13 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["AgentsTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable > 0")));
					if (flag13)
					{
						tableRow = new TableRow();
						paragraph = new Paragraph();
						paragraph2 = new Paragraph();
						paragraph3 = new Paragraph();
						paragraph2.TextAlignment = TextAlignment.Right;
						paragraph3.TextAlignment = TextAlignment.Right;
						double num15 = Conversions.ToDouble(MainWindow.DSet.Tables["AgentsTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable > 0"));
						num11 += num15;
						paragraph.Inlines.Add("Accounts Receivable (Agents)");
						paragraph2.Inlines.Add(num15.ToString("N", CultureInfo.CurrentCulture));
						num += num15;
						paragraph3.Inlines.Add("");
						DataRow[] array9 = MainWindow.DSet.Tables["AgentsTable"].Select("PaymentReceivable > 0");
						foreach (DataRow dataRow6 in array9)
						{
							NewLateBinding.LateCall(paragraph.Inlines, null, "Add", new object[]
							{
								Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("\r\n\t", dataRow6["Name"]), ": "), "\t"), dataRow6["PaymentReceivable"])
							}, null, null, null, true);
						}
						this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
						item = new TableCell(paragraph);
						item2 = new TableCell(paragraph2);
						item3 = new TableCell(paragraph3);
						tableRow.Cells.Add(item);
						tableRow.Cells.Add(item2);
						tableRow.Cells.Add(item3);
					}
					bool flag14 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["AgentsTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable < 0")));
					if (flag14)
					{
						double num16 = Conversions.ToDouble(MainWindow.DSet.Tables["AgentsTable"].Compute("Sum(PaymentReceivable)", "PaymentReceivable < 0"));
						num13 += num16;
						bool flag15 = num16 != 0.0;
						if (flag15)
						{
							tableRow = new TableRow();
							paragraph = new Paragraph();
							paragraph2 = new Paragraph();
							paragraph3 = new Paragraph();
							paragraph2.TextAlignment = TextAlignment.Right;
							paragraph3.TextAlignment = TextAlignment.Right;
							paragraph.Inlines.Add("Accounts Payable (Agents)");
							double num17 = num16 * -1.0;
							paragraph3.Inlines.Add(num17.ToString("N", CultureInfo.CurrentCulture));
							num2 += num16 * -1.0;
							paragraph2.Inlines.Add("");
							DataRow[] array11 = MainWindow.DSet.Tables["AgentsTable"].Select("PaymentReceivable < 0");
							foreach (DataRow dataRow7 in array11)
							{
								NewLateBinding.LateCall(paragraph.Inlines, null, "Add", new object[]
								{
									Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("\r\n\t", dataRow7["Name"]), ": "), "\t"), Operators.MultiplyObject(dataRow7["PaymentReceivable"], -1))
								}, null, null, null, true);
							}
							this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
							item = new TableCell(paragraph);
							item2 = new TableCell(paragraph2);
							item3 = new TableCell(paragraph3);
							tableRow.Cells.Add(item);
							tableRow.Cells.Add(item2);
							tableRow.Cells.Add(item3);
						}
					}
				}
				bool flag16 = MainWindow.DSet.Tables["SalesTable"].Rows.Count > 0;
				if (flag16)
				{
					string text = string.Empty;
					bool flag17 = Operators.CompareString(MySettingsProperty.Settings.strclosingSalesRow, string.Empty, false) != 0;
					if (flag17)
					{
						text = "rowid > " + MySettingsProperty.Settings.strclosingSalesRow;
					}
					double num18 = (double)MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["SalesTable"].Compute("Sum(SalePrice)", text)));
					num3 += num18;
					ptr = ref this.closingProfit;
					this.closingProfit = ptr + num18;
					bool flag18 = Operators.CompareString(text, string.Empty, false) != 0;
					if (flag18)
					{
						text = " AND " + text;
					}
					bool flag19 = MainWindow.DSet.Tables["SalesTable"].Rows.Count > 0;
					double num24;
					checked
					{
						if (flag19)
						{
							bool flag20 = !this.boolCopied;
							if (flag20)
							{
								int num19 = MainWindow.DSet.Tables["StocksTable"].Columns.Count - 1;
								for (int n = 2; n <= num19; n++)
								{
									MainWindow.DSet.Tables["SalesTable"].Columns.Add(MainWindow.DSet.Tables["StocksTable"].Columns[n].ColumnName, MainWindow.DSet.Tables["StocksTable"].Columns[n].DataType);
								}
								this.boolCopied = true;
							}
							string str2 = string.Empty;
							try
							{
								foreach (object obj2 in MainWindow.DSet.Tables["SalesTable"].Columns)
								{
									DataColumn dataColumn = (DataColumn)obj2;
									str2 = str2 + ", " + dataColumn.ColumnName;
								}
							}
							finally
							{
								IEnumerator enumerator2;
								if (enumerator2 is IDisposable)
								{
									(enumerator2 as IDisposable).Dispose();
								}
							}
							int num20 = MainWindow.DSet.Tables["SalesTable"].Rows.Count - 1;
							for (int num21 = 0; num21 <= num20; num21++)
							{
								DataRow[] array13 = MainWindow.DSet.Tables["StocksTable"].Select("Chassis = '" + MainWindow.DSet.Tables["SalesTable"].Rows[num21]["SaleChassis"].ToString() + "'");
								int num22 = MainWindow.DSet.Tables["StocksTable"].Columns.Count - 1;
								for (int num23 = 2; num23 <= num22; num23++)
								{
									MainWindow.DSet.Tables["SalesTable"].Rows[num21][MainWindow.DSet.Tables["StocksTable"].Columns[num23].ColumnName] = RuntimeHelpers.GetObjectValue(array13[0][MainWindow.DSet.Tables["StocksTable"].Columns[num23]]);
								}
							}
						}
						num24 = (double)MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["SalesTable"].Compute("Sum(Cost) - Sum(Duty) - Sum(MiscExpense)", "Status = 'Sold'" + text)));
					}
					num3 -= num24;
					ptr = ref this.closingProfit;
					this.closingProfit = ptr - num24;
				}
				bool flag21 = MainWindow.DSet.Tables["StocksTable"].Rows.Count > 0 | MainWindow.DSet.Tables["PaymentsTable"].Rows.Count > 0;
				if (flag21)
				{
					tableRow = new TableRow();
					paragraph = new Paragraph();
					paragraph2 = new Paragraph();
					paragraph3 = new Paragraph();
					paragraph2.TextAlignment = TextAlignment.Right;
					paragraph3.TextAlignment = TextAlignment.Right;
					paragraph.Inlines.Add("Accounts Payable YEN (converted to PKR)");
					bool flag22 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountYen)", "")));
					int num25;
					int num26;
					if (flag22)
					{
						num25 = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountYen)", "")));
						num26 = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountPkr)", "")));
					}
					bool flag23 = !Information.IsDBNull(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PriceYen)", "")));
					int num28;
					checked
					{
						int num27;
						if (flag23)
						{
							this.payableYens = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PriceYen)", ""))) - num25;
							num27 = MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PaidAmount)", "")));
						}
						bool flag24 = this.payableYens < 0;
						if (flag24)
						{
							num28 = (num26 - num27) * -1;
						}
						else
						{
							num28 = (int)Math.Round((double)(unchecked((float)this.payableYens * MySettingsProperty.Settings.exchangeRate)));
						}
					}
					num13 += (double)num28;
					paragraph3.Inlines.Add(num28.ToString("N", CultureInfo.CurrentCulture));
					num2 += (double)num28;
					paragraph2.Inlines.Add("");
					this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
					item = new TableCell(paragraph);
					item2 = new TableCell(paragraph2);
					item3 = new TableCell(paragraph3);
					tableRow.Cells.Add(item);
					tableRow.Cells.Add(item2);
					tableRow.Cells.Add(item3);
				}
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph3 = new Paragraph();
				paragraph2.TextAlignment = TextAlignment.Right;
				paragraph3.TextAlignment = TextAlignment.Right;
				paragraph.Inlines.Add("Profit/Loss Account Balance");
				double num29 = 0.0;
				bool flag25 = MainWindow.DSet.Tables["ProfitTable"].Rows.Count > 0;
				if (flag25)
				{
					num29 = Conversions.ToDouble(MainWindow.DSet.Tables["ProfitTable"].Compute("SUM(Amount)", ""));
				}
				paragraph3.Inlines.Add(num29.ToString("N", CultureInfo.CurrentCulture));
				num13 += num29;
				num2 += num29;
				paragraph2.Inlines.Add("");
				this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
				item = new TableCell(paragraph);
				item2 = new TableCell(paragraph2);
				item3 = new TableCell(paragraph3);
				tableRow.Cells.Add(item);
				tableRow.Cells.Add(item2);
				tableRow.Cells.Add(item3);
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph3 = new Paragraph();
				paragraph2.TextAlignment = TextAlignment.Right;
				paragraph3.TextAlignment = TextAlignment.Right;
				paragraph.Inlines.Add("Profit On Current Sales");
				paragraph3.Inlines.Add(num3.ToString("N", CultureInfo.CurrentCulture));
				num2 += num3;
				paragraph2.Inlines.Add("");
				this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
				item = new TableCell(paragraph);
				item2 = new TableCell(paragraph2);
				item3 = new TableCell(paragraph3);
				tableRow.Cells.Add(item);
				tableRow.Cells.Add(item2);
				tableRow.Cells.Add(item3);
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph3 = new Paragraph();
				paragraph.Inlines.Add("Total");
				paragraph3.Inlines.Add(num2.ToString("N", CultureInfo.CurrentCulture));
				paragraph2.Inlines.Add(num.ToString("N", CultureInfo.CurrentCulture));
				this.TableTrialBalance.RowGroups.Insert(2, new TableRowGroup());
				this.TableTrialBalance.RowGroups[2].FontSize = Conversions.ToDouble("14");
				this.TableTrialBalance.RowGroups[2].FontWeight = FontWeights.DemiBold;
				this.TableTrialBalance.RowGroups[2].Background = Brushes.LightGray;
				this.TableTrialBalance.RowGroups[2].Rows.Add(tableRow);
				item = new TableCell(paragraph);
				item2 = new TableCell(paragraph2);
				item3 = new TableCell(paragraph3);
				tableRow.Cells.Add(item);
				tableRow.Cells.Add(item2);
				tableRow.Cells.Add(item3);
				this.documentViewer2.Visibility = Visibility.Collapsed;
				this.flowviewertrial.Visibility = Visibility.Visible;
				this.btnClosing.Visibility = Visibility.Visible;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("Trial Balance Creation: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00012BB8 File Offset: 0x00010FB8
		public static int convertInteger(object intInteger)
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

		// Token: 0x06000210 RID: 528 RVA: 0x00012BE4 File Offset: 0x00010FE4
		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
			bool valueOrDefault = printDialog.ShowDialog().GetValueOrDefault();
			if (valueOrDefault)
			{
				MemoryStream stream = new MemoryStream();
				TextRange textRange = new TextRange(this.flowTrialBalance.ContentStart, this.flowTrialBalance.ContentEnd);
				textRange.Save(stream, System.Windows.DataFormats.Xaml);
				FlowDocument flowDocument = new FlowDocument();
				flowDocument.PageHeight = this.flowTrialBalance.PageHeight;
				flowDocument.PageWidth = this.flowTrialBalance.PageWidth;
				flowDocument.ColumnWidth = this.flowTrialBalance.ColumnWidth;
				flowDocument.PagePadding = this.flowTrialBalance.PagePadding;
				TextRange textRange2 = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
				textRange2.Load(stream, System.Windows.DataFormats.Xaml);
				DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
				printDialog.PrintDocument(documentPaginator, "Trial Balance");
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00012CD0 File Offset: 0x000110D0
		private void LstCust_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.LstCust.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditCust_Click(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00003142 File Offset: 0x00001542
		private void txtCustFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			MainWindow.DSet.Tables["CustomersTable"].DefaultView.RowFilter = "Name LIKE '*" + this.txtCustFilter.Text + "*'";
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00012D04 File Offset: 0x00011104
		private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.comboReport.Text, "Sold Cars", false) == 0;
			if (flag)
			{
				this.Sales_Report();
			}
			else
			{
				bool flag2 = Operators.CompareString(this.comboReport.Text, "Trial Balance", false) == 0;
				if (flag2)
				{
					this.TabTrialBalance_GotFocus();
				}
				else
				{
					bool flag3 = Operators.CompareString(this.comboReport.Text, "Income Statement", false) == 0;
					if (flag3)
					{
						this.IncomeStatement();
					}
					else
					{
						bool flag4 = Operators.CompareString(this.comboReport.Text, "Accounts Receivable", false) == 0;
						if (flag4)
						{
							this.TabAccountsReceivable_GotFocus();
						}
						else
						{
							bool flag5 = Operators.CompareString(this.comboReport.Text, "Accounts Payable", false) == 0;
							if (flag5)
							{
								this.TabAccountsPayable_GotFocus();
							}
							else
							{
								bool flag6 = Operators.CompareString(this.comboReport.Text, "Stocks", false) == 0;
								if (flag6)
								{
									this.Stocks_Report();
								}
								else
								{
									bool flag7 = Operators.CompareString(this.comboReport.Text, "Office Expenses", false) == 0;
									if (flag7)
									{
										this.OfficeExpense_Report();
									}
									else
									{
										bool flag8 = Operators.CompareString(this.comboReport.Text, "Duty Expenses", false) == 0;
										if (flag8)
										{
											this.DutyExpense_Report();
										}
										else
										{
											bool flag9 = Operators.CompareString(this.comboReport.Text, "Misc. Auto Expenses", false) == 0;
											if (flag9)
											{
												this.MiscAutoExpense_Report();
											}
											else
											{
												bool flag10 = Operators.CompareString(this.comboReport.Text, "Yen Payments", false) == 0;
												if (flag10)
												{
													this.Payments_Report();
												}
												else
												{
													bool flag11 = Operators.CompareString(this.comboReport.Text, "Party Ledger", false) == 0;
													if (flag11)
													{
														this.PartyPayments_Report();
													}
													else
													{
														bool flag12 = Operators.CompareString(this.comboReport.Text, "Receipts", false) == 0;
														if (flag12)
														{
															this.Receipts_Report();
														}
														else
														{
															bool flag13 = Operators.CompareString(this.comboReport.Text, "Accounts", false) == 0;
															if (flag13)
															{
																this.Accounts_Report();
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00012F38 File Offset: 0x00011338
		private void DutyExpense_Report()
		{
			try
			{
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = new StreamReader(new FileStream(MyWpfExtension.Application.Info.DirectoryPath + "\\Resources\\SimpleReportDutyExpenses.xaml", FileMode.Open, FileAccess.Read));
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Resources\\");
				streamReader.Close();
				string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
				string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
				DataTable dataTable = new DataTable();
				dataTable = MainWindow.DSet.Tables["DutyExpTable"].Clone();
				bool flag = Operators.CompareString(this.comboReportDutyAgents.Text, "All", false) == 0;
				DataRow[] array;
				if (flag)
				{
					dataTable.Clear();
					array = MainWindow.DSet.Tables["DutyExpTable"].Select(string.Concat(new string[]
					{
						"DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					reportDocument.ReportTitle = "All Agents";
				}
				else
				{
					dataTable.Clear();
					dataTable = MainWindow.DSet.Tables["DutyExpTable"].Clone();
					array = MainWindow.DSet.Tables["DutyExpTable"].Select(string.Concat(new string[]
					{
						"DutyExpAgent = '",
						this.comboReportDutyAgents.Text,
						"' AND DATE >='",
						text,
						"' AND DATE <='",
						text2,
						"'"
					}));
					reportDocument.ReportTitle = "Agent: " + this.comboReportDutyAgents.Text;
				}
				foreach (DataRow row in array)
				{
					dataTable.ImportRow(row);
				}
				XpsDocument xpsDocument = reportDocument.CreateXpsDocument(new ReportData
				{
					ReportDocumentValues = 
					{
						{
							"PrintDate",
							DateTime.Now
						}
					},
					DataTables = 
					{
						dataTable
					}
				});
				this.documentViewer2.Document = xpsDocument.GetFixedDocumentSequence();
				this.documentViewer2.Visibility = Visibility.Visible;
				this.flowviewertrial.Visibility = Visibility.Collapsed;
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("During Duty Exp Report: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0001320C File Offset: 0x0001160C
		private void IncomeStatement()
		{
			string text = this.reportDateFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
			string text2 = this.reportDateTo.SelectedDate.Value.ToString("yyyy-MM-dd");
			string text3 = string.Concat(new string[]
			{
				"DATE >='",
				text,
				"' AND DATE <='",
				text2,
				"'"
			});
			double num = 0.0;
			TableRow tableRow = new TableRow();
			Paragraph paragraph = new Paragraph();
			Paragraph paragraph2 = new Paragraph();
			TableCell tableCell = new TableCell();
			TableCell tableCell2 = new TableCell();
			this.runT.Text = "Income Statement";
			this.runD.Text = "From: " + text + " To: " + text2;
			this.TableTrialBalance.RowGroups.Clear();
			tableRow = new TableRow();
			paragraph = new Paragraph();
			paragraph2 = new Paragraph();
			paragraph.Inlines.Add("");
			paragraph2.Inlines.Add("Amount");
			this.TableTrialBalance.RowGroups.Insert(0, new TableRowGroup());
			this.TableTrialBalance.RowGroups[0].FontSize = Conversions.ToDouble("14");
			this.TableTrialBalance.RowGroups[0].FontWeight = FontWeights.DemiBold;
			this.TableTrialBalance.RowGroups[0].Background = Brushes.LightGray;
			this.TableTrialBalance.RowGroups[0].Rows.Add(tableRow);
			tableCell = new TableCell(paragraph);
			tableCell2 = new TableCell(paragraph2);
			tableRow.Cells.Add(tableCell);
			tableRow.Cells.Add(tableCell2);
			this.TableTrialBalance.RowGroups.Insert(1, new TableRowGroup());
			bool flag = MainWindow.DSet.Tables["SalesTable"].Rows.Count > 0;
			if (flag)
			{
				bool flag2 = !this.boolCopied;
				double num5;
				int num6;
				checked
				{
					if (flag2)
					{
						int num2 = MainWindow.DSet.Tables["StocksTable"].Columns.Count - 1;
						for (int i = 2; i <= num2; i++)
						{
							MainWindow.DSet.Tables["SalesTable"].Columns.Add(MainWindow.DSet.Tables["StocksTable"].Columns[i].ColumnName, MainWindow.DSet.Tables["StocksTable"].Columns[i].DataType);
						}
						this.boolCopied = true;
					}
					string str = string.Empty;
					try
					{
						foreach (object obj in MainWindow.DSet.Tables["SalesTable"].Columns)
						{
							DataColumn dataColumn = (DataColumn)obj;
							str = str + ", " + dataColumn.ColumnName;
						}
					}
					finally
					{
						IEnumerator enumerator;
						if (enumerator is IDisposable)
						{
							(enumerator as IDisposable).Dispose();
						}
					}
					int num3 = MainWindow.DSet.Tables["SalesTable"].Rows.Count - 1;
					for (int j = 0; j <= num3; j++)
					{
						DataRow[] array = MainWindow.DSet.Tables["StocksTable"].Select("Chassis = '" + MainWindow.DSet.Tables["SalesTable"].Rows[j]["SaleChassis"].ToString() + "'");
						int num4 = MainWindow.DSet.Tables["StocksTable"].Columns.Count - 1;
						for (int k = 2; k <= num4; k++)
						{
							MainWindow.DSet.Tables["SalesTable"].Rows[j][MainWindow.DSet.Tables["StocksTable"].Columns[k].ColumnName] = RuntimeHelpers.GetObjectValue(array[0][MainWindow.DSet.Tables["StocksTable"].Columns[k]]);
						}
					}
					string filter = string.Concat(new string[]
					{
						"SaleDate >='",
						text,
						"' AND SaleDate <='",
						text2,
						"'"
					});
					num5 = Conversions.ToDouble(MainWindow.DSet.Tables["SalesTable"].Compute("Sum(SalePrice)", filter));
					num6 = Conversions.ToInteger(Operators.MultiplyObject(Operators.AddObject(Operators.MultiplyObject(Operators.SubtractObject(MainWindow.DSet.Tables["SalesTable"].Compute("Sum(PriceYen)", filter), MainWindow.DSet.Tables["SalesTable"].Compute("Sum(PaidYen)", filter)), MySettingsProperty.Settings.exchangeRate), MainWindow.DSet.Tables["SalesTable"].Compute("Sum(PaidAmount)", filter)), -1));
				}
				num += num5;
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph.Inlines.Add("Sales");
				paragraph2.Inlines.Add(num5.ToString("N", CultureInfo.CurrentCulture));
				this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
				tableCell = new TableCell(paragraph);
				tableCell2 = new TableCell(paragraph2);
				tableCell2.TextAlignment = TextAlignment.Right;
				tableRow.Cells.Add(tableCell);
				tableRow.Cells.Add(tableCell2);
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph.Inlines.Add("Estimated Purchase Price Conversion of Sold Cars");
				paragraph2.Inlines.Add(num6.ToString("N", CultureInfo.CurrentCulture));
				num += (double)num6;
				this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
				tableCell = new TableCell(paragraph);
				tableCell.TextAlignment = TextAlignment.Left;
				tableCell2 = new TableCell(paragraph2);
				tableCell2.TextAlignment = TextAlignment.Right;
				tableRow.Cells.Add(tableCell);
				tableRow.Cells.Add(tableCell2);
				bool flag3 = MainWindow.DSet.Tables["MiscExpTable"].Rows.Count > 0;
				if (flag3)
				{
					tableRow = new TableRow();
					paragraph = new Paragraph();
					paragraph2 = new Paragraph();
					paragraph.Inlines.Add("Misc. Auto Expenses");
					int num7 = Conversions.ToInteger(Operators.MultiplyObject(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(MiscExpense)", text3), -1));
					num += (double)num7;
					paragraph2.Inlines.Add(num7.ToString("N", CultureInfo.CurrentCulture));
					this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
					tableCell = new TableCell(paragraph);
					tableCell2 = new TableCell(paragraph2);
					tableCell2.TextAlignment = TextAlignment.Right;
					tableRow.Cells.Add(tableCell);
					tableRow.Cells.Add(tableCell2);
				}
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph.Inlines.Add("Duty Paid");
				int num8 = Conversions.ToInteger(Operators.MultiplyObject(MainWindow.DSet.Tables["StocksTable"].Compute("Sum(Duty)", text3), -1));
				num += (double)num8;
				paragraph2.Inlines.Add(num8.ToString("N", CultureInfo.CurrentCulture));
				this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
				tableCell = new TableCell(paragraph);
				tableCell2 = new TableCell(paragraph2);
				tableCell2.TextAlignment = TextAlignment.Right;
				tableRow.Cells.Add(tableCell);
				tableRow.Cells.Add(tableCell2);
				bool flag4 = MainWindow.DSet.Tables["OfficeExpTable"].Rows.Count > 0;
				if (flag4)
				{
					tableRow = new TableRow();
					paragraph = new Paragraph();
					paragraph2 = new Paragraph();
					paragraph.Inlines.Add("Office Expenses");
					string filter2 = text3.Replace("DATE", "Date");
					double num9 = Conversions.ToDouble(Operators.MultiplyObject(MainWindow.DSet.Tables["OfficeExpTable"].Compute("Sum(OfficeExpAmount)", filter2), -1));
					num += num9;
					paragraph2.Inlines.Add(num9.ToString("N", CultureInfo.CurrentCulture));
					this.TableTrialBalance.RowGroups[1].Rows.Add(tableRow);
					tableCell = new TableCell(paragraph);
					tableCell2 = new TableCell(paragraph2);
					tableCell2.TextAlignment = TextAlignment.Right;
					tableRow.Cells.Add(tableCell);
					tableRow.Cells.Add(tableCell2);
				}
				tableRow = new TableRow();
				paragraph = new Paragraph();
				paragraph2 = new Paragraph();
				paragraph.Inlines.Add("Estimated Profit on Sales");
				paragraph2.Inlines.Add(num.ToString("N", CultureInfo.CurrentCulture));
				this.TableTrialBalance.RowGroups.Insert(2, new TableRowGroup());
				this.TableTrialBalance.RowGroups[2].FontSize = Conversions.ToDouble("14");
				this.TableTrialBalance.RowGroups[2].FontWeight = FontWeights.DemiBold;
				this.TableTrialBalance.RowGroups[2].Background = Brushes.LightGray;
				this.TableTrialBalance.RowGroups[2].Rows.Add(tableRow);
				tableCell = new TableCell(paragraph);
				tableCell.TextAlignment = TextAlignment.Left;
				tableCell2 = new TableCell(paragraph2);
				tableCell2.TextAlignment = TextAlignment.Right;
				tableRow.Cells.Add(tableCell);
				tableRow.Cells.Add(tableCell2);
			}
			this.documentViewer2.Visibility = Visibility.Collapsed;
			this.flowviewertrial.Visibility = Visibility.Visible;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00013CF0 File Offset: 0x000120F0
		private void comboReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.comboReport.SelectedIndex == 1;
			if (flag)
			{
				this.comboReportAccounts.Visibility = Visibility.Visible;
				this.reportDate.Visibility = Visibility.Visible;
				this.comboReportDutyAgents.Visibility = Visibility.Collapsed;
				this.comboReportCustomers.Visibility = Visibility.Collapsed;
				this.comboReportStockDuty.Visibility = Visibility.Collapsed;
			}
			else
			{
				bool flag2 = this.comboReport.SelectedIndex == 2;
				if (flag2)
				{
					this.comboReportDutyAgents.Visibility = Visibility.Collapsed;
					this.comboReportAccounts.Visibility = Visibility.Collapsed;
					this.reportDate.Visibility = Visibility.Collapsed;
					this.comboReportCustomers.Visibility = Visibility.Collapsed;
					this.comboReportStockDuty.Visibility = Visibility.Visible;
				}
				else
				{
					bool flag3 = this.comboReport.SelectedIndex == 9;
					if (flag3)
					{
						this.comboReportDutyAgents.Visibility = Visibility.Visible;
						this.comboReportAccounts.Visibility = Visibility.Collapsed;
						this.reportDate.Visibility = Visibility.Collapsed;
						this.comboReportCustomers.Visibility = Visibility.Collapsed;
						this.comboReportStockDuty.Visibility = Visibility.Collapsed;
					}
					else
					{
						bool flag4 = this.comboReport.SelectedIndex == 10 | this.comboReport.SelectedIndex == 11;
						if (flag4)
						{
							this.comboReportDutyAgents.Visibility = Visibility.Collapsed;
							this.comboReportAccounts.Visibility = Visibility.Collapsed;
							this.reportDate.Visibility = Visibility.Collapsed;
							this.comboReportCustomers.Visibility = Visibility.Visible;
							this.comboReportStockDuty.Visibility = Visibility.Collapsed;
						}
						else
						{
							bool flag5 = this.comboReportAccounts != null;
							if (flag5)
							{
								this.comboReportAccounts.Visibility = Visibility.Collapsed;
								this.comboReportDutyAgents.Visibility = Visibility.Collapsed;
								this.reportDate.Visibility = Visibility.Collapsed;
								this.comboReportCustomers.Visibility = Visibility.Collapsed;
								this.comboReportStockDuty.Visibility = Visibility.Collapsed;
							}
						}
					}
				}
			}
			bool flag6 = this.comboReport.SelectedIndex < 3 || this.comboReport.SelectedIndex > 5;
			if (flag6)
			{
				bool flag7 = this.reportDate != null;
				if (flag7)
				{
					this.reportDate.Visibility = Visibility.Visible;
				}
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000317E File Offset: 0x0000157E
		private void hyplnk2_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("http://hostineasy.com");
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00013F0C File Offset: 0x0001230C
		private void btnDutyExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
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
								DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Duty Expense Entry?", "Alert", MessageBoxButton.YesNo);
								bool flag5 = dialogResult == System.Windows.Forms.DialogResult.Yes;
								if (flag5)
								{
									using (SQLiteCommand sqliteCommand = new SQLiteCommand())
									{
										using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
										{
											SQLiteCommand sqliteCommand2 = sqliteCommand;
											sqliteCommand2.Connection = MainWindow.Connection;
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"INSERT INTO DutyExpTable(chassis, DutyExpDate, DutyExpAmount, DutyExpDetail, DutyExpAgent) VALUES ('",
												this.comboDutyChassis.Text,
												"', '",
												this.DutydatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
												"', ",
												this.txtDutyExpAmount.Text,
												", '",
												this.txtDutyExpDetail.Text,
												"', '",
												this.comboDutyAgents.Text,
												"');"
											});
											sqliteCommand2.ExecuteNonQuery();
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable - ",
												this.txtDutyExpAmount.Text,
												" WHERE Name = '",
												this.comboDutyAgents.Text,
												"'"
											});
											sqliteCommand2.ExecuteNonQuery();
											double value = Conversions.ToDouble(this.txtDutyExpAmount.Text);
											sqliteCommand2.CommandText = string.Concat(new string[]
											{
												"UPDATE StocksTable SET Duty = Duty + ",
												Conversions.ToString(value),
												" WHERE Chassis='",
												this.comboDutyChassis.Text,
												"'"
											});
											sqliteCommand2.ExecuteNonQuery();
											sqliteTransaction.Commit();
										}
										this.controlupdater();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Duty Expense: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00014238 File Offset: 0x00012638
		private void btnDutyAddAgent_Click(object sender, RoutedEventArgs e)
		{
			string text = Interaction.InputBox("Please Enter a Unique Agent Name", "Create New Agent", "", -1, -1);
			bool flag = Operators.CompareString(text.Trim(), string.Empty, false) != 0;
			if (flag)
			{
				using (SQLiteCommand sqliteCommand = new SQLiteCommand())
				{
					using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
					{
						SQLiteCommand sqliteCommand2 = sqliteCommand;
						sqliteCommand2.Connection = MainWindow.Connection;
						sqliteCommand2.CommandText = "INSERT INTO AgentsTable(Name) VALUES ('" + text.Trim() + "');";
						sqliteCommand2.ExecuteNonQuery();
						sqliteTransaction.Commit();
					}
					this.controlupdater();
				}
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0001430C File Offset: 0x0001270C
		private void LstBlogs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridSales.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridSales.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditSaleEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00014358 File Offset: 0x00012758
		private void DataGridSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridSales.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditSaleEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditSaleEntry.IsEnabled = false;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0001439C File Offset: 0x0001279C
		private void btnEditSaleEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaleAuto saleAuto = new SaleAuto();
				SaleAuto saleAuto2 = saleAuto;
				saleAuto2.Title = "Edit Sales Entry";
				saleAuto2.AccInfoText.Text = "Edit Sales Entry";
				saleAuto2.comboChassis.Visibility = Visibility.Collapsed;
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
				{
					"SaleDate"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				saleAuto2.SaleDatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				System.Windows.Controls.Label lblchassis;
				(lblchassis = saleAuto2.lblchassis).Content = Operators.AddObject(lblchassis.Content, NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
				{
					"SaleChassis"
				}, null));
				saleAuto2.txtSalePrice.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
				{
					"SalePrice"
				}, null));
				saleAuto2.txtAmountReceived.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
				{
					"SaleAmountReceived"
				}, null));
				saleAuto2.comboCust.ItemsSource = this.comboCust.ItemsSource;
				saleAuto2.comboCust.DisplayMemberPath = this.comboCust.DisplayMemberPath;
				saleAuto2.comboCust.SelectedValuePath = this.comboCust.SelectedValuePath;
				saleAuto2.comboCust.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
				{
					"SaleCustomer"
				}, null));
				saleAuto2.comboSaleAccounts.ItemsSource = this.comboSaleAccounts.ItemsSource;
				saleAuto2.comboSaleAccounts.DisplayMemberPath = this.comboSaleAccounts.DisplayMemberPath;
				saleAuto2.comboSaleAccounts.SelectedValuePath = this.comboSaleAccounts.SelectedValuePath;
				saleAuto2.comboSaleAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
				{
					"PaymentReceivedIn"
				}, null));
				saleAuto.ShowDialog();
				bool flag = saleAuto.DialogResult != null && saleAuto.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Sale Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE SalesTable SET SaleDate = '",
									saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', SaleCustomer = '",
									saleAuto.comboCust.Text,
									"', SalePrice =",
									saleAuto.txtSalePrice.Text,
									", SaleAmountReceived =",
									saleAuto.txtAmountReceived.Text,
									", PaymentReceivedIn ='",
									saleAuto.comboSaleAccounts.Text,
									"' WHERE SaleChassis='"
								}), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
								{
									"SaleChassis"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								double num = Conversions.ToDouble(saleAuto.txtAmountReceived.Text);
								bool flag3 = Operators.ConditionalCompareObjectNotEqual(saleAuto.comboCust.Text, NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
								{
									"SaleCustomer"
								}, null), false);
								if (flag3)
								{
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ",
										Conversions.ToString(num),
										", PaymentReceivable = PaymentReceivable + ",
										saleAuto.txtSalePrice.Text,
										" - ",
										Conversions.ToString(num),
										" WHERE Name = '",
										saleAuto.comboCust.Text,
										"'"
									});
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE CustomersTable SET PaymentReceived = PaymentReceived - ", NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null)), ", PaymentReceivable = PaymentReceivable - "), Operators.SubtractObject(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SalePrice"
									}, null), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null))), " WHERE Name = '"), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleCustomer"
									}, null)), "'"));
									sqliteCommand2.ExecuteNonQuery();
								}
								else
								{
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ", Operators.SubtractObject(num, NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null))), ", PaymentReceivable = PaymentReceivable + "), saleAuto.txtSalePrice.Text), " - "), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SalePrice"
									}, null)), " - "), num), " + "), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null)), " WHERE Name = '"), saleAuto.comboCust.Text), "'"));
									sqliteCommand2.ExecuteNonQuery();
								}
								bool flag4 = Operators.ConditionalCompareObjectEqual(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
								{
									"SaleAmountReceived"
								}, null), 0, false) && Operators.ConditionalCompareObjectNotEqual(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
								{
									"SaleAmountReceived"
								}, null), saleAuto.txtAmountReceived.Text, false);
								if (flag4)
								{
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
									{
										"INSERT INTO ReceiptsTable(receiptDate, receiptAmount, receiptDetail, receivedIn, receivedFrom) VALUES ('",
										saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
										"', ",
										Conversions.ToString(num),
										", 'Auto Sold Chasssis No.: "
									}), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleChassis"
									}, null)), "', '"), saleAuto.comboSaleAccounts.Text), "', '"), saleAuto.comboCust.Text), "');"));
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
									{
										"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
										saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
										"', ",
										Conversions.ToString(num),
										", 'Sale Payment Received Chassis: "
									}), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleChassis"
									}, null)), " from: "), saleAuto.comboCust.Text), "', '"), saleAuto.comboSaleAccounts.Text), "');"));
									sqliteCommand2.ExecuteNonQuery();
								}
								else
								{
									bool flag5 = Operators.ConditionalCompareObjectNotEqual(NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null), 0, false);
									if (flag5)
									{
										sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
										{
											"UPDATE ReceiptsTable SET receiptDate = '",
											saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', receiptAmount =",
											Conversions.ToString(num),
											", receiptDetail ='Auto Sold Chasssis No.: "
										}), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
										{
											"SaleChassis"
										}, null)), "', receivedIn = '"), saleAuto.comboSaleAccounts.Text), "', receivedFrom ='"), saleAuto.comboCust.Text), "' WHERE rowid IN (SELECT rowid FROM ReceiptsTable WHERE receiptDetail LIKE '%Auto Sold Chasssis No.: "), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
										{
											"SaleChassis"
										}, null)), "%' LIMIT 1)"));
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
										{
											"UPDATE LedgerTable SET Date ='",
											saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', Amount = ",
											Conversions.ToString(num),
											", Detail = 'Sale Payment Received Chassis: "
										}), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
										{
											"SaleChassis"
										}, null)), " from: "), saleAuto.comboCust.Text), "', Account = '"), saleAuto.comboSaleAccounts.Text), "' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Detail LIKE '%Sale Payment Received Chassis: "), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
										{
											"SaleChassis"
										}, null)), "%' LIMIT 1)"));
										sqliteCommand2.ExecuteNonQuery();
									}
								}
								bool flag6 = Operators.ConditionalCompareObjectNotEqual(saleAuto.comboSaleAccounts.Text, NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
								{
									"PaymentReceivedIn"
								}, null), false);
								if (flag6)
								{
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
										Conversions.ToString(num),
										" WHERE AccountName ='",
										saleAuto.comboSaleAccounts.Text,
										"'"
									});
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance - ", NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"PaymentReceivedIn"
									}, null)), "'"));
									sqliteCommand2.ExecuteNonQuery();
								}
								else
								{
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance + " + Conversions.ToString(num) + " - ", NewLateBinding.LateIndexGet(this.DataGridSales.SelectedItem, new object[]
									{
										"SaleAmountReceived"
									}, null)), " WHERE AccountName ='"), saleAuto.comboSaleAccounts.Text), "'"));
									sqliteCommand2.ExecuteNonQuery();
								}
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Sales Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00015004 File Offset: 0x00013404
		private void btnNewSaleEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaleAuto saleAuto = new SaleAuto();
				SaleAuto saleAuto2 = saleAuto;
				saleAuto2.comboChassis.ItemsSource = this.comboChassis.ItemsSource;
				saleAuto2.comboChassis.DisplayMemberPath = this.comboChassis.DisplayMemberPath;
				saleAuto2.comboChassis.SelectedValuePath = "Chassis";
				saleAuto2.comboChassis.SelectedIndex = 0;
				saleAuto2.comboCust.ItemsSource = this.comboCust.ItemsSource;
				saleAuto2.comboCust.DisplayMemberPath = this.comboCust.DisplayMemberPath;
				saleAuto2.comboCust.SelectedValuePath = this.comboCust.SelectedValuePath;
				saleAuto2.comboCust.SelectedIndex = this.comboCust.SelectedIndex;
				saleAuto2.comboSaleAccounts.ItemsSource = this.comboSaleAccounts.ItemsSource;
				saleAuto2.comboSaleAccounts.DisplayMemberPath = this.comboSaleAccounts.DisplayMemberPath;
				saleAuto2.comboSaleAccounts.SelectedValuePath = this.comboSaleAccounts.SelectedValuePath;
				saleAuto2.comboSaleAccounts.SelectedIndex = this.comboSaleAccounts.SelectedIndex;
				saleAuto.ShowDialog();
				bool flag = saleAuto.DialogResult != null && saleAuto.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Sale Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO SalesTable(SaleDate, SaleChassis, SaleCustomer, SalePrice, SaleAmountReceived, PaymentReceivedIn) VALUES ('",
									saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', '",
									saleAuto.comboChassis.Text,
									"', '",
									saleAuto.comboCust.Text,
									"', '",
									saleAuto.txtSalePrice.Text,
									"', '",
									saleAuto.txtAmountReceived.Text,
									"', '",
									saleAuto.comboSaleAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = "UPDATE StocksTable SET Status = 'Sold' WHERE Chassis='" + saleAuto.comboChassis.Text + "'";
								sqliteCommand2.ExecuteNonQuery();
								bool flag3 = Operators.CompareString(saleAuto.txtAmountReceived.Text, string.Empty, false) != 0;
								if (flag3)
								{
									double value = Conversions.ToDouble(saleAuto.txtAmountReceived.Text);
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
										saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
										"', ",
										Conversions.ToString(value),
										", 'Sale Payment Received Chassis: ",
										saleAuto.comboChassis.Text,
										" from: ",
										saleAuto.comboCust.Text,
										"', '",
										saleAuto.comboSaleAccounts.Text,
										"');"
									});
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ",
										Conversions.ToString(value),
										", PaymentReceivable = PaymentReceivable + ",
										saleAuto.txtSalePrice.Text,
										" - ",
										Conversions.ToString(value),
										" WHERE Name = '",
										saleAuto.comboCust.Text,
										"'"
									});
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"INSERT INTO ReceiptsTable(receiptDate, receiptAmount, receiptDetail, receivedIn, receivedFrom) VALUES ('",
										saleAuto.SaleDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
										"', ",
										Conversions.ToString(value),
										", 'Auto Sold Chasssis No.: ",
										saleAuto.comboChassis.Text,
										"', '",
										saleAuto.comboSaleAccounts.Text,
										"', '",
										saleAuto.comboCust.Text,
										"');"
									});
									sqliteCommand2.ExecuteNonQuery();
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
										Conversions.ToString(value),
										" WHERE AccountName ='",
										saleAuto.comboSaleAccounts.Text,
										"'"
									});
									sqliteCommand2.ExecuteNonQuery();
								}
								else
								{
									sqliteCommand2.CommandText = string.Concat(new string[]
									{
										"UPDATE CustomersTable SET PaymentReceivable = PaymentReceivable + ",
										saleAuto.txtSalePrice.Text,
										" WHERE Name = '",
										saleAuto.comboCust.Text,
										"'"
									});
									sqliteCommand2.ExecuteNonQuery();
								}
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Sales Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000155D4 File Offset: 0x000139D4
		private void btnPaymentPkrEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool flag = Operators.CompareString(this.comboPaymentPkrAccounts.Text, string.Empty, false) == 0;
				if (flag)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag2 = Operators.CompareString(this.comboPaymentPkrCust.Text, string.Empty, false) == 0;
					if (flag2)
					{
						Interaction.MsgBox("Please Select Customer to continue...", MsgBoxStyle.OkOnly, null);
					}
					else
					{
						bool flag3 = Operators.CompareString(this.txtPaymentPkrAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtPaymentPkrDetail.Text, string.Empty, false) != 0;
						if (flag3)
						{
							DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Payment Entry?", "Alert", MessageBoxButton.YesNo);
							bool flag4 = dialogResult == System.Windows.Forms.DialogResult.Yes;
							if (flag4)
							{
								using (SQLiteCommand sqliteCommand = new SQLiteCommand())
								{
									using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
									{
										SQLiteCommand sqliteCommand2 = sqliteCommand;
										sqliteCommand2.Connection = MainWindow.Connection;
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO PaymentsPkrTable(PaymentDate, PaymentAmount, PaymentDetail, PaidFrom, PaidTo) VALUES ('",
											this.PaymentPkrdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											this.txtPaymentPkrAmount.Text,
											", '",
											this.txtPaymentPkrDetail.Text,
											"', '",
											this.comboPaymentPkrAccounts.Text,
											"', '",
											this.comboPaymentPkrCust.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
											this.PaymentPkrdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											Conversions.ToString(Conversions.ToDouble(this.txtPaymentPkrAmount.Text) * -1.0),
											", 'Payment PKR Entry: ",
											this.txtPaymentPkrDetail.Text,
											" To: ",
											this.comboPaymentPkrCust.Text,
											"', '",
											this.comboPaymentPkrAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										double value = Conversions.ToDouble(this.txtPaymentPkrAmount.Text);
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
											Conversions.ToString(value),
											" WHERE AccountName ='",
											this.comboPaymentPkrAccounts.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE CustomersTable SET PaymentPaid = PaymentPaid + ",
											Conversions.ToString(value),
											", PaymentReceivable = PaymentReceivable + ",
											Conversions.ToString(value),
											" WHERE Name = '",
											this.comboPaymentPkrCust.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteTransaction.Commit();
									}
									this.controlupdater();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Payment Pkr: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000159A0 File Offset: 0x00013DA0
		private void btnNewReceiptEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ReceiptEntry receiptEntry = new ReceiptEntry();
				ReceiptEntry receiptEntry2 = receiptEntry;
				receiptEntry2.comboReceiptCust.ItemsSource = this.comboReceiptCust.ItemsSource;
				receiptEntry2.comboReceiptCust.DisplayMemberPath = this.comboReceiptCust.DisplayMemberPath;
				receiptEntry2.comboReceiptCust.SelectedValuePath = this.comboReceiptCust.SelectedValuePath;
				receiptEntry2.comboReceiptCust.SelectedIndex = this.comboReceiptCust.SelectedIndex;
				receiptEntry2.comboReceiptAccounts.ItemsSource = this.comboReceiptAccounts.ItemsSource;
				receiptEntry2.comboReceiptAccounts.DisplayMemberPath = this.comboReceiptAccounts.DisplayMemberPath;
				receiptEntry2.comboReceiptAccounts.SelectedValuePath = this.comboReceiptAccounts.SelectedValuePath;
				receiptEntry2.comboReceiptAccounts.SelectedIndex = this.comboReceiptAccounts.SelectedIndex;
				receiptEntry.ShowDialog();
				bool flag = receiptEntry.DialogResult != null && receiptEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Receipt Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO ReceiptsTable(receiptDate, receiptAmount, receiptDetail, receivedIn, receivedFrom) VALUES ('",
									receiptEntry.ReceiptdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									receiptEntry.txtReceiptAmount.Text,
									", '",
									receiptEntry.txtReceiptDetail.Text,
									"', '",
									receiptEntry.comboReceiptAccounts.Text,
									"', '",
									receiptEntry.comboReceiptCust.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
									receiptEntry.ReceiptdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									receiptEntry.txtReceiptAmount.Text,
									", 'Payment Received: ",
									receiptEntry.txtReceiptDetail.Text,
									" from: ",
									receiptEntry.comboReceiptCust.Text,
									"', '",
									receiptEntry.comboReceiptAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(receiptEntry.txtReceiptAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									receiptEntry.comboReceiptAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ",
									Conversions.ToString(value),
									", PaymentReceivable = PaymentReceivable - ",
									Conversions.ToString(value),
									" WHERE Name = '",
									receiptEntry.comboReceiptCust.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Receipt Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00015DB4 File Offset: 0x000141B4
		private void btnEditReceiptEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ReceiptEntry receiptEntry = new ReceiptEntry();
				ReceiptEntry receiptEntry2 = receiptEntry;
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
				{
					"DATE"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				receiptEntry2.ReceiptdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				receiptEntry2.comboReceiptCust.ItemsSource = this.comboReceiptCust.ItemsSource;
				receiptEntry2.comboReceiptCust.DisplayMemberPath = this.comboReceiptCust.DisplayMemberPath;
				receiptEntry2.comboReceiptCust.SelectedValuePath = this.comboReceiptCust.SelectedValuePath;
				receiptEntry2.comboReceiptCust.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
				{
					"receivedFrom"
				}, null));
				receiptEntry2.comboReceiptAccounts.ItemsSource = this.comboReceiptAccounts.ItemsSource;
				receiptEntry2.comboReceiptAccounts.DisplayMemberPath = this.comboReceiptAccounts.DisplayMemberPath;
				receiptEntry2.comboReceiptAccounts.SelectedValuePath = this.comboReceiptAccounts.SelectedValuePath;
				receiptEntry2.comboReceiptAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
				{
					"receivedIn"
				}, null));
				receiptEntry2.txtReceiptAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
				{
					"ReceiptAmount"
				}, null));
				receiptEntry2.txtReceiptDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
				{
					"ReceiptDetail"
				}, null));
				receiptEntry.ShowDialog();
				bool flag = receiptEntry.DialogResult != null && receiptEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Edit Receipt Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE ReceiptsTable SET receiptDate ='",
									receiptEntry.ReceiptdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', receiptAmount =",
									receiptEntry.txtReceiptAmount.Text,
									", receiptDetail ='",
									receiptEntry.txtReceiptDetail.Text,
									"', receivedIn ='",
									receiptEntry.comboReceiptAccounts.Text,
									"', receivedFrom ='",
									receiptEntry.comboReceiptCust.Text,
									"' WHERE rowid ="
								}), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									receiptEntry.ReceiptdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount =",
									receiptEntry.txtReceiptAmount.Text,
									", Detail ='Payment Received: ",
									receiptEntry.txtReceiptDetail.Text,
									" from: ",
									receiptEntry.comboReceiptCust.Text,
									"', Account='",
									receiptEntry.comboReceiptAccounts.Text,
									"' WHERE rowid in (SELECT rowid FROM LedgerTable WHERE Date ='"
								}), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"DATE"
								}, null)), "' AND Amount ="), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receiptAmount"
								}, null)), " AND Detail ='Payment Received: "), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receiptDetail"
								}, null)), " from: "), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receivedFrom"
								}, null)), "' AND Account='"), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receivedIn"
								}, null)), "' LIMIT 1)"));
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(receiptEntry.txtReceiptAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									receiptEntry.comboReceiptAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance - ", NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receiptAmount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receivedIn"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE CustomersTable SET PaymentReceived = PaymentReceived + ",
									Conversions.ToString(value),
									", PaymentReceivable = PaymentReceivable - ",
									Conversions.ToString(value),
									" WHERE Name = '",
									receiptEntry.comboReceiptCust.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE CustomersTable SET PaymentReceived = PaymentReceived - ", NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receiptAmount"
								}, null)), ", PaymentReceivable = PaymentReceivable + "), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receiptAmount"
								}, null)), " WHERE Name = '"), NewLateBinding.LateIndexGet(this.DataGridReceipts.SelectedItem, new object[]
								{
									"receivedFrom"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Receipt Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000164F8 File Offset: 0x000148F8
		private void DataGridReceipts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridReceipts.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridReceipts.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditReceiptEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00016544 File Offset: 0x00014944
		private void DataGridReceipts_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridReceipts.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditReceiptEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditReceiptEntry.IsEnabled = false;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00016588 File Offset: 0x00014988
		private void btnNewMiscExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MiscExpEntry miscExpEntry = new MiscExpEntry();
				MiscExpEntry miscExpEntry2 = miscExpEntry;
				miscExpEntry2.comboMiscChassis.ItemsSource = this.comboMiscChassis.ItemsSource;
				miscExpEntry2.comboMiscChassis.DisplayMemberPath = this.comboMiscChassis.DisplayMemberPath;
				miscExpEntry2.comboMiscChassis.SelectedValuePath = this.comboMiscChassis.SelectedValuePath;
				miscExpEntry2.comboMiscChassis.SelectedIndex = this.comboMiscChassis.SelectedIndex;
				miscExpEntry2.comboMiscAccounts.ItemsSource = this.comboMiscAccounts.ItemsSource;
				miscExpEntry2.comboMiscAccounts.DisplayMemberPath = this.comboMiscAccounts.DisplayMemberPath;
				miscExpEntry2.comboMiscAccounts.SelectedValuePath = this.comboMiscAccounts.SelectedValuePath;
				miscExpEntry2.comboMiscAccounts.SelectedIndex = this.comboMiscAccounts.SelectedIndex;
				miscExpEntry.ShowDialog();
				bool flag = miscExpEntry.DialogResult != null && miscExpEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Misc Expense Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO MiscExpTable(chassis, MiscExpDate, MiscExpAmount, MiscExpDetail, MiscExpPaidBy) VALUES ('",
									miscExpEntry.comboMiscChassis.Text,
									"', '",
									miscExpEntry.MiscdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									miscExpEntry.txtMiscExpAmount.Text,
									", '",
									miscExpEntry.txtMiscExpDetail.Text,
									"', '",
									miscExpEntry.comboMiscAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
									miscExpEntry.MiscdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									Conversions.ToString(checked(MainWindow.convertInteger(miscExpEntry.txtMiscExpAmount.Text) * -1)),
									", 'MiscExp: ",
									miscExpEntry.txtMiscExpDetail.Text,
									" (chassis): ",
									miscExpEntry.comboMiscChassis.Text,
									"', '",
									miscExpEntry.comboMiscAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(miscExpEntry.txtMiscExpAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE StocksTable SET MiscExpense = MiscExpense + ",
									Conversions.ToString(value),
									" WHERE Chassis='",
									miscExpEntry.comboMiscChassis.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									miscExpEntry.comboMiscAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Misc Entry 2972: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00016994 File Offset: 0x00014D94
		private void btnEditMiscExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MiscExpEntry miscExpEntry = new MiscExpEntry();
				MiscExpEntry miscExpEntry2 = miscExpEntry;
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
				{
					"DATE"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				miscExpEntry2.MiscdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				miscExpEntry2.comboMiscChassis.ItemsSource = this.comboMiscChassis.ItemsSource;
				miscExpEntry2.comboMiscChassis.DisplayMemberPath = this.comboMiscChassis.DisplayMemberPath;
				miscExpEntry2.comboMiscChassis.SelectedValuePath = this.comboMiscChassis.SelectedValuePath;
				miscExpEntry2.comboMiscChassis.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
				{
					"chassis"
				}, null));
				miscExpEntry2.comboMiscAccounts.ItemsSource = this.comboMiscAccounts.ItemsSource;
				miscExpEntry2.comboMiscAccounts.DisplayMemberPath = this.comboMiscAccounts.DisplayMemberPath;
				miscExpEntry2.comboMiscAccounts.SelectedValuePath = this.comboMiscAccounts.SelectedValuePath;
				miscExpEntry2.comboMiscAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
				{
					"MiscExpPaidBy"
				}, null));
				miscExpEntry2.txtMiscExpAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
				{
					"MiscExpAmount"
				}, null));
				miscExpEntry2.txtMiscExpDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
				{
					"MiscExpDetail"
				}, null));
				miscExpEntry2.AccInfoText.Text = "Edit Misc Expense Entry";
				miscExpEntry2.Title = "Edit Misc Expense Entry";
				miscExpEntry.ShowDialog();
				bool flag = miscExpEntry.DialogResult != null && miscExpEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Misc Expense Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE MiscExpTable SET chassis ='",
									miscExpEntry.comboMiscChassis.Text,
									"', MiscExpDate ='",
									miscExpEntry.MiscdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', MiscExpAmount =",
									miscExpEntry.txtMiscExpAmount.Text,
									", MiscExpDetail ='",
									miscExpEntry.txtMiscExpDetail.Text,
									"', MiscExpPaidBy ='",
									miscExpEntry.comboMiscAccounts.Text,
									"' WHERE rowid = "
								}), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(checked(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									miscExpEntry.MiscdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount =",
									Conversions.ToString(MainWindow.convertInteger(miscExpEntry.txtMiscExpAmount.Text) * -1),
									", Detail='MiscExp: ",
									miscExpEntry.txtMiscExpDetail.Text,
									" (chassis): ",
									miscExpEntry.comboMiscChassis.Text,
									"', Account ='",
									miscExpEntry.comboMiscAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Date ='"
								}), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"DATE"
								}, null)), "' AND Amount ="), MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"MiscExpAmount"
								}, null))) * -1)), " AND Detail='MiscExp: "), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"MiscExpDetail"
								}, null)), " (chassis): "), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"chassis"
								}, null)), "' AND Account ='"), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"MiscExpPaidBy"
								}, null)), "' LIMIT 1)"));
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(miscExpEntry.txtMiscExpAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE StocksTable SET MiscExpense = MiscExpense + ",
									Conversions.ToString(value),
									" WHERE Chassis='",
									miscExpEntry.comboMiscChassis.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE StocksTable SET MiscExpense = MiscExpense - ", NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"MiscExpAmount"
								}, null)), " WHERE Chassis='"), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"chassis"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									miscExpEntry.comboMiscAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance + ", NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"MiscExpAmount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridMiscExp.SelectedItem, new object[]
								{
									"MiscExpPaidBy"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Misc Entry : " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000170D4 File Offset: 0x000154D4
		private void DataGridMiscExp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridMiscExp.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridMiscExp.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditMiscExpEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00017120 File Offset: 0x00015520
		private void DataGridMiscExp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridMiscExp.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditMiscExpEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditMiscExpEntry.IsEnabled = false;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00017164 File Offset: 0x00015564
		private void btnNewDutyExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				DutyExpEntry dutyExpEntry = new DutyExpEntry();
				DutyExpEntry dutyExpEntry2 = dutyExpEntry;
				dutyExpEntry2.comboDutyChassis.ItemsSource = this.comboDutyChassis.ItemsSource;
				dutyExpEntry2.comboDutyChassis.DisplayMemberPath = this.comboDutyChassis.DisplayMemberPath;
				dutyExpEntry2.comboDutyChassis.SelectedValuePath = this.comboDutyChassis.SelectedValuePath;
				dutyExpEntry2.comboDutyChassis.SelectedIndex = this.comboDutyChassis.SelectedIndex;
				dutyExpEntry.ShowDialog();
				bool flag = dutyExpEntry.DialogResult != null && dutyExpEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Duty Expense Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO DutyExpTable(chassis, DutyExpDate, DutyExpAmount, DutyExpDetail, DutyExpAgent) VALUES ('",
									dutyExpEntry.comboDutyChassis.Text,
									"', '",
									dutyExpEntry.DutydatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									dutyExpEntry.txtDutyExpAmount.Text,
									", '",
									dutyExpEntry.txtDutyExpDetail.Text,
									"', '",
									dutyExpEntry.comboDutyAgents.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable - ",
									dutyExpEntry.txtDutyExpAmount.Text,
									" WHERE Name = '",
									dutyExpEntry.comboDutyAgents.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(dutyExpEntry.txtDutyExpAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE StocksTable SET Duty = Duty + ",
									Conversions.ToString(value),
									" WHERE Chassis='",
									dutyExpEntry.comboDutyChassis.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Duty Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00017464 File Offset: 0x00015864
		private void btnEditDutyExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				DutyExpEntry dutyExpEntry = new DutyExpEntry();
				DutyExpEntry dutyExpEntry2 = dutyExpEntry;
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
				{
					"DATE"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				dutyExpEntry2.DutydatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				dutyExpEntry2.comboDutyChassis.ItemsSource = this.comboDutyChassis.ItemsSource;
				dutyExpEntry2.comboDutyChassis.DisplayMemberPath = this.comboDutyChassis.DisplayMemberPath;
				dutyExpEntry2.comboDutyChassis.SelectedValuePath = this.comboDutyChassis.SelectedValuePath;
				dutyExpEntry2.comboDutyChassis.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
				{
					"chassis"
				}, null));
				dutyExpEntry2.txtDutyExpAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
				{
					"DutyExpAmount"
				}, null));
				dutyExpEntry2.txtDutyExpDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
				{
					"DutyExpDetail"
				}, null));
				dutyExpEntry2.AccInfoText.Text = "Edit Duty Expense Entry";
				dutyExpEntry2.Title = "Edit Duty Expense Entry";
				dutyExpEntry.ShowDialog();
				bool flag = dutyExpEntry.DialogResult != null && dutyExpEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Duty Expense Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE DutyExpTable SET chassis ='",
									dutyExpEntry.comboDutyChassis.Text,
									"', DutyExpDate ='",
									dutyExpEntry.DutydatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', DutyExpAmount =",
									dutyExpEntry.txtDutyExpAmount.Text,
									", DutyExpDetail ='",
									dutyExpEntry.txtDutyExpDetail.Text,
									"', DutyExpAgent ='",
									dutyExpEntry.comboDutyAgents.Text,
									"' WHERE rowid = "
								}), NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable - ",
									dutyExpEntry.txtDutyExpAmount.Text,
									" WHERE Name = '",
									dutyExpEntry.comboDutyAgents.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AgentsTable SET PaymentReceivable = PaymentReceivable + ", NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
								{
									"DutyExpAmount"
								}, null)), " WHERE Name = '"), NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
								{
									"DutyExpAgent"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(dutyExpEntry.txtDutyExpAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE StocksTable SET Duty = Duty + ",
									Conversions.ToString(value),
									" WHERE Chassis='",
									dutyExpEntry.comboDutyChassis.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE StocksTable SET Duty = Duty - ", NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
								{
									"DutyExpAmount"
								}, null)), " WHERE Chassis='"), NewLateBinding.LateIndexGet(this.DataGridDutyExp.SelectedItem, new object[]
								{
									"chassis"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Duty Entry : " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00017980 File Offset: 0x00015D80
		private void DataGridDutyExp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridDutyExp.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridDutyExp.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditDutyExpEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000179CC File Offset: 0x00015DCC
		private void DataGridDutyExp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridDutyExp.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditDutyExpEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditDutyExpEntry.IsEnabled = false;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00017A10 File Offset: 0x00015E10
		private void btnNewOfficeExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OfficeExpEntry officeExpEntry = new OfficeExpEntry();
				OfficeExpEntry officeExpEntry2 = officeExpEntry;
				officeExpEntry2.comboOfficeAccounts.ItemsSource = this.comboOfficeAccounts.ItemsSource;
				officeExpEntry2.comboOfficeAccounts.DisplayMemberPath = this.comboOfficeAccounts.DisplayMemberPath;
				officeExpEntry2.comboOfficeAccounts.SelectedValuePath = this.comboOfficeAccounts.SelectedValuePath;
				officeExpEntry2.comboOfficeAccounts.SelectedIndex = this.comboOfficeAccounts.SelectedIndex;
				officeExpEntry.ShowDialog();
				bool flag = officeExpEntry.DialogResult != null && officeExpEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Office Expense Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO OfficeExpTable(OfficeExpDate, OfficeExpAmount, OfficeExpDetail, OfficeExpPaidBy) VALUES ('",
									officeExpEntry.OfficedatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									officeExpEntry.txtOfficeExpAmount.Text,
									", '",
									officeExpEntry.txtOfficeExpDetail.Text,
									"', '",
									officeExpEntry.comboOfficeAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
									officeExpEntry.OfficedatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									Conversions.ToString(Conversions.ToDouble(officeExpEntry.txtOfficeExpAmount.Text) * -1.0),
									", 'Office Expense: ",
									officeExpEntry.txtOfficeExpDetail.Text,
									"', '",
									officeExpEntry.comboOfficeAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(officeExpEntry.txtOfficeExpAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									officeExpEntry.comboOfficeAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Office Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00017D50 File Offset: 0x00016150
		private void btnEditOfficeExpEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OfficeExpEntry officeExpEntry = new OfficeExpEntry();
				OfficeExpEntry officeExpEntry2 = officeExpEntry;
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
				{
					"Date"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				officeExpEntry2.OfficedatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				officeExpEntry2.comboOfficeAccounts.ItemsSource = this.comboOfficeAccounts.ItemsSource;
				officeExpEntry2.comboOfficeAccounts.DisplayMemberPath = this.comboOfficeAccounts.DisplayMemberPath;
				officeExpEntry2.comboOfficeAccounts.SelectedValuePath = this.comboOfficeAccounts.SelectedValuePath;
				officeExpEntry2.comboOfficeAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
				{
					"OfficeExpPaidBy"
				}, null));
				officeExpEntry2.Title = "Edit Office Expense Entry";
				officeExpEntry2.AccInfoText.Text = officeExpEntry2.Title;
				officeExpEntry2.txtOfficeExpAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
				{
					"OfficeExpAmount"
				}, null));
				officeExpEntry2.txtOfficeExpDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
				{
					"OfficeExpDetail"
				}, null));
				officeExpEntry.ShowDialog();
				bool flag = officeExpEntry.DialogResult != null && officeExpEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Office Expense Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE OfficeExpTable SET OfficeExpDate ='",
									officeExpEntry.OfficedatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', OfficeExpAmount =",
									officeExpEntry.txtOfficeExpAmount.Text,
									", OfficeExpDetail ='",
									officeExpEntry.txtOfficeExpDetail.Text,
									"', OfficeExpPaidBy ='",
									officeExpEntry.comboOfficeAccounts.Text,
									"' WHERE rowid ="
								}), NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									officeExpEntry.OfficedatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount =",
									Conversions.ToString(Conversions.ToDouble(officeExpEntry.txtOfficeExpAmount.Text) * -1.0),
									", Detail ='Office Expense: ",
									officeExpEntry.txtOfficeExpDetail.Text,
									"', Account ='",
									officeExpEntry.comboOfficeAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Date ='"
								}), NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"Date"
								}, null)), "' AND Amount ="), Operators.MultiplyObject(NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"OfficeExpAmount"
								}, null), -1)), " AND Detail ='Office Expense: "), NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"OfficeExpDetail"
								}, null)), "' AND Account ='"), NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"OfficeExpPaidBy"
								}, null)), "' LIMIT 1)"));
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(officeExpEntry.txtOfficeExpAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									officeExpEntry.comboOfficeAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance + ", NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"OfficeExpAmount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridOfficeExp.SelectedItem, new object[]
								{
									"OfficeExpPaidBy"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Office Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000182FC File Offset: 0x000166FC
		private void DataGridOfficeExp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridOfficeExp.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridOfficeExp.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditOfficeExpEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00018348 File Offset: 0x00016748
		private void DataGridOfficeExp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridOfficeExp.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditOfficeExpEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditOfficeExpEntry.IsEnabled = false;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001838C File Offset: 0x0001678C
		private void btnNewPayPkrEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PaymentPkrEntry paymentPkrEntry = new PaymentPkrEntry();
				PaymentPkrEntry paymentPkrEntry2 = paymentPkrEntry;
				paymentPkrEntry2.comboPaymentPkrAccounts.ItemsSource = this.comboPaymentPkrAccounts.ItemsSource;
				paymentPkrEntry2.comboPaymentPkrAccounts.SelectedValuePath = this.comboPaymentPkrAccounts.SelectedValuePath;
				paymentPkrEntry2.comboPaymentPkrAccounts.DisplayMemberPath = this.comboPaymentPkrAccounts.DisplayMemberPath;
				paymentPkrEntry2.comboPaymentPkrAccounts.SelectedIndex = this.comboPaymentPkrAccounts.SelectedIndex;
				paymentPkrEntry2.comboPaymentPkrCust.ItemsSource = this.comboPaymentPkrCust.ItemsSource;
				paymentPkrEntry2.comboPaymentPkrCust.SelectedValuePath = this.comboPaymentPkrCust.SelectedValuePath;
				paymentPkrEntry2.comboPaymentPkrCust.DisplayMemberPath = this.comboPaymentPkrCust.DisplayMemberPath;
				paymentPkrEntry2.comboPaymentPkrCust.SelectedIndex = this.comboPaymentPkrCust.SelectedIndex;
				paymentPkrEntry.ShowDialog();
				bool flag = paymentPkrEntry.DialogResult != null && paymentPkrEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Customer Payment Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO PaymentsPkrTable(PaymentDate, PaymentAmount, PaymentDetail, PaidFrom, PaidTo) VALUES ('",
									paymentPkrEntry.PaymentPkrdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									paymentPkrEntry.txtPaymentPkrAmount.Text,
									", '",
									paymentPkrEntry.txtPaymentPkrDetail.Text,
									"', '",
									paymentPkrEntry.comboPaymentPkrAccounts.Text,
									"', '",
									paymentPkrEntry.comboPaymentPkrCust.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
									paymentPkrEntry.PaymentPkrdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									Conversions.ToString(Conversions.ToDouble(paymentPkrEntry.txtPaymentPkrAmount.Text) * -1.0),
									", 'Payment PKR Entry: ",
									paymentPkrEntry.txtPaymentPkrDetail.Text,
									" To: ",
									paymentPkrEntry.comboPaymentPkrCust.Text,
									"', '",
									paymentPkrEntry.comboPaymentPkrAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(paymentPkrEntry.txtPaymentPkrAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									paymentPkrEntry.comboPaymentPkrAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE CustomersTable SET PaymentPaid = PaymentPaid + ",
									Conversions.ToString(value),
									", PaymentReceivable = PaymentReceivable + ",
									Conversions.ToString(value),
									" WHERE Name = '",
									paymentPkrEntry.comboPaymentPkrCust.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Payment Pkr Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000187B4 File Offset: 0x00016BB4
		private void btnEditPayPkrEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PaymentPkrEntry paymentPkrEntry = new PaymentPkrEntry();
				PaymentPkrEntry paymentPkrEntry2 = paymentPkrEntry;
				paymentPkrEntry2.Title = "Edit Customer Payment Entry";
				paymentPkrEntry2.AccInfoText.Text = paymentPkrEntry2.Title;
				paymentPkrEntry2.comboPaymentPkrAccounts.ItemsSource = this.comboPaymentPkrAccounts.ItemsSource;
				paymentPkrEntry2.comboPaymentPkrAccounts.SelectedValuePath = this.comboPaymentPkrAccounts.SelectedValuePath;
				paymentPkrEntry2.comboPaymentPkrAccounts.DisplayMemberPath = this.comboPaymentPkrAccounts.DisplayMemberPath;
				paymentPkrEntry2.comboPaymentPkrAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
				{
					"PaidFrom"
				}, null));
				paymentPkrEntry2.comboPaymentPkrCust.ItemsSource = this.comboPaymentPkrCust.ItemsSource;
				paymentPkrEntry2.comboPaymentPkrCust.SelectedValuePath = this.comboPaymentPkrCust.SelectedValuePath;
				paymentPkrEntry2.comboPaymentPkrCust.DisplayMemberPath = this.comboPaymentPkrCust.DisplayMemberPath;
				paymentPkrEntry2.comboPaymentPkrCust.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
				{
					"PaidTo"
				}, null));
				paymentPkrEntry2.txtPaymentPkrAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
				{
					"PaymentAmount"
				}, null));
				paymentPkrEntry2.txtPaymentPkrDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
				{
					"PaymentDetail"
				}, null));
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
				{
					"DATE"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				paymentPkrEntry2.PaymentPkrdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				paymentPkrEntry2.selectedCustomer = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
				{
					"PaidTo"
				}, null));
				paymentPkrEntry.ShowDialog();
				bool flag = paymentPkrEntry.DialogResult != null && paymentPkrEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Edit Customer Payment Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE PaymentsPkrTable SET PaymentDate ='",
									paymentPkrEntry.PaymentPkrdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', PaymentAmount =",
									paymentPkrEntry.txtPaymentPkrAmount.Text,
									", PaymentDetail ='",
									paymentPkrEntry.txtPaymentPkrDetail.Text,
									"', PaidFrom='",
									paymentPkrEntry.comboPaymentPkrAccounts.Text,
									"', PaidTo ='",
									paymentPkrEntry.comboPaymentPkrCust.Text,
									"' WHERE rowid = "
								}), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									paymentPkrEntry.PaymentPkrdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount =",
									Conversions.ToString(Conversions.ToDouble(paymentPkrEntry.txtPaymentPkrAmount.Text) * -1.0),
									", Detail ='Payment PKR Entry: ",
									paymentPkrEntry.txtPaymentPkrDetail.Text,
									" To: ",
									paymentPkrEntry.comboPaymentPkrCust.Text,
									"', Account ='",
									paymentPkrEntry.comboPaymentPkrAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Date ='"
								}), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"DATE"
								}, null)), "' AND Amount ="), Operators.MultiplyObject(NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null), -1)), " AND Detail ='Payment PKR Entry: "), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaymentDetail"
								}, null)), " To: "), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaidTo"
								}, null)), "' AND Account ='"), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaidFrom"
								}, null)), "' LIMIT 1)"));
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(paymentPkrEntry.txtPaymentPkrAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									paymentPkrEntry.comboPaymentPkrAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance + ", NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaidFrom"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE CustomersTable SET PaymentPaid = PaymentPaid + ",
									Conversions.ToString(value),
									", PaymentReceivable = PaymentReceivable + ",
									Conversions.ToString(value),
									" WHERE Name = '",
									paymentPkrEntry.comboPaymentPkrCust.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE CustomersTable SET PaymentPaid = PaymentPaid - ", NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null)), ", PaymentReceivable = PaymentReceivable - "), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null)), " WHERE Name = '"), NewLateBinding.LateIndexGet(this.DataGridPayPkr.SelectedItem, new object[]
								{
									"PaidTo"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Payment Pkr Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00018F60 File Offset: 0x00017360
		private void DataGridPayPkr_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridPayPkr.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridPayPkr.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditPayPkrEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00018FAC File Offset: 0x000173AC
		private void DataGridPayPkr_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridPayPkr.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditPayPkrEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditPayPkrEntry.IsEnabled = false;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00018FF0 File Offset: 0x000173F0
		private void btnClosing_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Closing Entry?", "Alert", MessageBoxButton.YesNo);
				bool flag = dialogResult == System.Windows.Forms.DialogResult.Yes;
				if (flag)
				{
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							sqliteCommand2.Connection = MainWindow.Connection;
							sqliteCommand2.CommandText = "INSERT INTO ProfitTable(Amount) VALUES ('" + Conversions.ToString(this.closingProfit) + "');";
							sqliteCommand2.ExecuteNonQuery();
							bool flag2 = this.payableYens > 0;
							if (flag2)
							{
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO PayableYenTable(Amount, Rate) VALUES ('",
									Conversions.ToString(this.payableYens),
									"', '",
									Conversions.ToString(MySettingsProperty.Settings.exchangeRate),
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
							}
							MySettings settings = MySettingsProperty.Settings;
							settings.strClosing = Conversions.ToString(DateTime.Today);
							settings.strclosingDutyExp = Conversions.ToString(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["DutyExpTable"].Compute("max(rowid)", ""))));
							settings.strclosingmiscexp = Conversions.ToString(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["MiscExpTable"].Compute("max(rowid)", ""))));
							settings.strclosingOfficeRow = Conversions.ToString(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["OfficeExpTable"].Compute("max(rowid)", ""))));
							settings.strclosingSalesRow = Conversions.ToString(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(MainWindow.DSet.Tables["SalesTable"].Compute("max(rowid)", ""))));
							settings.Save();
							sqliteCommand2.CommandText = string.Concat(new string[]
							{
								"INSERT INTO InfoTable(MiscExpLast, DutyExpLast, OfficeExpLast, SaleLast) VALUES (",
								MySettingsProperty.Settings.strclosingmiscexp,
								", ",
								MySettingsProperty.Settings.strclosingDutyExp,
								", ",
								MySettingsProperty.Settings.strclosingOfficeRow,
								", ",
								MySettingsProperty.Settings.strclosingSalesRow,
								");"
							});
							sqliteCommand2.ExecuteNonQuery();
							sqliteTransaction.Commit();
						}
						this.btnClosing.Visibility = Visibility.Collapsed;
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Closing Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00019314 File Offset: 0x00017714
		private void btnProfitWithdrawalEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool flag = Operators.CompareString(this.comboProfitWithdrawalAccounts.Text, string.Empty, false) == 0;
				if (flag)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag2 = MainWindow.convertInteger(this.txtProfitWithdrawalAmount.Text) < 0;
					if (flag2)
					{
						Interaction.MsgBox("Negative Amount Not Allowed", MsgBoxStyle.Exclamation, null);
					}
					else
					{
						bool flag3 = Operators.CompareString(this.txtProfitWithdrawalAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtProfitWithdrawalDetail.Text, string.Empty, false) != 0;
						if (flag3)
						{
							DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Profit Withdrawal Entry?", "Alert", MessageBoxButton.YesNo);
							bool flag4 = dialogResult == System.Windows.Forms.DialogResult.Yes;
							if (flag4)
							{
								using (SQLiteCommand sqliteCommand = new SQLiteCommand())
								{
									using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
									{
										SQLiteCommand sqliteCommand2 = sqliteCommand;
										sqliteCommand2.Connection = MainWindow.Connection;
										sqliteCommand2.CommandText = "INSERT INTO ProfitTable(Amount) VALUES (" + Conversions.ToString(Conversions.ToDouble(this.txtProfitWithdrawalAmount.Text) * -1.0) + ");";
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
											this.ProfitWithdrawaldatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											Conversions.ToString(Conversions.ToDouble(this.txtProfitWithdrawalAmount.Text) * -1.0),
											", 'Profit Withdrawal: ",
											this.txtProfitWithdrawalDetail.Text,
											"', '",
											this.comboProfitWithdrawalAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										double value = Conversions.ToDouble(this.txtProfitWithdrawalAmount.Text);
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
											Conversions.ToString(value),
											" WHERE AccountName ='",
											this.comboProfitWithdrawalAccounts.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteTransaction.Commit();
									}
									this.controlupdater();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Profit Withdrawal: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000195F8 File Offset: 0x000179F8
		private void btnOfficeAccountEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool flag = Operators.CompareString(this.comboCAccounts.Text, string.Empty, false) == 0 || Operators.CompareString(this.comboDAccounts.Text, string.Empty, false) == 0;
				if (flag)
				{
					Interaction.MsgBox("Please Select Account to continue...", MsgBoxStyle.OkOnly, null);
				}
				else
				{
					bool flag2 = MainWindow.convertInteger(this.txtOfficeAccountAmount.Text) < 0;
					if (flag2)
					{
						Interaction.MsgBox("Negative Value Not Allowed", MsgBoxStyle.Exclamation, null);
					}
					else
					{
						bool flag3 = Operators.CompareString(this.txtOfficeAccountAmount.Text, string.Empty, false) != 0 && Operators.CompareString(this.txtOfficeAccountDetail.Text, string.Empty, false) != 0;
						if (flag3)
						{
							DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter A/c to A/c Entry?", "Alert", MessageBoxButton.YesNo);
							bool flag4 = dialogResult == System.Windows.Forms.DialogResult.Yes;
							if (flag4)
							{
								using (SQLiteCommand sqliteCommand = new SQLiteCommand())
								{
									using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
									{
										SQLiteCommand sqliteCommand2 = sqliteCommand;
										sqliteCommand2.Connection = MainWindow.Connection;
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
											this.OfficeAccountdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											Conversions.ToString(Conversions.ToDouble(this.txtOfficeAccountAmount.Text) * -1.0),
											", 'A/c to A/c: ",
											this.txtOfficeAccountDetail.Text,
											"', '",
											this.comboCAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										int value = 0;
										bool flag5 = MainWindow.DSet.Tables["LedgerTable"].Rows.Count > 0;
										if (flag5)
										{
											value = Conversions.ToInteger(MainWindow.DSet.Tables["LedgerTable"].Rows[checked(MainWindow.DSet.Tables["LedgerTable"].Rows.Count - 1)]["rowid"]);
										}
										double value2 = Conversions.ToDouble(this.txtOfficeAccountAmount.Text);
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
											Conversions.ToString(value2),
											" WHERE AccountName ='",
											this.comboCAccounts.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
											this.OfficeAccountdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											this.txtOfficeAccountAmount.Text,
											", 'A/c to A/c: ",
											this.txtOfficeAccountDetail.Text,
											"', '",
											this.comboDAccounts.Text,
											"');"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"INSERT INTO OfficeAccountTable(Date, Amount, Detail, CreditFrom, DebitTo, LedgerRowId) VALUES ('",
											this.OfficeAccountdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
											"', ",
											this.txtOfficeAccountAmount.Text,
											", '",
											this.txtOfficeAccountDetail.Text,
											"', '",
											this.comboCAccounts.Text,
											"', '",
											this.comboDAccounts.Text,
											"', ",
											Conversions.ToString(value),
											");"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteCommand2.CommandText = string.Concat(new string[]
										{
											"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
											Conversions.ToString(value2),
											" WHERE AccountName ='",
											this.comboDAccounts.Text,
											"'"
										});
										sqliteCommand2.ExecuteNonQuery();
										sqliteTransaction.Commit();
									}
									this.controlupdater();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding A/c To A/c: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00019AD0 File Offset: 0x00017ED0
		private void btnEditOfficeAccountEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AccounttoAccount accounttoAccount = new AccounttoAccount();
				AccounttoAccount accounttoAccount2 = accounttoAccount;
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
				{
					"Date"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				accounttoAccount2.OfficeAccountdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				accounttoAccount2.comboCAccounts.ItemsSource = this.comboOfficeAccounts.ItemsSource;
				accounttoAccount2.comboCAccounts.DisplayMemberPath = this.comboOfficeAccounts.DisplayMemberPath;
				accounttoAccount2.comboCAccounts.SelectedValuePath = this.comboOfficeAccounts.SelectedValuePath;
				accounttoAccount2.comboCAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
				{
					"CreditFrom"
				}, null));
				accounttoAccount2.comboDAccounts.ItemsSource = this.comboOfficeAccounts.ItemsSource;
				accounttoAccount2.comboDAccounts.DisplayMemberPath = this.comboOfficeAccounts.DisplayMemberPath;
				accounttoAccount2.comboDAccounts.SelectedValuePath = this.comboOfficeAccounts.SelectedValuePath;
				accounttoAccount2.comboDAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
				{
					"Debitto"
				}, null));
				accounttoAccount2.AccInfoText.Text = accounttoAccount2.Title;
				accounttoAccount2.txtOfficeAccountAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
				{
					"Amount"
				}, null));
				accounttoAccount2.txtOfficeAccountDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
				{
					"Detail"
				}, null));
				accounttoAccount.ShowDialog();
				bool flag = accounttoAccount.DialogResult != null && accounttoAccount.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Edit A/c to A/c Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									accounttoAccount.OfficeAccountdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount = ",
									Conversions.ToString(Conversions.ToDouble(accounttoAccount.txtOfficeAccountAmount.Text) * -1.0),
									", Detail ='A/c to A/c: ",
									accounttoAccount.txtOfficeAccountDetail.Text,
									"', Account ='",
									accounttoAccount.comboCAccounts.Text,
									"' WHERE rowid = ",
									Conversions.ToString(checked(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
									{
										"LedgerRowId"
									}, null))) + 1))
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									accounttoAccount.OfficeAccountdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount = ",
									accounttoAccount.txtOfficeAccountAmount.Text,
									", Detail ='A/c to A/c: ",
									accounttoAccount.txtOfficeAccountDetail.Text,
									"', Account ='",
									accounttoAccount.comboDAccounts.Text,
									"' WHERE rowid = ",
									Conversions.ToString(checked(MainWindow.convertInteger(RuntimeHelpers.GetObjectValue(NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
									{
										"LedgerRowId"
									}, null))) + 2))
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(accounttoAccount.txtOfficeAccountAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									accounttoAccount.comboCAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance + ", NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
								{
									"Amount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
								{
									"CreditFrom"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance + ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									accounttoAccount.comboDAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance - ", NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
								{
									"Amount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
								{
									"DebitTo"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE OfficeAccountTable SET Date ='",
									accounttoAccount.OfficeAccountdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount = ",
									accounttoAccount.txtOfficeAccountAmount.Text,
									", Detail ='",
									accounttoAccount.txtOfficeAccountDetail.Text,
									"', CreditFrom ='",
									accounttoAccount.comboCAccounts.Text,
									"', DebitTo ='",
									accounttoAccount.comboDAccounts.Text,
									"' WHERE rowid = "
								}), NewLateBinding.LateIndexGet(this.DataGridOfficeAccount.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing A/c to A/c Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001A1F4 File Offset: 0x000185F4
		private void DataGridOfficeAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridOfficeAccount.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditOfficeAccountEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditOfficeAccountEntry.IsEnabled = false;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0001A238 File Offset: 0x00018638
		private void DataGridOfficeAccount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridOfficeAccount.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridOfficeAccount.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditOfficeAccountEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0001A284 File Offset: 0x00018684
		private void PaymentYen()
		{
			try
			{
				DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Adjust Stock Payable?", "Alert", MessageBoxButton.YesNo);
				bool flag = dialogResult == System.Windows.Forms.DialogResult.Yes;
				if (flag)
				{
					using (SQLiteCommand sqliteCommand = new SQLiteCommand())
					{
						using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
						{
							SQLiteCommand sqliteCommand2 = sqliteCommand;
							sqliteCommand2.Connection = MainWindow.Connection;
							DataRow[] array = MainWindow.DSet.Tables["StocksTable"].Select("PriceYen > PaidYen");
							double num = (double)MainWindow.convertInteger(Operators.SubtractObject(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountYen)", ""), MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PaidYen)", "")));
							foreach (DataRow dataRow in array)
							{
								bool flag2 = num == 0.0;
								if (flag2)
								{
									break;
								}
								bool flag3 = true;
								double num2 = Conversions.ToDouble(dataRow["PaidYen"]);
								double num3 = Conversions.ToDouble(Operators.SubtractObject(dataRow["PriceYen"], num2));
								bool flag4 = num3 > num;
								if (flag4)
								{
									num3 = num;
									flag3 = false;
								}
								double num4 = Convert.ToDouble(decimal.Round(Conversions.ToDecimal(Operators.DivideObject(Operators.SubtractObject(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountPkr)", ""), MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PaidAmount)", "")), Operators.SubtractObject(MainWindow.DSet.Tables["PaymentsTable"].Compute("Sum(PaymentAmountYen)", ""), MainWindow.DSet.Tables["StocksTable"].Compute("Sum(PaidYen)", "")))), 4));
								double num5 = num3 * num4;
								num4 = Convert.ToDouble(decimal.Round(Conversions.ToDecimal(Operators.DivideObject(Operators.AddObject(dataRow["PaidAmount"], num5), num2 + num3)), 4));
								bool flag5 = flag3;
								if (flag5)
								{
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE StocksTable Set Rate = " + Conversions.ToString(num4) + ", PricePkr = ", Operators.MultiplyObject(dataRow["PriceYen"], num4)), ", PaidYen = PaidYen + "), num3), ", PaidAmount = PaidAmount + "), num5), ", Cost = Duty + MiscExpense + PaidAmount + "), num5), " WHERE rowid ='"), dataRow["rowid"]), "'"));
								}
								else
								{
									sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE StocksTable Set Rate = " + Conversions.ToString(num4) + ", PricePkr = ", Operators.MultiplyObject(dataRow["PriceYen"], num4)), ", PaidYen = PaidYen + "), num3), ", PaidAmount = PaidAmount + "), num5), " WHERE rowid ='"), dataRow["rowid"]), "'"));
								}
								sqliteCommand2.ExecuteNonQuery();
								num -= num3;
							}
							sqliteTransaction.Commit();
						}
						this.controlupdater();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("PaymentYen Adjustment: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000318C File Offset: 0x0000158C
		private void BtnYenAdjust_Click(object sender, RoutedEventArgs e)
		{
			this.PaymentYen();
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00003196 File Offset: 0x00001596
		private void txtAgentFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			MainWindow.DSet.Tables["AgentsTable"].DefaultView.RowFilter = "Name LIKE '*" + this.txtAgentFilter.Text + "*'";
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0001A6D0 File Offset: 0x00018AD0
		private void LstAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.LstAgent.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditAgent.IsEnabled = true;
				bool flag2 = Operators.ConditionalCompareObjectGreater(NewLateBinding.LateIndexGet(this.LstAgent.SelectedItem, new object[]
				{
					"PaymentReceivable"
				}, null), 0, false);
				if (flag2)
				{
					this.runAgentRecPay.Text = "Payment Receivable: ";
				}
				else
				{
					this.runAgentRecPay.Text = "Payment Payable: ";
				}
			}
			else
			{
				this.BtnEditAgent.IsEnabled = false;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0001A768 File Offset: 0x00018B68
		private void LstAgent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.LstAgent.SelectedIndex != -1;
			if (flag)
			{
				this.BtnEditAgent_Click(RuntimeHelpers.GetObjectValue(sender), e);
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0001A79C File Offset: 0x00018B9C
		private void BtnAddAgent_Click(object sender, RoutedEventArgs e)
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

		// Token: 0x0600023F RID: 575 RVA: 0x0001A990 File Offset: 0x00018D90
		private void BtnEditAgent_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AddAgentForm addAgentForm = new AddAgentForm();
				addAgentForm.Title = "Edit Agent Entry";
				addAgentForm.AccInfoText.Text = "Edit Agent";
				AddAgentForm addAgentForm2 = addAgentForm;
				string[] array = this.RunAgentDate.Text.Split(new char[]
				{
					'-'
				});
				addAgentForm2.AgentdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				addAgentForm2.txtAgentName.Text = this.RunAgentName.Text;
				addAgentForm2.txtCnicNo.Value = this.RunAgentCNIC.Text;
				addAgentForm2.txtAgentPhoneNo.Text = this.RunAgentPhone.Text;
				addAgentForm2.txtAgentAddress.Text = this.RunAgentAddress.Text;
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
							sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
							{
								"UPDATE AgentsTable SET Date ='",
								addAgentForm.AgentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
								"', Name ='",
								addAgentForm.txtAgentName.Text,
								"', CNIC ='"
							}), addAgentForm.txtCnicNo.Value), "', Phone ='"), addAgentForm.txtAgentPhoneNo.Text), "', Address ='"), addAgentForm.txtAgentAddress.Text), "' WHERE rowid = "), this.RunAgentId.Text));
							sqliteCommand2.ExecuteNonQuery();
							bool flag2 = Operators.CompareString(addAgentForm.txtAgentName.Text, this.RunAgentName.Text, false) != 0;
							if (flag2)
							{
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE DutyExpTable SET DutyExpAgent = '",
									addAgentForm.txtAgentName.Text,
									"' WHERE DutyExpAgent = '",
									this.RunAgentName.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Detail = replace(Detail, '",
									this.RunAgentName.Text,
									"', '",
									addAgentForm.txtAgentName.Text,
									"') WHERE Detail LIKE 'Payment Agent%",
									this.RunAgentName.Text,
									"%'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE PaymentsAgentTable SET PaidTo = '",
									addAgentForm.txtAgentName.Text,
									"' WHERE Paidto = '",
									this.RunAgentName.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
							}
							sqliteTransaction.Commit();
						}
						int selectedIndex = this.LstAgent.SelectedIndex;
						this.controlupdater();
						this.LstAgent.SelectedIndex = selectedIndex;
						this.LstAgent.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Agent: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0001AD9C File Offset: 0x0001919C
		private void btnNewPayAgentEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PaymentAgentEntry paymentAgentEntry = new PaymentAgentEntry();
				PaymentAgentEntry paymentAgentEntry2 = paymentAgentEntry;
				paymentAgentEntry2.comboPaymentAgentAccounts.ItemsSource = this.comboPaymentPkrAccounts.ItemsSource;
				paymentAgentEntry2.comboPaymentAgentAccounts.SelectedValuePath = this.comboPaymentPkrAccounts.SelectedValuePath;
				paymentAgentEntry2.comboPaymentAgentAccounts.DisplayMemberPath = this.comboPaymentPkrAccounts.DisplayMemberPath;
				paymentAgentEntry2.comboPaymentAgentAccounts.SelectedIndex = this.comboPaymentPkrAccounts.SelectedIndex;
				paymentAgentEntry2.comboPaymentAgent.ItemsSource = this.comboDutyAgents.ItemsSource;
				paymentAgentEntry2.comboPaymentAgent.SelectedValuePath = this.comboDutyAgents.SelectedValuePath;
				paymentAgentEntry2.comboPaymentAgent.DisplayMemberPath = this.comboDutyAgents.DisplayMemberPath;
				paymentAgentEntry2.comboPaymentAgent.SelectedIndex = this.comboDutyAgents.SelectedIndex;
				paymentAgentEntry.ShowDialog();
				bool flag = paymentAgentEntry.DialogResult != null && paymentAgentEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Enter Agent Payment Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO PaymentsAgentTable(PaymentDate, PaymentAmount, PaymentDetail, PaidFrom, PaidTo) VALUES ('",
									paymentAgentEntry.PaymentAgentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									paymentAgentEntry.txtPaymentAgentAmount.Text,
									", '",
									paymentAgentEntry.txtPaymentAgentDetail.Text,
									"', '",
									paymentAgentEntry.comboPaymentAgentAccounts.Text,
									"', '",
									paymentAgentEntry.comboPaymentAgent.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"INSERT INTO LedgerTable(Date, Amount, Detail, Account) VALUES ('",
									paymentAgentEntry.PaymentAgentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', ",
									Conversions.ToString(Conversions.ToDouble(paymentAgentEntry.txtPaymentAgentAmount.Text) * -1.0),
									", 'Payment Agent Entry: ",
									paymentAgentEntry.txtPaymentAgentDetail.Text,
									" To: ",
									paymentAgentEntry.comboPaymentAgent.Text,
									"', '",
									paymentAgentEntry.comboPaymentAgentAccounts.Text,
									"');"
								});
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(paymentAgentEntry.txtPaymentAgentAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									paymentAgentEntry.comboPaymentAgentAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AgentsTable SET PaymentPaid = PaymentPaid + ",
									Conversions.ToString(value),
									", PaymentReceivable = PaymentReceivable + ",
									Conversions.ToString(value),
									" WHERE Name = '",
									paymentAgentEntry.comboPaymentAgent.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Adding Payment Agent Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0001B1C4 File Offset: 0x000195C4
		private void btnEditPayAgentEntry_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				PaymentAgentEntry paymentAgentEntry = new PaymentAgentEntry();
				PaymentAgentEntry paymentAgentEntry2 = paymentAgentEntry;
				paymentAgentEntry2.Title = "Edit Agent Payment Entry";
				paymentAgentEntry2.AccInfoText.Text = paymentAgentEntry2.Title;
				paymentAgentEntry2.comboPaymentAgentAccounts.ItemsSource = this.comboPaymentPkrAccounts.ItemsSource;
				paymentAgentEntry2.comboPaymentAgentAccounts.SelectedValuePath = this.comboPaymentPkrAccounts.SelectedValuePath;
				paymentAgentEntry2.comboPaymentAgentAccounts.DisplayMemberPath = this.comboPaymentPkrAccounts.DisplayMemberPath;
				paymentAgentEntry2.comboPaymentAgentAccounts.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
				{
					"PaidFrom"
				}, null));
				paymentAgentEntry2.comboPaymentAgent.ItemsSource = this.comboDutyAgents.ItemsSource;
				paymentAgentEntry2.comboPaymentAgent.SelectedValuePath = this.comboDutyAgents.SelectedValuePath;
				paymentAgentEntry2.comboPaymentAgent.DisplayMemberPath = this.comboDutyAgents.DisplayMemberPath;
				paymentAgentEntry2.comboPaymentAgent.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
				{
					"PaidTo"
				}, null));
				paymentAgentEntry2.txtPaymentAgentAmount.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
				{
					"PaymentAmount"
				}, null));
				paymentAgentEntry2.txtPaymentAgentDetail.Text = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
				{
					"PaymentDetail"
				}, null));
				string[] array = (string[])NewLateBinding.LateGet(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
				{
					"DATE"
				}, null), null, "Split", new object[]
				{
					"-"
				}, null, null, null);
				paymentAgentEntry2.PaymentAgentdatePicker.SelectedDate = new DateTime?(new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2])));
				paymentAgentEntry2.selectedAgent = Conversions.ToString(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
				{
					"PaidTo"
				}, null));
				paymentAgentEntry.ShowDialog();
				bool flag = paymentAgentEntry.DialogResult != null && paymentAgentEntry.DialogResult.Value;
				if (flag)
				{
					DialogResult dialogResult = (DialogResult)System.Windows.MessageBox.Show("Are You Sure to Edit Agent Payment Entry?", "Alert", MessageBoxButton.YesNo);
					bool flag2 = dialogResult == System.Windows.Forms.DialogResult.Yes;
					if (flag2)
					{
						using (SQLiteCommand sqliteCommand = new SQLiteCommand())
						{
							using (SQLiteTransaction sqliteTransaction = MainWindow.Connection.BeginTransaction())
							{
								SQLiteCommand sqliteCommand2 = sqliteCommand;
								sqliteCommand2.Connection = MainWindow.Connection;
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE PaymentsAgentTable SET PaymentDate ='",
									paymentAgentEntry.PaymentAgentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', PaymentAmount =",
									paymentAgentEntry.txtPaymentAgentAmount.Text,
									", PaymentDetail ='",
									paymentAgentEntry.txtPaymentAgentDetail.Text,
									"', PaidFrom='",
									paymentAgentEntry.comboPaymentAgentAccounts.Text,
									"', PaidTo ='",
									paymentAgentEntry.comboPaymentAgent.Text,
									"' WHERE rowid = "
								}), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"rowid"
								}, null)));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(string.Concat(new string[]
								{
									"UPDATE LedgerTable SET Date ='",
									paymentAgentEntry.PaymentAgentdatePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
									"', Amount =",
									Conversions.ToString(Conversions.ToDouble(paymentAgentEntry.txtPaymentAgentAmount.Text) * -1.0),
									", Detail ='Payment Agent Entry: ",
									paymentAgentEntry.txtPaymentAgentDetail.Text,
									" To: ",
									paymentAgentEntry.comboPaymentAgent.Text,
									"', Account ='",
									paymentAgentEntry.comboPaymentAgentAccounts.Text,
									"' WHERE rowid IN (SELECT rowid FROM LedgerTable WHERE Date ='"
								}), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"DATE"
								}, null)), "' AND Amount ="), Operators.MultiplyObject(NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null), -1)), " AND Detail ='Payment Agent Entry: "), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaymentDetail"
								}, null)), " To: "), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaidTo"
								}, null)), "' AND Account ='"), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaidFrom"
								}, null)), "' LIMIT 1)"));
								sqliteCommand2.ExecuteNonQuery();
								double value = Conversions.ToDouble(paymentAgentEntry.txtPaymentAgentAmount.Text);
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AccountTable Set CurrentBalance = CurrentBalance - ",
									Conversions.ToString(value),
									" WHERE AccountName ='",
									paymentAgentEntry.comboPaymentAgentAccounts.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AccountTable Set CurrentBalance = CurrentBalance + ", NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null)), " WHERE AccountName ='"), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaidFrom"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = string.Concat(new string[]
								{
									"UPDATE AgentsTable SET PaymentPaid = PaymentPaid + ",
									Conversions.ToString(value),
									", PaymentReceivable = PaymentReceivable + ",
									Conversions.ToString(value),
									" WHERE Name = '",
									paymentAgentEntry.comboPaymentAgent.Text,
									"'"
								});
								sqliteCommand2.ExecuteNonQuery();
								sqliteCommand2.CommandText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("UPDATE AgentsTable SET PaymentPaid = PaymentPaid - ", NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null)), ", PaymentReceivable = PaymentReceivable - "), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaymentAmount"
								}, null)), " WHERE Name = '"), NewLateBinding.LateIndexGet(this.DataGridPayAgent.SelectedItem, new object[]
								{
									"PaidTo"
								}, null)), "'"));
								sqliteCommand2.ExecuteNonQuery();
								sqliteTransaction.Commit();
							}
							this.controlupdater();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainWindow.errorlog("When Editing Payment Agent Entry: " + ex.Message + "\r\n" + ex.ToString());
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0001B970 File Offset: 0x00019D70
		private void DataGridPayAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.DataGridPayAgent.SelectedIndex != -1;
			if (flag)
			{
				this.btnEditPayAgentEntry.IsEnabled = true;
			}
			else
			{
				this.btnEditPayAgentEntry.IsEnabled = false;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0001B9B4 File Offset: 0x00019DB4
		private void DataGridPayAgent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.DataGridPayAgent.SelectedIndex != -1;
			if (flag)
			{
				bool flag2 = this.DataGridPayAgent.SelectedIndex != -1;
				if (flag2)
				{
					this.btnEditPayAgentEntry_Click(RuntimeHelpers.GetObjectValue(sender), e);
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000251A File Offset: 0x0000091A
		private void MainWindow_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
		{
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000245 RID: 581 RVA: 0x000031D2 File Offset: 0x000015D2
		// (set) Token: 0x06000246 RID: 582 RVA: 0x000031DC File Offset: 0x000015DC
		internal virtual WrapPanel WrapExcEdit { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000247 RID: 583 RVA: 0x000031E5 File Offset: 0x000015E5
		// (set) Token: 0x06000248 RID: 584 RVA: 0x000031EF File Offset: 0x000015EF
		internal virtual System.Windows.Controls.TextBox txtEditExcRate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000031F8 File Offset: 0x000015F8
		// (set) Token: 0x0600024A RID: 586 RVA: 0x00003202 File Offset: 0x00001602
		internal virtual System.Windows.Controls.Button btnExcEdit { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000320B File Offset: 0x0000160B
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00003215 File Offset: 0x00001615
		internal virtual WrapPanel WrapExcRate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000321E File Offset: 0x0000161E
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00003228 File Offset: 0x00001628
		internal virtual TextBlock txtExcRate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00003231 File Offset: 0x00001631
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000323B File Offset: 0x0000163B
		internal virtual Hyperlink hyplnk { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00003244 File Offset: 0x00001644
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000324E File Offset: 0x0000164E
		internal virtual WrapPanel AdjustLink { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00003257 File Offset: 0x00001657
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00003261 File Offset: 0x00001661
		internal virtual Hyperlink BtnYenAdjust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000326A File Offset: 0x0000166A
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00003274 File Offset: 0x00001674
		internal virtual Hyperlink hyplnk2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000327D File Offset: 0x0000167D
		// (set) Token: 0x06000258 RID: 600 RVA: 0x00003287 File Offset: 0x00001687
		internal virtual DockPanel stackData { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000259 RID: 601 RVA: 0x00003290 File Offset: 0x00001690
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000329A File Offset: 0x0000169A
		internal virtual System.Windows.Controls.TabControl MainGrid { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600025B RID: 603 RVA: 0x000032A3 File Offset: 0x000016A3
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0001BA00 File Offset: 0x00019E00
		internal virtual System.Windows.Controls.Button BtnCreateData
		{
			[CompilerGenerated]
			get
			{
				return this._BtnCreateData;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnCreateData_Click);
				System.Windows.Controls.Button btnCreateData = this._BtnCreateData;
				if (btnCreateData != null)
				{
					btnCreateData.Click -= value2;
				}
				this._BtnCreateData = value;
				btnCreateData = this._BtnCreateData;
				if (btnCreateData != null)
				{
					btnCreateData.Click += value2;
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600025D RID: 605 RVA: 0x000032AD File Offset: 0x000016AD
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0001BA44 File Offset: 0x00019E44
		internal virtual System.Windows.Controls.Button BtnOpenData
		{
			[CompilerGenerated]
			get
			{
				return this._BtnOpenData;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnOpenData_Click);
				System.Windows.Controls.Button btnOpenData = this._BtnOpenData;
				if (btnOpenData != null)
				{
					btnOpenData.Click -= value2;
				}
				this._BtnOpenData = value;
				btnOpenData = this._BtnOpenData;
				if (btnOpenData != null)
				{
					btnOpenData.Click += value2;
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600025F RID: 607 RVA: 0x000032B7 File Offset: 0x000016B7
		// (set) Token: 0x06000260 RID: 608 RVA: 0x000032C1 File Offset: 0x000016C1
		internal virtual System.Windows.Controls.TabControl TabCtrAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000032CA File Offset: 0x000016CA
		// (set) Token: 0x06000262 RID: 610 RVA: 0x000032D4 File Offset: 0x000016D4
		internal virtual Grid MainAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000032DD File Offset: 0x000016DD
		// (set) Token: 0x06000264 RID: 612 RVA: 0x000032E7 File Offset: 0x000016E7
		internal virtual DockPanel staAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000032F0 File Offset: 0x000016F0
		// (set) Token: 0x06000266 RID: 614 RVA: 0x000032FA File Offset: 0x000016FA
		internal virtual StackPanel GrpAccountsBtns { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00003303 File Offset: 0x00001703
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0001BA88 File Offset: 0x00019E88
		internal virtual System.Windows.Controls.Button BtnAddAccount
		{
			[CompilerGenerated]
			get
			{
				return this._BtnAddAccount;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnAddAccount_Click);
				System.Windows.Controls.Button btnAddAccount = this._BtnAddAccount;
				if (btnAddAccount != null)
				{
					btnAddAccount.Click -= value2;
				}
				this._BtnAddAccount = value;
				btnAddAccount = this._BtnAddAccount;
				if (btnAddAccount != null)
				{
					btnAddAccount.Click += value2;
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000330D File Offset: 0x0000170D
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0001BACC File Offset: 0x00019ECC
		internal virtual System.Windows.Controls.Button BtnEditAccount
		{
			[CompilerGenerated]
			get
			{
				return this._BtnEditAccount;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnEditAccount_Click);
				System.Windows.Controls.Button btnEditAccount = this._BtnEditAccount;
				if (btnEditAccount != null)
				{
					btnEditAccount.Click -= value2;
				}
				this._BtnEditAccount = value;
				btnEditAccount = this._BtnEditAccount;
				if (btnEditAccount != null)
				{
					btnEditAccount.Click += value2;
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00003317 File Offset: 0x00001717
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0001BB10 File Offset: 0x00019F10
		internal virtual System.Windows.Controls.Button BtnDeleteAccount
		{
			[CompilerGenerated]
			get
			{
				return this._BtnDeleteAccount;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnDeleteAccount_Click);
				System.Windows.Controls.Button btnDeleteAccount = this._BtnDeleteAccount;
				if (btnDeleteAccount != null)
				{
					btnDeleteAccount.Click -= value2;
				}
				this._BtnDeleteAccount = value;
				btnDeleteAccount = this._BtnDeleteAccount;
				if (btnDeleteAccount != null)
				{
					btnDeleteAccount.Click += value2;
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00003321 File Offset: 0x00001721
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000332B File Offset: 0x0000172B
		internal virtual System.Windows.Controls.TextBox txtAccountFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00003334 File Offset: 0x00001734
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000333E File Offset: 0x0000173E
		internal virtual Grid AccountsGrid { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00003347 File Offset: 0x00001747
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00003351 File Offset: 0x00001751
		internal virtual System.Windows.Controls.ListView LstAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000335A File Offset: 0x0000175A
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00003364 File Offset: 0x00001764
		internal virtual DockPanel AccountDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000336D File Offset: 0x0000176D
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00003377 File Offset: 0x00001777
		internal virtual TextBlock TxtBlkAccountDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00003380 File Offset: 0x00001780
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000338A File Offset: 0x0000178A
		internal virtual Run runRowId { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00003393 File Offset: 0x00001793
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000339D File Offset: 0x0000179D
		internal virtual Run runDate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600027B RID: 635 RVA: 0x000033A6 File Offset: 0x000017A6
		// (set) Token: 0x0600027C RID: 636 RVA: 0x000033B0 File Offset: 0x000017B0
		internal virtual Run runAccountType { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600027D RID: 637 RVA: 0x000033B9 File Offset: 0x000017B9
		// (set) Token: 0x0600027E RID: 638 RVA: 0x000033C3 File Offset: 0x000017C3
		internal virtual Run runAccountName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000033CC File Offset: 0x000017CC
		// (set) Token: 0x06000280 RID: 640 RVA: 0x000033D6 File Offset: 0x000017D6
		internal virtual Run runAccountNumber { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000281 RID: 641 RVA: 0x000033DF File Offset: 0x000017DF
		// (set) Token: 0x06000282 RID: 642 RVA: 0x000033E9 File Offset: 0x000017E9
		internal virtual Run runAccountTitle { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000283 RID: 643 RVA: 0x000033F2 File Offset: 0x000017F2
		// (set) Token: 0x06000284 RID: 644 RVA: 0x000033FC File Offset: 0x000017FC
		internal virtual Run runBankName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00003405 File Offset: 0x00001805
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000340F File Offset: 0x0000180F
		internal virtual Run runBankBranch { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00003418 File Offset: 0x00001818
		// (set) Token: 0x06000288 RID: 648 RVA: 0x00003422 File Offset: 0x00001822
		internal virtual Run runOpeningBalance { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000342B File Offset: 0x0000182B
		// (set) Token: 0x0600028A RID: 650 RVA: 0x00003435 File Offset: 0x00001835
		internal virtual Run runCurrentBalance { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000343E File Offset: 0x0000183E
		// (set) Token: 0x0600028C RID: 652 RVA: 0x00003448 File Offset: 0x00001848
		internal virtual Grid TabOfficeAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00003451 File Offset: 0x00001851
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000345B File Offset: 0x0000185B
		internal virtual System.Windows.Controls.Button btnEditOfficeAccountEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00003464 File Offset: 0x00001864
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000346E File Offset: 0x0000186E
		internal virtual System.Windows.Controls.DataGrid DataGridOfficeAccount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00003477 File Offset: 0x00001877
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00003481 File Offset: 0x00001881
		internal virtual StackPanel UniOfficeAccountDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000348A File Offset: 0x0000188A
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00003494 File Offset: 0x00001894
		internal virtual DatePicker OfficeAccountdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000349D File Offset: 0x0000189D
		// (set) Token: 0x06000296 RID: 662 RVA: 0x000034A7 File Offset: 0x000018A7
		internal virtual System.Windows.Controls.TextBox txtOfficeAccountAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000297 RID: 663 RVA: 0x000034B0 File Offset: 0x000018B0
		// (set) Token: 0x06000298 RID: 664 RVA: 0x000034BA File Offset: 0x000018BA
		internal virtual System.Windows.Controls.TextBox txtOfficeAccountDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000299 RID: 665 RVA: 0x000034C3 File Offset: 0x000018C3
		// (set) Token: 0x0600029A RID: 666 RVA: 0x000034CD File Offset: 0x000018CD
		internal virtual System.Windows.Controls.ComboBox comboCAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600029B RID: 667 RVA: 0x000034D6 File Offset: 0x000018D6
		// (set) Token: 0x0600029C RID: 668 RVA: 0x000034E0 File Offset: 0x000018E0
		internal virtual System.Windows.Controls.ComboBox comboDAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600029D RID: 669 RVA: 0x000034E9 File Offset: 0x000018E9
		// (set) Token: 0x0600029E RID: 670 RVA: 0x000034F3 File Offset: 0x000018F3
		internal virtual System.Windows.Controls.Button btnOfficeAccountEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000034FC File Offset: 0x000018FC
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x00003506 File Offset: 0x00001906
		internal virtual Grid TabMiscExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000350F File Offset: 0x0000190F
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x00003519 File Offset: 0x00001919
		internal virtual System.Windows.Controls.Button btnNewMiscExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00003522 File Offset: 0x00001922
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000352C File Offset: 0x0000192C
		internal virtual System.Windows.Controls.Button btnEditMiscExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00003535 File Offset: 0x00001935
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000353F File Offset: 0x0000193F
		internal virtual System.Windows.Controls.DataGrid DataGridMiscExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00003548 File Offset: 0x00001948
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x00003552 File Offset: 0x00001952
		internal virtual StackPanel UniMiscDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000355B File Offset: 0x0000195B
		// (set) Token: 0x060002AA RID: 682 RVA: 0x00003565 File Offset: 0x00001965
		internal virtual System.Windows.Controls.ComboBox comboMiscChassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000356E File Offset: 0x0000196E
		// (set) Token: 0x060002AC RID: 684 RVA: 0x00003578 File Offset: 0x00001978
		internal virtual DatePicker MiscdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00003581 File Offset: 0x00001981
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000358B File Offset: 0x0000198B
		internal virtual System.Windows.Controls.TextBox txtMiscExpAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00003594 File Offset: 0x00001994
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000359E File Offset: 0x0000199E
		internal virtual System.Windows.Controls.TextBox txtMiscExpDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x000035A7 File Offset: 0x000019A7
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x000035B1 File Offset: 0x000019B1
		internal virtual System.Windows.Controls.ComboBox comboMiscAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x000035BA File Offset: 0x000019BA
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0001BB54 File Offset: 0x00019F54
		internal virtual System.Windows.Controls.Button btnMiscExpEntry
		{
			[CompilerGenerated]
			get
			{
				return this._btnMiscExpEntry;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.btnMiscExpEntry_Click);
				System.Windows.Controls.Button btnMiscExpEntry = this._btnMiscExpEntry;
				if (btnMiscExpEntry != null)
				{
					btnMiscExpEntry.Click -= value2;
				}
				this._btnMiscExpEntry = value;
				btnMiscExpEntry = this._btnMiscExpEntry;
				if (btnMiscExpEntry != null)
				{
					btnMiscExpEntry.Click += value2;
				}
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x000035C4 File Offset: 0x000019C4
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x000035CE File Offset: 0x000019CE
		internal virtual Grid TabDutyExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x000035D7 File Offset: 0x000019D7
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x000035E1 File Offset: 0x000019E1
		internal virtual System.Windows.Controls.Button btnNewDutyExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x000035EA File Offset: 0x000019EA
		// (set) Token: 0x060002BA RID: 698 RVA: 0x000035F4 File Offset: 0x000019F4
		internal virtual System.Windows.Controls.Button btnEditDutyExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002BB RID: 699 RVA: 0x000035FD File Offset: 0x000019FD
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00003607 File Offset: 0x00001A07
		internal virtual System.Windows.Controls.DataGrid DataGridDutyExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00003610 File Offset: 0x00001A10
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000361A File Offset: 0x00001A1A
		internal virtual UniformGrid UniDutyDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00003623 File Offset: 0x00001A23
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000362D File Offset: 0x00001A2D
		internal virtual System.Windows.Controls.ComboBox comboDutyChassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00003636 File Offset: 0x00001A36
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00003640 File Offset: 0x00001A40
		internal virtual DatePicker DutydatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00003649 File Offset: 0x00001A49
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00003653 File Offset: 0x00001A53
		internal virtual System.Windows.Controls.TextBox txtDutyExpAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000365C File Offset: 0x00001A5C
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x00003666 File Offset: 0x00001A66
		internal virtual System.Windows.Controls.TextBox txtDutyExpDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000366F File Offset: 0x00001A6F
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x00003679 File Offset: 0x00001A79
		internal virtual System.Windows.Controls.ComboBox comboDutyAgents { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00003682 File Offset: 0x00001A82
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000368C File Offset: 0x00001A8C
		internal virtual System.Windows.Controls.Button btnDutyAddAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00003695 File Offset: 0x00001A95
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000369F File Offset: 0x00001A9F
		internal virtual System.Windows.Controls.Button btnDutyExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002CD RID: 717 RVA: 0x000036A8 File Offset: 0x00001AA8
		// (set) Token: 0x060002CE RID: 718 RVA: 0x000036B2 File Offset: 0x00001AB2
		internal virtual Grid TabOfficeExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002CF RID: 719 RVA: 0x000036BB File Offset: 0x00001ABB
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x000036C5 File Offset: 0x00001AC5
		internal virtual System.Windows.Controls.Button btnNewOfficeExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000036CE File Offset: 0x00001ACE
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x000036D8 File Offset: 0x00001AD8
		internal virtual System.Windows.Controls.Button btnEditOfficeExpEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x000036E1 File Offset: 0x00001AE1
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x000036EB File Offset: 0x00001AEB
		internal virtual System.Windows.Controls.DataGrid DataGridOfficeExp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x000036F4 File Offset: 0x00001AF4
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x000036FE File Offset: 0x00001AFE
		internal virtual StackPanel UniOfficeDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00003707 File Offset: 0x00001B07
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00003711 File Offset: 0x00001B11
		internal virtual DatePicker OfficedatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000371A File Offset: 0x00001B1A
		// (set) Token: 0x060002DA RID: 730 RVA: 0x00003724 File Offset: 0x00001B24
		internal virtual System.Windows.Controls.TextBox txtOfficeExpAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000372D File Offset: 0x00001B2D
		// (set) Token: 0x060002DC RID: 732 RVA: 0x00003737 File Offset: 0x00001B37
		internal virtual System.Windows.Controls.TextBox txtOfficeExpDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00003740 File Offset: 0x00001B40
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000374A File Offset: 0x00001B4A
		internal virtual System.Windows.Controls.ComboBox comboOfficeAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00003753 File Offset: 0x00001B53
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0001BB98 File Offset: 0x00019F98
		internal virtual System.Windows.Controls.Button btnOfficeExpEntry
		{
			[CompilerGenerated]
			get
			{
				return this._btnOfficeExpEntry;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.btnOfficeExpEntry_Click);
				System.Windows.Controls.Button btnOfficeExpEntry = this._btnOfficeExpEntry;
				if (btnOfficeExpEntry != null)
				{
					btnOfficeExpEntry.Click -= value2;
				}
				this._btnOfficeExpEntry = value;
				btnOfficeExpEntry = this._btnOfficeExpEntry;
				if (btnOfficeExpEntry != null)
				{
					btnOfficeExpEntry.Click += value2;
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000375D File Offset: 0x00001B5D
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x00003767 File Offset: 0x00001B67
		internal virtual Grid TabReceipt { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00003770 File Offset: 0x00001B70
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000377A File Offset: 0x00001B7A
		internal virtual System.Windows.Controls.Button btnNewReceiptEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00003783 File Offset: 0x00001B83
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000378D File Offset: 0x00001B8D
		internal virtual System.Windows.Controls.Button btnEditReceiptEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00003796 File Offset: 0x00001B96
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x000037A0 File Offset: 0x00001BA0
		internal virtual System.Windows.Controls.DataGrid DataGridReceipts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000037A9 File Offset: 0x00001BA9
		// (set) Token: 0x060002EA RID: 746 RVA: 0x000037B3 File Offset: 0x00001BB3
		internal virtual UniformGrid UniReceiptDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000037BC File Offset: 0x00001BBC
		// (set) Token: 0x060002EC RID: 748 RVA: 0x000037C6 File Offset: 0x00001BC6
		internal virtual System.Windows.Controls.ComboBox comboReceiptCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000037CF File Offset: 0x00001BCF
		// (set) Token: 0x060002EE RID: 750 RVA: 0x000037D9 File Offset: 0x00001BD9
		internal virtual DatePicker ReceiptdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002EF RID: 751 RVA: 0x000037E2 File Offset: 0x00001BE2
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x000037EC File Offset: 0x00001BEC
		internal virtual System.Windows.Controls.TextBox txtReceiptAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000037F5 File Offset: 0x00001BF5
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x000037FF File Offset: 0x00001BFF
		internal virtual System.Windows.Controls.TextBox txtReceiptDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00003808 File Offset: 0x00001C08
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x00003812 File Offset: 0x00001C12
		internal virtual System.Windows.Controls.ComboBox comboReceiptAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000381B File Offset: 0x00001C1B
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x0001BBDC File Offset: 0x00019FDC
		internal virtual System.Windows.Controls.Button btnReceiptEntry
		{
			[CompilerGenerated]
			get
			{
				return this._btnReceiptEntry;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.btnReceiptEntry_Click);
				System.Windows.Controls.Button btnReceiptEntry = this._btnReceiptEntry;
				if (btnReceiptEntry != null)
				{
					btnReceiptEntry.Click -= value2;
				}
				this._btnReceiptEntry = value;
				btnReceiptEntry = this._btnReceiptEntry;
				if (btnReceiptEntry != null)
				{
					btnReceiptEntry.Click += value2;
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00003825 File Offset: 0x00001C25
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x0000382F File Offset: 0x00001C2F
		internal virtual Grid TabPayment { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00003838 File Offset: 0x00001C38
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00003842 File Offset: 0x00001C42
		internal virtual StackPanel UniPaymentDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000384B File Offset: 0x00001C4B
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00003855 File Offset: 0x00001C55
		internal virtual TextBlock TotalAccountsPayable { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000385E File Offset: 0x00001C5E
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00003868 File Offset: 0x00001C68
		internal virtual DatePicker PaymentdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00003871 File Offset: 0x00001C71
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000387B File Offset: 0x00001C7B
		internal virtual System.Windows.Controls.TextBox txtPaymentAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00003884 File Offset: 0x00001C84
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000388E File Offset: 0x00001C8E
		internal virtual System.Windows.Controls.TextBox txtPaymentRate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00003897 File Offset: 0x00001C97
		// (set) Token: 0x06000304 RID: 772 RVA: 0x000038A1 File Offset: 0x00001CA1
		internal virtual System.Windows.Controls.TextBox txtPaymentAmountPkr { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000305 RID: 773 RVA: 0x000038AA File Offset: 0x00001CAA
		// (set) Token: 0x06000306 RID: 774 RVA: 0x000038B4 File Offset: 0x00001CB4
		internal virtual System.Windows.Controls.TextBox txtPaymentDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000307 RID: 775 RVA: 0x000038BD File Offset: 0x00001CBD
		// (set) Token: 0x06000308 RID: 776 RVA: 0x000038C7 File Offset: 0x00001CC7
		internal virtual System.Windows.Controls.ComboBox comboPaymentAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000309 RID: 777 RVA: 0x000038D0 File Offset: 0x00001CD0
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0001BC20 File Offset: 0x0001A020
		internal virtual System.Windows.Controls.Button btnPaymentEntry
		{
			[CompilerGenerated]
			get
			{
				return this._btnPaymentEntry;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.btnPaymentEntry_Click);
				System.Windows.Controls.Button btnPaymentEntry = this._btnPaymentEntry;
				if (btnPaymentEntry != null)
				{
					btnPaymentEntry.Click -= value2;
				}
				this._btnPaymentEntry = value;
				btnPaymentEntry = this._btnPaymentEntry;
				if (btnPaymentEntry != null)
				{
					btnPaymentEntry.Click += value2;
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000038DA File Offset: 0x00001CDA
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000038E4 File Offset: 0x00001CE4
		internal virtual Grid TabPaymentPkr { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000038ED File Offset: 0x00001CED
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000038F7 File Offset: 0x00001CF7
		internal virtual System.Windows.Controls.Button btnNewPayPkrEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00003900 File Offset: 0x00001D00
		// (set) Token: 0x06000310 RID: 784 RVA: 0x0000390A File Offset: 0x00001D0A
		internal virtual System.Windows.Controls.Button btnEditPayPkrEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00003913 File Offset: 0x00001D13
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000391D File Offset: 0x00001D1D
		internal virtual System.Windows.Controls.DataGrid DataGridPayPkr { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00003926 File Offset: 0x00001D26
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00003930 File Offset: 0x00001D30
		internal virtual StackPanel UniPaymentPkrDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00003939 File Offset: 0x00001D39
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00003943 File Offset: 0x00001D43
		internal virtual System.Windows.Controls.ComboBox comboPaymentPkrCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000394C File Offset: 0x00001D4C
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00003956 File Offset: 0x00001D56
		internal virtual System.Windows.Controls.Button BtnAddCustatPay { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000395F File Offset: 0x00001D5F
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00003969 File Offset: 0x00001D69
		internal virtual DatePicker PaymentPkrdatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00003972 File Offset: 0x00001D72
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000397C File Offset: 0x00001D7C
		internal virtual System.Windows.Controls.TextBox txtPaymentPkrAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00003985 File Offset: 0x00001D85
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000398F File Offset: 0x00001D8F
		internal virtual System.Windows.Controls.TextBox txtPaymentPkrDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00003998 File Offset: 0x00001D98
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000039A2 File Offset: 0x00001DA2
		internal virtual System.Windows.Controls.ComboBox comboPaymentPkrAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000039AB File Offset: 0x00001DAB
		// (set) Token: 0x06000322 RID: 802 RVA: 0x000039B5 File Offset: 0x00001DB5
		internal virtual System.Windows.Controls.Button btnPaymentPkrEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000323 RID: 803 RVA: 0x000039BE File Offset: 0x00001DBE
		// (set) Token: 0x06000324 RID: 804 RVA: 0x000039C8 File Offset: 0x00001DC8
		internal virtual Grid TabPaymentAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000039D1 File Offset: 0x00001DD1
		// (set) Token: 0x06000326 RID: 806 RVA: 0x000039DB File Offset: 0x00001DDB
		internal virtual System.Windows.Controls.Button btnNewPayAgentEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000327 RID: 807 RVA: 0x000039E4 File Offset: 0x00001DE4
		// (set) Token: 0x06000328 RID: 808 RVA: 0x000039EE File Offset: 0x00001DEE
		internal virtual System.Windows.Controls.Button btnEditPayAgentEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000329 RID: 809 RVA: 0x000039F7 File Offset: 0x00001DF7
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00003A01 File Offset: 0x00001E01
		internal virtual System.Windows.Controls.DataGrid DataGridPayAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00003A0A File Offset: 0x00001E0A
		// (set) Token: 0x0600032C RID: 812 RVA: 0x00003A14 File Offset: 0x00001E14
		internal virtual Grid TabProfitWithdrawal { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00003A1D File Offset: 0x00001E1D
		// (set) Token: 0x0600032E RID: 814 RVA: 0x00003A27 File Offset: 0x00001E27
		internal virtual StackPanel UniProfitWithdrawalDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00003A30 File Offset: 0x00001E30
		// (set) Token: 0x06000330 RID: 816 RVA: 0x00003A3A File Offset: 0x00001E3A
		internal virtual TextBlock TotalProfit { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00003A43 File Offset: 0x00001E43
		// (set) Token: 0x06000332 RID: 818 RVA: 0x00003A4D File Offset: 0x00001E4D
		internal virtual DatePicker ProfitWithdrawaldatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00003A56 File Offset: 0x00001E56
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00003A60 File Offset: 0x00001E60
		internal virtual System.Windows.Controls.TextBox txtProfitWithdrawalAmount { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00003A69 File Offset: 0x00001E69
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00003A73 File Offset: 0x00001E73
		internal virtual System.Windows.Controls.TextBox txtProfitWithdrawalDetail { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00003A7C File Offset: 0x00001E7C
		// (set) Token: 0x06000338 RID: 824 RVA: 0x00003A86 File Offset: 0x00001E86
		internal virtual System.Windows.Controls.ComboBox comboProfitWithdrawalAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00003A8F File Offset: 0x00001E8F
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00003A99 File Offset: 0x00001E99
		internal virtual System.Windows.Controls.Button btnProfitWithdrawalEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00003AA2 File Offset: 0x00001EA2
		// (set) Token: 0x0600033C RID: 828 RVA: 0x00003AAC File Offset: 0x00001EAC
		internal virtual Grid MainRow { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00003AB5 File Offset: 0x00001EB5
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00003ABF File Offset: 0x00001EBF
		internal virtual DockPanel staData { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00003AC8 File Offset: 0x00001EC8
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00003AD2 File Offset: 0x00001ED2
		internal virtual StackPanel GrpStocksBtns { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00003ADB File Offset: 0x00001EDB
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0001BC64 File Offset: 0x0001A064
		internal virtual System.Windows.Controls.Button BtnAddStocks
		{
			[CompilerGenerated]
			get
			{
				return this._BtnAddStocks;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnAddStocks_Click);
				System.Windows.Controls.Button btnAddStocks = this._BtnAddStocks;
				if (btnAddStocks != null)
				{
					btnAddStocks.Click -= value2;
				}
				this._BtnAddStocks = value;
				btnAddStocks = this._BtnAddStocks;
				if (btnAddStocks != null)
				{
					btnAddStocks.Click += value2;
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00003AE5 File Offset: 0x00001EE5
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0001BCA8 File Offset: 0x0001A0A8
		internal virtual System.Windows.Controls.Button BtnEditStock
		{
			[CompilerGenerated]
			get
			{
				return this._BtnEditStock;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnEditStock_Click);
				System.Windows.Controls.Button btnEditStock = this._BtnEditStock;
				if (btnEditStock != null)
				{
					btnEditStock.Click -= value2;
				}
				this._BtnEditStock = value;
				btnEditStock = this._BtnEditStock;
				if (btnEditStock != null)
				{
					btnEditStock.Click += value2;
				}
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00003AEF File Offset: 0x00001EEF
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0001BCEC File Offset: 0x0001A0EC
		internal virtual System.Windows.Controls.Button BtnDeleteStocks
		{
			[CompilerGenerated]
			get
			{
				return this._BtnDeleteStocks;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnDeleteStocks_Click);
				System.Windows.Controls.Button btnDeleteStocks = this._BtnDeleteStocks;
				if (btnDeleteStocks != null)
				{
					btnDeleteStocks.Click -= value2;
				}
				this._BtnDeleteStocks = value;
				btnDeleteStocks = this._BtnDeleteStocks;
				if (btnDeleteStocks != null)
				{
					btnDeleteStocks.Click += value2;
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00003AF9 File Offset: 0x00001EF9
		// (set) Token: 0x06000348 RID: 840 RVA: 0x00003B03 File Offset: 0x00001F03
		internal virtual System.Windows.Controls.Label lblFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00003B0C File Offset: 0x00001F0C
		// (set) Token: 0x0600034A RID: 842 RVA: 0x00003B16 File Offset: 0x00001F16
		internal virtual System.Windows.Controls.TextBox txtFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00003B1F File Offset: 0x00001F1F
		// (set) Token: 0x0600034C RID: 844 RVA: 0x00003B29 File Offset: 0x00001F29
		internal virtual Grid DomainGrid { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00003B32 File Offset: 0x00001F32
		// (set) Token: 0x0600034E RID: 846 RVA: 0x00003B3C File Offset: 0x00001F3C
		internal virtual System.Windows.Controls.ListView LstStocks { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00003B45 File Offset: 0x00001F45
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00003B4F File Offset: 0x00001F4F
		internal virtual DockPanel StockDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00003B58 File Offset: 0x00001F58
		// (set) Token: 0x06000352 RID: 850 RVA: 0x00003B62 File Offset: 0x00001F62
		internal virtual TextBlock TxtBlkDomainDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00003B6B File Offset: 0x00001F6B
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00003B75 File Offset: 0x00001F75
		internal virtual Run runStockId { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00003B7E File Offset: 0x00001F7E
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00003B88 File Offset: 0x00001F88
		internal virtual Run runStockDAte { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00003B91 File Offset: 0x00001F91
		// (set) Token: 0x06000358 RID: 856 RVA: 0x00003B9B File Offset: 0x00001F9B
		internal virtual Run runStockchassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00003BA4 File Offset: 0x00001FA4
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00003BAE File Offset: 0x00001FAE
		internal virtual Run runStockModel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00003BB7 File Offset: 0x00001FB7
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00003BC1 File Offset: 0x00001FC1
		internal virtual Run runStockColor { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00003BCA File Offset: 0x00001FCA
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00003BD4 File Offset: 0x00001FD4
		internal virtual Run runStockPriceYen { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00003BDD File Offset: 0x00001FDD
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00003BE7 File Offset: 0x00001FE7
		internal virtual Run runStockRate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00003BF0 File Offset: 0x00001FF0
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00003BFA File Offset: 0x00001FFA
		internal virtual Run runStockPricePkr { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00003C03 File Offset: 0x00002003
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00003C0D File Offset: 0x0000200D
		internal virtual Run runStockDuty { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00003C16 File Offset: 0x00002016
		// (set) Token: 0x06000366 RID: 870 RVA: 0x00003C20 File Offset: 0x00002020
		internal virtual Run runStockMiscExpense { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00003C29 File Offset: 0x00002029
		// (set) Token: 0x06000368 RID: 872 RVA: 0x00003C33 File Offset: 0x00002033
		internal virtual Run runStockCost { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00003C3C File Offset: 0x0000203C
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00003C46 File Offset: 0x00002046
		internal virtual Run runStockStatus { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00003C4F File Offset: 0x0000204F
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00003C59 File Offset: 0x00002059
		internal virtual Run runStockAmountPaidYen { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00003C62 File Offset: 0x00002062
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00003C6C File Offset: 0x0000206C
		internal virtual Run runStockAmountPaid { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00003C75 File Offset: 0x00002075
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00003C7F File Offset: 0x0000207F
		internal virtual Run runStockComments { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00003C88 File Offset: 0x00002088
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00003C92 File Offset: 0x00002092
		internal virtual Grid TabCustomer { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00003C9B File Offset: 0x0000209B
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00003CA5 File Offset: 0x000020A5
		internal virtual DockPanel staCustData { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00003CAE File Offset: 0x000020AE
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00003CB8 File Offset: 0x000020B8
		internal virtual StackPanel GrpCustBtns { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00003CC1 File Offset: 0x000020C1
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0001BD30 File Offset: 0x0001A130
		internal virtual System.Windows.Controls.Button BtnAddCust
		{
			[CompilerGenerated]
			get
			{
				return this._BtnAddCust;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnAddCust_Click);
				System.Windows.Controls.Button btnAddCust = this._BtnAddCust;
				if (btnAddCust != null)
				{
					btnAddCust.Click -= value2;
				}
				this._BtnAddCust = value;
				btnAddCust = this._BtnAddCust;
				if (btnAddCust != null)
				{
					btnAddCust.Click += value2;
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00003CCB File Offset: 0x000020CB
		// (set) Token: 0x0600037A RID: 890 RVA: 0x0001BD74 File Offset: 0x0001A174
		internal virtual System.Windows.Controls.Button BtnEditCust
		{
			[CompilerGenerated]
			get
			{
				return this._BtnEditCust;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnEditCust_Click);
				System.Windows.Controls.Button btnEditCust = this._BtnEditCust;
				if (btnEditCust != null)
				{
					btnEditCust.Click -= value2;
				}
				this._BtnEditCust = value;
				btnEditCust = this._BtnEditCust;
				if (btnEditCust != null)
				{
					btnEditCust.Click += value2;
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00003CD5 File Offset: 0x000020D5
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0001BDB8 File Offset: 0x0001A1B8
		internal virtual System.Windows.Controls.Button BtnDeleteCust
		{
			[CompilerGenerated]
			get
			{
				return this._BtnDeleteCust;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.BtnDeleteCust_Click);
				System.Windows.Controls.Button btnDeleteCust = this._BtnDeleteCust;
				if (btnDeleteCust != null)
				{
					btnDeleteCust.Click -= value2;
				}
				this._BtnDeleteCust = value;
				btnDeleteCust = this._BtnDeleteCust;
				if (btnDeleteCust != null)
				{
					btnDeleteCust.Click += value2;
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00003CDF File Offset: 0x000020DF
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00003CE9 File Offset: 0x000020E9
		internal virtual System.Windows.Controls.Label lblCustFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00003CF2 File Offset: 0x000020F2
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00003CFC File Offset: 0x000020FC
		internal virtual System.Windows.Controls.TextBox txtCustFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00003D05 File Offset: 0x00002105
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00003D0F File Offset: 0x0000210F
		internal virtual Grid CustGrid { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00003D18 File Offset: 0x00002118
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00003D22 File Offset: 0x00002122
		internal virtual System.Windows.Controls.ListView LstCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00003D2B File Offset: 0x0000212B
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00003D35 File Offset: 0x00002135
		internal virtual DockPanel CustDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00003D3E File Offset: 0x0000213E
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00003D48 File Offset: 0x00002148
		internal virtual TextBlock TxtBlkCustDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00003D51 File Offset: 0x00002151
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00003D5B File Offset: 0x0000215B
		internal virtual Run RunCustId { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00003D64 File Offset: 0x00002164
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00003D6E File Offset: 0x0000216E
		internal virtual Run RunCustDate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00003D77 File Offset: 0x00002177
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00003D81 File Offset: 0x00002181
		internal virtual Run RunCustTitle { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00003D8A File Offset: 0x0000218A
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00003D94 File Offset: 0x00002194
		internal virtual Run RunCustName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00003D9D File Offset: 0x0000219D
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00003DA7 File Offset: 0x000021A7
		internal virtual Run RunCustCNIC { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00003DB0 File Offset: 0x000021B0
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00003DBA File Offset: 0x000021BA
		internal virtual Run RunCustPhone { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00003DC3 File Offset: 0x000021C3
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00003DCD File Offset: 0x000021CD
		internal virtual Run RunCustAddress { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00003DD6 File Offset: 0x000021D6
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00003DE0 File Offset: 0x000021E0
		internal virtual Run runCustRecPay { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00003DE9 File Offset: 0x000021E9
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00003DF3 File Offset: 0x000021F3
		internal virtual Run RunCustPaymentReceivable { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00003DFC File Offset: 0x000021FC
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00003E06 File Offset: 0x00002206
		internal virtual Grid TabAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00003E0F File Offset: 0x0000220F
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00003E19 File Offset: 0x00002219
		internal virtual DockPanel staAgentData { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00003E22 File Offset: 0x00002222
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00003E2C File Offset: 0x0000222C
		internal virtual StackPanel GrpAgentBtns { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00003E35 File Offset: 0x00002235
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00003E3F File Offset: 0x0000223F
		internal virtual System.Windows.Controls.Button BtnAddAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00003E48 File Offset: 0x00002248
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00003E52 File Offset: 0x00002252
		internal virtual System.Windows.Controls.Button BtnEditAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00003E5B File Offset: 0x0000225B
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00003E65 File Offset: 0x00002265
		internal virtual System.Windows.Controls.Label lblAgentFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00003E6E File Offset: 0x0000226E
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00003E78 File Offset: 0x00002278
		internal virtual System.Windows.Controls.TextBox txtAgentFilter { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00003E81 File Offset: 0x00002281
		// (set) Token: 0x060003AA RID: 938 RVA: 0x00003E8B File Offset: 0x0000228B
		internal virtual Grid AgentGrid { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00003E94 File Offset: 0x00002294
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00003E9E File Offset: 0x0000229E
		internal virtual System.Windows.Controls.ListView LstAgent { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00003EA7 File Offset: 0x000022A7
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00003EB1 File Offset: 0x000022B1
		internal virtual DockPanel AgentDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00003EBA File Offset: 0x000022BA
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x00003EC4 File Offset: 0x000022C4
		internal virtual TextBlock TxtBlkAgentDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00003ECD File Offset: 0x000022CD
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x00003ED7 File Offset: 0x000022D7
		internal virtual Run RunAgentId { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00003EE0 File Offset: 0x000022E0
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00003EEA File Offset: 0x000022EA
		internal virtual Run RunAgentDate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00003EF3 File Offset: 0x000022F3
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00003EFD File Offset: 0x000022FD
		internal virtual Run RunAgentName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00003F06 File Offset: 0x00002306
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00003F10 File Offset: 0x00002310
		internal virtual Run RunAgentCNIC { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00003F19 File Offset: 0x00002319
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00003F23 File Offset: 0x00002323
		internal virtual Run RunAgentPhone { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00003F2C File Offset: 0x0000232C
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00003F36 File Offset: 0x00002336
		internal virtual Run RunAgentAddress { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00003F3F File Offset: 0x0000233F
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00003F49 File Offset: 0x00002349
		internal virtual Run runAgentRecPay { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00003F52 File Offset: 0x00002352
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00003F5C File Offset: 0x0000235C
		internal virtual Run RunAgentPaymentReceivable { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00003F65 File Offset: 0x00002365
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00003F6F File Offset: 0x0000236F
		internal virtual Grid TabSale { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00003F78 File Offset: 0x00002378
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00003F82 File Offset: 0x00002382
		internal virtual System.Windows.Controls.Button btnNewSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00003F8B File Offset: 0x0000238B
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x00003F95 File Offset: 0x00002395
		internal virtual System.Windows.Controls.Button btnEditSaleEntry { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00003F9E File Offset: 0x0000239E
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x00003FA8 File Offset: 0x000023A8
		internal virtual System.Windows.Controls.DataGrid DataGridSales { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00003FB1 File Offset: 0x000023B1
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00003FBB File Offset: 0x000023BB
		internal virtual UniformGrid UniDetails { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00003FC4 File Offset: 0x000023C4
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00003FCE File Offset: 0x000023CE
		internal virtual System.Windows.Controls.ComboBox comboChassis { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00003FD7 File Offset: 0x000023D7
		// (set) Token: 0x060003CE RID: 974 RVA: 0x00003FE1 File Offset: 0x000023E1
		internal virtual System.Windows.Controls.ComboBox comboCust { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00003FEA File Offset: 0x000023EA
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x00003FF4 File Offset: 0x000023F4
		internal virtual System.Windows.Controls.TextBox txtSalePrice { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00003FFD File Offset: 0x000023FD
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x00004007 File Offset: 0x00002407
		internal virtual IntegerUpDown txtAmountReceived { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00004010 File Offset: 0x00002410
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0000401A File Offset: 0x0000241A
		internal virtual System.Windows.Controls.ComboBox comboSaleAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00004023 File Offset: 0x00002423
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000402D File Offset: 0x0000242D
		internal virtual DatePicker SaleDatePicker { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00004036 File Offset: 0x00002436
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0001BDFC File Offset: 0x0001A1FC
		internal virtual System.Windows.Controls.Button btnSaleEntry
		{
			[CompilerGenerated]
			get
			{
				return this._btnSaleEntry;
			}
			[CompilerGenerated]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				RoutedEventHandler value2 = new RoutedEventHandler(this.btnSaleEntry_Click);
				System.Windows.Controls.Button btnSaleEntry = this._btnSaleEntry;
				if (btnSaleEntry != null)
				{
					btnSaleEntry.Click -= value2;
				}
				this._btnSaleEntry = value;
				btnSaleEntry = this._btnSaleEntry;
				if (btnSaleEntry != null)
				{
					btnSaleEntry.Click += value2;
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00004040 File Offset: 0x00002440
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0000404A File Offset: 0x0000244A
		internal virtual System.Windows.Controls.ComboBox comboReport { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00004053 File Offset: 0x00002453
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0000405D File Offset: 0x0000245D
		internal virtual System.Windows.Controls.ComboBox comboReportAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00004066 File Offset: 0x00002466
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00004070 File Offset: 0x00002470
		internal virtual System.Windows.Controls.ComboBox comboReportCustomers { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00004079 File Offset: 0x00002479
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00004083 File Offset: 0x00002483
		internal virtual System.Windows.Controls.ComboBox comboReportDutyAgents { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000408C File Offset: 0x0000248C
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x00004096 File Offset: 0x00002496
		internal virtual System.Windows.Controls.ComboBox comboReportStockDuty { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000409F File Offset: 0x0000249F
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x000040A9 File Offset: 0x000024A9
		internal virtual WrapPanel reportDate { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000040B2 File Offset: 0x000024B2
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x000040BC File Offset: 0x000024BC
		internal virtual DatePicker reportDateFrom { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x000040C5 File Offset: 0x000024C5
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x000040CF File Offset: 0x000024CF
		internal virtual DatePicker reportDateTo { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x000040D8 File Offset: 0x000024D8
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x000040E2 File Offset: 0x000024E2
		internal virtual System.Windows.Controls.Button btnGenerateReport { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000040EB File Offset: 0x000024EB
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x000040F5 File Offset: 0x000024F5
		internal virtual System.Windows.Controls.Button btnClosing { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000040FE File Offset: 0x000024FE
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x00004108 File Offset: 0x00002508
		internal virtual DocumentViewer documentViewer2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00004111 File Offset: 0x00002511
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0000411B File Offset: 0x0000251B
		internal virtual FlowDocumentScrollViewer flowviewertrial { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00004124 File Offset: 0x00002524
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x0000412E File Offset: 0x0000252E
		internal virtual FlowDocument flowTrialBalance { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00004137 File Offset: 0x00002537
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00004141 File Offset: 0x00002541
		internal virtual Run runT { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000414A File Offset: 0x0000254A
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00004154 File Offset: 0x00002554
		internal virtual Run runD { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000415D File Offset: 0x0000255D
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00004167 File Offset: 0x00002567
		internal virtual Table TableTrialBalance { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00004170 File Offset: 0x00002570
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x0000417A File Offset: 0x0000257A
		internal virtual TableRowGroup trg1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x040000B9 RID: 185
		private string FileName;

		// Token: 0x040000BA RID: 186
		public static string StrDataFile = MySettingsProperty.Settings.StrDataFile;

		// Token: 0x040000BB RID: 187
		public static DataSet DSet;

		// Token: 0x040000BC RID: 188
		public static SQLiteDataAdapter DataAdapter;

		// Token: 0x040000BD RID: 189
		public static SQLiteCommandBuilder CmdBuilder;

		// Token: 0x040000BE RID: 190
		public static DataTable dtable;

		// Token: 0x040000BF RID: 191
		private bool analyzeenable;

		// Token: 0x040000C0 RID: 192
		private bool EditCommited;

		// Token: 0x040000C1 RID: 193
		public static SQLiteConnection Connection = new SQLiteConnection();

		// Token: 0x040000C2 RID: 194
		private bool loadedbool;

		// Token: 0x040000C3 RID: 195
		private bool boolCopied;

		// Token: 0x040000C4 RID: 196
		public double closingProfit;

		// Token: 0x040000C5 RID: 197
		public int payableYens;
	}
}
