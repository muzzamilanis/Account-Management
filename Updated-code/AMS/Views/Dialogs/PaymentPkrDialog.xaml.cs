using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class PaymentPkrDialog : Window
    {
        public PaymentPkrDialog(bool isYen = false)
        {
            InitializeComponent();
            var vm = new PaymentPkrViewModel(isYen);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }

        // Edit mode — Yen payment
        public PaymentPkrDialog(Payment existing)
        {
            InitializeComponent();
            var vm = new PaymentPkrViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }

        // Edit mode — Party (PKR) payment
        public PaymentPkrDialog(PaymentPkr existing)
        {
            InitializeComponent();
            var vm = new PaymentPkrViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }

        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddCustomerDialog { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                var vm = (PaymentPkrViewModel)DataContext;
                var name = ((AddCustomerViewModel)dlg.DataContext).Customer.Name;
                vm.Customers.Add(name);
                vm.SelectedCustomer = name;
            }
        }
    }
}
