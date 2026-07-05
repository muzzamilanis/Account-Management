using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using AMS.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class SaleViewModel : ViewModelBase
    {
        public ObservableCollection<Sale> Sales { get; } = new ObservableCollection<Sale>();
        private Sale _selected;
        public Sale SelectedSale { get => _selected; set => SetField(ref _selected, value); }

        public ICommand NewSaleCommand { get; }
        public ICommand RefreshCommand { get; }

        public SaleViewModel()
        {
            NewSaleCommand = new RelayCommand(OpenNew);
            RefreshCommand = new RelayCommand(Load);
        }

        public void Load()
        {
            Sales.Clear();
            foreach (var s in DatabaseService.Instance.GetSales())
                Sales.Add(s);
        }

        private void OpenNew() { var d = new SaleAutoDialog(); if (d.ShowDialog() == true) Load(); }
    }
}
