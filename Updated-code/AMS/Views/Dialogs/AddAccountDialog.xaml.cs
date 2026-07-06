using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class AddAccountDialog : Window
    {
        public AddAccountDialog(Account existing = null)
        {
            InitializeComponent();
            var vm = new AddAccountViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
