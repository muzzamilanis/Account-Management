using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class ReceiptDialog : Window
    {
        public ReceiptDialog()
        {
            InitializeComponent();
            var vm = new ReceiptViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
