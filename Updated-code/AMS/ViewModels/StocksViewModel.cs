using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using AMS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class StocksViewModel : ViewModelBase
    {
        public ObservableCollection<Stock> Stocks { get; } = new ObservableCollection<Stock>();
        private Stock _selectedStock;
        public Stock SelectedStock { get => _selectedStock; set => SetField(ref _selectedStock, value); }
        private string _filterText;
        public string FilterText { get => _filterText; set { SetField(ref _filterText, value); Load(); } }

        public ICommand AddStockCommand { get; }
        public ICommand EditStockCommand { get; }
        public ICommand RefreshCommand { get; }

        public StocksViewModel()
        {
            AddStockCommand = new RelayCommand(OpenAdd);
            EditStockCommand = new RelayCommand(() => OpenEdit(SelectedStock), () => SelectedStock != null);
            RefreshCommand = new RelayCommand(Load);
        }

        public void Load()
        {
            Stocks.Clear();
            foreach (var s in DatabaseService.Instance.GetStocks(FilterText ?? ""))
                Stocks.Add(s);
        }

        private void OpenAdd() { var d = new PurchaseAutoDialog(null); if (d.ShowDialog() == true) Load(); }
        private void OpenEdit(Stock s) { if (s == null) return; var d = new PurchaseAutoDialog(s); if (d.ShowDialog() == true) Load(); }
    }
}
