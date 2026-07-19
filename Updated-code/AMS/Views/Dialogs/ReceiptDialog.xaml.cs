using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class ReceiptDialog : Window
    {
        public ReceiptDialog(Receipt existing = null)
        {
            InitializeComponent();
            var vm = new ReceiptViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
