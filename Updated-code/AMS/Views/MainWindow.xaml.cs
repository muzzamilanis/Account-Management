using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            GridDemurrageExp.ItemsSource = _accVm.DemurrageExps;
            GridNoPlateExp.ItemsSource = _accVm.NoPlateExps;
            GridCommissionExp.ItemsSource = _accVm.CommissionExps;
            GridTaxExp.ItemsSource = _accVm.TaxExps;
            GridOfficeExp.ItemsSource = _accVm.OfficeExps;
            GridReceipts.ItemsSource = _accVm.Receipts;
            GridYenPayments.ItemsSource = _accVm.YenPayments;
            GridPkrPayments.ItemsSource = _accVm.PkrPayments;
            GridAgentPayments.ItemsSource = _accVm.AgentPayments;
            LstStocks.ItemsSource = _stocksVm.Stocks;
            LstCust.ItemsSource = _custVm.Customers;
            LstAgent.ItemsSource = _agentsVm.Agents;
            GridSales.ItemsSource = _saleVm.Sales;

            // Currency-dependent labels that can't be data-bound (static DataGrid column headers,
            // a label bound to a plain POCO with no ViewModel of its own)
            ColYenPaymentAmountPkr.Header = $"Amount ({CurrencyLabel.Symbol})";
            TxtStockPricePkrLabel.Text = $"Price ({CurrencyLabel.Symbol}):";

            // Reports
            RefreshReportTypesList();
            DpReportTo.SelectedDate = DateTime.Today;

            // Auto-load the last opened database, if any
            string lastPath = SettingsService.Instance.LastDatabasePath;
            if (!string.IsNullOrEmpty(lastPath) && File.Exists(lastPath))
            {
                try
                {
                    DatabaseService.Instance.OpenDatabase(lastPath);
                    TxtDbStatus.Text = $"✓ Opened: {Path.GetFileName(lastPath)}";
                    TxtDbStatus.Visibility = Visibility.Visible;
                    TxtStatus.Text = $"Database opened: {lastPath}";
                    VM.RefreshDbState();
                    RefreshAll();
                }
                catch
                {
                    // Fall back to manual open/create from the Welcome page
                }
            }
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
            TxtTotalProfit.Text = _accVm.TotalProfit.ToString("N2") + " " + CurrencyLabel.Symbol;
            // Profit accounts combo
            ComboProfitAccount.ItemsSource = DatabaseService.Instance.GetAccountNames();
            if (ComboProfitAccount.Items.Count > 0) ComboProfitAccount.SelectedIndex = 0;
            // Payment reminders (Welcome page) — hidden entirely when credit sales are disabled
            bool creditEnabled = SettingsService.Instance.Settings.EnableCreditSales;
            var reminders = creditEnabled
                ? DatabaseService.Instance.GetUnpaidInstallments().Where(i => i.HasActiveReminder).OrderBy(i => i.DaysUntilDue).ToList()
                : new System.Collections.Generic.List<Models.Installment>();
            LstReminders.ItemsSource = reminders;
            PnlReminders.Visibility = reminders.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RefreshReportTypesList()
        {
            string previous = ComboReport.SelectedItem?.ToString();
            bool creditEnabled = SettingsService.Instance.Settings.EnableCreditSales;
            var types = creditEnabled
                ? ReportViewModel.ReportTypes
                : ReportViewModel.ReportTypes.Where(t => t != "Active Credit Sales").ToArray();
            ComboReport.ItemsSource = types;
            int idx = Array.IndexOf(types, previous);
            ComboReport.SelectedIndex = idx >= 0 ? idx : 0;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            var win = new AMS.Views.CompanySettingsWindow();
            win.ShowDialog();
            VM.CompanyName = SettingsService.Instance.CompanyName;
            RefreshReportTypesList();
            RefreshAll();
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
        {
            bool hasSelection = LstAccounts.SelectedItem != null;
            BtnEditAccount.IsEnabled = hasSelection;
            BtnDeleteAccount.IsEnabled = hasSelection;
        }

        private void LstAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LstAccounts.SelectedItem is Models.Account) BtnEditAccount_Click(sender, new RoutedEventArgs());
        }

        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!(LstAccounts.SelectedItem is Models.Account a)) return;
                if (a.CurrentBalance != 0)
                {
                    MessageBox.Show($"This account has a non-zero balance ({a.CurrentBalance:N2}). Transfer or clear the balance before deleting it.", "Cannot Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var dlg = new ConfirmDialog($"Delete account \"{a.AccountName}\"? This cannot be undone.", "Delete Account") { Owner = this };
                if (dlg.ShowDialog() != true) return;
                DatabaseService.Instance.DeleteAccount(a.RowId);
                _accVm.LoadAccounts();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void TxtAccFilter_TextChanged(object sender, TextChangedEventArgs e)
            => _accVm.FilterText = TxtAccFilter.Text;

        private void BtnNewTransfer_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new AccountTransferDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); RefreshAll(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridTransfers_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditTransfer.IsEnabled = GridTransfers.SelectedItem != null;
        private void GridTransfers_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridTransfers.SelectedItem != null) BtnEditTransfer_Click(sender, new RoutedEventArgs()); }
        private void BtnEditTransfer_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridTransfers.SelectedItem is Models.OfficeAccount t)) return; var d = new AccountTransferDialog(t); if (d.ShowDialog() == true) { _accVm.LoadAll(); RefreshAll(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewMiscExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new MiscExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridMiscExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditMiscExp.IsEnabled = GridMiscExp.SelectedItem != null;
        private void GridMiscExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridMiscExp.SelectedItem != null) BtnEditMiscExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditMiscExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridMiscExp.SelectedItem is Models.MiscExp m)) return; var d = new MiscExpDialog(m); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewDutyExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new DutyExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _agentsVm.Load(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridDutyExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditDutyExp.IsEnabled = GridDutyExp.SelectedItem != null;
        private void GridDutyExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridDutyExp.SelectedItem != null) BtnEditDutyExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditDutyExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridDutyExp.SelectedItem is Models.DutyExp d0)) return; var d = new DutyExpDialog(d0); if (d.ShowDialog() == true) { _accVm.LoadAll(); _agentsVm.Load(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewDemurrageExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new DemurrageExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridDemurrageExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditDemurrageExp.IsEnabled = GridDemurrageExp.SelectedItem != null;
        private void GridDemurrageExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridDemurrageExp.SelectedItem != null) BtnEditDemurrageExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditDemurrageExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridDemurrageExp.SelectedItem is Models.DemurrageExp m)) return; var d = new DemurrageExpDialog(m); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewNoPlateExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new NoPlateExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridNoPlateExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditNoPlateExp.IsEnabled = GridNoPlateExp.SelectedItem != null;
        private void GridNoPlateExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridNoPlateExp.SelectedItem != null) BtnEditNoPlateExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditNoPlateExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridNoPlateExp.SelectedItem is Models.NoPlateExp m)) return; var d = new NoPlateExpDialog(m); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewCommissionExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new CommissionExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridCommissionExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditCommissionExp.IsEnabled = GridCommissionExp.SelectedItem != null;
        private void GridCommissionExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridCommissionExp.SelectedItem != null) BtnEditCommissionExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditCommissionExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridCommissionExp.SelectedItem is Models.CommissionExp m)) return; var d = new CommissionExpDialog(m); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewTaxExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new TaxExpDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridTaxExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditTaxExp.IsEnabled = GridTaxExp.SelectedItem != null;
        private void GridTaxExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridTaxExp.SelectedItem != null) BtnEditTaxExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditTaxExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridTaxExp.SelectedItem is Models.TaxExp m)) return; var d = new TaxExpDialog(m); if (d.ShowDialog() == true) { _accVm.LoadAll(); _stocksVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewOfficeExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new OfficeExpDialog(); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridOfficeExp_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditOfficeExp.IsEnabled = GridOfficeExp.SelectedItem != null;
        private void GridOfficeExp_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridOfficeExp.SelectedItem != null) BtnEditOfficeExp_Click(sender, new RoutedEventArgs()); }
        private void BtnEditOfficeExp_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridOfficeExp.SelectedItem is Models.OfficeExp m)) return; var d = new OfficeExpDialog(m); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewReceipt_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new ReceiptDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridReceipts_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditReceipt.IsEnabled = GridReceipts.SelectedItem != null;
        private void GridReceipts_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridReceipts.SelectedItem != null) BtnEditReceipt_Click(sender, new RoutedEventArgs()); }
        private void BtnEditReceipt_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridReceipts.SelectedItem is Models.Receipt r)) return; var d = new ReceiptDialog(r); if (d.ShowDialog() == true) { _accVm.LoadAll(); _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewYenPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PaymentPkrDialog(isYen: true); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridYenPayments_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditYenPayment.IsEnabled = GridYenPayments.SelectedItem != null;
        private void GridYenPayments_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridYenPayments.SelectedItem != null) BtnEditYenPayment_Click(sender, new RoutedEventArgs()); }
        private void BtnEditYenPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridYenPayments.SelectedItem is Models.Payment p)) return; var d = new PaymentPkrDialog(p); if (d.ShowDialog() == true) _accVm.LoadAll(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewPkrPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PaymentPkrDialog(isYen: false); if (d.ShowDialog() == true) { _accVm.LoadAll(); _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridPkrPayments_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditPkrPayment.IsEnabled = GridPkrPayments.SelectedItem != null;
        private void GridPkrPayments_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridPkrPayments.SelectedItem != null) BtnEditPkrPayment_Click(sender, new RoutedEventArgs()); }
        private void BtnEditPkrPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridPkrPayments.SelectedItem is Models.PaymentPkr p)) return; var d = new PaymentPkrDialog(p); if (d.ShowDialog() == true) { _accVm.LoadAll(); _custVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnNewAgentPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); var d = new PaymentAgentDialog(); if (d.ShowDialog() == true) { _accVm.LoadAll(); _agentsVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void GridAgentPayments_SelectionChanged(object sender, SelectionChangedEventArgs e) => BtnEditAgentPayment.IsEnabled = GridAgentPayments.SelectedItem != null;
        private void GridAgentPayments_MouseDoubleClick(object sender, MouseButtonEventArgs e) { if (GridAgentPayments.SelectedItem != null) BtnEditAgentPayment_Click(sender, new RoutedEventArgs()); }
        private void BtnEditAgentPayment_Click(object sender, RoutedEventArgs e)
        {
            try { GuardDb(); if (!(GridAgentPayments.SelectedItem is Models.PaymentAgent p)) return; var d = new PaymentAgentDialog(p); if (d.ShowDialog() == true) { _accVm.LoadAll(); _agentsVm.Load(); } }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnWithdrawProfit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                double amount = TxtWithdrawAmount.Value;
                if (amount <= 0)
                { MessageBox.Show("Enter a valid withdrawal amount."); return; }
                string account = ComboProfitAccount.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(account)) { MessageBox.Show("Select an account."); return; }
                DatabaseService.Instance.WithdrawProfit(amount, account, "Profit withdrawal");
                TxtWithdrawAmount.Value = 0;
                _accVm.LoadAll();
                TxtTotalProfit.Text = _accVm.TotalProfit.ToString("N2") + " " + CurrencyLabel.Symbol;
                MessageBox.Show($"{CurrencyLabel.Symbol} {amount:N2} withdrawn from profit.", "Success",
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

        private void GridSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasPlan = SettingsService.Instance.Settings.EnableCreditSales
                && GridSales.SelectedItem is Models.Sale s && s.InstallmentMonths > 0;
            BtnPayInstallment.IsEnabled = hasPlan;
            BtnEditPlan.IsEnabled = hasPlan;
            BtnEditSale.IsEnabled = GridSales.SelectedItem != null;
        }

        private void GridSales_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridSales.SelectedItem != null) BtnEditSale_Click(sender, new RoutedEventArgs());
        }

        private void BtnEditSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!(GridSales.SelectedItem is Models.Sale sale)) return;
                // Check for a real installment plan, not just the InstallmentMonths flag — a sale
                // can end up flagged as installment with zero actual Installment rows if the
                // balance came out <= 0 at save time (AddInstallmentPlan silently no-ops then),
                // and blocking edit on the flag alone would leave a sale like that permanently
                // stuck with no way to fix it.
                if (DatabaseService.Instance.GetInstallmentsForSale(sale.RowId).Count > 0)
                {
                    MessageBox.Show("Installment sales can't be edited directly — use \"Edit Plan\" to change the remaining schedule, or \"Pay Installment\" to record a payment.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                var d = new SaleAutoDialog(sale);
                if (d.ShowDialog() == true) RefreshAll();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnPayInstallment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!(GridSales.SelectedItem is Models.Sale sale)) return;
                var next = DatabaseService.Instance.GetNextUnpaidInstallment(sale.RowId);
                if (next == null) { MessageBox.Show("This sale has no pending installments.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
                var d = new PayInstallmentDialog(next);
                if (d.ShowDialog() == true) RefreshAll();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEditPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!(GridSales.SelectedItem is Models.Sale sale)) return;
                var d = new EditInstallmentPlanDialog(sale);
                if (d.ShowDialog() == true) RefreshAll();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Same Pay/Edit Plan actions as the Sale Autos tab, exposed directly on the Welcome-page
        // reminder row so a user paying a customer at the counter doesn't have to go hunt for the
        // matching sale first.
        private void BtnPayReminder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!(((FrameworkElement)sender).DataContext is Models.Installment installment)) return;
                var d = new PayInstallmentDialog(installment);
                if (d.ShowDialog() == true) RefreshAll();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEditReminderPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardDb();
                if (!(((FrameworkElement)sender).DataContext is Models.Installment installment)) return;
                var sale = DatabaseService.Instance.GetSales().FirstOrDefault(s => s.RowId == installment.SaleRowId);
                if (sale == null) return;
                var d = new EditInstallmentPlanDialog(sale);
                if (d.ShowDialog() == true) RefreshAll();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // ─────────────────────────────────── Reports ────────────────────────────────
        private void ComboReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ComboReport.SelectedItem?.ToString();
            bool isAccountStatement = selected == "Account Statement";
            TxtReportAccountLabel.Visibility = isAccountStatement ? Visibility.Visible : Visibility.Collapsed;
            ComboReportAccount.Visibility = isAccountStatement ? Visibility.Visible : Visibility.Collapsed;
            if (isAccountStatement && DatabaseService.Instance.IsConnected && ComboReportAccount.ItemsSource == null)
            {
                ComboReportAccount.ItemsSource = DatabaseService.Instance.GetAccountNames();
                ComboReportAccount.SelectedIndex = 0;
            }

            bool isSoldCars = selected == "Sold Cars";
            bool creditEnabled = SettingsService.Instance.Settings.EnableCreditSales;
            bool showSaleType = isSoldCars && creditEnabled;
            TxtSaleTypeLabel.Visibility = showSaleType ? Visibility.Visible : Visibility.Collapsed;
            ComboSaleType.Visibility = showSaleType ? Visibility.Visible : Visibility.Collapsed;

            // Profit Breakdown covers the whole database (lifetime cash collected vs. cost),
            // not a date range, so the From/To pickers don't apply to it.
            bool needsDateRange = selected != "Profit Breakdown";
            TxtReportFromLabel.Visibility = needsDateRange ? Visibility.Visible : Visibility.Collapsed;
            DpReportFrom.Visibility = needsDateRange ? Visibility.Visible : Visibility.Collapsed;
            TxtReportToLabel.Visibility = needsDateRange ? Visibility.Visible : Visibility.Collapsed;
            DpReportTo.Visibility = needsDateRange ? Visibility.Visible : Visibility.Collapsed;
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
                else if (reportType == "Active Credit Sales")
                {
                    data = DatabaseService.Instance.GetActiveCreditSales();
                }
                else if (reportType == "Profit Breakdown")
                {
                    data = DatabaseService.Instance.GetProfitBreakdown();
                }
                else if (reportType == "Sold Cars")
                {
                    string saleType = (ComboSaleType.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    data = DatabaseService.Instance.GetReportData(reportType, from, to, saleType == "All" ? null : saleType);
                    if (!string.IsNullOrEmpty(saleType) && saleType != "All") reportType = $"Sold Cars ({saleType})";
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

        private void hyplnkUgx_Click(object sender, RoutedEventArgs e)
        {
            WrapUgxEdit.Visibility = Visibility.Visible;
            PnlUgxView.Visibility = Visibility.Collapsed;
        }

        private void btnUgxEdit_Click(object sender, RoutedEventArgs e)
        {
            var bindingObj = txtEditUgxRate.GetBindingExpression(TextBox.TextProperty);
            if (bindingObj != null)
                bindingObj.UpdateSource();

            WrapUgxEdit.Visibility = Visibility.Collapsed;
            PnlUgxView.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlg = new ConfirmDialog("Are you sure you want to exit?", "Confirm Exit") { Owner = this };
            if (dlg.ShowDialog() != true) e.Cancel = true;
        }

        private void HypDeveloper_Click(object sender, RoutedEventArgs e)
        {
            try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://www.linkedin.com/in/muzzamil-nagda/") { UseShellExecute = true }); }
            catch { /* ignore — no browser to hand off to */ }
        }

        protected override void OnClosed(EventArgs e)
        {
            DatabaseService.Instance.CloseConnection();
            base.OnClosed(e);
        }
    }
}
