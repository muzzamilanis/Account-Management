using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class AddCustomerDialog : Window
    {
        public AddCustomerDialog(Customer existing = null)
        {
            InitializeComponent();
            var vm = new AddCustomerViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
