using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class SaleAutoDialog : Window
    {
        public SaleAutoDialog()
        {
            InitializeComponent();
            var vm = new SaleAutoViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
