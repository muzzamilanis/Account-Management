using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class SaleAutoDialog : Window
    {
        public SaleAutoDialog(Sale existing = null)
        {
            InitializeComponent();
            var vm = new SaleAutoViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
