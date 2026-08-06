using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class CommissionExpDialog : Window
    {
        public CommissionExpDialog()
        {
            InitializeComponent();
            var vm = new CommissionExpViewModel();
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
