using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class AccountTransferDialog : Window
    {
        public AccountTransferDialog(OfficeAccount existing = null)
        {
            InitializeComponent();
            var vm = new AccountTransferViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
