using AMS.Helpers;
using AMS.Services;
using System;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls;

namespace AMS.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        public static string[] ReportTypes { get; } = {
            "Sold Cars", "Stocks", "Accounts", "Account Statement", "Accounts Receivable", "Accounts Payable",
            "Trial Balance", "Office Expenses", "Misc. Auto Expenses", "Clearance Expenses",
            "Demurrage Expenses", "No Plate Expenses", "Commission Expenses", "Tax Expenses",
            "Receipts", "Yen Payments", "Party Payments", "Agent Payments", "Active Credit Sales", "Profit Breakdown"
        };

        private string _selectedReport = "Sold Cars";
        public string SelectedReport { get => _selectedReport; set => SetField(ref _selectedReport, value); }

        private DateTime _dateFrom = new DateTime(2017, 1, 1);
        public DateTime DateFrom { get => _dateFrom; set => SetField(ref _dateFrom, value); }

        private DateTime _dateTo = DateTime.Today;
        public DateTime DateTo { get => _dateTo; set => SetField(ref _dateTo, value); }

        private FlowDocument _reportDoc;
        public FlowDocument ReportDocument { get => _reportDoc; set => SetField(ref _reportDoc, value); }

        private bool _hasReport;
        public bool HasReport { get => _hasReport; set => SetField(ref _hasReport, value); }

        public ICommand GenerateCommand { get; }
        public ICommand PrintCommand { get; }

        public ReportViewModel()
        {
            GenerateCommand = new RelayCommand(Generate);
            PrintCommand = new RelayCommand(Print, () => HasReport);
        }

        private void Generate()
        {
            try
            {
                var data = DatabaseService.Instance.GetReportData(SelectedReport, DateFrom, DateTo);
                string company = Services.SettingsService.Instance.CompanyName;
                ReportDocument = ReportService.Instance.BuildReport(SelectedReport, company, data);
                HasReport = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error generating report: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void Print()
        {
            if (ReportDocument == null) return;
            var pd = new System.Windows.Controls.PrintDialog();
            if (pd.ShowDialog() == true)
            {
                var fds = new FlowDocumentReader();
                fds.Document = ReportDocument;
                IDocumentPaginatorSource idps = ReportDocument;
                pd.PrintDocument(idps.DocumentPaginator, SelectedReport);
            }
        }
    }
}
