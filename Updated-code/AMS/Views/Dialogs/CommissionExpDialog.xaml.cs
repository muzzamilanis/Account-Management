using AMS.Models;
using AMS.ViewModels.Dialogs;
using System.Windows;

namespace AMS.Views.Dialogs
{
    public partial class CommissionExpDialog : Window
    {
        public CommissionExpDialog(CommissionExp existing = null)
        {
            InitializeComponent();
            var vm = new CommissionExpViewModel(existing);
            vm.CloseAction = r => { DialogResult = r; Close(); };
            DataContext = vm;
        }
    }
}
