using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class AccountTransferDialog : Window
    {
        public AccountTransferDialog()
        {
            InitializeComponent();
            var vm = new AccountTransferViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
