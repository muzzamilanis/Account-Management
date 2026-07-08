using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AMS.Services;
using AMS.ViewModels;
using AMS.Views.Dialogs;
using Microsoft.Win32;

namespace AMS.Views
{
    public partial class MainWindow : Window
    {
        private readonly AccountsViewModel _accVm = new AccountsViewModel();
        private readonly StocksViewModel _stocksVm = new StocksViewModel();
        private readonly CustomersViewModel _custVm = new CustomersViewModel();
        private readonly AgentsViewModel _agentsVm = new AgentsViewModel();
        private readonly SaleViewModel _saleVm = new SaleViewModel();
        private readonly ReportViewModel _reportVm = new ReportViewModel();

        private MainViewModel VM => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            // Bind lists to ViewModels
            LstAccounts.ItemsSource = _accVm.Accounts;
            GridTransfers.ItemsSource = _accVm.Transfers;
            GridMiscExp.ItemsSource = _accVm.MiscExps;
            GridDutyExp.ItemsSource = _accVm.DutyExps;
            GridOfficeExp.ItemsSource = _accVm.OfficeExps;
            GridReceipts.ItemsSource = _accVm.Receipts;
            GridYenPayments.ItemsSource = _accVm.YenPayments;
            GridPkrPayments.ItemsSource = _accVm.PkrPayments;
            GridAgentPayments.ItemsSource = _accVm.AgentPayments;
            LstStocks.ItemsSource = _stocksVm.Stocks;
            LstCust.ItemsSource = _custVm.Customers;
            LstAgent.ItemsSource = _agentsVm.Agents;
            GridSales.ItemsSource = _saleVm.Sales;

            // Reports
            var reportTypes = ReportViewModel.ReportTypes;
            ComboReport.ItemsSource = reportTypes;
            ComboReport.SelectedIndex = 0;
            DpReportTo.SelectedDate = DateTime.Today;
        }

        // ─────────────────────────────────── Database ────────────────────────────────
        private void BtnCreateDb_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Title = "Create New Database",
                Filter = "AMS Database (*.bndb)|*.bndb|All Files (*.*)|*.*",
                DefaultExt = "amsdb",
                FileName = "accounts.bndb"
            };
            if (dlg.ShowDialog() != true) return;
            try
            {
                DatabaseService.Instance.CreateDatabase(dlg.FileName);
                SettingsService.Instance.LastDatabasePath = dlg.FileName;
                TxtDbStatus.Text = $"✓ Created: {Path.GetFileName(dlg.FileName)}";
                TxtDbStatus.Visibility = Visibility.Visible;
                TxtStatus.Text = $"Database created: {dlg.FileName}";
                VM.RefreshDbState();
                RefreshAll();
                MessageBox.Show($"Database created successfully!\n{dlg.FileName}", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating database:\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOpenDb_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Open AMS Database",
                Filter = "AMS Database (*.bndb)|*.bndb|SQLite DB (*.db;*.sqlite)|*.db;*.sqlite|All Files (*.*)|*.*",
                Multiselect = false
            };
            if (dlg.ShowDialog() != true) return;
            try
            {
                DatabaseService.Instance.OpenDatabase(dlg.FileName);
                SettingsService.Instance.LastDatabasePath = dlg.FileName;
                TxtDbStatus.Text = $"✓ Opened: {Path.GetFileName(dlg.FileName)}";
                TxtDbStatus.Visibility = Visibility.Visible;
                TxtStatus.Text = $"Database opened: {dlg.FileName}";
                VM.RefreshDbState();
                RefreshAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening database:\n{ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshAll()
        {
            _accVm.LoadAll();
            _stocksVm.Load();
            _custVm.Load();
            _agentsVm.Load();
            _saleVm.Load();
            // Profit
            TxtTotalProfit.Text = _accVm.TotalProfit.ToString("N2") + " PKR";
            // Profit accounts combo
            ComboProfitAccount.ItemsSource = DatabaseService.Instance.GetAccountNames();
            if (ComboProfitAccount.Items.Count > 0) ComboProfitAccount.SelectedIndex = 0;
            // Payment reminders (Welcome page)
            var reminders = _saleVm.Sales.Where(s => s.HasActiveReminder).OrderBy(s => s.DaysUntilDue).ToList();
            LstReminders.ItemsSource = reminders;
            PnlReminders.Visibility = reminders.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GuardDb()
        {
            if (!DatabaseService.Instance.IsConnected)
                throw new InvalidOperationException("Please open or create a database first.");
        }

        // ─────────────────────────────────── Accounts ────────────────────────────────
        private void BtnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new AddAccountDialog(); if (d.ShowDialog() == true) _accVm.LoadAccounts(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Info", MessageBoxButton.OK, MessageBoxImage.Information); }
        }

        private void BtnEditAccount_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (LstAccounts.SelectedItem is Models.Account a) { var d = new AddAccountDialog(a); if (d.ShowDialog() == true) _accVm.LoadAccounts(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void LstAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => BtnEditAccount.IsEnabled = LstAccounts.SelectedItem != null;

        private void TxtAccFilter_TextChanged(object sender, TextChangedEventArgs e)
            => _accVm.FilterText = TxtAccFilter.Text;

        private void BtnNewTransfer_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new AccountTransferDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); RefreshAll(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewMiscExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new MiscExpDialog(); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewDutyExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new DutyExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _agentsVm.Load(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewOfficeExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new OfficeExpDialog(); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewReceipt_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new ReceiptDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewYenPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PaymentPkrDialog(isYen: true); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewPkrPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PaymentPkrDialog(isYen: false); if (d.ShowDialog() == true) { _accVm.LoadAll(); _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewAgentPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PaymentAgentDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _agentsVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnWithdrawProfit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!double.TryParse(TxtWithdrawAmount.Text, out double amount) || amount <= 0)
                { MessageBox.Show("Enter a valid withdrawal amount."); return; }
                string account = ComboProfitAccount.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(account)) { MessageBox.Show("Select an account."); return; }
                DatabaseService.Instance.WithdrawProfit(amount, account, "Profit withdrawal");
                TxtWithdrawAmount.Clear();
                _accVm.LoadAll();
                TxtTotalProfit.Text = _accVm.TotalProfit.ToString("N2") + " PKR";
                MessageBox.Show($"PKR {amount:N2} withdrawn from profit.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // ─────────────────────────────────── Stocks ────────────────────────────────
        private void BtnAddStock_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PurchaseAutoDialog(); if (d.ShowDialog() == true) RefreshAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEditStock_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (LstStocks.SelectedItem is Models.Stock s) { var d = new PurchaseAutoDialog(s); if (d.ShowDialog() == true) RefreshAll(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void LstStocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => BtnEditStock.IsEnabled = LstStocks.SelectedItem != null;

        private void TxtStockFilter_TextChanged(object sender, TextChangedEventArgs e)
            => _stocksVm.FilterText = TxtStockFilter.Text;

        // ─────────────────────────────────── Customers ────────────────────────────────
        private void BtnAddCust_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new AddCustomerDialog(); if (d.ShowDialog() == true) _custVm.Load(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEditCust_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (LstCust.SelectedItem is Models.Customer c) { var d = new AddCustomerDialog(c); if (d.ShowDialog() == true) _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void LstCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => BtnEditCust.IsEnabled = LstCust.SelectedItem != null;

        private void TxtCustFilter_TextChanged(object sender, TextChangedEventArgs e)
            => _custVm.FilterText = TxtCustFilter.Text;

        // ─────────────────────────────────── Agents ────────────────────────────────
        private void BtnAddAgent_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new AddAgentDialog(); if (d.ShowDialog() == true) _agentsVm.Load(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEditAgent_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (LstAgent.SelectedItem is Models.Agent a) { var d = new AddAgentDialog(a); if (d.ShowDialog() == true) _agentsVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void LstAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => BtnEditAgent.IsEnabled = LstAgent.SelectedItem != null;

        private void TxtAgentFilter_TextChanged(object sender, TextChangedEventArgs e)
            => _agentsVm.FilterText = TxtAgentFilter.Text;

        // ─────────────────────────────────── Sales ────────────────────────────────
        private void BtnNewSale_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new SaleAutoDialog(); if (d.ShowDialog() == true) RefreshAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnRefreshSales_Click(object sender, RoutedEventArgs e) => _saleVm.Load();

        // ─────────────────────────────────── Reports ────────────────────────────────
        private void ComboReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isAccountStatement = ComboReport.SelectedItem?.ToString() == "Account Statement";
            TxtReportAccountLabel.Visibility = isAccountStatement ? Visibility.Visible : Visibility.Collapsed;
            ComboReportAccount.Visibility = isAccountStatement ? Visibility.Visible : Visibility.Collapsed;
            if (isAccountStatement && DatabaseService.Instance.IsConnected && ComboReportAccount.ItemsSource == null)
            {
                ComboReportAccount.ItemsSource = DatabaseService.Instance.GetAccountNames();
                ComboReportAccount.SelectedIndex = 0;
            }
        }

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                string reportType = ComboReport.SelectedItem?.ToString();
                DateTime from = DpReportFrom.SelectedDate ?? new DateTime(2017, 1, 1);
                DateTime to = DpReportTo.SelectedDate ?? DateTime.Today;
                DataTable data;
                if (reportType == "Account Statement")
                {
                    string account = ComboReportAccount.SelectedItem?.ToString();
                    if (string.IsNullOrEmpty(account)) { MessageBox.Show("Select an account."); return; }
                    data = DatabaseService.Instance.GetAccountStatement(account, from, to);
                    reportType = $"Account Statement: {account}";
                }
                else
                {
                    data = DatabaseService.Instance.GetReportData(reportType, from, to);
                }
                string company = SettingsService.Instance.CompanyName;
                var doc = ReportService.Instance.BuildReport(reportType, company, data);
                ReportViewer.Document = doc;
                BtnPrint.IsEnabled = true;

                // Show viewer, hide empty state
                PnlReportEmpty.Visibility = Visibility.Collapsed;
                PnlReportViewer.Visibility = Visibility.Visible;

                // Update header labels
                TxtReportTitle.Text = reportType;
                TxtReportCount.Text = $"({data.Rows.Count} record{(data.Rows.Count != 1 ? "s" : "")})"; 
                TxtReportDate.Text = $"Generated: {DateTime.Now:dd MMM yyyy  HH:mm}";

                TxtStatus.Text = $"Report generated: {reportType} ({data.Rows.Count} records)";
            }
            catch (Exception ex) { MessageBox.Show($"Error generating report:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReportViewer.Document == null) return;
                var pd = new System.Windows.Controls.PrintDialog();
                if (pd.ShowDialog() == true)
                {
                    System.Windows.Documents.IDocumentPaginatorSource idps = ReportViewer.Document;
                    pd.PrintDocument(idps.DocumentPaginator, ComboReport.SelectedItem?.ToString() ?? "Report");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void hyplnk_Click(object sender, RoutedEventArgs e)
        {
            WrapExcEdit.Visibility = Visibility.Visible;
            PnlExcView.Visibility = Visibility.Collapsed;
        }

        private void btnExcEdit_Click(object sender, RoutedEventArgs e)
        {
            // Explicitly force updating the bound ExchangeRate source
            var bindingObj = txtEditExcRate.GetBindingExpression(TextBox.TextProperty);
            if (bindingObj != null)
                bindingObj.UpdateSource();

            WrapExcEdit.Visibility = Visibility.Collapsed;
            PnlExcView.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) e.Cancel = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            DatabaseService.Instance.CloseConnection();
            base.OnClosed(e);
        }
    }
}
