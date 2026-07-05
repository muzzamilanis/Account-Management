using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class PurchaseAutoDialog : Window
    {
        public PurchaseAutoDialog(Stock existing = null)
        {
            InitializeComponent();
            var vm = new PurchaseAutoViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
