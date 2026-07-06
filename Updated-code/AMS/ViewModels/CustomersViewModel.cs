using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using AMS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
        private Customer _selected;
        public Customer SelectedCustomer { get => _selected; set => SetField(ref _selected, value); }
        private string _filter;
        public string FilterText { get => _filter; set { SetField(ref _filter, value); Load(); } }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RefreshCommand { get; }

        public CustomersViewModel()
        {
            AddCommand = new RelayCommand(OpenAdd);
            EditCommand = new RelayCommand(() => OpenEdit(SelectedCustomer), () => SelectedCustomer != null);
            RefreshCommand = new RelayCommand(Load);
        }

        public void Load()
        {
            Customers.Clear();
            foreach (var c in DatabaseService.Instance.GetCustomers(FilterText ?? ""))
                Customers.Add(c);
        }

        private void OpenAdd() { var d = new AddCustomerDialog(null); if (d.ShowDialog() == true) Load(); }
        private void OpenEdit(Customer c) { if (c == null) return; var d = new AddCustomerDialog(c); if (d.ShowDialog() == true) Load(); }
    }
}
